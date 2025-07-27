namespace Mediary.Core.Results;

public readonly struct Updated : IEquatable<Updated>
{
    public static readonly Updated Value = new();

    public override string ToString() => "Updated";
    public override bool Equals(object? obj) => obj is Updated;
    public bool Equals(Updated other) => true;
    public override int GetHashCode() => 0;

    public static bool operator ==(Updated left, Updated right) => true;
    public static bool operator !=(Updated left, Updated right) => false;
}

