
#ifndef IMAGEPROCESSING_GLOBALCONFIGPIMPL_H
#define IMAGEPROCESSING_GLOBALCONFIGPIMPL_H

#include "Common.h"

#include <string>
#include <map>
#include <cstdint>
#include <memory>

// Forward declarations
namespace Config {
    class GlobalConfig;
    class SingleConfig;
}

namespace Config
{
    //
    // Implementation for private parts of GlobalConfig
    //
    class GlobalConfigPIMPL
    {
        friend class GlobalConfig;
    public:
        GlobalConfigPIMPL(){}

    private:
        void createDefaultConfig(std::string name, std::initializer_list<SIMDLevel> availableImpls);
        void setBestImplementation(READWRITE(SingleConfig*) config);
        void identifyCPUFeatures();

        // The dictionary of single config elements
        std::map<std::string, std::unique_ptr<SingleConfig>> m_ConfigList;

        // Cpu details
        int32_t m_CPUCoreCount;
        int32_t m_CPUFlags[CPU_FLAGS];
    };
}

#endif /* IMAGEPROCESSING_GLOBALCONFIGPIMPL_H */
