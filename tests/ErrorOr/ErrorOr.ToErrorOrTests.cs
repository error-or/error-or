using ErrorOr;
using FluentAssertions;

namespace Tests;

public class ToErrorOrTests
{
    [Fact]
    public void ValueToErrorOr_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        int value = 5;

        // Act
        ErrorOr<int> result = value.ToErrorOr();

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public async Task TaskToErrorOrAsync_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        int value = 5;
        Task<int> task = Task.FromResult(value);

        // Act
        ErrorOr<int> result = await task.ToErrorOrAsync();

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ErrorToErrorOr_WhenAccessingFirstError_ShouldReturnSameError()
    {
        // Arrange
        Error error = Error.Unauthorized();

        // Act
        ErrorOr<int> result = error.ToErrorOr<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public async Task ErrorToErrorOrAsync_WhenAccessingFirstError_ShouldReturnSameError()
    {
        // Arrange
        Error error = Error.Unauthorized();
        Task<Error> task = Task.FromResult(error);

        // Act
        ErrorOr<int> result = await task.ToErrorOrAsync<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void ListOfErrorsToErrorOr_WhenAccessingErrors_ShouldReturnSameErrors()
    {
        // Arrange
        List<Error> errors = [Error.Unauthorized(), Error.Validation()];

        // Act
        ErrorOr<int> result = errors.ToErrorOr<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task ListOfErrorsToErrorOrAsync_WhenAccessingErrors_ShouldReturnSameErrors()
    {
        // Arrange
        List<Error> errors = [Error.Unauthorized(), Error.Validation()];
        Task<List<Error>> task = Task.FromResult(errors);

        // Act
        ErrorOr<int> result = await task.ToErrorOrAsync<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void EnumerableOfErrorsToErrorOr_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];

        // Act
        ErrorOr<int> result = errors.ToErrorOr<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    [Fact]
    public async Task ArrayOfErrorsToErrorOrAsync_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];
        Task<Error[]> task = Task.FromResult(errors);

        // Act
        ErrorOr<int> result = await task.ToErrorOrAsync<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }

    [Fact]
    public async Task EnumerableOfErrorsToErrorOrAsync_WhenAccessingErrors_ShouldReturnSimilarErrors()
    {
        // Arrange
        Error[] errors = [Error.Unauthorized(), Error.Validation()];
        Task<IEnumerable<Error>> task = Task.FromResult((IEnumerable<Error>)errors);

        // Act
        ErrorOr<int> result = await task.ToErrorOrAsync<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().Equal(errors);
    }
}
