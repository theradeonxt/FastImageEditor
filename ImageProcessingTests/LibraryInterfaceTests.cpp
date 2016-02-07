
#include "stdafx.h"
#include "..\ImageProcessing\GlobalConfig.h"

#include <vector>
#include <string>

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;

namespace ImageProcessingTests
{
    class TestSetup
    {
    public:
        static std::vector<std::string> GetModuleList()
        {
            std::vector<std::string> moduleList = {
                "Blend24bgr_24bgr",
                "Convert_32bgra_24hsv",
                "OpacityAdjust_32bgra",
                "AlphaBlend32bgra_32bgra",
                "ConvFilter_32bgra"
            };
            return moduleList;
        }
    };

    [TestClass]
    public ref class LibraryInterfaceTests
    {
    public:
        // Tests changing Multicore implementation between two states:
        // enabled & disabled
        [TestMethod]
        void MulticoreStatusChange()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto statusMT = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusMT != StatusCode::OperationFailed);

                auto newStatusMT = !statusMT;

                auto result = SetMultiThreadingStatus(module.c_str(), newStatusMT);
                Assert::IsTrue(result == StatusCode::OperationSuccess);
                Assert::IsTrue(result != StatusCode::OperationFailed);

                auto statusMTAfter = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusMT != statusMTAfter);
                Assert::IsTrue(statusMTAfter != StatusCode::OperationFailed);
            }
        }

        // Tests for availability of Reference and SSE2 implementations
        // These are considered the baseline for every module
        [TestMethod]
        void BaselineImplementationQuery()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto status = QueryAvailableImplementation(module.c_str(), SIMDLevel::None);
                Assert::IsTrue(status == OperationSuccess);

                status = QueryAvailableImplementation(module.c_str(), SIMDLevel::SSE2);
                Assert::IsTrue(status == OperationSuccess);
            }
        }

        // Tests changing the optimization level of modules 
        // This cycles the different levels a module has and changes them, then reverts back
        [TestMethod]
        void SIMDLevelChange()
        {
            auto queryLevels = { None, SSE2, SSSE3, AVX, FMA3 };

            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                // get all available implementation levels for this module
                std::vector<SIMDLevel> levelsList;
                for (auto level : queryLevels)
                {
                    auto status = QueryAvailableImplementation(module.c_str(), level);
                    if (status == OperationSuccess)
                    {
                        levelsList.push_back(level);
                    }
                }

                // try to cycle through each level
                for (auto level : levelsList)
                {
                    if (level != None)
                    {
                        auto statusToNone = SetImplementationLevel(module.c_str(), None);
                        Assert::IsTrue(statusToNone == OperationSuccess);

                        auto statusRevert = SetImplementationLevel(module.c_str(), level);
                        Assert::IsTrue(statusRevert == OperationSuccess);
                    }
                }
            }
        }
    };
}
