
#include "stdafx.h"

#include "ImageProcessing.h"
#include "ModuleInterface.h"
#include "ConfigTestAdaptor.hpp"

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;

namespace ImageProcessingTests
{
    //
    // Tests conversion of a 32bpp BGRA image into 3 HSV channels
    //
    [TestClass]
    public ref class RGBToHSVTests
    {
    private:
        static uint8_t *bgra;
        static uint8_t *h, *s, *v, *hM, *sM, *vM;
        static const int32_t imageBGRASize = 640 * 480;
        static const int32_t imageBGRABufferSize = 640 * 480 * 4;
        static const int32_t outputBufferSize = imageBGRASize;
        static const char *THIS_MODULE = "Convert_32bgra_24hsv";

        static void AssertHSVCompare()
        {
            Assert::IsTrue(memcmp(h, hM, imageBGRASize) == 0, "Hue channel different from reference");
            Assert::IsTrue(memcmp(s, sM, imageBGRASize) == 0, "Saturation channel different from reference");
            Assert::IsTrue(memcmp(v, vM, imageBGRASize) == 0, "Value channel different from reference");
        }

    public:
        [ClassInitialize]
        static void InitializeImageData(TestContext^ context)
        {
            // seed PRNG to ensure reproducible results across runs
            srand(42);

            bgra = (uint8_t*)_mm_malloc(imageBGRABufferSize, 16);
            h = (uint8_t*)_mm_malloc(outputBufferSize, 16);
            s = (uint8_t*)_mm_malloc(outputBufferSize, 16);
            v = (uint8_t*)_mm_malloc(outputBufferSize, 16);
            hM = (uint8_t*)_mm_malloc(outputBufferSize, 16);
            sM = (uint8_t*)_mm_malloc(outputBufferSize, 16);
            vM = (uint8_t*)_mm_malloc(outputBufferSize, 16);

            for (auto i = 0; i < imageBGRABufferSize; i++) bgra[i] = rand() % 32767;
        }
        [ClassCleanup]
        static void CleanupImageDataMemory()
        {
            _mm_free(bgra);
            _mm_free(h);
            _mm_free(s);
            _mm_free(v);
            _mm_free(hM);
            _mm_free(sM);
            _mm_free(vM);
        }
        [TestInitialize]
        void ResetTestData()
        {
            std::fill(h, h + outputBufferSize, 42);
            std::fill(s, s + outputBufferSize, 42);
            std::fill(v, v + outputBufferSize, 42);
            std::fill(hM, hM + outputBufferSize, 42);
            std::fill(sM, sM + outputBufferSize, 42);
            std::fill(vM, vM + outputBufferSize, 42);
        }

        [TestMethod]
        void RgbToHsvReference()
        {
            int result = StatusCode::OperationSuccess;

            SetMultiThreadingStatus(THIS_MODULE, 0);
            SetImplementationLevel(THIS_MODULE, SIMDLevel::None);
            result = Convert_32bgra_24hsv(bgra, h, s, v, imageBGRABufferSize);
            Assert::IsTrue(result == StatusCode::OperationSuccess);

            SetMultiThreadingStatus(THIS_MODULE, 1);
            result = Convert_32bgra_24hsv(bgra, hM, sM, vM, imageBGRABufferSize);
            Assert::IsTrue(result == StatusCode::OperationSuccess);

            AssertHSVCompare();
        }

        [TestMethod]
        void RgbToHsvSIMD()
        {
            int result = StatusCode::OperationSuccess;

            // Reference
            SetMultiThreadingStatus(THIS_MODULE, 0);
            SetImplementationLevel(THIS_MODULE, SIMDLevel::None);
            result = Convert_32bgra_24hsv(bgra, h, s, v, imageBGRABufferSize);

            for (auto level : TestSetup::GetAllAvailableLevels(THIS_MODULE))
            {
                // SIMD 
                SetMultiThreadingStatus(THIS_MODULE, 0);
                SetImplementationLevel(THIS_MODULE, level);
                result = Convert_32bgra_24hsv(bgra, hM, sM, vM, imageBGRABufferSize);
                Assert::IsTrue(result == StatusCode::OperationSuccess);

                MEMCMP(h, hM, imageBGRASize);

                AssertHSVCompare();

                ResetTestData();
            }
        }

        [TestMethod]
        void RgbToHsvSIMDWithMultithreading()
        {
            int result = StatusCode::OperationSuccess;

            // Reference
            SetMultiThreadingStatus(THIS_MODULE, 0);
            SetImplementationLevel(THIS_MODULE, SIMDLevel::None);
            result = Convert_32bgra_24hsv(bgra, h, s, v, imageBGRABufferSize);

            for (auto level : TestSetup::GetAllAvailableLevels(THIS_MODULE))
            {
                // SIMD with Multithreading
                SetMultiThreadingStatus(THIS_MODULE, 1);
                SetImplementationLevel(THIS_MODULE, level);
                result = Convert_32bgra_24hsv(bgra, hM, sM, vM, imageBGRABufferSize);
                Assert::IsTrue(result == StatusCode::OperationSuccess);

                MEMCMP(h, hM, imageBGRASize);

                AssertHSVCompare();

                ResetTestData();
            }
        }
    };
}
