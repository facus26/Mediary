namespace Mediary.Core.Results;

/// <summary>
/// Indicates that a resource was successfully deleted. Used as a semantic response for commands.
/// </summary>
public readonly struct Deleted : IEquatable<Deleted>
{
    public static readonly Deleted Value = new();

    public override string ToString() => "Deleted";
    public override bool Equals(object? obj) => obj is Deleted;
    public bool Equals(Deleted other) => true;
    public override int GetHashCode() => 0;

    public static bool operator ==(Deleted left, Deleted right) => true;
    public static bool operator !=(Deleted left, Deleted right) => false;
}

