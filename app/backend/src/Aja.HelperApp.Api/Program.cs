using Aja.HelperApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Aja.HelperApp.Api.Contracts;
using Aja.HelperApp.Api.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection antes de iniciar a API.");
var serverVersion = new MySqlServerVersion(Version.Parse(builder.Configuration["Database:ServerVersion"] ?? "8.0.41"));

builder.Services.AddDbContext<AjaDbContext>(options =>
    options.UseMySql(connectionString, serverVersion, mysql => mysql.EnableRetryOnFailure(3)));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
    });

builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken(options => options.BearerTokenExpiration = TimeSpan.FromDays(7));

builder.Services.AddAuthorization();

var dataProtection = builder.Services.AddDataProtection().SetApplicationName("Aja.HelperApp");
var keyDirectory = builder.Configuration["DataProtection:KeysDirectory"];

if (!string.IsNullOrWhiteSpace(keyDirectory))
{
    dataProtection.PersistKeysToFileSystem(new DirectoryInfo(keyDirectory));
}

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy("MobileApp", policy =>
{
    var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    if (origins.Length > 0)
    {
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    }
}));

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("MobileApp");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var database = scope.ServiceProvider.GetRequiredService<AjaDbContext>();

    await database.Database.MigrateAsync();
}

app.MapGet("/health", async (AjaDbContext database, CancellationToken cancellationToken) =>
{
    var conectado = await database.Database.CanConnectAsync(cancellationToken);

    if (!conectado)
    {
        return Results.Problem("Banco de dados indisponível.", statusCode: 503);
    }

    return Results.Ok(new { status = "ok", database = "MySQL" });
});

app.MapControllers();
app.Run();

public partial class Program
{
}
