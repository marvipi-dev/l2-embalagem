using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    private readonly IEmbalagemRepository _embalagemRepository;
    private readonly IEmbalagemSeparacaoService _embalagemSeparacaoService;
    private readonly IEmbalagemPreparacaoService _embalagemPreparacaoService;

    public EmbalagemService(IEmbalagemRepository embalagemRepository,
        IEmbalagemSeparacaoService embalagemSeparacaoService, IEmbalagemPreparacaoService embalagemPreparacaoService)
    {
        _embalagemRepository = embalagemRepository;
        _embalagemSeparacaoService = embalagemSeparacaoService;
        _embalagemPreparacaoService = embalagemPreparacaoService;
    }

    public async Task<IEnumerable<EmbalagemRegistro>?> BuscarEmbaladosAsync()
    {
        return await _embalagemRepository.LerEmbalagensAsync();
    }

    public async Task<IOrderedEnumerable<EmbalagemViewModel>?> EmbalarAsync(IEnumerable<PedidoViewModel> pedidos)
    {
        if (!pedidos.Any())
        {
            return new List<EmbalagemViewModel>().Order();
        }

        var caixas = await _embalagemRepository.LerCaixasAsync();
        if (caixas == null)
        {
            return null;
        }

        var (embalaveis, naoEmbalaveis) = _embalagemSeparacaoService.Classificar(pedidos, caixas);
        var prontosParaEmbalagem = _embalagemSeparacaoService.Separar(embalaveis, caixas);

        var embalados = _embalagemPreparacaoService.PrepararEmbalaveis(prontosParaEmbalagem, caixas);
        var naoEmbalados = _embalagemPreparacaoService.PrepararNaoEmbalaveis(naoEmbalaveis);
        var embalagensOrdernadasPorPedidoId = _embalagemPreparacaoService.AgruparPorPedidoId(embalados, naoEmbalados)
            .OrderBy(embalagem => embalagem.PedidoId);

        var registros = RegistrarEmbalagens(embalagensOrdernadasPorPedidoId);
        var sucesso = await _embalagemRepository.EscreverAsync(registros);
        if (!sucesso.HasValue || !sucesso.Value)
        {
            return null;
        }

        return embalagensOrdernadasPorPedidoId;
    }

    private static IEnumerable<EmbalagemRegistro> RegistrarEmbalagens(
        IOrderedEnumerable<EmbalagemViewModel> embalagensOrdenadasPorPedido)
    {
        return embalagensOrdenadasPorPedido
            .SelectMany(embalagem => embalagem.Caixas
                .SelectMany(caixa => caixa.Produtos
                    .Select(produtoId => new EmbalagemRegistro()
                    {
                        PedidoId = embalagem.PedidoId,
                        CaixaId = caixa.CaixaId,
                        ProdutoId = produtoId
                    })));
    }
}