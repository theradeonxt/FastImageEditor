
#include "ModuleInterface.h"
#include "ModuleConfig.h"
#include "ConfigManager.h"

using namespace Config;

IMPROC_CDECL IMPROC_API
int32_t SetMultiThreadingStatus(READONLY(char*) moduleName, int32_t status)
{
    std::string module = moduleName;
    ModuleConfig* config;

    if (g_ConfigManager().getSingleConfig(module, config))
    {
        config->setMultiThreadStatus(status);
        return OperationSuccess;
    }

    return OperationFailed;
}

IMPROC_CDECL IMPROC_API
int32_t GetMultiThreadingStatus(READONLY(char*) moduleName)
{
    std::string module = moduleName;
    ModuleConfig* config;

    if (g_ConfigManager().getSingleConfig(module, config))
    {
        return config->getMultiThreadStatus();
    }

    return OperationFailed;
}

IMPROC_CDECL IMPROC_API
int32_t QueryAvailableImplementation(READONLY(char*) moduleName, int32_t level)
{
    std::string module = moduleName;
    ModuleConfig* config;
    SIMDLevel simdLevel = static_cast<SIMDLevel>(level);

    if (g_ConfigManager().getSingleConfig(module, config))
    {
        if (config->queryAvailableImplementation(simdLevel) == OperationSuccess
            && g_ConfigManager().queryCPUFeature(simdLevel) == OperationSuccess)
        {
            return OperationSuccess;
        }
        return OperationFailed;
    }

    return OperationFailed;
}

IMPROC_CDECL IMPROC_API
int32_t SetImplementationLevel(READONLY(char*) moduleName, int32_t level)
{
    std::string module = moduleName;
    ModuleConfig* config;
    SIMDLevel simdLevel = static_cast<SIMDLevel>(level);

    if (g_ConfigManager().getSingleConfig(module, config))
    {
        if (config->queryAvailableImplementation(simdLevel) == OperationSuccess
            && g_ConfigManager().queryCPUFeature(simdLevel) == OperationSuccess)
        {
            config->setSIMDLevel(simdLevel);
            return OperationSuccess;
        }
        return OperationFailed;
    }

    return OperationFailed;
}

IMPROC_CDECL IMPROC_API
int32_t GetImplementationLevel(READONLY(char*) moduleName)
{
    std::string module = moduleName;
    ModuleConfig* config;

    if (g_ConfigManager().getSingleConfig(module, config))
    {
        return config->getSIMDLevel();
    }

    return OperationFailed;
}

IMPROC_CDECL IMPROC_API
int32_t MEMCMP(READONLY(void*) src, READONLY(void*) tar, int32_t sizeBytes)
{
    int32_t result = memcmp(src, tar, sizeBytes);

    for (int i = 0; i < sizeBytes; i++)
    {
        if (((uint8_t*)src)[i] != ((uint8_t*)tar)[i])
        {
            int u = 42;
        }
    }

    return result;
}
