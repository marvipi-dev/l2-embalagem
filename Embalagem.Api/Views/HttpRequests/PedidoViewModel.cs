using System.Text.Json.Serialization;

namespace Embalagem.Api.Views.HttpRequests;

public class PedidoViewModel
{
    [JsonPropertyName("pedido_id")] public required int PedidoId { get; set; }

    public required IEnumerable<ProdutoViewModel> Produtos { get; set; }

    [JsonIgnore]
    public int Volume => Produtos.Select(p => p.Dimensoes.Volume).Sum();
    
    protected bool Equals(PedidoViewModel other)
    {
        return PedidoId == other.PedidoId && Produtos.Equals(other.Produtos);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((PedidoViewModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PedidoId, Produtos);
    }
}