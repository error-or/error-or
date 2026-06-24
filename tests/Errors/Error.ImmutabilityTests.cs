using ErrorOr;

using FluentAssertions;

namespace Tests;

public class ErrorImmutabilityTests
{
    [Fact]
    public void ModificationToUnderlyingErrorMetadataDictionary_UsedToCreateError_ShouldNotAffectTheErrorInstance()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 21 },
            { "key3", true },
        };

        var error = Error.Failure("ErrorCode", "ErrorDescription", metadata);

        // Act
        metadata["key1"] = "newValue";
        metadata["key4"] = "value4";
        metadata.Remove("key3");

        // Assert
        error.Metadata.Should().ContainKeys("key1", "key2", "key3");
        error.Metadata.Should().NotContainKey("key4");
        error.Metadata["key1"].Should().Be("value1");
    }

    [Fact]
    public void ModificationToUnderlyingErrorMetadataDictionary_UsedToCreateErrorFromReadOnlyDictionary_ShouldNotAffectTheErrorInstance()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 21 },
            { "key3", true },
        };

        var error = Error.Failure("ErrorCode", "ErrorDescription", new ReadOnlyDictionary<string, object>(metadata));

        // Act
        metadata["key1"] = "newValue";
        metadata["key4"] = "value4";
        metadata.Remove("key3");

        // Assert
        error.Metadata.Should().ContainKeys("key1", "key2", "key3");
        error.Metadata.Should().NotContainKey("key4");
        error.Metadata["key1"].Should().Be("value1");
    }
}
