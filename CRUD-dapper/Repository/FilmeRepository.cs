﻿using CRUD_dapper.Models;
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
    public async Task<string> AdicionarAsync(FilmeRequest request)
    {
        try
        {
            if (!request.EhValido())
                throw new Exception("Informações inválidas");

            string sql = @"INSERT INTO dbo.Filmes (nome, ano, produtoraid) 
                       VALUES (@Nome, @Ano, @ProdutoraId)";
            using var con = new SqlConnection(connectionString);

            return await con.ExecuteAsync(sql, request) <= 0 ?
                throw new Exception("Não foi possível adicionar o filme") : "Adicionado com sucesso";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> AtualizarAsync(FilmeRequest request, int id)
    {
        try
        {
            FilmeResponse filme = await BuscaFilmeAsync(id);
            request.Atualizar(filme);

            string sql = @"
                            UPDATE dbo.Filmes 
                            SET nome = @Nome, 
                                ano = @Ano
                            WHERE id = @Id";
            using var con = new SqlConnection(connectionString);

            DynamicParameters parametros = new();
            parametros.Add("Nome", request.Nome);
            parametros.Add("Ano", request.Ano);
            parametros.Add("Id", id);

            return await con.ExecuteAsync(sql, parametros) <= 0 ?
                throw new Exception("Não foi possível atualizar o filme") : "Atualizado com sucesso";
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
            if (id <= 0) throw new Exception("Filme inválido.");

            string sql = @"
                            SELECT f.id Id,
                               f.nome Nome,
                               f.ano Ano,
                               p.nome Produtora
                            FROM dbo.Filmes f
                            JOIN dbo.Produtora p on f.ProdutoraId = p.id
                            WHERE f.id = @Id";
            using var con = new SqlConnection(connectionString);

            return await con.QueryFirstOrDefaultAsync<FilmeResponse>(sql, new { Id = id }) ?? throw new Exception("Filme não encontrado.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync()
    {
        try
        {
            string sql = @"
                            SELECT f.id Id,
                               f.nome Nome,
                               f.ano Ano,
                               p.nome Produtora
                            FROM dbo.Filmes f
                            JOIN dbo.Produtora p on f.ProdutoraId = p.Id";
            using var con = new SqlConnection(connectionString);

            return await con.QueryAsync<FilmeResponse>(sql) ??
                Enumerable.Empty<FilmeResponse>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> DeletarFilmeAsync(int id)
    {
        try
        {
            if (id <= 0) throw new Exception("Filme inválido.");
            FilmeResponse filme = await BuscaFilmeAsync(id);

            string sql = @"DELETE FROM dbo.Filmes WHERE id = @Id";
            using var con = new SqlConnection(connectionString);

            return await con.ExecuteAsync(sql, new { Id = filme.Id }) <= 0 ?
                throw new Exception("Erro ao deletar filme.") : "deletado com sucesso";
        }
        catch (Exception)
        {
            throw;
        }
    }
}
