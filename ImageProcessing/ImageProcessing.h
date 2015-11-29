
// =============================================================================
//              Image Processing Library Interface
//
// These functions define the interface of the library.
// This is the list of exported symbols that can be accessed from external code.
//
// =============================================================================

#ifndef IMAGEPROCESSINGAPI_H
#define IMAGEPROCESSINGAPI_H

#include "Common.h"
#include <cstdint>

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

#endif /* IMAGEPROCESSINGAPI_H */
