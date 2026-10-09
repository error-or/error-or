using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ThenGuardAsyncTests
{
    [Fact]
    public async Task CallingThenGuardAsync_WhenGuardSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenGuardAsync(Convert.ToStringAsync);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingThenGuardAsync_WhenGuardReturnsError_ShouldReturnGuardErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenGuardAsync(_ => Task.FromResult(ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed"))));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public async Task CallingThenGuardAsync_WhenIsError_ShouldNotInvokeGuard()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Error.NotFound();
        bool called = false;

        // Act
        ErrorOr<int> result = await errorOrInt
            .ThenGuardAsync<string>(num =>
            {
                called = true;
                return Task.FromResult<ErrorOr<string>>($"{num}");
            });

        // Assert
        called.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CallingThenGuardAsyncAfterThenAsync_WhenGuardSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        int valueFromThenDo = 0;

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num * 2)
            .ThenGuardAsync(num => Task.FromResult(ErrorOrFactory.From<string>($"{num + 100}")))
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
    public async Task CallingThenGuardAsyncAfterThenAsync_WhenGuardReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .Then(num => num + 1)
            .ThenGuardAsync(_ => Task.FromResult(ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed"))))
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
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public async Task CallingThenGuardAsyncExtensionMethod_WhenGuardSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenGuardAsync(Convert.ToStringAsync);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingThenGuardAsyncExtensionMethod_WhenGuardReturnsError_ShouldReturnGuardErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenGuardAsync(_ => Task.FromResult(ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed"))));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public async Task CallingThenGuardAsyncExtensionMethod_WhenGuardSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<string> result = await Task.FromResult(errorOrInt)
            .ThenGuardAsync(num => Task.FromResult(ErrorOrFactory.From<string>($"{num + 100}")))
            .ThenAsync(Convert.ToStringAsync);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("5");
    }
}
