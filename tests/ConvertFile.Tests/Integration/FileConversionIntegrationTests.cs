using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using ConvertFile.Api.Services;
using ConvertFile.Api.Services.Converters;
using ConvertFile.Api.Services.Readers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace ConvertFile.Tests.Integration;

public class FileConversionIntegrationTests
{
    private readonly IFileConverterService _converterService;

    public FileConversionIntegrationTests()
    {
        var readers = new List<IFileReader>
        {
            new FixedPositionReader(),
            new DelimitedReader(),
            new JsonReader()
        };

        var writers = new List<IFileWriter>
        {
            new FixedPositionWriter(),
            new DelimitedWriter(),
            new JsonWriter()
        };

        var logger = new Mock<ILogger<FileConverterService>>();
        
        _converterService = new FileConverterService(readers, writers, logger.Object);
    }

    [Fact]
    public void Convert_FromFixedPositionToJson_ShouldSucceed()
    {
        // Arrange
        var content = "00001John Doe    30\n00002Jane Smith  25";
        
        var positions = new[]
        {
            new { Name = "Id", Start = 0, Length = 5 },
            new { Name = "Name", Start = 5, Length = 12 },
            new { Name = "Age", Start = 17, Length = 2 }
        };

        var request = new ConvertFileRequest
        {
            FileName = "test.txt",
            FileContent = content,
            SourceFormat = "FixedPosition",
            TargetFormat = "Json",
            Configuration = new Dictionary<string, object>
            {
                { "positions", JsonSerializer.Serialize(positions) },
                { "indent", "true" }
            }
        };

        // Act
        var response = _converterService.Convert(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ConvertedContent.Should().NotBeNullOrEmpty();
        response.ConvertedContent.Should().Contain("John Doe");
        response.ConvertedContent.Should().Contain("Jane Smith");
    }

    [Fact]
    public void Convert_FromDelimitedToJson_ShouldSucceed()
    {
        // Arrange
        var content = "Id,Name,Age\n1,John Doe,30\n2,Jane Smith,25";
        
        var request = new ConvertFileRequest
        {
            FileName = "test.csv",
            FileContent = content,
            SourceFormat = "Delimited",
            TargetFormat = "Json",
            Configuration = new Dictionary<string, object>
            {
                { "delimiter", "," },
                { "hasHeader", "true" },
                { "indent", "true" }
            }
        };

        // Act
        var response = _converterService.Convert(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ConvertedContent.Should().Contain("John Doe");
        response.Message.Should().Be("Arquivo convertido com sucesso");
    }

    [Fact]
    public void Convert_FromJsonToDelimited_ShouldSucceed()
    {
        // Arrange
        var content = @"[
            {""id"": ""1"", ""name"": ""John Doe"", ""age"": ""30""},
            {""id"": ""2"", ""name"": ""Jane Smith"", ""age"": ""25""}
        ]";
        
        var request = new ConvertFileRequest
        {
            FileName = "test.json",
            FileContent = content,
            SourceFormat = "Json",
            TargetFormat = "Delimited",
            Configuration = new Dictionary<string, object>
            {
                { "delimiter", "," },
                { "includeHeader", "true" }
            }
        };

        // Act
        var response = _converterService.Convert(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ConvertedContent.Should().Contain("id,name,age");
        response.ConvertedContent.Should().Contain("John Doe");
    }

    [Fact]
    public void Convert_WithUnsupportedSourceFormat_ShouldFail()
    {
        // Arrange
        var request = new ConvertFileRequest
        {
            FileContent = "test",
            SourceFormat = "UnsupportedFormat",
            TargetFormat = "Json"
        };

        // Act
        var response = _converterService.Convert(request);

        // Assert
        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não suportado");
    }
}
