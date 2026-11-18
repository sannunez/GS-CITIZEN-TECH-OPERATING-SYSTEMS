using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Data;
using Aja.HelperApp.Api.Data.Queries;
using Aja.HelperApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Aja.HelperApp.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(AjaDbContext database) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioResponse>>> Listar(CancellationToken cancellationToken)
    {
        var usuarios = await database.Usuarios.AsNoTracking().OrderBy(usuario => usuario.Id).ToListAsync(cancellationToken);

        return Ok(usuarios.Select(usuario => usuario.ToResponse()).ToArray());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UsuarioResponse>> Obter(long id, CancellationToken cancellationToken)
    {
        var usuario = await database.Usuarios.AsNoTracking().SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

        return usuario is null ? NotFound() : Ok(usuario.ToResponse());
    }

    [HttpGet("{id:long}/postagens")]
    public async Task<ActionResult<PostagensFeedResponse>> Postagens(long id,
        [FromQuery, Range(1, 20)] int limite = 10,
        [FromQuery, Range(1, long.MaxValue)] long? antesDeId = null,
        CancellationToken cancellationToken = default)
    {
        if (!await database.Usuarios.AnyAsync(usuario => usuario.Id == id, cancellationToken))
        {
            return NotFound(new { mensagem = "Usuário não encontrado." });
        }

        var resultados = await PostagemFeedQuery.Criar(database, antesDeId, id)
            .Take(limite + 1)
            .ToListAsync(cancellationToken);
        var itens = resultados.Take(limite).ToArray();
        var temMais = resultados.Count > limite;

        return Ok(new PostagensFeedResponse(itens, temMais, temMais ? itens[^1].Id : null));
    }

    [HttpGet("{id:long}/topicos")]
    public async Task<ActionResult<IReadOnlyList<TopicoResponse>>> Topicos(long id, CancellationToken cancellationToken)
    {
        if (!await database.Usuarios.AnyAsync(usuario => usuario.Id == id, cancellationToken))
        {
            return NotFound(new { mensagem = "Usuário não encontrado." });
        }

        var topicos = await TopicoQuery.Criar(database, usuarioId: id)
            .ToListAsync(cancellationToken);

        return Ok(topicos);
    }

    [HttpPut("{id:long}")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponse>> Atualizar(long id, UsuarioRequest request, CancellationToken cancellationToken)
    {
        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
        {
            return Forbid();
        }

        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

        if (usuario is null)
        {
            return NotFound();
        }

        usuario.Nome = request.Nome.Trim();
        usuario.Biografia = request.Biografia.Trim();
        await database.SaveChangesAsync(cancellationToken);

        return Ok(usuario.ToResponse());
    }

    [HttpPatch("{id:long}/biografia")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponse>> AtualizarBiografia(long id, BiografiaRequest request, CancellationToken cancellationToken)
    {
        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
        {
            return Forbid();
        }

        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

        if (usuario is null) return NotFound();

        usuario.Biografia = request.Biografia.Trim();
        await database.SaveChangesAsync(cancellationToken);

        return Ok(usuario.ToResponse());
    }

    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> Excluir(long id, CancellationToken cancellationToken)
    {
        if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
        {
            return Forbid();
        }

        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

        if (usuario is null)
        {
            return NotFound();
        }

        if (await database.Postagens.AnyAsync(postagem => postagem.UsuarioId == id, cancellationToken)
            || await database.Topicos.AnyAsync(topico => topico.UsuarioId == id, cancellationToken)
            || await database.Denuncias.AnyAsync(denuncia => denuncia.UsuarioId == id, cancellationToken))
        {
            return Problem("O usuário possui postagens, tópicos ou denúncias e não pode ser excluído.", statusCode: StatusCodes.Status409Conflict);
        }

        database.Usuarios.Remove(usuario);
        await database.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
