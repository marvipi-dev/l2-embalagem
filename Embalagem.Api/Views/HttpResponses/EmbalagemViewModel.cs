using System.Text.Json.Serialization;

namespace Embalagem.Api.Views.HttpResponses;

public class EmbalagemViewModel
{
    [JsonPropertyName("pedido_id")] public int PedidoId { get; set; }

    public IEnumerable<CaixaViewModel> Caixas { get; set; }
}