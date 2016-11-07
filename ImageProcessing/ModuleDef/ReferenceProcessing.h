//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_REFERENCE_H
#define IMPROC_REFERENCE_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

IMPROC_CDECL int32_t
Blend24bgr_24bgr_ref(READONLY (uint8_t*) source,
                     READONLY (uint8_t*) target,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage);

IMPROC_CDECL int32_t
Blend24bgr_24bgr_ref_MT(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes,
                        float               percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL IMPROC_API int32_t
Convert_32bgra_24hsv_ref(READONLY(uint8_t*)  source,
                         READWRITE(uint8_t*) destinationHueChannel,
                         READWRITE(uint8_t*) destinationSaturationChannel,
                         READWRITE(uint8_t*) destinationValueChannel,
                         uint32_t            sizeBytes);

IMPROC_CDECL IMPROC_API int32_t
Convert_32bgra_24hsv_ref_MT(READONLY(uint8_t*)  source,
                            READWRITE(uint8_t*) destinationHueChannel,
                            READWRITE(uint8_t*) destinationSaturationChannel,
                            READWRITE(uint8_t*) destinationValueChannel,
                            uint32_t            sizeBytes);

IMPROC_CDECL int32_t
OpacityAdjust_32bgra_ref(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage);

IMPROC_CDECL int32_t
OpacityAdjust_32bgra_ref_MT(READONLY (uint8_t*) source,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes,
                            float               percentage);

IMPROC_CDECL int32_t
AlphaBlend32bgra_32bgra_ref(READONLY (uint8_t*) source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes);

IMPROC_CDECL int32_t
AlphaBlend32bgra_32bgra_ref_MT(READONLY (uint8_t*) source,
                               READONLY (uint8_t*) target,
                               READWRITE(uint8_t*) destination,
                               uint32_t            sizeBytes);

IMPROC_CDECL int32_t
ConvFilter_32bgra_ref(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      uint32_t            strideBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height);

IMPROC_CDECL int32_t
ConvFilter_32bgra_ref_MT(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         uint32_t            strideBytes,
                         READONLY (float*)   kernel,
                         uint32_t            width,
                         uint32_t            height);

#endif /* IMPROC_REFERENCE_H */
