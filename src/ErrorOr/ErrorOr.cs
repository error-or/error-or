using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ErrorOr;

/// <summary>
/// A discriminated union of errors or a value.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="Value"/>.</typeparam>
[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateErrorOr))]
public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    private readonly TValue? _value = default;
    private readonly List<Error>? _errors = null;

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
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public List<Error> Errors => IsError ? _errors : throw new InvalidOperationException("The Errors property cannot be accessed when no errors have been recorded. Check IsError before accessing Errors.");

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will be empty.
    /// </summary>
    public List<Error> ErrorsOrEmptyList => IsError ? _errors : EmptyErrors.Instance;

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

            return _errors[0];
        }
    }

    /// <inheritdoc/>
    public IEnumerator<Error> GetEnumerator() => _errors!.GetEnumerator();

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    [Obsolete("ErrorOrFactory.From<TValue>(errors) should be used instead.")]
    public static ErrorOr<TValue> From(List<Error> errors) => errors;

    /// <summary>
    /// Returns a JSON representation of the current <see cref="ErrorOr{TValue}"/> instance.
    /// </summary>
    /// <returns>A JSON string representation of the current <see cref="ErrorOr{TValue}"/> instance.</returns>
    public override string ToString() => GetRecording();
}
