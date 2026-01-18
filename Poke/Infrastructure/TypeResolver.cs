using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Poke.Infrastructure;

/// <summary>
/// Resolves types from the configured service provider.
/// </summary>
public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider =
        provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>
    /// Resolves the requested type from the service provider.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>The resolved instance, if found.</returns>
    public object? Resolve(Type? type)
    {
        return type == null ? null : _provider.GetRequiredService(type);
    }

    /// <summary>
    /// Disposes the underlying service provider if it is disposable.
    /// </summary>
    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
