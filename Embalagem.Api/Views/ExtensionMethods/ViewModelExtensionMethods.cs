using Embalagem.Api.Helpers;
using Embalagem.Api.Views.HttpRequests;

namespace Embalagem.Api.Views.ExtensionMethods;

public static class ViewModelExtensionMethods
{
    /// <summary>
    /// Calcula as dimensões totais de um pedido.
    /// </summary>
    /// <param name="pedido">Um pedido cujas dimensões serão calculadas.</param>
    /// <returns>As dimensões totais de um pedido, levando em conta todos os seus produtos.</returns>
    public static Dimensoes Dimensoes(this PedidoViewModel pedido)
    {
        return pedido.Produtos
            .Select(p => p.Dimensoes)
            .Aggregate(new Dimensoes()
            {
                Altura = 0,
                Comprimento = 0,
                Largura = 0
            }, (dimensoesPedido, d) => dimensoesPedido + d);
    }
}