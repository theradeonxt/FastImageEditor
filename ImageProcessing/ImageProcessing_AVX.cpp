
#include "ImageProcessing_AVX.h"
#include "HelpersSIMD.hpp"
#include "ProfileTools.h"

#include <cstdint>

// For MSVC compiler disable truncation of constant value warning
#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable: 4309) 
#endif

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_AVX(READONLY(uint8_t*)  source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes)
{
    __m256 mask000000FF = _mm256_castsi256_ps( _mm256_set1_epi32(0x000000FF));
    __m256 mask0000FF00 = _mm256_castsi256_ps(_mm256_set1_epi32(0x0000FF00));
    __m256 mask00FF0000 = _mm256_castsi256_ps(_mm256_set1_epi32(0x00FF0000));
    __m256 one255f32 = _mm256_set1_ps(255.0f);
    __m256 rcp255f32 = _mm256_rcp_ps(one255f32);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAlignedAVX(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_AVX_A);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE_AVX; index += SIMD_SIZE_AVX)
        {
            BEGIN_TIMED_BLOCK();

            // load 4 BGRA pixels (source and target)
            __m256i src = _mm256_lddqu_si256(reinterpret_cast<const __m256i*>(source + index));
            __m256i tar = _mm256_lddqu_si256(reinterpret_cast<const __m256i*>(target + index));

            __m256 sAAAAf32 = _mm256_cvtepi32_ps(srli(src, 24));
            __m256 tAAAAf32 = _mm256_cvtepi32_ps(srli(tar, 24));
            __m256 sBBBBf32 = _mm256_cvtepi32_ps(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(src), mask000000FF)));
            __m256 tBBBBf32 = _mm256_cvtepi32_ps(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(tar), mask000000FF)));
            __m256 sGGGGf32 = _mm256_cvtepi32_ps(srli(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(src), mask0000FF00)), 8));
            __m256 tGGGGf32 = _mm256_cvtepi32_ps(srli(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(tar), mask0000FF00)), 8));
            __m256 sRRRRf32 = _mm256_cvtepi32_ps(srli(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(src), mask00FF0000)), 16));
            __m256 tRRRRf32 = _mm256_cvtepi32_ps(srli(_mm256_castps_si256(_mm256_and_ps(_mm256_castsi256_ps(tar), mask00FF0000)), 16));

            __m256 tmpf32 = _mm256_mul_ps(_mm256_mul_ps(tAAAAf32, rcp255f32), _mm256_sub_ps(one255f32, sAAAAf32));
            __m256 dAAAAf32 = _mm256_add_ps(sAAAAf32, tmpf32);
            __m256 rcpAAAAf32 = _mm256_rcp_ps(dAAAAf32);

            __m256 dBBBBf32 = _mm256_mul_ps(_mm256_add_ps(_mm256_mul_ps(sAAAAf32, sBBBBf32),
                _mm256_mul_ps(tmpf32, tBBBBf32)), rcpAAAAf32);
            __m256 dGGGGf32 = _mm256_mul_ps(_mm256_add_ps(_mm256_mul_ps(sAAAAf32, sGGGGf32),
                _mm256_mul_ps(tmpf32, tGGGGf32)), rcpAAAAf32);
            __m256 dRRRRf32 = _mm256_mul_ps(_mm256_add_ps(_mm256_mul_ps(sAAAAf32, sRRRRf32),
                _mm256_mul_ps(tmpf32, tRRRRf32)), rcpAAAAf32);

            __m256i dAAAA = slli(_mm256_cvttps_epi32(dAAAAf32), 24);
            __m256i dBBBB = _mm256_cvttps_epi32(dBBBBf32);
            __m256i dGGGG = slli(_mm256_cvttps_epi32(dGGGGf32), 8);
            __m256i dRRRR = slli(_mm256_cvttps_epi32(dRRRRf32), 16);

            __m256 dsthi = _mm256_or_ps(_mm256_castsi256_ps(dAAAA), _mm256_castsi256_ps(dBBBB));
            __m256 dstlo = _mm256_or_ps(_mm256_castsi256_ps(dGGGG), _mm256_castsi256_ps(dRRRR));
            __m256 dst = _mm256_or_ps(dsthi, dstlo);

            // store 4 output pixels
            _mm256_stream_ps(reinterpret_cast<float*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/8Px: ");
    }

    return OperationSuccess;
}

#ifdef _MSC_VER
#pragma warning(pop)
#endif
