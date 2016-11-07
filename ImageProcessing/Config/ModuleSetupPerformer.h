//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_MODULE_SETUP_PERFORMER_H
#define IMPROC_MODULE_SETUP_PERFORMER_H

#include "Common.h"
#include "ModuleSetupDetails.hpp" // Note: Do not remove, needed by module implementations

#include <cstdint>
#include <functional>

// Forard declarations
namespace Config {
    class ModuleConfig;
}

namespace Config
{
    class ModuleSetupPerformer
    {
    public:
        //! \brief Constructor
        //!        Registers a config for the module associated with the given entry key
        //! \param [in] name:       The module entryKey (function signature).
        //! \param [in] entryPoint: Wrapper object for call abstraction.
        ModuleSetupPerformer(const std::string& entryKey, const std::function<int32_t(void**)>& entryPoint);

        //! \brief Destructor 
        ~ModuleSetupPerformer(){}

    private:
        //! \brief Parses the full module name to obtain its real name (without suffixes)
        //!        and the codepath given by its suffix.
        //! \param [in]  name:       The full module name.
        //! \param [out] moduleName: The real module name.
        //! \param [out] level:      The codepath associated with the suffix.
        void extractModuleInfo(const std::string& name, std::string& moduleName, SIMDLevel &level);
        
        //! \brief Adds the given codepath detail to the specified module.
        //! \param [in] config: Pointer to the module config.
        //! \param [in] level:  The codepath detail.
        void appendConfigDetails(READWRITE(ModuleConfig*) config, SIMDLevel level);
        
        //! \brief Creates a new config for the given module containing the given codepath detail.
        //! \param [in] moduleName:   The module real name.
        //! \param [in] levelDetails: The codepath detail.
        void createConfigWithDetails(const std::string& moduleName, SIMDLevel level);
    };
}

#endif /* IMPROC_MODULE_SETUP_PERFORMER_H */
