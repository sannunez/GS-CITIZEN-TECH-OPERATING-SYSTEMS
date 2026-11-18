using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aja.HelperApp.Api.Data.Configurations;

public sealed class PostagemConfiguration : IEntityTypeConfiguration<Postagem>
{
    public void Configure(EntityTypeBuilder<Postagem> builder)
    {
        builder.ToTable("pos_postagens");
        builder.HasKey(postagem => postagem.Id).HasName("pk_pos_postagens");

        builder.Property(postagem => postagem.Id)
            .HasColumnName("pos_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(postagem => postagem.CriadaEm)
            .HasColumnName("pos_criada_em")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasIndex(postagem => new { postagem.CriadaEm, postagem.Id }).HasDatabaseName("ix_pos_data_id");

        builder.Property(postagem => postagem.Texto)
            .HasColumnName("pos_texto")
            .HasColumnType("longtext");

        builder.Property(postagem => postagem.ImagemUrl)
            .HasColumnName("pos_imagem_url")
            .HasColumnType("longtext");

        builder.Property(postagem => postagem.UsuarioId)
            .HasColumnName("pos_usuario_id")
            .HasColumnType("bigint");

        builder.HasIndex(postagem => postagem.UsuarioId).HasDatabaseName("ix_pos_usuario_id");

        builder.HasOne(postagem => postagem.Usuario)
            .WithMany(usuario => usuario.Postagens)
            .HasForeignKey(postagem => postagem.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_pos_usuarios");
    }
}
