namespace ErrorOr;

/// <summary>
/// Provides factory methods for creating instances of <see cref="ErrorOr{TValue}"/>.
/// </summary>
public static class ErrorOrFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value.</returns>
    public static ErrorOr<TValue> From<TValue>(TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> from a tuple.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="tuple">The tuple containing the value and errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value or errors.</returns>
    public static ErrorOr<TValue> FromTuple<TValue>((TValue? Value, List<Error>? Errors) tuple)
    {
        return ErrorOr<TValue>.FromTuple(tuple);
    }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> from a custom object.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TCustom">The type of the custom object.</typeparam>
    /// <param name="customObject">The custom object containing the value and errors.</param>
    /// <param name="valueSelector">The function to select the value from the custom object.</param>
    /// <param name="errorsSelector">The function to select the errors from the custom object.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value or errors.</returns>
    public static ErrorOr<TValue> FromCustom<TValue, TCustom>(TCustom customObject, Func<TCustom, TValue?> valueSelector, Func<TCustom, List<Error>?> errorsSelector)
    {
        return ErrorOr<TValue>.FromCustom(customObject, valueSelector, errorsSelector);
    }
}
