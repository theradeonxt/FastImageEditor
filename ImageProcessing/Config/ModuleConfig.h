//! =============================================================================
//! \file 
//! \brief 
//!     A single config element describing the module properties
//!
//! \remarks The properties are atomic variables to account for thread-sharing.
//!          TODO: Properties are thread-shared, meaning different threads cannot
//!                have independent module configs.
//!
//! =============================================================================

#ifndef IMPROC_MODULECONFIG_H
#define IMPROC_MODULECONFIG_H

#include "Common.h"

#include <atomic>
#include <vector>

namespace Config
{
    class ModuleConfig
    {
        // Classes allowed to access private members
        friend class ConfigManager;
        friend class ConfigManagerPIMPL;
        friend class ModuleSetupPerformer;

    public:
        //! \brief Constructor
        //! Initializes the module properties wih default values.
        //! \param [in] name The module name.
        //! \remarks You can use compile-time definitions FORCE_REFERENCE_IMPL and PARALLEL_COMPUTE
        //!          to influence SIMD and multithreading default settings.
        ModuleConfig(std::string name);

        //! \brief Destructor 
        ~ModuleConfig(){}

        //! \brief Copy constructor
        ModuleConfig(const ModuleConfig& other);  

        //! \brief Copy-assignment operator
        ModuleConfig& operator=(const ModuleConfig& other);

        //! \brief Obtains which optimized codepath this module is currently using.
        //! \returns A value defined by SIMDLevel enum, including None.
        SIMDLevel getSIMDLevel() const;

        //! \brief Sets this module to use the requested codepath.
        //! \param [in] level The new value to set.
        //! \remarks This does not check the availability of the new value.
        void setSIMDLevel(SIMDLevel level);

        //! \brief Whether this module is currently using a multithreaded codepath.
        //! \returns 1 = Multithreaded enabled, 0 = Disabled.
        int32_t getMultiThreadStatus() const;

        //! \brief Enables or disabled multithreaded codepath for this module.
        //! \param [in] status The new status to set: 1 = Enabled, 0 = Disabled
        void setMultiThreadStatus(int32_t status);

        //! \brief Tests if this module has a codepath optimized for the requested value
        //! \param [in] level The requested codepath.
        //! \returns OperationSuccess if codepath found, OperationFailed otherwise.
        int32_t queryAvailableImplementation(SIMDLevel level) const;

    private:        
        std::atomic<int32_t>   m_EnableMultiThread; //!< Enable support of distributed work across multiple cores
        std::atomic<SIMDLevel> m_SIMDLevel;         //!< The optimization level the module is currently set to
        std::vector<SIMDLevel> m_availableImpls;    //!< All codepaths optimized for SIMD that this module supports
        std::string m_Name;

    public:
        std::string m_DispatcherKey;
    };

    //! \brief 
    //! Strings used as a name suffix associated with the optimization level
    extern const char* g_CodepathSuffix[];
    extern const char* g_MulticoreSuffix;
}

#endif /* IMPROC_MODULECONFIG_H */
