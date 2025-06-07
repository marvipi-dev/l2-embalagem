using Embalagem.Api.Models;
using Embalagem.Api.Services;
using Embalagem.Api.Views.HttpRequests;
using Embalagem.Api.Views.HttpResponses;

namespace Embalagem.Api.Test.Services;

public class EmbalagemPreparacaoServiceUnitTest
{
    [Fact]
    public void PrepararEmbalaveis_CaixasNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var pedidos = new List<PedidoViewModel>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => prepService.PrepararEmbalaveis(pedidos, caixas: null));
    }
    
    [Fact]
    public void PrepararEmbalaveis_CaixasVazio_LevantaUmArgumentException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var pedidos = new List<PedidoViewModel>();
        var caixas = new List<Caixa>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => prepService.PrepararEmbalaveis(pedidos, caixas));
    }
    
    [Fact]
    public void PrepararEmbalaveis_PedidosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => prepService.PrepararEmbalaveis(pedidos: null, Caixas));
    }
    
    [Fact]
    public void PrepararEmbalaveis_PedidosVazio_RetornaUmaSequenciaVazia()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var pedidos = new List<PedidoViewModel>();

        // Act
        var embalados = prepService.PrepararEmbalaveis(pedidos, Caixas);
        
        // Assert
        Assert.Empty(embalados);
    }
    // TODO: testar o resto dos casos
    
    
    
    [Fact]
    public void PrepararNaoEmbalaveis_PedidosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => prepService.PrepararNaoEmbalaveis(pedidos: null));
    }
    
    [Fact]
    public void PrepararNaoEmbalaveis_PedidosVazio_RetornaUmaSequenciaVazia()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var pedidos = new List<PedidoViewModel>();

        // Act
        var naoEmbalados = prepService.PrepararNaoEmbalaveis(pedidos);
        
        // Assert
        Assert.Empty(naoEmbalados);
    }
    // TODO: testar o resto dos casos
    
    
    
    [Fact]
    public void AgruparPorPedidoId_EmbaladosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var naoEmbalados = new List<EmbalagemViewModel>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => prepService.AgruparPorPedidoId(embalados: null, naoEmbalados));
    }
    
    [Fact]
    public void AgruparPorPedidoId_NaoEmbaladosNulo_LevantaUmArgumentNullException()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var embalados = new List<EmbalagemViewModel>();
        
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => prepService.AgruparPorPedidoId(embalados, naoEmbalados: null));
    }
    
    [Fact]
    public void AgruparPorPedidoId_EmbaladosVazio_RetornaNaoEmbalados()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var embalados = new List<EmbalagemViewModel>();
        var naoEmbalados = new List<EmbalagemViewModel>() { new() {PedidoId = 0, Caixas = new List<CaixaViewModel>() }};

        // Act
        var agrupadosPorPedidoId = prepService.AgruparPorPedidoId(embalados, naoEmbalados);
        
        // Assert
        Assert.Equivalent(naoEmbalados, agrupadosPorPedidoId);
    }
    
    [Fact]
    public void AgruparPorPedidoId_NaoEmbaladosVazio_RetornaEmbalados()
    {
        // Arrange
        var prepService = new EmbalagemPreparacaoService();
        var embalados = new List<EmbalagemViewModel>() { new() {PedidoId = 0, Caixas = new List<CaixaViewModel>() }};
        var naoEmbalados = new List<EmbalagemViewModel>();

        // Act
        var agrupadosPorPedidoId = prepService.AgruparPorPedidoId(embalados, naoEmbalados);
        
        // Assert
        Assert.Equivalent(embalados, agrupadosPorPedidoId);
    }
    // TODO: testar o resto dos casos
    
    
    
    public static IEnumerable<Caixa> Caixas => new List<Caixa>()
    {
        new() { CaixaId = "caixa 1", Altura = 30, Largura = 40, Comprimento = 80 },
        new() { CaixaId = "caixa 2", Altura = 80, Largura = 50, Comprimento = 40 },
        new() { CaixaId = "caixa 3", Altura = 50, Largura = 80, Comprimento = 60 },
    };
}