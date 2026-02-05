using ConvertFile.Api.Services.Readers;
using FluentAssertions;

namespace ConvertFile.Tests.Readers;

public class DelimitedReaderTests
{
    [Fact]
    public void Read_WithValidCsvFile_ShouldParseCorrectly()
    {
        // Arrange
        var reader = new DelimitedReader();
        var content = "Id,Name,Age\n1,John Doe,30\n2,Jane Smith,25";
        
        var config = new Dictionary<string, object>
        {
            { "delimiter", "," },
            { "hasHeader", "true" }
        };

        // Act
        var result = reader.Read(content, config);

        // Assert
        result.Should().NotBeNull();
        result.Records.Should().HaveCount(2);
        result.Headers.Should().BeEquivalentTo(new[] { "Id", "Name", "Age" });
        
        result.Records[0]["Name"].ToString().Should().Be("John Doe");
        result.Records[1]["Age"].ToString().Should().Be("25");
    }

    [Fact]
    public void Read_WithTabDelimiter_ShouldParseCorrectly()
    {
        // Arrange
        var reader = new DelimitedReader();
        var content = "Id\tName\tAge\n1\tJohn\t30";
        
        var config = new Dictionary<string, object>
        {
            { "delimiter", "\t" },
            { "hasHeader", "true" }
        };

        // Act
        var result = reader.Read(content, config);

        // Assert
        result.Records.Should().HaveCount(1);
        result.Records[0]["Name"].ToString().Should().Be("John");
    }

    [Fact]
    public void CanRead_WithDelimitedFormat_ShouldReturnTrue()
    {
        // Arrange
        var reader = new DelimitedReader();

        // Act
        var result = reader.CanRead("Delimited");

        // Assert
        result.Should().BeTrue();
    }
}
