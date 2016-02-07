
// =========================================================
// Implementation of lbirary modules
//
// This only aggregates all available codepaths for modules.
//
// Module Template(Structure):
//   -> CPU Dispatcher (trigger the best available codepath)
//   -> Special handling:
//      -> Error check
//      -> For SIMD operations data alignment is required;
//         This means a need to use the reference codepath
//         to process extra data either at the begin or end.
// =========================================================

#include "ImageProcessing.h"
#include "GlobalConfig.h"
#include "SingleConfig.h"
#include "Common.h"
#include "CpuDispatcher.h"
#include "ReferenceProcessing.h"
#include "ImageProcessing_SSE2.h"
#include "ImageProcessing_SSSE3.h"
#include "ImageProcessing_FMA3.h"
#include "ImageProcessing_AVX.h"

#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Blend24bgr_24bgr(READONLY (uint8_t*) source,
                 READONLY (uint8_t*) target,
                 READWRITE(uint8_t*) destination,
                 uint32_t            sizeBytes,
                 float               percentage)
{
    CPU_DISPATCH_LOGIC(Blend24bgr_24bgr, source, target, destination, sizeBytes, percentage);

    if (READ_DISPATCH_RESULT() == OperationSuccess)
    {
        // Handle non-multiple of SIMD size images using reference implementation
        if (uint32_t mod = sizeBytes % SIMD_SIZE) // if this is not 0
        {
            return REFERENCE_IMPL(Blend24bgr_24bgr,
                source + (sizeBytes - mod), target + (sizeBytes - mod), destination + (sizeBytes - mod), mod, percentage);
        }
        return OperationSuccess;
    }
    return READ_DISPATCH_RESULT();
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Convert_32bgra_24hsv(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destinationHueChannel,
                     READWRITE(uint8_t*) destinationSaturationChannel,
                     READWRITE(uint8_t*) destinationValueChannel,
                     uint32_t            sizeBytes)
{
    CPU_DISPATCH_LOGIC(Convert_32bgra_24hsv, source, destinationHueChannel, destinationSaturationChannel, destinationValueChannel, sizeBytes);

    if (READ_DISPATCH_RESULT() == OperationSuccess)
    {
        // Handle non-multiple of SIMD size images using reference implementation
        if (uint32_t mod = sizeBytes % SIMD_SIZE) // if this is not 0
        {
            return REFERENCE_IMPL(Convert_32bgra_24hsv,
                source + (sizeBytes - mod), destinationHueChannel + (sizeBytes - mod), destinationSaturationChannel + (sizeBytes - mod), destinationValueChannel + (sizeBytes - mod), mod);
        }
        return OperationSuccess;
    }
    return READ_DISPATCH_RESULT();
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
OpacityAdjust_32bgra(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage)
{
    CPU_DISPATCH_LOGIC(OpacityAdjust_32bgra, source, destination, sizeBytes, percentage);

    if (READ_DISPATCH_RESULT() == OperationSuccess)
    {
        // Handle non-multiple of SIMD size images using reference implementation
        if (uint32_t mod = sizeBytes % SIMD_SIZE) // if this is not 0
        {
            return REFERENCE_IMPL(OpacityAdjust_32bgra,
                source + (sizeBytes - mod), destination + (sizeBytes - mod), mod, percentage);
        }
        return OperationSuccess;
    }
    return READ_DISPATCH_RESULT();
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
AlphaBlend32bgra_32bgra(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes)
{
    CPU_DISPATCH_LOGIC(AlphaBlend32bgra_32bgra, source, target, destination, sizeBytes);

    // Handle non-multiple of SIMD size images using reference implementation
    if (uint32_t mod = sizeBytes % SIMD_SIZE) // if this is not 0
    {
        return REFERENCE_IMPL(AlphaBlend32bgra_32bgra,
            source + (sizeBytes - mod), target + (sizeBytes - mod), destination + (sizeBytes - mod), mod);
    }
    return READ_DISPATCH_RESULT();
}

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
ConvFilter_32bgra(READONLY (uint8_t*) source,
                  READWRITE(uint8_t*) destination,
                  uint32_t            sizeBytes,
                  READONLY (float*)   kernel,
                  uint32_t            width,
                  uint32_t            height)
{
    CPU_DISPATCH_LOGIC(ConvFilter_32bgra, source, destination, sizeBytes, kernel, width, height);

    // Handle non-multiple of SIMD size images using reference implementation
    if (uint32_t mod = sizeBytes % SIMD_SIZE) // if this is not 0
    {
        return REFERENCE_IMPL(ConvFilter_32bgra,
            source + (sizeBytes - mod), destination + (sizeBytes - mod), mod, kernel, width, height);
    }
    return READ_DISPATCH_RESULT();
}
