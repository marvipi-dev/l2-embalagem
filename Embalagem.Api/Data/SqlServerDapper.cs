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

    public IEnumerable<CaixaModel> LerCaixas()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.Query<CaixaModel>(SelectCaixas);
        }
    }

    public bool Escrever(IEnumerable<Views.Embalagem> embalagens)
    {
        var linhasEscritas = 0;
        using (var connection = new SqlConnection(_connectionString))
        {
            foreach (var embalagem in embalagens)
            foreach (var caixa in embalagem.Caixas)
            foreach (var produto in caixa.Produtos)
            {
                linhasEscritas += connection.Execute(InsertEmbalagens,
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

    public IEnumerable<RegistroEmbalagem> LerEmbalagens()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var embalagens = connection.Query(SelectEmbalagens);
            return embalagens.Select(e => new RegistroEmbalagem
            {
                PedidoId = e.pedido_id,
                CaixaId = e.caixa_id,
                ProdutoId = e.produto_id
            });
        }
    }

    public bool Existe(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var resposta = connection.QueryFirstOrDefault<Usuario>(ExisteUsuario, usuario);
            return resposta != null;
        }
    }

    public bool Escrever(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sucesso = connection.Execute(RegistrarUsuarioProcedure, usuario,
                commandType: CommandType.StoredProcedure);
            return sucesso == 1;
        }
    }

    public bool Validar(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sucesso = connection.ExecuteScalar<int>(ValidarUsuario, usuario);
            return sucesso == 1;
        }
    }
}