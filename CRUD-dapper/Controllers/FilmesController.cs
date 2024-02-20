using CRUD_dapper.Models;
using CRUD_dapper.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_dapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmesController : ControllerBase
{
    private readonly IFilmeRepository _filmeRepository;

    public FilmesController(IFilmeRepository filmeRepository)
    {
        _filmeRepository = filmeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<FilmeResponse> filmes = await _filmeRepository.BuscaFilmesAsync();
        return Ok(filmes.Any() ? filmes : NoContent());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        FilmeResponse filme = await _filmeRepository.BuscaFilmeAsync(id);
        return Ok(filme is not null ?
            Ok(filme) :
            NotFound("Filme não encontrado."));
    }
    [HttpPost]
    public async Task<IActionResult> Post(FilmeRequest request)
    {
        try
        {
            if (!request.EhValido()) return BadRequest("Informações inválidas");

            return await _filmeRepository.AdicionarAsync(request) ?
                Ok("Filme adicionado com sucesso.") :
                BadRequest("Erro ao adicionar filme.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}