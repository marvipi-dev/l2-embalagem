namespace Embalagem.Api.Views;

public class Dimensoes
{
    public int Altura { get; set; }
    public int Largura { get; set; }
    public int Comprimento { get; set; }
    public int Volume => Altura * Largura * Comprimento;

    protected bool Equals(Dimensoes other)
    {
        return Altura == other.Altura && Largura == other.Largura && Comprimento == other.Comprimento;
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

    public static Dimensoes operator +(Dimensoes esquerda, Dimensoes direita)
    {
        return new()
        {
            Altura = esquerda.Altura + direita.Altura,
            Largura = esquerda.Largura + direita.Largura,
            Comprimento = esquerda.Comprimento + direita.Comprimento
        };
    }
    
    public static bool operator <(Dimensoes esquerda, Dimensoes direita)
    {
        return esquerda.Altura < direita.Altura
               && esquerda.Largura < direita.Largura
               && esquerda.Comprimento < direita.Comprimento;
    }
    
    public static bool operator >(Dimensoes esquerda, Dimensoes direita)
    {
        return esquerda.Altura > direita.Altura
               && esquerda.Largura > direita.Largura
               && esquerda.Comprimento > direita.Comprimento;
    }
}