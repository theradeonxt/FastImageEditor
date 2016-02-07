
#include "GlobalConfig.h"
#include "SingleConfig.h"
#include "Common.h"
#include "ProfileTools.h"

#include <libcpuid.h>

namespace Config
{
    GlobalConfig g_ModulesConfig;

    GlobalConfig::GlobalConfig()
    {
        // Supress exceptions leaking from the global state
        try
        {
            // Get information at startup about current processor
            identifyCPUFeatures();

            /* Initialize the default configs for modules using the best codepath for the current CPU
             * Note: We need to specify the available implementations for each module;
             *       This should reflect the way modules are implemented inside their cpp's.
             */
            createDefaultConfig("AlphaBlend32bgra_32bgra",  { None, SSE2, SSSE3, AVX, FMA3 });
            createDefaultConfig("Blend24bgr_24bgr",         { None, SSE2 });
            createDefaultConfig("OpacityAdjust_32bgra",     { None, SSE2 });
            createDefaultConfig("ConvFilter_32bgra",        { None, SSE2 });
            createDefaultConfig("Convert_32bgra_24hsv",     { None, SSE2 });
        }
        catch (...)
        {
        }
    }

    bool GlobalConfig::getSingleConfig(std::string configName, SingleConfig*& config) const
    {
        auto it = m_ConfigList.find(configName);
        if (it != m_ConfigList.end())
        {
            config = it->second;
            return true;
        }
        return false;
    }

    void GlobalConfig::identifyCPUFeatures()
    {
        struct cpu_raw_data_t raw;
        struct cpu_id_t data;
        int32_t status;

        std::fill(m_CPUFlags, m_CPUFlags + CPU_FLAGS, 0);

        // check for CPUID presence
        status = cpuid_present();
        if (status == 0) TRACE_MESSAGE(L"Sorry, your CPU doesn't support CPUID!");

        // obtain the raw CPUID data
        status = cpuid_get_raw_data(&raw);
        if (status < 0) TRACE_MESSAGE(L"Sorry, cannot get the CPUID raw data.");

        // identify the CPU, using the given raw data
        status = cpu_identify(&raw, &data);
        if (status < 0) TRACE_MESSAGE(L"Sorrry, CPU identification failed.");

        // store CPU feature details
        m_CPUCoreCount    = data.num_cores;
        m_CPUFlags[SSE2]  = data.flags[CPU_FEATURE_SSE2];
        m_CPUFlags[SSSE3] = data.flags[CPU_FEATURE_SSSE3];
        m_CPUFlags[AVX]   = data.flags[CPU_FEATURE_AVX];
        m_CPUFlags[FMA3]  = data.flags[CPU_FEATURE_FMA3];
    }

    void GlobalConfig::setBestImplementation(READWRITE(SingleConfig*) config)
    {
        // the list af all possible implementations
        static const SIMDLevel featureFlags[CPU_FLAGS] = { FMA3, AVX, SSSE3, SSE2, None };

        /* Set multithreading status into the module config
         * if Multithreading wasn't disabled at compile-time 
         */
#ifdef PARALLEL_COMPUTE
        m_CPUCoreCount > 1 ? config->m_EnableMultiThread = true : 0;
#endif

        /* Set highest available SIMD codepath into the module config,
         * depending on available codepaths and hardware support,
         * if Reference implementation wasn't forced at compile-time 
         */
#ifndef FORCE_REFERENCE_IMPL
        for (SIMDLevel level : featureFlags)
        {
            if (queryCPUFeature(level) && config->queryAvailableImplementation(level))
            {
                config->setSIMDLevel(level);
                break;
            }
        }
#endif
    }

    bool GlobalConfig::queryCPUFeature(SIMDLevel level)
    {
        if (0 <= level && level < CPU_FLAGS) return m_CPUFlags[level] != 0;
                
        TRACE_MESSAGE(L"Invalid value as input parameter.");
        return false;
    }

    void GlobalConfig::createDefaultConfig(std::string name, std::initializer_list<SIMDLevel> availableImpls)
    {
        SingleConfig *config = new SingleConfig(name);
        if (config)
        {
            // copy the implementation availability into the module config
            for (SIMDLevel level : availableImpls)
            {
                config->m_availableImpls.push_back(level);
            }

            setBestImplementation(config);

            // register the module config into the dictionary
            m_ConfigList[name] = config;
        }
    }
}

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
    Config::SingleConfig* config;

    if (Config::g_ModulesConfig.getSingleConfig(module, config))
    {
        return config->queryAvailableImplementation(static_cast<SIMDLevel>(level));
    }

    TRACE_MESSAGE(L"Module does not exist.");
    return OperationFailed;
}
