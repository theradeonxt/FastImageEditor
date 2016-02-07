
#include "GlobalConfig.h"
#include "SingleConfig.h"
#include "Common.h"

//
// Define more readable flags for multithreading and impementation switches
//
#ifdef PARALLEL_COMPUTE
#define IS_PARALLEL_ENABLED 1
#else 
#define IS_PARALLEL_ENABLED 0
#endif

#ifdef FORCE_REFERENCE_IMPL
#define IS_FORCED_REFERENCE 1
#define IMPL_LEVEL SIMDLevel::None
#else 
#define IS_FORCED_REFERENCE 0
#define IMPL_LEVEL SIMDLevel::SSE2
#endif

namespace Config
{
    //
    // Initializes a named config with the default values.
    // The values are set at compile time, but they can be later changed at runtime.
    //
    SingleConfig::SingleConfig(const std::string& name)
        : m_Name             (name),
          m_EnableMultiThread(IS_PARALLEL_ENABLED),
          m_UseReferenceImpl (IS_FORCED_REFERENCE),
          m_SIMDLevel        (IMPL_LEVEL)
    {
    }

    const std::string& SingleConfig::getName() const
    {
        return m_Name;
    }

    int32_t SingleConfig::isMultiThreadEnabled() const
    {
        return m_EnableMultiThread;
    }

    void SingleConfig::setMultiThreadStatus(int32_t status)
    {
        m_EnableMultiThread = status;
    }

    int32_t SingleConfig::isUsingReferenceImpl() const
    {
        return m_UseReferenceImpl || m_SIMDLevel == None;
    }

    void SingleConfig::forceReferenceImpl()
    {
        m_UseReferenceImpl = true;
        m_SIMDLevel = None;
    }

    SIMDLevel SingleConfig::getSIMDLevel() const
    {
        return m_UseReferenceImpl ? None : m_SIMDLevel.operator SIMDLevel();
    }

    void SingleConfig::setSIMDLevel(SIMDLevel level)
    {
        m_UseReferenceImpl = (m_SIMDLevel == None);
        m_SIMDLevel = level;
    }

    int32_t SingleConfig::queryAvailableImplementation(SIMDLevel level) const
    {
        if (find(m_availableImpls.begin(), m_availableImpls.end(), level)
            != m_availableImpls.end())
        {
            return OperationSuccess;
        }
        return OperationFailed;
    }
}
