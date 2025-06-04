using Embalagem.Api.Helpers;
using Embalagem.Api.Models;

namespace Embalagem.Api.Test.Models;

public class CaixaUnitTest
{
    [Fact]
    public void Comporta_DimensoesMaiores_RetornaFalso()
    {
        // Arrange
        var caixa = new Caixa()
        {
            CaixaId = "_", Altura = 5,
            Largura = 10,
            Comprimento = 7
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Altura + 1,
            Largura = caixa.Largura + 1,
            Comprimento = caixa.Comprimento + 1
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
            CaixaId = "_", Altura = 25,
            Largura = 40,
            Comprimento = 30
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Altura,
            Largura = caixa.Largura,
            Comprimento = caixa.Comprimento
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
            CaixaId = "_", Altura = 15,
            Largura = 30,
            Comprimento = 45
        };
        var dimensoes = new Dimensoes()
        {
            Altura = caixa.Altura - 1,
            Largura = caixa.Largura - 1,
            Comprimento = caixa.Comprimento - 1
        };

        // Act
        var comporta = caixa.Comporta(dimensoes);

        // Assert
        Assert.True(comporta);
    }
}