
#include "stdafx.h"

#include "GlobalConfig.h"
#include "ConfigTestAdaptor.h"

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public ref class LibraryInterfaceTests
    {
    public:
        //
        // Tests changing Multicore implementation between two states: enabled & disabled
        //
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

                auto statusMTAfter = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusMT != statusMTAfter);
                Assert::IsTrue(statusMTAfter != StatusCode::OperationFailed);
            }
        }

        //
        // Tests for availability of Reference and SSE2 implementations
        // These are considered the baseline for every module
        //
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
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto& levelsList = TestSetup::GetAllAvailableLevels(module);

                // try to cycle through each level
                for (auto level : levelsList)
                {
                    // set to None and expect to get back None
                    auto statusSet = SetImplementationLevel(module.c_str(), None);
                    Assert::IsTrue(statusSet == OperationSuccess);
                    auto statusGet = GetImplementationLevel(module.c_str());
                    Assert::IsTrue(statusGet != OperationFailed);
                    Assert::IsTrue(statusGet == None);

                    // set to level and expect to get back level
                    statusSet = SetImplementationLevel(module.c_str(), level);
                    Assert::IsTrue(statusSet == OperationSuccess);
                    statusGet = GetImplementationLevel(module.c_str());
                    Assert::IsTrue(statusGet != OperationFailed);
                    Assert::IsTrue(statusGet == level);
                }
            }
        }
    };
}
