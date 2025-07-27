using Mediary.Core;

namespace Mediary.Dispatcher;

/// <summary>
/// Dispatches requests to their appropriate handlers, optionally with pipeline behaviors.
/// </summary>
public interface IRequestDispatcher
{
    /// <summary>
    /// Executes a request and returns a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="request">The request to execute.</param>
    /// <returns>A task containing the response.</returns>
    public Task<TResponse> DispatchAsync<TResponse, TRequest>(TRequest request)
        where TRequest : IRequest<TResponse>;
}
