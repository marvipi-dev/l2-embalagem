using Embalagem.Api.Models;
using Embalagem.Api.Views;
using Pedido = Embalagem.Api.Views.Pedido;

namespace Embalagem.Api.Services;

public class EmbalagemService : IEmbalagemService
{
    public List<(int PedidoId, Produto Produto)> IdentificarNaoEmbalaveis(IEnumerable<Pedido> pedidos,
        IOrderedEnumerable<CaixaModel> caixas)
    {
        var naoEmbalaveis = new List<(int PedidoId, Views.Produto Produto)>();

        var volumeMaiorCaixa = caixas.Last().Volume;
        foreach (var pedido in pedidos)
        foreach (var produto in pedido.Produtos)
        {
            if (produto.Dimensoes.Volume > volumeMaiorCaixa)
            {
                naoEmbalaveis.Add((pedido.PedidoId, produto));
            }
        }

        return naoEmbalaveis;
    }

    public IEnumerable<Pedido> FiltrarEmbalaveis(IEnumerable<(int PedidoId, Produto Produto)> naoEmbalaveis,
        IEnumerable<Pedido> pedidos)
    {
        if (!naoEmbalaveis.Any())
        {
            return pedidos;
        }
        
        var pedidosFiltrados = new List<Pedido>();
        foreach (var naoEmbalavel in naoEmbalaveis)
        {
            pedidosFiltrados = pedidos.Select(pe => new Pedido
            {
                PedidoId = pe.PedidoId,
                Produtos = pe.PedidoId == naoEmbalavel.PedidoId
                    ? pe.Produtos.Where(p => !p.Equals(naoEmbalavel.Produto))
                    : pe.Produtos
            }).ToList();
        }

        pedidosFiltrados.RemoveAll(pe => !pe.Produtos.Any());
        return pedidosFiltrados;
    }

    public (IEnumerable<Views.Embalagem> embalados, IEnumerable<Pedido> embalarEmVarias) EmbalagemUnica(
        IEnumerable<Pedido> pedidos, IEnumerable<CaixaModel> caixas)
    {
        var embalagens = new List<Views.Embalagem>();
        var embalarEmVarias = new List<Pedido>();

        foreach (var pedido in pedidos)
        {
            var cabemUnicaCaixa = false;
            foreach (var caixa in caixas)
            {
                if (cabemUnicaCaixa = caixa.CabemTodos(pedido.Volume))
                {
                    var pedidoEmbalado = caixa.Embalar(pedido);
                    embalagens.Add(pedidoEmbalado);
                    break;
                }
            }

            if (!cabemUnicaCaixa)
            {
                embalarEmVarias.Add(pedido);
            }
        }

        return (embalagens, embalarEmVarias);
    }

    public List<Views.Embalagem> EmbalarVariasCaixas(IEnumerable<Pedido> pedidosVariasCaixas,
        IOrderedEnumerable<CaixaModel> caixas)
    {
        var embaladosVariasCaixas = new List<Views.Embalagem>();
        foreach (var pedido in pedidosVariasCaixas)
        {
            var naoEmbalados = new List<Views.Produto>();

            foreach (var caixa in caixas)
            {
                var embalagem = caixa.EmbalarParcialmente(pedido);
                embaladosVariasCaixas.AddRange(embalagem.embalagens);
                if (!embalagem.incabiveis.Any())
                {
                    break;
                }

                naoEmbalados.AddRange(embalagem.incabiveis);
            }
        }

        return embaladosVariasCaixas;
    }

    public IEnumerable<Views.Embalagem> PrepararNaoEmbalaveis(
        IEnumerable<(int PedidoId, Views.Produto Produto)> naoEmbalaveis)
    {
        var naoEmbalaveisRetorno = naoEmbalaveis.Select(ne => new Views.Embalagem
        {
            PedidoId = ne.PedidoId,
            Caixas = new List<CaixaProdutos>
            {
                new()
                {
                    CaixaId = null,
                    Produtos = new List<string>
                    {
                        ne.Produto.ProdutoId
                    },
                    Observacao = "Produto não cabe em nenhuma caixa disponível."
                }
            }
        });
        return naoEmbalaveisRetorno;
    }
}