namespace Mediary.Core;

/// <summary>
/// Represents a request that returns a response of type <typeparamref name="TResponse"/>.
/// Base interface for queries and commands with results.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the request.</typeparam>
public interface IRequest<TResponse> { }
