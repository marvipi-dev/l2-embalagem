using Embalagem.Api.Services;
using Embalagem.Api.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmbalagemController : ControllerBase
{
    private readonly IEmbalagemService _embalagemService;

    public EmbalagemController(IEmbalagemService embalagemService)
    {
        _embalagemService = embalagemService;
    }

    [HttpGet]
    public IActionResult Embalagem()
    {
        var embalados = _embalagemService.BuscarEmbalados();
        if (embalados == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        return Ok(embalados.Select(e => new EmbalagemGetResponse()
        {
            ProdutoId = e.ProdutoId,
            CaixaId = e.CaixaId,
            PedidoId = e.PedidoId
        }));
    }

    [HttpPost]
    public IActionResult Embalagem(EmbalagemPostRequest postRequest)
    {
        // TODO: validar request
        // TODO: tornar assíncrono
        var embalados = _embalagemService.Embalar(postRequest.Pedidos);

        if (embalados == null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        
        var response = new EmbalagemPostResponse()
        {
            Pedidos = embalados
        };
        return Ok(response);
    }
}