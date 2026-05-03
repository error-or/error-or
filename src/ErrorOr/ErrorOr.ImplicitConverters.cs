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
    public static implicit operator ErrorOr<TValue>(List<Error> errors) => errors?.Count > 0 ? new(errors) : new(KnownErrors.CachedInvalidInitialErrorsList);

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error[] errors) => errors?.Length > 0 ? new([.. errors]) : new(KnownErrors.CachedInvalidInitialErrorsList);
}
