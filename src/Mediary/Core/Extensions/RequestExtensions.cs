using Mediary.Core.Attributes;
using Mediary.Dispatcher.Utils;

namespace Mediary.Core.Extensions;

/// <summary>
/// Extension methods for accessing metadata defined by <see cref="RequestInfoAttribute"/> on request types.
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// Gets the description of a request with response type from the <see cref="RequestInfoAttribute"/>, if available.
    /// </summary>
    public static string? GetDescription<TResponse>(
        this IRequest<TResponse> request
    ) => RequestInfo.GetDescription(request.GetType());

    /// <summary>
    /// Gets the tags of a request with response type from the <see cref="RequestInfoAttribute"/>, if available.
    /// </summary>
    public static string[] GetTags<TResponse>(
        this IRequest<TResponse> request
    ) => RequestInfo.GetTags(request.GetType());

    /// <summary>
    /// Gets the <see cref="RequestInfoAttribute"/> instance from a request with response type, if applied.
    /// </summary>
    public static RequestInfoAttribute? GetInfo<TResponse>(
        this IRequest<TResponse> request
    ) => RequestInfo.Get(request.GetType());

    /// <summary>
    /// Gets the description of a non-generic request from the <see cref="RequestInfoAttribute"/>, if available.
    /// </summary>
    public static string? GetDescription(
        this IRequest request
    ) => RequestInfo.GetDescription(request.GetType());

    /// <summary>
    /// Gets the tags of a non-generic request from the <see cref="RequestInfoAttribute"/>, if available.
    /// </summary>
    public static string[] GetTags(
        this IRequest request
    ) => RequestInfo.GetTags(request.GetType());

    /// <summary>
    /// Gets the <see cref="RequestInfoAttribute"/> instance from a non-generic request, if applied.
    /// </summary>
    public static RequestInfoAttribute? GetInfo(
        this IRequest request
    ) => RequestInfo.Get(request.GetType());
}

