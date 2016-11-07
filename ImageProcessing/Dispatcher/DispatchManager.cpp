
#include "DispatchManager.h"
#include "Common.h"
#include "ConfigManager.h"
#include "ModuleConfig.h"

DispatchManager& g_DispatchManager()
{
    static DispatchManager* ans = new DispatchManager();
    return *ans;
}

void DispatchManager::Register(std::string entryPointKey, std::function<int(void**)> entryPoint)
{
    if (ContainsKey(entryPointKey))
    {
        return;
    }
    dispatcher[entryPointKey] = entryPoint;
}

int32_t DispatchManager::Run(std::string moduleName, void** args)
{
    Config::ModuleConfig* config;
    bool configFound = Config::g_ConfigManager().getSingleConfig(moduleName, config);
    if (configFound)
    {
        return dispatcher.at(config->m_DispatcherKey)(args);
    }
    return OperationFailed;
}

int32_t DispatchManager::RunFallback(const std::string& moduleName, void** args)
{
    Config::ModuleConfig* config;
    bool configFound = Config::g_ConfigManager().getSingleConfig(moduleName, config);
    if (configFound)
    {
        return dispatcher.at(moduleName + Config::g_CodepathSuffix[None])(args);
    }
    return OperationFailed;
}

bool DispatchManager::ContainsKey(const std::string& key)
{
    return dispatcher.find(key) != dispatcher.end();
}
