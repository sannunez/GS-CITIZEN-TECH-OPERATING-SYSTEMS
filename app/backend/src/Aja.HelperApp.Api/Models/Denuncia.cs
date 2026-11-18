namespace Aja.HelperApp.Api.Models;

public sealed class Denuncia
{
    public long Id { get; set; }

    public string Texto { get; set; } = string.Empty;

    public string Cep { get; set; } = string.Empty;

    public string Logradouro { get; set; } = string.Empty;

    public string Numero { get; set; } = string.Empty;

    public string Complemento { get; set; } = string.Empty;

    public string Bairro { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public string? PontoReferencia { get; set; }

    public string? ImagensUrls { get; set; }

    public bool Solucionado { get; set; }

    public DateTime CriadaEm { get; set; }

    public long UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
