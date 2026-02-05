using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Services;

/// <summary>
/// Serviço de conversão de arquivos
/// Princípios: Dependency Inversion (DIP), Open/Closed (OCP)
/// </summary>
public class FileConverterService : IFileConverterService
{
    private readonly IEnumerable<IFileReader> _readers;
    private readonly IEnumerable<IFileWriter> _writers;
    private readonly ILogger<FileConverterService> _logger;

    public FileConverterService(
        IEnumerable<IFileReader> readers,
        IEnumerable<IFileWriter> writers,
        ILogger<FileConverterService> logger)
    {
        _readers = readers;
        _writers = writers;
        _logger = logger;
    }

    public ConvertFileResponse Convert(ConvertFileRequest request)
    {
        try
        {
            _logger.LogInformation(
                "Iniciando conversão de {SourceFormat} para {TargetFormat}", 
                request.SourceFormat, 
                request.TargetFormat);

            // Encontrar reader adequado (Liskov Substitution Principle)
            var reader = _readers.FirstOrDefault(r => r.CanRead(request.SourceFormat));
            if (reader == null)
            {
                return new ConvertFileResponse
                {
                    Success = false,
                    Message = $"Formato de origem '{request.SourceFormat}' não suportado"
                };
            }

            // Encontrar writer adequado
            var writer = _writers.FirstOrDefault(w => w.CanWrite(request.TargetFormat));
            if (writer == null)
            {
                return new ConvertFileResponse
                {
                    Success = false,
                    Message = $"Formato de destino '{request.TargetFormat}' não suportado"
                };
            }

            // Ler arquivo de origem
            var fileData = reader.Read(request.FileContent, request.Configuration);

            // Escrever no formato de destino
            var convertedContent = writer.Write(fileData, request.Configuration);

            _logger.LogInformation("Conversão concluída com sucesso");

            return new ConvertFileResponse
            {
                Success = true,
                ConvertedContent = convertedContent,
                Message = "Arquivo convertido com sucesso"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao converter arquivo");
            
            return new ConvertFileResponse
            {
                Success = false,
                Message = "Erro ao converter arquivo",
                ErrorDetails = ex.Message
            };
        }
    }
}
