
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

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Convert_32bgra_24hsv_SSE2(READONLY(uint8_t*)  source,
                          READWRITE(uint8_t*) destinationHueChannel,
                          READWRITE(uint8_t*) destinationSaturationChannel,
                          READWRITE(uint8_t*) destinationValueChannel,
                          uint32_t            sizeBytes)
{
    __m128i xmmMaskB = _mm_set1_epi32(0x000000ff);
    __m128i xmmMaskG = _mm_set1_epi32(0x0000ff00);
    __m128i xmmMaskR = _mm_set1_epi32(0x00ff0000);
    __m128 xmmZeroconst = _mm_set1_ps(0.0f);
    __m128 xmm255const = _mm_set1_ps(255.0f);
    __m128 xmm43const = _mm_set1_ps(43.0f);
    __m128 xmm171const = _mm_set1_ps(171.0f);
    __m128 xmm85const = _mm_set1_ps(85.0f);

    int32_t nSizeBytes = int32_t(sizeBytes);

    int ih = 0, is = 0, iv = 0;
    for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE, ih += 4, is += 4, iv += 4)
    {
        // read from input data, format: BGRA BGRA BGRA BGRA
        __m128i xmmBGRA = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));

        // extract the individual channels and expand them to floating point
        // 000B 000B 000B 000B (FP32)
        __m128 xmmBBBB = _mm_cvtepi32_ps(_mm_and_si128(xmmBGRA, xmmMaskB));
        __m128 xmmGGGG = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmMaskG), 8));
        __m128 xmmRRRR = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmMaskR), 16));

        // 000m 000m 000m 000m (FP32) (m = min(r, g, b))
        // 000M 000M 000M 000M (FP32) (M = max(r, g, b))
        __m128 xmmRgbMin = _mm_min_ps(xmmBBBB, _mm_min_ps(xmmGGGG, xmmRRRR));
        __m128 xmmRgbMax = _mm_max_ps(xmmBBBB, _mm_max_ps(xmmGGGG, xmmRRRR));

        // value component
        __m128 xmmVVVV = xmmRgbMax;

        // difference rgbMax - rgbMin
        __m128 xmmDiff = _mm_sub_ps(xmmRgbMax, xmmRgbMin);
        // precompute 1.0f / difference to save divisions later
        __m128 xmmInvDiff = _mm_rcp_ps(xmmDiff);

        // saturation component
        __m128 xmmSSSS = _mm_mul_ps(xmmDiff, xmm255const);
        xmmSSSS = _mm_mul_ps(xmmSSSS, xmmInvDiff);

        __m128 xmmCmpVZero = _mm_cmpeq_ps(xmmVVVV, xmmZeroconst);  // check if v == 0
        __m128 xmmCmpSZero = _mm_cmpeq_ps(xmmSSSS, xmmZeroconst);  // check if s == 0
        __m128 xmmCmpMaxR = _mm_cmpeq_ps(xmmRgbMax, xmmRRRR);      // check if rgbMax == r (CASE 1)
        __m128 xmmCmpMaxG = _mm_cmpeq_ps(xmmRgbMax, xmmGGGG);      // check if rgbMax == g (CASE 2)
        __m128 xmmElse = _mm_or_ps(xmmCmpMaxR, xmmCmpMaxG);        // check if (rgbMax != r && rgbMax != g) (CASE 3)

        // calculate hue component in CASE 1
        __m128 xmmHTemp1 = _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmGGGG, xmmBBBB)), xmmInvDiff);
        xmmHTemp1 = _mm_and_ps(xmmHTemp1, xmmCmpMaxR);
        // calculate hue component in CASE 2
        __m128 xmmHTemp2 = _mm_add_ps(xmm85const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmBBBB, xmmRRRR)), xmmInvDiff));
        xmmHTemp2 = _mm_and_ps(xmmHTemp2, xmmCmpMaxG);
        // calculate hue component in CASE 3
        __m128 xmmHTemp3 = _mm_add_ps(xmm171const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmRRRR, xmmGGGG)), xmmInvDiff));
        xmmHTemp3 = _mm_andnot_ps(xmmElse, xmmHTemp3);

        // the final hue value is mask-selected from the temporary ones
        // based on the result of comparisons
        __m128 xmmHHHH = _mm_or_ps(xmmHTemp1, _mm_or_ps(xmmHTemp2, xmmHTemp3));
        xmmHHHH = _mm_andnot_ps(xmmHHHH, xmmCmpSZero);
        xmmHHHH = _mm_andnot_ps(xmmHHHH, xmmCmpVZero);
        xmmSSSS = _mm_andnot_ps(xmmSSSS, xmmCmpVZero);

        // cast the values to integer domain
        __m128i xmmHHHHi = _mm_cvtps_epi32(xmmHHHH);

        _mm_store_ss(reinterpret_cast<float*>(destinationHueChannel + ih), xmmHHHH);
        _mm_store_ss(reinterpret_cast<float*>(destinationSaturationChannel + is), xmmSSSS);
        _mm_store_ss(reinterpret_cast<float*>(destinationValueChannel + iv), xmmVVVV);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Convert_32bgra_24hsv_SSE2_MT(READONLY(uint8_t*)  source,
                             READWRITE(uint8_t*) destinationHueChannel,
                             READWRITE(uint8_t*) destinationSaturationChannel,
                             READWRITE(uint8_t*) destinationValueChannel,
                             uint32_t            sizeBytes)
{
    __m128i xmmMaskB = _mm_set1_epi32(0x000000ff);
    __m128i xmmMaskG = _mm_set1_epi32(0x0000ff00);
    __m128i xmmMaskR = _mm_set1_epi32(0x00ff0000);
    __m128 xmmZeroconst = _mm_set1_ps(0.0f);
    __m128 xmm255const = _mm_set1_ps(255.0f);
    __m128 xmm43const = _mm_set1_ps(43.0f);
    __m128 xmm171const = _mm_set1_ps(171.0f);
    __m128 xmm85const = _mm_set1_ps(85.0f);

    int32_t nSizeBytes = int32_t(sizeBytes);

#pragma omp parallel for
    for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
    {
        __m128i xmmBGRA = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
        __m128 xmmBBBB = _mm_cvtepi32_ps(_mm_and_si128(xmmBGRA, xmmMaskB));
        __m128 xmmGGGG = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmMaskG), 8));
        __m128 xmmRRRR = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmMaskR), 16));
        __m128 xmmRgbMin = _mm_min_ps(xmmBBBB, _mm_min_ps(xmmGGGG, xmmRRRR));
        __m128 xmmRgbMax = _mm_max_ps(xmmBBBB, _mm_max_ps(xmmGGGG, xmmRRRR));
        __m128 xmmVVVV = xmmRgbMax;
        __m128 xmmDiff = _mm_sub_ps(xmmRgbMax, xmmRgbMin);
        __m128 xmmInvDiff = _mm_rcp_ps(xmmDiff);
        __m128 xmmSSSS = _mm_mul_ps(xmmDiff, xmm255const);
        xmmSSSS = _mm_mul_ps(xmmSSSS, xmmInvDiff);
        __m128 xmmCmpVZero = _mm_cmpeq_ps(xmmVVVV, xmmZeroconst);
        __m128 xmmCmpSZero = _mm_cmpeq_ps(xmmSSSS, xmmZeroconst); 
        __m128 xmmCmpMaxR = _mm_cmpeq_ps(xmmRgbMax, xmmRRRR);      
        __m128 xmmCmpMaxG = _mm_cmpeq_ps(xmmRgbMax, xmmGGGG);      
        __m128 xmmElse = _mm_or_ps(xmmCmpMaxR, xmmCmpMaxG);       
        __m128 xmmHTemp1 = _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmGGGG, xmmBBBB)), xmmInvDiff);
        xmmHTemp1 = _mm_and_ps(xmmHTemp1, xmmCmpMaxR);
        __m128 xmmHTemp2 = _mm_add_ps(xmm85const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmBBBB, xmmRRRR)), xmmInvDiff));
        xmmHTemp2 = _mm_and_ps(xmmHTemp2, xmmCmpMaxG);
        __m128 xmmHTemp3 = _mm_add_ps(xmm171const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmRRRR, xmmGGGG)), xmmInvDiff));
        xmmHTemp3 = _mm_andnot_ps(xmmElse, xmmHTemp3);
        __m128 xmmHHHH = _mm_or_ps(xmmHTemp1, _mm_or_ps(xmmHTemp2, xmmHTemp3));
        xmmHHHH = _mm_andnot_ps(xmmHHHH, xmmCmpSZero);
        xmmHHHH = _mm_andnot_ps(xmmHHHH, xmmCmpVZero);
        xmmSSSS = _mm_andnot_ps(xmmSSSS, xmmCmpVZero);
        __m128i xmmHHHHi = _mm_cvtps_epi32(xmmHHHH);
        _mm_store_ss(reinterpret_cast<float*>(destinationHueChannel + index / 4), xmmHHHH);
        _mm_store_ss(reinterpret_cast<float*>(destinationSaturationChannel + index / 4), xmmSSSS);
        _mm_store_ss(reinterpret_cast<float*>(destinationValueChannel + index / 4), xmmVVVV);
    }

    return OperationSuccess;
}

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
