
#pragma once

// local include
#include "Common.h"

// system include
#include <stdint.h>
#include <emmintrin.h> // SSE2 intrinsics: this is the baseline
#include <tmmintrin.h> // SSSE3 intrinsics

// ============================================================
// SIMD Extra Functions
// ============================================================

static inline __m128i
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

static inline __m128i
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

static inline __m128
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

// ============================================================
// Various Helper Functions
// ============================================================

static inline bool
AlignCheck(READONLY(uint8_t*) address,
           uint32_t by = SIMD_SIZE)
{
    // Check that the address given is a multiple of given value;
    // Treat address as 64-bit to keep x64 compatibility.

    return ((uintptr_t)address % by == 0);
}

static inline bool
AlignCheck(READONLY(uint8_t*) a1, 
           READONLY(uint8_t*) a2)
{
    return AlignCheck(a1) && AlignCheck(a2);
}

static inline bool
AlignCheck(READONLY(uint8_t*) a1,
           READONLY(uint8_t*) a2,
           READONLY(uint8_t*) a3)
{
    return AlignCheck(a1) && AlignCheck(a2) && AlignCheck(a3);
}

static inline uint8_t*
NextAlignedAddress(READWRITE(uint8_t*) start,
                   uint32_t by = SIMD_SIZE)
{
    // Finds the next adress that is a multiple of given value.
    // Note: the input and output addresses are writable because
    //       the caller might want to do some work on the data
    //       and needs write access

    uint8_t* address = start;
    while (AlignCheck(address, by) == false)
    {
        ++address;
    }
    return address;
}
