using System.Diagnostics.CodeAnalysis;

namespace ErrorOr;

/// <summary>
/// A discriminated union of errors or a value.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="Value"/>.</typeparam>
public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    private readonly TValue? _value = default;
    private readonly List<Error>? _errors = null;

    private ErrorOr(Error error)
    {
        _errors = [error];
    }

    private ErrorOr(List<Error> errors)
    {
        if (errors is null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        if (errors.Count == 0)
        {
            throw new ArgumentException("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error.", nameof(errors));
        }

        _errors = errors;
    }

    private ErrorOr(TValue value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value;
    }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    [MemberNotNullWhen(true, nameof(_errors))]
    [MemberNotNullWhen(true, nameof(Errors))]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsError => _errors is not null;

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will contain a single error representing the state.
    /// </summary>
    public List<Error> Errors => IsError ? _errors : KnownErrors.CachedNoErrorsList;

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will be empty.
    /// </summary>
    public List<Error> ErrorsOrEmptyList => IsError ? _errors : KnownErrors.CachedEmptyErrorsList;

    /// <summary>
    /// Gets the value.
    /// </summary>
    public TValue Value => _value!;

    /// <summary>
    /// Gets the first error.
    /// </summary>
    public Error FirstError
    {
        get
        {
            if (!IsError)
            {
                return KnownErrors.NoFirstError;
            }

            return _errors[0];
        }
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    [Obsolete("ErrorOrFactory.From<TValue>(errors) should be used instead.")]
    public static ErrorOr<TValue> From(List<Error> errors)
    {
        return errors;
    }
}
