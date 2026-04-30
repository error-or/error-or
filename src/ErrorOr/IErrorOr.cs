using System.Runtime.CompilerServices;

namespace ErrorOr;

[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIErrorOrValue))]
public interface IErrorOr<out TValue> : IErrorOr
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    TValue Value { get; }
}

/// <summary>
/// Type-less interface for the <see cref="ErrorOr"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="ErrorOr"/> object is unknown.
/// </remarks>
[CollectionBuilder(typeof(CollectionExpression), nameof(CollectionExpression.CreateIErrorOr))]
public interface IErrorOr : IRecordable
{
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }

    /// <summary>
    /// Gets enumerator with <see cref="Error"/> objects.
    /// </summary>
    /// <returns>Enunerator of <see cref="Error"/> objects.</returns>
    /// <remarks>This method is only for the purpose of collection expression support.</remarks>
    IEnumerator<Error> GetEnumerator();
}
