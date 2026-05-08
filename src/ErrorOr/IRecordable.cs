namespace ErrorOr;

/// <summary>
/// Interface for producing a loggable representation of an <see cref="ErrorOr{TValue}"/> value
/// without knowing its concrete type.
/// </summary>
/// <remarks>
/// Intended for use in cross-cutting concerns such as logging and auditing where the concrete
/// type of the value is unknown at the call site.
/// </remarks>
public interface IRecordable
{
    /// <summary>
    /// Returns a representation of the current state using the provided <paramref name="serializer"/>.
    /// </summary>
    /// <typeparam name="TOutput">The type of the serialized output (e.g. <see cref="string"/>, <c>byte[]</c>).</typeparam>
    /// <param name="serializer">An <see cref="IRecordingSerializer{TOutput}"/> that produces the representation.</param>
    /// <returns>
    /// When <see cref="IErrorOr.IsError"/> is <c>false</c>, returns the result of
    /// <see cref="IRecordingSerializer{TOutput}.SerializeValue{TValue}"/> called with the success value.
    /// When <see cref="IErrorOr.IsError"/> is <c>true</c>, returns the result of
    /// <see cref="IRecordingSerializer{TOutput}.SerializeErrors"/> called with the error list.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serializer"/> is <see langword="null"/>.</exception>
    TOutput GetRecording<TOutput>(IRecordingSerializer<TOutput> serializer);
}
