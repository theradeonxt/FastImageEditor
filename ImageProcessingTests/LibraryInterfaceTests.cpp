
#include "stdafx.h"

#include "ModuleInterface.h"
#include "ConfigTestAdaptor.hpp"

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
        void MulticoreStatusChangeSucceeds()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                // set to disabled and expect to get back disabled
                auto statusSet = SetMultiThreadingStatus(module.c_str(), 0);
                Assert::IsTrue(statusSet == StatusCode::OperationSuccess);

                auto statusGet = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusGet != StatusCode::OperationFailed);
                Assert::IsTrue(statusGet == 0);

                // set to enabled and expect to get back enabled
                statusSet = SetMultiThreadingStatus(module.c_str(), 1);
                Assert::IsTrue(statusSet == StatusCode::OperationSuccess);

                statusGet = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusGet != StatusCode::OperationFailed);
                Assert::IsTrue(statusGet == 1);
            }
        }

        //
        // Tests for availability of Reference
        //
        [TestMethod]
        void ModulesShouldHaveReferenceImplementation()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto status = QueryAvailableImplementation(module.c_str(), SIMDLevel::None);
                Assert::IsTrue(status == OperationSuccess);
            }
        }

        //
        // Tests for availability of SSE2 implementations
        //
        [TestMethod]
        void ModulesShouldHaveSSE2Implementation()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto status = QueryAvailableImplementation(module.c_str(), SIMDLevel::SSE2);
                Assert::IsTrue(status == OperationSuccess);
            }
        }

        //
        // Tests changing the optimization level of modules 
        // This cycles the different levels a module has and changes them, then reverts back
        //
        [TestMethod]
        void SIMDLevelChangeSucceeds()
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

        //
        // Tests that trying to change to an unavailable implementation is not permitted
        //
        [TestMethod]
        void SetUnavailableSIMDLevelShouldFail()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                // try to set an invalid level and expact to fail
                for (auto level : TestSetup::GetUnavailableLevels(module))
                {
                    auto statusGet = GetImplementationLevel(module.c_str());
                    Assert::IsTrue(statusGet != OperationFailed);

                    auto statusSet = SetImplementationLevel(module.c_str(), level);
                    Assert::IsTrue(statusSet == OperationFailed);

                    auto statusGet2 = GetImplementationLevel(module.c_str());
                    Assert::IsTrue(statusGet != OperationFailed);
                    Assert::IsTrue(statusGet == statusGet2);
                }
            }
        }
    };
}
