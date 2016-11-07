//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_GLOBALCONFIG_H
#define IMPROC_GLOBALCONFIG_H

#include "Common.h"

#include <string>
#include <cstdint>

// Forward declarations
namespace Config {
    class ModuleConfig;
    class ConfigManagerPIMPL;
}

namespace Config
{
    class ConfigManager
    {
    public:
        //! \brief Constructor
        //! Initializes the property collection (PIMPL), and identifies CPU features.
        ConfigManager();

        //! \brief Destructor 
        //! Frees the resources asociated with private PIMPL.
        ~ConfigManager();

        //! \brief Obtains the module config associated with the requested name.
        //! \param [in]  configName: The module name.
        //! \param [out] config:     A pointer to the module config where the result is placed.
        //! \returns: The status of the operation: true = Found, false: Not found.
        //! \remarks: If no module of the given name exists, the pointer is not modified.
        bool getSingleConfig(const std::string& configName, ModuleConfig*& config) const;

        //! \brief Checks if the current CPU supports the requested optimizations.
        //! \param [in] level: The requested optimization level.
        //! \returns The status of the operation: OperationSuccess = Supported, OperationFailed = Unsupported.
        int32_t queryCPUFeature(SIMDLevel level) const;

        //! \brief Obtains a reference to the private object.
        ConfigManagerPIMPL& PIMPL() { return *pimpl; }

    private:
        ConfigManager(const ConfigManager& other);            //!< Unimplemented
        ConfigManager& operator=(const ConfigManager& other); //!< Unimplemented

        ConfigManagerPIMPL *pimpl; //!< Private class implementation
    };

    //! \brief Configuration object used globally
    //! There is a single instance of it created on first use.
    extern ConfigManager& g_ConfigManager();
}

#endif /* IMPROC_GLOBALCONFIG_H */
