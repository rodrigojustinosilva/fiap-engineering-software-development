using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Services.Readers;

/// <summary>
/// Leitor de arquivos de posição fixa
/// Princípios: Single Responsibility (SRP), Open/Closed (OCP)
/// </summary>
public class FixedPositionReader : IFileReader
{
    public bool CanRead(string format) => 
        format.Equals("FixedPosition", StringComparison.OrdinalIgnoreCase);

    public FileData Read(string content, Dictionary<string, object>? configuration = null)
    {
        var fileData = new FileData();
        
        if (string.IsNullOrWhiteSpace(content))
            return fileData;

        // Configuração: posições dos campos
        // Exemplo: { "positions": [{"name": "Id", "start": 0, "length": 5}, ...] }
        if (configuration == null || !configuration.ContainsKey("positions"))
            throw new ArgumentException("Configuração de posições é obrigatória para formato Fixed Position");

        var positions = System.Text.Json.JsonSerializer.Deserialize<List<FieldPosition>>(
            configuration["positions"].ToString()!);

        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        fileData.Headers = positions!.Select(p => p.Name).ToList();

        foreach (var line in lines)
        {
            var record = new Dictionary<string, object>();
            
            foreach (var pos in positions!)
            {
                if (line.Length >= pos.Start + pos.Length)
                {
                    var value = line.Substring(pos.Start, pos.Length).Trim();
                    record[pos.Name] = value;
                }
            }
            
            fileData.Records.Add(record);
        }

        return fileData;
    }

    private class FieldPosition
    {
        public string Name { get; set; } = string.Empty;
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
