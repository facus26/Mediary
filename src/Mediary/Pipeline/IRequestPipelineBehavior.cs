using Mediary.Core;

namespace Mediary.Pipeline;

/// <summary>
/// Defines a pipeline behavior that wraps the handling of a request without a response.
/// This can be used for cross-cutting concerns like logging, validation, or transactions.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
public interface IRequestPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handles the behavior logic and invokes the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">A delegate representing the next step in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task HandleAsync(TRequest request, Func<Task> next);
}

/// <summary>
/// Defines a pipeline behavior that wraps the handling of a request with a response.
/// This can be used for cross-cutting concerns like logging, validation, caching, etc.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
public interface IRequestPipelineBehavior<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the behavior logic and invokes the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">A delegate representing the next step in the pipeline, which returns a response.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    public Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next);
}
