//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_MODULE_SETUP_DETAILS
#define IMPROC_MODULE_SETUP_DETAILS

#include <tuple>

namespace Config
{
    // Namespace to hide these implementation details
    namespace Details
    {
        //! \brief Generic way to unpack the parameters for a given function (Ret(*func)(Args...))
        //!        using the void** v as an argument list. This works by recursively expanding the template
        //!        to a lower level until 0 is reached where the actual function call happens.
        //!        See: http://seanmiddleditch.com/c-runtime-invocation-of-bound-function-via-variadic-templates/
        template <unsigned N>
        struct apply
        {
            template <typename Ret, typename... Args, typename... ArgsT>
            static Ret call(Ret(*func)(Args...), void** v, ArgsT... args)
            {
                return apply<N - 1>::call(func, v, args...,
                    *static_cast<typename std::tuple_element < sizeof...(args),
                    std::tuple<Args... >> ::type*>(v[sizeof...(ArgsT)]));
            }
        };

        template<>
        struct apply<0>
        {
            template <typename Ret, typename... Args, typename... ArgsT>
            static Ret call(Ret(*func)(Args...), void** v, ArgsT... args)
            {
                return func(args...);
            }
        };

        template <typename Ret, typename... Args>
        Ret call(Ret(*func)(Args...), void** v)
        {
            return apply<sizeof...(Args)>::call(func, v);
        }
    }

    //! \brief Declares the given module and triggers the registration process
    //! \param [in] MODULE:      The signature of the module's function
    //! \param [in] _optionals_: The function argument list
    //! \remarks This macro is used at compile time to auto-register the module written by the programmer.
    //!          It creates a wrapper function over the real module function and instantiates a ModuleSetupPerformer instance
    //!          to do the right initialization (the module config).
#define IMPROC_MODULE(MODULE, ...)                              \
    namespace Config {                                          \
        int32_t MODULE##__WRAPPER(void** args) {                \
            return Details::call(MODULE, args);                 \
        }                                                       \
        ModuleSetupPerformer MODULE##__INITIALIZER(             \
            std::string(""#MODULE),                             \
            std::function<int32_t(void**)>(MODULE##__WRAPPER)); \
    }                                                           \
    IMPROC_CDECL int32_t MODULE(__VA_ARGS__)
}

#endif /* IMPROC_MODULE_SETUP_DETAILS */
