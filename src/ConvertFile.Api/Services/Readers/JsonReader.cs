using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using System.Text.Json;

namespace ConvertFile.Api.Services.Readers;

/// <summary>
/// Leitor de arquivos JSON
/// Princípios: Single Responsibility (SRP), Open/Closed (OCP)
/// </summary>
public class JsonReader : IFileReader
{
    public bool CanRead(string format) => 
        format.Equals("Json", StringComparison.OrdinalIgnoreCase);

    public FileData Read(string content, Dictionary<string, object>? configuration = null)
    {
        var fileData = new FileData();
        
        if (string.IsNullOrWhiteSpace(content))
            return fileData;

        try
        {
            var jsonArray = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(content);
            
            if (jsonArray == null || jsonArray.Count == 0)
                return fileData;

            // Extrair headers do primeiro objeto
            fileData.Headers = jsonArray[0].Keys.ToList();

            // Converter cada objeto JSON em um registro
            foreach (var jsonObj in jsonArray)
            {
                var record = new Dictionary<string, object>();
                
                foreach (var kvp in jsonObj)
                {
                    record[kvp.Key] = kvp.Value.ToString();
                }
                
                fileData.Records.Add(record);
            }

            return fileData;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Erro ao fazer parse do JSON: {ex.Message}", ex);
        }
    }
}
