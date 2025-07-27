namespace Mediary.Core.Results;

public readonly struct Created : IEquatable<Created>
{
    public static readonly Created Value = new();

    public override string ToString() => "Created";
    public override bool Equals(object? obj) => obj is Created;
    public bool Equals(Created other) => true;
    public override int GetHashCode() => 0;

    public static bool operator ==(Created left, Created right) => true;
    public static bool operator !=(Created left, Created right) => false;
}

