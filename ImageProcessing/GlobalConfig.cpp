
#include "GlobalConfig.h"
#include "SingleConfig.h"
#include "GlobalConfigPIMPL.h"
#include "Common.h"
#include "ProfileTools.h"

namespace Config
{
    GlobalConfig g_ModulesConfig;

    GlobalConfig::GlobalConfig()
    {
        // Supress exceptions leaking from the global state
        try
        {
            pimpl = new GlobalConfigPIMPL();

            // Get information at startup about current processor
            pimpl->identifyCPUFeatures();

            /* Initialize the default configs for modules using the best codepath for the current CPU
             * Note: We need to specify the available implementations for each module;
             *       This should reflect the way modules are implemented inside their cpp's.
             */
            pimpl->createDefaultConfig("AlphaBlend32bgra_32bgra", { None, SSE2, SSSE3, AVX, FMA3 });
            pimpl->createDefaultConfig("Blend24bgr_24bgr", { None, SSE2 });
            pimpl->createDefaultConfig("OpacityAdjust_32bgra", { None, SSE2 });
            pimpl->createDefaultConfig("ConvFilter_32bgra", { None, SSE2 });
            pimpl->createDefaultConfig("Convert_32bgra_24hsv", { None, SSE2 });
        }
        catch (...)
        {
            TRACE_MESSAGE(L"GlobalConfig Fatal error at initialization");
            abort();
        }
    }

    GlobalConfig::~GlobalConfig()
    {
        delete pimpl;
        pimpl = nullptr;
    }

    bool GlobalConfig::getSingleConfig(std::string configName, SingleConfig*& config) const
    {
        auto it = pimpl->m_ConfigList.find(configName);
        if (it != pimpl->m_ConfigList.end())
        {
            config = it->second.get();
            return true;
        }
        return false;
    }

    int32_t GlobalConfig::queryCPUFeature(SIMDLevel level)
    {
        if (0 <= level && level < CPU_FLAGS)
        {
            if (pimpl->m_CPUFlags[level] != 0)
            {
                return OperationSuccess;
            }
        }

        TRACE_MESSAGE(L"Invalid value as input parameter.");
        return OperationFailed;
    }

    //
    // Exported helper functions from the library
    //

    IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
        int32_t SetMultiThreadingStatus(READONLY(char*) moduleName, int32_t status)
    {
        std::string module = moduleName;
        Config::SingleConfig* config;

        if (Config::g_ModulesConfig.getSingleConfig(module, config))
        {
            config->setMultiThreadStatus(status);
            return OperationSuccess;
        }

        TRACE_MESSAGE(L"Module does not exist.");
        return OperationFailed;
    }

    IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
        int32_t GetMultiThreadingStatus(READONLY(char*) moduleName)
    {
        std::string module = moduleName;
        Config::SingleConfig* config;

        if (Config::g_ModulesConfig.getSingleConfig(module, config))
        {
            return config->isMultiThreadEnabled();
        }

        TRACE_MESSAGE(L"Module does not exist.");
        return OperationFailed;
    }

    IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
        int32_t QueryAvailableImplementation(READONLY(char*) moduleName, int32_t level)
    {
        std::string module = moduleName;
        SingleConfig* config;
        SIMDLevel simdLevel = static_cast<SIMDLevel>(level);

        if (g_ModulesConfig.getSingleConfig(module, config))
        {
            if (config->queryAvailableImplementation(simdLevel) == OperationSuccess
                && g_ModulesConfig.queryCPUFeature(simdLevel) == OperationSuccess)
            {
                return OperationSuccess;
            }
            return OperationFailed;
        }

        TRACE_MESSAGE(L"Module does not exist.");
        return OperationFailed;
    }

    IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
        int32_t SetImplementationLevel(READONLY(char*) moduleName, int32_t level)
    {
        std::string module = moduleName;
        SingleConfig* config;
        SIMDLevel simdLevel = static_cast<SIMDLevel>(level);

        if (g_ModulesConfig.getSingleConfig(module, config))
        {
            if (config->queryAvailableImplementation(simdLevel) == OperationSuccess
                && g_ModulesConfig.queryCPUFeature(simdLevel) == OperationSuccess)
            {
                config->setSIMDLevel(simdLevel);
                return OperationSuccess;
            }
            return OperationFailed;
        }

        TRACE_MESSAGE(L"Module does not exist.");
        return OperationFailed;
    }
}
