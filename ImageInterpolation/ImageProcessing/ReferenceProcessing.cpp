
#include "ReferenceProcessing.h"
#include "Helpers.cpp"
#include "Common.h"
#include "CommonPrivate.h"

#include <stdint.h>

// ====================================================
// 24bpp Image Operations
// ====================================================

int
Blend24bgr_24bgr_ref(READONLY(byte*) source,
                     READONLY(byte*) target,
                     READWRITE(byte*) destination,
                     unsigned sizeBytes,
                     float percentage)
{
    int szb = sizeBytes;
    int ipercent = (int)(percentage * 255.0f);
    int _1ipercent = (int)((1.0f - percentage) * 255.0f);

    PARALLELFOR(4)(int index = 0; index < szb; index++)
    {
        int src = (int)source[index];
        int tar = (int)target[index];
        int dst = (src * _1ipercent + tar * ipercent) / 255;
        destination[index] = (byte)dst;
    }

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

int
OpacityAdjust_32bgra_ref(READONLY(byte*) source,
                         READWRITE(byte*) destination,
                         unsigned sizeBytes,
                         float percentage)
{
    int szb = sizeBytes;
    uint32_t alphaLevel = ((byte)(percentage * 255.0f)) << 24;

    #pragma omp parallel for
    for(int index = 0; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        uint32_t bgra = *(uint32_t*)(source + index);
        uint32_t dst = (bgra & 0x00ffffff) | alphaLevel;
        *(uint32_t*)(destination + index) = dst;
    }

    return OperationSuccess;
}

int
AlphaBlend32bgra_32bgra_ref(READONLY(byte*) source,
                            READONLY(byte*) target,
                            READWRITE(byte*) destination,
                            unsigned sizeBytes)
{
    int32_t index = 0, szb = sizeBytes;

    PARALLELFOR(2)(; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        // read source & target pixels and get their alpha values
        uint32_t sp = *(uint32_t*)(source + index);
        uint32_t tp = *(uint32_t*)(target + index);
        uint32_t sa = (sp >> 24);
        uint32_t ta = (tp >> 24);

        // this was the original code, but unsigned divs (/da) can be replaced with /256 
        // which is optimized into a left shift; /da is done only once when precomputing factors.
        /*
        uint32_t da = sa + ta * (255 - sa) / 255;
        uint32_t dr = (sa * ((sp & 0x00FF0000) >> 16) + (da - sa) * ((tp & 0x00FF0000) >> 16)) / da;
        uint32_t dg = (sa * ((sp & 0x0000FF00) >> 8)  + (da - sa) * ((tp & 0x0000FF00) >> 8) ) / da;
        uint32_t db = (sa * ( sp & 0x000000FF)        + (da - sa) * ( tp & 0x000000FF)       ) / da;
        *(uint32_t*)(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);
        */

        // compute resulting alpha value (simple lerp)
        uint32_t da = sa + ta * (255 - sa) / 255;
        if (da == 0)
        {
            *(uint32_t*)(destination + index) = 0;
            continue;
        }

        // precompute division factors
        uint32_t scaled_dsa = ((da - sa) * 256) / da;

        // blend each color value according to the alpha values in each pixel(optimized lerp)
        uint32_t dr = ((256 - scaled_dsa) * ((sp & 0x00FF0000) >> 16) + scaled_dsa * ((tp & 0x00FF0000) >> 16)) / 256;
        uint32_t dg = ((256 - scaled_dsa) * ((sp & 0x0000FF00) >> 8)  + scaled_dsa * ((tp & 0x0000FF00) >> 8) ) / 256;
        uint32_t db = ((256 - scaled_dsa) * ( sp & 0x000000FF)        + scaled_dsa * ( tp & 0x000000FF)       ) / 256;
        
        // assemble the output pixel
        *(uint32_t*)(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);
    }

    return OperationSuccess;
}

int
ConvFilter_32bgra_ref(READONLY(byte*) source,
                      READWRITE(byte*) destination,
                      unsigned sizeBytes,
                      READONLY(float*) kernel,
                      unsigned width,
                      unsigned height)
{
    READONLY(int*) offsets = PixelOffsetsLookup(width, height, 4);
    if (offsets == NULL)
    {
        return OutOfMemory;
    }

    unsigned dimension = width * height;
    int szb = sizeBytes;

    PARALLELFOR(2)(int index = 0; index < szb; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        float sumB(0.0f), sumG(0.0f), sumR(0.0f), sumA(0.0f);
        for (unsigned k = 0; k < dimension; k++)
        {
            sumB += *(source + index + offsets[k]) * kernel[k];
            sumG += *(source + index + 1 + offsets[k]) * kernel[k];
            sumR += *(source + index + 2 + offsets[k]) * kernel[k];
            sumA += *(source + index + 3 + offsets[k]) * kernel[k];
        }
        *(destination + index) = (byte)Clamp(sumB, 0, 255);
        *(destination + index + 1) = (byte)Clamp(sumG, 0, 255);
        *(destination + index + 2) = (byte)Clamp(sumR, 0, 255);
        *(destination + index + 3) = (byte)Clamp(sumA, 0, 255);
    }

    delete[] offsets;

    return OperationSuccess;
}
