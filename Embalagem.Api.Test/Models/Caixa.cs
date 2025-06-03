using Embalagem.Api.Helpers;
using Embalagem.Api.Models;

namespace Embalagem.Api.Test.Models;

public class CaixaTest
{
    [Fact]
    public void Comporta_DimensoesMaiores_RetornaFalso()
    {
        // Arrange
        var caixa = new Caixa()
        {
            CaixaId = "_",
            Dimensoes = new()
            {
                Altura = 5,
                Largura = 10,
                Comprimento = 7
            }
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Dimensoes.Altura + 1,
            Largura = caixa.Dimensoes.Largura + 1,
            Comprimento = caixa.Dimensoes.Comprimento + 1
        };

        // Act
        var comporta = caixa.Comporta(dimensoes);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_DimensoesIguais_RetornaFalso()
    {
        // Arrange
        var caixa = new Caixa()
        {
            CaixaId = "_",
            Dimensoes = new()
            {
                Altura = 25,
                Largura = 40,
                Comprimento = 30
            }
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Dimensoes.Altura,
            Largura = caixa.Dimensoes.Largura,
            Comprimento = caixa.Dimensoes.Comprimento
        };

        // Act
        var comporta = caixa.Comporta(dimensoes);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_DimensoesMenores_RetornaVerdadeiro()
    {
        // Arrange
        var caixa = new Caixa()
        {
            CaixaId = "_",
            Dimensoes = new()
            {
                Altura = 15,
                Largura = 30,
                Comprimento = 45
            }
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Dimensoes.Altura - 1,
            Largura = caixa.Dimensoes.Largura - 1,
            Comprimento = caixa.Dimensoes.Comprimento - 1
        };

        // Act
        var comporta = caixa.Comporta(dimensoes);

        // Assert
        Assert.True(comporta);
    }
}