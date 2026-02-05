using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using System.Text.Json;

namespace ConvertFile.Api.Services.Converters;

/// <summary>
/// Escritor de arquivos JSON
/// Princípios: Single Responsibility (SRP)
/// </summary>
public class JsonWriter : IFileWriter
{
    public bool CanWrite(string format) => 
        format.Equals("Json", StringComparison.OrdinalIgnoreCase);

    public string Write(FileData data, Dictionary<string, object>? configuration = null)
    {
        var indent = configuration?.ContainsKey("indent") == true 
            && bool.Parse(configuration["indent"].ToString()!);

        var options = new JsonSerializerOptions
        {
            WriteIndented = indent,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(data.Records, options);
    }
}
