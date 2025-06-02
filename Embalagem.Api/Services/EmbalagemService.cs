using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    private readonly IRepository _repository;

    public EmbalagemService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RegistroEmbalagem>?> BuscarEmbaladosAsync()
    {
        return await _repository.LerEmbalagensAsync();
    }

    public async Task<IOrderedEnumerable<Views.Embalagem>?> EmbalarAsync(IEnumerable<Pedido> pedidos)
    {
        var embalagens = new List<Views.Embalagem>();
        if (!pedidos.Any())
        {
            return embalagens.Order();
        }

        var caixas = await _repository.LerCaixasAsync();
        if (caixas == null)
        {
            return null;
        }

        caixas = caixas.OrderBy(c => c.Dimensoes.Volume);
        var pedidosPorVolume = pedidos.OrderBy(p => p.Volume);

        // Separar e classificar os pedidos.
        var naoEmbalaveisEmUmaCaixa = pedidosPorVolume.Where(p => caixas.All(c => !c.Comporta(p)));

        var embalaveisEmUmaCaixa = pedidosPorVolume.Except(naoEmbalaveisEmUmaCaixa);

        var embalavelEmVariasCaixas = naoEmbalaveisEmUmaCaixa.Select(pe => new Pedido()
            {
                PedidoId = pe.PedidoId,
                Produtos = pe.Produtos.Where(p => caixas.Any(c => c.Comporta(p)))
            }).Where(pe => pe.Produtos.Any()) // Pedidos que contém um único produto não embalável ficarão vazios.
            .ToList();

        var naoEmbalaveis = naoEmbalaveisEmUmaCaixa.Select(pe => new Pedido()
        {
            PedidoId = pe.PedidoId,
            Produtos = pe.Produtos.Where(p => caixas.All(c => !c.Comporta(p)))
        }).Where(pe => pe.Produtos.Any()); // Pedidos que contém um único produto não embalável ficarão vazios.


        // Embalar os pedidos que cabem inteiramente em uma única caixa
        embalagens.AddRange(embalaveisEmUmaCaixa.Select(pe => new Views.Embalagem
        {
            PedidoId = pe.PedidoId,
            Caixas = new List<CaixaView> { caixas.First(c => c.Comporta(pe)).Embalar(pe.Produtos) }
        }));

        // Embalar os pedidos que não cabem inteiramente em uma única caixa.
        while (embalavelEmVariasCaixas.Any())
        {
            foreach (var caixa in caixas)
            {
                foreach (var pedido in new List<Pedido>(embalavelEmVariasCaixas))
                {
                    var qtdEmbalaveis = pedido.Produtos.Count(caixa.Comporta);

                    var couberam = pedido.Produtos.Take(qtdEmbalaveis);
                    var naoCouberam = pedido.Produtos.Skip(qtdEmbalaveis);

                    if (couberam.Any())
                    {
                        embalagens.Add(new()
                        {
                            PedidoId = pedido.PedidoId,
                            Caixas = new List<CaixaView>
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
        }


        // Preparar os produtos não embalaveis para retorno
        var naoEmbalados = naoEmbalaveis.Select(pe => new Views.Embalagem()
        {
            PedidoId = pe.PedidoId,
            Caixas = new List<CaixaView>
            {
                new()
                {
                    CaixaId = null,
                    Produtos = pe.Produtos.Select(p => p.ProdutoId),
                    Observacao = $"Produto não cabe em nenhuma caixa disponível."
                }
            }
        });

        // Agrupar embalagens por id
        var embalagensAgrupadas = embalagens.Concat(naoEmbalados)
            .OrderBy(e => e.PedidoId)
            .GroupBy(e => (e.PedidoId, e.Caixas));

        var embalagensFinal = new List<Views.Embalagem>();
        foreach (var grupo in embalagensAgrupadas)
        {
            var i = embalagensFinal.FindIndex(e => e.PedidoId == grupo.Key.PedidoId);
            if (i < 0)
            {
                embalagensFinal.Add(new Views.Embalagem()
                {
                    PedidoId = grupo.Key.PedidoId,
                    Caixas = grupo.Key.Caixas
                });
            }
            else
            {
                embalagensFinal[i].Caixas = embalagensFinal[i].Caixas.Concat(grupo.Key.Caixas);
            }
        }

        var embalagensOrdenadasPorPedido = embalagensFinal.OrderBy(e => e.PedidoId);
        var sucesso = await _repository.EscreverAsync(embalagensOrdenadasPorPedido);
        if (!sucesso.HasValue || !sucesso.Value)
        {
            return null;
        }

        return embalagensOrdenadasPorPedido;
    }
}