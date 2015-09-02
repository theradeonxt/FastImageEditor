
#pragma once

// local include
#include "Common.h"

// system include
#include <stdint.h>
#include <emmintrin.h> // SSE2 intrinsics: this is the baseline
// ReSharper disable once CppUnusedIncludeDirective
#include <tmmintrin.h> // SSSE3 intrinsics

// ============================================================
// SIMD Extra Functions
// ============================================================

static __m128i
_mm_div255_epu16(__m128i x)
{
    // http://www.alfredklomp.com/programming/sse-intrinsics/
    // Divide 8 16-bit uints by 255:
    // x := ((x + 1) + (x >> 8)) >> 8:

    // TODO: generate a vector of constants without touching memory
    //       profile if this is any faster
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
    // x := x * (y / 255)
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
    // Limits the range of 4 packed floats between [min, max].
    // Notes: - use this when working with SIMD data, 
    //          because if the compiler inlines the call 
    //          the parameters will already be in CPU registers;
    //        - this will amount to just 2 instructions
    //          for clamping 4 values simultaneously.

    return _mm_min_ps(_mm_max_ps(value, min), max);
}

// ============================================================
// Various Helper Functions
// ============================================================

template<typename T> static bool
AlignCheck1(READONLY(T*) address, uint32_t by = SIMD_SIZE)
{
    // Check that the address given is a multiple of given value;
    // Treat address as 64-bit to keep x64 compatibility.

    return uintptr_t(address) % uintptr_t(by) == 0;
}

template<typename T> static bool
AlignCheck(READONLY(T*) a1, 
           READONLY(T*) a2 = nullptr, READONLY(T*) a3 = nullptr, 
           READONLY(T*) a4 = nullptr, READONLY(T*) a5 = nullptr)
{
    return AlignCheck1(a1) 
        && AlignCheck1(a2) && AlignCheck1(a3) 
        && AlignCheck1(a4) && AlignCheck1(a5);
}

template<typename T> static T*
NextAlignedAddress(READWRITE(T*) start, uint32_t by = SIMD_SIZE)
{
    // Finds the next adress that is a multiple of given value.
    // Note: the input and output addresses are writable because
    //       the caller might want to do some work on the data
    //       and needs write access

    T* address = start;
    while (AlignCheck1(address++, by) == false){}
    return address;
}
