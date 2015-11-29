
#include "ImageProcessing_FMA3.h"
#include "HelpersSIMD.hpp"
#include "ProfileTools.h"

#include <cstdint>

// For MSVC compiler disable truncation of constant value warning
#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable: 4309) 
#endif


// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3(READONLY (uint8_t*) source,
                             READONLY (uint8_t*) target,
                             READWRITE(uint8_t*) destination,
                             uint32_t            sizeBytes)
{
    __m128i mask000000FF = _mm_set1_epi32(0x000000FF);
    __m128i mask0000FF00 = _mm_set1_epi32(0x0000FF00);
    __m128i mask00FF0000 = _mm_set1_epi32(0x00FF0000);
    __m128 one255f32 = _mm_set1_ps(255.0f);
    __m128 onef32 = _mm_set1_ps(1.0f);
    __m128 rcp255f32 = _mm_div_ps(onef32, one255f32);

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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, mask000000FF));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, mask000000FF));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask0000FF00), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask0000FF00), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask00FF0000), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask00FF0000), 16));

            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, rcp255f32);
            __m128 scaledTarf32 = _mm_sub_ps(one255f32, sAAAAf32);
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
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, mask000000FF));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, mask000000FF));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask0000FF00), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask0000FF00), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask00FF0000), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask00FF0000), 16));
            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, rcp255f32);
            __m128 scaledTarf32 = _mm_sub_ps(one255f32, sAAAAf32);
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
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3_MT(READONLY (uint8_t*) source,
                                READONLY (uint8_t*) target,
                                READWRITE(uint8_t*) destination,
                                uint32_t            sizeBytes)
{
    __m128i mask000000FF = _mm_set1_epi32(0x000000FF);
    __m128i mask0000FF00 = _mm_set1_epi32(0x0000FF00);
    __m128i mask00FF0000 = _mm_set1_epi32(0x00FF0000);
    __m128 one255f32 = _mm_set1_ps(255.0f);
    __m128 onef32 = _mm_set1_ps(1.0f);
    __m128 rcp255f32 = _mm_div_ps(onef32, one255f32);

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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, mask000000FF));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, mask000000FF));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask0000FF00), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask0000FF00), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask00FF0000), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask00FF0000), 16));

            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, rcp255f32);
            __m128 scaledTarf32 = _mm_sub_ps(one255f32, sAAAAf32);
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
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, mask000000FF));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, mask000000FF));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask0000FF00), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask0000FF00), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, mask00FF0000), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, mask00FF0000), 16));
            __m128 tmp1f32 = _mm_mul_ps(tAAAAf32, rcp255f32);
            __m128 scaledTarf32 = _mm_sub_ps(one255f32, sAAAAf32);
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
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

#ifdef _MSC_VER
#pragma warning(pop)
#endif
