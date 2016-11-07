//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#include "stdafx.h"

#include "ImageProcessing.h"
#include "ModuleInterface.h"

class TestSetup
{
public:
    static std::vector<std::string> GetModuleList()
    {
        std::vector<std::string> moduleList = {
            "Blend24bgr_24bgr",
            "Convert_32bgra_24hsv",
            "OpacityAdjust_32bgra",
            "AlphaBlend32bgra_32bgra",
            "ConvFilter_32bgra"
        };
        return moduleList;
    }

    static std::vector<SIMDLevel> GetAllAvailableLevels(std::string module)
    {
        auto queryLevels = { None, SSE2, SSSE3, AVX, FMA3 };
        std::vector<SIMDLevel> result;
        for (auto level : queryLevels)
        {
            auto status = QueryAvailableImplementation(module.c_str(), level);
            if (status == OperationSuccess)
            {
                result.push_back(level);
            }
        }
        return result;
    }

    static std::vector<SIMDLevel> GetAllLevels()
    {
        auto result = { None, SSE2, SSSE3, AVX, FMA3 };
        return result;
    }

    static std::vector<SIMDLevel> GetUnavailableLevels(std::string module)
    {
        std::vector<SIMDLevel> result;
        auto& levelsAccepted = GetAllAvailableLevels(module);
        auto& levels = GetAllLevels();
        for (auto level : levels)
        {
            if (std::find(levelsAccepted.begin(), levelsAccepted.end(), level) == levelsAccepted.end())
            {
                result.push_back(level);
            }
        }
        return result;
    }
};