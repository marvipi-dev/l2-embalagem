using Embalagem.Api.Helpers;

namespace Embalagem.Api.Test.Helpers;

public class DimensoesUnitTest
{
    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(8, 11, 13, 8)]
    [InlineData(3, 2, 9, 2)]
    [InlineData(30, 15, 10, 10)]
    [InlineData(5, 5, 7, 5)]
    [InlineData(17, 14, 17, 14)]
    [InlineData(6, 4, 4, 4)]
    public void Menor_Medida_RetornaAMedidaMenor(int altura, int largura, int comprimento, int menor)
    {
        // Arrange
        var dimensoes = new Dimensoes()
        {
            Altura = altura,
            Largura = largura,
            Comprimento = comprimento
        };

        // Act
        var resultado = dimensoes.Menor;

        // Assert
        Assert.Equal(menor, resultado);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(8, 11, 13, 11)]
    [InlineData(3, 2, 9, 3)]
    [InlineData(30, 10, 15, 15)]
    [InlineData(5, 5, 7, 5)]
    [InlineData(17, 14, 17, 17)]
    [InlineData(6, 4, 4, 4)]
    public void Media_Medida_RetornaAMedidaMedia(int altura, int largura, int comprimento, int media)
    {
        // Arrange
        var dimensoes = new Dimensoes()
        {
            Altura = altura,
            Largura = largura,
            Comprimento = comprimento
        };

        // Act
        var resultado = dimensoes.Media;

        // Assert
        Assert.Equal(media, resultado);
    }


    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(8, 11, 13, 13)]
    [InlineData(9, 2, 3, 9)]
    [InlineData(15, 30, 10, 30)]
    [InlineData(5, 7, 7, 7)]
    [InlineData(17, 14, 17, 17)]
    [InlineData(6, 6, 4, 6)]
    public void Maior_Medida_RetornaAMedidaMaior(int altura, int largura, int comprimento, int maior)
    {
        // Arrange
        var dimensoes = new Dimensoes()
        {
            Altura = altura,
            Largura = largura,
            Comprimento = comprimento
        };

        // Act
        var resultado = dimensoes.Maior;

        // Assert
        Assert.Equal(maior, resultado);
    }

    [Fact]
    public void Adicionar_AdicionaAsMedidasCorrespondentes()
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = 1,
            Largura = 2,
            Comprimento = 3
        };
        var direita = new Dimensoes()
        {
            Altura = 4,
            Largura = 5,
            Comprimento = 6
        };

        // Act
        var resultado = esquerda + direita;
        var alturaEsperada = esquerda.Altura + direita.Altura;
        var larguraEsperada = esquerda.Largura + direita.Largura;
        var comprimentoEsperado = esquerda.Comprimento + direita.Comprimento;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(alturaEsperada, resultado.Altura);
            Assert.Equal(larguraEsperada, resultado.Largura);
            Assert.Equal(comprimentoEsperado, resultado.Comprimento);
        });
    }

    [Fact]
    public void Subtrair_SubtraiAsMedidasCorrespondente()
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = 10,
            Largura = 9,
            Comprimento = 8
        };
        var direita = new Dimensoes()
        {
            Altura = 2,
            Largura = 2,
            Comprimento = 2
        };

        // Act
        var resultado = esquerda - direita;
        var alturaEsperada = esquerda.Altura - direita.Altura;
        var larguraEsperada = esquerda.Largura - direita.Largura;
        var comprimentoEsperado = esquerda.Comprimento - direita.Comprimento;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(alturaEsperada, resultado.Altura);
            Assert.Equal(larguraEsperada, resultado.Largura);
            Assert.Equal(comprimentoEsperado, resultado.Comprimento);
        });
    }

    [Theory]
    [MemberData(nameof(DadosMenorQuePositivo))]
    public void MenorQue_TodasAsMedidasDeMesmoGrauMenores_RetornaVerdadeiro(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.True(esquerda < direita);
    }

    [Theory]
    [MemberData(nameof(DadosMenorQueNegativo))]
    public void MenorQue_PeloMenosUmaMedidaDeMesmoGrauMaiorOuIgual_RetornaFalso(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.False(esquerda < direita);
    }

    [Theory]
    [MemberData(nameof(DadosMenorQueNegativo))]
    public void MaiorQue_TodasAsMedidasDeMesmoGrauMaiores_RetornaVerdadeiro(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.True(esquerda > direita);
    }

    [Theory]
    [MemberData(nameof(DadosMenorQuePositivo))]
    public void MaiorQue_PeloMenosUmaMedidaDeMesmoGrauMenorOuIgual_RetornaFalso(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.False(esquerda > direita);
    }

    [Theory]
    [MemberData(nameof(DadosIgualAPositivo))]
    public void IgualA_TodasAsMedidasDeMesmoGrauIguais_RetornaVerdadeiro(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.True(esquerda == direita);
    }

    [Theory]
    [MemberData(nameof(DadosIgualANegativo))]
    public void IgualA_PeloMenosUmaMedidaDeMesmoGrauDiferente_RetornaFalso(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.False(esquerda == direita);
    }

    [Theory]
    [MemberData(nameof(DadosIgualANegativo))]
    public void DiferenteDe_TodasAsMedidasDeMesmoGrauDiferentes_RetornaVerdadeiro(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.True(esquerda != direita);
    }

    [Theory]
    [MemberData(nameof(DadosIgualAPositivo))]
    public void DiferenteDe_PeloMenosUmaMedidaDeMesmoGrauIgual_RetornaFalso(
        int medidaEsq1, int medidaEsq2, int medidaEsq3,
        int medidaDir1, int medidaDir2, int medidaDir3)
    {
        // Arrange
        var esquerda = new Dimensoes()
        {
            Altura = medidaEsq1,
            Largura = medidaEsq2,
            Comprimento = medidaEsq3
        };
        var direita = new Dimensoes()
        {
            Altura = medidaDir1,
            Largura = medidaDir2,
            Comprimento = medidaDir3
        };

        // Act
        // Assert
        Assert.False(esquerda != direita);
    }

    public static IEnumerable<object[]> DadosMenorQuePositivo
    {
        get
        {
            var dadosEsq = new List<object[]>()
            {
                new object[] { 1, 2, 3 },
                new object[] { 21, 35, 36 },
                new object[] { 8, 94, 101 }
            };
            var dadosDir = new List<object[]>()
            {
                new object[] { 4, 5, 6 },
                new object[] { 22, 36, 37 },
                new object[] { 40, 100, 150 }
            };

            var dados = Embaralhar(dadosEsq, dadosDir);
            return dados;
        }
    }



    public static IEnumerable<object[]> DadosMenorQueNegativo
    {
        get
        {
            var dadosEsq = new List<object[]>()
            {
                new object[] { 4, 5, 6 },
                new object[] { 22, 36, 37 },
                new object[] { 40, 100, 150 }
            };
            var dadosDir = new List<object[]>()
            {
                new object[] { 1, 2, 3 },
                new object[] { 21, 35, 36 },
                new object[] { 8, 94, 101 }
            };

            var dados = Embaralhar(dadosEsq, dadosDir);
            return dados;
        }
    }

    public static IEnumerable<object[]> DadosIgualAPositivo
    {
        get
        {
            var dadosEsq = new List<object[]>()
            {
                new object[] { 4, 5, 6 },
                new object[] { 22, 36, 37 },
                new object[] { 40, 100, 150 }
            };
            var dadosDir = new List<object[]>()
            {
                new object[] { 4, 5, 6 },
                new object[] { 22, 36, 37 },
                new object[] { 40, 100, 150 }
            };

            var dados = Embaralhar(dadosEsq, dadosDir);
            return dados;
        }
    }

    public static IEnumerable<object[]> DadosIgualANegativo
    {
        get
        {
            var dadosEsq = new List<object[]>()
            {
                new object[] { 2, 7, 9 },
                new object[] { 23, 36, 38 },
                new object[] { 50, 120, 160 }
            };
            var dadosDir = new List<object[]>()
            {
                new object[] { 1, 5, 6 },
                new object[] { 22, 35, 37 },
                new object[] { 40, 100, 150 }
            };

            var dados = Embaralhar(dadosEsq, dadosDir);
            return dados;
        }
    }
    
    private static IEnumerable<object[]> Embaralhar(List<object[]> dadosEsq, List<object[]> dadosDir)
    {
        var random = new Random();
        foreach (var dado in dadosEsq)
        {
            random.Shuffle(dado);
        }

        foreach (var dado in dadosDir)
        {
            random.Shuffle(dado);
        }

        var dados = new List<object[]>();
        for (int i = 0; i < dadosEsq.Count; i++)
        {
            dados.Add(dadosEsq[i].Concat(dadosDir[i]).ToArray());
        }

        return dados;
    }
}