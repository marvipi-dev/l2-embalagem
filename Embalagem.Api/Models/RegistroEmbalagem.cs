namespace Embalagem.Api.Models;

public class RegistroEmbalagem
{
    public int PedidoId { get; set; }

    public string? CaixaId { get; set; }

    public required string ProdutoId { get; set; }
}