namespace Tests;

using ErrorOr;

using FluentAssertions;

public class ErrorOrInstantiationTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        ErrorOr<IEnumerable<string>> errorOrPerson = ErrorOrFactory.From(value);

        // Assert
        errorOrPerson.IsError.Should().BeFalse();
        errorOrPerson.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        ErrorOr<IEnumerable<string>> errorOrPerson = ErrorOrFactory.From(value);

        // Act
        List<Error> errors = errorOrPerson.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        ErrorOr<IEnumerable<string>> errorOrPerson = ErrorOrFactory.From(value);

        // Act
        List<Error> errors = errorOrPerson.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        ErrorOr<IEnumerable<string>> errorOrPerson = ErrorOrFactory.From(value);

        // Act
        Error firstError = errorOrPerson.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = ErrorOrFactory.From<Person>([error]);

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    [Obsolete]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = [Error.Validation("User.Name", "Name is too short")];
        ErrorOr<Person> errorOrPerson = ErrorOr<Person>.From(errors);

        // Act & Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        // Arrange
        var error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = ErrorOrFactory.From<Person>([error]);

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.ErrorsOrEmptyList.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    [Obsolete]
    public void CreateFromErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = [Error.Validation("User.Name", "Name is too short")];
        ErrorOr<Person> errorOrPerson = ErrorOr<Person>.From(errors);

        // Act & Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.ErrorsOrEmptyList.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_UsingFactory_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = ErrorOrFactory.From<Person>([Error.Validation("User.Name", "Name is too short")]);

        // Act
        Person value = errorOrPerson.Value;

        // Assert
        value.Should().Be(default);
    }

    [Fact]
    [Obsolete]
    public void CreateFromErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        List<Error> errors = [Error.Validation("User.Name", "Name is too short")];
        ErrorOr<Person> errorOrPerson = ErrorOr<Person>.From(errors);

        // Act
        Person value = errorOrPerson.Value;

        // Assert
        value.Should().Be(default);
    }

    [Fact]
    public void CreateFromSingleError_UsingFactory_ShouldBeError()
    {
        // Act
        ErrorOr<Person> errorOrPerson = ErrorOrFactory.From<Person>(Error.Validation("User.Name", "Name is too short"));

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
    }

    [Fact]
    public void CreateFromArrayOfErrors_UsingFactory_ShouldBeError()
    {
        // Arrange
        Error[] errors = [
            Error.Validation("User.Name", "Name is too short"),
            Error.Forbidden("User.Forbidden", "You are not allowed to create user")
        ];

        // Act
        ErrorOr<Person> errorOrPerson = ErrorOrFactory.From<Person>(errors);

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        Person result = new Person("Amici");

        // Act
        ErrorOr<Person> errorOr = result;

        // Assert
        errorOr.IsError.Should().BeFalse();
        errorOr.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        ErrorOr<Person> errorOrPerson = new Person("Amichai");

        // Act
        List<Error> errors = errorOrPerson.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        ErrorOr<Person> errorOrPerson = new Person("Amichai");

        // Act
        Error firstError = errorOrPerson.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        const int result = 4;

        // Act
        ErrorOr<int> errorOrInt = result;

        // Assert
        errorOrInt.IsError.Should().BeFalse();
        errorOrInt.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastErrorOrType_WhenAccessingResult_ShouldReturnValue()
    {
        // Act
        ErrorOr<Success> errorOrSuccess = Result.Success;
        ErrorOr<Created> errorOrCreated = Result.Created;
        ErrorOr<Deleted> errorOrDeleted = Result.Deleted;
        ErrorOr<Updated> errorOrUpdated = Result.Updated;

        // Assert
        errorOrSuccess.IsError.Should().BeFalse();
        errorOrSuccess.Value.Should().Be(Result.Success);

        errorOrCreated.IsError.Should().BeFalse();
        errorOrCreated.Value.Should().Be(Result.Created);

        errorOrDeleted.IsError.Should().BeFalse();
        errorOrDeleted.Value.Should().Be(Result.Deleted);

        errorOrUpdated.IsError.Should().BeFalse();
        errorOrUpdated.Value.Should().Be(Result.Updated);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = error;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = Error.Validation("User.Name", "Name is too short");

        // Act
        Person value = errorOrPerson.Value;

        // Assert
        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = error;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(errors.Count).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void CreateErrorOr_WhenUsingEmptyConstructor_ShouldThrow()
    {
        // Act
        Func<ErrorOr<int>> action = () => new ErrorOr<int>();

        // Assert
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyErrorsList_ShouldThrow()
    {
        // Act
        Func<ErrorOr<int>> errorOrInt = () => new List<Error>();

        // Assert
        var exception = errorOrInt.Should().ThrowExactly<ArgumentException>().Which;
        exception.Message.Should().Be("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error. (Parameter 'errors')");
        exception.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyErrorsArray_ShouldThrow()
    {
        // Act
        Func<ErrorOr<int>> errorOrInt = () => Array.Empty<Error>();

        // Assert
        var exception = errorOrInt.Should().ThrowExactly<ArgumentException>().Which;
        exception.Message.Should().Be("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error. (Parameter 'errors')");
        exception.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsList_ShouldThrowArgumentNullException()
    {
        Func<ErrorOr<int>> act = () => default(List<Error>)!;

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsArray_ShouldThrowArgumentNullException()
    {
        Func<ErrorOr<int>> act = () => default(Error[])!;

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenValueIsNull_ShouldThrowArgumentNullException()
    {
        Func<ErrorOr<int?>> act = () => default(int?);

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("value");
    }
}
