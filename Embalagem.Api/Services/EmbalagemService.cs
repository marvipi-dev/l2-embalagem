using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Views.ExtensionMethods;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    private readonly IEmbalagemRepository _embalagemRepository;
    private readonly IEmbalagemSeparacaoService _embalagemSeparacaoService;

    public EmbalagemService(IRepository repository, IEmbalagemSeparacaoService embalagemSeparacaoService)
    {
        _embalagemRepository = embalagemRepository;
        _embalagemSeparacaoService = embalagemSeparacaoService;
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

        caixas = caixas.OrderBy(caixa => caixa.Volume);

        var embalados = new List<EmbalagemViewModel>();
        foreach (var pedido in prontosParaEmbalagem)
        {
            embalados.Add(new()
            {
                PedidoId = pedido.PedidoId,
                Caixas = new List<CaixaViewModel>()
                {
                    new()
                    {
                        CaixaId = caixas.First(caixa => caixa.Comporta(pedido.Dimensoes())).CaixaId,
                        Produtos = pedido.Produtos.Select(produto => produto.ProdutoId)
                    }
                }
            });
        }

        var naoEmbalados = new List<EmbalagemViewModel>();
        foreach (var pedido in naoEmbalaveis)
        {
            naoEmbalados.Add(new()
            {
                PedidoId = pedido.PedidoId,
                Caixas = new List<CaixaViewModel>
                {
                    new()
                    {
                        CaixaId = null,
                        Produtos = pedido.Produtos.Select(p => p.ProdutoId),
                        Observacao = $"Produto não cabe em nenhuma caixa disponível."
                    }
                }
            });
        }

        var embalagens = embalados.Concat(naoEmbalados);
        var embalagensAgrupadasPorPedidoId = embalagens.Aggregate(
            new List<EmbalagemViewModel>(),
            (agrupadas, embalagem) =>
            {
                var agrupada = agrupadas.FirstOrDefault(a => a.PedidoId == embalagem.PedidoId);
                if (agrupada == null)
                {
                    agrupadas.Add(embalagem);
                }
                else
                {
                    agrupada.Caixas = agrupada.Caixas.Concat(embalagem.Caixas);
                }

                return agrupadas;
            });

        var embalagensOrdenadasPorPedido = embalagensAgrupadasPorPedidoId.OrderBy(e => e.PedidoId);

        var registros = RegistrarEmbalagens(embalagensOrdenadasPorPedido);
        var sucesso = await _embalagemRepository.EscreverAsync(registros);
        if (!sucesso.HasValue || !sucesso.Value)
        {
            return null;
        }

        return embalagensOrdenadasPorPedido;
    }

    private CaixaViewModel Embalar(IEnumerable<ProdutoViewModel> produtos, Caixa caixa)
    {
        return new CaixaViewModel()
        {
            CaixaId = caixa.CaixaId,
            Produtos = produtos.Select(p => p.ProdutoId)
        };
    }

    private static IEnumerable<EmbalagemRegistro> RegistrarEmbalagens(
        IOrderedEnumerable<EmbalagemViewModel> embalagensOrdenadasPorPedido)
    {
        return embalagensOrdenadasPorPedido
            .SelectMany(e => e.Caixas
                .SelectMany(c => c.Produtos
                    .Select(p => new EmbalagemRegistro()
                    {
                        PedidoId = e.PedidoId,
                        CaixaId = c.CaixaId,
                        ProdutoId = p
                    })));
    }
}