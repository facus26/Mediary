namespace Mediary.Core.Results;

/// <summary>
/// Represents a generic successful result for a command that does not return data.
/// </summary>
public readonly struct Success : IEquatable<Success>
{
    public static readonly Success Value = new();

    public override string ToString() => "Success";
    public override bool Equals(object? obj) => obj is Success;
    public bool Equals(Success other) => true;
    public override int GetHashCode() => 0;

    public static bool operator ==(Success left, Success right) => true;
    public static bool operator !=(Success left, Success right) => false;
}

