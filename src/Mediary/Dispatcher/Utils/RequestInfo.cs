using System.Reflection;
using Mediary.Core.Attributes;

namespace Mediary.Dispatcher.Utils;

/// <summary>
/// Helper methods to retrieve metadata from <see cref="RequestInfoAttribute"/> applied to request types.
/// </summary>
public static class RequestInfo
{
    /// <summary>
    /// Gets the <see cref="RequestInfoAttribute"/> from a given request type, if present.
    /// </summary>
    public static RequestInfoAttribute? Get(Type handlerType) =>
        handlerType.GetCustomAttribute<RequestInfoAttribute>(inherit: true);

    /// <summary>
    /// Gets the description from the <see cref="RequestInfoAttribute"/> on the request type.
    /// </summary>
    public static string? GetDescription(Type handlerType) =>
        Get(handlerType)?.Description;

    /// <summary>
    /// Gets the tags from the <see cref="RequestInfoAttribute"/> on the request type.
    /// </summary>
    public static string[] GetTags(Type handlerType) =>
        Get(handlerType)?.Tags ?? Array.Empty<string>();
}
