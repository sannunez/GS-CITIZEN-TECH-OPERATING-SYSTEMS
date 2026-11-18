using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Aja.HelperApp.Api.Data;

public sealed class AjaDbContextFactory : IDesignTimeDbContextFactory<AjaDbContext>
{
    public AjaDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<AjaDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection.");
        var serverVersion = new MySqlServerVersion(Version.Parse(configuration["Database:ServerVersion"] ?? "8.0.41"));

        return new AjaDbContext(new DbContextOptionsBuilder<AjaDbContext>().UseMySql(connectionString, serverVersion).Options);
    }
}
