namespace ErrorOr;

/// <summary>
/// Represents the errors in an <see cref="ErrorOr{TValue}"/>.
/// </summary>
public readonly struct Errors
{
    private readonly List<Error>? _errors;

    internal Errors(List<Error> errors) => _errors = errors;

    /// <summary>
    /// Gets the first error, or the no-first-error sentinel when there are no errors.
    /// </summary>
    public Error First => _errors is { Count: > 0 } ? _errors[0] : KnownErrors.NoFirstError;

    /// <summary>
    /// Gets all errors.
    /// </summary>
    public IReadOnlyList<Error> All => _errors ?? [];

    internal List<Error>? ErrorList => _errors;

    /// <summary>
    /// Creates an <see cref="Errors"/> value from a single error.
    /// </summary>
    public static implicit operator Errors(Error error) => new([error]);

    /// <summary>
    /// Creates an <see cref="Errors"/> value from a list of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="errors"/> is null.</exception>
    public static implicit operator Errors(List<Error> errors)
    {
        if (errors is null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        return new(errors);
    }

    /// <summary>
    /// Creates an <see cref="Errors"/> value from an array of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="errors"/> is null.</exception>
    public static implicit operator Errors(Error[] errors)
    {
        if (errors is null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        return new([.. errors]);
    }
}
