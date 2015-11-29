
#ifndef IMAGEPROCESSING_FMA3_H
#define IMAGEPROCESSING_FMA3_H

#include "Common.h"
#include <cstdint>

#define Blend24bgr_24bgr_FMA3(...) NotAvailable
#define Blend24bgr_24bgr_FMA3_MT(...) NotAvailable

// ====================================================
// 32bpp Image Operations
// ====================================================

#define OpacityAdjust_32bgra_FMA3(...) NotAvailable
#define OpacityAdjust_32bgra_FMA3_MT(...) NotAvailable

#define ConvFilter_32bgra_FMA3(...) NotAvailable
#define ConvFilter_32bgra_FMA3_MT(...) NotAvailable

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3(READONLY (uint8_t*) source,
                             READONLY (uint8_t*) target,
                             READWRITE(uint8_t*) destination,
                             uint32_t            sizeBytes);

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3_MT(READONLY (uint8_t*) source,
                                READONLY (uint8_t*) target,
                                READWRITE(uint8_t*) destination,
                                uint32_t            sizeBytes);

#endif /* IMAGEPROCESSING_FMA3_H */
