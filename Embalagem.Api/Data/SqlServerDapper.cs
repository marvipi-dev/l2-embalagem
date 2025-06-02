using System.Data;
using Dapper;
using Embalagem.Api.Models;
using Embalagem.Api.Views;
using Microsoft.Data.SqlClient;

namespace Embalagem.Api.Data;

public class SqlServerDapper : IRepository
{
    private const string SelectCaixas = @"SELECT caixa_id AS CaixaId, altura, largura, comprimento FROM Caixa";

    private const string InsertEmbalagens = @"INSERT INTO [PedidoEmbalado] 
                                                     (pedido_id, caixa_id, produto_id)
                                              VALUES (@pedido_id, @caixa_id, @produto_id)";

    private const string SelectEmbalagens = @"SELECT * FROM [PedidoEmbalado]";

    private const string ExisteUsuario = @"SELECT email FROM Usuario WHERE email = @Email";
    private const string RegistrarUsuarioProcedure = @"dbo.RegistrarUsuario";
    private const string ValidarUsuario = @"SELECT dbo.ValidarUsuario(@Email, @Senha);";


    private readonly string _connectionString;

    public SqlServerDapper(IConfiguration config)
    {
        _connectionString = config["ConnectionStrings:Default"];
    }

    public async Task<IEnumerable<CaixaModel>?> LerCaixasAsync()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var caixas = await connection.QueryAsync(SelectCaixas);
                return caixas.Select(c => new CaixaModel()
                {
                    CaixaId = c.CaixaId,
                    Dimensoes = new()
                    {
                        Altura = c.altura,
                        Largura = c.largura,
                        Comprimento = c.comprimento
                    }
                });
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> EscreverAsync(IEnumerable<Views.Embalagem> embalagens)
    {
        try
        {
            var linhasEscritas = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var embalagem in embalagens)
                foreach (var caixa in embalagem.Caixas)
                foreach (var produto in caixa.Produtos)
                {
                    linhasEscritas += await connection.ExecuteAsync(InsertEmbalagens,
                        new
                        {
                            pedido_id = embalagem.PedidoId,
                            caixa_id = caixa.CaixaId,
                            produto_id = produto
                        });
                }
            }

            return linhasEscritas > 0;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<RegistroEmbalagem>?> LerEmbalagensAsync()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var embalagens = await connection.QueryAsync(SelectEmbalagens);
                return embalagens.Select(e => new RegistroEmbalagem
                {
                    PedidoId = e.pedido_id,
                    CaixaId = e.caixa_id,
                    ProdutoId = e.produto_id
                });
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> ExisteAsync(Usuario usuario)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var resposta = await connection.QueryFirstOrDefaultAsync<Usuario>(ExisteUsuario, usuario);
                return resposta != null;
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> EscreverAsync(Usuario usuario)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sucesso = await connection.ExecuteAsync(RegistrarUsuarioProcedure, usuario,
                    commandType: CommandType.StoredProcedure);
                return sucesso == 1;
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> ValidarAsync(Usuario usuario)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sucesso = await connection.ExecuteScalarAsync<int>(ValidarUsuario, usuario);
                return sucesso == 1;
            }
        }
        catch
        {
            return null;
        }
    }
}