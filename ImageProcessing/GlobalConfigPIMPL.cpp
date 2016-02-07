
#include "GlobalConfigPIMPL.h"
#include "ProfileTools.h"
#include "SingleConfig.h"

#include <libcpuid.h>

namespace Config
{
    void GlobalConfigPIMPL::identifyCPUFeatures()
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
        m_CPUCoreCount = data.num_cores;
        m_CPUFlags[None] = 1;
        m_CPUFlags[SSE2] = data.flags[CPU_FEATURE_SSE2];
        m_CPUFlags[SSSE3] = data.flags[CPU_FEATURE_SSSE3];
        m_CPUFlags[AVX] = data.flags[CPU_FEATURE_AVX];
        m_CPUFlags[FMA3] = data.flags[CPU_FEATURE_FMA3];
    }

    void GlobalConfigPIMPL::setBestImplementation(READWRITE(SingleConfig*) config)
    {
        // the list af all possible implementations, sorted descending
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

    void GlobalConfigPIMPL::createDefaultConfig(std::string name, std::initializer_list<SIMDLevel> availableImpls)
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
            m_ConfigList[name].reset(config);
        }
    }
}
