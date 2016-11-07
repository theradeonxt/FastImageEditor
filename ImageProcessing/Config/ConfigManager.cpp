
#include "ConfigManager.h"
#include "ModuleConfig.h"
#include "ConfigManagerPIMPL.h"
#include "Common.h"
#include "ProfileTools.h"

namespace Config
{
    ConfigManager& g_ConfigManager()
    {
        static ConfigManager* ans = new ConfigManager();
        return *ans;
    }

    ConfigManager::ConfigManager()
    {
        // Supress exceptions leaking from the global state
        try
        {
            pimpl = new ConfigManagerPIMPL();
            pimpl->identifyCPUFeatures();
        }
        catch (...)
        {
            TRACE_MESSAGE(L"ConfigManager Fatal error at initialization");
            abort();
        }
    }

    ConfigManager::~ConfigManager()
    {
        delete pimpl;
        pimpl = nullptr;
    }

    bool ConfigManager::getSingleConfig(const std::string& configName, ModuleConfig*& config) const
    {
        auto it = pimpl->m_ConfigList.find(configName);
        if (it != pimpl->m_ConfigList.end())
        {
            config = it->second.get();
            return true;
        }
        TRACE_MESSAGE(L"ConfigManager Attempt to get a module that does not exist.");
        return false;
    }

    int32_t ConfigManager::queryCPUFeature(SIMDLevel level) const
    {
        if (0 <= level && level < CPU_FLAGS)
        {
            if (pimpl->m_CPUFlags[level] != 0)
            {
                return OperationSuccess;
            }
        }
        TRACE_MESSAGE(L"ConfigManager Invalid value as input parameter.");
        return OperationFailed;
    }
}
