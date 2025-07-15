using Mediary.Core;

namespace Mediary.Pipeline;

/// <summary>
/// Static helper to execute a request handler within a pipeline of behaviors.
/// It builds the invocation chain by wrapping the handler call with the behaviors in order.
/// </summary>
public static class BehaviorInvoker
{
    /// <summary>
    /// Executes a request without response, invoking the pipeline behaviors in order,
    /// and finally the request handler.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="handler">The handler to execute at the end of the pipeline.</param>
    /// <param name="behaviors">The pipeline behaviors to apply around the handler.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task ExecuteWithPipeline<TRequest>(
        TRequest request,
        IRequestHandler<TRequest> handler,
        IEnumerable<IRequestPipelineBehavior<TRequest>> behaviors)
        where TRequest : IRequest
    {
        var pipeline = () => handler.HandleAsync(request);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = () => behavior.HandleAsync(request, next);
        }

        return pipeline();
    }

    /// <summary>
    /// Executes a request with response, invoking the pipeline behaviors in order,
    /// and finally the request handler.
    /// </summary>
    /// <typeparam name="TResponse">The response type returned by the handler.</typeparam>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="handler">The handler to execute at the end of the pipeline.</param>
    /// <param name="behaviors">The pipeline behaviors to apply around the handler.</param>
    /// <returns>A task containing the response of type <typeparamref name="TResponse"/>.</returns>
    public static Task<TResponse> ExecuteWithPipeline<TRequest, TResponse>(
        TRequest request,
        IRequestHandler<TResponse, TRequest> handler,
        IEnumerable<IRequestPipelineBehavior<TResponse, TRequest>> behaviors)
        where TRequest : IRequest<TResponse>
    {
        var pipeline = () => handler.HandleAsync(request);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = () => behavior.HandleAsync(request, next);
        }

        return pipeline();
    }
}
