namespace ErrorOr;

/// <summary>
/// Defines a serialization strategy for recording the state of an <see cref="ErrorOr{TValue}"/> instance
/// in any format without coupling to a specific serialization library.
/// </summary>
/// <remarks>
/// Implement this interface to record <see cref="ErrorOr{TValue}"/> state in a custom format
/// (e.g. XML, Json, plain text). The library calls <see cref="SerializeValue{TValue}"/>
/// with the fully-typed value, so no boxing is visible to the implementor.
/// </remarks>
public interface IRecordingSerializer
{
    /// <summary>
    /// Serializes a success value to a string.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The success value to serialize.</param>
    /// <returns>A string representation of <paramref name="value"/>.</returns>
    string SerializeValue<TValue>(TValue value);

    /// <summary>
    /// Serializes a list of errors to a string.
    /// </summary>
    /// <param name="errors">The list of errors to serialize.</param>
    /// <returns>A string representation of <paramref name="errors"/>.</returns>
    string SerializeErrors(List<Error> errors);
}
