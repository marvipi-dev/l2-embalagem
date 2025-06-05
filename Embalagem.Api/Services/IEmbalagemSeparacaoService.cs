using Embalagem.Api.Models;
using Embalagem.Api.Views.HttpRequests;

namespace Embalagem.Api.Services;

public interface IEmbalagemSeparacaoService
{
    /// <summary>
    /// Identifica produtos que não cabem em nenhuma caixa e os move para outro pedido.
    /// </summary>
    /// <param name="pedidos">Os <see cref="PedidoViewModel"/>s para classificar.</param>
    /// <param name="caixas">As caixas disponíveis para embalagem.</param>
    /// <exception cref="ArgumentNullException">Quando <paramref name="caixas"/> ou <paramref name="pedidos"/> forem nulo.</exception>
    /// <exception cref="ArgumentException">Quando <paramref name="caixas"/> for vazio.</exception>
    /// <returns>
    /// Uma sequência de pedidos contendo somente produtos embaláveis e outra contendo somente produtos não embaláveis.
    /// </returns>
    public (IEnumerable<PedidoViewModel> Embalaveis, IEnumerable<PedidoViewModel> NaoEmbalaveis)
        Classificar(IEnumerable<PedidoViewModel> pedidos, IEnumerable<Caixa> caixas);
    
    /// <summary>
    /// Separa os produtos de cada pedido de modo que eles possam ser embalados na menor quantidade de caixas o possível. 
    /// </summary>
    /// <param name="pedidos">Os <see cref="PedidoViewModel"/>s para separar. </param>
    /// <param name="caixas">As caixas disponíveis para embalagem.</param>
    /// <exception cref="ArgumentNullException">Quando <paramref name="caixas"/> ou <paramref name="pedidos"/> forem nulo.</exception>
    /// <exception cref="ArgumentException">Quando <paramref name="caixas"/> for vazio.</exception>
    /// <returns>Uma lista de pedidos que cabem inteiramente em uma única caixa.</returns>
    /// <remarks>Pressupõe que todos os produtos já foram classificados pelo método <see cref="Classificar"/>.</remarks>
    public IEnumerable<PedidoViewModel> Separar(IEnumerable<PedidoViewModel> pedidos, IEnumerable<Caixa> caixas);
}