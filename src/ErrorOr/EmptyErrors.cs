namespace ErrorOr;

internal static class EmptyErrors
{
    public static ReadOnlyCollection<Error> Instance { get; } = new ReadOnlyCollection<Error>([]);
}
