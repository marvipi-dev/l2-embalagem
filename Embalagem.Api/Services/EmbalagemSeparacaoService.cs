using Embalagem.Api.Helpers;
using Embalagem.Api.Models;
using Embalagem.Api.Views.ExtensionMethods;
using Embalagem.Api.Views.HttpRequests;

namespace Embalagem.Api.Services;

public class EmbalagemSeparacaoService : IEmbalagemSeparacaoService
{
    public (IEnumerable<PedidoViewModel> Embalaveis, IEnumerable<PedidoViewModel> NaoEmbalaveis)
        Classificar(IEnumerable<PedidoViewModel> pedidos, IEnumerable<Caixa> caixas)
    {
        Validar(pedidos, caixas);

        var embalaveis = new List<PedidoViewModel>();
        var naoEmbalaveis = new List<PedidoViewModel>();

        if (!pedidos.Any())
        {
            return (embalaveis, naoEmbalaveis);
        }

        caixas = caixas.OrderByDescending(caixa => caixa.Volume);
        foreach (var pedido in pedidos)
        {
            foreach (var produto in pedido.Produtos)
            {
                if (!caixas.Any(caixa => caixa.Comporta(produto.Dimensoes)))
                {
                    Adicionar(produto, pedido.PedidoId, naoEmbalaveis);
                }
                else
                {
                    Adicionar(produto, pedido.PedidoId, embalaveis);
                }
            }
        }

        return (embalaveis, naoEmbalaveis);
    }

    public IEnumerable<PedidoViewModel> Separar(IEnumerable<PedidoViewModel> pedidos, IEnumerable<Caixa> caixas)
    {
        Validar(pedidos, caixas);

        if (!pedidos.Any())
        {
            return pedidos;
        }

        caixas = caixas.OrderBy(c => c.Volume);
        var prontosParaEmbalagem = new List<PedidoViewModel>();
        var precisaSeparacao = new List<PedidoViewModel>(pedidos);
        while (precisaSeparacao.Any())
        {
            foreach (var pedido in new List<PedidoViewModel>(precisaSeparacao))
            {
                if (caixas.Any(caixa => caixa.Comporta(pedido.Dimensoes())))
                {
                    prontosParaEmbalagem.Add(pedido);
                    precisaSeparacao.Remove(pedido);
                    continue;
                }
                
                var produtos = pedido.Produtos.OrderBy(produto => produto.Dimensoes);
                var qtdSeparados = 0;
                var dimensoesSeparados = new Dimensoes() { Altura = 0, Largura = 0, Comprimento = 0 };
                foreach (var produto in produtos)
                {
                    if (!caixas.Any(caixa => caixa.Comporta(dimensoesSeparados + produto.Dimensoes)))
                    {
                        break;
                    }
                    
                    qtdSeparados++;
                    dimensoesSeparados += produto.Dimensoes;
                }

                var separados = produtos.Take(qtdSeparados);
                var naoSeparados = produtos.Skip(qtdSeparados);

                precisaSeparacao.Remove(pedido);
                
                if (separados.Any())
                {
                    prontosParaEmbalagem.Add(new()
                    {
                        PedidoId = pedido.PedidoId,
                        Produtos = separados
                    });
                }

                if (naoSeparados.Any())
                {
                    precisaSeparacao.Add(new()
                    {
                        PedidoId = pedido.PedidoId,
                        Produtos = naoSeparados
                    });
                }
            }
        }

        return prontosParaEmbalagem;
    }

    // Assegura que pedidos e caixas não são nulos e que caixas não está vazio.
    private static void Validar(IEnumerable<PedidoViewModel> pedidos, IEnumerable<Caixa> caixas)
    {
        if (caixas == null)
        {
            throw new ArgumentNullException(paramName: nameof(caixas),
                message: $"O argumento '{nameof(caixas)}' não pode ser nulo.");
        }

        if (!caixas.Any())
        {
            throw new ArgumentException($"O argumento '{nameof(caixas)}' não pode estar vazio.");
        }

        if (pedidos == null)
        {
            throw new ArgumentNullException(paramName: nameof(pedidos),
                message: $"O argumento '{nameof(pedidos)}' não pode ser nulo.");
        }
    }

    // Adiciona um determinado produto de um determindo pedido a uma lista de pedidos.
    private static void Adicionar(ProdutoViewModel produto, int pedidoId, List<PedidoViewModel> pedidos)
    {
        var indicePedido = pedidos.FindIndex(pedido => pedido.PedidoId == pedidoId);
        if (indicePedido < 0)
        {
            pedidos.Add(new()
            {
                PedidoId = pedidoId,
                Produtos = new List<ProdutoViewModel>() { produto }
            });
        }
        else
        {
            var pedido = pedidos[indicePedido];
            pedido.Produtos = pedido.Produtos.Append(produto);
        }
    }
}