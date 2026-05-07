using System.Text.Json;

namespace ErrorOr;

/// <summary>
/// Interface for producing a loggable string representation of an <see cref="ErrorOr{TValue}"/> value
/// without knowing its concrete type.
/// </summary>
/// <remarks>
/// Intended for use in cross-cutting concerns such as logging and auditing where the concrete
/// type of the value is unknown at the call site.
/// </remarks>
public interface IRecordable
{
    /// <summary>
    /// Returns a JSON representation of the current state.
    /// </summary>
    /// <returns>
    /// When <see cref="IErrorOr.IsError"/> is <c>false</c>, returns a JSON representation of the value.
    /// When <see cref="IErrorOr.IsError"/> is <c>true</c>, returns a JSON array of the recorded errors.
    /// </returns>
    string GetRecording();

    /// <summary>
    /// Returns a JSON representation of the current state using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> to use for serialization.</param>
    /// <returns>
    /// When <see cref="IErrorOr.IsError"/> is <c>false</c>, returns a JSON representation of the value.
    /// When <see cref="IErrorOr.IsError"/> is <c>true</c>, returns a JSON array of the recorded errors.
    /// </returns>
    string GetRecording(JsonSerializerOptions options);

    /// <summary>
    /// Returns a string representation of the current state using the provided <paramref name="serializer"/>.
    /// </summary>
    /// <param name="serializer">An <see cref="IRecordingSerializer"/> that produces the string representation.</param>
    /// <returns>
    /// When <see cref="IErrorOr.IsError"/> is <c>false</c>, returns the result of
    /// <see cref="IRecordingSerializer.SerializeValue{TValue}"/> called with the success value.
    /// When <see cref="IErrorOr.IsError"/> is <c>true</c>, returns the result of
    /// <see cref="IRecordingSerializer.SerializeErrors"/> called with the error list.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serializer"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload to produce recordings in any format from a type-erased <see cref="IErrorOr"/> or
    /// <see cref="IRecordable"/> reference. The library dispatches to <see cref="IRecordingSerializer.SerializeValue{TValue}"/>
    /// with the fully-typed value — no boxing is visible to the <see cref="IRecordingSerializer"/> implementor.
    /// </remarks>
    string GetRecording(IRecordingSerializer serializer);
}
