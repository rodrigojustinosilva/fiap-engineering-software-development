using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Interfaces;

/// <summary>
/// Interface para leitura de arquivos
/// Princípio: Interface Segregation (ISP)
/// </summary>
public interface IFileReader
{
    FileData Read(string content, Dictionary<string, object>? configuration = null);
    bool CanRead(string format);
}
