using ConvertFile.Api.Services.Readers;
using FluentAssertions;
using System.Text.Json;

namespace ConvertFile.Tests.Readers;

public class FixedPositionReaderTests
{
    [Fact]
    public void Read_WithValidFixedPositionFile_ShouldParseCorrectly()
    {
        // Arrange
        var reader = new FixedPositionReader();
        var content = "00001John Doe    30\n00002Jane Smith  25";
        
        var positions = new[]
        {
            new { Name = "Id", Start = 0, Length = 5 },
            new { Name = "Name", Start = 5, Length = 12 },
            new { Name = "Age", Start = 17, Length = 2 }
        };

        var config = new Dictionary<string, object>
        {
            { "positions", JsonSerializer.Serialize(positions) }
        };

        // Act
        var result = reader.Read(content, config);

        // Assert
        result.Should().NotBeNull();
        result.Records.Should().HaveCount(2);
        result.Headers.Should().BeEquivalentTo(new[] { "Id", "Name", "Age" });
        
        result.Records[0]["Id"].ToString().Should().Be("00001");
        result.Records[0]["Name"].ToString().Should().Be("John Doe");
        result.Records[0]["Age"].ToString().Should().Be("30");
    }

    [Fact]
    public void CanRead_WithFixedPositionFormat_ShouldReturnTrue()
    {
        // Arrange
        var reader = new FixedPositionReader();

        // Act
        var result = reader.CanRead("FixedPosition");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Read_WithoutConfiguration_ShouldThrowException()
    {
        // Arrange
        var reader = new FixedPositionReader();
        var content = "00001John Doe    30";

        // Act
        Action act = () => reader.Read(content);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*posições é obrigatória*");
    }
}
