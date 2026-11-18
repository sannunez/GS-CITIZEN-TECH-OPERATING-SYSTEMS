using Aja.HelperApp.Api.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Data.Queries;

public static class PostagemFeedQuery
{
    public static IQueryable<FeedPostagemResponse> Criar(AjaDbContext database, long? antesDeId, long? usuarioId = null)
    {
        return
            from postagem in database.Postagens.AsNoTracking()
            join usuario in database.Usuarios.AsNoTracking() on postagem.UsuarioId equals usuario.Id
            where !usuarioId.HasValue || postagem.UsuarioId == usuarioId.Value
            where !antesDeId.HasValue || database.Postagens.Any(cursor =>
                cursor.Id == antesDeId.Value &&
                (postagem.CriadaEm < cursor.CriadaEm ||
                    (postagem.CriadaEm == cursor.CriadaEm && postagem.Id < cursor.Id)))
            orderby postagem.CriadaEm descending, postagem.Id descending
            select new FeedPostagemResponse(
                postagem.Id,
                postagem.Texto,
                postagem.ImagemUrl,
                new UsuarioResponse(usuario.Id, usuario.Nome, usuario.Biografia),
                postagem.CriadaEm);
    }
}
