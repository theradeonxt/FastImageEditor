
#pragma once

// ======= Enable/Disable reference implementation for the library function ======= //

//#define FORCE_SCALAR_IMPL

#define REFERENCE_IMPL(FUNCTION_NAME, ...) FUNCTION_NAME##_ref(__VA_ARGS__)
#ifdef FORCE_SCALAR_IMPL
    #define WANT_SCALAR_IMPL(FUNCTION_NAME, ...) { return FUNCTION_NAME##_ref(__VA_ARGS__); }
#else
    #define WANT_SCALAR_IMPL(FUNCTION_NAME, ...)
#endif // FORCE_SCALAR_IMPL

// ======= Enable/disable auto-parallelization support ======= //

//#define DISABLE_MULTITHREAD

#ifdef DISABLE_MULTITHREAD
    #define PARALLELFOR(PARALLELISM) for
#else
    #ifdef _MSC_VER // MT pragmas specific to VC++
        #define PARALLELFOR(PARALLELISM)                \
            __pragma(loop(ivdep))                       \
            __pragma(loop(hint_parallel(PARALLELISM)))  \
            for
    #else
        #define PARALLELFOR(PARALLELISM) for
    #endif // _MSC_VER
#endif // DISABLE_MULTITHREAD
