
#ifndef IMAGEPROCESSING_SINGLECONFIG_H
#define IMAGEPROCESSING_SINGLECONFIG_H

#include "Common.h"

#include <string>
#include <atomic>
#include <vector>

// Forward declarations
enum SIMDLevel;

namespace Config
{
    //
    // A single config element describing properties for a module
    //
    class SingleConfig
    {
        friend class GlobalConfig;
    public:
        SingleConfig(const std::string& name);

        const std::string& getName()              const;
        SIMDLevel          getSIMDLevel()         const;
        int32_t            isUsingReferenceImpl() const;
        int32_t            isMultiThreadEnabled() const;

        void forceReferenceImpl();
        void setSIMDLevel(SIMDLevel level);
        void setMultiThreadStatus(int32_t status);

        int32_t queryAvailableImplementation(SIMDLevel level) const;

    private:
        // Make instances non-copyable
        SingleConfig(const SingleConfig& other);
        SingleConfig& operator=(const SingleConfig& other);

        // Config unique identifier
        std::string m_Name;

        // Enable support of distributed work across multiple cores
        std::atomic<int32_t> m_EnableMultiThread;

        // Whether to use the standard C++ implementation or the optimized version 
        std::atomic<int32_t> m_UseReferenceImpl;

        // The version of SIMD it's using - selected automatically at runtime
        std::atomic<SIMDLevel> m_SIMDLevel;

        // All codepaths optimized for SIMD that this module supports
        std::vector<SIMDLevel> m_availableImpls;
    };
}

#endif /* IMAGEPROCESSING_SINGLECONFIG_H */
