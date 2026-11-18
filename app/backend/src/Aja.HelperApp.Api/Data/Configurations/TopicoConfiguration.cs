using Aja.HelperApp.Api.Models;
using Aja.HelperApp.Api.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aja.HelperApp.Api.Data.Configurations;

public sealed class TopicoConfiguration : IEntityTypeConfiguration<Topico>
{
    public void Configure(EntityTypeBuilder<Topico> builder)
    {
        builder.ToTable("top_topicos");
        builder.HasKey(topico => topico.Id).HasName("pk_top_topicos");

        builder.Property(topico => topico.Id)
            .HasColumnName("top_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(topico => topico.Titulo)
            .HasColumnName("top_titulo")
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(topico => topico.Texto)
            .HasColumnName("top_texto")
            .HasColumnType("longtext")
            .IsRequired();

        builder.Property(topico => topico.Interesses)
            .HasColumnName("top_interesses")
            .IsRequired();

        builder.Property(topico => topico.UsuarioId)
            .HasColumnName("top_usuario_id")
            .HasColumnType("bigint");

        builder.HasOne(topico => topico.Usuario)
            .WithMany(usuario => usuario.Topicos)
            .HasForeignKey(topico => topico.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_top_usuarios");

        builder.Property(topico => topico.Assuntos)
            .HasColumnName("top_assuntos")
            .HasConversion(new StringArrayJsonConverter())
            .HasColumnType("json")
            .IsRequired().Metadata.SetValueComparer(StringArrayJsonConverter.Comparer);

        builder.Property(topico => topico.PostagemId)
            .HasColumnName("top_postagem_id")
            .HasColumnType("bigint");

        builder.HasIndex(topico => topico.PostagemId).HasDatabaseName("ix_top_postagem_id");

        builder.HasOne(topico => topico.Postagem)
            .WithMany(postagem => postagem.Topicos)
            .HasForeignKey(topico => topico.PostagemId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_top_postagens");
    }
}
