namespace ConvertFile.Api.Models.DTOs;

/// <summary>
/// Request para conversão de arquivo
/// </summary>
public class ConvertFileRequest
{
    public string FileName { get; set; } = string.Empty;
    public string FileContent { get; set; } = string.Empty;
    public string SourceFormat { get; set; } = string.Empty;
    public string TargetFormat { get; set; } = string.Empty;
    public Dictionary<string, object>? Configuration { get; set; }
}
