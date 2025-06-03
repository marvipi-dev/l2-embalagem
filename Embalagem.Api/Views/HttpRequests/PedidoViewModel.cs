using System.Text.Json.Serialization;
using Embalagem.Api.Helpers;

namespace Embalagem.Api.Views.HttpRequests;

public class PedidoViewModel
{
    [JsonPropertyName("pedido_id")] public required int PedidoId { get; set; }

    public required IEnumerable<ProdutoViewModel> Produtos { get; set; }

    [JsonIgnore]
    public int Volume => Produtos.Select(p => p.Dimensoes.Volume).Sum();

    [JsonIgnore]
    public Dimensoes Dimensoes => Produtos.Select(p => p.Dimensoes).Aggregate(new Dimensoes()
    {
        Altura = 0,
        Comprimento = 0,
        Largura = 0
    }, (dimensoesPedido, d) => dimensoesPedido + d);
    
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