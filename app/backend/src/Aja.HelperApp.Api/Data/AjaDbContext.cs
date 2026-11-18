using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Data;

public sealed class AjaDbContext(DbContextOptions<AjaDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Postagem> Postagens => Set<Postagem>();

    public DbSet<Topico> Topicos => Set<Topico>();

    public DbSet<Denuncia> Denuncias => Set<Denuncia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AjaDbContext).Assembly);
        DemonstrationData.Configure(modelBuilder);
    }
}
