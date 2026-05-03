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
    public void CreateFromValue_WhenAccessingValue_ViaStronglyTypedInterface_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IErrorOr<IEnumerable<string>> errorOrValue = ErrorOrFactory.From(value);
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Assert
        errorOrValue.IsError.Should().BeFalse();
        errorOrValue.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ViaIRecordable_ShouldReturnJson()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IRecordable errorOrValue = ErrorOrFactory.From(value);
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Assert
        errorOrValue.GetRecording().Should().Be(System.Text.Json.JsonSerializer.Serialize(value, new System.Text.Json.JsonSerializerOptions { WriteIndented = true, IncludeFields = true }));
    }

    [Fact]
    public void CreateFromError_WhenAccessingValue_ViaIRecordable_ShouldReturnJsonErrors()
    {
        // Arrange
        var error = Error.Unexpected();
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IRecordable errorOrValue = (ErrorOr<string>)error;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Act
        var recording = errorOrValue.GetRecording();

        // Assert
        recording.Should().Be(System.Text.Json.JsonSerializer.Serialize(new[] { error }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true, IncludeFields = true, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() } }));
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
    public async Task ImplicitCastConditionalToObject_WhenBranchesAreDifferentErrorOrTypes_ShouldNestErrorOrValue()
    {
        // Repro guard for issue #144: conditional branch assignment can wrap ErrorOr<T> into ErrorOr<object>.
        var useFirstBranch = true;

        ErrorOr<object> request = useFirstBranch
            ? await CreateRequestModel1Async()
            : await CreateRequestModel2Async();

        request.IsError.Should().BeFalse();
        request.Value.Should().BeOfType<ErrorOr<ModelClass1>>();

        return;

        static Task<ErrorOr<ModelClass1>> CreateRequestModel1Async()
        {
            return Task.FromResult<ErrorOr<ModelClass1>>(new ModelClass1("first"));
        }

        static Task<ErrorOr<ModelClass2>> CreateRequestModel2Async()
        {
            return Task.FromResult<ErrorOr<ModelClass2>>(new ModelClass2(7));
        }
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

    private record ModelClass1(string Name);

    private record ModelClass2(int Count);

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

#pragma warning disable SA1129 // Do not use default value type constructor
    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new ErrorOr<int>();

        // Act &Assert
        errorOrInt.IsError.Should().BeFalse();
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new ErrorOr<int>();

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new ErrorOr<int>();

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateWithEmptyConstructor_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new ErrorOr<int>();

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }
#pragma warning restore SA1129 // Do not use default value type constructor

    [Fact]
    public void CreateWithDefault_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default;

        // Act & Assert
        errorOrInt.IsError.Should().BeFalse();
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default;

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default;

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ErrorOr_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        ErrorOr<Person> errorOrPerson = [nameTooShort, userTooYoung];

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(2).And.BeEquivalentTo([nameTooShort, userTooYoung]);
    }

    [Fact]
    public void GenericErrorOrInterface_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        IErrorOr<Person> errorOrPerson = [nameTooShort, userTooYoung];

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(2).And.BeEquivalentTo([nameTooShort, userTooYoung]);
    }

    [Fact]
    public void ErrorOrInterface_FromErrorCollectionExpression_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        var nameTooShort = Error.Validation("User.Name", "Name is too short");
        var userTooYoung = Error.Validation("User.Age", "User is too young");

        // Act
        IErrorOr errorOrPerson = [nameTooShort, userTooYoung];

        // Assert
        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(2).And.BeEquivalentTo([nameTooShort, userTooYoung]);
    }

    [Fact]
    public void CreateWithDefault_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default;

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_ShouldBeError()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new List<Error>();

        // Act & Assert
        errorOrInt.IsError.Should().BeTrue();
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new List<Error>();

        // Act & Assert
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new List<Error>();

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new List<Error>();

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorList_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = new List<Error>();

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_ShouldBeError()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Array.Empty<Error>();

        // Act & Assert
        errorOrInt.IsError.Should().BeTrue();
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Array.Empty<Error>();

        // Act & Assert
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Array.Empty<Error>();

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Array.Empty<Error>();

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastEmptyErrorArray_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = Array.Empty<Error>();

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingValue_ShouldReturnNull()
    {
        // Arrange
        ErrorOr<int?> errorOrInt = default(int?);

        // Act & Assert
        errorOrInt.IsError.Should().BeFalse();
        errorOrInt.Value.Should().BeNull();
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int?> errorOrInt = default(int?);

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        ErrorOr<int?> errorOrInt = default(int?);

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitCastNullValue_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int?> errorOrInt = default(int?);

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorList_ShouldBeError()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(List<Error>)!;

        // Act & Assert
        errorOrInt.IsError.Should().BeTrue();
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(List<Error>)!;

        // Act & Assert
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(List<Error>)!;

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(List<Error>)!;

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorList_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(List<Error>)!;

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_ShouldBeError()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(Error[])!;

        // Act & Assert
        errorOrInt.IsError.Should().BeTrue();
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(Error[])!;

        // Act & Assert
        errorOrInt.Value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingErrors_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(Error[])!;

        // Act
        List<Error> errors = errorOrInt.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingErrorsOrEmptyList_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(Error[])!;

        // Act
        List<Error> errors = errorOrInt.ErrorsOrEmptyList;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastNullErrorArray_WhenAccessingFirstError_ShouldReturnUnexpected()
    {
        // Arrange
        ErrorOr<int> errorOrInt = default(Error[])!;

        // Act
        Error firstError = errorOrInt.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }
}
