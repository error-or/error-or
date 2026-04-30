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
