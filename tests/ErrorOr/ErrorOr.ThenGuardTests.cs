using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ThenGuardTests
{
    [Fact]
    public void CallingThenGuard_WhenGuardSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenGuard(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public void CallingThenGuard_WhenGuardReturnsError_ShouldReturnGuardErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenGuard(_ => ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed")));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public void CallingThenGuard_WhenIsError_ShouldNotInvokeGuard()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Error.NotFound();
        bool called = false;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenGuard<string>(num =>
            {
                called = true;
                return $"{num}";
            });

        // Assert
        called.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void CallingThenGuardAfterThen_WhenGuardSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        int valueFromThenDo = 0;

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .ThenGuard(num => ErrorOrFactory.From<string>($"{num + 10}"))
            .ThenDo(num => valueFromThenDo = num)
            .Then(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("5");
        valueFromThenDo.Should().Be(5);
    }

    [Fact]
    public void CallingThenGuardAfterThen_WhenGuardReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        ErrorOr<int> result = errorOrInt
            .Then(num => num + 1)
            .ThenGuard(_ => ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed")))
            .ThenDo(_ => thenDoCalled = true)
            .Then(num =>
            {
                thenCalled = true;
                return num * 2;
            });

        // Assert
        thenDoCalled.Should().BeFalse();
        thenCalled.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public async Task CallingThenGuardExtensionMethod_WhenGuardSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenGuard(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public async Task CallingThenGuardExtensionMethod_WhenGuardReturnsError_ShouldReturnGuardErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenGuard(_ => ErrorOrFactory.From<string>(Error.Validation(description: "Guard failed")));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Guard failed");
    }

    [Fact]
    public async Task CallingThenGuardExtensionMethod_WhenGuardSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<string> result = await Task.FromResult(errorOrInt)
            .ThenGuard(num => ErrorOrFactory.From<string>($"{num + 100}"))
            .Then(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("5");
    }
}
