using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public interface IEmbalagemService
{
    /// <summary>
    /// Busca todas os pedidos que já foram embalados.
    /// </summary>
    /// <returns>
    ///     Uma sequência de <see cref="RegistroEmbalagem"/> que contém dados de todos os pedidos já processados.
    ///     Ou null, se não for possível buscar embalagens.
    /// </returns>
    public Task<IEnumerable<RegistroEmbalagem>?> BuscarEmbaladosAsync();
    
    /// <summary>
    /// Embala pedidos na menor quantidade de caixas o possível, priorizando as de menor volume.
    /// </summary>
    /// <param name="pedidos">Uma sequência de <see cref="Pedido"/>.</param>
    /// <returns>
    ///     Uma sequência de <see cref="Embalagem"/> que contêm os pedidos embaláveis e não embaláveis, ordenados pelo id do pedido.
    ///     Ou null, se não for possível embalar nenhum pedido.
    /// </returns>
    public Task<IOrderedEnumerable<Views.Embalagem>?> EmbalarAsync(IEnumerable<Pedido> pedidos);
}