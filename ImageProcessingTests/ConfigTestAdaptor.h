
#include "stdafx.h"

#include "ImageProcessing.h"
#include "GlobalConfig.h"

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
        static auto queryLevels = { None, SSE2, SSSE3, AVX, FMA3 };
        std::vector<SIMDLevel> levelsList;
        for (auto level : queryLevels)
        {
            auto status = QueryAvailableImplementation(module.c_str(), level);
            if (status == OperationSuccess)
            {
                levelsList.push_back(level);
            }
        }
        return levelsList;
    }
};