namespace Aja.HelperApp.Api.Models;

public sealed class Usuario
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public string Biografia { get; set; } = string.Empty;

    public ICollection<Postagem> Postagens { get; set; } = new List<Postagem>();

    public ICollection<Topico> Topicos { get; set; } = new List<Topico>();

    public ICollection<Denuncia> Denuncias { get; set; } = new List<Denuncia>();
}
