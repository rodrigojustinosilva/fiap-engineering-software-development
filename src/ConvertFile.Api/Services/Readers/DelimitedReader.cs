using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;

namespace ConvertFile.Api.Services.Readers;

/// <summary>
/// Leitor de arquivos delimitados (CSV, TSV, etc)
/// Princípios: Single Responsibility (SRP), Open/Closed (OCP)
/// </summary>
public class DelimitedReader : IFileReader
{
    public bool CanRead(string format) => 
        format.Equals("Delimited", StringComparison.OrdinalIgnoreCase);

    public FileData Read(string content, Dictionary<string, object>? configuration = null)
    {
        var fileData = new FileData();
        
        if (string.IsNullOrWhiteSpace(content))
            return fileData;

        // Configuração: delimitador (padrão: vírgula)
        var delimiter = configuration?.ContainsKey("delimiter") == true 
            ? configuration["delimiter"].ToString()! 
            : ",";

        var hasHeader = configuration?.ContainsKey("hasHeader") == true 
            && bool.Parse(configuration["hasHeader"].ToString()!);

        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        if (lines.Length == 0)
            return fileData;

        // Primeira linha como header
        var headers = lines[0].Split(delimiter);
        fileData.Headers = headers.Select(h => h.Trim()).ToList();

        var startIndex = hasHeader ? 1 : 0;

        for (int i = startIndex; i < lines.Length; i++)
        {
            var values = lines[i].Split(delimiter);
            var record = new Dictionary<string, object>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                record[fileData.Headers[j]] = values[j].Trim();
            }

            fileData.Records.Add(record);
        }

        return fileData;
    }
}
