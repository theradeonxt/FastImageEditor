
#ifndef IMAGEPROCESSING_PROFILETOOLS_H
#define IMAGEPROCESSING_PROFILETOOLS_H

//
// Output information to a Debug Console
// This is supplied by the environment: - standard logging output (GCC environment)
//                                      - platform/IDE dependent  (MSVC environment)
// Note: active in Release too.
//
#ifdef _MSC_VER /* MSVC compiler specific */

#define WIN32_LEAN_AND_MEAN
#define NOMINMAX
#include <Windows.h>
#include <string>

#define TRACE_MESSAGE(MESSAGE) OutputDebugStringW((std::wstring(MESSAGE) + L"\n").c_str());

#elif defined(__GNUC__) /* GCC compiler specific */

#include <string>
#include <iostream>

#define TRACE_MESSAGE(MESSAGE) std::wclog << MESSAGE << L"\n";

#else /* Sorry, environment not supported */

#define TRACE_MESSAGE(MESSAGE)

#endif /* _MSC_VER || __GNUC__ */

//
// Record elapsed CPU clock cycles between 2 timepoints
// 
// Note: active in Release too.
//
#ifdef PROFILE_DEBUGINFO

#include "CpuRdtsc.hpp"

#define PROFILE_MSG_PIXELS_PER_ITER L" - Cycles/4Px: "
#define REGISTER_TIMED_BLOCK(ID)                    \
    int64_t start = 0, end = 0, avg = 0, count = 0; \
    std::wstring blockID = L#ID;
#define BEGIN_TIMED_BLOCK() \
    start = rdtsc();  
#define END_TIMED_BLOCK() \
    end = rdtsc();        \
    avg += (end - start); \
    count++;
#define PROFILE_TRACE_BLOCK(MESSAGE) \
    TRACE_MESSAGE(blockID + std::wstring(MESSAGE) + std::to_wstring((count != 0) ? (avg / count) : avg))

#else /* !defined( PROFILE_DEBUGINFO )*/

#define PROFILE_MSG_PIXELS_PER_ITER ""
#define REGISTER_TIMED_BLOCK(ID)
#define BEGIN_TIMED_BLOCK()
#define END_TIMED_BLOCK()
#define PROFILE_TRACE_BLOCK(MESSAGE)

#endif /* defined( PROFILE_DEBUGINFO ) */

#endif /* IMAGEPROCESSING_PROFILETOOLS_H */
