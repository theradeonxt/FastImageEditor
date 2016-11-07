//! =============================================================================
//! \file 
//! \brief 
//!     This is a sample description
//!
//! =============================================================================

#ifndef IMPROC_DISPATCH_MANAGER_H
#define IMPROC_DISPATCH_MANAGER_H

//#include "Common.h"
#include "HelpersSIMD.hpp"

#include <functional>
#include <map>
#include <cstdint>

template<typename T = int>
class AbstractArgument
{
protected:
    AbstractArgument(T item)
        :m_Item(item)
    {
    }

public:
    virtual ~AbstractArgument() {}
    virtual void apply(int inc) = 0;
    virtual bool isEmpty() const = 0;

    T m_Item;
};

template<typename T = int>
class DefaultArgument : public AbstractArgument<T>
{
public:
    explicit DefaultArgument()
        : AbstractArgument<T>(T())
    {
    }

    void apply(int inc) override { ; }
    bool isEmpty() const override { return true; }
};

template<typename T = int>
class AdressableArgument : public AbstractArgument<T>
{
public:
    explicit AdressableArgument(T item)
        : AbstractArgument<T>(item)
    {
    }

    void apply(int inc) override { m_Item += inc; }
    bool isEmpty() const override { return false; }
};

template<typename T = int>
class ValueArgument : public AbstractArgument<T>
{
public:
    explicit ValueArgument(T item)
        : AbstractArgument<T>(item)
    {
    }

    void apply(int inc) override { ; }
    bool isEmpty() const override { return false; }
};

template<typename T = int>
class SizeArgument : public AbstractArgument<T>
{
public:
    explicit SizeArgument(T item)
        : AbstractArgument<T>(item)
    {
    }

    void apply(int inc) override { m_Item -= inc; }
    bool isEmpty() const override { return false; }
};


#define ADR_ARG(X) ArgumentFactory::makeAdressable(X)
#define VAL_ARG(X) ArgumentFactory::makeValue(X)
#define SZ_ARG(X) ArgumentFactory::makeSize(X)

class DispatchManager
{
public:
    void Register(std::string entryPointKey, std::function<int(void**)> entryPoint);

    //! \brief Executes the requested module using its current configuration.
    //! \param [in] moduleName The module name.
    //! \param [in] args Pointer to argument array pased to the module.
    //! \returns A StatusCode value with the status of the operation.
    int32_t Run(std::string moduleName, void** args);

    //! \brief Executes the requested module using reference implementation,
    //!        meaning SIMDLevel::None and with multithreading disabled.
    //! \param [in] moduleName The module name.
    //! \param [in] args Pointer to argument array pased to the module.
    //! \returns A StatusCode value with the status of the operation.
    int32_t RunFallback(const std::string& moduleName, void** args);

private:
    std::map<std::string, std::function<int(void**)>> dispatcher;

    bool ContainsKey(const std::string& key);
};

//! \brief Single instance DispatchManager object.
extern DispatchManager& g_DispatchManager();

class ArgumentFactory
{
public:
    template<typename T>
    static AdressableArgument<T> makeAdressable(T item)
    {
        return AdressableArgument<T>(item);
    }

    template<typename T>
    static ValueArgument<T> makeValue(T item)
    {
        return ValueArgument<T>(item);
    }

    template<typename T>
    static SizeArgument<T> makeSize(T item)
    {
        return SizeArgument<T>(item);
    }
};

enum AlignPolicy { PreferAlignment, IgnoreAlignment };
enum SIMDPolicy { PreferSIMD, IgnoreSIMD };

template<SIMDPolicy simd, AlignPolicy alignment>
struct Runner{};

struct RunnerBase
{
    RunnerBase(const std::string& moduleName)
        : m_ModuleName(moduleName)
    {
    }

    virtual ~RunnerBase()
    {
    }

    virtual int32_t Run(void** args) = 0;

    template<typename T1,
        typename T2 = int, typename T3 = int, typename T4 = int,
        typename T5 = int, typename T6 = int, typename T7 = int, typename T8 = int>
        int32_t RunFrom(uint32_t position, uint32_t sizeBytes,
        AbstractArgument<T1>& t1,
        AbstractArgument<T2>& t2 = DefaultArgument<>(), AbstractArgument<T3>& t3 = DefaultArgument<>(),
        AbstractArgument<T4>& t4 = DefaultArgument<>(), AbstractArgument<T5>& t5 = DefaultArgument<>(),
        AbstractArgument<T6>& t6 = DefaultArgument<>(), AbstractArgument<T7>& t7 = DefaultArgument<>(),
        AbstractArgument<T8>& t8 = DefaultArgument<>())
    {
        // count real arguments
        int count = 1;
        if (!t2.isEmpty()){ count++; }
        if (!t3.isEmpty()){ count++; }
        if (!t4.isEmpty()){ count++; }
        if (!t5.isEmpty()){ count++; }
        if (!t6.isEmpty()){ count++; }
        if (!t7.isEmpty()){ count++; }
        if (!t8.isEmpty()){ count++; }

        // pack arguments into a void* array
        void** args = new void*[count];

        t1.apply(position); args[0] = &t1.m_Item;
        if (!t2.isEmpty()){ t2.apply(position); args[1] = &t2.m_Item; }
        if (!t3.isEmpty()){ t3.apply(position); args[2] = &t3.m_Item; }
        if (!t4.isEmpty()){ t4.apply(position); args[3] = &t4.m_Item; }
        if (!t5.isEmpty()){ t5.apply(position); args[4] = &t5.m_Item; }
        if (!t6.isEmpty()){ t6.apply(position); args[5] = &t6.m_Item; }
        if (!t7.isEmpty()){ t7.apply(position); args[6] = &t7.m_Item; }
        if (!t8.isEmpty()){ t8.apply(position); args[7] = &t8.m_Item; }

        auto result = Run(args);

        // TODO: Stack Overflow for sizeBytes = 12
        // Handle non-multiple of SIMD size images using reference implementation
        /*auto remainder = sizeBytes % SIMD_SIZE;
        if (remainder != 0 && // if remainder is not 0, leftover bytes to process
            position == 0)    // if position is 0 to prevent further calls to Runner<IgnoreSIMD, IgnoreAlignment>
        {
            auto offset = sizeBytes - remainder;
            Runner<IgnoreSIMD, IgnoreAlignment>(m_ModuleName).RunFrom(offset, remainder, t1, t2, t3, t4, t5, t6, t7, t8);
        }*/

        delete[] args;

        return result;
    }

    const std::string& m_ModuleName;
};

template<>
struct Runner<PreferSIMD, PreferAlignment> : RunnerBase
{
    Runner(const std::string& moduleName)
    : RunnerBase(moduleName)
    {
    }

    int32_t Run(void** args) override
    {
        return g_DispatchManager().Run(m_ModuleName, args);
    }
};

template<>
struct Runner<IgnoreSIMD, IgnoreAlignment> :RunnerBase
{
    Runner(const std::string& moduleName)
    : RunnerBase(moduleName)
    {
    }

    int32_t Run(void** args) override
    {
        return g_DispatchManager().RunFallback(m_ModuleName, args);
    }
};

#endif /* IMPROC_DISPATCH_MANAGER_H */
