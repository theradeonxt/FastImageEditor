//! =============================================================================
//! \file 
//! \brief 
//!     Image Processing Library Interface
//!
//! These functions define the interface of the library.
//! This is the list of exported symbols that can be accessed from external code.
//!
//! =============================================================================

#ifndef IMPROC_LIBINTERFACE_H
#define IMPROC_LIBINTERFACE_H

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
                 float               percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL IMPROC_API int32_t
Convert_32bgra_24hsv(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destinationHueChannel,
                     READWRITE(uint8_t*) destinationSaturationChannel,
                     READWRITE(uint8_t*) destinationValueChannel,
                     uint32_t            sizeBytes);

IMPROC_CDECL IMPROC_API int32_t
OpacityAdjust_32bgra(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage);

IMPROC_CDECL IMPROC_API int32_t
AlphaBlend32bgra_32bgra(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes);

IMPROC_CDECL IMPROC_API int32_t
ConvFilter_32bgra(READONLY (uint8_t*) source,
                  READWRITE(uint8_t*) destination,
                  uint32_t            sizeBytes,
                  uint32_t            strideBytes,
                  READONLY (float*)   kernel,
                  uint32_t            width,
                  uint32_t            height);

#endif /* IMPROC_LIBINTERFACE_H */
