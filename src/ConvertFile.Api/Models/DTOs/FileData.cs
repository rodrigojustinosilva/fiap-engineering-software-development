namespace ConvertFile.Api.Models.DTOs;

/// <summary>
/// Representa dados estruturados do arquivo
/// </summary>
public class FileData
{
    public List<Dictionary<string, object>> Records { get; set; } = new();
    public List<string> Headers { get; set; } = new();
}
