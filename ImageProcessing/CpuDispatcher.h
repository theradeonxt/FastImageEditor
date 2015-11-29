
#ifndef IMAGEPROCESSING_CPUDISPATCHER_H
#define IMAGEPROCESSING_CPUDISPATCHER_H

//
// Utility macros
//
#define REFERENCE_IMPL(FUNCTION_NAME, ...) FUNCTION_NAME##_ref(__VA_ARGS__)
#define REFERENCE_IMPL_MT(FUNCTION_NAME, ...) FUNCTION_NAME##_ref##_MT(__VA_ARGS__)

#define SIMD_IMPL(REFERENCE_PART, SIMD_PART, ...) REFERENCE_PART##SIMD_PART(__VA_ARGS__)
#define SIMD_IMPL_MT(REFERENCE_PART, SIMD_PART, ...) REFERENCE_PART##SIMD_PART##_MT(__VA_ARGS__)

#define STRINGIFY(NAME) #NAME

#define DISPATCH_REFERENCE_MT(FUNCTION_NAME, ...)         \
    config->isMultiThreadEnabled() ?                      \
        REFERENCE_IMPL_MT(FUNCTION_NAME, __VA_ARGS__) :   \
        REFERENCE_IMPL(FUNCTION_NAME, __VA_ARGS__) 

#define DISPATCH_SIMD_MT(FUNCTION_NAME, SIMD_PART, ...)         \
    config->isMultiThreadEnabled() ?                            \
        SIMD_IMPL_MT(FUNCTION_NAME, SIMD_PART, __VA_ARGS__) :   \
        SIMD_IMPL(FUNCTION_NAME, SIMD_PART, __VA_ARGS__) 

//
// Get runtime config for given module and perform runtime CPU dispatching
//
#define CPU_DISPATCH_LOGIC(MODULE_NAME, ...)                \
Config::SingleConfig* config = nullptr;                     \
bool configFound = Config::g_ModulesConfig.getSingleConfig( \
    STRINGIFY(MODULE_NAME), config);                        \
int32_t returnCode = OperationFailed;                       \
if (configFound) {                                          \
    switch (config->getSIMDLevel()) {                       \
    case None:                                              \
        return DISPATCH_REFERENCE_MT(MODULE_NAME, __VA_ARGS__); \
    case SSE2:                                              \
        returnCode = DISPATCH_SIMD_MT(MODULE_NAME, _SSE2, __VA_ARGS__);  \
        break;                                              \
    case SSSE3:                                             \
        returnCode = DISPATCH_SIMD_MT(MODULE_NAME, _SSSE3, __VA_ARGS__);  \
        break;                                              \
    case AVX:                                               \
        returnCode = DISPATCH_SIMD_MT(MODULE_NAME, _AVX, __VA_ARGS__);  \
        break;                                              \
    case FMA3:                                              \
        returnCode = DISPATCH_SIMD_MT(MODULE_NAME, _FMA3, __VA_ARGS__);  \
        break;                                              \
    }                                                       \
}

#define READ_DISPATCH_RESULT() returnCode

#endif /* IMAGEPROCESSING_CPUDISPATCHER_H */
