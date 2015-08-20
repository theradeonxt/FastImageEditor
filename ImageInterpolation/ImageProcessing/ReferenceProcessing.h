
#pragma once

#include "Common.h"

// =============================================================
// Function declarations
// Note: these are reference implementations that target
//       corectness before efficiency and are used as 
//       a fallback for the functions defined by the interface. 
// =============================================================

int
Blend24bgr_24bgr_ref(READONLY(byte*) source,
                     READONLY(byte*) target,
                     READWRITE(byte*) destination,
                     unsigned sizeBytes,
                     float percentage);

int
OpacityAdjust_32bgra_ref(READONLY(byte*) source,
                         READWRITE(byte*) destination,
                         unsigned sizeBytes,
                         float percentage);

int
AlphaBlend32bgra_32bgra_ref(READONLY(byte*) source,
                            READONLY(byte*) target,
                            READWRITE(byte*) destination,
                            unsigned sizeBytes);

int
ConvFilter_32bgra_ref(READONLY(byte*) source,
                      READWRITE(byte*) destination,
                      unsigned sizeBytes,
                      READONLY(float*) kernel,
                      unsigned width,
                      unsigned height);
