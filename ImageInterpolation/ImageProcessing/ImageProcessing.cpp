
// local include
#include "ImageProcessing.h"
#include "ReferenceProcessing.h"
#include "Common.h"
#include "HelpersSIMD.cpp"

// system include
#include <stdint.h>


#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable: 4309) // disable truncation of constant value warning
#endif

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Blend24bgr_24bgr(READONLY (uint8_t*) source,
                 READONLY (uint8_t*) target,
                 READWRITE(uint8_t*) destination,
                 uint32_t            sizeBytes,
                 float               percentage)
{
    WANT_REFERENCE_IMPL(Blend24bgr_24bgr,
        source, target, destination, sizeBytes, percentage);

    int32_t szb = sizeBytes;

    // prescaled alpha coefficients mapped from [0.0-1.0] to [0-255]
    __m128i alpha16   = _mm_set1_epi16((int16_t)(percentage * 255.0f));
    __m128i _1alpha16 = _mm_set1_epi16((int16_t)((1.0f - percentage) * 255.0f));

    if (AlignCheck(source, target, destination))
    {
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            __m128i src = _mm_load_si128((const __m128i *)(source + index));
            __m128i tar = _mm_load_si128((const __m128i *)(target + index));

            // unpack [BGRBGR...] into [B0G0R0B0G0R0...] (extend each color channel to 16-bit)
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            // compute 16-bit alpha * target, (1-alpha) * source
            __m128i taralphalo   = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi   = _mm_mullo_epi16(tarhi, alpha16);
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
            _mm_stream_si128((__m128i *)(destination + index), dst);
        }
    }
    else
    {
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            __m128i src   = _mm_loadu_si128((const __m128i *)(source + index));
            __m128i tar   = _mm_loadu_si128((const __m128i *)(target + index));
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i taralphalo   = _mm_mullo_epi16(tarlo, alpha16);
            __m128i taralphahi   = _mm_mullo_epi16(tarhi, alpha16);
            __m128i _1srcalphalo = _mm_mullo_epi16(srclo, _1alpha16);
            __m128i _1srcalphahi = _mm_mullo_epi16(srchi, _1alpha16);
            __m128i dstlo = _mm_add_epi16(taralphalo, _1srcalphalo);
            __m128i dsthi = _mm_add_epi16(taralphahi, _1srcalphahi);
            __m128i dst   = _mm_packus_epi16(_mm_div255_epu16(dstlo), _mm_div255_epu16(dsthi));
            _mm_storeu_si128((__m128i *)(destination + index), dst);
        }
    }

    // Handle non-multiple of SIMD size images using reference implementation
    /*if (index < szb)
    {
        return REFERENCE_IMPL(Blend24bgr_24bgr,
            source + index, target + index, destination + index, sizeBytes - index, percentage);
    }*/

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
OpacityAdjust_32bgra(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage)
{
    WANT_REFERENCE_IMPL(OpacityAdjust_32bgra,
       source, destination, sizeBytes, percentage);

    int32_t szb = sizeBytes;

    uint8_t a = (uint8_t)(percentage * 255.0f);
    __m128i alphaMask  = _mm_set_epi8(0, 0x80, 0x80, 0x80, 0, 0x80, 0x80, 0x80, 0, 0x80, 0x80, 0x80, 0, 0x80, 0x80, 0x80);
    __m128i alphaLevel = _mm_set_epi8(a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0, a, 0, 0, 0);
    
    if (AlignCheck(source, destination))
    {
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            __m128i src   = _mm_load_si128((const __m128i *)(source + index));
            __m128i dst   = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            //__m128i dst = _mm_blendv_epi8(alphaLevel, src, alphaMask); // NOTE: SSE4.1 instruction!
            _mm_stream_si128((__m128i *)(destination + index), dst);
        }
    }
    else
    {
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            __m128i src   = _mm_loadu_si128((const __m128i *)(source + index));
            __m128i dst   = _mm_or_si128(_mm_and_si128(src, alphaMask), alphaLevel);
            //__m128i dst = _mm_blendv_epi8(alphaLevel, src, alphaMask);
            _mm_storeu_si128((__m128i *)(destination + index), dst);
        }
    }

    // Handle non-multiple of SIMD size images using reference implementation
    /*if (index < szb)
    {
        return REFERENCE_IMPL(OpacityAdjust_32bgra,
            source + index, destination + index, sizeBytes - index, percentage);
    }*/

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
AlphaBlend32bgra_32bgra(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes)
{
    WANT_REFERENCE_IMPL(AlphaBlend32bgra_32bgra,
        source, target, destination, sizeBytes);

    int32_t szb = sizeBytes;

    // Mask out alpha channel in 4 pixels
    __m128i alpha_mask = _mm_set_epi8(0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0);
    
    // Splicing masks: convert from 4 x INT32 => 8 x INT16
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AL2|0|AL2|0|AL2|0|AL2|0|AL1|0|AL1|0|AL1|0AL1]
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AH2|0|AH2|0|AH2|0|AH2|0|AH1|0|AH1|0|AH1|0AH1]
    __m128i alpha_splice_lo_mask = _mm_set_epi8(0x80, 4, 0x80, 4, 0x80, 4, 0x80, 4, 0x80, 0, 0x80, 0, 0x80, 0, 0x80, 0);
    __m128i alpha_splice_hi_mask = _mm_set_epi8(0x80, 12, 0x80, 12, 0x80, 12, 0x80, 12, 0x80, 8, 0x80, 8, 0x80, 8, 0x80, 8);
    
    // Constant vectors: 8 x INT16 = {256}, 16  x INT8 = {255}
    __m128i bunch_8x_256  = _mm_set1_epi16(256); 
    __m128i bunch_16x_255 = _mm_set1_epi8(255);

    if (AlignCheck(source, destination))
    {
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            // load 4 BGRA pixels (source and target)
            __m128i src = _mm_load_si128((const __m128i *)(source + index));
            __m128i tar = _mm_load_si128((const __m128i *)(target + index));

            __m128i tara_scaled = _mm_scale_epu8(_mm_sub_epi8(bunch_16x_255, src), tar); // alpha_tar * (255 - alpha_src) / 255            
            __m128i dsta_scaled = _mm_srli_si128(_mm_and_si128(src, alpha_mask), 3);     // Keep the 4 alpha values from tara_scaled
            dsta_scaled         = _mm_slli_si128(dsta_scaled, 8);                        // Multiply by 256
            __m128i dsta        = _mm_add_epi8(tara_scaled, src);                        // Output alpha
            dsta                = _mm_srli_si128(_mm_and_si128(dsta, alpha_mask), 3);    // Keep the 4 alpha values from dsta
            
            __m128 dsta_scaled_f32 = _mm_cvtepi32_ps(dsta_scaled);          // Convert the 4 alpha values to FP32
            __m128 dsta_f32        = _mm_cvtepi32_ps(dsta);                 // Convert the 4 alpha values to FP32                                                                                                   
            __m128 scale_fact_f32  = _mm_div_ps(dsta_scaled_f32, dsta_f32); // Perform FP32 divide: (alpha_dst - alpha_src) * 256 / alpha_dst
            __m128i scale_fact     = _mm_cvttps_epi32(scale_fact_f32);      // Truncate result back to INT32   

            // TODO: this is SuplementalSSE3, make it also SSE2
            __m128i scale_fact_lo = _mm_shuffle_epi8(scale_fact, alpha_splice_lo_mask); // Distribute the 4 scaling factors 
            __m128i scale_fact_hi = _mm_shuffle_epi8(scale_fact, alpha_splice_hi_mask); // to every color channel for each pixel

            __m128i scale_fact_lo_offset = _mm_sub_epi16(bunch_8x_256, scale_fact_lo);  // Offsetted scale_factor (in 16bit): 
            __m128i scale_fact_hi_offset = _mm_sub_epi16(bunch_8x_256, scale_fact_hi);  // 256 - scale_factor

            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());  // Unpack [...ARGB] into [...0A0R0G0B]
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());  // aka zero-extend each channel 8bit -> 16bit   
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            __m128i dstlo = _mm_srli_epi16(_mm_add_epi16(       // Output color channel calculation (16bit precision):
                _mm_mullo_epi16(srclo, scale_fact_lo_offset),   // ((256 - scale_factor) * source + scale_factor * target) / 256
                _mm_mullo_epi16(tarlo, scale_fact_lo)), 8);     // Note: only BGR outputs matter, the 4th alpha
            __m128i dsthi = _mm_srli_epi16(_mm_add_epi16(       // component in every pixel is not usable here
                _mm_mullo_epi16(srchi, scale_fact_hi_offset),
                _mm_mullo_epi16(tarhi, scale_fact_hi)), 8);

            __m128i dst = _mm_packus_epi16(dstlo, dsthi);       // Repack the 16bit pixel values to [0-255] 

            // store 4 output pixels
            _mm_stream_si128((__m128i *)(destination + index), dst);
        }
    }
    else
    {
        /*
#pragma omp parallel for
        for (int32_t index = 0; index <= szb - SIMD_SIZE; index += SIMD_SIZE)
        {
            __m128i src   = _mm_loadu_si128((const __m128i *)(source + index));
            __m128i tar   = _mm_loadu_si128((const __m128i *)(target + index));
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i dstlo = _mm_alphablendx2_epi16(srclo, tarlo);
            __m128i dsthi = _mm_alphablendx2_epi16(srchi, tarhi);
            __m128i dst   = _mm_packus_epi16(dstlo, dsthi);
            _mm_storeu_si128((__m128i *)(destination + index), dst);
        }*/
    }

    // Handle non-multiple of SIMD size images using reference implementation
    /*if (index < szb)
    {
        return REFERENCE_IMPL(AlphaBlend32bgra_32bgra,
            source + index, target + index, destination + index, sizeBytes - index);
    }*/

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
ConvFilter_32bgra(READONLY (uint8_t*) source,
                  READWRITE(uint8_t*) destination,
                  uint32_t            sizeBytes,
                  READONLY (float*)   kernel,
                  uint32_t            width,
                  uint32_t            height)
{
    WANT_REFERENCE_IMPL(ConvFilter_32bgra,
        source, destination, sizeBytes, kernel, width, height);

    uint32_t index = 0;

    // BUG: this crashes for values near the edges of filter window

    // Handle non-multiple of SIMD size images using reference implementation
    if (index < sizeBytes)
    {
        return REFERENCE_IMPL(ConvFilter_32bgra,
            source + index, destination + index, sizeBytes - index, kernel, width, height);
    }

    return OperationSuccess;
}

#ifdef _MSC_VER
#pragma warning(pop)
#endif
