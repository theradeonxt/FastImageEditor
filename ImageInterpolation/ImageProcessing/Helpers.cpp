
#pragma once

// local include
#include "Common.h"

// system include
#include <new>

READONLY(int*)
PixelOffsetsLookup(unsigned width,
                   unsigned height,
                   unsigned pixelSize)
{
    // Calculate offsets from center of kernel to outside. 
    // This is used to jump from the current pixel to 
    // the ones on the edges of the kernel.

    unsigned midpointX = width / 2;
    unsigned midpointY = height / 2;
    unsigned byteMid = (midpointY * pixelSize + midpointX * pixelSize);

    int* offsets = new (std::nothrow) int[width * height];

    if (offsets)
    {
        for (unsigned i = 0; i < height; i++)
        for (unsigned j = 0; j < width; j++)
        {
            // subtract from current byte location the midpoint byte location
            unsigned byte = (i * pixelSize + j * pixelSize);
            offsets[i*width + j] = byte - byteMid;
        }
    }

    return offsets;
}

float 
Clamp(float value, float min, float max)
{
    // Limit the range of input value between [min, max].
    // This implementation is a good tradeoff and let the compiler
    // try to optimize it into conditional moves or intrinsics.
    // http://stackoverflow.com/a/16659263

    const float t = value < min ? min : value;
    return t > max ? max : t;
}
