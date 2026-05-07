namespace Tests;

using System.Text.Json;
using System.Text.Json.Serialization;
using ErrorOr;
using FluentAssertions;

public class ErrorOrRecordableTests
{
    [Fact]
    public void GetRecording_WhenTValueIsRecordClass_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            new AddressRecord("123 Main St", "Springfield", null),
            ["Developer", "Admin"]);

        IRecordable errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording();

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WhenTValueIsRecordStruct_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecordStruct(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            [new AddressRecord("123 Main St", "Springfield", null), new AddressRecord("456 Oak Ave", "Shelbyville", "62565")]);

        IRecordable errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording();

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WhenTValueIsPlainClass_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonClass(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            [new AddressRecord("123 Main St", "Springfield", null), new AddressRecord("456 Oak Ave", "Shelbyville", "62565")]);

        IRecordable errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording();

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WhenTValueIsPlainStruct_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonStruct(
            "Alice",
            null,
            PersonStatus.Active,
            [new AddressRecord("123 Main St", "Springfield", null)]);

        IRecordable errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording();

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void GetRecording_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        var error = Error.Unexpected();

        IRecordable errorOr = (ErrorOr<PersonRecord>)error;

        // Act
        var recording = errorOr.GetRecording();

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void ToString_WhenTValueIsValue_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Alice",
            null,
            30,
            PersonStatus.Active,
            new AddressRecord("123 Main St", "Springfield", null),
            ["Developer", "Admin"]);

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var result = errorOr.ToString();

        // Assert
        result.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void ToString_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        var error = Error.Unexpected();

        ErrorOr<PersonRecord> errorOr = error;

        // Act
        var result = errorOr.ToString();

        // Assert
        result.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void GetRecording_WithCompactJsonOptions_ShouldReturnCompactJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Bob",
            "James",
            25,
            PersonStatus.Active,
            new AddressRecord("456 Elm St", "Springfield", null),
            ["Developer"]);

        var compactOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() },
        };

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(compactOptions);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, compactOptions));
        recording.Should().NotContain("\n");
        recording.Should().NotContain("  ");
    }

    [Fact]
    public void GetRecording_WithIgnoreNullValuesOption_ShouldOmitNullProperties()
    {
        // Arrange
        var person = new PersonRecord(
            "Charlie",
            null,
            35,
            PersonStatus.Inactive,
            null,
            null);

        var ignoreNullOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() },
        };

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(ignoreNullOptions);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, ignoreNullOptions));
        recording.Should().NotContain("\"middleName\"");
        recording.Should().NotContain("\"address\"");
        recording.Should().NotContain("\"tags\"");
    }

    [Fact]
    public void GetRecording_WithPropertyNamingPolicy_ShouldUseCamelCase()
    {
        // Arrange
        var person = new PersonRecord(
            "Diana",
            "Marie",
            28,
            PersonStatus.Active,
            new AddressRecord("789 Oak Ave", "Capital City", "12345"),
            ["Engineer", "Lead"]);

        var camelCaseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() },
        };

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(camelCaseOptions);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, camelCaseOptions));
        recording.Should().Contain("\"name\"");
        recording.Should().Contain("\"middleName\"");
        recording.Should().Contain("\"age\"");
        recording.Should().NotContain("\"Name\"");
        recording.Should().NotContain("\"MiddleName\"");
    }

    [Fact]
    public void GetRecording_WithCustomOptions_WhenError_ShouldRespectOptions()
    {
        // Arrange
        var error = Error.Unexpected();
        var compactOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() },
        };

        ErrorOr<PersonRecord> errorOr = error;

        // Act
        var recording = errorOr.GetRecording(compactOptions);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(new[] { error }, compactOptions));
        recording.Should().NotContain("\n");
    }

    [Fact]
    public void GetRecording_CustomOptionsVsDefault_ShouldProduceDifferentOutput()
    {
        // Arrange
        var person = new PersonRecord(
            "Eve",
            null,
            40,
            PersonStatus.Suspended,
            new AddressRecord("321 Pine St", "Metropolis", null),
            null);

        var compactOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() },
        };

        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(person);

        // Act
        var defaultRecording = errorOr.GetRecording();
        var customRecording = errorOr.GetRecording(compactOptions);

        // Assert
        defaultRecording.Should().NotBe(customRecording);
        defaultRecording.Should().Contain("\n");
        customRecording.Should().NotContain("\n");
    }

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
    public void GetRecording_WithFunc_WhenIsError_ShouldReturnFuncResult()
    {
        // Arrange
        var error = Error.Unexpected("General.Unexpected", "Something went wrong.");
        ErrorOr<PersonRecord> errorOr = error;

        // Act
        var recording = errorOr.GetRecording(e => $"Error={e.FirstError.Code}:{e.FirstError.Description}");

        // Assert
        recording.Should().Be("Error=General.Unexpected:Something went wrong.");
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
    public void GetRecording_WithFunc_NullRecorder_ShouldThrowArgumentNullException()
    {
        // Arrange
        ErrorOr<PersonRecord> errorOr = ErrorOrFactory.From(new PersonRecord("Alice", null, 30, PersonStatus.Active, null, null));

        // Act
        var act = () => errorOr.GetRecording((Func<ErrorOr<PersonRecord>, string>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("recorder");
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

    [Fact]
    public void GetRecording_WithInterfaceType_CustomOptions_ShouldReturnJson()
    {
        // Arrange
        var person = new PersonRecord(
            "Frank",
            "Xavier",
            32,
            PersonStatus.Active,
            new AddressRecord("654 Maple Dr", "Smallville", "54321"),
            ["Manager"]);

        var camelCaseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = { new JsonStringEnumConverter() },
        };

        IRecordable errorOr = ErrorOrFactory.From(person);

        // Act
        var recording = errorOr.GetRecording(camelCaseOptions);

        // Assert
        recording.Should().Be(JsonSerializer.Serialize(person, camelCaseOptions));
        recording.Should().Contain("\"name\"");
        recording.Should().Contain("\"status\"");
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters =
        {
            new JsonStringEnumConverter(),
        },
    };

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

    private readonly record struct PersonRecordStruct(
        string Name,
        string? MiddleName,
        int Age,
        PersonStatus Status,
        List<AddressRecord>? Addresses);

    private class PersonClass(string name, string? middleName, int age, PersonStatus status, List<AddressRecord>? addresses)
    {
        public string Name { get; } = name;
        public string? MiddleName { get; } = middleName;
        public int Age { get; } = age;
        public PersonStatus Status { get; } = status;
        public List<AddressRecord>? Addresses { get; } = addresses;
    }

    private readonly struct PersonStruct(string name, string? middleName, PersonStatus status, List<AddressRecord>? addresses)
    {
        public string Name { get; } = name;
        public string? MiddleName { get; } = middleName;
        public PersonStatus Status { get; } = status;
        public List<AddressRecord>? Addresses { get; } = addresses;
    }
}
