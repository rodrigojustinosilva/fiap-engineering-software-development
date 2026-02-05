namespace ConvertFile.Api.Models.Enums;

/// <summary>
/// Formatos de arquivo suportados
/// </summary>
public enum FileFormat
{
    FixedPosition,    // Arquivo de posição fixa
    Delimited,        // Arquivo delimitado (CSV, TSV, etc)
    Json,             // JSON
    Xml               // XML (para expansão futura)
}
