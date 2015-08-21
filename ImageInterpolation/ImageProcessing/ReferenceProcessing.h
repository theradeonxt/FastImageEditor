
#pragma once

// local include
#include "Common.h"

// system include
#include <stdint.h>

// =============================================================
// Note: these are reference implementations that are used as 
//       a fallback for the functions defined by the interface. 
// =============================================================

// ====================================================
// 24bpp Image Operations
// ====================================================

int32_t
Blend24bgr_24bgr_ref(READONLY (uint8_t*) source,
                     READONLY (uint8_t*) target,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage);

// ====================================================
// 32bpp Image Operations
// ====================================================

int32_t
OpacityAdjust_32bgra_ref(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage);

int32_t
AlphaBlend32bgra_32bgra_ref(READONLY (uint8_t*) source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes);

int32_t
ConvFilter_32bgra_ref(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height);
