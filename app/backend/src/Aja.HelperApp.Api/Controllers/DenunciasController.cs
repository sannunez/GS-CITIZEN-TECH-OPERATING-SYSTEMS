using System.Security.Claims;
using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Data;
using Aja.HelperApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Controllers;

[ApiController, Authorize, Route("api/denuncias")]
public sealed class DenunciasController(AjaDbContext database) : ControllerBase
{
    private long AutorId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DenunciaResponse>>> Listar(CancellationToken cancellationToken)
    {
        var itens = await database.Denuncias.AsNoTracking()
            .Include(d => d.Usuario)
            .OrderByDescending(d => d.CriadaEm)
            .ThenByDescending(d => d.Id)
            .ToListAsync(cancellationToken);

        return Ok(itens.Select(DenunciaResponse.De).ToArray());
    }

    [HttpGet("minhas")]
    public async Task<ActionResult<IReadOnlyList<DenunciaResponse>>> Minhas(CancellationToken cancellationToken)
    {
        var autorId = AutorId;
        var itens = await database.Denuncias.AsNoTracking()
            .Include(d => d.Usuario)
            .Where(d => d.UsuarioId == autorId)
            .OrderByDescending(d => d.CriadaEm)
            .ThenByDescending(d => d.Id)
            .ToListAsync(cancellationToken);

        return Ok(itens.Select(DenunciaResponse.De).ToArray());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DenunciaResponse>> Obter(long id, CancellationToken cancellationToken)
    {
        var item = await database.Denuncias.AsNoTracking()
            .Include(d => d.Usuario)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);

        return item is null ? NotFound() : Ok(DenunciaResponse.De(item));
    }

    [HttpPost]
    public async Task<ActionResult<DenunciaResponse>> Criar(DenunciaRequest request, CancellationToken cancellationToken)
    {
        var autor = await database.Usuarios.FindAsync([AutorId], cancellationToken);

        if (autor is null)
        {
            return Unauthorized();
        }

        var item = new Denuncia
        {
            UsuarioId = autor.Id,
            Usuario = autor,
            CriadaEm = DateTime.UtcNow,
            Solucionado = false
        };

        Preencher(item, request);
        database.Denuncias.Add(item);
        await database.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(Obter), new { id = item.Id }, DenunciaResponse.De(item));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<DenunciaResponse>> Atualizar(long id, DenunciaRequest request, CancellationToken cancellationToken)
    {
        var item = await database.Denuncias.Include(d => d.Usuario).SingleOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        if (item.UsuarioId != AutorId)
        {
            return Forbid();
        }

        Preencher(item, request);
        await database.SaveChangesAsync(cancellationToken);

        return Ok(DenunciaResponse.De(item));
    }

    [HttpPatch("{id:long}/situacao")]
    public async Task<ActionResult<DenunciaResponse>> Situacao(long id, SituacaoDenunciaRequest request, CancellationToken cancellationToken)
    {
        var item = await database.Denuncias.Include(d => d.Usuario).SingleOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        if (item.UsuarioId != AutorId)
        {
            return Forbid();
        }

        item.Solucionado = request.Solucionado!.Value;
        await database.SaveChangesAsync(cancellationToken);

        return Ok(DenunciaResponse.De(item));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Excluir(long id, CancellationToken cancellationToken)
    {
        var item = await database.Denuncias.FindAsync([id], cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        if (item.UsuarioId != AutorId)
        {
            return Forbid();
        }

        database.Denuncias.Remove(item);
        await database.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static void Preencher(Denuncia item, DenunciaRequest request)
    {
        var imagens = request.SepararImagens();

        item.Texto = request.Texto.Trim();
        item.Cep = request.Cep;
        item.Logradouro = request.Logradouro?.Trim() ?? "";
        item.Numero = request.Numero.Trim();
        item.Complemento = request.Complemento?.Trim() ?? "";
        item.Bairro = request.Bairro?.Trim() ?? "";
        item.Cidade = request.Cidade.Trim();
        item.Uf = request.Uf;
        item.PontoReferencia = string.IsNullOrWhiteSpace(request.PontoReferencia) ? null : request.PontoReferencia.Trim();
        item.ImagensUrls = imagens.Length == 0 ? null : string.Join(';', imagens);
    }
}
