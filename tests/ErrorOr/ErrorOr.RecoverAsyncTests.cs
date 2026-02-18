using ErrorOr;
using FluentAssertions;

namespace Tests;

public class RecoverAsyncTests
{
    [Fact]
    public async Task CallingRecoverAsync_WhenIsSuccess_ShouldNotInvokeRecoverFunc()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        var recoverInvoked = false;

        // Act
        ErrorOr<int> result = await errorOrInt
            .RecoverAsync(_ =>
            {
                recoverInvoked = true;
                return Task.FromResult<ErrorOr<int>>(10);
            });

        // Assert
        recoverInvoked.Should().BeFalse();
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingRecoverAsync_WhenIsError_ShouldInvokeRecoverFuncAndReturnValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .RecoverAsync(errors => Task.FromResult<ErrorOr<string>>($"Recovered with {errors.Count} errors"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("Recovered with 1 errors");
    }

    [Fact]
    public async Task CallingRecoverAsync_WhenIsError_ShouldInvokeRecoverFuncAndReturnError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .RecoverAsync(_ => Task.FromResult<ErrorOr<string>>(Error.Conflict()));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task CallingRecoverAsyncExtensionMethod_WhenIsSuccess_ShouldNotInvokeRecoverFunc()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        var recoverInvoked = false;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .RecoverAsync(_ =>
            {
                recoverInvoked = true;
                return Task.FromResult<ErrorOr<int>>(10);
            });

        // Assert
        recoverInvoked.Should().BeFalse();
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingRecoverAsyncExtensionMethod_WhenIsError_ShouldPassOriginalErrorsToRecoverFunc()
    {
        // Arrange
        var originalErrors = new List<Error>
        {
            Error.NotFound(),
            Error.Validation(),
        };
        ErrorOr<string> errorOrString = originalErrors;
        var passedErrorsCount = 0;
        var passedFirstErrorType = ErrorType.Unexpected;

        // Act
        ErrorOr<string> result = await Task.FromResult(errorOrString)
            .RecoverAsync(errors =>
            {
                passedErrorsCount = errors.Count;
                passedFirstErrorType = errors[0].Type;
                return Task.FromResult<ErrorOr<string>>(Error.Unexpected());
            });

        // Assert
        passedErrorsCount.Should().Be(2);
        passedFirstErrorType.Should().Be(ErrorType.NotFound);
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }
}
