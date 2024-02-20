using CRUD_dapper.Models;

namespace CRUD_dapper.Repository;

public interface IFilmeRepository
{
    Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync();
    Task<FilmeResponse> BuscaFilmeAsync(int id);
    Task<string> AdicionarAsync(FilmeRequest request);
    Task<string> AtualizarAsync(FilmeRequest request, int id);
    Task<string> DeletarFilmeAsync(int id);
}
