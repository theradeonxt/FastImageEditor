
#ifndef IMAGEPROCESSING_GLOBALCONFIG_H
#define IMAGEPROCESSING_GLOBALCONFIG_H

#include "Common.h"

#include <string>
#include <cstdint>
#include <map>

// Forward declarations
enum SIMDLevel;

namespace Config {
    class SingleConfig;
}

namespace Config
{
    //
    // The dictionary of all single configs
    // This initializes all modules with their default properties
    //
    class GlobalConfig
    {
    public:
        GlobalConfig();

        bool getSingleConfig(std::string configName, SingleConfig*& config) const;

    private:
        void createDefaultConfig    (std::string name, std::initializer_list<SIMDLevel> availableImpls);
        void setBestImplementation  (READWRITE(SingleConfig*) config);
        bool queryCPUFeature        (SIMDLevel level);
        void identifyCPUFeatures    ();

        // Make instances non-copyable
        GlobalConfig(const GlobalConfig& other);
        GlobalConfig& operator=(const GlobalConfig& other);

        // The dictionary of single config elements
        std::map<std::string, SingleConfig*> m_ConfigList;

        // Cpu details
        int32_t m_CPUCoreCount;
        int32_t m_CPUFlags[CPU_FLAGS];

        typedef std::map<std::string, SingleConfig*>::const_iterator cfgIterator_t;
    };

    //
    // Configuration object used by modules
    // There is one instance of this, created at library loadtime.
    //
    extern GlobalConfig g_ModulesConfig;
}

//
// Exported helper functions from the library
//
IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
extern int32_t SetMultiThreadingStatus(READONLY(char*) moduleName, int32_t status);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
extern int32_t GetMultiThreadingStatus(READONLY(char*) moduleName);

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
extern int32_t QueryAvailableImplementation(READONLY(char*) moduleName, int32_t level);

#endif /* IMAGEPROCESSING_GLOBALCONFIG_H */
