using System.ComponentModel.DataAnnotations;

namespace Aja.HelperApp.Api.Contracts;

public sealed class TopicoRequest : IValidatableObject
{
    [StringLength(180)]
    public string Titulo { get; init; } = string.Empty;

    public string Texto { get; init; } = string.Empty;

    [Required]
    public string[] Assuntos { get; init; } = [];

    [Range(1, long.MaxValue)]
    public long? PostagemId { get; init; }

    public string[] NormalizarAssuntos() => (Assuntos ?? []).SelectMany(assunto => (assunto ?? string.Empty).Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NormalizarAssuntos().Length == 0 && string.IsNullOrWhiteSpace(Texto))
        {
            yield return new ValidationResult("Informe pelo menos um assunto válido.", [nameof(Assuntos)]);
        }
    }
}

public sealed record TopicoResponse(
    long Id,
    string[] Assuntos,
    long? PostagemId,
    string Titulo,
    string Texto,
    UsuarioResponse Autor,
    int Interesses
);
public sealed record InteresseTopicoResponse(long Id, int Interesses);
