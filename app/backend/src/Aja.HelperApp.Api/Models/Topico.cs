namespace Aja.HelperApp.Api.Models;

public sealed class Topico
{
    public long Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Texto { get; set; } = string.Empty;

    public int Interesses { get; set; }

    public long UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public string[] Assuntos { get; set; } = [];

    public long? PostagemId { get; set; }

    public Postagem? Postagem { get; set; }
}
