using Embalagem.Api.Models;
using Embalagem.Api.Services;
using Embalagem.Api.Views.HttpRequests;

namespace Embalagem.Api.Test.Services;

public class EmbalagemSeparacaoServiceUnitTest
{
    [Fact]
    public void Classificar_CaixasNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => sepService.Classificar(pedidos, null));
    }
    
    [Fact]
    public void Classificar_CaixasVazio_LevantaUmArgumentException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();
        var caixas = new List<Caixa>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => sepService.Classificar(pedidos, caixas));
    }
    
    [Fact]
    public void Classificar_PedidosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => sepService.Classificar(null, Caixas));
    }
    
    [Fact]
    public void Classificar_PedidosVazios_RetornaEmbalaveisENaoEmbalaveisVazios()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();

        // Act
        (var embalaveis, var naoEmbalaveis) = sepService.Classificar(pedidos, Caixas);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Empty(embalaveis);
            Assert.Empty(naoEmbalaveis);
        });
    }

    [Theory]
    [MemberData(nameof(PedidosCaixaUnica))]
    public void Classificar_TodosOsProdutosCabemEmAlgumaCaixa_RetornaEmbalaveisContendoOsPedidos
        (IEnumerable<PedidoViewModel> pedidos)
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        // Act
        (var embalaveis, _) = sepService.Classificar(pedidos, Caixas);


        // Assert
        Assert.Equivalent(pedidos, embalaveis);
    }

    [Theory]
    [MemberData(nameof(PedidosCaixaUnica))]
    public void Classificar_TodosOsProdutosCabemEmAlgumaCaixa_RetornaNaoEmbalaveisVazio
        (IEnumerable<PedidoViewModel> pedidos)
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        // Act
        (_, var naoEmbalaveis) = sepService.Classificar(pedidos, Caixas);


        // Assert
        Assert.Empty(naoEmbalaveis);
    }

    [Theory]
    [MemberData(nameof(PedidosNaoEmbalaveis))]
    public void Classificar_TodosOsProdutosNaoCabemEmNenhumaCaixa_RetornaEmbalaveisVazio
        (IEnumerable<PedidoViewModel> pedidos)
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        // Act
        (var embalaveis, _) = sepService.Classificar(pedidos, Caixas);

        // Assert
        Assert.Empty(embalaveis);
    }

    [Theory]
    [MemberData(nameof(PedidosNaoEmbalaveis))]
    public void Classificar_TodosOsProdutosNaoCabemEmNenhumaCaixa_RetornaNaoEmbalaveisContendoOsPedidos
        (IEnumerable<PedidoViewModel> pedidos)
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        // Act
        (_, var naoEmbalaveis) = sepService.Classificar(pedidos, Caixas);

        // Assert
        Assert.Equivalent(pedidos, naoEmbalaveis);
    }

    [Fact]
    public void Classificar_AlgunsProdutosCabemEmAlgumaCaixa_SeparaOsProdutosCorretamente()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        var cabivelTablet = new ProdutoViewModel()
            { ProdutoId = "Tablet", Dimensoes = new() { Altura = 1, Largura = 25, Comprimento = 17 } };
        var incabivelCadeiraGamer = new ProdutoViewModel()
            { ProdutoId = "Cadeira Gamer", Dimensoes = new() { Altura = 120, Largura = 60, Comprimento = 70 } };

        var incabivelDelorian = new ProdutoViewModel()
            { ProdutoId = "Delorian Modelo", Dimensoes = new() { Altura = 114, Largura = 184, Comprimento = 428 } };
        var cabivelHdExterno = new ProdutoViewModel()
            { ProdutoId = "HD Externo", Dimensoes = new() { Altura = 2, Largura = 8, Comprimento = 12 } };
        var cabivelPendrive = new ProdutoViewModel()
            { ProdutoId = "Pendrive", Dimensoes = new() { Altura = 1, Largura = 2, Comprimento = 5 } };
        
        var pedidos = new List<PedidoViewModel>()
        {
            new() { PedidoId = 13, Produtos = new List<ProdutoViewModel>() { cabivelTablet, incabivelCadeiraGamer } },
            new() { PedidoId = 14, Produtos = new List<ProdutoViewModel>() { incabivelDelorian, cabivelHdExterno, cabivelPendrive } }
        };
        
        var cabiveis = new List<PedidoViewModel>()
        {
            new() { PedidoId = 13, Produtos = new List<ProdutoViewModel>() { cabivelTablet } },
            new() { PedidoId = 14, Produtos = new List<ProdutoViewModel>() { cabivelHdExterno, cabivelPendrive } }
        };
        
        var incabiveis = new List<PedidoViewModel>()
        {
            new() { PedidoId = 13, Produtos = new List<ProdutoViewModel>() { incabivelCadeiraGamer } },
            new() { PedidoId = 14, Produtos = new List<ProdutoViewModel>() { incabivelDelorian } }
        };

        // Act
        (var embalaveis, var naoEmbalaveis) = sepService.Classificar(pedidos, Caixas);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equivalent(cabiveis, embalaveis);
            Assert.Equivalent(incabiveis, naoEmbalaveis);
        });
    }
    
    
    [Fact]
    public void Separar_CaixasNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => sepService.Separar(pedidos, null));
    }
    
    [Fact]
    public void Separar_CaixasVazio_LevantaUmArgumentException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();
        var caixas = new List<Caixa>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => sepService.Separar(pedidos, caixas));
    }
    
    [Fact]
    public void Separar_PedidosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => sepService.Separar(null, Caixas));
    }
    
    [Fact]
    public void Separar_PedidosVazios_RetornaUmaListaVazia()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        var pedidos = new List<PedidoViewModel>();

        // Act
        var prontosParaEmbalagem = sepService.Separar(pedidos, Caixas);

        // Assert
        Assert.Empty(prontosParaEmbalagem);
    }

    [Theory]
    [MemberData(nameof(PedidosCaixaUnica))]
    public void Separar_TodosOsPedidosCabemInteiramenteEmUmaUnicaCaixa_RetornaOsPedidosSemAlteracao
        (IEnumerable<PedidoViewModel> pedidos)
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();

        // Act
        var prontosParaEmbalagem = sepService.Separar(pedidos, Caixas);
        
        // Assert
        Assert.Equivalent(pedidos, prontosParaEmbalagem);
    }

    [Fact]
    public void Separar_NenhumPedidoCabeInteiramenteEmUmaCaixa_SeparaOsProdutosEmPedidosDiferentes()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        
        var ps5 = new ProdutoViewModel()
            { ProdutoId = "PS5", Dimensoes = new() { Altura = 40, Largura = 10, Comprimento = 25 } };
        var volante = new ProdutoViewModel()
            { ProdutoId = "Volante", Dimensoes = new() { Altura = 40, Largura = 30, Comprimento = 30 } };

        var webCam = new ProdutoViewModel()
            { ProdutoId = "Webcam", Dimensoes = new() { Altura = 7, Largura = 10, Comprimento = 5 } };
        var microfone = new ProdutoViewModel()
            { ProdutoId = "Microfone", Dimensoes = new() { Altura = 25, Largura = 10, Comprimento = 10 } };
        var monitor = new ProdutoViewModel()
            { ProdutoId = "Monitor", Dimensoes = new() { Altura = 50, Largura = 60, Comprimento = 20 } };
        var notebook = new ProdutoViewModel()
            { ProdutoId = "Notebook", Dimensoes = new() { Altura = 2, Largura = 35, Comprimento = 25 } };
        
        var pedidos = new List<PedidoViewModel>()
        {
            new() { PedidoId = 1, Produtos = new List<ProdutoViewModel>() { ps5, volante } },
            new() { PedidoId = 6, Produtos = new List<ProdutoViewModel>() { webCam, microfone, monitor, notebook } }
        };
        
        var esperado = new List<PedidoViewModel>()
        {
            new() { PedidoId = 1, Produtos = new List<ProdutoViewModel>() { ps5 } },
            new() { PedidoId = 1, Produtos = new List<ProdutoViewModel>() { volante } },
            new() { PedidoId = 6, Produtos = new List<ProdutoViewModel>() { webCam, microfone, notebook } },
            new() { PedidoId = 6, Produtos = new List<ProdutoViewModel>() { monitor } }
        };
        
        // Act
        var prontosParaEmbalar = sepService.Separar(pedidos, Caixas);
        
        // Assert
        Assert.Equivalent(esperado, prontosParaEmbalar);
    }
    
    [Fact]
    public void Separar_AlgunsPedidosCabemInteiramenteEmUmaUnicaCaixa_SeparaOsProdutosEmPedidosDiferentes()
    {
        // Arrange
        var sepService = new EmbalagemSeparacaoService();
        
        var notebook = new ProdutoViewModel()
            { ProdutoId = "Notebook", Dimensoes = new() { Altura = 2, Largura = 35, Comprimento = 25 } };
        var ps5 = new ProdutoViewModel()
            { ProdutoId = "PS5", Dimensoes = new() { Altura = 40, Largura = 10, Comprimento = 25 } };
        var volante = new ProdutoViewModel()
            { ProdutoId = "Volante", Dimensoes = new() { Altura = 40, Largura = 30, Comprimento = 30 } };
        

        var webCam = new ProdutoViewModel()
            { ProdutoId = "Webcam", Dimensoes = new() { Altura = 7, Largura = 10, Comprimento = 5 } };
        var microfone = new ProdutoViewModel()
            { ProdutoId = "Microfone", Dimensoes = new() { Altura = 25, Largura = 10, Comprimento = 10 } };

        
        var pedidos = new List<PedidoViewModel>()
        {
            new() { PedidoId = 15, Produtos = new List<ProdutoViewModel>() { notebook, ps5, volante } },
            new() { PedidoId = 16, Produtos = new List<ProdutoViewModel>() { webCam, microfone } }
        };
        
        var esperado = new List<PedidoViewModel>()
        {
            new() { PedidoId = 15, Produtos = new List<ProdutoViewModel>() { notebook, ps5 } },
            new() { PedidoId = 15, Produtos = new List<ProdutoViewModel>() { volante } },
            new() { PedidoId = 16, Produtos = new List<ProdutoViewModel>() { webCam, microfone } },
        };
        
        // Act
        var prontosParaEmbalar = sepService.Separar(pedidos, Caixas);
        
        // Assert
        Assert.Equivalent(esperado, prontosParaEmbalar);
    }
    

    public static IEnumerable<Caixa> Caixas => new List<Caixa>()
    {
        new() { CaixaId = "_", Altura = 30, Largura = 40, Comprimento = 80 },
        new() { CaixaId = "__", Altura = 80, Largura = 50, Comprimento = 40 },
        new() { CaixaId = "___", Altura = 50, Largura = 80, Comprimento = 60 },
    };

    public static TheoryData<IEnumerable<PedidoViewModel>> PedidosCaixaUnica => new()
    {
        new List<PedidoViewModel>()
        {
            new()
            {
                PedidoId = 1, Produtos = new List<ProdutoViewModel>()
                {
                    new() { ProdutoId = "Joystick", Dimensoes = new() { Altura = 15, Largura = 20, Comprimento = 10 } },
                    new() { ProdutoId = "Fifa 24", Dimensoes = new() { Altura = 10, Largura = 30, Comprimento = 10 } },
                    new()
                    {
                        ProdutoId = "Call of Duty", Dimensoes = new() { Altura = 30, Largura = 15, Comprimento = 10 }
                    }
                }
            },
            new()
            {
                PedidoId = 3, Produtos = new List<ProdutoViewModel>()
                {
                    new() { ProdutoId = "Headset", Dimensoes = new() { Altura = 25, Largura = 15, Comprimento = 20 } }
                }
            },
            new()
            {
                PedidoId = 4, Produtos = new List<ProdutoViewModel>()
                {
                    new()
                    {
                        ProdutoId = "Mouse Gamer", Dimensoes = new() { Altura = 5, Largura = 8, Comprimento = 12 }
                    },
                    new()
                    {
                        ProdutoId = "Teclado Mecanico", Dimensoes = new() { Altura = 4, Largura = 45, Comprimento = 15 }
                    }
                }
            },
            new()
            {
                PedidoId = 7, Produtos = new List<ProdutoViewModel>()
                {
                    new()
                    {
                        ProdutoId = "Jogo de Cabos", Dimensoes = new() { Altura = 5, Largura = 15, Comprimento = 10 }
                    }
                }
            },
            new()
            {
                PedidoId = 8, Produtos = new List<ProdutoViewModel>()
                {
                    new()
                    {
                        ProdutoId = "Controle Xbox", Dimensoes = new() { Altura = 10, Largura = 15, Comprimento = 10 }
                    },
                    new() { ProdutoId = "Carregador", Dimensoes = new() { Altura = 3, Largura = 8, Comprimento = 8 } }
                }
            },
            new()
            {
                PedidoId = 9, Produtos = new List<ProdutoViewModel>()
                {
                    new() { ProdutoId = "Tablet", Dimensoes = new() { Altura = 1, Largura = 25, Comprimento = 17 } }
                }
            },
            new()
            {
                PedidoId = 10, Produtos = new List<ProdutoViewModel>()
                {
                    new() { ProdutoId = "HD Externo", Dimensoes = new() { Altura = 2, Largura = 8, Comprimento = 12 } },
                    new() { ProdutoId = "Pendrive", Dimensoes = new() { Altura = 1, Largura = 2, Comprimento = 5 } }
                }
            }
        }
    };

    public static TheoryData<IEnumerable<PedidoViewModel>> PedidosNaoEmbalaveis => new()
    {
        new List<PedidoViewModel>()
        {
            new()
            {
                PedidoId = 11, Produtos = new List<ProdutoViewModel>()
                {
                    new()
                    {
                        ProdutoId = "Cadeira Gamer", Dimensoes = new() { Altura = 120, Largura = 60, Comprimento = 70 }
                    }
                }
            },
            new()
            {
                PedidoId = 12, Produtos = new List<ProdutoViewModel>()
                {
                    new()
                    {
                        ProdutoId = "Delorian Modelo",
                        Dimensoes = new() { Altura = 114, Largura = 184, Comprimento = 428 }
                    },
                    new()
                    {
                        ProdutoId = "Cabine VR", Dimensoes = new() { Altura = 960, Largura = 960, Comprimento = 190 }
                    }
                }
            }
        }
    };
}