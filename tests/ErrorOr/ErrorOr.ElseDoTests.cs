using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ElseDoTests
{
    [Fact]
    public async Task CallingElseDo_WhenIsError_ShouldInvokeSyncAction()
    {
        // Arrange
        Task<ErrorOr<string>> errorOrTask = Task.FromResult(ErrorOrFactory.From<string>(Error.Validation()));

        // Act
        int errorCounter = 0;
        ErrorOr<string> result = await errorOrTask
            .ElseDo(error => errorCounter += error.Count);

        // Assert
        errorCounter.Should().Be(1);
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public async Task CallingElseDo_WhenIsSuccess_ShouldInvokeSyncAction()
    {
        // Arrange
        Task<ErrorOr<string>> errorOrTask = Task.FromResult(ErrorOrFactory.From<string>("5"));

        // Act
        int errorCounter = 0;
        ErrorOr<string> result = await errorOrTask
            .ElseDo(error => errorCounter += error.Count);

        // Assert
        errorCounter.Should().Be(1);
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("5");
    }

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
    public void CallingElseDo_WhenIsSuccess_ShouldInvokeGivenAction()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        int errorCounter = 0;
        ErrorOr<int> result = errorOrString
            .Then(Convert.ToInt)
            .ElseDo(error => errorCounter += error.Count())
            .Then(_ => 10);

        // Assert
        result.IsError.Should().BeFalse();
        errorCounter.Should().Be(0);
        result.Value.Should().Be(10);
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
            .ElseDoAsync(error =>
            {
                errorCounter += error.Count;
                return Task.CompletedTask;
            })
            .Else(error => $"error count is {errorCounter}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("error count is 1");
        errorCounter.Should().Be(1);
    }
}
