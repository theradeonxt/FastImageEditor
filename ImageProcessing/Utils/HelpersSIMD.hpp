//! =============================================================================
//! \file 
//! \brief 
//!     SIMD Extra Functions and Helpers
//!
//! These are short utility functions to extend frequently used
//! SIMD operations. Usually the compiler will inline these.
//!
//! =============================================================================

#ifndef IMPROC_HELPERSSIMD_HPP
#define IMPROC_HELPERSSIMD_HPP

#include "Common.h"
#include <cstdint>
#include <assert.h>

// SIMD intrinsics have a "one-header-to-rule-them-all" (compiler dependent) 
#ifdef _MSC_VER
#include <intrin.h>
#elif defined(__GNUC__)
#include <x86intrin.h>
#endif

//! =================================================================
//!
//! Utility Methods
//!
//! =================================================================

//! \brief
//! Performs a bitwise left shift logical by the specified count
//!
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

//! \brief
//! Performs a bitwise right shift logical by the specified count
//!
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

//! \brief
//! Divide 8 16-bit uints by 255:
//! x := ((x + 1) + (x >> 8)) >> 8:
//! See: http://www.alfredklomp.com/programming/sse-intrinsics/
//!
inline __m128i
_mm_div255_epu16(__m128i x)
{
    return _mm_srli_epi16(_mm_adds_epu16(
        _mm_adds_epu16(x, _mm_set1_epi16(1)),
        _mm_srli_epi16(x, 8)), 8);
}

//! \brief
//! Returns an "alpha blend" of x scaled by y/255;
//! x := x * (y / 255)
//! Reorder: x := (x * y) / 255
//! See: http://www.alfredklomp.com/programming/sse-intrinsics/
//!
inline __m128i
_mm_scale_epu8(__m128i x, __m128i y)
{
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

//! \brief
//! Limits the range of 4 packed floats between [min, max].
//! Notes: - use this when working with SIMD data, 
//!          because if the compiler inlines the call 
//!          the parameters will already be in CPU registers;
//!        - this will amount to just 2 instructions
//!          for clamping 4 values simultaneously.
//!
inline __m128
_mm_clamp_ps(__m128 value, __m128 min, __m128 max)
{
    return _mm_min_ps(_mm_max_ps(value, min), max);
}

inline __m128i
_mm_clamp_epi32(__m128i value, __m128i min, __m128i max)
{
    return _mm_min_epi32(_mm_max_epi32(value, min), max);
}

//! \brief
//! Check that the address given is a multiple of given value;
//! Treat address as 64-bit to keep x64 compatibility.
//! \param [in] address A readonly pointer address.
//! \param [in] by      A power of 2 value, by >= 2.
//! \returns True if it is, false if not.
//!
template<typename T> inline bool
SIMDAligned1(READONLY(T*) address, uint32_t by = SIMD_SIZE)
{
    assert(by >= 2 && (by % 2 == 0) && "Value must be a multiple of 2, >= 2");
    return (uintptr_t(address) & (by-1)) == 0;
}

//! \brief
//! Templated version of SIMDAligned1 with multiple arguments.
//!
template<typename T> inline bool
SIMDAligned(READONLY(T*) a1, 
            READONLY(T*) a2 = nullptr, READONLY(T*) a3 = nullptr, 
            READONLY(T*) a4 = nullptr, READONLY(T*) a5 = nullptr)
{
    return SIMDAligned1(a1) 
        && SIMDAligned1(a2) && SIMDAligned1(a3) 
        && SIMDAligned1(a4) && SIMDAligned1(a5);
}

//! \brief
//! Templated version of SIMDAligned1 with multiple arguments.
//! It checks for multiples of SIMD_SIZE_AVX.
//!
template<typename T> inline bool
SIMDAlignedAVX(READONLY(T*) a1,
               READONLY(T*) a2 = nullptr, READONLY(T*) a3 = nullptr,
               READONLY(T*) a4 = nullptr, READONLY(T*) a5 = nullptr)
{
    return SIMDAligned1(a1, SIMD_SIZE_AVX)
        && SIMDAligned1(a2, SIMD_SIZE_AVX) && SIMDAligned1(a3, SIMD_SIZE_AVX)
        && SIMDAligned1(a4, SIMD_SIZE_AVX) && SIMDAligned1(a5, SIMD_SIZE_AVX);
}

//! \brief
//! Finds the next adress that is a multiple of given value.
//! \param [in] address A readonly pointer address.
//! \param [in] by      A power of 2 value, by >= 2.
//! \remark The input and output addresses are writable because the caller
//!         might want to do some work on the data and needs write access.
//!
template<typename T> inline READWRITE(T*)
NextAlignedAddress(READWRITE(T*) start, uint32_t by = SIMD_SIZE)
{
    T* address = start;
    while (SIMDAligned1(address++, by) == false){ ; }
    return address;
}

//! =================================================================
//!
//! SIMD Constants
//!
//! =================================================================

namespace Constants
{
    extern __m128i xmmBGRAMaskB;
    extern __m128i xmmBGRAMaskG;
    extern __m128i xmmBGRAMaskR;

    extern __m128 xmm0const;
    extern __m128 xmm255const;
    extern __m128 xmm43const;
    extern __m128 xmm171const;
    extern __m128 xmm85const;
    extern __m128 xmmRcp255const;

    //!< Mask out alpha channel in 4 pixels
    extern __m128i alpha_mask;

    //!< Mask out color channels in 4 BGRA pixels
    extern __m128i alphaInvMask;

    //!< Splicing masks: convert from low-8-bit parts of 4 x INT32 => 8 x INT16
    extern __m128i alpha_splice_lo_mask;
    extern __m128i alpha_splice_hi_mask;

    //!< Constant vectors: 8 x INT16 = {256}, 16  x INT8 = {255}
    extern __m128i one256_8x;
    extern __m128i one255_16x;
}

#endif /* IMPROC_HELPERSSIMD_HPP */
