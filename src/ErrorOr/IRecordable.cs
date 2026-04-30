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
}
