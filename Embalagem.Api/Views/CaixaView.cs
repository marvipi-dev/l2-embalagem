using System.Text.Json.Serialization;

namespace Embalagem.Api.Views;

public class CaixaView
{
    [JsonPropertyName("caixa_id")] public string? CaixaId { get; set; }

    public IEnumerable<string> Produtos { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Observacao { get; set; }
}