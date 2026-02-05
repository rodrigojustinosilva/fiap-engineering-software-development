using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using System.Text;

namespace ConvertFile.Api.Services.Converters;

/// <summary>
/// Escritor de arquivos de posição fixa
/// Princípios: Single Responsibility (SRP)
/// </summary>
public class FixedPositionWriter : IFileWriter
{
    public bool CanWrite(string format) => 
        format.Equals("FixedPosition", StringComparison.OrdinalIgnoreCase);

    public string Write(FileData data, Dictionary<string, object>? configuration = null)
    {
        if (configuration == null || !configuration.ContainsKey("positions"))
            throw new ArgumentException("Configuração de posições é obrigatória");

        var positions = System.Text.Json.JsonSerializer.Deserialize<List<FieldPosition>>(
            configuration["positions"].ToString()!);

        var sb = new StringBuilder();

        foreach (var record in data.Records)
        {
            var line = new char[positions!.Sum(p => p.Length)];
            Array.Fill(line, ' ');

            foreach (var pos in positions)
            {
                if (record.ContainsKey(pos.Name))
                {
                    var value = record[pos.Name].ToString() ?? string.Empty;
                    var truncated = value.Length > pos.Length 
                        ? value.Substring(0, pos.Length) 
                        : value.PadRight(pos.Length);
                    
                    truncated.CopyTo(0, line, pos.Start, truncated.Length);
                }
            }

            sb.AppendLine(new string(line));
        }

        return sb.ToString();
    }

    private class FieldPosition
    {
        public string Name { get; set; } = string.Empty;
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
