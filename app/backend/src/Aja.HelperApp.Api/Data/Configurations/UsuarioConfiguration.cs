using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aja.HelperApp.Api.Data.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usu_usuarios");
        builder.HasKey(usuario => usuario.Id).HasName("pk_usu_usuarios");

        builder.Property(usuario => usuario.Id)
            .HasColumnName("usu_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(usuario => usuario.Nome)
            .HasColumnName("usu_nome")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(usuario => usuario.Email)
            .HasColumnName("usu_email")
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(usuario => usuario.SenhaHash)
            .HasColumnName("usu_senha_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Email).IsUnique().HasDatabaseName("ux_usu_email");

        builder.Property(usuario => usuario.Biografia)
            .HasColumnName("usu_biografia")
            .HasColumnType("longtext")
            .IsRequired();
    }
}
