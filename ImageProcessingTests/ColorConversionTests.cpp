
#include "stdafx.h"
#include "..\ImageProcessing\ImageProcessing.h"
#include "..\ImageProcessing\GlobalConfig.h"

#include <stdlib.h>

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;

namespace ImageProcessingTests
{
    private class TestSetup
    {
    public:
        template<typename T>
        static T* GetRandomBuffer(size_t size)
        {
            T* buffer = new T[size];
            for (size_t i = 0; i < size; i++)
            {
                buffer[i] = rand() % 32767;
            }
            return buffer;
        }
        template<typename T>
        static T* GetZeroedBuffer(size_t size)
        {
            T* buffer = new T[size];
            for (size_t i = 0; i < size; i++)
            {
                buffer[i] = 0;
            }
            return buffer;
        }
    };

    [TestClass]
    public ref class ColorConversionTests
    {
    public:
        [TestMethod]
        void RgbToHsv()
        {
            auto IMAGE_SIZE = 640 * 480;

            auto bgra = TestSetup::GetRandomBuffer<uint8_t>(IMAGE_SIZE * 4);
            auto h = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            auto s = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            auto v = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            auto hM = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            auto sM = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            auto vM = TestSetup::GetZeroedBuffer<uint8_t>(IMAGE_SIZE);
            
            SetMultiThreadingStatus("Convert_32bgra_24hsv", 0);
            int resultScalar = Convert_32bgra_24hsv(bgra, h, s, v, IMAGE_SIZE);
            Assert::IsTrue(resultScalar == StatusCode::OperationSuccess, "Convert_32bgra_24hsv did not return OperationSuccess");
            
            SetMultiThreadingStatus("Convert_32bgra_24hsv", 1);
            auto resultMulticore = Convert_32bgra_24hsv(bgra, hM, sM, vM, IMAGE_SIZE);
            Assert::IsTrue(resultMulticore == StatusCode::OperationSuccess, "Convert_32bgra_24hsv[Multicore] did not return OperationSuccess");

            auto compareHue = memcmp(h, hM, IMAGE_SIZE);
            auto compareSaturation = memcmp(s, sM, IMAGE_SIZE);
            auto compareValue = memcmp(v, vM, IMAGE_SIZE);

            Assert::IsTrue(compareHue == 0, "Hue channel diferrent for Multicore version");
            Assert::IsTrue(compareSaturation == 0, "Saturation channel diferrent for Multicore version");
            Assert::IsTrue(compareValue == 0, "Value channel diferrent for Multicore version");

            delete[] bgra;
            delete[] h, delete[] hM, delete[] s, delete[] sM, delete[] v, delete[] vM;
        }
    };
}
