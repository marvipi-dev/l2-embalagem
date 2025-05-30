using Embalagem.Api.ExtensionMethods;
using Embalagem.Api.Views;

namespace Embalagem.Api.Models;

public class CaixaModel
{
    public string CaixaId { get; set; }
    public int Altura { get; set; }
    public int Largura { get; set; }
    public int Comprimento { get; set; }
    public int Volume => Altura * Largura * Comprimento;

    public bool CabemTodos(int volumeProduto)
    {
        return volumeProduto <= Volume;
    }

    public bool CabeDentro(Produto produto)
    {
        return produto.Dimensoes.Volume <= Volume;
    }

    public Views.Embalagem Embalar(Pedido pedido)
    {
        return new Views.Embalagem
        {
            PedidoId = pedido.PedidoId,
            Caixas = new List<CaixaProdutos>
            {
                new()
                {
                    CaixaId = CaixaId,
                    Produtos = pedido.Produtos.Select(p => p.ProdutoId)
                }
            }
        };
    }

    public (IEnumerable<Views.Embalagem> embalagens, IEnumerable<Produto> incabiveis) EmbalarParcialmente(Pedido pedido)
    {
        var embalagens = new List<Views.Embalagem>();
        var incabiveis = new List<Produto>();
        var produtos = pedido.Produtos.OrderBy(p => p.Dimensoes.Volume);

        var qntsCabem = QuantosCabem(produtos);


        var embalaveis = new Pedido
        {
            PedidoId = pedido.PedidoId,
            Produtos = produtos.Take(qntsCabem)
        };
        var embalados = Embalar(embalaveis);
        embalagens.AddRange(embalados);


        var naoCouberamProdutos = produtos.Skip(qntsCabem);
        if (!naoCouberamProdutos.Any()) return (embalagens, incabiveis);

        var naoCouberamPedido = new Pedido
        {
            PedidoId = pedido.PedidoId,
            Produtos = naoCouberamProdutos
        };
        if (CabemTodos(naoCouberamPedido.VolumeProdutos()))
            embalagens.AddRange(Embalar(naoCouberamPedido));
        else
            incabiveis.AddRange(naoCouberamProdutos);

        return (embalagens, incabiveis);
    }

    private int QuantosCabem(IOrderedEnumerable<Produto> produtos)
    {
        var somaVolume = 0;
        var qntsCabem = 0;
        foreach (var produto in produtos)
        {
            if (somaVolume + produto.Dimensoes.Volume > Volume) break;

            somaVolume += produto.Dimensoes.Volume;
            qntsCabem++;
        }

        return qntsCabem;
    }
}