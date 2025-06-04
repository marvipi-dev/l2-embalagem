namespace Embalagem.Api.Helpers;

public class Dimensoes : IComparable<Dimensoes>, IComparable
{
    public required int Altura { get; set; }
    public required int Largura { get; set; }
    public required int Comprimento { get; set; }
    public int Volume => Altura * Largura * Comprimento;

    public int Menor => new List<int>() { Altura, Largura, Comprimento }.Order().First();
    public int Media => new List<int>() { Altura, Largura, Comprimento }.Order().ElementAt(1);
    public int Maior => new List<int>() { Altura, Largura, Comprimento }.Order().Last();
    

    public static Dimensoes operator +(Dimensoes esquerda, Dimensoes direita)
    {
        return new()
        {
            Altura = esquerda.Altura + direita.Altura,
            Largura = esquerda.Largura + direita.Largura,
            Comprimento = esquerda.Comprimento + direita.Comprimento
        };
    }
    
    public static Dimensoes operator -(Dimensoes esquerda, Dimensoes direita)
    {
        return new()
        {
            Altura = esquerda.Altura - direita.Altura,
            Largura = esquerda.Largura - direita.Largura,
            Comprimento = esquerda.Comprimento - direita.Comprimento
        };
    }
    
    public static bool operator <(Dimensoes esquerda, Dimensoes direita)
    {
        return esquerda.Menor < direita.Menor
               && esquerda.Media < direita.Media
               && esquerda.Maior < direita.Maior;
    }
    
    public static bool operator >(Dimensoes esquerda, Dimensoes direita)
    {
        return  esquerda.Menor > direita.Menor
                && esquerda.Media > direita.Media
                && esquerda.Maior > direita.Maior;
    }
    
    public static bool operator ==(Dimensoes esquerda, Dimensoes direita)
    {
        return esquerda.Menor == direita.Menor
               && esquerda.Media == direita.Media
               && esquerda.Maior == direita.Maior;
    }
    
    public static bool operator !=(Dimensoes esquerda, Dimensoes direita)
    {
        return esquerda.Menor != direita.Menor
               || esquerda.Media != direita.Media
               || esquerda.Maior != direita.Maior;
    }
    
    protected bool Equals(Dimensoes other)
    {
        return this == other;
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

        return Equals((Dimensoes)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Altura, Largura, Comprimento);
    }
    
    public int CompareTo(Dimensoes? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        if (this < other)
        {
            return -1;
        }

        if (this == other)
        {
            return 0;
        }

        return 1;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (ReferenceEquals(this, obj))
        {
            return 0;
        }

        return obj is Dimensoes other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Dimensoes)}");
    }
}