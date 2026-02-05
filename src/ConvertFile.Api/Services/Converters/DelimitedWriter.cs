using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using System.Text;

namespace ConvertFile.Api.Services.Converters;

/// <summary>
/// Escritor de arquivos delimitados
/// Princípios: Single Responsibility (SRP)
/// </summary>
public class DelimitedWriter : IFileWriter
{
    public bool CanWrite(string format) => 
        format.Equals("Delimited", StringComparison.OrdinalIgnoreCase);

    public string Write(FileData data, Dictionary<string, object>? configuration = null)
    {
        var delimiter = configuration?.ContainsKey("delimiter") == true 
            ? configuration["delimiter"].ToString()! 
            : ",";

        var includeHeader = configuration?.ContainsKey("includeHeader") != true 
            || bool.Parse(configuration["includeHeader"].ToString()!);

        var sb = new StringBuilder();

        // Header
        if (includeHeader && data.Headers.Count > 0)
        {
            sb.AppendLine(string.Join(delimiter, data.Headers));
        }

        // Records
        foreach (var record in data.Records)
        {
            var values = data.Headers.Select(h => 
                record.ContainsKey(h) ? record[h].ToString() : string.Empty);
            sb.AppendLine(string.Join(delimiter, values));
        }

        return sb.ToString();
    }
}
