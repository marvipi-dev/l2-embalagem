using System.Text.Json.Serialization;
using Embalagem.Api.Helpers;

namespace Embalagem.Api.Views.HttpRequests;

public class ProdutoViewModel
{
    [JsonPropertyName("produto_id")] public required string ProdutoId { get; set; }

    public required Dimensoes Dimensoes { get; set; }

    protected bool Equals(ProdutoViewModel other)
    {
        return ProdutoId == other.ProdutoId && Dimensoes.Equals(other.Dimensoes);
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

        return Equals((ProdutoViewModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ProdutoId, Dimensoes);
    }
}