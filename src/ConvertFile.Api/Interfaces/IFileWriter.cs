using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Interfaces;

/// <summary>
/// Interface para escrita de arquivos
/// Princípio: Interface Segregation (ISP)
/// </summary>
public interface IFileWriter
{
    string Write(FileData data, Dictionary<string, object>? configuration = null);
    bool CanWrite(string format);
}
