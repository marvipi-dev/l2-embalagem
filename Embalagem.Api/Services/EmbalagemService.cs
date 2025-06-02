using Embalagem.Api.Data;
using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    private readonly IRepository _repository;

    public EmbalagemService(IRepository repository)
    {
        _repository = repository;
    }

    public IOrderedEnumerable<Views.Embalagem> Embalar(IEnumerable<Pedido> pedidos)
    {
        var embalagens = new List<Views.Embalagem>();
        var caixas = _repository.LerCaixas().OrderBy(c => c.Volume);
        var pedidosPorVolume = pedidos.OrderBy(p => p.Volume);

        // Separar os pedidos que não cabem em nenhuma caixa.
        var pedidosNaoEmbalaveis = new List<Pedido>();
        var pedidosEmbalaveis = new List<Pedido>();
        foreach (var pedido in pedidosPorVolume)
        {
            var produtosNaoEmbalaveis = new List<Produto>();
            foreach (var produto in pedido.Produtos)
            {
                var embalavel = false;
                foreach (var caixa in caixas)
                {
                    if (embalavel = embalavel || caixa.Comporta(produto))
                    {
                        break;
                    }
                }

                if (!embalavel)
                {
                    produtosNaoEmbalaveis.Add(produto);
                }
            }

            if (produtosNaoEmbalaveis.Any())
            {
                pedidosNaoEmbalaveis.Add(new()
                {
                    PedidoId = pedido.PedidoId,
                    Produtos = produtosNaoEmbalaveis
                });
            }

            var produtosEmbalaveis = pedido.Produtos
                .Except(produtosNaoEmbalaveis)
                .Where(p => p.ProdutoId != string.Empty);

            if (produtosEmbalaveis.Any())
            {
                pedidosEmbalaveis.Add(new()
                {
                    PedidoId = pedido.PedidoId,
                    Produtos = produtosEmbalaveis
                });
            }
        }

        // Preparar os não embalaveis para retorno
        var naoEmbalados = new List<Views.Embalagem>();
        foreach (var pedidoNaoEmbalavel in pedidosNaoEmbalaveis)
        {
            var produtoIds = pedidoNaoEmbalavel.Produtos.Select(p => p.ProdutoId);
            var msg = produtoIds.Count() > 1 ? "Produtos" : "Produto";
            naoEmbalados.Add(new Views.Embalagem
            {
                PedidoId = pedidoNaoEmbalavel.PedidoId,
                Caixas = new List<CaixaView>
                {
                    new()
                    {
                        CaixaId = null,
                        Produtos = produtoIds,
                        Observacao = $"{msg} não cabe em nenhuma caixa disponível."
                    }
                }
            });
        }

        // Separar os pedidos que cabem inteiramente em uma única caixa
        var embalavelEmUma = new List<Pedido>();
        var embalavelEmVarias = new List<Pedido>();
        foreach (var pedido in pedidosEmbalaveis)
        {
            foreach (var caixa in caixas)
            {
                if (caixa.Comporta(pedido))
                {
                    embalavelEmUma.Add(pedido);
                    break;
                }
            }
        }

        // Embalar os pedidos que cabem inteiramente em uma única caixa
        foreach (var pedido in embalavelEmUma)
        {
            foreach (var caixa in caixas)
            {
                if (caixa.Comporta(pedido))
                {
                    var caixaView = caixa.Embalar(pedido.Produtos);
                    embalagens.Add(new()
                    {
                        PedidoId = pedido.PedidoId,
                        Caixas = new List<CaixaView> { caixaView }
                    });
                    break;
                }
            }
        }

        // Embalar os pedidos que não cabem inteiramente em uma única caixa
        embalavelEmVarias = pedidosEmbalaveis.Except(embalavelEmUma).ToList();
        while (embalavelEmVarias.Any())
        {
            var embalar = new List<Pedido>(embalavelEmVarias);

            foreach (var caixa in caixas)
            {
                foreach (var pedido in embalar)
                {
                    var qtdEmbalaveis = pedido.Produtos.Count(caixa.Comporta);

                    var couberam = pedido.Produtos.Take(qtdEmbalaveis);
                    var naoCouberam = pedido.Produtos.Skip(qtdEmbalaveis);

                    if (couberam.Any())
                    {
                        var caixaView = new CaixaView()
                        {
                            CaixaId = caixa.CaixaId,
                            Produtos = couberam.Select(p => p.ProdutoId)
                        };

                        embalagens.Add(new()
                        {
                            PedidoId = pedido.PedidoId,
                            Caixas = new List<CaixaView> { caixaView },
                        });
                    }

                    embalavelEmVarias.Remove(pedido);
                    if (naoCouberam.Any())
                    {
                        var embalarNaProxima = new Pedido()
                        {
                            PedidoId = pedido.PedidoId,
                            Produtos = naoCouberam
                        };
                        embalavelEmVarias.Insert(0, embalarNaProxima);
                    }
                    
                    embalar = new List<Pedido>(embalavelEmVarias);
                }
            }
        }

        // Agrupar embalagens por id
        var embalagensAgrupadas = embalagens
            .Concat(naoEmbalados)
            .OrderBy(e => e.PedidoId)
            .GroupBy(e => (e.PedidoId, e.Caixas)); // TODO: AGRUPAR EMBALAGENS POR ID

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

        var retorno = embalagensFinal.OrderBy(e => e.PedidoId);
        _repository.Escrever(retorno); // TODO retornar algo quando o banco de dados não conseguir escrever
        
        return retorno;
    }
}