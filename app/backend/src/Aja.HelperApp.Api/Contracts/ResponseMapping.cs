using Aja.HelperApp.Api.Models;

namespace Aja.HelperApp.Api.Contracts;

public static class ResponseMapping
{
    public static UsuarioResponse ToResponse(this Usuario usuario) => new(usuario.Id, usuario.Nome, usuario.Biografia);

    public static TopicoResponse ToResponse(this Topico topico) => new(
        topico.Id,
        topico.Assuntos,
        topico.PostagemId,
        topico.Titulo,
        topico.Texto,
        topico.Usuario.ToResponse(),
        topico.Interesses
    );

    public static PostagemResponse ToResponse(this Postagem postagem) => new(
        postagem.Id,
        postagem.Texto,
        postagem.ImagemUrl,
        postagem.Usuario.ToResponse(),
        postagem.Topicos.OrderBy(topico => topico.Id).Select(topico => topico.ToResponse()).ToArray(),
        postagem.CriadaEm
    );
}
