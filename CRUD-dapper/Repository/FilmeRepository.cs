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
    public Task<bool> AdicionarAsync(FilmeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AtualizarAsync(FilmeRequest request, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<FilmeResponse> BuscaFilmeAsync(int id)
    {
        string sql = @"
    SELECT f.id Id,
       f.nome Nome,
       f.ano Ano,
       p.nome Produtora
    FROM dbo.Filmes f
    JOIN dbo.Produtora p on f.ProdutoraId = @Id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<FilmeResponse>(sql, new { Id = id });
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
