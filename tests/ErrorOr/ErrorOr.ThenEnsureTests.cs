using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ThenEnsureTests
{
    [Fact]
    public void CallingThenEnsure_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenEnsure(num => num > 10 ? Error.Validation() : num + 10);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public void CallingThenEnsure_WhenEnsureReturnsError_ShouldReturnEnsureErrors()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenEnsure(num => num > 3 ? Error.Validation(description: $"{num} is too big") : num);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("5 is too big");
    }

    [Fact]
    public void CallingThenEnsure_WhenIsError_ShouldNotInvokeEnsure()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Error.NotFound();
        bool called = false;

        // Act
        ErrorOr<int> result = errorOrInt
            .ThenEnsure(num =>
            {
                called = true;
                return num;
            });

        // Assert
        called.Should().BeFalse();
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void CallingThenEnsureAfterThen_WhenEnsureSucceeds_ShouldContinueChainWithOriginalValue()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";
        int valueFromThenDo = 0;

        // Act
        ErrorOr<string> result = errorOrString
            .Then(Convert.ToInt)
            .ThenEnsure(num => num > 3 ? num + 10 : Error.Validation())
            .ThenDo(num => valueFromThenDo = num)
            .Then(Convert.ToString);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("5");
        valueFromThenDo.Should().Be(5);
    }

    [Fact]
    public void CallingThenEnsureAfterThen_WhenEnsureReturnsError_ShouldNotInvokeLaterChain()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;
        bool thenDoCalled = false;
        bool thenCalled = false;

        // Act
        ErrorOr<int> result = errorOrInt
            .Then(num => num + 1)
            .ThenEnsure(num => num > 3 ? Error.Validation(description: $"{num} is too big") : num)
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
        result.FirstError.Description.Should().Be("6 is too big");
    }

    [Fact]
    public async Task CallingThenEnsureExtensionMethod_WhenEnsureSucceeds_ShouldReturnOriginalValue()
    {
        // Arrange
        ErrorOr<int> errorOrInt = 5;

        // Act
        ErrorOr<int> result = await Task.FromResult(errorOrInt)
            .ThenEnsure(num => num > 3 ? num + 100 : Error.Validation());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }
}
