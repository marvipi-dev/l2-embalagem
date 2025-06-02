using Embalagem.Api.Data;
using Embalagem.Api.Views;
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
    public IActionResult Registrar([FromBody] Usuario usuario)
    {
        var existe = _repository.Existe(usuario);
        if (existe == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        if (existe.Value)
        {
            return Conflict("Usuario já cadastrado");
        }

        var sucesso = _repository.Escrever(usuario);
        if (sucesso == null || !sucesso.Value)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        return Created();
    }
}