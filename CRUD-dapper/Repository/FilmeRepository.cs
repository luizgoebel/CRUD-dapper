using CRUD_dapper.Models;
using Dapper;
using System.Data.SqlClient;

namespace CRUD_dapper.Repository;

public class FilmeRepository : IFilmeRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;
    public FilmeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("FilmesContext");
    }
    public async Task<bool> AdicionarAsync(FilmeRequest request)
    {
        try
        {
            string sql = @"INSERT INTO dbo.Filmes (nome, ano, produtoraid) 
                       VALUES (@Nome, @Ano, @ProdutoraId)";
            using var con = new SqlConnection(connectionString);
            return await con.ExecuteAsync(sql, request) > 0;
        }
        catch (SqlException)
        {
            throw new Exception("Não existe essa produtora no banco de dados.");
        }
    }

    public async Task<bool> AtualizarAsync(FilmeRequest request, int id)
    {
        try
        {
            string sql = @"
                            UPDATE dbo.Filmes 
                            SET nome = @Nome, 
                                ano = @Ano
                            WHERE id = @Id";


            DynamicParameters parametros = new();
            parametros.Add("Nome", request.Nome);
            parametros.Add("Ano", request.Ano);
            parametros.Add("Id", id);

            using var con = new SqlConnection(connectionString);
            return await con.ExecuteAsync(sql, parametros) > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<FilmeResponse> BuscaFilmeAsync(int id)
    {
        try
        {
            string sql = @"
    SELECT f.id Id,
       f.nome Nome,
       f.ano Ano,
       p.nome Produtora
    FROM dbo.Filmes f
    JOIN dbo.Produtora p on f.ProdutoraId = p.id
    WHERE f.id = @Id";
            using var con = new SqlConnection(connectionString);
            return await con.QueryFirstOrDefaultAsync<FilmeResponse>(sql, new { Id = id });
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync()
    {
        string sql = @"
    SELECT f.id Id,
       f.nome Nome,
       f.ano Ano,
       p.nome Produtora
    FROM dbo.Filmes f
    JOIN dbo.Produtora p on f.ProdutoraId = p.Id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<FilmeResponse>(sql);
    }

    public Task<bool> DeletarFilmeAsync(int id)
    {
        throw new NotImplementedException();
    }
}
