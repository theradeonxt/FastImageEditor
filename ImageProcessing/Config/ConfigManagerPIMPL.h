//! =============================================================================
//! \file 
//! \brief 
//!     Implementation for private parts of ConfigManager
//!
//! Contains the collection of module configs,
//!     i.e. a mapping from the config name to their settings.
//! Also, it contains the CPU supported features of interest.
//!
//! =============================================================================

#ifndef IMPROC_CONFIGMANAGER_PIMPL_H
#define IMPROC_CONFIGMANAGER_PIMPL_H

#include "Common.h"

#include <string>
#include <cstdint>
#include <memory>
#include <map>

// Forward declarations
namespace Config {
    class ModuleConfig;
}

namespace Config
{
    class ConfigManagerPIMPL
    {
        friend class ConfigManager;
        friend class ModuleSetupPerformer;

    public:
        //! \brief Constructor
        ConfigManagerPIMPL() : m_CPUCoreCount(1) {}

        //! \brief Destructor 
        ~ConfigManagerPIMPL(){}

        //! \brief Creates the module config associated with the requested name,
        //!        by default using the best codepath available.
        //! \param [in]  configName:     The module name.
        //! \param [out] availableImpls: The available codepaths for this module.
        //! \remarks: If no module of the given name exists, nothing is done.
        void createDefaultConfig(const std::string& name, const std::initializer_list<SIMDLevel>& availableImpls);

    private:        
        //! \brief Sets the given module config to use the autodetected best codepath.
        //!        This is a combination of CPU features and module available codepaths.
        //! \param [in] The module config to modify.
        //! \remarks Global flags PARALLEL_COMPUTE and FORCE_REFERENCE_IMPL affect the default behavior.
        //!          This means, autodetection can be overriden at compile time, if desired.
        void setBestImplementation(READWRITE(ModuleConfig*) config);
        
        //! \brief Obtains information about the running CPU and stores features of interest.
        void identifyCPUFeatures();

        //! \brief The dictionary of single config elements
        std::map<std::string, std::unique_ptr<ModuleConfig>> m_ConfigList;

        int32_t m_CPUCoreCount;        //!< Number of CPU logical cores 
        int32_t m_CPUFlags[CPU_FLAGS]; //!< CPU features of interest
    };
}

#endif /* IMPROC_CONFIGMANAGER_PIMPL_H */
