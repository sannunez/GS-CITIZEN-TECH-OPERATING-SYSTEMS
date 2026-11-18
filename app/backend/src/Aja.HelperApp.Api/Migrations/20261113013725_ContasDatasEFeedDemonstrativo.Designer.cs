using System;
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
    [Migration("20261113013725_ContasDatasEFeedDemonstrativo")]
    partial class ContasDatasEFeedDemonstrativo
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

                    b.Property<DateTime>("CriadaEm")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("pos_criada_em");

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

                    b.HasIndex("CriadaEm", "Id")
                        .HasDatabaseName("ix_pos_data_id");

                    b.ToTable("pos_postagens", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CriadaEm = new DateTime(2026, 11, 12, 9, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo: uma travessia no Parque Doroteia precisa de recursos de orientação para pessoas cegas.",
                            UsuarioId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            CriadaEm = new DateTime(2026, 11, 12, 8, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo: informações de atendimento em um espaço público do Parque Doroteia poderiam contar com comunicação visual acessível a pessoas surdas.",
                            UsuarioId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            CriadaEm = new DateTime(2026, 11, 12, 7, 0, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/1200x800/png?text=Imagem+demonstrativa",
                            Texto = "Caso demonstrativo: obstáculos na calçada dificultam a circulação e a orientação de moradores do Parque Doroteia.",
                            UsuarioId = 3L
                        },
                        new
                        {
                            Id = 4L,
                            CriadaEm = new DateTime(2026, 11, 12, 11, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 4: discussão sobre calçadas livres de obstáculos no Parque Doroteia, São Paulo.",
                            UsuarioId = 5L
                        },
                        new
                        {
                            Id = 5L,
                            CriadaEm = new DateTime(2026, 11, 12, 11, 15, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 5: discussão sobre sinalização visual nos serviços públicos no Parque Doroteia, São Paulo.",
                            UsuarioId = 6L
                        },
                        new
                        {
                            Id = 6L,
                            CriadaEm = new DateTime(2026, 11, 12, 11, 30, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/900x600/png?text=Caso+demonstrativo+6",
                            Texto = "Caso demonstrativo 6: discussão sobre orientação tátil nas travessias no Parque Doroteia, São Paulo.",
                            UsuarioId = 7L
                        },
                        new
                        {
                            Id = 7L,
                            CriadaEm = new DateTime(2026, 11, 12, 11, 45, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 7: discussão sobre comunicação acessível a pessoas surdas no Parque Doroteia, São Paulo.",
                            UsuarioId = 1L
                        },
                        new
                        {
                            Id = 8L,
                            CriadaEm = new DateTime(2026, 11, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 8: discussão sobre acesso seguro às praças no Parque Doroteia, São Paulo.",
                            UsuarioId = 2L
                        },
                        new
                        {
                            Id = 9L,
                            CriadaEm = new DateTime(2026, 11, 12, 12, 15, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/900x600/png?text=Caso+demonstrativo+9",
                            Texto = "Caso demonstrativo 9: discussão sobre participação de pessoas cegas nas melhorias no Parque Doroteia, São Paulo.",
                            UsuarioId = 3L
                        },
                        new
                        {
                            Id = 10L,
                            CriadaEm = new DateTime(2026, 11, 12, 12, 30, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 10: discussão sobre informações de transporte em formato visual no Parque Doroteia, São Paulo.",
                            UsuarioId = 4L
                        },
                        new
                        {
                            Id = 11L,
                            CriadaEm = new DateTime(2026, 11, 12, 12, 45, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 11: discussão sobre continuidade do piso tátil no Parque Doroteia, São Paulo.",
                            UsuarioId = 5L
                        },
                        new
                        {
                            Id = 12L,
                            CriadaEm = new DateTime(2026, 11, 12, 13, 0, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/900x600/png?text=Caso+demonstrativo+12",
                            Texto = "Caso demonstrativo 12: discussão sobre atendimento inclusivo no Parque Doroteia, São Paulo.",
                            UsuarioId = 6L
                        },
                        new
                        {
                            Id = 13L,
                            CriadaEm = new DateTime(2026, 11, 12, 13, 15, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 13: discussão sobre travessias acessíveis no Parque Doroteia, São Paulo.",
                            UsuarioId = 7L
                        },
                        new
                        {
                            Id = 14L,
                            CriadaEm = new DateTime(2026, 11, 12, 13, 30, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 14: discussão sobre calçadas livres de obstáculos no Parque Doroteia, São Paulo.",
                            UsuarioId = 1L
                        },
                        new
                        {
                            Id = 15L,
                            CriadaEm = new DateTime(2026, 11, 12, 13, 45, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/900x600/png?text=Caso+demonstrativo+15",
                            Texto = "Caso demonstrativo 15: discussão sobre sinalização visual nos serviços públicos no Parque Doroteia, São Paulo.",
                            UsuarioId = 2L
                        },
                        new
                        {
                            Id = 16L,
                            CriadaEm = new DateTime(2026, 11, 12, 14, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 16: discussão sobre orientação tátil nas travessias no Parque Doroteia, São Paulo.",
                            UsuarioId = 3L
                        },
                        new
                        {
                            Id = 17L,
                            CriadaEm = new DateTime(2026, 11, 12, 14, 15, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 17: discussão sobre comunicação acessível a pessoas surdas no Parque Doroteia, São Paulo.",
                            UsuarioId = 4L
                        },
                        new
                        {
                            Id = 18L,
                            CriadaEm = new DateTime(2026, 11, 12, 14, 30, 0, 0, DateTimeKind.Utc),
                            ImagemUrl = "https://placehold.co/900x600/png?text=Caso+demonstrativo+18",
                            Texto = "Caso demonstrativo 18: discussão sobre acesso seguro às praças no Parque Doroteia, São Paulo.",
                            UsuarioId = 5L
                        },
                        new
                        {
                            Id = 19L,
                            CriadaEm = new DateTime(2026, 11, 12, 14, 45, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 19: discussão sobre participação de pessoas cegas nas melhorias no Parque Doroteia, São Paulo.",
                            UsuarioId = 6L
                        },
                        new
                        {
                            Id = 20L,
                            CriadaEm = new DateTime(2026, 11, 12, 15, 0, 0, 0, DateTimeKind.Utc),
                            Texto = "Caso demonstrativo 20: discussão sobre informações de transporte em formato visual no Parque Doroteia, São Paulo.",
                            UsuarioId = 7L
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

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("varchar(254)")
                        .HasColumnName("usu_email");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("usu_nome");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("usu_senha_hash");

                    b.HasKey("Id")
                        .HasName("pk_usu_usuarios");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ux_usu_email");

                    b.ToTable("usu_usuarios", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Biografia = "Perfil fictício de uma moradora cega do Parque Doroteia, Zona Sul de São Paulo.",
                            Email = "ana@aja.example",
                            Nome = "Ana Ribeiro",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 2L,
                            Biografia = "Perfil fictício de um morador surdo interessado em comunicação acessível no Parque Doroteia.",
                            Email = "bruno@aja.example",
                            Nome = "Bruno Santos",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 3L,
                            Biografia = "Perfil fictício de uma moradora que participa de melhorias de acessibilidade no Parque Doroteia.",
                            Email = "carla@aja.example",
                            Nome = "Carla Lima",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 4L,
                            Biografia = "Perfil fictício do Parque Doroteia.",
                            Email = "demo4@aja.example",
                            Nome = "Lucas Oliveira",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 5L,
                            Biografia = "Perfil fictício do Parque Doroteia.",
                            Email = "demo5@aja.example",
                            Nome = "Mariana Costa",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 6L,
                            Biografia = "Perfil fictício do Parque Doroteia.",
                            Email = "demo6@aja.example",
                            Nome = "Pedro Alves",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
                        },
                        new
                        {
                            Id = 7L,
                            Biografia = "Perfil fictício do Parque Doroteia.",
                            Email = "demo7@aja.example",
                            Nome = "Juliana Souza",
                            SenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow=="
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
