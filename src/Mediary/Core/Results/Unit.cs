namespace Mediary.Core.Results;

public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = new();

    public override string ToString() => "()";
    public override bool Equals(object? obj) => obj is Unit;
    public bool Equals(Unit other) => true;
    public override int GetHashCode() => 0;

    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
}

