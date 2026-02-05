using ConvertFile.Api.Interfaces;
using ConvertFile.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConvertFile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FileConverterController : ControllerBase
{
    private readonly IFileConverterService _converterService;
    private readonly ILogger<FileConverterController> _logger;

    public FileConverterController(
        IFileConverterService converterService,
        ILogger<FileConverterController> logger)
    {
        _converterService = converterService;
        _logger = logger;
    }

    /// <summary>
    /// Converte arquivo de um formato para outro
    /// </summary>
    /// <param name="request">Dados do arquivo e configurações de conversão</param>
    /// <returns>Arquivo convertido</returns>
    [HttpPost("convert")]
    [ProducesResponseType(typeof(ConvertFileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Convert([FromBody] ConvertFileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FileContent))
        {
            return BadRequest(new { message = "Conteúdo do arquivo é obrigatório" });
        }

        var response = _converterService.Convert(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Retorna os formatos suportados
    /// </summary>
    [HttpGet("formats")]
    [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
    public IActionResult GetSupportedFormats()
    {
        var formats = new[] 
        { 
            "FixedPosition", 
            "Delimited", 
            "Json" 
        };
        
        return Ok(formats);
    }
}
