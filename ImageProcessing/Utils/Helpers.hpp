//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMAGEPROCESSINGAPI_HELPERS_HPP
#define IMAGEPROCESSINGAPI_HELPERS_HPP

#include <new>
#include <cstdint>
#include <cassert>

//! \brief
//! Calculate offsets from center of kernel to outside. 
//! This is used to jump from the current pixel to 
//! the ones on the edges of the kernel.
//!
inline int32_t*
PixelOffsetsLookup(uint32_t width, uint32_t height, uint32_t pixelSize, uint32_t strideBytes)
{

    int32_t* offsets = new (std::nothrow) int32_t[width * height];
    if (offsets == nullptr)
    {
        return nullptr;
    }

    for (uint32_t i = 0; i < height; i++)
    {
        for (uint32_t j = 0; j < width; j++)
        {
            offsets[i*height+j] = (i - height / 2) * strideBytes + (i - width / 2) * pixelSize;
        }
    }

    return offsets;
}

//! \brief
//! Limit the range of input value between [min, max].
//! This implementation is a good tradeoff and let the compiler
//! try to optimize it into conditional moves or intrinsics.
//! http://stackoverflow.com/a/16659263
//!
inline float 
Clamp(float value, float min, float max)
{
    const float t = (value < min) ? min : value;
    return (t > max) ? max : t;
}

struct RgbColor
{
    uint8_t r, g, b;
    uint8_t pad;
};
struct HsvColor
{
    uint8_t h, s, v;
};

inline HsvColor 
RgbToHsv(RgbColor rgb)
{
    HsvColor hsv;
    uint8_t rgbMin, rgbMax;

    // min(r, g, b)
    rgbMin = rgb.r < rgb.g ? (rgb.r < rgb.b ? rgb.r : rgb.b) : (rgb.g < rgb.b ? rgb.g : rgb.b);
    // max(r, g, b)
    rgbMax = rgb.r > rgb.g ? (rgb.r > rgb.b ? rgb.r : rgb.b) : (rgb.g > rgb.b ? rgb.g : rgb.b);

    hsv.v = rgbMax;
    if (hsv.v == 0)
    {
        hsv.h = 0;
        hsv.s = 0;
        return hsv;
    }

    uint8_t rgbDiff = rgbMax - rgbMin;
    hsv.s = 255 * long(rgbDiff) / hsv.v;
    if (hsv.s == 0)
    {
        hsv.h = 0;
        return hsv;
    }

    if (rgbMax == rgb.r)
        hsv.h = 0 + 43 * (rgb.g - rgb.b) / (rgbDiff);
    else if (rgbMax == rgb.g)
        hsv.h = 85 + 43 * (rgb.b - rgb.r) / (rgbDiff);
    else
        hsv.h = 171 + 43 * (rgb.r - rgb.g) / (rgbDiff);

    return hsv;
}

#endif /* IMAGEPROCESSINGAPI_HELPERS_HPP */
