//! =============================================================================
//! \file 
//! \brief 
//!     Module Interface
//!
//! Exported helper functions from the library
//! These are provided to enable users to tune different settings depending on
//! their usecase.
//!
//! =============================================================================

#ifndef IMPROC_MODULE_INTERFACE_H
#define IMPROC_MODULE_INTERFACE_H

#include "Common.h"
#include <cstdint>

IMPROC_CDECL IMPROC_API
extern int32_t SetMultiThreadingStatus(READONLY(char*) moduleName, int32_t status);

IMPROC_CDECL IMPROC_API
extern int32_t GetMultiThreadingStatus(READONLY(char*) moduleName);

IMPROC_CDECL IMPROC_API
extern int32_t QueryAvailableImplementation(READONLY(char*) moduleName, int32_t level);

IMPROC_CDECL IMPROC_API
extern int32_t SetImplementationLevel(READONLY(char*) moduleName, int32_t level);

IMPROC_CDECL IMPROC_API
extern int32_t GetImplementationLevel(READONLY(char*) moduleName);

IMPROC_CDECL IMPROC_API
extern int32_t MEMCMP(READONLY(void*) src, READONLY(void*) tar, int32_t sizeBytes);

#endif /* IMPROC_MODULE_INTERFACE_H */
