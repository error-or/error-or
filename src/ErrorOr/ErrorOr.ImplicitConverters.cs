namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a value.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an error.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error error) => new(error);

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty list.</exception>
    public static implicit operator ErrorOr<TValue>(List<Error> errors) =>
        errors is null
            ? throw new ArgumentNullException(nameof(errors))
            : new ErrorOr<TValue>([.. errors], nameof(errors));

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an array of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static implicit operator ErrorOr<TValue>(Error[] errors) =>
        errors is null
            ? throw new ArgumentNullException(nameof(errors))
            : new ErrorOr<TValue>([.. errors], nameof(errors));

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an <see cref="ReadOnlyCollection{Error}"/> of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static implicit operator ErrorOr<TValue>(ReadOnlyCollection<Error> errors) =>
        errors is null
            ? throw new ArgumentNullException(nameof(errors))
            : new ErrorOr<TValue>([.. errors], nameof(errors));
}
