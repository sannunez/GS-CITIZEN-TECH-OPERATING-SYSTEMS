using System.ComponentModel.DataAnnotations;

namespace Aja.HelperApp.Api.Contracts;

public sealed class CadastroRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Nome { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 8)]
    public string Senha { get; init; } = string.Empty;
}

public sealed class LoginRequest
{
    [Required, EmailAddress, StringLength(254)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(128)]
    public string Senha { get; init; } = string.Empty;
}

public sealed record SessaoUsuarioResponse(long Id, string Nome, string Email, string Biografia);
