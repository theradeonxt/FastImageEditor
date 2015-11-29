
#ifndef IMAGEPROCESSING_REFERENCE_H
#define IMAGEPROCESSING_REFERENCE_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_ref(READONLY (uint8_t*) source,
                     READONLY (uint8_t*) target,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage);

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_ref_MT(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes,
                        float               percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_ref(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage);

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_ref_MT(READONLY (uint8_t*) source,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes,
                            float               percentage);

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_ref(READONLY (uint8_t*) source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes);

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_ref_MT(READONLY (uint8_t*) source,
                               READONLY (uint8_t*) target,
                               READWRITE(uint8_t*) destination,
                               uint32_t            sizeBytes);

IMAGEPROCESSING_CDECL int32_t
ConvFilter_32bgra_ref(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height);

IMAGEPROCESSING_CDECL int32_t
ConvFilter_32bgra_ref_MT(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height);

#endif /* IMAGEPROCESSING_REFERENCE_H */
