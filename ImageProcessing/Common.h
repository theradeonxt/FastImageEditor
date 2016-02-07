
// ===================================================
// Public Global Config File
// ===================================================

#ifndef IMAGEPROCESSING_COMMON_H
#define IMAGEPROCESSING_COMMON_H

//
// C-Style Calling Convention
//
#ifdef __cplusplus
#define IMAGEPROCESSING_CDECL extern "C"
#else
#define IMAGEPROCESSING_CDECL
#endif /* __cplusplus */

//
// DLL exporting/importing
//
#ifdef _MSC_VER /* MSVC compiler specific */

#ifdef IMAGEPROCESSING_EXPORTS
#define IMAGEPROCESSING_API __declspec(dllexport)
#else
#define IMAGEPROCESSING_API __declspec(dllimport)
#endif /* IMAGEPROCESSING_EXPORTS */

#elif defined(__GNUC__) /* GCC compiler specific */

#define IMAGEPROCESSING_API __attribute__ ((visibility("default"))) 

#endif /* _MSC_VER || __GNUC__ */

/* Other environments get the fallback version */
#ifndef IMAGEPROCESSING_API
#define IMAGEPROCESSING_API
#endif

//
// Return Codes signaling the completion status of an operation
//
enum StatusCode
{
    OperationFailed = -64,  /* Generic failure */
    NotAvailable,           /* Module was explicitly invoked with an invalid config
                               Possible reasons: 1. No implementation available.
                                                 2. No hardware support available. */
    OutOfMemory,            /* Failure because the operation required additional
                               memory which could not be allocated */
    OperationSuccess = 0    /* Everything is ok */
};

//
// Denotes the SIMD level used to optimize a module.
// Higher values imply availability of previous ones (ex: FMA3 implies AVX).
//
enum SIMDLevel
{
    None,       /* Standard C++, x86 integer and x87 FPU code */
    SSE2,       /* Optimized for Streaming SIMD Extensions 2 */
    SSSE3,      /* Optimized for Supplemental Streaming SIMD Extensions 3 */
    AVX,        /* Full AVX-1 support - includes VEX prefix instructions; may not include 256-bit */
    FMA3        /* Fused-multiply-add with three-operand instructions */
};

//
// Constants used Project-wide
//
// Provide guarantees on input data (useful for const corectness)
#define READONLY(PTR_TYPE) const PTR_TYPE const
#define READWRITE(PTR_TYPE) PTR_TYPE const

static const int SIMD_SIZE       = 16;  /* Default SSE chunk size (bytes processed in one step) */
static const int SIMD_SIZE_AVX   = 32;  /* Default AVX chunk size (bytes processed in one step) */
static const int CPU_FLAGS       = 5;   /* How many CPU features we are interested to know */

//
// Enable/Disable reference implementation (Scalar)
//
#define FORCE_REFERENCE_IMPL

//
// Parallelize computations
//
//#define PARALLEL_COMPUTE

//
// Activate precise CPU clock cycle measurement for every module
// Note: active in Release too.
//
//#define PROFILE_DEBUGINFO

#endif /* IMAGEPROCESSING_COMMON_H */
