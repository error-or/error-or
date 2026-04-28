using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ElseDoTests
{
    [Fact]
    public void CallingElseDo_WhenIsError_ShouldInvokeGivenAction()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.Validation();

        // Act
        int errorCounter = 0;
        ErrorOr<int> result = errorOrString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count())
            .Then(_ => 10);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        errorCounter.Should().Be(1);
    }

    [Fact]
    public void CallingElseDo_WhenIsSuccess_ShouldNotInvokeGivenAction()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        int errorCounter = 0;
        ErrorOr<int> result = errorOrString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
        errorCounter.Should().Be(0);
    }

    [Fact]
    public void CallingElseDo_WhenIsError_ShouldInvokeNextElse()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.Failure();

        // Act
        int errorCounter = 0;
        ErrorOr<string> result = errorOrString
            .ElseDo(error => errorCounter += error.Count())
            .Else(error => $"error count is {errorCounter}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("error count is 1");
        errorCounter.Should().Be(1);
    }

    [Fact]
    public async Task CallingElseDoAsync_WhenIsError_ShouldInvokeNextElse()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.Validation();

        // Act
        int errorCounter = 0;
        ErrorOr<string> result = await errorOrString
            .ElseDoAsync(error => Task.Run(() => errorCounter += error.Count()))
            .Else(error => $"error count is {errorCounter}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("error count is 1");
        errorCounter.Should().Be(1);
    }

    [Fact]
    public async Task CallingThenAsync_WhenIsSuccess_ShouldNotInvokeGivenAction()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        int errorCounter = 0;
        ErrorOr<int> result = await errorOrString
            .ElseDoAsync(error => Task.Run(() => errorCounter += error.Count()))
            .Then(Convert.ToInt)
            .Then(x => x * 2);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(10);
        errorCounter.Should().Be(0);
    }
}
