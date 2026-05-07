namespace Tests;

using ErrorOr;
using FluentAssertions;

public class ErrorOrRecordableTests
{
    [Fact]
    public void GetRecording_WithSerializer_WhenIsValue_ShouldCallSerializeValue()
    {
        // Arrange
        var person = new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null);
        IRecordable errorOr = ErrorOrFactory.From(person);
        var serializer = new PlainTextRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be($"value:{person.Name}:{person.Age}");
    }

    [Fact]
    public void GetRecording_WithSerializer_WhenIsError_ShouldCallSerializeErrors()
    {
        // Arrange
        var error = Error.Unexpected("Test.Error", "Oops.");
        IRecordable errorOr = (ErrorOr<PersonRecord>)error;
        var serializer = new PlainTextRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be("errors:Test.Error");
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIErrorOr_WhenIsValue_ShouldCallSerializeValue()
    {
        // Arrange
        var person = new PersonRecord("Bob", null, 25, PersonStatus.Active, null, null);
        IErrorOr errorOr = ErrorOrFactory.From(person);
        var serializer = new PlainTextRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be($"value:{person.Name}:{person.Age}");
    }

    [Fact]
    public void GetRecording_WithSerializer_ViaIErrorOr_WhenIsError_ShouldCallSerializeErrors()
    {
        // Arrange
        var error = Error.Validation("Val.Error", "Invalid.");
        IErrorOr errorOr = (ErrorOr<PersonRecord>)error;
        var serializer = new PlainTextRecordingSerializer();

        // Act
        var recording = errorOr.GetRecording(serializer);

        // Assert
        recording.Should().Be("errors:Val.Error");
    }

    [Fact]
    public void GetRecording_WithSerializer_NullSerializer_ShouldThrowArgumentNullException()
    {
        // Arrange
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((IRecordingSerializer)null!);

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

    private sealed class PlainTextRecordingSerializer : IRecordingSerializer
    {
        public string SerializeValue<TValue>(TValue value)
        {
            if (value is PersonRecord p)
            {
                return $"value:{p.Name}:{p.Age}";
            }

            return $"value:{value}";
        }

        public string SerializeErrors(List<Error> errors)
            => $"errors:{string.Join(",", errors.Select(e => e.Code))}";
    }
}
