
// ============================================================
//          SIMD Extra Functions and Helpers
//
// These are short utility functions to extend frequently used
// SIMD operations. Usually the compiler will inline these.
//
// ============================================================

#ifndef IMAGEPROCESSINGAPI_HELPERSSIMD_HPP
#define IMAGEPROCESSINGAPI_HELPERSSIMD_HPP

#include "Common.h"
#include <cstdint>

// SIMD intrinsics have a "one-header-to-rule-them-all" (compiler dependent) 
#ifdef _MSC_VER
#include <intrin.h>
#elif defined(__GNUC__)
#include <x86intrin.h>
#endif

inline __m256i slli(__m256i arg, int count)
{
    __m128i arg_low = _mm256_castsi256_si128(arg);
    __m128i arg_hi = _mm256_extractf128_si256(arg, 1);

    __m128i newlow = _mm_slli_epi32(arg_low, count);
    __m128i newhi = _mm_slli_epi32(arg_hi, count);

    __m256i result = _mm256_castsi128_si256(newlow);
    result = _mm256_insertf128_si256(result, newhi, 1);
    return result;
}

inline __m256i srli(__m256i arg, int count)
{
    __m128i arg_low = _mm256_castsi256_si128(arg);
    __m128i arg_hi = _mm256_extractf128_si256(arg, 1);

    __m128i newlow = _mm_srli_epi32(arg_low, count);
    __m128i newhi = _mm_srli_epi32(arg_hi, count);

    __m256i result = _mm256_castsi128_si256(newlow);
    result = _mm256_insertf128_si256(result, newhi, 1);
    return result;
}

inline __m128i
_mm_div255_epu16(__m128i x)
{
    // http://www.alfredklomp.com/programming/sse-intrinsics/
    // Divide 8 16-bit uints by 255:
    // x := ((x + 1) + (x >> 8)) >> 8:

    return _mm_srli_epi16(_mm_adds_epu16(
        _mm_adds_epu16(x, _mm_set1_epi16(1)),
        _mm_srli_epi16(x, 8)), 8);
}

inline __m128i
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

inline __m128
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

template<typename T> inline bool
SIMDAligned1(READONLY(T*) address, uint32_t by = SIMD_SIZE)
{
    // Check that the address given is a multiple of given value;
    // Treat address as 64-bit to keep x64 compatibility.

    return uintptr_t(address) % uintptr_t(by) == 0;
}

template<typename T> inline bool
SIMDAligned(READONLY(T*) a1, 
           READONLY(T*) a2 = nullptr, READONLY(T*) a3 = nullptr, 
           READONLY(T*) a4 = nullptr, READONLY(T*) a5 = nullptr)
{
    return SIMDAligned1(a1) 
        && SIMDAligned1(a2) && SIMDAligned1(a3) 
        && SIMDAligned1(a4) && SIMDAligned1(a5);
}

template<typename T> inline bool
SIMDAlignedAVX(READONLY(T*) a1,
               READONLY(T*) a2 = nullptr, READONLY(T*) a3 = nullptr,
               READONLY(T*) a4 = nullptr, READONLY(T*) a5 = nullptr)
{
    return SIMDAligned1(a1, SIMD_SIZE_AVX)
        && SIMDAligned1(a2, SIMD_SIZE_AVX) && SIMDAligned1(a3, SIMD_SIZE_AVX)
        && SIMDAligned1(a4, SIMD_SIZE_AVX) && SIMDAligned1(a5, SIMD_SIZE_AVX);
}

template<typename T> inline READWRITE(T*)
NextAlignedAddress(READWRITE(T*) start, uint32_t by = SIMD_SIZE)
{
    // Finds the next adress that is a multiple of given value.
    // Note: the input and output addresses are writable because the caller
    //        might want to do some work on the data and needs write access

    T* address = start;
    while (SIMDAligned1(address++, by) == false){ ; }
    return address;
}

#endif /* IMAGEPROCESSINGAPI_HELPERSSIMD_HPP */
