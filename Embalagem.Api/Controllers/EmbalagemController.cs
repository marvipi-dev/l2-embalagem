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
    public IEnumerable<EmbalagemGetResponse> Embalagem()
    {
        return _embalagemService.BuscarEmbalados().Select(e => new EmbalagemGetResponse()
        {
            ProdutoId = e.ProdutoId,
            CaixaId = e.CaixaId,
            PedidoId = e.PedidoId
        });
    }

    [HttpPost]
    public EmbalagemPostResponse Embalagem(EmbalagemPostRequest postRequest)
    {
        // TODO: tratamento de erros
        // TODO: validar request
        // TODO: validar embalados
        // TODO: tornar assíncrono
        var embalados = _embalagemService.Embalar(postRequest.Pedidos);
        var response = new EmbalagemPostResponse()
        {
            Pedidos = embalados
        };
        return response;
    }
}