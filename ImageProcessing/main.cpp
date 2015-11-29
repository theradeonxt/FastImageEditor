#include "ImageProcessing.h"
#include "CpuRdtsc.hpp"
#include <iostream>
#include <random>
#include <stdlib.h>

#define ITERATIONS 3
#define WIDTH 1920 //7680
#define HEIGHT 1080 //4320
#define SIZE_BYTES (WIDTH * HEIGHT * 4)

void fillRandom(uint8_t* data, uint32_t size)
{
    for (uint32_t i = 0; i < size; i += 4)
    {
        *(uint32_t*) (data + i) = (rand() % 256);
    }
}

int main()
{
    uint64_t start, end;
    uint64_t average = 0;

    std::cout << "Allocating memory: for 2 images: " << WIDTH << "x" << HEIGHT
            << "..." << std::endl;
    start = rdtsc();

    uint8_t* src = new uint8_t[SIZE_BYTES];
    uint8_t* dst = new uint8_t[SIZE_BYTES];

    if (!src || !dst)
        return -1;

    end = rdtsc();
    std::cout << "Allocated memory: " << SIZE_BYTES * 2.0f / 1024 / 1024 << "MB"
            << ". Elapsed(clocks): " << (end - start) << std::endl;
    std::cout << "--------------------------------" << std::endl;

    for (int i = 1; i <= ITERATIONS; i++)
    {
        fillRandom(src, SIZE_BYTES);
        fillRandom(dst, SIZE_BYTES);

        std::cout << "Running AlphaBlend32bgra_32bgra..." << std::endl;
        start = rdtsc();

        int ret = AlphaBlend32bgra_32bgra(src, dst, dst, SIZE_BYTES);

        end = rdtsc();
        std::cout << "Completed AlphaBlend32bgra_32bgra (ret=" << ret
                << "). Elapsed(clocks): " << (end - start) << std::endl;
        average += (end - start);
    }

    std::cout << "--------------------------------" << std::endl;
    std::cout << "Average Elapsed(clocks): " << average / ITERATIONS
            << std::endl;

    delete[] src;
    delete[] dst;

    return 0;
}
