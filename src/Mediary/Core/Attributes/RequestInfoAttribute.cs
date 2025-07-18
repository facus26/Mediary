namespace Mediary.Core.Attributes;

/// <summary>
/// Provides descriptive metadata for a request type, including a human-readable description and optional tags.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class RequestInfoAttribute : Attribute
{
    /// <summary>
    /// A human-readable description of the request's purpose.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Optional tags that classify the request (e.g., "Query", "Command").
    /// </summary>
    public string[] Tags { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestInfoAttribute"/> class.
    /// </summary>
    /// <param name="description">The request description.</param>
    /// <param name="tags">Optional classification tags.</param>
    public RequestInfoAttribute(string description, params string[] tags)
    {
        Description = description;
        Tags = tags ?? Array.Empty<string>();
    }
}
