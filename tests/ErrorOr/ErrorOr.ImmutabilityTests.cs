using ErrorOr;

using FluentAssertions;

namespace Tests;

public class ErrorOrImmutabilityTests
{
    [Fact]
    public void AdditionToUnderlyingErrorList_UsedToCreateErrorOr_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList;

        // Act
        errorList.Add(Error.Validation("Validation.Error3", "Validation error 3")); // Adding an item to the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AdditionToUnderlyingErrorList_UsedToCreateErrorOrFromReadOnlyCollection_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList.AsReadOnly();

        // Act
        errorList.Add(Error.Validation("Validation.Error3", "Validation error 3")); // Adding an item to the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void ModificationToUnderlyingErrorList_UsedToCreateErrorOr_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList;

        // Act
        errorList[1] = Error.Validation("Validation.Error3", "Validation error 3"); // Modifying an item of the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
        errorOrInt.Errors[1].Should().BeEquivalentTo(Error.Validation("Validation.Error2", "Validation error 2")); // The second error should remain unchanged
    }

    [Fact]
    public void ModificationToUnderlyingErrorList_UsedToCreateErrorOrFromReadOnlyCollection_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList.AsReadOnly();

        // Act
        errorList[1] = Error.Validation("Validation.Error3", "Validation error 3"); // Modifying an item of the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
        errorOrInt.Errors[1].Should().BeEquivalentTo(Error.Validation("Validation.Error2", "Validation error 2")); // The second error should remain unchanged
    }

    [Fact]
    public void ModificationToUnderlyingErrorArray_UsedToCreateErrorOr_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        Error[] errorArray = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorArray;

        // Act
        errorArray[1] = Error.Validation("Validation.Error3", "Validation error 3"); // Modifying an item of the original array should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
        errorOrInt.Errors[1].Should().BeEquivalentTo(Error.Validation("Validation.Error2", "Validation error 2")); // The second error should remain unchanged
    }

    [Fact]
    public void ModificationToUnderlyingErrorArray_UsedToCreateErrorOrFromReadOnlyCollection_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        Error[] errorArray = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = Array.AsReadOnly(errorArray);

        // Act
        errorArray[1] = Error.Validation("Validation.Error3", "Validation error 3"); // Modifying an item of the original array should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
        errorOrInt.Errors[1].Should().BeEquivalentTo(Error.Validation("Validation.Error2", "Validation error 2")); // The second error should remain unchanged
    }

    [Fact]
    public void RemovalFromUnderlyingErrorList_UsedToCreateErrorOr_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList;

        // Act
        errorList.RemoveAt(1); // Removing an item from the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void RemovalFromUnderlyingErrorList_UsedToCreateErrorOrFromReadOnlyCollection_ShouldNotAffectTheErrorOrInstance()
    {
        // Arrange
        List<Error> errorList = [
            Error.Validation("Validation.Error1", "Validation error 1"),
            Error.Validation("Validation.Error2", "Validation error 2")
        ];

        ErrorOr<int> errorOrInt = errorList.AsReadOnly();

        // Act
        errorList.RemoveAt(1); // Removing an item from the original list should not affect the ErrorOr instance)

        // Assert
        errorOrInt.IsError.Should().BeTrue();
        errorOrInt.Errors.Should().HaveCount(2);
    }
}
