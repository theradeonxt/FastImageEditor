
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

    __m128i maskA = _mm_set_epi32(0xFF000000, 0, 0, 0);
    __m128i maskR = _mm_set_epi32(0, 0x00FF0000, 0, 0);
    __m128i maskG = _mm_set_epi32(0, 0, 0x0000FF00, 0);
    __m128i maskB = _mm_set_epi32(0, 0, 0, 0x000000FF);

    __m128 zero = _mm_setzero_ps();
    __m128 _255 = _mm_set1_ps(255.0f);

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_ref_MT);
    //#pragma omp parallel for
    for (int32_t index = startIndex; index < endIndex; index += 4)
    {
        BEGIN_TIMED_BLOCK();

        __m128 sum = _mm_setzero_ps();

        for (int32_t k = 0; k < dimension; k++)
        {
            __m128 argblow = _mm_load_ss((const float*)(source + index + offsetLookup[k]));

            // argb | argb | argb | argb
            __m128i splat = _mm_shuffle_epi32(_mm_castps_si128(argblow), _MM_SHUFFLE(0, 0, 0, 0));

            __m128i _0r00 = _mm_srli_epi32(_mm_and_si128(splat, maskR), 16);
            __m128i _00g0 = _mm_srli_epi32(_mm_and_si128(splat, maskG), 8);
            __m128i _000b = _mm_and_si128(splat, maskB);

            __m128i _0rgbi = _mm_or_si128(_mm_or_si128(_0r00, _00g0), _000b);

            __m128 _0rgb = _mm_cvtepi32_ps(_0rgbi);

            float x = kernel[k];
            __m128 kkkk = _mm_set_ps(x, x, x, x);

            __m128 prod = _mm_mul_ps(_0rgb, kkkk);

            sum = _mm_add_ps(sum, prod);
        }

        sum = _mm_clamp_ps(sum, zero, _255);

        __m128i sumi = _mm_cvtps_epi32(sum);

        int br = _mm_extract_epi32(sumi, 0x0);
        int gr = _mm_extract_epi32(sumi, 0x1);
        int rr = _mm_extract_epi32(sumi, 0x2);
        int ar = *(int*)(source + index) & 0xFF000000;

        *(int*)(destination + index) = ar | rr << 16 | gr << 8 | br;

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}
