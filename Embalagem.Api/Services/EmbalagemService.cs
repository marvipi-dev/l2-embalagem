using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Views.ExtensionMethods;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    private readonly IRepository _repository;

    public EmbalagemService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmbalagemRegistro>?> BuscarEmbaladosAsync()
    {
        return await _repository.LerEmbalagensAsync();
    }

    public async Task<IOrderedEnumerable<EmbalagemViewModel>?> EmbalarAsync(IEnumerable<PedidoViewModel> pedidos)
    {
        var embalagens = new List<EmbalagemViewModel>();
        if (!pedidos.Any())
        {
            return embalagens.Order();
        }

        var caixas = await _repository.LerCaixasAsync();
        if (caixas == null)
        {
            return null;
        }

        caixas = caixas.OrderBy(c => c.Volume);
        var pedidosPorVolume = pedidos.OrderBy(pe => pe.Dimensoes());

        // Separar e classificar os pedidos.
        var naoEmbalaveisEmUmaCaixa = pedidosPorVolume
            .Where(pe => caixas.All(c => !c.Comporta(pe.Dimensoes())));

        var embalaveisEmUmaCaixa = pedidosPorVolume.Except(naoEmbalaveisEmUmaCaixa);

        var embalavelEmVariasCaixas = naoEmbalaveisEmUmaCaixa.Select(pe => new PedidoViewModel()
            {
                PedidoId = pe.PedidoId,
                Produtos = pe.Produtos.Where(p => caixas.Any(c => c.Comporta(p.Dimensoes)))
            }).Where(pe => pe.Produtos.Any()) // Pedidos que contém um único produto não embalável ficarão vazios.
            .ToList();

        var naoEmbalaveis = naoEmbalaveisEmUmaCaixa.Select(pe => new PedidoViewModel()
        {
            PedidoId = pe.PedidoId,
            Produtos = pe.Produtos.Where(p => caixas.All(c => !c.Comporta(p.Dimensoes)))
        }).Where(pe => pe.Produtos.Any()); // Pedidos que contém um único produto não embalável ficarão vazios.


        // Embalar os pedidos que cabem inteiramente em uma única caixa.
        embalagens.AddRange(embalaveisEmUmaCaixa.Select(pe => new EmbalagemViewModel
        {
            PedidoId = pe.PedidoId,
            Caixas = new List<CaixaViewModel>
            {
                Embalar(pe.Produtos, caixas.First(c => c.Comporta(pe.Dimensoes())))
            }
        }));

        // Embalar os pedidos que não cabem inteiramente em uma única caixa.
        while (embalavelEmVariasCaixas.Any())
        {
            foreach (var pedido in new List<PedidoViewModel>(embalavelEmVariasCaixas))
            {
                var produtosOrdenados = pedido.Produtos.OrderBy(p => p.ProdutoId);
                var qtdEmbalaveis = produtosOrdenados.Count();
                var dimensoesProdutosParaEmbalar = pedido.Dimensoes();

                foreach (var produto in produtosOrdenados)
                {
                    if (caixas.Any(c => c.Comporta(dimensoesProdutosParaEmbalar)))
                    {
                        break;
                    }

                    qtdEmbalaveis--;
                    dimensoesProdutosParaEmbalar -= produto.Dimensoes;
                }

                var caixa = caixas.First(c => c.Comporta(dimensoesProdutosParaEmbalar));
                var couberam = produtosOrdenados.Take(qtdEmbalaveis);
                var naoCouberam = produtosOrdenados.Skip(qtdEmbalaveis);

                if (couberam.Any())
                {
                    embalagens.Add(new()
                    {
                        PedidoId = pedido.PedidoId,
                        Caixas = new List<CaixaViewModel>
                        {
                            new()
                            {
                                CaixaId = caixa.CaixaId,
                                Produtos = couberam.Select(p => p.ProdutoId)
                            }
                        },
                    });
                }

                if (naoCouberam.Any())
                {
                    embalavelEmVariasCaixas.Add(new()
                    {
                        PedidoId = pedido.PedidoId,
                        Produtos = naoCouberam
                    });
                }

                embalavelEmVariasCaixas.Remove(pedido);
            }
        }

        // Preparar os produtos não embalaveis para retorno.
        embalagens.AddRange(naoEmbalaveis.Select(pe => new EmbalagemViewModel()
        {
            PedidoId = pe.PedidoId,
            Caixas = new List<CaixaViewModel>
            {
                new()
                {
                    CaixaId = null,
                    Produtos = pe.Produtos.Select(p => p.ProdutoId),
                    Observacao = $"Produto não cabe em nenhuma caixa disponível."
                }
            }
        }));


        // Agrupar embalagens por id.
        var embalagensAgrupadasPorPedido = new List<EmbalagemViewModel>();
        foreach (var grupo in embalagens.GroupBy(e => (e.PedidoId, e.Caixas)))
        {
            var i = embalagensAgrupadasPorPedido.FindIndex(e => e.PedidoId == grupo.Key.PedidoId);
            if (i < 0)
            {
                embalagensAgrupadasPorPedido.Add(new()
                {
                    PedidoId = grupo.Key.PedidoId,
                    Caixas = grupo.Key.Caixas
                });
            }
            else
            {
                var embalagemAgrupavel = embalagensAgrupadasPorPedido[i];
                embalagemAgrupavel.Caixas = embalagemAgrupavel.Caixas.Concat(grupo.Key.Caixas);
            }
        }

        var embalagensOrdenadasPorPedido = embalagensAgrupadasPorPedido.OrderBy(e => e.PedidoId);

        var registros = RegistrarEmbalagens(embalagensOrdenadasPorPedido);
        var sucesso = await _repository.EscreverAsync(registros);
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