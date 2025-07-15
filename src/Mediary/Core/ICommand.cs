namespace Mediary.Core;

/// <summary>
/// Represents a write command that does not return a value.
/// Typically used to change system state (create, update, delete).
/// </summary>
public interface ICommand : IRequest { }

/// <summary>
/// Represents a write command that returns a response of type <typeparamref name="TResponse"/>.
/// Typically used to change system state and return a result.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the command.</typeparam>
public interface ICommand<TResponse> : IRequest<TResponse> { }
