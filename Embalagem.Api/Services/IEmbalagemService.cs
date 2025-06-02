using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public interface IEmbalagemService
{
    /// <summary>
    /// Embala pedidos na menor quantidade de caixas o possível, priorizando as de menor volume.
    /// </summary>
    /// <param name="pedidos">Uma sequência de <see cref="Pedido"/>.</param>
    /// <returns>Uma sequência de <see cref="Embalagem"/> que contêm os pedidos embaláveis e não embaláveis, ordenados pelo id do pedido.</returns>
    public IOrderedEnumerable<Views.Embalagem> Embalar(IEnumerable<Pedido> pedidos);
}