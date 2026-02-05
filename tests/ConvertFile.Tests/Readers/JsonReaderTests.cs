using ConvertFile.Api.Services.Readers;
using FluentAssertions;

namespace ConvertFile.Tests.Readers;

public class JsonReaderTests
{
    [Fact]
    public void Read_WithValidJsonArray_ShouldParseCorrectly()
    {
        // Arrange
        var reader = new JsonReader();
        var content = @"[
            {""id"": ""1"", ""name"": ""John Doe"", ""age"": ""30""},
            {""id"": ""2"", ""name"": ""Jane Smith"", ""age"": ""25""}
        ]";

        // Act
        var result = reader.Read(content);

        // Assert
        result.Should().NotBeNull();
        result.Records.Should().HaveCount(2);
        result.Headers.Should().Contain("id");
        result.Headers.Should().Contain("name");
        result.Records[0]["name"].ToString().Should().Be("John Doe");
    }

    [Fact]
    public void Read_WithInvalidJson_ShouldThrowException()
    {
        // Arrange
        var reader = new JsonReader();
        var content = "{ invalid json }";

        // Act
        Action act = () => reader.Read(content);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*parse do JSON*");
    }

    [Fact]
    public void CanRead_WithJsonFormat_ShouldReturnTrue()
    {
        // Arrange
        var reader = new JsonReader();

        // Act
        var result = reader.CanRead("Json");

        // Assert
        result.Should().BeTrue();
    }
}
