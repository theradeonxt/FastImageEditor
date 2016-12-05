//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_SSE41_H
#define IMPROC_SSE41_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL int32_t
ConvFilter_32bgra_SSE41(READONLY(uint8_t*)  source,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes,
                        uint32_t            strideBytes,
                        READONLY(float*)    kernel,
                        uint32_t            width,
                        uint32_t            height);

#endif /* IMPROC_SSE41_H */
