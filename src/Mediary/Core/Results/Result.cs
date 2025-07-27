namespace Mediary.Core.Results;

/// <summary>
/// Provides static access to semantic result types such as <see cref="Unit"/>, <see cref="Success"/>, <see cref="Created"/>, etc.
/// </summary>
public static class Result
{
    public static Unit Unit => Unit.Value;
    public static Success Success => Success.Value;
    public static Created Created => Created.Value;
    public static Updated Updated => Updated.Value;
    public static Deleted Deleted => Deleted.Value;
}
