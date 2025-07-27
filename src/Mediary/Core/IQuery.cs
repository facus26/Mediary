namespace Mediary.Core;

/// <summary>
/// Represents a read-only query that returns a response of type <typeparamref name="TResponse"/>.
/// Typically used to retrieve data without causing side effects.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the query.</typeparam>
public interface IQuery<TResponse> : IRequest<TResponse> { }
