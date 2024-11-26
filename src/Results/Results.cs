namespace ErrorOr;

#pragma warning disable SA1649 // File name should match first type name
public readonly record struct Success;
#pragma warning restore SA1649 // File name should match first type name
public readonly record struct Created;
public readonly record struct Deleted;
public readonly record struct Updated;
public readonly record struct Accepted;
public readonly record struct NoContent;

public static class Result
{
    public static Success Success => default;

    public static Created Created => default;

    public static Deleted Deleted => default;

    public static Updated Updated => default;

    public static Accepted Accepted => default;

    public static NoContent NoContent => default;
}
