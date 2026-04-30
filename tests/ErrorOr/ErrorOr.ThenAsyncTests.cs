using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ThenAsyncTests
{
    [Fact]
    public async Task CallingThenAsync_WhenIsSuccess_ShouldInvokeNextThen()
    {
        // Arrange
        ErrorOr<string> errorOrString = "5";

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenDoAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(Convert.ToStringAsync);

        // Assert
        result.IsError.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo("10");
    }

    [Fact]
    public async Task CallingThenAsync_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        ErrorOr<string> errorOrString = Error.NotFound();

        // Act
        ErrorOr<string> result = await errorOrString
            .ThenAsync(Convert.ToIntAsync)
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenDoAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(Convert.ToStringAsync);

        // Assert
        result.IsError.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.FirstError.Should().BeEquivalentTo(errorOrString.FirstError);
    }
}
