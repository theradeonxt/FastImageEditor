
#ifndef IMAGEPROCESSINGAPI_HELPERS_HPP
#define IMAGEPROCESSINGAPI_HELPERS_HPP

#include <new>
#include <cstdint>
#include <cassert>

static int32_t*
PixelOffsetsLookup(uint32_t width, uint32_t height, uint32_t pixelSize)
{
    // Calculate offsets from center of kernel to outside. 
    // This is used to jump from the current pixel to 
    // the ones on the edges of the kernel.

    int32_t* offsets = new (std::nothrow) int32_t[width * height];
    if (offsets == nullptr)
    {
        return nullptr;
    }

    uint32_t midpointX = width / 2;
    uint32_t midpointY = height / 2;
    uint32_t byteMid   = (midpointY * pixelSize + midpointX * pixelSize);
    
    for (uint32_t i = 0; i < height; i++)
    {
        for (uint32_t j = 0; j < width; j++)
        {
            // subtract from current byte location the midpoint byte location
            uint32_t byteNow = (i * pixelSize + j * pixelSize);
            assert(byteNow - byteMid >= 0);
            offsets[i*width + j] = byteNow - byteMid;
        }
    }

    return offsets;
}

inline float 
Clamp(float value, float min, float max)
{
    // Limit the range of input value between [min, max].
    // This implementation is a good tradeoff and let the compiler
    // try to optimize it into conditional moves or intrinsics.
    // http://stackoverflow.com/a/16659263

    const float t = (value < min) ? min : value;
    return (t > max) ? max : t;
}

#endif /* IMAGEPROCESSINGAPI_HELPERS_HPP */