using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        var existe = await _usuarioRepository.ExisteAsync(usuario);
        if (existe == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        if (existe.Value)
        {
            return Conflict("Usuario já cadastrado");
        }

        var sucesso = await _usuarioRepository.EscreverAsync(usuario);
        if (sucesso == null || !sucesso.Value)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        return Created();
    }
}