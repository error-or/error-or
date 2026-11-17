#if NET11_0_OR_GREATER
namespace ErrorOr;

using System.Runtime.CompilerServices;

[Union]
public readonly partial record struct ErrorOr<TValue> : ErrorOr<TValue>.IUnionMembers
{
    /// <summary>
    /// Represents a discriminated union of a value of type <typeparamref name="TValue"/> or a list of errors.
    /// </summary>
    /// <remarks>
    /// https://learn.microsoft.com/en-nz/dotnet/csharp/language-reference/builtin-types/union#union-member-providers
    /// </remarks>
    public interface IUnionMembers
    {
        object? Value { get; }

        bool HasValue { get; }

        static ErrorOr<TValue> Create(TValue? value) => value!;

        static ErrorOr<TValue> Create(List<Error> errors) => errors;

        bool TryGetValue(out TValue value);

        bool TryGetValue(out List<Error> value);
    }

    object? IUnionMembers.Value => IsError ? _errors : _value;

    bool IUnionMembers.HasValue => IsError || _value is not null;

    bool IUnionMembers.TryGetValue(out TValue value)
    {
        if (IsSuccess && _value is TValue typedValue)
        {
            value = typedValue;
            return true;
        }

        if (_errors is TValue errorValue)
        {
            value = errorValue;
            return true;
        }

        value = default!;
        return false;
    }

    bool IUnionMembers.TryGetValue(out List<Error> value)
    {
        if (IsError)
        {
            value = _errors;
            return true;
        }

        if (_value is List<Error> errorList)
        {
            value = errorList;
            return true;
        }

        value = default!;
        return false;
    }
}
#endif
