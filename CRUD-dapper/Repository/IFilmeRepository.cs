using CRUD_dapper.Models;

namespace CRUD_dapper.Repository;

public interface IFilmeRepository
{
    Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync();
    Task<FilmeResponse> BuscaFilmeAsync(int id);
    Task<bool> AdicionarAsync(FilmeRequest request);
    Task<bool> AtualizarAsync(FilmeRequest request, int id);
    Task<bool> DeletarFilmeAsync(int id);
}
