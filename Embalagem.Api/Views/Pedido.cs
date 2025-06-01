using System.Text.Json.Serialization;

namespace Embalagem.Api.Views;

public class Pedido
{
    [JsonPropertyName("pedido_id")] public int PedidoId { get; set; }

    public IEnumerable<Produto> Produtos { get; set; }

    [JsonIgnore]
    public int Volume => Produtos.Select(p => p.Dimensoes.Volume).Sum();
}