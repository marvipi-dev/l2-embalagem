using Embalagem.Api.Models;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Services;

public interface IEmbalagemService
{
    /// <summary>
    /// Busca todas os pedidos que já foram embalados.
    /// </summary>
    /// <returns>
    ///     Uma sequência de <see cref="EmbalagemRegistro"/> que contém dados de todos os pedidos já processados.
    ///     Ou null, se não for possível buscar embalagens.
    /// </returns>
    public Task<IEnumerable<EmbalagemRegistro>?> BuscarEmbaladosAsync();
    
    /// <summary>
    /// Embala pedidos na menor quantidade de caixas o possível, priorizando as de menor volume.
    /// </summary>
    /// <param name="pedidos">Uma sequência de <see cref="PedidoViewModel"/>.</param>
    /// <returns>
    ///     Uma sequência de <see cref="EmbalagemViewModel"/> que contêm os pedidos embaláveis e não embaláveis, ordenados pelo id do pedido.
    ///     Ou null, se não for possível embalar nenhum pedido.
    /// </returns>
    public Task<IOrderedEnumerable<EmbalagemViewModel>?> EmbalarAsync(IEnumerable<PedidoViewModel> pedidos);
}