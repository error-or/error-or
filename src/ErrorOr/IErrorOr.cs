using System.Runtime.CompilerServices;

namespace ErrorOr;

/// <summary>
/// Strongly-typed interface for <see cref="ErrorOr{TValue}"/> object.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="ErrorOr{TValue}.Value"/>.</typeparam>
[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIErrorOrValue))]
public interface IErrorOr<out TValue> : IErrorOr
{
    /// <summary>
    /// Gets strongly-typed value.
    /// </summary>
    new TValue Value { get; }
}

/// <summary>
/// Weakly-typed interface for the <see cref="ErrorOr{TValue}"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="ErrorOr{TValue}"/> object is unknown.
/// </remarks>
[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIErrorOr))]
public interface IErrorOr : IRecordable
{
    /// <summary>
    /// Gets weakly-typed value.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }

    /// <summary>
    /// Gets a value indicating whether the state is a success.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets enumerator with <see cref="Error"/> objects.
    /// </summary>
    /// <returns>Enunerator of <see cref="Error"/> objects.</returns>
    /// <remarks>This method is only for the purpose of collection expression support.</remarks>
    IEnumerator<Error> GetEnumerator();
}
