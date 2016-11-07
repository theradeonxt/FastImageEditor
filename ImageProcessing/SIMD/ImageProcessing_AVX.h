//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_AVX_H
#define IMPROC_AVX_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL int32_t
AlphaBlend32bgra_32bgra_AVX(READONLY(uint8_t*)  source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes);

#endif /* IMPROC_AVX_H */
