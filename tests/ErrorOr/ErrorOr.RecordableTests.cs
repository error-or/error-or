namespace Tests;

using ErrorOr;
using FluentAssertions;

public class ErrorOrRecordableTests
{
    [Fact]
    public void GetRecording_WithFunc_WhenIsValue_ShouldReturnFuncResult()
    {
        // Arrange
        var person = new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            new AddressRecord("123 Main St", "Springfield", null),
            ["Developer"]);

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(e => $"Name={e.Value.Name},Age={e.Value.Age}");

        // Assert
        recording.Should().Be("Name=Alice,Age=30");
    }

    [Fact]
    public void GetRecording_WithFunc_CustomFormat_ShouldNotRequireJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Bob",
            null,
            25,
            PersonStatus.Active,
            null,
            null);

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act — plain text format, no JSON serializer involved
        var recording = errorOr.GetRecording(e =>
            e.IsError
                ? string.Join(", ", e.Errors!.Select(err => err.Code))
                : $"{e.Value.Name} ({e.Value.Age})");

        // Assert
        recording.Should().Be("Bob (25)");
        recording.Should().NotContain("{");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOr_WhenIsValue_ShouldReturnFuncResult()
    {
        // Arrange
        var person = new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null);
        IErrorOr errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(e => e.IsError ? "error" : "value");

        // Assert
        recording.Should().Be("value");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOr_WhenIsError_ShouldReturnFuncResult()
    {
        // Arrange
        var error = Error.Unexpected("Test.Error", "Oops.");
        IErrorOr errorOr = (ErrorOr<PersonRecord>)error;

        // Act
        var recording = errorOr.GetRecording(e => string.Join(";", e.Errors!.Select(err => err.Code)));

        // Assert
        recording.Should().Be("Test.Error");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOrTyped_WhenIsValue_ShouldReturnFuncResult()
    {
        // Arrange
        var person = new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null);
        IErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(e => $"Name={e.Value.Name}");

        // Assert
        recording.Should().Be("Name=Alice");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOrTyped_WhenIsError_ShouldReturnFuncResult()
    {
        // Arrange
        var error = Error.Unexpected("Test.Error", "Oops.");
        IErrorOr<PersonRecord> errorOr = (ErrorOr<PersonRecord>)error;

        // Act
        var recording = errorOr.GetRecording(e => e.IsError ? e.Errors![0].Code : e.Value.Name);

        // Assert
        recording.Should().Be("Test.Error");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOr_NullRecorder_ShouldThrowArgumentNullException()
    {
        // Arrange
        IErrorOr errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((Func<IErrorOr, string>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("recorder");
    }

    [Fact]
    public void GetRecording_WithFunc_ViaIErrorOrTyped_NullRecorder_ShouldThrowArgumentNullException()
    {
        // Arrange
        IErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((Func<IErrorOr<PersonRecord>, string>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("recorder");
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

    [Fact]
    public void GetRecording_WithTwoFuncs_WhenIsValue_ShouldCallOnValue()
    {
        // Arrange
        var person = new PersonRecord("Carol", null, 40, PersonStatus.Active, null, null);
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(
            onValue: p => $"Name={p.Name},Age={p.Age}",
            onError: errors => $"Errors={errors.Count}");

        // Assert
        recording.Should().Be("Name=Carol,Age=40");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_WhenIsError_ShouldCallOnError()
    {
        // Arrange
        var error = Error.Unexpected("Err.Code", "Bad.");
        ErrorOr<PersonRecord> errorOr = error;

        // Act
        var recording = errorOr.GetRecording(
            onValue: p => $"Name={p.Name}",
            onError: errors => string.Join(",", errors.Select(e => e.Code)));

        // Assert
        recording.Should().Be("Err.Code");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_NullOnValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((Func<PersonRecord, string>)null!, errors => "errors");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("onValue");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_NullOnError_ShouldThrowArgumentNullException()
    {
        // Arrange
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording(p => p.Name, (Func<List<Error>, string>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("onError");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_ViaIErrorOrTyped_WhenIsValue_ShouldCallOnValue()
    {
        // Arrange
        var person = new PersonRecord("Dave", null, 35, PersonStatus.Active, null, null);
        IErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(
            onValue: p => $"Name={p.Name}",
            onError: errors => "errors");

        // Assert
        recording.Should().Be("Name=Dave");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_ViaIErrorOrTyped_WhenIsError_ShouldCallOnError()
    {
        // Arrange
        var error = Error.Unexpected("X.Code", "Desc.");
        IErrorOr<PersonRecord> errorOr = (ErrorOr<PersonRecord>)error;

        // Act
        var recording = errorOr.GetRecording(
            onValue: p => p.Name,
            onError: errors => string.Join(";", errors.Select(e => e.Code)));

        // Assert
        recording.Should().Be("X.Code");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_ViaIErrorOrTyped_NullOnValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        IErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((Func<PersonRecord, string>)null!, errors => "errors");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("onValue");
    }

    [Fact]
    public void GetRecording_WithTwoFuncs_ViaIErrorOrTyped_NullOnError_ShouldThrowArgumentNullException()
    {
        // Arrange
        IErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording(p => p.Name, (Func<List<Error>, string>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("onError");
    }

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

    private readonly struct PersonStruct(string name, string? middleName, PersonStatus status, List<AddressRecord>? addresses)
    {
        public string Name { get; } = name;
        public string? MiddleName { get; } = middleName;
        public PersonStatus Status { get; } = status;
        public List<AddressRecord>? Addresses { get; } = addresses;
    }
}
