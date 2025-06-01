using Embalagem.Api.Views;

namespace Embalagem.Api.ExtensionMethods;

public static class PedidoExtensions
{
    public static int VolumeProdutos(this Pedido pedido)
    {
        var volume = 0;
        foreach (var produto in pedido.Produtos)
        {
            volume += produto.Dimensoes.Altura * produto.Dimensoes.Largura * produto.Dimensoes.Comprimento;
        }

        return volume;
    }
}