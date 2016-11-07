// SIMD intrinsics have a "one-header-to-rule-them-all" (compiler dependent) 
#ifdef _MSC_VER
#include <intrin.h>
#elif defined(__GNUC__)
#include <x86intrin.h>
#endif

// For MSVC compiler disable truncation of constant value warning
#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable: 4309) 
#endif

namespace Constants
{
    __m128i xmmBGRAMaskB = _mm_set1_epi32(0x000000ff);
    __m128i xmmBGRAMaskG = _mm_set1_epi32(0x0000ff00);
    __m128i xmmBGRAMaskR = _mm_set1_epi32(0x00ff0000);

    __m128 xmm0const = _mm_set1_ps(0.0f);
    __m128 xmm255const = _mm_set1_ps(255.0f);
    __m128 xmm43const = _mm_set1_ps(43.0f);
    __m128 xmm171const = _mm_set1_ps(171.0f);
    __m128 xmm85const = _mm_set1_ps(85.0f);
    __m128 xmm1const = _mm_set1_ps(1.0f);
    __m128 xmmRcp255const = _mm_set1_ps(1.0f / 255.0f);

    // Mask out alpha channel in 4 BGRA pixels
    __m128i alpha_mask = _mm_set_epi8(0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0, 0xff, 0, 0, 0);

    // Mask out color channels in 4 BGRA pixels
    __m128i alphaInvMask = _mm_set_epi8(0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF, 0, 0xFF, 0xFF, 0xFF);
    
    // Splicing masks: convert from low-8-bit parts of 4 x INT32 => 8 x INT16
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AL2|0|AL2|0|AL2|0|AL2|0|AL1|0|AL1|0|AL1|0AL1]
    // [0|0|0|AH2|0|0|0|AH1|0|0|0|AL2|0|0|0|AL1] => [0|AH2|0|AH2|0|AH2|0|AH2|0|AH1|0|AH1|0|AH1|0AH1]
    __m128i alpha_splice_lo_mask = _mm_set_epi8(0x80, 4, 0x80, 4, 0x80, 4, 0x80, 4, 0x80, 0, 0x80, 0, 0x80, 0, 0x80, 0);
    __m128i alpha_splice_hi_mask = _mm_set_epi8(0x80, 12, 0x80, 12, 0x80, 12, 0x80, 12, 0x80, 8, 0x80, 8, 0x80, 8, 0x80, 8);

    // Constant vectors: 8 x INT16 = {256}, 16  x INT8 = {255}
    __m128i one256_8x = _mm_set1_epi16(256);
    __m128i one255_16x = _mm_set1_epi8(255);
}
