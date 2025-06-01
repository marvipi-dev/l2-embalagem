using Embalagem.Api.Data;
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
    private readonly IRepository _repository;
    private readonly IEmbalagemService _embalagemService;

    public EmbalagemController(IRepository repository, IEmbalagemService embalagemService)
    {
        _repository = repository;
        _embalagemService = embalagemService;
    }

    [HttpGet]
    public IEnumerable<EmbalagemGetResponse> Embalagem()
    {
        return _repository.LerEmbalagens();
    }

    [HttpPost]
    public EmbalagemPostResponse Embalagem(EmbalagemPostRequest postRequest)
    {
        // Dar preferência às caixas de menor volume
        var caixas = _repository.LerCaixas().OrderBy(c => c.Volume);
        var pedidos = postRequest.Pedidos;

        var naoEmbalaveis = _embalagemService.IdentificarNaoEmbalaveis(pedidos, caixas);
        var pedidosFiltrados = _embalagemService.FiltrarEmbalaveis(naoEmbalaveis, pedidos);

        var embalagens = _embalagemService.EmbalagemUnica(pedidosFiltrados, caixas);
        var embaladosUmaCaixa = embalagens.embalados;
        var pedidosVariasCaixas = embalagens.embalarEmVarias;

        var embaladosVariasCaixas = _embalagemService.EmbalarVariasCaixas(pedidosVariasCaixas, caixas);

        var naoEmbalaveisRetorno = _embalagemService.PrepararNaoEmbalaveis(naoEmbalaveis);

        var embalagemPostResponse = new EmbalagemPostResponse
        {
            Pedidos = naoEmbalaveisRetorno
                .Concat(embaladosVariasCaixas)
                .Concat(embaladosUmaCaixa)
        };

        _repository.Escrever(embalagemPostResponse.Pedidos);

        return embalagemPostResponse;
    }
}