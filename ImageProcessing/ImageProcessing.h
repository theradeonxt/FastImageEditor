
#pragma once

// local include
#include "Common.h"

// system include
#include <stdint.h>

// ====================================================
// Note: these define the interface of the .dll module
//       we need to have them visible to the outside.
// ====================================================

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
Blend24bgr_24bgr(READONLY (uint8_t*) source,
                 READONLY (uint8_t*) target,
                 READWRITE(uint8_t*) destination,
                 uint32_t            sizeBytes,
                 float               percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
OpacityAdjust_32bgra(READONLY (uint8_t*) source,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
AlphaBlend32bgra_32bgra(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int32_t
ConvFilter_32bgra(READONLY (uint8_t*) source,
                  READWRITE(uint8_t*) destination,
                  uint32_t            sizeBytes,
                  READONLY (float*)   kernel,
                  uint32_t            width,
                  uint32_t            height);
