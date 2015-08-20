
#pragma once

#include "Common.h"

// ===================================================
// Function declarations
// Note: these define the interface of the .dll module
//       we need to have them visible to the outside.
// ===================================================

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int
Blend24bgr_24bgr(READONLY(byte*) source,
                 READONLY(byte*) target,
                 READWRITE(byte*) destination,
                 unsigned sizeBytes,
                 float percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int
OpacityAdjust_32bgra(READONLY(byte*) source,
                     READWRITE(byte*) destination,
                     unsigned sizeBytes,
                     float percentage);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int
AlphaBlend32bgra_32bgra(READONLY(byte*) source,
                        READONLY(byte*) target,
                        READWRITE(byte*) destination,
                        unsigned sizeBytes);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API int
ConvFilter_32bgra(READONLY(byte*) source,
                  READWRITE(byte*) destination,
                  unsigned sizeBytes,
                  READONLY(float*) kernel,
                  unsigned width,
                  unsigned height);
