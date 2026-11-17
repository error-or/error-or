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
    public IReadOnlyList<Error> All
    {
        get
        {
            if (_errors is not null)
            {
                return _errors;
            }

            return Array.Empty<Error>();
        }
    }

    internal List<Error>? ErrorList => _errors;
}
