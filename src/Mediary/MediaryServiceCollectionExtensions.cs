using Mediary.Dispatcher;
using Microsoft.Extensions.DependencyInjection;

namespace Mediary;

/// <summary>
/// Extension methods for registering Mediary core services into the dependency injection container.
/// </summary>
public static class MediaryServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default <see cref="IRequestDispatcher"/> and returns a builder for configuring Mediary.
    /// </summary>
    /// <param name="services">The service collection to add Mediary to.</param>
    /// <returns>A <see cref="MediaryBuilder"/> to further configure request handlers and pipeline behaviors.</returns>
    public static MediaryBuilder AddMediary(this IServiceCollection services)
    {
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();
        var builder = new MediaryBuilder(services);
        return builder;
    }

    /// <summary>
    /// Registers a custom implementation of <see cref="IRequestDispatcher"/> and returns a builder for configuring Mediary.
    /// </summary>
    /// <typeparam name="TImplementation">The custom implementation of <see cref="IRequestDispatcher"/> to use.</typeparam>
    /// <param name="services">The service collection to add Mediary to.</param>
    /// <returns>A <see cref="MediaryBuilder"/> to further configure request handlers and pipeline behaviors.</returns>
    public static MediaryBuilder AddMediary<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IRequestDispatcher
    {
        services.AddScoped<IRequestDispatcher, TImplementation>();
        var builder = new MediaryBuilder(services);
        return builder;
    }
}

