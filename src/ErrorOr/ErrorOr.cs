using System.Diagnostics.CodeAnalysis;

using InnerErrors = System.Collections.Generic.List<ErrorOr.Error>;

namespace ErrorOr;

/// <summary>
/// A discriminated union of errors or a value.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="Value"/>.</typeparam>
public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    private readonly TValue? _value = default;
    private readonly InnerErrors? _innerErrors = null;
    private readonly ReadOnlyCollection<Error>? _errors = null;

    /// <summary>
    /// Prevents a default <see cref="ErrorOr"/> struct from being created.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when this method is called.</exception>
    public ErrorOr()
    {
        throw new InvalidOperationException("Default construction of ErrorOr<TValue> is invalid. Please use provided factory methods to instantiate.");
    }

    private ErrorOr(Error error)
    {
        _innerErrors = [error];
        _errors = new ReadOnlyCollection<Error>(_innerErrors);
    }

    private ErrorOr(InnerErrors innerErrors, string paramName)
    {
        if (innerErrors.Count == 0)
        {
            throw new ArgumentException("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error.", paramName);
        }

        _innerErrors = innerErrors;
        _errors = new ReadOnlyCollection<Error>(_innerErrors);
    }

    private ErrorOr(InnerErrors innerErrors, ReadOnlyCollection<Error> errors)
    {
        _innerErrors = innerErrors;
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
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(_value))]
    [MemberNotNullWhen(true, nameof(Errors))]
    [MemberNotNullWhen(true, nameof(_errors))]
    [MemberNotNullWhen(true, nameof(_innerErrors))]
    public bool IsError => _errors is not null;

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will contain a single error representing the state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public ReadOnlyCollection<Error> Errors => IsError ? _errors : throw new InvalidOperationException("The Errors property cannot be accessed when no errors have been recorded. Check IsError before accessing Errors.");

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will be empty.
    /// </summary>
    public ReadOnlyCollection<Error> ErrorsOrEmptyList => IsError ? _errors : EmptyErrors.Instance;

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no value is present.</exception>
    public TValue Value
    {
        get
        {
            if (IsError)
            {
                throw new InvalidOperationException("The Value property cannot be accessed when errors have been recorded. Check IsError before accessing Value.");
            }

            return _value;
        }
    }

    /// <summary>
    /// Gets the first error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public Error FirstError
    {
        get
        {
            if (!IsError)
            {
                throw new InvalidOperationException("The FirstError property cannot be accessed when no errors have been recorded. Check IsError before accessing FirstError.");
            }

            return _innerErrors[0];
        }
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is empty.</exception>
    public static ErrorOr<TValue> From(IEnumerable<Error> errors) =>
        errors is null
            ? throw new ArgumentNullException(nameof(errors))
            : new ErrorOr<TValue>([.. errors], nameof(errors));
}
