namespace Embalagem.Api.Models;

public class EmbalagemRegistro
{
    public int PedidoId { get; set; }

    public string? CaixaId { get; set; }

    public required string ProdutoId { get; set; }
}