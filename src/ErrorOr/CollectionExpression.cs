namespace ErrorOr;

/// <summary>
/// Contains methods supporting collection expressions.
/// </summary>
public static class CollectionExpression
{
    /// <summary>
    /// Creates <see cref="ErrorOr{TValue}"/> from read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static ErrorOr<TValue> CreateErrorOr<TValue>(ReadOnlySpan<Error> errors)
    {
        return errors.ToArray();
    }

    /// <summary>
    /// Creates <see cref="IErrorOr{TValue}"/> from read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static IErrorOr<TValue> CreateIErrorOrValue<TValue>(ReadOnlySpan<Error> errors)
    {
        return CreateErrorOr<TValue>(errors);
    }

    /// <summary>
    /// Creates <see cref="IErrorOr"/> from read-only span of errors.
    /// </summary>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>Error or vale.</returns>
    /// <remarks>Enables support for collection expressions.</remarks>
    public static IErrorOr CreateIErrorOr(ReadOnlySpan<Error> errors)
    {
        return CreateErrorOr<object>(errors);
    }
}
