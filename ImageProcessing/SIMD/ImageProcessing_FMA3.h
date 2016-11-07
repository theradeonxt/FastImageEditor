//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_FMA3_H
#define IMPROC_FMA3_H

#include "Common.h"
#include <cstdint>

// ====================================================
// 32bpp Image Operations
// ====================================================

IMPROC_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3(READONLY (uint8_t*) source,
                             READONLY (uint8_t*) target,
                             READWRITE(uint8_t*) destination,
                             uint32_t            sizeBytes);

IMPROC_CDECL int32_t
AlphaBlend32bgra_32bgra_FMA3_MT(READONLY (uint8_t*) source,
                                READONLY (uint8_t*) target,
                                READWRITE(uint8_t*) destination,
                                uint32_t            sizeBytes);

#endif /* IMPROC_FMA3_H */
