
#pragma once

// Use C-style functions
#if defined __cplusplus
    #define IMAGEPROCESSING_CDECL extern "C"
#else
    #define IMAGEPROCESSING_CDECL
#endif // __cplusplus

// Dll exporting/importing
#ifdef IMAGEPROCESSING_EXPORTS
    #define IMAGEPROCESSING_API __declspec(dllexport)
#else
    #define IMAGEPROCESSING_API __declspec(dllimport)
#endif // IMAGEPROCESSING_EXPORTS

// ====================================================================
// Constants Definitions
// ====================================================================

// SIMD Settings
#include <stdint.h>
static const int32_t SIMD_SIZE = 16;

#define READONLY(PTR_TYPE) const PTR_TYPE const // CAN'T modify pointer and data
#define READWRITE(PTR_TYPE) PTR_TYPE const      // CAN'T modify pointer but CAN modify data

// Error Codes
enum ErrorValues
{
    OutOfMemory = -1,
    OperationSuccess = 0,
};
