using Aja.HelperApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Aja.HelperApp.Api.Migrations
{
    [DbContext(typeof(AjaDbContext))]
    [Migration("20261112023833_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Postagem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("pos_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ImagemUrl")
                        .HasColumnType("longtext")
                        .HasColumnName("pos_imagem_url");

                    b.Property<string>("Texto")
                        .HasColumnType("longtext")
                        .HasColumnName("pos_texto");

                    b.Property<long>("UsuarioId")
                        .HasColumnType("bigint")
                        .HasColumnName("pos_usuario_id");

                    b.HasKey("Id")
                        .HasName("pk_pos_postagens");

                    b.HasIndex("UsuarioId")
                        .HasDatabaseName("ix_pos_usuario_id");

                    b.ToTable("pos_postagens", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Texto = "Caso demonstrativo: uma travessia no Parque Doroteia precisa de recursos de orientação para pessoas cegas.",
                            UsuarioId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Texto = "Caso demonstrativo: informações de atendimento em um espaço público do Parque Doroteia poderiam contar com comunicação visual acessível a pessoas surdas.",
                            UsuarioId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            ImagemUrl = "https://placehold.co/1200x800/png?text=Imagem+demonstrativa",
                            Texto = "Caso demonstrativo: obstáculos na calçada dificultam a circulação e a orientação de moradores do Parque Doroteia.",
                            UsuarioId = 3L
                        });
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Topico", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("top_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Assuntos")
                        .IsRequired()
                        .HasColumnType("json")
                        .HasColumnName("top_assuntos");

                    b.Property<long?>("PostagemId")
                        .HasColumnType("bigint")
                        .HasColumnName("top_postagem_id");

                    b.HasKey("Id")
                        .HasName("pk_top_topicos");

                    b.HasIndex("PostagemId")
                        .HasDatabaseName("ix_top_postagem_id");

                    b.ToTable("top_topicos", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Assuntos = "[\"defici\\u00EAncia visual\",\"travessia\",\"orienta\\u00E7\\u00E3o acess\\u00EDvel\"]",
                            PostagemId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Assuntos = "[\"defici\\u00EAncia auditiva\",\"comunica\\u00E7\\u00E3o visual\",\"servi\\u00E7os p\\u00FAblicos\"]",
                            PostagemId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            Assuntos = "[\"cal\\u00E7adas\",\"mobilidade\",\"Parque Doroteia\"]",
                            PostagemId = 3L
                        },
                        new
                        {
                            Id = 4L,
                            Assuntos = "[\"acessibilidade\",\"participa\\u00E7\\u00E3o cidad\\u00E3\",\"S\\u00E3o Paulo\"]"
                        });
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Usuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("usu_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Biografia")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("usu_biografia");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("usu_nome");

                    b.HasKey("Id")
                        .HasName("pk_usu_usuarios");

                    b.ToTable("usu_usuarios", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Biografia = "Perfil fictício de uma moradora cega do Parque Doroteia, Zona Sul de São Paulo.",
                            Nome = "Ana Ribeiro"
                        },
                        new
                        {
                            Id = 2L,
                            Biografia = "Perfil fictício de um morador surdo interessado em comunicação acessível no Parque Doroteia.",
                            Nome = "Bruno Santos"
                        },
                        new
                        {
                            Id = 3L,
                            Biografia = "Perfil fictício de uma moradora que participa de melhorias de acessibilidade no Parque Doroteia.",
                            Nome = "Carla Lima"
                        });
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Postagem", b =>
                {
                    b.HasOne("Aja.HelperApp.Api.Models.Usuario", "Usuario")
                        .WithMany("Postagens")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_pos_usuarios");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Topico", b =>
                {
                    b.HasOne("Aja.HelperApp.Api.Models.Postagem", "Postagem")
                        .WithMany("Topicos")
                        .HasForeignKey("PostagemId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_top_postagens");

                    b.Navigation("Postagem");
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Postagem", b =>
                {
                    b.Navigation("Topicos");
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Usuario", b =>
                {
                    b.Navigation("Postagens");
                });
#pragma warning restore 612, 618
        }
    }
}
