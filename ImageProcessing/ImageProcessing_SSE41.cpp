
#include "ImageProcessing_SSE41.h"
#include "ModuleSetupPerformer.h"
#include "Helpers.hpp"
#include "HelpersSIMD.hpp"
#include "ProfileTools.h"

#include <cstdint>

using namespace Constants;

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_MODULE(ConvFilter_32bgra_SSE41,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes,
    uint32_t            strideBytes,
    READONLY(float*)    kernel,
    uint32_t            width,
    uint32_t            height)
{
    // precalculate jumps from a pixel position to neighbouring pixels 
    READONLY(int32_t*) offsetLookup = PixelOffsetsLookup(width, height, 4, strideBytes);
    if (offsetLookup == nullptr)
    {
        return OutOfMemory;
    }

    int32_t dimension = width * height;
    int32_t nSizeBytes = int32_t(sizeBytes);
    int32_t startIndex = (height / 2) * strideBytes + (width / 2) * 4;
    // TODO: The first 4 should not be here, but otherwise results in an access violation
    int32_t endIndex = nSizeBytes - 4 * ((height / 2) * strideBytes + (width / 2) * 4);

    __m128i xmmMaskR = _mm_set_epi32(0, 0x00FF0000, 0, 0);
    __m128i xmmMaskG = _mm_set_epi32(0, 0, 0x0000FF00, 0);
    __m128i xmmMaskB = _mm_set_epi32(0, 0, 0, 0x000000FF);

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_SSE41);

//#pragma omp parallel for num_threads(4)
    for (int32_t index = startIndex; index < endIndex; index += 4)
    {
        BEGIN_TIMED_BLOCK();

        __m128 xmmSumf32 = _mm_setzero_ps();

        for (int32_t k = 0; k < dimension; k++)
        {
            // load input pixel into low 32bits of XMM register
            __m128 xmmArgbLow = _mm_load_ss(reinterpret_cast<const float*>(source + index + offsetLookup[k]));

            // shuffle into: argb|argb|argb|argb
            __m128i xmlArgb = _mm_shuffle_epi32(_mm_castps_si128(xmmArgbLow), _MM_SHUFFLE(0, 0, 0, 0));

            // create 0|r|g|b (32bit ints)
            __m128i xmm0R00 = _mm_srli_epi32(_mm_and_si128(xmlArgb, xmmMaskR), 16);
            __m128i xmm00G0 = _mm_srli_epi32(_mm_and_si128(xmlArgb, xmmMaskG), 8);
            __m128i xmm000B = _mm_and_si128(xmlArgb, xmmMaskB);
            __m128i xmm0RGBi = _mm_or_si128(_mm_or_si128(xmm0R00, xmm00G0), xmm000B);

            // create 0|r|g|b (float)
            __m128 xmm0RGBf32 = _mm_cvtepi32_ps(xmm0RGBi);

            // load kernel value into the 4 floats of XMM register
            float f = kernel[k];
            __m128 xmmFilter = _mm_set_ps(f, f, f, f);

            // multiply pixel values with kernel
            __m128 xmmDot = _mm_mul_ps(xmm0RGBf32, xmmFilter);

            // accumulate products
            xmmSumf32 = _mm_add_ps(xmmSumf32, xmmDot);
        }

        // limit result to [0.0-255.0]
        xmmSumf32 = _mm_clamp_ps(xmmSumf32, xmm0const, xmm255const);

        // convert to integer values
        __m128i xmmSumi = _mm_cvtps_epi32(xmmSumf32);

        // extract individual b g r results, alpha is same as source
        uint32_t br = _mm_extract_epi32(xmmSumi, 0x0);
        uint32_t gr = _mm_extract_epi32(xmmSumi, 0x1);
        uint32_t rr = _mm_extract_epi32(xmmSumi, 0x2);
        uint32_t ar = *reinterpret_cast<const uint32_t*>(source + index) & 0xFF000000;

        // store output pixel
        *reinterpret_cast<uint32_t*>(destination + index) = ar | rr << 16 | gr << 8 | br;

        END_TIMED_BLOCK();
    }
    
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}
