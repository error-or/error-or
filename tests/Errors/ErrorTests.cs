using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ErrorTests
{
    private const string ErrorCode = "ErrorCode";
    private const string ErrorDescription = "ErrorDescription";
    private static readonly ReadOnlyDictionary<string, object> Dictionary = new(
    new Dictionary<string, object>
    {
        { "key1", "value1" },
        { "key2", 21 },
    });

    [Fact]
    public void CreateError_WhenFailureError_ShouldHaveErrorTypeFailure()
    {
        // Act
        Error error = Error.Failure(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Failure);
    }

    [Fact]
    public void CreateError_WhenUnexpectedError_ShouldHaveErrorTypeFailure()
    {
        // Act
        Error error = Error.Unexpected(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Unexpected);
    }

    [Fact]
    public void CreateError_WhenValidationError_ShouldHaveErrorTypeValidation()
    {
        // Act
        Error error = Error.Validation(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Validation);
    }

    [Fact]
    public void CreateError_WhenConflictError_ShouldHaveErrorTypeConflict()
    {
        // Act
        Error error = Error.Conflict(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Conflict);
    }

    [Fact]
    public void CreateError_WhenNotFoundError_ShouldHaveErrorTypeNotFound()
    {
        // Act
        Error error = Error.NotFound(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.NotFound);
    }

    [Fact]
    public void CreateError_WhenNotAuthorizedError_ShouldHaveErrorTypeUnauthorized()
    {
        // Act
        Error error = Error.Unauthorized(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Unauthorized);
    }

    [Fact]
    public void CreateError_WhenForbiddenError_ShouldHaveErrorTypeForbidden()
    {
        // Act
        Error error = Error.Forbidden(ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: ErrorType.Forbidden);
    }

    [Fact]
    public void CreateError_WhenCustomType_ShouldHaveCustomErrorType()
    {
        // Act
        Error error = Error.Custom(1232, ErrorCode, ErrorDescription, Dictionary);

        // Assert
        ValidateError(error, expectedErrorType: (ErrorType)1232);
    }

    private static void ValidateError(Error error, ErrorType expectedErrorType)
    {
        error.Code.Should().Be(ErrorCode);
        error.Description.Should().Be(ErrorDescription);
        error.Type.Should().Be(expectedErrorType);
        error.NumericType.Should().Be((int)expectedErrorType);
        error.Metadata.Should().BeEquivalentTo(Dictionary);
    }
}
