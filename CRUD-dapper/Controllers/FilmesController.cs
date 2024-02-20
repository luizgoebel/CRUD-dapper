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
        return Ok(await _filmeRepository.BuscaFilmesAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(_filmeRepository.BuscaFilmeAsync(id));
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpPost]
    public async Task<IActionResult> Post(FilmeRequest request)
    {
        try
        {
            return Ok(await _filmeRepository.AdicionarAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(FilmeRequest request, int id)
    {
        try
        {
            FilmeResponse filme = await _filmeRepository.BuscaFilmeAsync(id);
            request.Atualizar(filme);

            return Ok(await _filmeRepository.AtualizarAsync(request, id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            FilmeResponse filme = await _filmeRepository.BuscaFilmeAsync(id);

            return Ok(await _filmeRepository.DeletarFilmeAsync(id));
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
}