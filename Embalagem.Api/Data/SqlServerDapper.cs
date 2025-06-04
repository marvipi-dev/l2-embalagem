using System.Data;
using Dapper;
using Embalagem.Api.Models;
using Microsoft.Data.SqlClient;

namespace Embalagem.Api.Data;

public class SqlServerDapper : IRepository
{
    private const string SelectCaixas = @"SELECT caixa_id AS CaixaId, altura, largura, comprimento FROM Caixa";

    private const string InsertEmbalagens = @"INSERT INTO [PedidoEmbalado] 
                                                     (pedido_id, caixa_id, produto_id)
                                              VALUES (@PedidoId, @CaixaId, @ProdutoId)";

    private const string SelectEmbalagens = @"SELECT * FROM [PedidoEmbalado]";

    private const string ExisteUsuario = @"SELECT email FROM Usuario WHERE email = @Email";
    private const string RegistrarUsuarioProcedure = @"dbo.RegistrarUsuario";
    private const string ValidarUsuario = @"SELECT dbo.ValidarUsuario(@Email, @Senha);";


    private readonly string _connectionString;

    public SqlServerDapper(IConfiguration config)
    {
        _connectionString = config["ConnectionStrings:Default"]!;
    }

    public async Task<IEnumerable<Caixa>?> LerCaixasAsync()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Caixa>(SelectCaixas);
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> EscreverAsync(IEnumerable<EmbalagemRegistro> embalagens)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var linhasEscritas = await connection.ExecuteAsync(InsertEmbalagens, embalagens);
                return linhasEscritas > 0;
            }
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<EmbalagemRegistro>?> LerEmbalagensAsync()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var embalagens = await connection.QueryAsync(SelectEmbalagens);
                return embalagens.Select(e => new EmbalagemRegistro
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