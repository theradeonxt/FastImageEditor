
#include "ConfigManager.h"
#include "ModuleConfig.h"

//
// Define more readable flags for multithreading and impementation switches
//
#ifdef PARALLEL_COMPUTE
  #define IS_PARALLEL_ENABLED 1
#else 
  #define IS_PARALLEL_ENABLED 0
#endif /* PARALLEL_COMPUTE */

#ifdef FORCE_REFERENCE_IMPL
  #define IMPL_LEVEL SIMDLevel::None
#else 
  #define IMPL_LEVEL SIMDLevel::SSE2
#endif /* FORCE_REFERENCE_IMPL */

namespace Config
{
    ModuleConfig::ModuleConfig(const std::string name)
        : m_EnableMultiThread(IS_PARALLEL_ENABLED),
          m_SIMDLevel        (IMPL_LEVEL),
          m_Name             (name),
          m_DispatcherKey(name + g_CodepathSuffix[SIMDLevel::None])
    {
    }

    ModuleConfig::ModuleConfig(const ModuleConfig& other)
    {
        *this = other;
    }

    ModuleConfig& ModuleConfig::operator= (const ModuleConfig& other)
    {
        m_EnableMultiThread = other.m_EnableMultiThread.operator int();
        m_SIMDLevel = other.m_SIMDLevel.operator SIMDLevel();
        m_availableImpls = other.m_availableImpls;
        return *this;
    }

    int32_t ModuleConfig::getMultiThreadStatus() const
    {
        return m_EnableMultiThread;
    }

    void ModuleConfig::setMultiThreadStatus(int32_t status)
    {
        m_EnableMultiThread = status;
        m_DispatcherKey = m_Name 
            + g_CodepathSuffix[m_SIMDLevel]
            + (status ? g_MulticoreSuffix : "");
    }

    SIMDLevel ModuleConfig::getSIMDLevel() const
    {
        return m_SIMDLevel.operator SIMDLevel();
    }

    void ModuleConfig::setSIMDLevel(SIMDLevel level)
    {
        m_SIMDLevel = level;
        m_DispatcherKey = m_Name
            + g_CodepathSuffix[m_SIMDLevel]
            + (m_EnableMultiThread ? g_MulticoreSuffix : "");
    }

    int32_t ModuleConfig::queryAvailableImplementation(SIMDLevel level) const
    {
        if (find(m_availableImpls.begin(), m_availableImpls.end(), level)
            != m_availableImpls.end())
        {
            return OperationSuccess;
        }
        return OperationFailed;
    }

    const char* g_CodepathSuffix[] = { "_ref", "_SSE2", "_SSSE3", "_AVX", "_FMA3" };
    const char* g_MulticoreSuffix = { "_MT" };
}
