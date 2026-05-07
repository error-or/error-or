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
    public void Accept_WithSerializerVisitor_WhenIsValue_ShouldReturnJson()
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
        var serializer = new SystemTextJsonErrorOrVisitor();

        // Act
        errorOr.Accept(serializer);

        // Assert
        serializer.SerializationResult.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void Accept_WithSerializerVisitor_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Unexpected("Test.Error", "Oops.");
        IErrorOr errorOr = ErrorOrFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonErrorOrVisitor();

        // Act
        errorOr.Accept(serializer);

        // Assert
        serializer.SerializationResult.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void Accept_WithSerializerVisitor_ViaIErrorOr_WhenIsValue_ShouldReturnJson()
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
        var serializer = new SystemTextJsonErrorOrVisitor();

        // Act
        errorOr.Accept(serializer);

        // Assert
        serializer.SerializationResult.Should().Be(JsonSerializer.Serialize(person, JsonOptions));
    }

    [Fact]
    public void Accept_WithSerializerVisitor_ViaIErrorOr_WhenIsError_ShouldReturnJsonErrors()
    {
        // Arrange
        Error error = Error.Validation("Val.Error", "Invalid.");
        IErrorOr errorOr = ErrorOrFactory.From<PersonRecord>(error);
        var serializer = new SystemTextJsonErrorOrVisitor();

        // Act
        errorOr.Accept(serializer);

        // Assert
        serializer.SerializationResult.Should().Be(JsonSerializer.Serialize(new[] { error }, JsonOptions));
    }

    [Fact]
    public void Accept_WithNullVisitor_ShouldThrowArgumentNullException()
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
        var act = () => errorOr.Accept(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("visitor");
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

    private sealed class SystemTextJsonErrorOrVisitor : IErrorOrVisitor
    {
        public string? SerializationResult { get; private set; }

        public void VisitValue<TValue>(TValue value) => SerializationResult = JsonSerializer.Serialize(value, JsonOptions);

        public void VisitErrors(List<Error> errors) => SerializationResult = JsonSerializer.Serialize(errors, JsonOptions);
    }
}
