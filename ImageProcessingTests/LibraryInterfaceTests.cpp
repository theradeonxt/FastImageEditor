
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
        [TestMethod]
        void MulticoreStatusChange()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto statusMT = GetMultiThreadingStatus(module.c_str());
                auto newStatusMT = !statusMT;

                auto result = SetMultiThreadingStatus(module.c_str(), newStatusMT);
                Assert::IsTrue(result == StatusCode::OperationSuccess);
                Assert::IsTrue(result != StatusCode::OperationFailed);

                auto statusMTAfter = GetMultiThreadingStatus(module.c_str());
                Assert::IsTrue(statusMT != statusMTAfter);
            }
        }

        [TestMethod]
        void BaselineImplementationQuery()
        {
            auto& moduleList = TestSetup::GetModuleList();
            for (auto& module : moduleList)
            {
                auto status = QueryAvailableImplementation(module.c_str(), SIMDLevel::None);
                Assert::IsTrue(status != StatusCode::OperationFailed);
                Assert::IsTrue(status == 1);

                status = QueryAvailableImplementation(module.c_str(), SIMDLevel::SSE2);
                Assert::IsTrue(status != StatusCode::OperationFailed);
                Assert::IsTrue(status == 1);
            }
        }
    };
}
