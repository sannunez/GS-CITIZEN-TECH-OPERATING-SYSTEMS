using System.ComponentModel.DataAnnotations;

namespace Aja.HelperApp.Api.Contracts;

public sealed class PostagemRequest : IValidatableObject
{
    public string? Texto { get; init; }

    [StringLength(10000)]
    public string? ImagemUrl { get; init; }

    [Range(1, long.MaxValue)]
    public long UsuarioId { get; init; }

    public string[] SepararImagens() => (ImagemUrl ?? "").Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    public string? NormalizarImagens()
    {
        var imagens = SepararImagens();

        return imagens.Length > 0 ? string.Join(';', imagens) : null;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var imagens = SepararImagens();

        if (string.IsNullOrWhiteSpace(Texto) && imagens.Length == 0)
        {
            yield return new ValidationResult("Informe um texto ou uma URL de imagem.", [nameof(Texto), nameof(ImagemUrl)]);
        }

        if (imagens.Length > 10)
        {
            yield return new ValidationResult("Adicione no máximo dez imagens.", [nameof(ImagemUrl)]);
        }

        foreach (var imagem in imagens)
        {
            if (imagem.Length > 1000
                || !Uri.TryCreate(imagem, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                yield return new ValidationResult("Informe URLs HTTP ou HTTPS válidas, separadas por ponto e vírgula, com até 1000 caracteres cada.", [nameof(ImagemUrl)]);

                break;
            }
        }
    }
}

public sealed record PostagemResponse(
    long Id,
    string? Texto,
    string? ImagemUrl,
    UsuarioResponse Autor,
    IReadOnlyList<TopicoResponse> Topicos,
    DateTime CriadaEm
);
