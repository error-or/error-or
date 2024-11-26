using ErrorOr;
using ErrorOr.ErrorOr;
using FluentAssertions;

namespace Tests.ErrorOr;

public class AggregationExtensionsTests
{
    [Fact]
    public void AppendErrors_NoAdditionalErrors_ReturnsOriginalErrorOr()
    {
        // Arrange
        var originalErrorOr = ErrorOr<int>.FromError([Error.Validation("Initial error")]);

        // Act
        var result = originalErrorOr.AppendErrors();

        // Assert
        result.Should().Be(originalErrorOr);
    }

    [Fact]
    public void AppendErrors_NullErrorsArray_ReturnsOriginalErrorOr()
    {
        // Arrange
        var originalErrorOr = ErrorOr<int>.FromError([Error.Validation("Initial error")]);

        // Act
        var result = originalErrorOr.AppendErrors(null);

        // Assert
        result.Should().Be(originalErrorOr);
    }

    [Fact]
    public void AppendErrors_EmptyErrorsArray_ReturnsOriginalErrorOr()
    {
        // Arrange
        var originalErrorOr = ErrorOr<int>.FromError([Error.Validation("Initial error")]);

        // Act
        var result = originalErrorOr.AppendErrors(Array.Empty<IErrorOr>());

        // Assert
        result.Should().Be(originalErrorOr);
    }

    [Fact]
    public void AppendErrors_WithAdditionalErrors_AppendsErrorsCorrectly()
    {
        // Arrange
        var initialError = ErrorOr<int>.FromError([
            Error.Validation("Initial error", "A validation error has occurred.")
        ]);
        var additionalError1 = ErrorOr<int>.FromError([
            Error.Validation("Additional error 1", "A validation error has occurred.")
        ]);
        var additionalError2 = ErrorOr<int>.FromError([
            Error.Validation("Additional error 2", "A validation error has occurred.")
        ]);

        // Act
        var result = initialError.AppendErrors(additionalError1, additionalError2);

        // Assert
        result.Errors.Should().Contain(e => e.Description == "A validation error has occurred.");
    }

    [Fact]
    public void AppendErrors_WithNullErrorInArray_IgnoresNullError()
    {
        // Arrange
        var originalErrorOr = ErrorOr<int>.FromError([Error.Validation("Initial error")]);
        ErrorOr<int>[] errors = new ErrorOr<int>[] { default };

        // Act
        var result = originalErrorOr.AppendErrors(errors.Cast<IErrorOr>().ToArray());

        // Assert
        result.Should().Be(originalErrorOr);
    }

    [Fact]
    public void AppendErrors_ShouldReturnSuccessValue_WhenNoErrorsInAppendedErrorOr_WithInt()
    {
        // Arrange
        var success1 = ErrorOr<int>.FromValue(1);
        var success2 = ErrorOr<int>.FromValue(2);

        // Act
        var result = success1.AppendErrors(success2);

        // Assert
        result.IsError.Should().BeFalse(); // Should be success
        result.Value.Should().Be(1); // Should return the original success value
    }

    [Fact]
    public void AppendErrors_ShouldReturnSuccessValue_WhenNoErrorsInAppendedErrorOr_WithString()
    {
        // Arrange
        var success1 = ErrorOr<string>.FromValue("Hello");
        var success2 = ErrorOr<string>.FromValue("World");

        // Act
        var result = success1.AppendErrors(success2);

        // Assert
        result.IsError.Should().BeFalse(); // Should be success
        result.Value.Should().Be("Hello"); // Should return the original success value
    }

    [Fact]
    public void AppendErrors_ShouldReturnSuccessValue_WhenMultipleSuccessValuesAreAppended_WithUserType()
    {
        // Arrange
        var success1 = ErrorOr<User>.FromValue(new User { Id = 1, Name = "John" });
        var success2 = ErrorOr<User>.FromValue(new User { Id = 2, Name = "Jane" });
        var success3 = ErrorOr<User>.FromValue(new User { Id = 3, Name = "Doe" });

        // Act
        var result = success1.AppendErrors(success2, success3);

        // Assert
        result.IsError.Should().BeFalse(); // Should be success
        result.Value.Name.Should().Be("John"); // Should return the first user
    }

    [Fact]
    public void AppendErrors_ShouldReturnFailure_WhenErrorIsAppended_WithInt()
    {
        // Arrange
        var success1 = ErrorOr<int>.FromValue(1);
        var failure = ErrorOr<int>.FromError([Error.Validation("ValidationError", "Validation failed")]);

        // Act
        var result = success1.AppendErrors(failure);

        // Assert
        result.IsError.Should().BeTrue(); // Should be failure
        result.Errors.Should().HaveCount(1); // Should contain one error
        result.Errors[0].Description.Should().Be("Validation failed"); // Should match the error message
    }

    [Fact]
    public void AppendErrors_ShouldReturnCombinedErrors_WhenMultipleErrorsAreAppended_WithString()
    {
        // Arrange
        var success1 = ErrorOr<string>.FromValue("Success");
        var failure1 = ErrorOr<string>.FromError([Error.Validation("ValidationError1", "First validation failed")]);
        var failure2 = ErrorOr<string>.FromError([Error.Validation("ValidationError2", "Second validation failed")]);

        // Act
        var result = success1.AppendErrors(failure1, failure2);

        // Assert
        result.IsError.Should().BeTrue(); // Should be failure
        result.Errors.Should().HaveCount(2); // Should contain two errors
        result.Errors[0].Description.Should().Be("First validation failed");
        result.Errors[1].Description.Should().Be("Second validation failed");
    }

    [Fact]
    public void AppendErrors_ShouldReturnFailure_WhenErrorIsAppended_WithUserType()
    {
        // Arrange
        var success1 = ErrorOr<User>.FromValue(new User { Id = 1, Name = "John" });
        var failure = ErrorOr<User>.FromError([Error.Validation("UserValidationError", "User not valid")]);

        // Act
        var result = success1.AppendErrors(failure);

        // Assert
        result.IsError.Should().BeTrue(); // Should be failure
        result.Errors.Should().HaveCount(1); // Should contain one error
        result.Errors[0].Description.Should().Be("User not valid"); // Should match the error message
    }

    [Fact]
    public void AppendErrors_ShouldReturnOriginal_WhenNullErrorsArrayIsPassed()
    {
        // Arrange
        var success1 = ErrorOr<int>.FromValue(1);

        // Act
        var result = success1.AppendErrors(null as IErrorOr[] ?? Array.Empty<IErrorOr>());

        // Assert
        result.Should().Be(success1); // Should return the original instance if null is passed
    }

    [Fact]
    public void AppendErrors_ShouldReturnOriginal_WhenEmptyErrorsArrayIsPassed()
    {
        // Arrange
        var success1 = ErrorOr<int>.FromValue(1);

        // Act
        var result = success1.AppendErrors(Array.Empty<IErrorOr>());

        // Assert
        result.Should().Be(success1); // Should return the original instance if empty array is passed
    }

    [Fact]
    public void AppendErrors_ShouldHandleNullInTheErrorsArray()
    {
        // Arrange
        var success1 = ErrorOr<int>.FromValue(1);
        IErrorOr?[] errors = new IErrorOr?[] { null, null };

        // Act
        var result = success1.AppendErrors(errors.Where(e => e != null).Cast<IErrorOr>().ToArray());

        // Assert
        result.Should().Be(success1); // Should return the original instance if null elements are passed
    }

    [Fact]
    public void AppendErrors_ShouldHandleNullErrorInEachItemGracefully_WithUserType()
    {
        // Arrange
        var success1 = ErrorOr<User>.FromValue(new User { Id = 1, Name = "John" });
        var failure1 = ErrorOr<User>.FromError([Error.Validation("ValidationError1", "User invalid")]);
        var failure2 = ErrorOr<User>.FromError([Error.Validation("ValidationError2", "User inactive")]);

        // Act
        var result = success1.AppendErrors(failure1, failure2);

        // Assert
        result.IsError.Should().BeTrue(); // Should be failure
        result.Errors.Should().HaveCount(2); // Should contain two errors
        result.Errors[0].Description.Should().Be("User invalid");
        result.Errors[1].Description.Should().Be("User inactive");
    }

    [Fact]
    public void AppendErrors_WithMixedSuccessAndErrorInstances_AppendsOnlyErrors()
    {
        // Arrange
        var success = ErrorOr<int>.FromValue(42);
        var error1 = ErrorOr<object>.FromError([Error.Failure("Error1")]);
        var error2 = ErrorOr<object>.FromError([Error.Failure("Error2")]);

        // Act
        var result = success.AppendErrors(error1, error2);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Code == "Error1");
        result.Errors.Should().Contain(e => e.Code == "Error2");
    }
}
