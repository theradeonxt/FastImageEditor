
#pragma once

// local include
#include "Common.h"

#include <stdint.h>
#include <vectori128.h>

// system include
#include <emmintrin.h> // SSE2 intrinsics
#include <smmintrin.h> // SSE4.1 intrinsics

// ============================================================
// SIMD Extra Functions
// ============================================================

static __m128i
_mm_div255_epu16(__m128i x)
{
    // http://www.alfredklomp.com/programming/sse-intrinsics/
    // Divide 8 16-bit uints by 255:
    // x := ((x + 1) + (x >> 8)) >> 8:

    /*__m128i const_one = _mm_cmpeq_epi16(x, x);
    const_one = _mm_srli_epi16(const_one, 15);*/
    return _mm_srli_epi16(_mm_adds_epu16(
        _mm_adds_epu16(x, _mm_set1_epi16(1)),
        _mm_srli_epi16(x, 8)), 8);
}

static __m128i
_mm_scale_epu8(__m128i x, __m128i y)
{
    // http://www.alfredklomp.com/programming/sse-intrinsics/
    // Returns an "alpha blend" of x scaled by y/255;
    //   x := x * (y / 255)
    // Reorder: x := (x * y) / 255

    // Unpack x and y into 16-bit uints:
    __m128i xlo = _mm_unpacklo_epi8(x, _mm_setzero_si128());
    __m128i ylo = _mm_unpacklo_epi8(y, _mm_setzero_si128());
    __m128i xhi = _mm_unpackhi_epi8(x, _mm_setzero_si128());
    __m128i yhi = _mm_unpackhi_epi8(y, _mm_setzero_si128());

    // Multiply x with y, keeping the low 16 bits:
    xlo = _mm_mullo_epi16(xlo, ylo);
    xhi = _mm_mullo_epi16(xhi, yhi);

    // Divide by 255:
    xlo = _mm_div255_epu16(xlo);
    xhi = _mm_div255_epu16(xhi);

    // Repack the 16-bit uints to clamped 8-bit values:
    return _mm_packus_epi16(xlo, xhi);
}

static __m128
_mm_clamp_ps(__m128 value, __m128 min, __m128 max)
{
    // Limits the range of 4 packed singles between [min, max].
    // Notes: - use this when working with SIMD data, 
    //          because if the compiler inlines the call 
    //          the parameters will already be in CPU registers;
    //        - this will amount to just 3 instructions
    //          for working on 4 values simultaneously.

    return _mm_min_ps(_mm_max_ps(value, min), max);
}

struct divLookup_t
{
    divLookup_t()
    {
        // precomputed factors for division with 8bit values from 1-255
        divLookup[0] = 1; // don't use this
        for (int32_t i = 1; i < 256; i++) divLookup[i] = Divisor_us(i);
    }
    Divisor_us divLookup[256];
};
static const divLookup_t divLookup;

// vector with 8x16bit values of 255 and 256
static const __m128i m255 = _mm_set1_epi16(255);
static const __m128i m256 = _mm_set1_epi16(256);

// shuffle mask to propagate highest 16 bit values
// in each qword into every other 16 bit value
static const __m128i alpha_extend_mask = _mm_set_epi8(
    0xF, 0xF, 0xF, 0xF, 0xF, 0xF, 0xF, 0xF,
    0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7);

static __m128i 
_mm_alphablendx2_epi16(__m128i src, __m128i tar)
{
    // Perform alphablend operation on the 2 pixels in source and target operands
    // Each operand stores pixel channels as 16bit values, the pixel format is 32bpp-BGRA     

    // compute diff(alpha out - alpha source)
    // compute alpha out
    __m128i adas = _mm_scale_epu8(_mm_sub_epi16(m255, src), tar);
    __m128i ad = _mm_add_epi16(adas, src);

    // (alpha out - alpha source) * 256
    __m128i scaled_dsa = _mm_slli_epi16(adas, 8);

    // extract the resulting alpha values for pixel 0 and 1
    int adp0 = _mm_extract_epi16(ad, 3);
    int adp1 = _mm_extract_epi16(ad, 7);

    // ((alpha out - alpha source) * 256) / alpha out
    /*__m128i divdap0 = (Vec8us)scaled_dsa / divLookup.divLookup[adp0]; // [x  |x|x|x|Ap0|x|x|x]
    __m128i divdap1 = (Vec8us)scaled_dsa / divLookup.divLookup[adp1]; // [Ap1|x|x|x|x  |x|x|x]

    // mask = 11110111
    scaled_dsa = _mm_blend_epi16(divdap0, divdap1, 0xF7); // [Ap1|x|x|x|Ap0|x|x|x]
    */
    // propagate values to prepare for multiply with color channels
    scaled_dsa = _mm_shuffle_epi8(scaled_dsa, alpha_extend_mask); // [Ap1|Ap1|Ap1|Ap1|Ap0|Ap0|Ap0|Ap0]

    // multiply output alpha with color channels from sorce and target
    // 256 - scaled_dsa
    __m128i _256dsa = _mm_sub_epi16(m256, scaled_dsa);
    // tarlo * scaled_dsa
    __m128i blendtar = _mm_mullo_epi16(scaled_dsa, tar);
    // srclo * (256 - scaled_dsa)
    __m128i blendsrc = _mm_mullo_epi16(_256dsa, src);

    // tarlo * scaled_dsa + srclo * (256 - scaled_dsa)
    __m128i blenddst = _mm_add_epi16(blendtar, blendsrc); // [x|b|b|b|x|b|b|b]

    // (tarlo * scaled_dsa + srclo * (256 - scaled_dsa)) / 256
    __m128i dst = _mm_srli_epi16(blenddst, 8);

    // mask = 01110111
   return _mm_blend_epi16(ad, dst, 0x77); // [0A|0R|0G|0B|0A|0R|0G|0R]
}

// ============================================================
// Various Helper Functions
// ============================================================

static bool
AlignCheck(READONLY(byte*) address,
           unsigned by = SIMD_SIZE)
{
    // Check that the address given is a multiple of given value;
    // Treat address as 64-bit to keep x64 compatibility.

    return ((uintptr_t)address % by == 0);
}

static bool
AlignCheck(READONLY(byte*) a1, 
           READONLY(byte*) a2)
{
    return AlignCheck(a1) && AlignCheck(a2);
}

static bool
AlignCheck(READONLY(byte*) a1,
           READONLY(byte*) a2,
           READONLY(byte*) a3)
{
    return AlignCheck(a1) && AlignCheck(a2) && AlignCheck(a3);
}

static byte*
NextAlignedAddress(READWRITE(byte*) start,
                   unsigned by = SIMD_SIZE)
{
    // Finds the next adress that is a multiple of given value.
    // Note: the input and output addresses are writable because
    //       the caller might want to do some work on the data
    //       and needs write access

    byte* address = start;
    while (AlignCheck(address, by) == false)
    {
        ++address;
    }
    return address;
}
