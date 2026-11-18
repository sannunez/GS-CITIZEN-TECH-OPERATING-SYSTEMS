using System.ComponentModel.DataAnnotations;
using Aja.HelperApp.Api.Models;

namespace Aja.HelperApp.Api.Contracts;

public sealed class DenunciaRequest : IValidatableObject
{
    [Required(ErrorMessage = "Descreva a denúncia."), StringLength(500, ErrorMessage = "A denúncia deve ter até 500 caracteres.")]
    public string Texto { get; init; } = string.Empty;

    [Required, RegularExpression(@"^\d{8}$", ErrorMessage = "Informe um CEP com oito dígitos.")]
    public string Cep { get; init; } = string.Empty;

    [StringLength(200)]
    public string Logradouro { get; init; } = string.Empty;

    [Required(ErrorMessage = "Informe o número ou S/N."), StringLength(20)]
    public string Numero { get; init; } = string.Empty;

    [StringLength(200)]
    public string Complemento { get; init; } = string.Empty;

    [StringLength(100)]
    public string Bairro { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Cidade { get; init; } = string.Empty;

    [Required, RegularExpression("^(AC|AL|AP|AM|BA|CE|DF|ES|GO|MA|MT|MS|MG|PA|PB|PR|PE|PI|RJ|RN|RS|RO|RR|SC|SP|SE|TO)$")]
    public string Uf { get; init; } = string.Empty;

    [StringLength(200)]
    public string? PontoReferencia { get; init; }

    [StringLength(10000)]
    public string? ImagensUrls { get; init; }

    public string[] SepararImagens() => (ImagensUrls ?? "").Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var imagens = SepararImagens();

        if (imagens.Length > 10)
        {
            yield return new ValidationResult("Adicione no máximo dez imagens.", [nameof(ImagensUrls)]);
        }

        foreach (var imagem in imagens)
        {
            if (imagem.Length > 1000
                || !Uri.TryCreate(imagem, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp
                && uri.Scheme != Uri.UriSchemeHttps))
            {
                yield return new ValidationResult("Informe URLs HTTP ou HTTPS válidas, separadas por ponto e vírgula, com até 1000 caracteres cada.", [nameof(ImagensUrls)]);

                break;
            }
        }
    }
}

public sealed class SituacaoDenunciaRequest
{
    [Required]
    public bool? Solucionado { get; init; }
}

public sealed record DenunciaResponse(
    long Id,
    string Texto,
    string Cep,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Uf,
    string? PontoReferencia,
    string? ImagensUrls,
    bool Solucionado,
    DateTime CriadaEm,
    long UsuarioId,
    string AutorNome
)
{
    public static DenunciaResponse De(Denuncia d) => new(
        d.Id,
        d.Texto,
        d.Cep,
        d.Logradouro,
        d.Numero,
        d.Complemento,
        d.Bairro,
        d.Cidade,
        d.Uf,
        d.PontoReferencia,
        d.ImagensUrls,
        d.Solucionado,
        d.CriadaEm,
        d.UsuarioId,
        d.Usuario.Nome
    );
}
