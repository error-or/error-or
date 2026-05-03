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

    private ErrorOr(TValue value) => _value = value;

    private ErrorOr(Error error) => _errors = [error];

    private ErrorOr(List<Error> errors) => _errors = errors?.Count == 0 ? null : errors;

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    [MemberNotNullWhen(true, nameof(_errors))]
    [MemberNotNullWhen(true, nameof(Errors))]
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsError => !IsSuccess;

    /// <summary>
    /// Gets a value indicating whether the state is a success.
    /// </summary>
    [MemberNotNullWhen(false, nameof(_errors))]
    [MemberNotNullWhen(false, nameof(Errors))]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsSuccess => _errors is null;

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
            if (IsSuccess)
            {
                return KnownErrors.NoFirstError;
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
