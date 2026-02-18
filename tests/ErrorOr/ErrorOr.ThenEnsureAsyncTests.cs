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
