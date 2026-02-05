namespace ConvertFile.Api.Models.DTOs;

/// <summary>
/// Response da conversão de arquivo
/// </summary>
public class ConvertFileResponse
{
    public bool Success { get; set; }
    public string? ConvertedContent { get; set; }
    public string? Message { get; set; }
    public string? ErrorDetails { get; set; }
}
