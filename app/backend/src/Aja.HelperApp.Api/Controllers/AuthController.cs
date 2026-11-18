using System.Security.Claims;
using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Data;
using Aja.HelperApp.Api.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Aja.HelperApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AjaDbContext database, IPasswordHasher<Usuario> hasher) : ControllerBase
{
    [HttpPost("cadastro")]
    [HttpPost("/api/usuarios")]
    public async Task<IActionResult> Cadastrar(CadastroRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await database.Usuarios.AnyAsync(usuario => usuario.Email == email, cancellationToken))
        {
            return Conflict(new { mensagem = "Este e-mail já está cadastrado." });
        }

        var usuario = new Usuario
        {
            Nome = request.Nome.Trim(),
            Email = email
        };

        usuario.SenhaHash = hasher.HashPassword(usuario, request.Senha);
        database.Usuarios.Add(usuario);

        try
        {
            await database.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return Conflict(new { mensagem = "Este e-mail já está cadastrado." });
        }

        return EmitirToken(usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Entrar(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var usuario = await database.Usuarios.SingleOrDefaultAsync(usuario => usuario.Email == email, cancellationToken);

        if (usuario is null)
        {
            return NotFound(new { mensagem = "Usuário inexistente" });
        }

        if (string.IsNullOrEmpty(usuario.SenhaHash))
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        var resultado = hasher.VerifyHashedPassword(usuario, usuario.SenhaHash, request.Senha);

        if (resultado == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.SenhaHash = hasher.HashPassword(usuario, request.Senha);
            await database.SaveChangesAsync(cancellationToken);
        }

        return EmitirToken(usuario);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<SessaoUsuarioResponse>> ObterSessao(CancellationToken cancellationToken)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
        {
            return Unauthorized();
        }

        var usuario = await database.Usuarios.AsNoTracking().SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

        if (usuario is null)
        {
            return Unauthorized();
        }

        return Ok(new SessaoUsuarioResponse(usuario.Id, usuario.Nome, usuario.Email, usuario.Biografia));
    }

    private Microsoft.AspNetCore.Mvc.SignInResult EmitirToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };
        var identidade = new ClaimsIdentity(claims, BearerTokenDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identidade);

        return SignIn(principal, BearerTokenDefaults.AuthenticationScheme);
    }
}
