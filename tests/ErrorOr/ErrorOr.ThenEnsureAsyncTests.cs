using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ThenEnsureAsyncTests
{
    [Fact]
    public async Task CallingThenEnsureAsync_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenEnsureAsync(num => Task.FromResult<ErrorOr<int>>(num > 3 ? num + 10 : Error.Validation()));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingThenEnsureAsync_WhenEnsureReturnsError_ShouldReturnEnsureErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenEnsureAsync(num => Task.FromResult<ErrorOr<int>>(num > 3 ? Error.Validation(description: $"{num} is too big") : num));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("5 is too big");
    }

    [Fact]
    public async Task CallingThenEnsureAsync_WhenIsError_ShouldNotInvokeEnsure()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Error.NotFound();
        bool called = false;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenEnsureAsync(num =>
            {
                called = true;
                return Task.FromResult<ErrorOr<int>>(num);
            });

        // Assert
        called.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CallingThenEnsureAsyncAfterThenAsync_WhenEnsureSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        int valueFromThenDo = 0;

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num * 2)
            .ThenEnsureAsync(num => Task.FromResult<ErrorOr<int>>(num > 3 ? num + 100 : Error.Validation()))
            .ThenDoAsync(num =>
            {
                valueFromThenDo = num;
                return Task.CompletedTask;
            })
            .Then(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("10");
        valueFromThenDo.Should().Be(10);
    }

    [Fact]
    public async Task CallingThenEnsureAsyncAfterThenAsync_WhenEnsureReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num + 1)
            .ThenEnsureAsync(num => Task.FromResult<ErrorOr<int>>(num > 3 ? Error.Validation(description: $"{num} is too big") : num))
            .ThenDoAsync(_ =>
            {
                thenDoCalled = true;
                return Task.CompletedTask;
            })
            .Then(num =>
            {
                thenCalled = true;
                return num.ToString();
            });

        // Assert
        thenDoCalled.Should().BeFalse();
        thenCalled.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("6 is too big");
    }

    [Fact]
    public async Task CallingThenEnsureAsyncExtensionMethod_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenEnsureAsync(num => Task.FromResult<ErrorOr<int>>(num > 3 ? num + 100 : Error.Validation()));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }
}
