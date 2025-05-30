using System.Text.Json.Serialization;

namespace Embalagem.Api.Views;

public class CaixaProdutos
{
    [JsonPropertyName("caixa_id")] public string? CaixaId { get; set; }

    public IEnumerable<string> Produtos { get; set; }
    public string? Observacao { get; set; }
}