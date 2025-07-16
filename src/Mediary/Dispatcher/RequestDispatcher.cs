using Mediary.Core;
using Mediary.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Mediary.Dispatcher;

/// <summary>
/// Default implementation of <see cref="IRequestDispatcher"/> using dependency injection
/// to resolve request handlers and pipeline behaviors.
/// </summary>
public sealed class RequestDispatcher : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestDispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for resolving handlers and behaviors.</param>
    public RequestDispatcher(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public async Task DispatchAsync<TRequest>(TRequest request)
        where TRequest : IRequest
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        var behaviors = _serviceProvider.GetServices<IRequestPipelineBehavior<TRequest>>().ToList();

        await BehaviorInvoker.ExecuteWithPipeline(request, handler, behaviors);
    }

    /// <inheritdoc />
    public async Task<TResponse> ExecuteAsync<TResponse, TRequest>(TRequest request)
        where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TResponse, TRequest>>();
        var behaviors = _serviceProvider.GetServices<IRequestPipelineBehavior<TResponse, TRequest>>().ToList();

        return await BehaviorInvoker.ExecuteWithPipeline(request, handler, behaviors);
    }
}
