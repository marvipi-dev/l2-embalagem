using Embalagem.Api.Services;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Embalagem()
    {
        var embalados = await _embalagemService.BuscarEmbaladosAsync();
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult>  Embalagem(EmbalagemPostRequest postRequest)
    {
        var embalados = await _embalagemService.EmbalarAsync(postRequest.Pedidos);

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