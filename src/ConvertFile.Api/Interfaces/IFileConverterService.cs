using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Interfaces;

/// <summary>
/// Interface para serviço de conversão
/// Princípio: Dependency Inversion (DIP)
/// </summary>
public interface IFileConverterService
{
    ConvertFileResponse Convert(ConvertFileRequest request);
}
