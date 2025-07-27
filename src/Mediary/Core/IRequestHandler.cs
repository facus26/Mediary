namespace Mediary.Core;

/// <summary>
/// Defines a handler for a request that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
/// <typeparam name="TRequest">The request type handled.</typeparam>
public interface IRequestHandler<TResponse, TRequest>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request and returns a response.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <returns>A task representing the asynchronous operation, with a response of type <typeparamref name="TResponse"/>.</returns>
    public Task<TResponse> HandleAsync(TRequest request);
}
