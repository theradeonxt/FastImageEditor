
#include "ModuleSetupPerformer.h"
#include "ConfigManager.h"
#include "ConfigManagerPIMPL.h"
#include "ModuleConfig.h"
#include "DispatchManager.h"
#include "ProfileTools.h"

namespace Config
{
    ModuleSetupPerformer::ModuleSetupPerformer(const std::string& name, const std::function<int32_t(void**)>& entryPoint)
    {
        // Supress exceptions leaking from the global state
        try
        {
            SIMDLevel newLevel = None;
            std::string moduleName;
            extractModuleInfo(name, moduleName, newLevel);

            ModuleConfig* config;
            if (g_ConfigManager().getSingleConfig(moduleName, config) == true)
            {
                appendConfigDetails(config, newLevel);
                g_ConfigManager().PIMPL().setBestImplementation(config);
            }
            else
            {
                createConfigWithDetails(moduleName, newLevel);
            }

            g_DispatchManager().Register(name, entryPoint);
        }
        catch (...)
        {
            TRACE_MESSAGE(L"ModuleSetupPerformer Fatal error at initialization");
            abort();
        }
    }

    void ModuleSetupPerformer::extractModuleInfo(const std::string& name, std::string& moduleName, SIMDLevel &level)
    {
        for (int i = 0; i < SIMDLevel::Count; i++)
        {
            std::string prefix(g_CodepathSuffix[i]);
            auto index = name.find(prefix);
            if (index != std::string::npos)
            {
                moduleName = std::string(name.begin(), name.begin() + index);
                level = static_cast<SIMDLevel>(i);
                return;
            }
        }
    }

    void ModuleSetupPerformer::appendConfigDetails(ModuleConfig* config, SIMDLevel levelDetails)
    {
        // only add given details if they are not already present
        auto& availableLevels = config->m_availableImpls;
        if (std::find(availableLevels.begin(), availableLevels.end(), levelDetails) == availableLevels.end())
        {
            availableLevels.push_back(levelDetails);
        }
    }

    void ModuleSetupPerformer::createConfigWithDetails(const std::string& moduleName, SIMDLevel levelDetails)
    {
        g_ConfigManager().PIMPL()
            .createDefaultConfig(moduleName, { levelDetails });
    }
}
