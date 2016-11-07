
#include "ImageProcessing_SSE2.h"
#include "ModuleSetupPerformer.h"
#include "HelpersSIMD.hpp"
#include "ProfileTools.h"

#include <cstdint>

using namespace Constants;

// ====================================================
// 24bpp Image Operations
// ====================================================

IMPROC_MODULE(Blend24bgr_24bgr_SSE2,
    READONLY(uint8_t*) source,
    READONLY(uint8_t*) target,
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(Blend24bgr_24bgr_SSE2_MT,
    READONLY(uint8_t*) source,
    READONLY(uint8_t*) target,
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_MODULE(Convert_32bgra_24hsv_SSE2,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destinationHueChannel,
    READWRITE(uint8_t*) destinationSaturationChannel,
    READWRITE(uint8_t*) destinationValueChannel,
    uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

    int ih = 0, is = 0, iv = 0;
    for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE, ih += 4, is += 4, iv += 4)
    {
        // read from input data, format: BGRA BGRA BGRA BGRA
        __m128i xmmBGRA = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));

        // extract the individual channels and expand them to floating point
        // 000B 000B 000B 000B (FP32)
        __m128 xmmBBBB = _mm_cvtepi32_ps(_mm_and_si128(xmmBGRA, xmmBGRAMaskB));
        __m128 xmmGGGG = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmBGRAMaskG), 8));
        __m128 xmmRRRR = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmBGRAMaskR), 16));

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
        xmmSSSS = _mm_mul_ps(xmmSSSS, _mm_rcp_ps(xmmVVVV));

        __m128 xmmCmpVZero = _mm_cmpeq_ps(xmmVVVV, xmm0const);  // check if v == 0
        __m128 xmmCmpSZero = _mm_cmpeq_ps(xmmSSSS, xmm0const);  // check if s == 0
        __m128 xmmCmpMaxR = _mm_cmpeq_ps(xmmRgbMax, xmmRRRR);      // check if rgbMax == r (CASE 1)
        __m128 xmmCmpMaxG = _mm_cmpeq_ps(xmmRgbMax, xmmGGGG);      // check if rgbMax == g (CASE 2)
        __m128 xmmElse = _mm_cmpeq_ps(xmmRgbMax, xmmBBBB);         // check if rgbMax == b (CASE 3)

        // calculate hue component in CASE 1
        __m128 xmmHTemp1 = _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmGGGG, xmmBBBB)), xmmInvDiff);
        xmmHTemp1 = _mm_and_ps(xmmHTemp1, xmmCmpMaxR);
        // calculate hue component in CASE 2
        __m128 xmmHTemp2 = _mm_add_ps(xmm85const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmBBBB, xmmRRRR)), xmmInvDiff));
        xmmHTemp2 = _mm_and_ps(xmmHTemp2, xmmCmpMaxG);
        // calculate hue component in CASE 3
        __m128 xmmHTemp3 = _mm_add_ps(xmm171const, _mm_mul_ps(_mm_mul_ps(xmm43const, _mm_sub_ps(xmmRRRR, xmmGGGG)), xmmInvDiff));
        xmmHTemp3 = _mm_and_ps(xmmElse, xmmHTemp3);

        // the final hue value is mask-selected from the temporary ones
        // based on the result of comparisons
        __m128 xmmHHHH = _mm_or_ps(xmmHTemp1, _mm_or_ps(xmmHTemp2, xmmHTemp3));
        xmmHHHH = _mm_andnot_ps(xmmCmpSZero, xmmHHHH);
        xmmHHHH = _mm_andnot_ps(xmmCmpVZero, xmmHHHH);
        xmmSSSS = _mm_andnot_ps(xmmCmpVZero, xmmSSSS);

        __m128i xmmHHHHi = _mm_packus_epi16(_mm_cvtps_epi32(xmmHHHH), _mm_cvtps_epi32(xmmHHHH));
        __m128i xmmSSSSi = _mm_packus_epi16(_mm_cvtps_epi32(xmmSSSS), _mm_cvtps_epi32(xmmSSSS));
        __m128i xmmVVVVi = _mm_packus_epi16(_mm_cvtps_epi32(xmmVVVV), _mm_cvtps_epi32(xmmVVVV));

        //h3h1h2h0
        int dword = _mm_extract_epi16(_mm_and_si128(xmmHHHHi, _mm_srli_si128(xmmHHHHi, 24)), 0);
        int htuple = ((dword << 8) & 0x00FF0000) || ((dword >> 8) & 0x0000FF00) || (dword & 0xFF0000FF);

        dword = _mm_extract_epi16(_mm_and_si128(xmmSSSSi, _mm_srli_si128(xmmSSSSi, 24)), 0);
        int stuple = ((dword << 8) & 0x00FF0000) || ((dword >> 8) & 0x0000FF00) || (dword & 0xFF0000FF);

        dword = _mm_extract_epi16(_mm_and_si128(xmmVVVVi, _mm_srli_si128(xmmVVVVi, 24)), 0);
        int vtuple = ((dword << 8) & 0x00FF0000) || ((dword >> 8) & 0x0000FF00) || (dword & 0xFF0000FF);

        *reinterpret_cast<int*>(destinationHueChannel + index / 4) = htuple;
        *reinterpret_cast<int*>(destinationSaturationChannel + index / 4) = stuple;
        *reinterpret_cast<int*>(destinationValueChannel + index / 4) = vtuple;
    }

    return OperationSuccess;
}

IMPROC_MODULE(Convert_32bgra_24hsv_SSE2_MT,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destinationHueChannel,
    READWRITE(uint8_t*) destinationSaturationChannel,
    READWRITE(uint8_t*) destinationValueChannel,
    uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

#pragma omp parallel for
    for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
    {
        __m128i xmmBGRA = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
        __m128 xmmBBBB = _mm_cvtepi32_ps(_mm_and_si128(xmmBGRA, xmmBGRAMaskB));
        __m128 xmmGGGG = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmBGRAMaskG), 8));
        __m128 xmmRRRR = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(xmmBGRA, xmmBGRAMaskR), 16));
        __m128 xmmRgbMin = _mm_min_ps(xmmBBBB, _mm_min_ps(xmmGGGG, xmmRRRR));
        __m128 xmmRgbMax = _mm_max_ps(xmmBBBB, _mm_max_ps(xmmGGGG, xmmRRRR));
        __m128 xmmVVVV = xmmRgbMax;
        __m128 xmmDiff = _mm_sub_ps(xmmRgbMax, xmmRgbMin);
        __m128 xmmInvDiff = _mm_rcp_ps(xmmDiff);
        __m128 xmmSSSS = _mm_mul_ps(xmmDiff, xmm255const);
        xmmSSSS = _mm_mul_ps(xmmSSSS, xmmInvDiff);
        __m128 xmmCmpVZero = _mm_cmpeq_ps(xmmVVVV, xmm0const);
        __m128 xmmCmpSZero = _mm_cmpeq_ps(xmmSSSS, xmm0const);
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

IMPROC_MODULE(OpacityAdjust_32bgra_SSE2,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes,
    float               percentage)
{
    uint8_t a = uint8_t(percentage * 255.0f);
    __m128i alphaLevel = _mm_set_epi8(a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0);
    int32_t nSizeBytes = int32_t(sizeBytes);

    int64_t alphaInvMaski64Low = alphaInvMask.m128i_u64[0];
    int64_t alphaInvMaski64High = alphaInvMask.m128i_u64[1];

    int64_t alphaLeveli64Low = alphaLevel.m128i_u64[0];
    int64_t alphaLeveli64High = alphaLevel.m128i_u64[1];

    if (SIMDAligned(source, destination))
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_A);

        // PIPELINED
        /*__m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source));
        __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
        src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + 16));*/

        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            // PIPELINED
            /*_mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);
            dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index + 48));*/

            // NAIVE
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            // INT + SSE pipelines
            /*int64_t lowSrc = *(int64_t*)(source + index + 16);
            int64_t highSrc = *(int64_t*)(source + index + 24);

            int64_t lowDst = (lowSrc & alphaInvMaski64Low) | alphaLeveli64Low;
            int64_t highDst = (highSrc & alphaInvMaski64High) | alphaLeveli64High;

            *(int64_t*)(destination + index + 16) = lowDst;
            *(int64_t*)(destination + index + 24) = highDst;*/

            END_TIMED_BLOCK();
        }

        // pointer addressing
        const uint8_t *srcEnd = source + nSizeBytes - SIMD_SIZE;
        const uint8_t *dstEnd = destination + nSizeBytes - SIMD_SIZE;
        uint8_t *d = destination;
        for (const uint8_t *s = source; s <= srcEnd; s += SIMD_SIZE, d += SIMD_SIZE)
        {
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(s));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            _mm_stream_si128(reinterpret_cast<__m128i*>(d), dst);
        }

        // PIPELINED pointer addressing (unrolled x2)
        /*const uint8_t *srcEnd = source + nSizeBytes - 8*SIMD_SIZE;
        const uint8_t *dstEnd = destination + nSizeBytes - 8*SIMD_SIZE;
        uint8_t *d = destination;

        __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source));
        __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
        src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + 16));

        __m128i src2 = _mm_load_si128(reinterpret_cast<const __m128i*>(source + 48));
        __m128i dst2 = _mm_or_si128(_mm_and_si128(src2, alphaInvMask), alphaLevel);
        src2 = _mm_load_si128(reinterpret_cast<const __m128i*>(source + 48 + 16));

        for (const uint8_t *s = source; s <= srcEnd; s += 2*SIMD_SIZE, d += 2*SIMD_SIZE)
        {
        _mm_prefetch((const char*)(s + 96 + 16), _MM_HINT_T1);

        _mm_stream_si128(reinterpret_cast<__m128i*>(d), dst);
        dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
        src = _mm_load_si128(reinterpret_cast<const __m128i*>(s + 48));

        _mm_stream_si128(reinterpret_cast<__m128i*>(d+48), dst);
        dst2 = _mm_or_si128(_mm_and_si128(src2, alphaInvMask), alphaLevel);
        src2 = _mm_load_si128(reinterpret_cast<const __m128i*>(s + 96));
        }*/

        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }
    else
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_U);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(OpacityAdjust_32bgra_SSE2_MT,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes,
    float               percentage)
{
    uint8_t a = uint8_t(percentage * 255.0f);
    __m128i alphaLevel = _mm_set_epi8(a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, destination))
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_MT_A);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }
    else
    {
        REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_SSE2_MT_U);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i dst = _mm_or_si128(_mm_and_si128(src, alphaInvMask), alphaLevel);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(AlphaBlend32bgra_32bgra_SSE2,
    READONLY(uint8_t*) source,
    READONLY(uint8_t*) target,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes)
{
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));

            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, xmmRcp255const), _mm_sub_ps(xmm255const, sAAAAf32));
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));
            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, xmmRcp255const), _mm_sub_ps(xmm255const, sAAAAf32));
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(AlphaBlend32bgra_32bgra_SSE2_MT,
    READONLY(uint8_t*) source,
    READONLY(uint8_t*) target,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes)
{
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));

            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, xmmRcp255const), _mm_sub_ps(xmm255const, sAAAAf32));
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
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
            __m128 sBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(src, xmmBGRAMaskB));
            __m128 tBBBBf32 = _mm_cvtepi32_ps(_mm_and_si128(tar, xmmBGRAMaskB));
            __m128 sGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskG), 8));
            __m128 tGGGGf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskG), 8));
            __m128 sRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(src, xmmBGRAMaskR), 16));
            __m128 tRRRRf32 = _mm_cvtepi32_ps(_mm_srli_epi32(_mm_and_si128(tar, xmmBGRAMaskR), 16));
            __m128 tmpf32 = _mm_mul_ps(_mm_mul_ps(tAAAAf32, xmmRcp255const), _mm_sub_ps(xmm255const, sAAAAf32));
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
        PROFILE_TRACE_BLOCK(L" - Cycles/4Px: ");
    }

    return OperationSuccess;
}

IMPROC_MODULE(ConvFilter_32bgra_SSE2,
    READONLY(uint8_t*)  source,
    READWRITE(uint8_t*) destination,
    uint32_t            sizeBytes,
    uint32_t            strideBytes,
    READONLY(float*)    kernel,
    uint32_t            width,
    uint32_t            height)
{
    /*
    * 3x3 kernel
    * Pixel Pij (source) -> Pij'(result)
    * 3 SIMD loads, from consecutive rows:
    * l0 = [p0 p1 p2  p3 ]
    * l1 = [p4 p5 p6  p7 ] (p5 = pij, p6 = pij+1)
    * l2 = [p8 p9 p10 p11]
    *
    * fi(filter) [32-bit float] -> fi*N [16-bit int] (normalize) (N=255)
    *
    * f0 f1 f2
    * f3 f4 f5
    * f6 f7 f8
    *
    *
    */

    int32_t nSizeBytes = int32_t(sizeBytes);
    int32_t begin = strideBytes + 4 * width / 2;
    int32_t upperLimit = nSizeBytes - strideBytes * 4 * height / 2 - 4 * width / 2 * width;
    int32_t iterationStep = 4;
    int32_t windowStride = height - 1 * strideBytes;

    //if (SIMDAligned(source, destination))
    {
        //REGISTER_TIMED_BLOCK(ConvFilter_32bgra_SSE2_A);
        for (int32_t index = strideBytes; index <= upperLimit; index += iterationStep)
        {
            //BEGIN_TIMED_BLOCK();

            for (int i = 0; i <= windowStride; i++)
                _mm_prefetch(reinterpret_cast<const char*>(source + index + i), _MM_HINT_T0);

        }
    }

    return OperationSuccess;
}