using Aja.HelperApp.Api.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Data.Queries;

public static class TopicoQuery
{
    public static IQueryable<TopicoResponse> Criar(AjaDbContext database, long? id = null, long? usuarioId = null)
    {
        return
            from topico in database.Topicos.AsNoTracking()
            join usuario in database.Usuarios.AsNoTracking() on topico.UsuarioId equals usuario.Id
            where !id.HasValue || topico.Id == id.Value
            where !usuarioId.HasValue || topico.UsuarioId == usuarioId.Value
            orderby topico.Id descending
            select new TopicoResponse(
                topico.Id,
                topico.Assuntos,
                topico.PostagemId,
                topico.Titulo,
                topico.Texto,
                new UsuarioResponse(usuario.Id, usuario.Nome, usuario.Biografia),
                topico.Interesses
            );
    }
}
