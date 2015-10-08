
#pragma once

// Use C-style functions
#if defined __cplusplus
    #define IMAGEPROCESSING_CDECL extern "C"
#else
    #define IMAGEPROCESSING_CDECL
#endif // __cplusplus

// Dll exporting/importing
#if defined(IMAGEPROCESSING_EXPORTS) && defined(_MSC_VER)
    #define IMAGEPROCESSING_API __declspec(dllexport)
#else
    #define IMAGEPROCESSING_API __declspec(dllimport)
#endif // IMAGEPROCESSING_EXPORTS
#ifndef IMAGEPROCESSING_API
    #define IMAGEPROCESSING_API
#endif

// Enable/Disable reference implementation
#define FORCE_SCALAR_IMPL
#define REFERENCE_IMPL(FUNCTION_NAME, ...) FUNCTION_NAME##_ref(__VA_ARGS__)
#ifdef FORCE_SCALAR_IMPL
    #define WANT_REFERENCE_IMPL(FUNCTION_NAME, ...) { return FUNCTION_NAME##_ref(__VA_ARGS__); }
#else
    #define WANT_REFERENCE_IMPL(FUNCTION_NAME, ...)
#endif // FORCE_SCALAR_IMPL

// ====================================================================
// Constants Definitions
// ====================================================================

static const auto SIMD_SIZE = 16i32;

#define READONLY(PTR_TYPE) const PTR_TYPE const // CAN'T modify pointer and data
#define READWRITE(PTR_TYPE) PTR_TYPE const      // CAN'T modify pointer but CAN modify data

// Error Codes
enum ErrorValues
{
    OutOfMemory = -1,
    OperationSuccess = 0,
};
