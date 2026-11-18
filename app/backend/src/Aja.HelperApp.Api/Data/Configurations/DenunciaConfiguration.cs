using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aja.HelperApp.Api.Data.Configurations;

public sealed class DenunciaConfiguration : IEntityTypeConfiguration<Denuncia>
{
    public void Configure(EntityTypeBuilder<Denuncia> builder)
    {
        builder.ToTable("den_denuncias");
        builder.HasKey(d => d.Id).HasName("pk_den_denuncias");

        builder.Property(d => d.Id)
            .HasColumnName("den_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Texto)
            .HasColumnName("den_texto")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.Cep)
            .HasColumnName("den_cep")
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(d => d.Logradouro)
            .HasColumnName("den_logradouro")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.Numero)
            .HasColumnName("den_numero")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.Complemento)
            .HasColumnName("den_complemento")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.Bairro)
            .HasColumnName("den_bairro")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Cidade)
            .HasColumnName("den_cidade")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Uf)
            .HasColumnName("den_uf")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(d => d.PontoReferencia)
            .HasColumnName("den_ponto_referencia")
            .HasColumnType("varchar(200)");

        builder.Property(d => d.ImagensUrls)
            .HasColumnName("den_imagens_urls")
            .HasColumnType("text");

        builder.Property(d => d.Solucionado)
            .HasColumnName("den_solucionado")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(d => d.CriadaEm)
            .HasColumnName("den_criada_em")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(d => d.UsuarioId)
            .HasColumnName("den_usuario_id")
            .HasColumnType("bigint");

        builder.HasIndex(d => new { d.CriadaEm, d.Id }).HasDatabaseName("ix_den_data_id");

        builder.HasOne(d => d.Usuario)
            .WithMany(u => u.Denuncias)
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_den_usuarios");
    }
}
