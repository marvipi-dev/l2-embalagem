using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService;
    private readonly IUsuarioRepository _usuarioRepository;

    public AutenticacaoController(IUsuarioRepository usuarioRepository, IAutenticacaoService autenticacao)
    {
        _usuarioRepository = usuarioRepository;
        _autenticacaoService = autenticacao;
    }

    [HttpPost]
    public async Task<IActionResult> GerarToken([FromBody] Usuario usuario)
    {
        var valido = await _usuarioRepository.ValidarAsync(usuario);
        if (valido == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        if (valido.Value)
        {
            return Ok(_autenticacaoService.GerarToken());
        }

        return Unauthorized("Usuario não cadastrado.");
    }
}