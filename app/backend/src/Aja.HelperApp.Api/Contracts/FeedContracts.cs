namespace Aja.HelperApp.Api.Contracts;

public sealed record FeedPostagemResponse(
    long Id,
    string? Texto,
    string? ImagemUrl,
    UsuarioResponse Autor,
    DateTime CriadaEm
);
public sealed record PostagensFeedResponse(IReadOnlyList<FeedPostagemResponse> Itens, bool TemMais, long? ProximoCursor);
