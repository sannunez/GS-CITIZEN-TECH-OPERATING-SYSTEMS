using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Data;
using Aja.HelperApp.Api.Models;
using Aja.HelperApp.Api.Data.Queries;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Controllers;

[ApiController]
[Route("api/topicos")]
public sealed class TopicosController(AjaDbContext database) : ControllerBase
{
    [HttpPost("{id:long}/interesse")]
    [Authorize]
    public async Task<ActionResult<InteresseTopicoResponse>> DemonstrarInteresse(long id, CancellationToken cancellationToken)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioId)
            || !await database.Usuarios.AnyAsync(usuario => usuario.Id == usuarioId, cancellationToken))
        {
            return Unauthorized();
        }

        var alterados = await database.Topicos.Where(topico => topico.Id == id)
            .ExecuteUpdateAsync(atualizacao => atualizacao.SetProperty(topico => topico.Interesses, topico => topico.Interesses + 1), cancellationToken);

        if (alterados == 0)
        {
            return NotFound();
        }

        var interesses = await database.Topicos.Where(topico => topico.Id == id)
            .Select(topico => topico.Interesses)
            .SingleOrDefaultAsync(cancellationToken);

        return Ok(new InteresseTopicoResponse(id, interesses));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TopicoResponse>>> Listar(CancellationToken cancellationToken)
    {
        var topicos = await TopicoQuery.Criar(database).ToListAsync(cancellationToken);

        return Ok(topicos);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TopicoResponse>> Obter(long id, CancellationToken cancellationToken)
    {
        var topico = await TopicoQuery.Criar(database, id).SingleOrDefaultAsync(cancellationToken);

        if (topico is null)
        {
            return NotFound();
        }

        return Ok(topico);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TopicoResponse>> Criar(TopicoRequest request, CancellationToken cancellationToken)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioId))
        {
            return Unauthorized();
        }

        var usuario = await database.Usuarios.FindAsync([usuarioId], cancellationToken);

        if (usuario is null)
        {
            return Unauthorized();
        }

        if (!await PostagemExiste(request.PostagemId, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.PostagemId), "A postagem informada não existe.");

            return ValidationProblem(ModelState);
        }

        var topico = new Topico
        {
            Titulo = request.Titulo.Trim(),
            Texto = request.Texto.Trim(),
            Assuntos = request.NormalizarAssuntos(),
            PostagemId = request.PostagemId,
            UsuarioId = usuario.Id,
            Usuario = usuario
        };

        database.Topicos.Add(topico);
        await database.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Obter), new { id = topico.Id }, topico.ToResponse());
    }

    [HttpPut("{id:long}")]
    [Authorize]
    public async Task<ActionResult<TopicoResponse>> Atualizar(long id, TopicoRequest request, CancellationToken cancellationToken)
    {
        var topico = await database.Topicos.Include(topico => topico.Usuario)
            .SingleOrDefaultAsync(topico => topico.Id == id, cancellationToken);

        if (topico is null)
        {
            return NotFound();
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != topico.UsuarioId.ToString())
        {
            return Forbid();
        }

        if (!await PostagemExiste(request.PostagemId, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.PostagemId), "A postagem informada não existe.");

            return ValidationProblem(ModelState);
        }

        topico.Assuntos = request.NormalizarAssuntos();
        topico.Titulo = request.Titulo.Trim();
        topico.Texto = request.Texto.Trim();
        topico.PostagemId = request.PostagemId;
        await database.SaveChangesAsync(cancellationToken);

        return Ok(topico.ToResponse());
    }

    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> Excluir(long id, CancellationToken cancellationToken)
    {
        var topico = await database.Topicos.SingleOrDefaultAsync(topico => topico.Id == id, cancellationToken);

        if (topico is null)
        {
            return NotFound();
        }

        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != topico.UsuarioId.ToString())
        {
            return Forbid();
        }

        database.Topicos.Remove(topico);
        await database.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private Task<bool> PostagemExiste(long? id, CancellationToken cancellationToken) =>
        id.HasValue
            ? database.Postagens.AnyAsync(postagem => postagem.Id == id.Value, cancellationToken)
            : Task.FromResult(true);
}
