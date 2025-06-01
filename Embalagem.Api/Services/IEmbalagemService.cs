using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Services;

public interface IEmbalagemService
{
    public List<(int PedidoId, Produto Produto)> IdentificarNaoEmbalaveis(IEnumerable<Pedido> pedidos,
        IOrderedEnumerable<CaixaModel> caixas);

    public IEnumerable<Pedido> FiltrarEmbalaveis(IEnumerable<(int PedidoId, Views.Produto Produto)> naoEmbalaveis,
        IEnumerable<Pedido> pedidos);

    public (IEnumerable<Views.Embalagem> embalados, IEnumerable<Pedido> embalarEmVarias) EmbalagemUnica(
        IEnumerable<Pedido> pedidos, IEnumerable<CaixaModel> caixas);

    public List<Views.Embalagem> EmbalarVariasCaixas(IEnumerable<Pedido> pedidosVariasCaixas,
        IOrderedEnumerable<CaixaModel> caixas);

    public IEnumerable<Views.Embalagem> PrepararNaoEmbalaveis(
        IEnumerable<(int PedidoId, Produto Produto)> naoEmbalaveis);
}