using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IRepository _repository;

    public UsuarioController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        var existe = await _repository.ExisteAsync(usuario);
        if (existe == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        if (existe.Value)
        {
            return Conflict("Usuario já cadastrado");
        }

        var sucesso = await _repository.EscreverAsync(usuario);
        if (sucesso == null || !sucesso.Value)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        return Created();
    }
}