using ErrorOr;
using FluentAssertions;

namespace Tests;

public class RecoverTests
{
    [Fact]
    public void CallingRecover_WhenIsSuccess_ShouldNotInvokeRecoverFunc()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        var recoverInvoked = false;

        // Act
        ErrorOr<int> result = errorOrInt
            .Recover(_ =>
            {
                recoverInvoked = true;
                return 10;
            });

        // Assert
        recoverInvoked.Should().BeFalse();
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public void CallingRecover_WhenIsError_ShouldInvokeRecoverFuncAndReturnValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Recover(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("Error count: 1");
    }

    [Fact]
    public void CallingRecover_WhenIsError_ShouldInvokeRecoverFuncAndReturnError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Recover(_ => Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingRecoverExtensionMethod_WhenIsSuccess_ShouldNotInvokeRecoverFunc()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        var recoverInvoked = false;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .Recover(_ =>
            {
                recoverInvoked = true;
                return 10;
            });

        // Assert
        recoverInvoked.Should().BeFalse();
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingRecoverExtensionMethod_WhenIsError_ShouldPassOriginalErrorsToRecoverFunc()
    {
        // Arrange
        var originalErrors = new List<Error>
        {
            Error.NotFound(description: "missing"),
            Error.Validation(description: "invalid"),
        };
        ErrorOr<string> errorOrString = originalErrors;
        var passedErrorsCount = 0;
        var passedFirstErrorType = ErrorType.Unexpected;

        // Act
        ErrorOr<string> result = await Task.FromResult(errorOrString)
            .Recover(errors =>
            {
                passedErrorsCount = errors.Count;
                passedFirstErrorType = errors[0].Type;
                return $"Recovered with {errors.Count} errors";
            });

        // Assert
        passedErrorsCount.Should().Be(2);
        passedFirstErrorType.Should().Be(ErrorType.NotFound);
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("Recovered with 2 errors");
    }
}
