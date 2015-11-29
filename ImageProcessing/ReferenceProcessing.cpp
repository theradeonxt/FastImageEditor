
#include "ReferenceProcessing.h"
#include "Helpers.hpp"
#include "ProfileTools.h"

#include <cstdint>

// ====================================================
// 24bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_ref(READONLY (uint8_t*) source,
                     READONLY (uint8_t*) target,
                     READWRITE(uint8_t*) destination,
                     uint32_t            sizeBytes,
                     float               percentage)
{
    int32_t nSizeBytes     = int32_t(sizeBytes);
    uint32_t alpha         = uint32_t(percentage * 255.0f);
    uint32_t oneMinusAlpha = uint32_t((1.0f - percentage) * 255.0f);

    REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_ref);
    for (int32_t index = 0; index < nSizeBytes; index++)
    {
        BEGIN_TIMED_BLOCK();

        uint32_t src = uint32_t(source[index]);
        uint32_t tar = uint32_t(target[index]);
        uint32_t dst = (src * oneMinusAlpha + tar * alpha) / 255;
        destination[index] = uint8_t(dst);

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Channel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
Blend24bgr_24bgr_ref_MT(READONLY (uint8_t*) source,
                        READONLY (uint8_t*) target,
                        READWRITE(uint8_t*) destination,
                        uint32_t            sizeBytes,
                        float               percentage)
{
    int32_t nSizeBytes = int32_t(sizeBytes);
    uint32_t alpha = uint32_t(percentage * 255.0f);
    uint32_t oneMinusAlpha = uint32_t((1.0f - percentage) * 255.0f);

    REGISTER_TIMED_BLOCK(Blend24bgr_24bgr_ref_MT);
#pragma omp parallel for
    for (int32_t index = 0; index < nSizeBytes; index++)
    {
        BEGIN_TIMED_BLOCK();

        uint32_t src = uint32_t(source[index]);
        uint32_t tar = uint32_t(target[index]);
        uint32_t dst = (src * oneMinusAlpha + tar * alpha) / 255;
        destination[index] = uint8_t(dst);

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Channel: ");

    return OperationSuccess;
}

// ====================================================
// 32bpp Image Operations
// ====================================================

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_ref(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         float               percentage)
{
    int32_t nSizeBytes = int32_t(sizeBytes);
    uint32_t alphaLevel = uint32_t(percentage * 255.0f) << 24;

    REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_ref);
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        uint32_t bgra = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t dst  = (bgra & 0x00ffffff) | alphaLevel;
        *reinterpret_cast<uint32_t*>(destination + index) = dst;
        
        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
OpacityAdjust_32bgra_ref_MT(READONLY (uint8_t*) source,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes,
                            float               percentage)
{
    int32_t nSizeBytes  = int32_t(sizeBytes);
    uint32_t alphaLevel = uint32_t(percentage * 255.0f) << 24;

    REGISTER_TIMED_BLOCK(OpacityAdjust_32bgra_ref_MT);
#pragma omp parallel for
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        uint32_t bgra = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t dst  = (bgra & 0x00ffffff) | alphaLevel;
        *reinterpret_cast<uint32_t*>(destination + index) = dst;

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_ref(READONLY (uint8_t*) source,
                            READONLY (uint8_t*) target,
                            READWRITE(uint8_t*) destination,
                            uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

    REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_ref);
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        // read source & target pixels and get their alpha values
        uint32_t sp = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t tp = *reinterpret_cast<const uint32_t*>(target + index);
        uint32_t sa = (sp >> 24);
        uint32_t ta = (tp >> 24);

        // compute resulting alpha value (simple lerp)
        uint32_t da = sa + ta * (255 - sa) / 255;

        // exit case to prevent division by 0
        if (da == 0)
        {
            *reinterpret_cast<uint32_t*>(destination + index) = 0;
            continue;
        }

        // precompute factors to workaround expensive division
        uint32_t scaled_dsa = ((da - sa) * 256) / da;

        // blend each color value according to the alpha values in each pixel (optimized lerp)
        uint32_t dr = ((256 - scaled_dsa) * ((sp & 0x00FF0000) >> 16) + scaled_dsa * ((tp & 0x00FF0000) >> 16)) / 256;
        uint32_t dg = ((256 - scaled_dsa) * ((sp & 0x0000FF00) >> 8)  + scaled_dsa * ((tp & 0x0000FF00) >> 8) ) / 256;
        uint32_t db = ((256 - scaled_dsa) * ( sp & 0x000000FF)        + scaled_dsa * ( tp & 0x000000FF)       ) / 256; 

        // assemble the output pixel
        *reinterpret_cast<uint32_t*>(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);
        
        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
AlphaBlend32bgra_32bgra_ref_MT(READONLY (uint8_t*) source,
                               READONLY (uint8_t*) target,
                               READWRITE(uint8_t*) destination,
                               uint32_t            sizeBytes)
{
    int32_t nSizeBytes = int32_t(sizeBytes);

    REGISTER_TIMED_BLOCK(AlphaBlend32bgra_32bgra_ref_MT);
#pragma omp parallel for
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        // read source & target pixels and get their alpha values
        uint32_t sp = *reinterpret_cast<const uint32_t*>(source + index);
        uint32_t tp = *reinterpret_cast<const uint32_t*>(target + index);
        uint32_t sa = (sp >> 24);
        uint32_t ta = (tp >> 24);

        // compute resulting alpha value (simple lerp)
        uint32_t da = sa + ta * (255 - sa) / 255;

        // exit case to prevent division by 0
        if (da == 0)
        {
            *reinterpret_cast<uint32_t*>(destination + index) = 0;
            continue;
        }

        // precompute factors to workaround expensive division
        uint32_t scaled_dsa = ((da - sa) * 256) / da;

        // blend each color value according to the alpha values in each pixel (optimized lerp)
        uint32_t dr = ((256 - scaled_dsa) * ((sp & 0x00FF0000) >> 16) + scaled_dsa * ((tp & 0x00FF0000) >> 16)) / 256;
        uint32_t dg = ((256 - scaled_dsa) * ((sp & 0x0000FF00) >> 8) + scaled_dsa * ((tp & 0x0000FF00) >> 8)) / 256;
        uint32_t db = ((256 - scaled_dsa) * (sp & 0x000000FF) + scaled_dsa * (tp & 0x000000FF)) / 256;

        // assemble the output pixel
        *reinterpret_cast<uint32_t*>(destination + index) = (da << 24) | (dr << 16) | (dg << 8) | (db);

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
ConvFilter_32bgra_ref(READONLY (uint8_t*) source,
                      READWRITE(uint8_t*) destination,
                      uint32_t            sizeBytes,
                      READONLY (float*)   kernel,
                      uint32_t            width,
                      uint32_t            height)
{
    READONLY(int32_t*) offsetLookup = PixelOffsetsLookup(width, height, 4);
    if (offsetLookup == nullptr)
    {
        return OutOfMemory;
    }

    int32_t dimension = width * height;
    int32_t nSizeBytes = int32_t(sizeBytes);

    // BUG: this crashes for values near the edges of filter window

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_ref);
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        float sumB(0.0f);
        float sumG(0.0f);
        float sumR(0.0f);
        float sumA(0.0f);
        for (int32_t k = 0; k < dimension; k++)
        {
            sumB += *(source + index + 0 + offsetLookup[k]) * kernel[k];
            sumG += *(source + index + 1 + offsetLookup[k]) * kernel[k];
            sumR += *(source + index + 2 + offsetLookup[k]) * kernel[k];
            sumA += *(source + index + 3 + offsetLookup[k]) * kernel[k];
        }
        *(destination + index + 0) = uint8_t(Clamp(sumB, 0, 255));
        *(destination + index + 1) = uint8_t(Clamp(sumG, 0, 255));
        *(destination + index + 2) = uint8_t(Clamp(sumR, 0, 255));
        *(destination + index + 3) = uint8_t(Clamp(sumA, 0, 255));
    
        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}

IMAGEPROCESSING_CDECL int32_t
ConvFilter_32bgra_ref_MT(READONLY (uint8_t*) source,
                         READWRITE(uint8_t*) destination,
                         uint32_t            sizeBytes,
                         READONLY (float*)   kernel,
                         uint32_t            width,
                         uint32_t            height)
{
    READONLY(int32_t*) offsetLookup = PixelOffsetsLookup(width, height, 4);
    if (offsetLookup == nullptr)
    {
        return OutOfMemory;
    }

    int32_t dimension = width * height;
    int32_t nSizeBytes = int32_t(sizeBytes);

    // BUG: this crashes for values near the edges of filter window

    REGISTER_TIMED_BLOCK(ConvFilter_32bgra_ref_MT);
#pragma omp parallel for
    for (int32_t index = 0; index < nSizeBytes; index += 4) // guaranteed to be multiple of 4bytes (32bpp-BGRA)
    {
        BEGIN_TIMED_BLOCK();

        float sumB(0.0f);
        float sumG(0.0f);
        float sumR(0.0f);
        float sumA(0.0f);
        for (int32_t k = 0; k < dimension; k++)
        {
            sumB += *(source + index + 0 + offsetLookup[k]) * kernel[k];
            sumG += *(source + index + 1 + offsetLookup[k]) * kernel[k];
            sumR += *(source + index + 2 + offsetLookup[k]) * kernel[k];
            sumA += *(source + index + 3 + offsetLookup[k]) * kernel[k];
        }
        *(destination + index + 0) = uint8_t(Clamp(sumB, 0, 255));
        *(destination + index + 1) = uint8_t(Clamp(sumG, 0, 255));
        *(destination + index + 2) = uint8_t(Clamp(sumR, 0, 255));
        *(destination + index + 3) = uint8_t(Clamp(sumA, 0, 255));

        END_TIMED_BLOCK();
    }
    PROFILE_TRACE_BLOCK(L" - Cycles/Pixel: ");

    return OperationSuccess;
}
