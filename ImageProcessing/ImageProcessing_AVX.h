
#ifndef IMAGEPROCESSING_AVX_H
#define IMAGEPROCESSING_AVX_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

#define Blend24bgr_24bgr_AVX(...) NotAvailable
#define Blend24bgr_24bgr_AVX_MT(...) NotAvailable

// ====================================================
// 32bpp Image Operations
// ====================================================

#define OpacityAdjust_32bgra_AVX(...) NotAvailable
#define OpacityAdjust_32bgra_AVX_MT(...) NotAvailable

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_AVX(READONLY(uint8_t*)  source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes);

#define AlphaBlend32bgra_32bgra_AVX_MT(...) NotAvailable

#define ConvFilter_32bgra_AVX(...) NotAvailable
#define ConvFilter_32bgra_AVX_MT(...) NotAvailable

#endif /* IMAGEPROCESSING_AVX_H */
