
#include "ImageProcessing_SSE2.h"
#include "HelpersSIMD.hpp"
#include "ProfileTools.h"

#include <cstdint>

// For MSVC compiler disable truncation of constant value warning
#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable: 4309) 
#endif

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_SSE2(READONLY (uint8_t*) source,
                      READONLY (uint8_t*) target,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      float               percentage)
{
    // prescaled alpha coefficients mapped from [0.0-1.0] to [0-255]
    __m128i alpha16 = _mm_set1_epi16(int16_t(percentage * 255.0f));
    __m128i _1alpha16 = _mm_set1_epi16(int16_t((1.0f - percentage) * 255.0f));

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_SSE2_A);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            // unpack [BGRBGR...] into [B0G0R0B0G0R0...] (extend each color channel to 16-bit)
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            // compute 16-bit alpha * target, (1-alpha) * source
            __m128i taralphalo = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi = _mm_mullo_epi16(tarhi, alpha16);
            __m128i _1srcalphalo = _mm_mullo_epi16(srclo, _1alpha16);
            __m128i _1srcalphahi = _mm_mullo_epi16(srchi, _1alpha16);

            // sum the two 16-bit products
            __m128i dstlo = _mm_add_epi16(taralphalo, _1srcalphalo);
            __m128i dsthi = _mm_add_epi16(taralphahi, _1srcalphahi);

            // scale down 16-bit results (/255)
            dstlo = _mm_div255_epu16(dstlo);
            dsthi = _mm_div255_epu16(dsthi);

            // clamp the results to [0-255]
            __m128i dst = _mm_packus_epi16(dstlo, dsthi);

            // bypass the cache, non-temporal write to memory (this saves a few cycles)
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_SSE2_U);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i taralphalo = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi = _mm_mullo_epi16(tarhi, alpha16);
            __m128i _1srcalphalo = _mm_mullo_epi16(srclo, _1alpha16);
            __m128i _1srcalphahi = _mm_mullo_epi16(srchi, _1alpha16);
            __m128i dstlo = _mm_add_epi16(taralphalo, _1srcalphalo);
            __m128i dsthi = _mm_add_epi16(taralphahi, _1srcalphahi);
            __m128i dst = _mm_packus_epi16(_mm_div255_epu16(dstlo), _mm_div255_epu16(dsthi));
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_SSE2_MT(READONLY (uint8_t*) source,
                         READONLY (uint8_t*) target,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage)
{
    // prescaled alpha coefficients mapped from [0.0-1.0] to [0-255]
    __m128i alpha16 = _mm_set1_epi16(int16_t(percentage * 255.0f));
    __m128i _1alpha16 = _mm_set1_epi16(int16_t((1.0f - percentage) * 255.0f));

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_SSE2_MT_A);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            // unpack [BGRBGR...] into [B0G0R0B0G0R0...] (extend each color channel to 16-bit)
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            // compute 16-bit alpha * target, (1-alpha) * source
            __m128i taralphalo = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi = _mm_mullo_epi16(tarhi, alpha16);
            __m128i _1srcalphalo = _mm_mullo_epi16(srclo, _1alpha16);
            __m128i _1srcalphahi = _mm_mullo_epi16(srchi, _1alpha16);

            // sum the two 16-bit products
            __m128i dstlo = _mm_add_epi16(taralphalo, _1srcalphalo);
            __m128i dsthi = _mm_add_epi16(taralphahi, _1srcalphahi);

            // scale down 16-bit results (/255)
            dstlo = _mm_div255_epu16(dstlo);
            dsthi = _mm_div255_epu16(dsthi);

            // clamp the results to [0-255]
            __m128i dst = _mm_packus_epi16(dstlo, dsthi);

            // bypass the cache, non-temporal write to memory (this saves a few cycles)
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_SSE2_MT_U);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i taralphalo = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi = _mm_mullo_epi16(tarhi, alpha16);
            __m128i _1srcalphalo = _mm_mullo_epi16(srclo, _1alpha16);
            __m128i _1srcalphahi = _mm_mullo_epi16(srchi, _1alpha16);
            __m128i dstlo = _mm_add_epi16(taralphalo, _1srcalphalo);
            __m128i dsthi = _mm_add_epi16(taralphahi, _1srcalphahi);
            __m128i dst = _mm_packus_epi16(_mm_div255_epu16(dstlo), _mm_div255_epu16(dsthi));
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_SSE2(READONLY (uint8_t*) source,
                          READWRITE(uint8_t*) destination,
                          uint32_t            sizeBytes,
                          float               percentage)
{
    uint8_t a = uint8_t(percentage * 255.0f);
    __m128i alphaLevel = _mm_set_epi8(a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0);
    __m128i alphaMask = _mm_set_epi8(0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, destination))
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_A);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_U);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_SSE2_MT(READONLY (uint8_t*) source,
                             READWRITE(uint8_t*) destination,
                             uint32_t            sizeBytes,
                             float               percentage)
{
    uint8_t a = uint8_t(percentage * 255.0f);
    __m128i alphaLevel = _mm_set_epi8(a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0);
    __m128i alphaMask = _mm_set_epi8(0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, destination))
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_MT_A);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_MT_U);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_SSE2(READONLY (uint8_t*) source,
                             READONLY (uint8_t*) target,
                             READWRITE(uint8_t*) destination,
                             uint32_t            sizeBytes)
{
    __m128i mask000000FF = _mm_set1_epi32(0x000000FF);
    __m128i mask0000FF00 = _mm_set1_epi32(0x0000FF00);
    __m128i mask00FF0000 = _mm_set1_epi32(0x00FF0000);
    __m128 one255f32 = _mm_set1_ps(255.0f);
    __m128 rcp255f32 = _mm_rcp_ps(one255f32);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSE2_A);
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

            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, rcp255f32), _mm_sub_ps(one255f32, sAAAAf32));
            __m128 dAAAAf32 = _mm_add_ps(sAAAAf32, tmpf32);
            __m128 rcpAAAAf32 = _mm_rcp_ps(dAAAAf32);

            __m128 dBBBBf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sBBBBf32),
                _mm_mul_ps(tmpf32, tBBBBf32)), rcpAAAAf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sGGGGf32),
                _mm_mul_ps(tmpf32, tGGGGf32)), rcpAAAAf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sRRRRf32),
                _mm_mul_ps(tmpf32, tRRRRf32)), rcpAAAAf32);

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
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSE2_U);
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
            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, rcp255f32), _mm_sub_ps(one255f32, sAAAAf32));
            __m128 dAAAAf32 = _mm_add_ps(sAAAAf32, tmpf32);
            __m128 rcpAAAAf32 = _mm_rcp_ps(dAAAAf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sBBBBf32),
                _mm_mul_ps(tmpf32, tBBBBf32)), rcpAAAAf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sGGGGf32),
                _mm_mul_ps(tmpf32, tGGGGf32)), rcpAAAAf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sRRRRf32),
                _mm_mul_ps(tmpf32, tRRRRf32)), rcpAAAAf32);
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
AlphaBlend32bgra_32bgra_SSE2_MT(READONLY (uint8_t*) source,
                                READONLY (uint8_t*) target,
                                READWRITE(uint8_t*) destination,
                                uint32_t            sizeBytes)
{
    __m128i mask000000FF = _mm_set1_epi32(0x000000FF);
    __m128i mask0000FF00 = _mm_set1_epi32(0x0000FF00);
    __m128i mask00FF0000 = _mm_set1_epi32(0x00FF0000);
    __m128 one255f32 = _mm_set1_ps(255.0f);
    __m128 rcp255f32 = _mm_rcp_ps(one255f32);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSE2_MT_A);
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

            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, rcp255f32), _mm_sub_ps(one255f32, sAAAAf32));
            __m128 dAAAAf32 = _mm_add_ps(sAAAAf32, tmpf32);
            __m128 rcpAAAAf32 = _mm_rcp_ps(dAAAAf32);

            __m128 dBBBBf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sBBBBf32),
                _mm_mul_ps(tmpf32, tBBBBf32)), rcpAAAAf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sGGGGf32),
                _mm_mul_ps(tmpf32, tGGGGf32)), rcpAAAAf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sRRRRf32),
                _mm_mul_ps(tmpf32, tRRRRf32)), rcpAAAAf32);

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
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSE2_MT_U);
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
            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, rcp255f32), _mm_sub_ps(one255f32, sAAAAf32));
            __m128 dAAAAf32 = _mm_add_ps(sAAAAf32, tmpf32);
            __m128 rcpAAAAf32 = _mm_rcp_ps(dAAAAf32);
            __m128 dBBBBf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sBBBBf32),
                _mm_mul_ps(tmpf32, tBBBBf32)), rcpAAAAf32);
            __m128 dGGGGf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sGGGGf32),
                _mm_mul_ps(tmpf32, tGGGGf32)), rcpAAAAf32);
            __m128 dRRRRf32 = _mm_mul_ps(_mm_add_ps(_mm_mul_ps(sAAAAf32, sRRRRf32),
                _mm_mul_ps(tmpf32, tRRRRf32)), rcpAAAAf32);
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
