using System.Reflection;
using Mediary.Core;
using Mediary.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Mediary;

/// <summary>
/// Provides a fluent interface for configuring and registering Mediary request handlers and pipeline behaviors
/// into the dependency injection container.
/// </summary>
public sealed class MediaryBuilder
{
    /// <summary>
    /// The underlying <see cref="IServiceCollection"/> used for registration.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaryBuilder"/> class with the specified services collection.
    /// </summary>
    /// <param name="services">The service collection to add registrations to.</param>
    public MediaryBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Registers a single non-generic request handler for a specific request type.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation of the handler.</typeparam>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddRequestHandler<TRequest, TImplementation>()
        where TRequest : IRequest
        where TImplementation : class, IRequestHandler<TRequest>
    {
        Services.AddScoped<IRequestHandler<TRequest>, TImplementation>();
        return this;
    }

    /// <summary>
    /// Registers a request handler with a response for a specific request type.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation of the handler.</typeparam>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddRequestHandler<TResponse, TRequest, TImplementation>()
        where TRequest : IRequest<TResponse>
        where TImplementation : class, IRequestHandler<TResponse, TRequest>
    {
        Services.AddScoped<IRequestHandler<TResponse, TRequest>, TImplementation>();
        return this;
    }

    /// <summary>
    /// Registers a pipeline behavior for a non-generic request type.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation of the behavior.</typeparam>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddPipelineBehaviors<TRequest, TImplementation>()
        where TImplementation : class, IRequestPipelineBehavior<TRequest>
        where TRequest : IRequest
    {
        Services.AddScoped<IRequestPipelineBehavior<TRequest>, TImplementation>();
        return this;
    }

    /// <summary>
    /// Registers a pipeline behavior for a request type that returns a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation of the behavior.</typeparam>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddPipelineBehaviors<TResponse, TRequest, TImplementation>()
        where TImplementation : class, IRequestPipelineBehavior<TResponse, TRequest>
        where TRequest : IRequest<TResponse>
    {
        Services.AddScoped<IRequestPipelineBehavior<TResponse, TRequest>, TImplementation>();
        return this;
    }

    /// <summary>
    /// Registers an open generic pipeline behavior (e.g., LoggingBehavior&lt;,&gt;) 
    /// for all compatible request types globally.
    /// </summary>
    /// <param name="implementationType">The open generic type implementing IRequestPipelineBehavior&lt;&gt; or IRequestPipelineBehavior&lt;,&gt;.</param>
    /// <returns>The current <see cref="MediaryBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the provided type does not implement a valid pipeline behavior interface.</exception>
    public MediaryBuilder AddOpenPipelineBehaviors(Type implementationType)
    {
        var interfaces = implementationType.GetInterfaces()
            .Where(iface => iface.IsGenericType &&
                (iface.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<>) ||
                 iface.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<,>)))
            .Where(iface => iface.ContainsGenericParameters)
            .Distinct()
            .ToList();

        if (!interfaces.Any())
            throw new InvalidOperationException($"{implementationType.Name} does not implement any valid IRequestPipelineBehavior interface.");

        foreach (var iface in interfaces)
            Services.AddScoped(iface.GetGenericTypeDefinition(), implementationType.GetGenericTypeDefinition());

        return this;
    }

    /// <summary>
    /// Registers all request handlers found in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for handler implementations.</param>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddRequestHandlersFromAssembly(Assembly assembly)
    {
        var handlerTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x => x.iface.IsGenericType &&
                        (x.iface.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                         x.iface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

        foreach (var reg in handlerTypes)
        {
            Services.AddScoped(reg.iface, reg.impl);
        }

        return this;
    }

    /// <summary>
    /// Registers all pipeline behaviors found in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for pipeline behavior implementations.</param>
    /// <returns>The current instance of <see cref="MediaryBuilder"/>.</returns>
    public MediaryBuilder AddPipelineBehaviorsFromAssembly(Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x => x.iface.IsGenericType &&
                (x.iface.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<>) ||
                 x.iface.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<,>)))
            .ToList();

        foreach (var reg in types)
        {
            if (reg.iface.ContainsGenericParameters)
                Services.AddScoped(reg.iface.GetGenericTypeDefinition(), reg.impl.GetGenericTypeDefinition());
            else
                Services.AddScoped(reg.iface, reg.impl);
        }

        return this;
    }

}
