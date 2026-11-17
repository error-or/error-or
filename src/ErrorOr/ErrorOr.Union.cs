#if NET11_0_OR_GREATER
namespace ErrorOr;

using System.Runtime.CompilerServices;

[Union]
public readonly partial record struct ErrorOr<TValue> : ErrorOr<TValue>.IUnionMembers
{
    /// <summary>
    /// Represents a discriminated union of a value of type <typeparamref name="TValue"/> or <see cref="Errors"/>.
    /// </summary>
    /// <remarks>
    /// https://learn.microsoft.com/en-nz/dotnet/csharp/language-reference/builtin-types/union#union-member-providers.
    /// </remarks>
    public interface IUnionMembers
    {
        object? Value { get; }

        bool HasValue { get; }

        static ErrorOr<TValue> Create(TValue? value) => value!;

        static ErrorOr<TValue> Create(Errors errors) => new(errors);

        bool TryGetValue(out TValue value);

        bool TryGetValue(out Errors value);
    }

    object? IUnionMembers.Value => IsError ? new Errors(_errors!) : _value;

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

    bool IUnionMembers.TryGetValue(out Errors value)
    {
        if (IsError)
        {
            value = new Errors(_errors!);
            return true;
        }

        if (_value is Errors errorCase)
        {
            value = errorCase;
            return true;
        }

        value = default!;
        return false;
    }
}
#endif
