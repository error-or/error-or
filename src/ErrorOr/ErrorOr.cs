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

        if (errors is null || errors.Count == 0)
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

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    public static ErrorOr<TValue> From(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to create the <see cref="ErrorOr{TValue}"/> from.</param>
    /// <returns>An <see cref="ErrorOr{TValue}"/> instance containing the provided errors.</returns>
    public static ErrorOr<TValue> FromError(List<Error> errors)
    {
        return new ErrorOr<TValue>(errors);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a value.
    /// </summary>
    /// <param name="value">The value to create the <see cref="ErrorOr{TValue}"/> from.</param>
    /// <returns>An <see cref="ErrorOr{TValue}"/> instance containing the provided value.</returns>
    public static ErrorOr<TValue> FromValue(TValue value)
    {
        return new ErrorOr<TValue>(value);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a tuple.
    /// </summary>
    /// <param name="tuple">The tuple to create the <see cref="ErrorOr{TValue}"/> from.</param>
    /// <returns>An <see cref="ErrorOr{TValue}"/> instance containing the provided value or errors.</returns>
    public static ErrorOr<TValue> FromTuple((TValue? Value, List<Error>? Errors) tuple)
    {
        if (tuple.Errors is not null)
        {
            return new ErrorOr<TValue>(tuple.Errors);
        }

        if (tuple.Value is not null)
        {
            return new ErrorOr<TValue>(tuple.Value);
        }

        throw new ArgumentException("The tuple must contain either a value or errors.", nameof(tuple));
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a custom object.
    /// </summary>
    /// <typeparam name="TCustom">The type of the custom object.</typeparam>
    /// <param name="customObject">The custom object to create the <see cref="ErrorOr{TValue}"/> from.</param>
    /// <param name="valueSelector">The function to select the value from the custom object.</param>
    /// <param name="errorsSelector">The function to select the errors from the custom object.</param>
    /// <returns>An <see cref="ErrorOr{TValue}"/> instance containing the provided value or errors.</returns>
    public static ErrorOr<TValue> FromCustom<TCustom>(TCustom customObject, Func<TCustom, TValue?> valueSelector, Func<TCustom, List<Error>?> errorsSelector)
    {
        var errors = errorsSelector(customObject);
        if (errors is not null)
        {
            return new ErrorOr<TValue>(errors);
        }

        var value = valueSelector(customObject);
        if (value is not null)
        {
            return new ErrorOr<TValue>(value);
        }

        throw new ArgumentException("The custom object must contain either a value or errors.", nameof(customObject));
    }
}
