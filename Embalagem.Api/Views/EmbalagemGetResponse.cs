using System.Text.Json.Serialization;

namespace Embalagem.Api.Views;

public class EmbalagemGetResponse
{
    [JsonPropertyName("pedido_id")] public int PedidoId { get; set; }

    [JsonPropertyName("caixa_id")] public string? CaixaId { get; set; }

    [JsonPropertyName("produto_id")] public string ProdutoId { get; set; }
}