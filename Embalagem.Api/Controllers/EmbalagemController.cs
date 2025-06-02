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