namespace Embalagem.Api.Views.HttpRequests;

public class EmbalagemPostRequest
{
    public IEnumerable<PedidoViewModel> Pedidos { get; set; }
}