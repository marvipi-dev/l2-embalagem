using Embalagem.Api.Data;
using Embalagem.Api.Models;
using Embalagem.Api.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Embalagem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmbalagemController : ControllerBase
{
    private readonly IRepository _repository;

    public EmbalagemController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<EmbalagemGetResponse> Embalagem()
    {
        return _repository.LerEmbalagens();
    }

    [HttpPost]
    public EmbalagemPostResponse Embalagem(EmbalagemPostRequest postRequest)
    {
        // Dar preferência às caixas de menor volume
        var caixas = _repository.LerCaixas().OrderBy(c => c.Volume);
        var pedidos = postRequest.Pedidos;

        var naoEmbalaveis = IdentificarNaoEmbalaveis(pedidos, caixas);
        var pedidosFiltrados = FiltrarEmbalaveis(naoEmbalaveis, pedidos);

        var embalagens = EmbalagemUnica(pedidosFiltrados, caixas);
        var embaladosUmaCaixa = embalagens.embalados;
        var pedidosVariasCaixas = embalagens.embalarEmVarias;

        var embaladosVariasCaixas = EmbalarVariasCaixas(pedidosVariasCaixas, caixas);

        var naoEmbalaveisRetorno = PrepararNaoEmbalaveis(naoEmbalaveis);

        var embalagemPostResponse = new EmbalagemPostResponse
        {
            Pedidos = naoEmbalaveisRetorno
                .Concat(embaladosVariasCaixas)
                .Concat(embaladosUmaCaixa)
        };

        _repository.Escrever(embalagemPostResponse.Pedidos);

        return embalagemPostResponse;
    }

    private List<(int PedidoId, Produto Produto)> IdentificarNaoEmbalaveis(IEnumerable<Pedido> pedidos,
        IOrderedEnumerable<CaixaModel> caixas)
    {
        var naoEmbalaveis = new List<(int PedidoId, Produto Produto)>();

        var volumeMaiorCaixa = caixas.Last().Volume;
        foreach (var pedido in pedidos)
        foreach (var produto in pedido.Produtos)
        {
            if (produto.Dimensoes.Volume > volumeMaiorCaixa)
            {
                naoEmbalaveis.Add((pedido.PedidoId, produto));
            }
        }

        return naoEmbalaveis;
    }

    private IEnumerable<Pedido> FiltrarEmbalaveis(IEnumerable<(int PedidoId, Produto Produto)> naoEmbalaveis,
        IEnumerable<Pedido> pedidos)
    {
        if (!naoEmbalaveis.Any())
        {
            return pedidos;
        }
        
        var pedidosFiltrados = new List<Pedido>();
        foreach (var naoEmbalavel in naoEmbalaveis)
        {
            pedidosFiltrados = pedidos.Select(pe => new Pedido
            {
                PedidoId = pe.PedidoId,
                Produtos = pe.PedidoId == naoEmbalavel.PedidoId
                    ? pe.Produtos.Where(p => !p.Equals(naoEmbalavel.Produto))
                    : pe.Produtos
            }).ToList();
        }

        pedidosFiltrados.RemoveAll(pe => !pe.Produtos.Any());
        return pedidosFiltrados;
    }

    private (IEnumerable<Views.Embalagem> embalados, IEnumerable<Pedido> embalarEmVarias) EmbalagemUnica(
        IEnumerable<Pedido> pedidos, IEnumerable<CaixaModel> caixas)
    {
        var embalagens = new List<Views.Embalagem>();
        var embalarEmVarias = new List<Pedido>();

        foreach (var pedido in pedidos)
        {
            var cabemUnicaCaixa = false;
            foreach (var caixa in caixas)
            {
                if (cabemUnicaCaixa = caixa.CabemTodos(pedido.Volume))
                {
                    var pedidoEmbalado = caixa.Embalar(pedido);
                    embalagens.Add(pedidoEmbalado);
                    break;
                }
            }

            if (!cabemUnicaCaixa)
            {
                embalarEmVarias.Add(pedido);
            }
        }

        return (embalagens, embalarEmVarias);
    }

    private List<Views.Embalagem> EmbalarVariasCaixas(IEnumerable<Pedido> pedidosVariasCaixas,
        IOrderedEnumerable<CaixaModel> caixas)
    {
        var embaladosVariasCaixas = new List<Views.Embalagem>();
        foreach (var pedido in pedidosVariasCaixas)
        {
            var naoEmbalados = new List<Produto>();

            foreach (var caixa in caixas)
            {
                var embalagem = caixa.EmbalarParcialmente(pedido);
                embaladosVariasCaixas.AddRange(embalagem.embalagens);
                if (!embalagem.incabiveis.Any())
                {
                    break;
                }

                naoEmbalados.AddRange(embalagem.incabiveis);
            }
        }

        return embaladosVariasCaixas;
    }

    private IEnumerable<Views.Embalagem> PrepararNaoEmbalaveis(
        IEnumerable<(int PedidoId, Produto Produto)> naoEmbalaveis)
    {
        var naoEmbalaveisRetorno = naoEmbalaveis.Select(ne => new Views.Embalagem
        {
            PedidoId = ne.PedidoId,
            Caixas = new List<CaixaProdutos>
            {
                new()
                {
                    CaixaId = null,
                    Produtos = new List<string>
                    {
                        ne.Produto.ProdutoId
                    },
                    Observacao = "Produto não cabe em nenhuma caixa disponível."
                }
            }
        });
        return naoEmbalaveisRetorno;
    }
}