using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ElseTests
{
    [Fact]
    public void CallingElseWithValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(errorOrString.Value);
    }

    [Fact]
    public void CallingElseWithValueFunc_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("Error count: 1");
    }

    [Fact]
    public void CallingElseWithValue_WhenIsSuccess_ShouldNotReturnElseValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(errorOrString.Value);
    }

    [Fact]
    public void CallingElseWithValue_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("oh no");
    }

    [Fact]
    public void CallingElseWithError_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithError_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(errorOrString.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(errorOrString.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseErrors()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .Then(Convert.ToString)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(errorOrString.Value);
    }

    [Fact]
    public async Task CallingElseWithValueAfterThenAsync_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("oh no");
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("Error count: 1");
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc_ButWithErrorOrReturn()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> GetNewValue() => "OK";

        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetNewValue());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("OK");
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldInvokeElseFunc_ButWithErrorOrReturn_ContinueChaining()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> GetNewValue() => "OK";

        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => GetNewValue())
            .Then(value => value + "!");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("OK!");
    }

    [Fact]
    public async Task CallingElseWithErrorAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .Then(Convert.ToInt)
            .ThenAsync(Convert.ToStringAsync)
            .Else(errors => [Error.Unexpected()]);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }
}
