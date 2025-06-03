using Embalagem.Api.Helpers;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Models;

public class Caixa
{
    public required string CaixaId { get; set; }
    public required Dimensoes Dimensoes { get; set; }

    /// <summary>
    /// Verifica se um produto cabe dentro da caixa.
    /// </summary>
    /// <param name="produtoViewModel">O <see cref="ProdutoViewModel"/> para verificar.</param>
    /// <returns>true se o produto cabe dentro da caixa, senão false.</returns>
    public bool Comporta(ProdutoViewModel produtoViewModel)
    {
        return produtoViewModel.Dimensoes < Dimensoes;
    }
    
    /// <summary>
    /// Verifica se todos os produtos de um único pedido cabem dentro da caixa.
    /// </summary>
    /// <param name="pedido">Um <see cref="PedidoViewModel"/> que contém uma quantidade arbitrária de produtos.</param>
    /// <returns>true se todos os produtos cabem dentro da caixa, senão false.</returns>
    public bool Comporta(PedidoViewModel pedido)
    {
        var dimensoesPedido = Medir(pedido);

        return dimensoesPedido < Dimensoes;
    }

    /// <summary>
    /// Verifica se todos os produtos de uma sequência de pedidos cabem dentro da caixa.
    /// </summary>
    /// <param name="pedidos">A sequência de <see cref="PedidoViewModel"/> para verificar.</param>
    /// <returns>true se todos os produtos de todos os pedidos cabem dentro da caixa, senão false.</returns>
    public bool Comporta(IEnumerable<PedidoViewModel> pedidos)
    {
        var dimensaoTotal = pedidos.
            Select(Medir)
            .Aggregate(new Dimensoes() {Altura = 0, Largura = 0, Comprimento = 0}, (d, next) => d + next);

        return dimensaoTotal < Dimensoes;
    }
    
    private static Dimensoes Medir(PedidoViewModel pedido)
    {
        var dimensoesPedido = pedido.Produtos
            .Select(p => p.Dimensoes)
            .Aggregate(new Dimensoes() {Altura = 0, Largura = 0, Comprimento = 0}, (d, next) => d + next);
        
        return new()
        {
            Altura = dimensoesPedido.Altura,
            Largura = dimensoesPedido.Largura,
            Comprimento = dimensoesPedido.Comprimento
        };
    }
    
    /// <summary>
    /// Embala todos os produtos dentro da caixa.
    /// </summary>
    /// <param name="produtos">A sequência de <see cref="ProdutoViewModel"/> para embalar.</param>
    /// <returns>Um <see cref="CaixaViewModel"/> que contém todos os produtos e o id da caixa.</returns>
    /// <remarks>Pressupõe que a caixa é grande o suficiente para armazenar todos os produtos.</remarks>
    public CaixaViewModel Embalar(IEnumerable<ProdutoViewModel> produtos)
    {
        return new CaixaViewModel()
        {
            CaixaId = CaixaId,
            Produtos = produtos.Select(p => p.ProdutoId)
        };
    }
}