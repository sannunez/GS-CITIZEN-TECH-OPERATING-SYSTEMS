using System.ComponentModel.DataAnnotations;

namespace Aja.HelperApp.Api.Contracts;

public sealed class UsuarioRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nome { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string Biografia { get; init; } = string.Empty;
}

public sealed record UsuarioResponse(long Id, string Nome, string Biografia);

public sealed class BiografiaRequest
{
    [Required(AllowEmptyStrings = true), StringLength(1000)]
    public string Biografia { get; init; } = string.Empty;
}
