using ErrorOr;
using FluentAssertions;

namespace Tests;

public sealed class ErrorOrEqualityTests
{
    // ReSharper disable once NotAccessedPositionalProperty.Local -- we require this property for these tests
    private record Person(string Name);

    public static readonly TheoryData<Error[], Error[]> DifferentErrors =
        new()
        {
            {
                // Different number of entries
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                },
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                    Error.Validation("User.Age", "User is too young"),
                }
            },
            {
                // Different errors
                new[]
                {
                    Error.Validation("User.Name", "Name is too short"),
                },
                new[]
                {
                    Error.Validation("User.Age", "User is too young"),
                }
            },
        };

    public static readonly TheoryData<string> Names = new() { "Amichai", "feO2x" };

    public static readonly TheoryData<string, string> DifferentNames =
        new() { { "Amichai", "feO2x" }, { "Tyrion", "Cersei" } };

    [Fact]
    public void Equals_WhenTwoInstancesHaveTheSameErrorsCollection_ShouldReturnTrue()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        ErrorOr<Person> errorOrPerson1 = errors;
        ErrorOr<Person> errorOrPerson2 = errors;

        var result = errorOrPerson1.Equals(errorOrPerson2);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenTwoInstancesHaveDifferentErrorCollectionsWithSameErrors_ShouldReturnTrue()
    {
        var errors1 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        var errors2 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        ErrorOr<Person> errorOrPerson1 = errors1;
        ErrorOr<Person> errorOrPerson2 = errors2;

        var result = errorOrPerson1.Equals(errorOrPerson2);

        result.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(DifferentErrors))]
    public void Equals_WhenTwoInstancesHaveDifferentErrors_ShouldReturnFalse(Error[] errors1, Error[] errors2)
    {
        ErrorOr<Person> errorOrPerson1 = errors1;
        ErrorOr<Person> errorOrPerson2 = errors2;

        var result = errorOrPerson1.Equals(errorOrPerson2);

        result.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(Names))]
    public void Equals_WhenTwoInstancesHaveEqualValues_ShouldReturnTrue(string name)
    {
        ErrorOr<Person> errorOrPerson1 = new Person(name);
        ErrorOr<Person> errorOrPerson2 = new Person(name);

        var result = errorOrPerson1.Equals(errorOrPerson2);

        result.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(DifferentNames))]
    public void Equals_WhenTwoInstancesHaveDifferentValues_ShouldReturnFalse(string name1, string name2)
    {
        ErrorOr<Person> errorOrPerson1 = new Person(name1);
        ErrorOr<Person> errorOrPerson2 = new Person(name2);

        var result = errorOrPerson1.Equals(errorOrPerson2);

        result.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(Names))]
    public void GetHashCode_WhenTwoInstancesHaveEqualValues_ShouldReturnSameHashCode(string name)
    {
        ErrorOr<Person> errorOrPerson1 = new Person(name);
        ErrorOr<Person> errorOrPerson2 = new Person(name);

        var hashCode1 = errorOrPerson1.GetHashCode();
        var hashCode2 = errorOrPerson2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Theory]
    [MemberData(nameof(DifferentNames))]
    public void GetHashCode_WhenTwoInstanceHaveDifferentValues_ShouldReturnDifferentHashCodes(
        string name1,
        string name2)
    {
        ErrorOr<Person> errorOrPerson1 = new Person(name1);
        ErrorOr<Person> errorOrPerson2 = new Person(name2);

        var hashCode1 = errorOrPerson1.GetHashCode();
        var hashCode2 = errorOrPerson2.GetHashCode();

        hashCode1.Should().NotBe(hashCode2);
    }

    [Fact]
    public void GetHashCode_WhenValueIsNull_ShouldNotThrow()
    {
        ErrorOr<Person?> errorOrPerson = (Person?)null;

        var act = () => errorOrPerson.GetHashCode();

        act.Should().NotThrow();
    }

    [Fact]
    public void GetHashCode_WhenBothValuesAreNull_ShouldReturnSameHashCode()
    {
        ErrorOr<Person?> errorOrPerson1 = (Person?)null;
        ErrorOr<Person?> errorOrPerson2 = (Person?)null;

        var hashCode1 = errorOrPerson1.GetHashCode();
        var hashCode2 = errorOrPerson2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void GetHashCode_WhenTwoInstancesHaveEqualErrors_ShouldReturnSameHashCode()
    {
        var errors1 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        var errors2 = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };
        ErrorOr<Person> errorOrPerson1 = errors1;
        ErrorOr<Person> errorOrPerson2 = errors2;

        var hashCode1 = errorOrPerson1.GetHashCode();
        var hashCode2 = errorOrPerson2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Theory]
    [MemberData(nameof(DifferentErrors))]
    public void GetHashCode_WhenTwoInstancesHaveDifferentErrors_ShouldReturnDifferentHashCodes(
        Error[] errors1,
        Error[] errors2)
    {
        ErrorOr<Person> errorOrPerson1 = errors1;
        ErrorOr<Person> errorOrPerson2 = errors2;

        var hashCode1 = errorOrPerson1.GetHashCode();
        var hashCode2 = errorOrPerson2.GetHashCode();

        hashCode1.Should().NotBe(hashCode2);
    }
}
