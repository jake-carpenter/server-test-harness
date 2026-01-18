using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Poke.Infrastructure;

/// <summary>
/// Registers services for Spectre.Console's dependency injection.
/// </summary>
public sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    /// <summary>
    /// Builds the type resolver from the registered services.
    /// </summary>
    /// <returns>The type resolver.</returns>
    public ITypeResolver Build()
    {
        return new TypeResolver(services.BuildServiceProvider());
    }

    /// <summary>
    /// Registers a service and implementation pair.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="implementation">The implementation type.</param>
    public void Register(Type service, Type implementation)
    {
        services.AddSingleton(service, implementation);
    }

    /// <summary>
    /// Registers an existing service instance.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="implementation">The service instance.</param>
    public void RegisterInstance(Type service, object implementation)
    {
        services.AddSingleton(service, implementation);
    }

    /// <summary>
    /// Registers a lazy factory for a service.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="func">The factory function.</param>
    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
            throw new ArgumentNullException(nameof(func));

        services.AddSingleton(service, func);
    }
}
