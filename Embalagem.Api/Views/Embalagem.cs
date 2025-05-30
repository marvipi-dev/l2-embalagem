using System.Text.Json.Serialization;

namespace Embalagem.Api.Views;

public class Embalagem
{
    [JsonPropertyName("pedido_id")] public int PedidoId { get; set; }

    public IEnumerable<CaixaProdutos> Caixas { get; set; }
}