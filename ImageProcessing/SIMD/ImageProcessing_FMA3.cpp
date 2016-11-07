
#include "ImageProcessing_FMA3.h"
#include "ModuleSetupPerformer.h"
#include "HelpersSIMD.hpp"
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
