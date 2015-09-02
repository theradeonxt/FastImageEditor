
// local include
#include "ReferenceProcessing.h"
#include "Helpers.cpp"
#include "Common.h"

// system include
#include <stdint.h>
#include <memory>

// ====================================================
// 24bpp Image Operations
// ====================================================

int32_t
Blend24bgr_24bgr_ref(READONLY (uint8_t*) source,
                     READONLY (uint8_t*) target,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage)
{
    int32_t szb        = sizeBytes;
    int32_t ipercent   = int32_t(percentage * 255.0f);
    int32_t _1ipercent = int32_t((1.0f - percentage) * 255.0f);

#pragma omp parallel for
    for (int32_t index = 0; index < szb; index++)
    {
        int32_t src = int32_t(source[index]);
        int32_t tar = int32_t(target[index]);
        int32_t dst = (src * _1ipercent + tar * ipercent) / 255;
        destination[index] = uint8_t(dst);
    }

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

int32_t
OpacityAdjust_32bgra_ref(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage)
{
    int32_t  szb        = sizeBytes;
    uint32_t alphaLevel = uint32_t(percentage * 255.0f) << 24;

#pragma omp parallel for
    for (int32_t index = 0; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        uint32_t bgra = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t dst  = (bgra & 0x00ffffff) | alphaLevel;
        *reinterpret_cast<uint32_t*>(destination + index) = dst;
    }

    return OperationSuccess;
}

int32_t
AlphaBlend32bgra_32bgra_ref(READONLY (uint8_t*) source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes)
{
    int32_t szb = sizeBytes;

#pragma omp parallel for
    for (int32_t index = 0; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        // read source & target pixels and get their alpha values
        uint32_t sp = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t tp = *reinterpret_cast<const uint32_t*>(target + index);
        uint32_t sa = (sp >> 24);
        uint32_t ta = (tp >> 24);

        // compute resulting alpha value (simple lerp)
        uint32_t da = sa + ta * (255 - sa) / 255;
        if (da == 0)
        {
            *reinterpret_cast<uint32_t*>(destination + index) = 0;
            continue;
        }

        // precompute division factors
        uint32_t scaled_dsa = ((da - sa) * 256) / da;

        // blend each color value according to the alpha values in each pixel (optimized lerp)
        uint32_t dr = ((256 - scaled_dsa) * ((sp & 0x00FF0000) >> 16) + scaled_dsa * ((tp & 0x00FF0000) >> 16)) / 256;
        uint32_t dg = ((256 - scaled_dsa) * ((sp & 0x0000FF00) >> 8)  + scaled_dsa * ((tp & 0x0000FF00) >> 8) ) / 256;
        uint32_t db = ((256 - scaled_dsa) * ( sp & 0x000000FF)        + scaled_dsa * ( tp & 0x000000FF)       ) / 256; 

        // assemble the output pixel
        *reinterpret_cast<uint32_t*>(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);
    }

    // this was the original code, but unsigned divs (/da) can be replaced with /256 
    // which is optimized into a right shift; /da is done only once when precomputing factors.
    /*
    uint32_t da = sa + ta * (255 - sa) / 255;
    uint32_t dr = (sa * ((sp & 0x00FF0000) >> 16) + (da - sa) * ((tp & 0x00FF0000) >> 16)) / da;
    uint32_t dg = (sa * ((sp & 0x0000FF00) >> 8)  + (da - sa) * ((tp & 0x0000FF00) >> 8) ) / da;
    uint32_t db = (sa * ( sp & 0x000000FF)        + (da - sa) * ( tp & 0x000000FF)       ) / da;
    *(uint32_t*)(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);
    */

    return OperationSuccess;
}

int32_t
ConvFilter_32bgra_ref(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height)
{
    std::unique_ptr<const int32_t> offsetsVect(PixelOffsetsLookup(width, height, 4));
    if (offsetsVect.get() == nullptr)
    {
        return OutOfMemory;
    }

    READONLY(int32_t*) offsets = offsetsVect.get();
    uint32_t dimension = width * height;
    int32_t  szb       = sizeBytes;

    // BUG: this crashes for values near the edges of filter window
#pragma omp parallel for
    for (int32_t index = 0; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        float sumB(0.0f);
        float sumG(0.0f);
        float sumR(0.0f);
        float sumA(0.0f);
        for (uint32_t k = 0; k < dimension; k++)
        {
            // TODO: jumps in memory inside the filter window (offsets[k])
            // are hindering cache performance
            sumB += *(source + index + 0 + offsets[k]) * kernel[k];
            sumG += *(source + index + 1 + offsets[k]) * kernel[k];
            sumR += *(source + index + 2 + offsets[k]) * kernel[k];
            sumA += *(source + index + 3 + offsets[k]) * kernel[k];
        }
        *(destination + index + 0) = uint8_t(Clamp(sumB, 0, 255));
        *(destination + index + 1) = uint8_t(Clamp(sumG, 0, 255));
        *(destination + index + 2) = uint8_t(Clamp(sumR, 0, 255));
        *(destination + index + 3) = uint8_t(Clamp(sumA, 0, 255));
    }

    return OperationSuccess;
}
