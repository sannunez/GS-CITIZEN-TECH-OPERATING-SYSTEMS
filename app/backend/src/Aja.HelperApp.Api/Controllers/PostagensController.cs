using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Data;
using Aja.HelperApp.Api.Data.Queries;
using System.ComponentModel.DataAnnotations;
using Aja.HelperApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Aja.HelperApp.Api.Controllers;

[ApiController]
[Route("api/postagens")]
public sealed class PostagensController(AjaDbContext database) : ControllerBase
{
    [HttpGet("minhas")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<FeedPostagemResponse>>> ListarMinhas(CancellationToken cancellationToken)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioId))
        {
            return Unauthorized();
        }

        var postagens = await PostagemFeedQuery.Criar(database, null, usuarioId).ToListAsync(cancellationToken);

        return Ok(postagens);
    }

    [HttpGet("feed")]
    public async Task<ActionResult<PostagensFeedResponse>> Feed(
        [FromQuery, Range(1, 3)] int limite = 3,
        [FromQuery, Range(1, long.MaxValue)] long? antesDeId = null,
        CancellationToken cancellationToken = default)
    {
        var resultados = await PostagemFeedQuery.Criar(database, antesDeId).Take(limite + 1).ToListAsync(cancellationToken);
        var itens = resultados.Take(limite).ToArray();
        var temMais = resultados.Count > limite;

        return Ok(new PostagensFeedResponse(itens, temMais, temMais ? itens[^1].Id : null));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostagemResponse>>> Listar(CancellationToken cancellationToken)
    {
        var postagens = await database.Postagens.AsNoTracking()
            .Include(postagem => postagem.Usuario)
            .Include(postagem => postagem.Topicos)
            .ThenInclude(topico => topico.Usuario)
            .OrderByDescending(postagem => postagem.CriadaEm)
            .ThenByDescending(postagem => postagem.Id)
            .ToListAsync(cancellationToken);

        return Ok(postagens.Select(postagem => postagem.ToResponse()).ToArray());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostagemResponse>> Obter(long id, CancellationToken cancellationToken)
    {
        var postagem = await database.Postagens.AsNoTracking()
            .Include(postagem => postagem.Usuario)
            .Include(postagem => postagem.Topicos)
            .ThenInclude(topico => topico.Usuario)
            .SingleOrDefaultAsync(postagem => postagem.Id == id, cancellationToken);

        return postagem is null ? NotFound() : Ok(postagem.ToResponse());
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PostagemResponse>> Criar(PostagemRequest request, CancellationToken cancellationToken)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioId))
        {
            return Unauthorized();
        }

        if (request.UsuarioId != usuarioId)
        {
            return Forbid();
        }

        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == usuarioId, cancellationToken);

        if (usuario is null)
        {
            ModelState.AddModelError(nameof(request.UsuarioId), "O usuário autor informado não existe.");

            return ValidationProblem(ModelState);
        }

        var postagem = new Postagem
        {
            Texto = NormalizarTexto(request.Texto),
            ImagemUrl = request.NormalizarImagens(),
            UsuarioId = usuario.Id,
            Usuario = usuario,
            CriadaEm = DateTime.UtcNow
        };

        database.Postagens.Add(postagem);
        await database.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Obter), new { id = postagem.Id }, postagem.ToResponse());
    }

    [HttpPut("{id:long}")]
    [Authorize]
    public async Task<ActionResult<PostagemResponse>> Atualizar(long id, PostagemRequest request, CancellationToken cancellationToken)
    {
        var postagem = await database.Postagens.Include(postagem => postagem.Topicos)
            .ThenInclude(topico => topico.Usuario)
            .SingleOrDefaultAsync(postagem => postagem.Id == id, cancellationToken);

        if (postagem is null)
        {
            return NotFound();
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != postagem.UsuarioId.ToString()
            || request.UsuarioId != postagem.UsuarioId)
        {
            return Forbid();
        }

        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == request.UsuarioId, cancellationToken);

        if (usuario is null)
        {
            ModelState.AddModelError(nameof(request.UsuarioId), "O usuário autor informado não existe.");

            return ValidationProblem(ModelState);
        }

        postagem.Texto = NormalizarTexto(request.Texto);
        postagem.ImagemUrl = request.NormalizarImagens();
        postagem.UsuarioId = usuario.Id;
        postagem.Usuario = usuario;
        await database.SaveChangesAsync(cancellationToken);

        return Ok(postagem.ToResponse());
    }

    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> Excluir(long id, CancellationToken cancellationToken)
    {
        var postagem = await database.Postagens.SingleOrDefaultAsync(postagem => postagem.Id == id, cancellationToken);

        if (postagem is null)
        {
            return NotFound();
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != postagem.UsuarioId.ToString())
        {
            return Forbid();
        }

        database.Postagens.Remove(postagem);
        await database.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static string? NormalizarTexto(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
