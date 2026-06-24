namespace ErrorOr;

/// <summary>
/// Defines a serialization strategy for recording the state of an <see cref="ErrorOr{TValue}"/> instance
/// in any format without coupling to a specific serialization library.
/// </summary>
/// <typeparam name="TOutput">The type of the serialized output (e.g. <see cref="string"/>, <c>byte[]</c>).</typeparam>
/// <remarks>
/// Implement this interface to record <see cref="ErrorOr{TValue}"/> state in a custom format
/// (e.g. JSON, XML, Protobuf, plain text). The library calls <see cref="SerializeValue{TValue}"/>
/// with the fully-typed value, so no boxing is visible to the implementor.
/// </remarks>
public interface IRecordingSerializer<TOutput>
{
    /// <summary>
    /// Serializes a success value to <typeparamref name="TOutput"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The success value to serialize.</param>
    /// <returns>A <typeparamref name="TOutput"/> representation of <paramref name="value"/>.</returns>
    TOutput SerializeValue<TValue>(TValue value);

    /// <summary>
    /// Serializes a list of errors to <typeparamref name="TOutput"/>.
    /// </summary>
    /// <param name="errors">The list of errors to serialize.</param>
    /// <returns>A <typeparamref name="TOutput"/> representation of <paramref name="errors"/>.</returns>
    TOutput SerializeErrors(ReadOnlyCollection<Error> errors);
}
