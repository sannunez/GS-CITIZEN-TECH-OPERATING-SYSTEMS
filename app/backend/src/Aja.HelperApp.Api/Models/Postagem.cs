namespace Aja.HelperApp.Api.Models;

public sealed class Postagem
{
    public long Id { get; set; }

    public DateTime CriadaEm { get; set; }

    public string? Texto { get; set; }

    public string? ImagemUrl { get; set; }

    public long UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public ICollection<Topico> Topicos { get; set; } = new List<Topico>();
}
