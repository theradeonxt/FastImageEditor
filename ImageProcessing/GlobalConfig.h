
#ifndef IMAGEPROCESSING_GLOBALCONFIG_H
#define IMAGEPROCESSING_GLOBALCONFIG_H

#include "Common.h"

#include <string>
#include <cstdint>

// Forward declarations
enum SIMDLevel;
namespace Config {
    class SingleConfig;
    class GlobalConfigPIMPL;
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
        ~GlobalConfig();

        bool getSingleConfig(std::string configName, SingleConfig*& config) const;
        int32_t queryCPUFeature(SIMDLevel level);

    private:
        // Make instances non-copyable
        GlobalConfig(const GlobalConfig& other);
        GlobalConfig& operator=(const GlobalConfig& other);

        // Private class implementation
        GlobalConfigPIMPL *pimpl;
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

IMAGEPROCESSING_CDECL IMAGEPROCESSING_API
extern int32_t SetImplementationLevel(READONLY(char*) moduleName, int32_t level);

#endif /* IMAGEPROCESSING_GLOBALCONFIG_H */
