
#include "ImageProcessing_FMA3.h"
#include "ModuleSetupPerformer.h"
#include "HelpersSIMD.hpp"
#include "Helpers.hpp"
#include "ProfileTools.h"

#include <cstdint>

using namespace Constants;

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_MODULE(AlphaBlend32bgra_32bgra_FMA3,
    READONLY (uint8_t*) source,
    READONLY (uint8_t*) target,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_FMA3_A);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            // load 4 BGRA pixels (source and target)
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            __m128 sAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(src, 24));
            __m128 tAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(tar, 24));
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));

            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, xmmRcp255const);
            __m128 scaledTarf32 = _mm_sub_ps(xmm255const, sAAAAf32);
            __m128 dAAAAf32 = _mm_fmadd_ps(tmp1f32, scaledTarf32, sAAAAf32);
            __m128 invdAAAAf32 = _mm_rcp_ps(dAAAAf32);

            scaledTarf32 = _mm_mul_ps(scaledTarf32, tmp1f32);

            __m128 premsBBBBf32 = _mm_mul_ps(sAAAAf32, sBBBBf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tBBBBf32, premsBBBBf32), invdAAAAf32);
            __m128 premsGGGGf32 = _mm_mul_ps(sAAAAf32, sGGGGf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tGGGGf32, premsGGGGf32), invdAAAAf32);
            __m128 premsRRRRf32 = _mm_mul_ps(sAAAAf32, sRRRRf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tRRRRf32, premsRRRRf32), invdAAAAf32);

            __m128i dAAAA = _mm_slli_epi32(_mm_cvttps_epi32(dAAAAf32), 24);
            __m128i dBBBB = _mm_cvttps_epi32(dBBBBf32);
            __m128i dGGGG = _mm_slli_epi32(_mm_cvttps_epi32(dGGGGf32), 8);
            __m128i dRRRR = _mm_slli_epi32(_mm_cvttps_epi32(dRRRRf32), 16);

            __m128i dsthi = _mm_or_si128(dAAAA, dBBBB);
            __m128i dstlo = _mm_or_si128(dGGGG, dRRRR);
            __m128i dst = _mm_or_si128(dsthi, dstlo);

            // store 4 output pixels
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }
    else
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_FMA3_U);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128 sAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(src, 24));
            __m128 tAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(tar, 24));
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));
            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, xmmRcp255const);
            __m128 scaledTarf32 = _mm_sub_ps(xmm255const, sAAAAf32);
            __m128 dAAAAf32 = _mm_fmadd_ps(tmp1f32, scaledTarf32, sAAAAf32);
            __m128 invdAAAAf32 = _mm_rcp_ps(dAAAAf32);
            scaledTarf32 = _mm_mul_ps(scaledTarf32, tmp1f32);
            __m128 premsBBBBf32 = _mm_mul_ps(sAAAAf32, sBBBBf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tBBBBf32, premsBBBBf32), invdAAAAf32);
            __m128 premsGGGGf32 = _mm_mul_ps(sAAAAf32, sGGGGf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tGGGGf32, premsGGGGf32), invdAAAAf32);
            __m128 premsRRRRf32 = _mm_mul_ps(sAAAAf32, sRRRRf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tRRRRf32, premsRRRRf32), invdAAAAf32);
            __m128i dAAAA = _mm_slli_epi32(_mm_cvttps_epi32(dAAAAf32), 24);
            __m128i dBBBB = _mm_cvttps_epi32(dBBBBf32);
            __m128i dGGGG = _mm_slli_epi32(_mm_cvttps_epi32(dGGGGf32), 8);
            __m128i dRRRR = _mm_slli_epi32(_mm_cvttps_epi32(dRRRRf32), 16);
            __m128i dsthi = _mm_or_si128(dAAAA, dBBBB);
            __m128i dstlo = _mm_or_si128(dGGGG, dRRRR);
            __m128i dst = _mm_or_si128(dsthi, dstlo);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(AlphaBlend32bgra_32bgra_FMA3_MT,
    READONLY (uint8_t*) source,
    READONLY (uint8_t*) target,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_FMA3_MT_A);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            // load 4 BGRA pixels (source and target)
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            __m128 sAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(src, 24));
            __m128 tAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(tar, 24));
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));

            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, xmmRcp255const);
            __m128 scaledTarf32 = _mm_sub_ps(xmm255const, sAAAAf32);
            __m128 dAAAAf32 = _mm_fmadd_ps(tmp1f32, scaledTarf32, sAAAAf32);
            __m128 invdAAAAf32 = _mm_rcp_ps(dAAAAf32);

            scaledTarf32 = _mm_mul_ps(scaledTarf32, tmp1f32);

            __m128 premsBBBBf32 = _mm_mul_ps(sAAAAf32, sBBBBf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tBBBBf32, premsBBBBf32), invdAAAAf32);
            __m128 premsGGGGf32 = _mm_mul_ps(sAAAAf32, sGGGGf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tGGGGf32, premsGGGGf32), invdAAAAf32);
            __m128 premsRRRRf32 = _mm_mul_ps(sAAAAf32, sRRRRf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tRRRRf32, premsRRRRf32), invdAAAAf32);

            __m128i dAAAA = _mm_slli_epi32(_mm_cvttps_epi32(dAAAAf32), 24);
            __m128i dBBBB = _mm_cvttps_epi32(dBBBBf32);
            __m128i dGGGG = _mm_slli_epi32(_mm_cvttps_epi32(dGGGGf32), 8);
            __m128i dRRRR = _mm_slli_epi32(_mm_cvttps_epi32(dRRRRf32), 16);

            __m128i dsthi = _mm_or_si128(dAAAA, dBBBB);
            __m128i dstlo = _mm_or_si128(dGGGG, dRRRR);
            __m128i dst = _mm_or_si128(dsthi, dstlo);

            // store 4 output pixels
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }
    else
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_FMA3_MT_U);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128 sAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(src, 24));
            __m128 tAAAAf32 = _mm_cvtepi32_ps(_mm_srli_epi32(tar, 24));
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));
            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, xmmRcp255const);
            __m128 scaledTarf32 = _mm_sub_ps(xmm255const, sAAAAf32);
            __m128 dAAAAf32 = _mm_fmadd_ps(tmp1f32, scaledTarf32, sAAAAf32);
            __m128 invdAAAAf32 = _mm_rcp_ps(dAAAAf32);
            scaledTarf32 = _mm_mul_ps(scaledTarf32, tmp1f32);
            __m128 premsBBBBf32 = _mm_mul_ps(sAAAAf32, sBBBBf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tBBBBf32, premsBBBBf32), invdAAAAf32);
            __m128 premsGGGGf32 = _mm_mul_ps(sAAAAf32, sGGGGf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tGGGGf32, premsGGGGf32), invdAAAAf32);
            __m128 premsRRRRf32 = _mm_mul_ps(sAAAAf32, sRRRRf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_fmadd_ps(scaledTarf32, tRRRRf32, premsRRRRf32), invdAAAAf32);
            __m128i dAAAA = _mm_slli_epi32(_mm_cvttps_epi32(dAAAAf32), 24);
            __m128i dBBBB = _mm_cvttps_epi32(dBBBBf32);
            __m128i dGGGG = _mm_slli_epi32(_mm_cvttps_epi32(dGGGGf32), 8);
            __m128i dRRRR = _mm_slli_epi32(_mm_cvttps_epi32(dRRRRf32), 16);
            __m128i dsthi = _mm_or_si128(dAAAA, dBBBB);
            __m128i dstlo = _mm_or_si128(dGGGG, dRRRR);
            __m128i dst = _mm_or_si128(dsthi, dstlo);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

// broadcastss => 46 ms, only for 255-alpha images

IMPROC_MODULE(ConvFilter_32bgra_FMA3,
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

    uint32_t size = width * height;
    __m128* xmmKernel = (__m128*)_mm_malloc(sizeof(__m128)*size, 16);
    for (uint32_t i = 0; i < size; i++)
    {
        float k = kernel[i];
        xmmKernel[i] = _mm_set_ps(k, k, k, k);
    }

    int32_t dimension = width * height;
    int32_t nSizeBytes = int32_t(sizeBytes);
    int32_t startIndex = (height / 2) * strideBytes + (width / 2) * 4;
    // TODO: The first 4 should not be here, but otherwise results in an access violation
    int32_t endIndex = nSizeBytes - 4 * ((height / 2) * strideBytes + (width / 2) * 4);

    __m128i xmmMaskR = _mm_set_epi32(0, 0x00FF0000, 0, 0);
    __m128i xmmMaskG = _mm_set_epi32(0, 0, 0x0000FF00, 0);
    __m128i xmmMaskB = _mm_set_epi32(0, 0, 0, 0x000000FF);
    __m128i xmmFullAlpha = _mm_set_epi32(0, 0, 0, 0xFF << 24);

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_FMA3);

//#pragma omp parallel for num_threads(4)
    for (int32_t index = startIndex; index < endIndex; index += 4)
    {
        BEGIN_TIMED_BLOCK();

        __m128 xmmSumf32 = _mm_setzero_ps();

        for (int32_t k = 0; k < dimension; k++)
        {
            // load input pixel into all 32bits of XMM register
            __m128i xmmArgb = _mm_castps_si128(
                _mm_broadcast_ss(reinterpret_cast<const float*>(source + index + offsetLookup[k])));

            // create 0|r|g|b (32bit ints)
            __m128i xmm0R00 = _mm_srli_epi32(_mm_and_si128(xmmArgb, xmmMaskR), 16);
            __m128i xmm00G0 = _mm_srli_epi32(_mm_and_si128(xmmArgb, xmmMaskG), 8);
            __m128i xmm000B = _mm_and_si128(xmmArgb, xmmMaskB);
            __m128i xmm0RGBi = _mm_or_si128(_mm_or_si128(xmm0R00, xmm00G0), xmm000B);

            // create 0|r|g|b (float)
            __m128 xmm0RGBf32 = _mm_cvtepi32_ps(xmm0RGBi);

            // load kernel value into the 4 floats of XMM register
            __m128 xmmFilter = _mm_load_ps(reinterpret_cast<float*>(xmmKernel + k));

            // multiply accumulate pixel values with kernel
            xmmSumf32 = _mm_fmadd_ps(xmm0RGBf32, xmmFilter, xmmSumf32);
        }

        // limit result to [0.0-255.0]
        xmmSumf32 = _mm_clamp_ps(xmmSumf32, xmm0const, xmm255const);

        // convert to integer values
        __m128i xmmSumi = _mm_cvtps_epi32(xmmSumf32);

        // obtain x|x|x|ARGB output pixel
        __m128i xmm00RR = _mm_slli_epi32(_mm_unpackhi_epi32(xmmSumi, xmmSumi), 16);
        __m128i xmm00RG = _mm_slli_epi32(_mm_srli_epi64(xmmSumi, 32), 8);
        __m128i xmmResult = _mm_or_si128(_mm_or_si128(xmmSumi, xmm00RG), xmm00RR);
        xmmResult = _mm_or_si128(xmmResult, xmmFullAlpha);

        // store output pixel
        _mm_store_ss(reinterpret_cast<float*>(destination + index), _mm_castsi128_ps(xmmResult));

        END_TIMED_BLOCK();
    }
    
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

// unroll 2x inner loop => 33 ms (!missing last product!)

/*IMPROC_MODULE(ConvFilter_32bgra_FMA3,
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

    uint32_t size = width * height;
    __m128* xmmKernel = (__m128*)_mm_malloc(sizeof(__m128)*size, 16);
    for (uint32_t i = 0; i < size; i++)
    {
        float k = kernel[i];
        xmmKernel[i] = _mm_set_ps(k, k, k, k);
    }

    int32_t dimension = width * height;
    int32_t nSizeBytes = int32_t(sizeBytes);
    int32_t startIndex = (height / 2) * strideBytes + (width / 2) * 4;
    // TODO: The first 4 should not be here, but otherwise results in an access violation
    int32_t endIndex = nSizeBytes - 4 * ((height / 2) * strideBytes + (width / 2) * 4);

    __m128i xmmMaskR = _mm_set_epi32(0, 0x00FF0000, 0, 0);
    __m128i xmmMaskG = _mm_set_epi32(0, 0, 0x0000FF00, 0);
    __m128i xmmMaskB = _mm_set_epi32(0, 0, 0, 0x000000FF);
    __m128i xmmFullAlpha = _mm_set_epi32(0, 0, 0, 0xFF << 24);

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_FMA3);

    //#pragma omp parallel for num_threads(4)
    for (int32_t index = startIndex; index < endIndex; index += 4)
    {
        BEGIN_TIMED_BLOCK();

        __m128 xmmSumf32 = _mm_setzero_ps();
        __m128 xmmSumf32_2 = _mm_setzero_ps();

        for (int32_t k = 0; k < dimension-1; k+=2)
        {
            // load input pixel into all 32bits of XMM register
            __m128i xmmArgb = _mm_castps_si128(
                _mm_broadcast_ss(reinterpret_cast<const float*>(source + index + offsetLookup[k])));

            // create 0|r|g|b (32bit ints)
            __m128i xmm0R00 = _mm_srli_epi32(_mm_and_si128(xmmArgb, xmmMaskR), 16);
            __m128i xmm00G0 = _mm_srli_epi32(_mm_and_si128(xmmArgb, xmmMaskG), 8);
            __m128i xmm000B = _mm_and_si128(xmmArgb, xmmMaskB);
            __m128i xmm0RGBi = _mm_or_si128(_mm_or_si128(xmm0R00, xmm00G0), xmm000B);

            // create 0|r|g|b (float)
            __m128 xmm0RGBf32 = _mm_cvtepi32_ps(xmm0RGBi);

            // load kernel value into the 4 floats of XMM register
            __m128 xmmFilter = _mm_load_ps(reinterpret_cast<float*>(xmmKernel + k));

            // multiply accumulate pixel values with kernel
            xmmSumf32 = _mm_fmadd_ps(xmm0RGBf32, xmmFilter, xmmSumf32);

            __m128i xmmArgb_2 = _mm_castps_si128(
                _mm_broadcast_ss(reinterpret_cast<const float*>(source + index + offsetLookup[k + 1])));
            __m128i xmm0R00_2 = _mm_srli_epi32(_mm_and_si128(xmmArgb_2, xmmMaskR), 16);
            __m128i xmm00G0_2 = _mm_srli_epi32(_mm_and_si128(xmmArgb_2, xmmMaskG), 8);
            __m128i xmm000B_2 = _mm_and_si128(xmmArgb_2, xmmMaskB);
            __m128i xmm0RGBi_2 = _mm_or_si128(_mm_or_si128(xmm0R00_2, xmm00G0_2), xmm000B_2);
            __m128 xmm0RGBf32_2 = _mm_cvtepi32_ps(xmm0RGBi_2);
            __m128 xmmFilter_2 = _mm_load_ps(reinterpret_cast<float*>(xmmKernel + k + 1));
            xmmSumf32_2 = _mm_fmadd_ps(xmm0RGBf32_2, xmmFilter_2, xmmSumf32_2);
        }

        xmmSumf32 = _mm_add_ps(xmmSumf32, xmmSumf32_2);

        // limit result to [0.0-255.0]
        xmmSumf32 = _mm_clamp_ps(xmmSumf32, xmm0const, xmm255const);

        // convert to integer values
        __m128i xmmSumi = _mm_cvtps_epi32(xmmSumf32);

        // obtain x|x|x|ARGB output pixel
        __m128i xmm00RR = _mm_slli_epi32(_mm_unpackhi_epi32(xmmSumi, xmmSumi), 16);
        __m128i xmm00RG = _mm_slli_epi32(_mm_srli_epi64(xmmSumi, 32), 8);
        __m128i xmmResult = _mm_or_si128(_mm_or_si128(xmmSumi, xmm00RG), xmm00RR);
        xmmResult = _mm_or_si128(xmmResult, xmmFullAlpha);

        // store output pixel
        _mm_stream_ss(reinterpret_cast<float*>(destination + index), _mm_castsi128_ps(xmmResult));

        END_TIMED_BLOCK();
    }

    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}*/
