
//
// Implementation for reading the CPU clock cycles
// by measuring consecutive hardware time stamp counters
//

#ifndef IMAGEPROCESSING_CPURDTSC_HPP
#define IMAGEPROCESSING_CPURDTSC_HPP

#ifdef _MSC_VER

#include <intrin.h>
#include <cstdint>

inline int64_t rdtsc()
{
    return __rdtsc();
}

#elif defined(__GNUC__)

// ===================================================
// See: http://www.mcs.anl.gov/~kazutomo/rdtsc.html
// ===================================================

#if defined(__i386__)
static __inline__ unsigned long long rdtsc(void)
{
    unsigned long long int x;
    __asm__ volatile (".byte 0x0f, 0x31" : "=A" (x));
    return x;
}

#elif defined(__x86_64__)
static __inline__ unsigned long long rdtsc(void)
{
    unsigned hi, lo;
    __asm__ __volatile__("rdtsc" : "=a"(lo), "=d"(hi));
    return ((unsigned long long)lo) | (((unsigned long long)hi) << 32);
}

#elif defined(__powerpc__)
static __inline__ unsigned long long rdtsc(void)
{
    unsigned long long int result = 0;
    unsigned long int upper, lower, tmp;
    __asm__ volatile(
            "0:                  \n"
            "\tmftbu   %0           \n"
            "\tmftb    %1           \n"
            "\tmftbu   %2           \n"
            "\tcmpw    %2,%0        \n"
            "\tbne     0b         \n"
            : "=r"(upper), "=r"(lower), "=r"(tmp)
    );
    result = upper;
    result = result << 32;
    result = result | lower;

    return(result);
}
#endif

#endif /* __GNUC__ */

#endif /* IMAGEPROCESSING_CPURDTSC_HPP */
