
#include "ImageProcessing_SSSE3.h"
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
AlphaBlend32bgra_32bgra_SSSE3(READONLY (uint8_t*) source,
                              READONLY (uint8_t*) target,
                              READWRITE(uint8_t*) destination,
                              uint32_t            sizeBytes)
{
    // Mask out alpha channel in 4 pixels
    __m128i alpha_mask = _mm_set_epi8(0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0);

    // Splicing masks: convert from low-8-bit parts of 4 x INT32 => 8 x INT16
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AL2|0|AL2|0|AL2|0|AL2|0|AL1|0|AL1|0|AL1|0AL1]
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AH2|0|AH2|0|AH2|0|AH2|0|AH1|0|AH1|0|AH1|0AH1]
    __m128i alpha_splice_lo_mask = _mm_set_epi8(0x80, 4, 0x80, 4, 0x80, 4, 0x80, 4, 0x80, 0, 0x80, 0, 0x80, 0, 0x80, 0);
    __m128i alpha_splice_hi_mask = _mm_set_epi8(0x80, 12, 0x80, 12, 0x80, 12, 0x80, 12, 0x80, 8, 0x80, 8, 0x80, 8, 0x80, 8);

    // Constant vectors: 8 x INT16 = {256}, 16  x INT8 = {255}
    __m128i one256_8x = _mm_set1_epi16(256);
    __m128i one255_16x = _mm_set1_epi8(255);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSSE3_A);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            // load 4 BGRA pixels (source and target)
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            // TODO: This will be 0 if source alpha = 255, handle this here
            __m128i tara_scaled = _mm_scale_epu8(_mm_sub_epi8(one255_16x, src), tar);  // alpha_tar * (255 - alpha_src) / 255            
            __m128i dsta_scaled = _mm_srli_epi32(_mm_and_si128(src, alpha_mask), 24);  // Keep the 4 alpha values from tara_scaled
            dsta_scaled = _mm_slli_epi32(dsta_scaled, 8);                              // Multiply by 256
            __m128i dsta = _mm_add_epi8(tara_scaled, src);                             // Output alpha [000A 000A 000A 000A]
            dsta = _mm_srli_epi32(_mm_and_si128(dsta, alpha_mask), 24);                // Keep the 4 alpha values from dsta

            __m128 dsta_scaled_f32 = _mm_cvtepi32_ps(dsta_scaled);          // Convert the 4 alpha values to FP32
            __m128 dsta_f32 = _mm_cvtepi32_ps(dsta);                        // Convert the 4 alpha values to FP32                                                                                                   
            // BUG: Division by 0 if resulting alpha = 0
            __m128 scale_fact_f32 = _mm_div_ps(dsta_scaled_f32, dsta_f32);  // Perform FP32 divide: (alpha_dst - alpha_src) * 256 / alpha_dst
            __m128i scale_fact = _mm_cvttps_epi32(scale_fact_f32);          // Truncate result back to INT32   

            __m128i scale_fact_lo = _mm_shuffle_epi8(scale_fact, alpha_splice_lo_mask); // Distribute the 4 scaling factors 
            __m128i scale_fact_hi = _mm_shuffle_epi8(scale_fact, alpha_splice_hi_mask); // to every color channel for each pixel

            __m128i scale_fact_lo_offset = _mm_sub_epi16(one256_8x, scale_fact_lo);  // Offsetted scale_factor (in 16bit): 
            __m128i scale_fact_hi_offset = _mm_sub_epi16(one256_8x, scale_fact_hi);  // 256 - scale_factor

            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());  // Unpack [...ARGB] into [...0A0R0G0B]
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());  // aka zero-extend each channel 8bit -> 16bit   
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            __m128i dstlo = _mm_srli_epi16(_mm_add_epi16(       // Output color channel calculation (16bit precision):
                _mm_mullo_epi16(srclo, scale_fact_lo_offset),   // ((256 - scale_factor) * source + scale_factor * target) / 256
                _mm_mullo_epi16(tarlo, scale_fact_lo)), 8);     // Note: only BGR outputs matter, the 4th alpha is discarded here
            __m128i dsthi = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srchi, scale_fact_hi_offset),
                _mm_mullo_epi16(tarhi, scale_fact_hi)), 8);

            __m128i dst = _mm_packus_epi16(dstlo, dsthi);       // Repack the 16bit pixel values to [0-255] 

            // store 4 output pixels
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSSE3_U);
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128i tara_scaled = _mm_scale_epu8(_mm_sub_epi8(one255_16x, src), tar);
            __m128i dsta_scaled = _mm_srli_epi32(_mm_and_si128(src, alpha_mask), 24);
            dsta_scaled = _mm_slli_epi32(dsta_scaled, 8);
            __m128i dsta = _mm_add_epi8(tara_scaled, src);
            dsta = _mm_srli_epi32(_mm_and_si128(dsta, alpha_mask), 24);
            __m128 dsta_scaled_f32 = _mm_cvtepi32_ps(dsta_scaled);
            __m128 dsta_f32 = _mm_cvtepi32_ps(dsta);
            __m128 scale_fact_f32 = _mm_div_ps(dsta_scaled_f32, dsta_f32);
            __m128i scale_fact = _mm_cvttps_epi32(scale_fact_f32);
            __m128i scale_fact_lo = _mm_shuffle_epi8(scale_fact, alpha_splice_lo_mask);
            __m128i scale_fact_hi = _mm_shuffle_epi8(scale_fact, alpha_splice_hi_mask);
            __m128i scale_fact_lo_offset = _mm_sub_epi16(one256_8x, scale_fact_lo);
            __m128i scale_fact_hi_offset = _mm_sub_epi16(one256_8x, scale_fact_hi);
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i dstlo = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srclo, scale_fact_lo_offset),
                _mm_mullo_epi16(tarlo, scale_fact_lo)), 8);
            __m128i dsthi = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srchi, scale_fact_hi_offset),
                _mm_mullo_epi16(tarhi, scale_fact_hi)), 8);
            __m128i dst = _mm_packus_epi16(dstlo, dsthi);
            _mm_storeu_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_SSSE3_MT(READONLY (uint8_t*) source,
                                 READONLY (uint8_t*) target,
                                 READWRITE(uint8_t*) destination,
                                 uint32_t            sizeBytes)
{
    // Mask out alpha channel in 4 pixels
    __m128i alpha_mask = _mm_set_epi8(0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0);

    // Splicing masks: convert from low-8-bit parts of 4 x INT32 => 8 x INT16
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AL2|0|AL2|0|AL2|0|AL2|0|AL1|0|AL1|0|AL1|0AL1]
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AH2|0|AH2|0|AH2|0|AH2|0|AH1|0|AH1|0|AH1|0AH1]
    __m128i alpha_splice_lo_mask = _mm_set_epi8(0x80, 4, 0x80, 4, 0x80, 4, 0x80, 4, 0x80, 0, 0x80, 0, 0x80, 0, 0x80, 0);
    __m128i alpha_splice_hi_mask = _mm_set_epi8(0x80, 12, 0x80, 12, 0x80, 12, 0x80, 12, 0x80, 8, 0x80, 8, 0x80, 8, 0x80, 8);

    // Constant vectors: 8 x INT16 = {256}, 16  x INT8 = {255}
    __m128i one256_8x = _mm_set1_epi16(256);
    __m128i one255_16x = _mm_set1_epi8(255);

    int32_t nSizeBytes = int32_t(sizeBytes);

    if (SIMDAligned(source, target, destination))
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSSE3_MT_A);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            // load 4 BGRA pixels (source and target)
            __m128i src = _mm_load_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_load_si128(reinterpret_cast<const __m128i*>(target + index));

            // TODO: This will be 0 if source alpha = 255, handle this here
            __m128i tara_scaled = _mm_scale_epu8(_mm_sub_epi8(one255_16x, src), tar);   // alpha_tar * (255 - alpha_src) / 255            
            __m128i dsta_scaled = _mm_srli_epi32(_mm_and_si128(src, alpha_mask), 24);   // Keep the 4 alpha values from tara_scaled
            dsta_scaled = _mm_slli_epi32(dsta_scaled, 8);                              // Multiply by 256
            __m128i dsta = _mm_add_epi8(tara_scaled, src);                              // Output alpha [000A 000A 000A 000A]
            dsta = _mm_srli_epi32(_mm_and_si128(dsta, alpha_mask), 24);         // Keep the 4 alpha values from dsta

            __m128 dsta_scaled_f32 = _mm_cvtepi32_ps(dsta_scaled);              // Convert the 4 alpha values to FP32
            __m128 dsta_f32 = _mm_cvtepi32_ps(dsta);                     // Convert the 4 alpha values to FP32                                                                                                   
            // BUG: Division by 0 if resulting alpha = 0
            __m128 scale_fact_f32 = _mm_div_ps(dsta_scaled_f32, dsta_f32);     // Perform FP32 divide: (alpha_dst - alpha_src) * 256 / alpha_dst
            __m128i scale_fact = _mm_cvttps_epi32(scale_fact_f32);          // Truncate result back to INT32   

            __m128i scale_fact_lo = _mm_shuffle_epi8(scale_fact, alpha_splice_lo_mask); // Distribute the 4 scaling factors 
            __m128i scale_fact_hi = _mm_shuffle_epi8(scale_fact, alpha_splice_hi_mask); // to every color channel for each pixel

            __m128i scale_fact_lo_offset = _mm_sub_epi16(one256_8x, scale_fact_lo);  // Offsetted scale_factor (in 16bit): 
            __m128i scale_fact_hi_offset = _mm_sub_epi16(one256_8x, scale_fact_hi);  // 256 - scale_factor

            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());  // Unpack [...ARGB] into [...0A0R0G0B]
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());  // aka zero-extend each channel 8bit -> 16bit   
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());

            __m128i dstlo = _mm_srli_epi16(_mm_add_epi16(       // Output color channel calculation (16bit precision):
                _mm_mullo_epi16(srclo, scale_fact_lo_offset),   // ((256 - scale_factor) * source + scale_factor * target) / 256
                _mm_mullo_epi16(tarlo, scale_fact_lo)), 8);     // Note: only BGR outputs matter, the 4th alpha is discarded here
            __m128i dsthi = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srchi, scale_fact_hi_offset),
                _mm_mullo_epi16(tarhi, scale_fact_hi)), 8);

            __m128i dst = _mm_packus_epi16(dstlo, dsthi);       // Repack the 16bit pixel values to [0-255] 

            // store 4 output pixels
            _mm_stream_si128(reinterpret_cast<__m128i*>(destination + index), dst);

            END_TIMED_BLOCK();
        }
        PROFILE_TRACE_BLOCK(PROFILE_MSG_PIXELS_PER_ITER);
    }
    else
    {
        REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_SSSE3_MT_U);
#pragma omp parallel for
        for (int32_t index = 0; index <= nSizeBytes - SIMD_SIZE; index += SIMD_SIZE)
        {
            BEGIN_TIMED_BLOCK();

            __m128i src = _mm_loadu_si128(reinterpret_cast<const __m128i*>(source + index));
            __m128i tar = _mm_loadu_si128(reinterpret_cast<const __m128i*>(target + index));
            __m128i tara_scaled = _mm_scale_epu8(_mm_sub_epi8(one255_16x, src), tar);
            __m128i dsta_scaled = _mm_srli_epi32(_mm_and_si128(src, alpha_mask), 24);
            dsta_scaled = _mm_slli_epi32(dsta_scaled, 8);
            __m128i dsta = _mm_add_epi8(tara_scaled, src);
            dsta = _mm_srli_epi32(_mm_and_si128(dsta, alpha_mask), 24);
            __m128 dsta_scaled_f32 = _mm_cvtepi32_ps(dsta_scaled);
            __m128 dsta_f32 = _mm_cvtepi32_ps(dsta);
            __m128 scale_fact_f32 = _mm_div_ps(dsta_scaled_f32, dsta_f32);
            __m128i scale_fact = _mm_cvttps_epi32(scale_fact_f32);
            __m128i scale_fact_lo = _mm_shuffle_epi8(scale_fact, alpha_splice_lo_mask);
            __m128i scale_fact_hi = _mm_shuffle_epi8(scale_fact, alpha_splice_hi_mask);
            __m128i scale_fact_lo_offset = _mm_sub_epi16(one256_8x, scale_fact_lo);
            __m128i scale_fact_hi_offset = _mm_sub_epi16(one256_8x, scale_fact_hi);
            __m128i srclo = _mm_unpacklo_epi8(src, _mm_setzero_si128());
            __m128i tarlo = _mm_unpacklo_epi8(tar, _mm_setzero_si128());
            __m128i srchi = _mm_unpackhi_epi8(src, _mm_setzero_si128());
            __m128i tarhi = _mm_unpackhi_epi8(tar, _mm_setzero_si128());
            __m128i dstlo = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srclo, scale_fact_lo_offset),
                _mm_mullo_epi16(tarlo, scale_fact_lo)), 8);
            __m128i dsthi = _mm_srli_epi16(_mm_add_epi16(
                _mm_mullo_epi16(srchi, scale_fact_hi_offset),
                _mm_mullo_epi16(tarhi, scale_fact_hi)), 8);
            __m128i dst = _mm_packus_epi16(dstlo, dsthi);
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
