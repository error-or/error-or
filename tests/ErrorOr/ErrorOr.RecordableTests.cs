using System.Text.Json;
using System.Text.Json.Serialization;
using ErrorOr;
using FluentAssertions;
namespace Tests;

public class ErrorOrRecordableTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    };

    [Fact]
    public void GetRecording_WithSerializer_WhenIsValue_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            new AddressRecord("123 Main St", "Springfield", null),
            ["Developer", "Admin"]);

        IErrorOr errorOr = ErrorOrFactory.From(person);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WithSerializer_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Unexpected("Test.Error", "Oops.");
        IRecordable errorOr = ErrorOrFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIErrorOr_WhenIsValue_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Bob",
            null,
            25,
            PersonStatus.Inactive,
            null,
            null);

        IErrorOr errorOr = ErrorOrFactory.From(person);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIErrorOr_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Validation("Val.Error", "Invalid.");
        IRecordable errorOr = ErrorOrFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void GetRecording_WithSerializer_NullSerializer_ShouldThrowArgumentNullException()
    {
        // Arrange
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Suspended,
            null,
            null));

        // Act
        var act = () => errorOr.GetRecording(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("serializer");
    }

    private enum PersonStatus
    {
        Active,
        Inactive,
        Suspended,
    }

    private record AddressRecord(string Street, string City, string? PostalCode);

    private record PersonRecord(
        string Name,
        string? MiddleName,
        int Age,
        PersonStatus Status,
        AddressRecord? Address,
        List<string>? Tags);

    private sealed class SystemTextJsonRecordingSerializer : IRecordingSerializer
    {
        public string SerializeValue<TValue>(TValue value) => JsonSerializer.Serialize(value, JsonOptions);

        public string SerializeErrors(List<Error> errors) => JsonSerializer.Serialize(errors, JsonOptions);
    }
}
