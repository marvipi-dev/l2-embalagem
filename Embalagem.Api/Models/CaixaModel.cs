using Embalagem.Api.Views;

namespace Embalagem.Api.Models;

public class CaixaModel
{
    public required string CaixaId { get; set; }
    public required Dimensoes Dimensoes { get; set; }

    /// <summary>
    /// Verifica se um produto cabe dentro da caixa.
    /// </summary>
    /// <param name="produto">O <see cref="Produto"/> para verificar.</param>
    /// <returns>true se o produto cabe dentro da caixa, senão false.</returns>
    public bool Comporta(Produto produto)
    {
        return produto.Dimensoes < Dimensoes;
    }
    
    /// <summary>
    /// Verifica se todos os produtos de um único pedido cabem dentro da caixa.
    /// </summary>
    /// <param name="pedido">Um <see cref="Pedido"/> que contém uma quantidade arbitrária de produtos.</param>
    /// <returns>true se todos os produtos cabem dentro da caixa, senão false.</returns>
    public bool Comporta(Pedido pedido)
    {
        var dimensoesPedido = Medir(pedido);

        return dimensoesPedido < Dimensoes;
    }

    /// <summary>
    /// Verifica se todos os produtos de uma sequência de pedidos cabem dentro da caixa.
    /// </summary>
    /// <param name="pedidos">A sequência de <see cref="Pedido"/> para verificar.</param>
    /// <returns>true se todos os produtos de todos os pedidos cabem dentro da caixa, senão false.</returns>
    public bool Comporta(IEnumerable<Pedido> pedidos)
    {
        var dimensaoTotal = pedidos.
            Select(Medir)
            .Aggregate(new Dimensoes() {Altura = 0, Largura = 0, Comprimento = 0}, (d, next) => d + next);

        return dimensaoTotal < Dimensoes;
    }
    
    private static Dimensoes Medir(Pedido pedido)
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
    /// <param name="produtos">A sequência de <see cref="Produto"/> para embalar.</param>
    /// <returns>Um <see cref="CaixaView"/> que contém todos os produtos e o id da caixa.</returns>
    /// <remarks>Pressupõe que a caixa é grande o suficiente para armazenar todos os produtos.</remarks>
    public CaixaView Embalar(IEnumerable<Produto> produtos)
    {
        return new CaixaView()
        {
            CaixaId = CaixaId,
            Produtos = produtos.Select(p => p.ProdutoId)
        };
    }
}