using Embalagem.Api.Models;
using Embalagem.Api.Views;

namespace Embalagem.Api.Test.Models;

public class CaixaModelTest
{
    [Fact]
    public void Comporta_ProdutoDeVolumeMaior_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 5,
            Largura = 10,
            Comprimento = 7
        };
        var produto = new Produto()
        {
            ProdutoId = "_",
            Dimensoes = new()
            {
                Altura = caixa.Altura + 1,
                Largura = caixa.Largura + 1,
                Comprimento = caixa.Comprimento + 1
            }
        };

        // Act
        var comporta = caixa.Comporta(produto);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_ProdutoDeVolumeIgual_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 25,
            Largura = 40,
            Comprimento = 30
        };
        var produto = new Produto()
        {
            ProdutoId = "_",
            Dimensoes = new()
            {
                Altura = caixa.Altura,
                Largura = caixa.Largura,
                Comprimento = caixa.Comprimento
            }
        };

        // Act
        var comporta = caixa.Comporta(produto);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_ProdutoDeVolumeMenor_RetornaVerdadeiro()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 15,
            Largura = 30,
            Comprimento = 45
        };
        var produto = new Produto()
        {
            ProdutoId = "_",
            Dimensoes = new()
            {
                Altura = caixa.Altura - 1,
                Largura = caixa.Largura - 1,
                Comprimento = caixa.Comprimento - 1
            }
        };

        // Act
        var comporta = caixa.Comporta(produto);

        // Assert
        Assert.True(comporta);
    }


    [Fact]
    public void Comporta_PedidoDeVolumeMaior_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 30,
            Largura = 40,
            Comprimento = 50
        };
        var pedido = new Pedido()
        {
            PedidoId = 0,
            Produtos = new List<Produto>()
            {
                new()
                {
                    ProdutoId = "_",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura / 2,
                        Largura = caixa.Largura / 2,
                        Comprimento = caixa.Comprimento / 2
                    }
                },
                new()
                {
                    ProdutoId = "__",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura - 1,
                        Largura = caixa.Largura - 1,
                        Comprimento = caixa.Comprimento - 1
                    }
                }
            }
        };

        // Act
        var comporta = caixa.Comporta(pedido);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_PedidoDeVolumeIgual_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 50,
            Largura = 60,
            Comprimento = 40
        };
        var pedido = new Pedido()
        {
            PedidoId = 0,
            Produtos = new List<Produto>()
            {
                new()
                {
                    ProdutoId = "_",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura / 2,
                        Largura = caixa.Largura / 2,
                        Comprimento = caixa.Comprimento / 2
                    }
                },
                new()
                {
                    ProdutoId = "__",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura / 2,
                        Largura = caixa.Largura / 2,
                        Comprimento = caixa.Comprimento / 2
                    }
                }
            }
        };

        // Act
        var comporta = caixa.Comporta(pedido);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_PedidoDeVolumeMenor_RetornaVerdadeiro()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 20,
            Largura = 15,
            Comprimento = 18
        };
        var pedido = new Pedido()
        {
            PedidoId = 0,
            Produtos = new List<Produto>()
            {
                new()
                {
                    ProdutoId = "_",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura / 3,
                        Largura = caixa.Largura / 3,
                        Comprimento = caixa.Comprimento / 3
                    }
                },
                new()
                {
                    ProdutoId = "__",
                    Dimensoes = new()
                    {
                        Altura = caixa.Altura / 3,
                        Largura = caixa.Largura / 3,
                        Comprimento = caixa.Comprimento / 3
                    }
                }
            }
        };

        // Act
        var comporta = caixa.Comporta(pedido);

        // Assert
        Assert.True(comporta);
    }

    [Fact]
    public void Comporta_PedidosDeVolumeMaior_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 55,
            Largura = 70,
            Comprimento = 60
        };
        var pedidos = new List<Pedido>()
        {
            new()
            {
                PedidoId = 0,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "x",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 3,
                            Largura = caixa.Largura / 3,
                            Comprimento = caixa.Comprimento / 3
                        }
                    },
                    new()
                    {
                        ProdutoId = "y",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 3,
                            Largura = caixa.Largura / 3,
                            Comprimento = caixa.Comprimento / 3
                        }
                    }
                }
            },
            new()
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "z",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura,
                            Largura = caixa.Largura,
                            Comprimento = caixa.Comprimento
                        }
                    }
                }
            }
        }.OrderBy(p => p.Volume);

        // Act
        var comporta = caixa.Comporta(pedidos);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_PedidosDeVolumeIgual_RetornaFalso()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 15,
            Largura = 15,
            Comprimento = 15
        };
        var pedidos = new List<Pedido>()
        {
            new()
            {
                PedidoId = 0,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "x",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 3,
                            Largura = caixa.Largura / 3,
                            Comprimento = caixa.Comprimento / 3
                        }
                    },
                    new()
                    {
                        ProdutoId = "y",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 3,
                            Largura = caixa.Largura / 3,
                            Comprimento = caixa.Comprimento / 3
                        }
                    }
                }
            },
            new()
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "z",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 3,
                            Largura = caixa.Largura / 3,
                            Comprimento = caixa.Comprimento / 3
                        }
                    }
                }
            }
        }.OrderBy(p => p.Volume);

        // Act
        var comporta = caixa.Comporta(pedidos);

        // Assert
        Assert.False(comporta);
    }

    [Fact]
    public void Comporta_PedidosDeVolumeMenor_RetornaVerdadeiro()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "_",
            Altura = 24,
            Largura = 32,
            Comprimento = 48
        };
        var pedidos = new List<Pedido>()
        {
            new()
            {
                PedidoId = 0,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "x",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 4,
                            Largura = caixa.Largura / 4,
                            Comprimento = caixa.Comprimento / 4
                        }
                    },
                    new()
                    {
                        ProdutoId = "y",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 8,
                            Largura = caixa.Largura / 8,
                            Comprimento = caixa.Comprimento / 8
                        }
                    }
                }
            },
            new()
            {
                PedidoId = 1,
                Produtos = new List<Produto>()
                {
                    new()
                    {
                        ProdutoId = "z",
                        Dimensoes = new()
                        {
                            Altura = caixa.Altura / 6,
                            Largura = caixa.Largura / 6,
                            Comprimento = caixa.Comprimento / 6
                        }
                    }
                }
            }
        }.OrderBy(p => p.Volume);

        // Act
        var comporta = caixa.Comporta(pedidos);

        // Assert
        Assert.True(comporta);
    }

    [Fact]
    public void Embalar_EmbalaTodosOsProdutosDoPedido()
    {
        // Arrange
        var caixa = new CaixaModel()
        {
            CaixaId = "caixa 1",
            Altura = 100,
            Largura = 100,
            Comprimento = 100
        };
        var produtos = new List<Produto>()
        {
            new()
            {
                ProdutoId = "a",
                Dimensoes = new() { Altura = 1, Largura = 1, Comprimento = 1}
            },
            new()
            {
                ProdutoId = "b",
                Dimensoes = new() { Altura = 2, Largura = 2, Comprimento = 2}
            },
            new()
            {
               ProdutoId = "c",
               Dimensoes = new() { Altura = 3, Largura = 3, Comprimento = 3}
            }
        };
        
        // Act
        var embalagem = caixa.Embalar(produtos);
        var produtoIds = produtos.Select(p => p.ProdutoId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(caixa.CaixaId, embalagem.CaixaId);
            Assert.Equivalent(produtoIds, embalagem.Produtos);
            Assert.Null(embalagem.Observacao);
        });
    }
}