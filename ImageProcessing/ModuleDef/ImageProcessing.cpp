
//! =============================================================================
//! \file 
//! \brief 
//!     Implementation of library modules
//!
//! This only aggregates all available codepaths for modules.
//!
//! Module Template(Structure):
//!   -> CPU Dispatcher (trigger the best available codepath)
//!   -> Special handling:
//!      -> Error check
//!      -> For SIMD operations data alignment is required;
//!         This means a need to use the reference codepath
//!         to process extra data at the end.
//! =============================================================================

#include "ImageProcessing.h"
#include "DispatchManager.h"
#include "Common.h"

#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

IMPROC_CDECL IMPROC_API int32_t
Blend24bgr_24bgr(READONLY (uint8_t*) source,
                 READONLY (uint8_t*) target,
                 READWRITE(uint8_t*) destination,
                 uint32_t            sizeBytes,
                 float               percentage)
{
    return Runner<PreferSIMD, PreferAlignment>(__FUNCTION__).RunFrom(
        0, sizeBytes, ADR_ARG(source), ADR_ARG(target), ADR_ARG(destination), SZ_ARG(sizeBytes), VAL_ARG(percentage));
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL IMPROC_API int32_t
Convert_32bgra_24hsv(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destinationHueChannel,
                     READWRITE(uint8_t*) destinationSaturationChannel,
                     READWRITE(uint8_t*) destinationValueChannel,
                     uint32_t            sizeBytes)
{
    return Runner<PreferSIMD, PreferAlignment>(__FUNCTION__).RunFrom(
        0, sizeBytes, ADR_ARG(source), ADR_ARG(destinationHueChannel), ADR_ARG(destinationSaturationChannel), 
        ADR_ARG(destinationValueChannel), SZ_ARG(sizeBytes));
}

IMPROC_CDECL IMPROC_API int32_t
OpacityAdjust_32bgra(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage)
{
    return Runner<PreferSIMD, PreferAlignment>(__FUNCTION__).RunFrom(
        0, sizeBytes, ADR_ARG(source), ADR_ARG(destination), SZ_ARG(sizeBytes), VAL_ARG(percentage));
}

IMPROC_CDECL IMPROC_API int32_t
AlphaBlend32bgra_32bgra(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes)
{
    return Runner<PreferSIMD, PreferAlignment>(__FUNCTION__).RunFrom(
        0, sizeBytes, ADR_ARG(source), ADR_ARG(target), ADR_ARG(destination), SZ_ARG(sizeBytes));
}

IMPROC_CDECL IMPROC_API int32_t
ConvFilter_32bgra(READONLY (uint8_t*) source,
                  READWRITE(uint8_t*) destination,
                  uint32_t            sizeBytes,
                  uint32_t            strideBytes,
                  READONLY (float*)   kernel,
                  uint32_t            width,
                  uint32_t            height)
{
    return Runner<PreferSIMD, PreferAlignment>(__FUNCTION__).RunFrom(
        0, sizeBytes, ADR_ARG(source), ADR_ARG(destination), SZ_ARG(sizeBytes), VAL_ARG(kernel), VAL_ARG(width), VAL_ARG(height));
}
