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
    [Migration("20261113031857_TopicosComAutoria")]
    partial class TopicosComAutoria
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

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("top_texto");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("varchar(180)")
                        .HasColumnName("top_titulo");

                    b.Property<long>("UsuarioId")
                        .HasColumnType("bigint")
                        .HasColumnName("top_usuario_id");

                    b.HasKey("Id")
                        .HasName("pk_top_topicos");

                    b.HasIndex("PostagemId")
                        .HasDatabaseName("ix_top_postagem_id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("top_topicos", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Assuntos = "[\"defici\\u00EAncia visual\",\"travessia\",\"orienta\\u00E7\\u00E3o acess\\u00EDvel\"]",
                            PostagemId = 1L,
                            Texto = "Quem caminha pelo bairro sabe como uma orientação clara faz diferença. Vamos reunir experiências sobre travessias e entender o que ajuda pessoas cegas a circular com autonomia? Conte sua experiência e ajude a construir uma proposta com quem vive esse desafio. #DoroteiaAcessível #SuaVozTransforma",
                            Titulo = "Uma travessia que acolha todo mundo",
                            UsuarioId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Assuntos = "[\"defici\\u00EAncia auditiva\",\"comunica\\u00E7\\u00E3o visual\",\"servi\\u00E7os p\\u00FAblicos\"]",
                            PostagemId = 2L,
                            Texto = "Avisos só por alto-falante deixam muita gente de fora. Que tal conversar sobre painéis visuais, legendas e outras formas de comunicação nos espaços públicos? Pessoas surdas precisam participar dessa conversa desde o começo. #ComunicaçãoParaTodos #DoroteiaInclui",
                            Titulo = "Informação também precisa ser acessível",
                            UsuarioId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            Assuntos = "[\"cal\\u00E7adas\",\"mobilidade\",\"Parque Doroteia\"]",
                            PostagemId = 3L,
                            Texto = "Um obstáculo pequeno pode virar uma grande barreira para quem usa bengala. Compartilhe observações sobre os caminhos do bairro e explique como isso afeta sua rotina. Vamos organizar os relatos sem expor dados pessoais de ninguém. #CaminhoLivre #JuntosPeloDoroteia",
                            Titulo = "Calçada livre, bairro mais acolhedor",
                            UsuarioId = 3L
                        },
                        new
                        {
                            Id = 4L,
                            Assuntos = "[\"acessibilidade\",\"participa\\u00E7\\u00E3o cidad\\u00E3\",\"S\\u00E3o Paulo\"]",
                            Texto = "Aqui, cada experiência importa. Apoiar uma melhoria não significa concordar com todas as soluções propostas. Ouça quem encontra barreiras no dia a dia, traga evidências e mantenha o respeito. Estes são exemplos fictícios para experimentar o AJA. #Participe #AcessibilidadeÉDireito",
                            Titulo = "Boas-vindas à conversa do bairro!",
                            UsuarioId = 1001L
                        },
                        new
                        {
                            Id = 1001L,
                            Assuntos = "[\"Ouvir pessoas com defici\\u00EAncia visual antes de escolher as interven\\u00E7\\u00F5es.\",\"Mapear obst\\u00E1culos e a continuidade dos percursos de pedestres.\",\"Avaliar orienta\\u00E7\\u00E3o t\\u00E1til e sinaliza\\u00E7\\u00E3o com profissionais habilitados.\",\"Preservar faixas livres de circula\\u00E7\\u00E3o e acessos aos servi\\u00E7os.\",\"Encaminhar o dossi\\u00EA para an\\u00E1lise do \\u00F3rg\\u00E3o competente.\"]",
                            Texto = "Exemplo de mobilização: está aberto um abaixo-assinado demonstrativo para propor guias de orientação e percursos acessíveis no entorno do fórum que atende o bairro. A ideia é ouvir pessoas cegas, reunir relatos e encaminhar uma proposta documentada aos responsáveis. Apoie a discussão, conte quais trajetos merecem atenção e compartilhe este tópico com a comunidade. Nenhuma obra foi aprovada e nenhuma coleta oficial de assinaturas está acontecendo neste protótipo. Premissas da proposta: #DoroteiaAcessível #CaminharComAutonomia #JuntosPelaInclusão",
                            Titulo = "Vamos construir caminhos mais acessíveis no Doroteia?",
                            UsuarioId = 1002L
                        },
                        new
                        {
                            Id = 1002L,
                            Assuntos = "[\"Autonomia de pessoas cegas nos espa\\u00E7os de uso p\\u00FAblico.\",\"Barreiras de orienta\\u00E7\\u00E3o que passam despercebidas no cotidiano.\",\"Como descrever informa\\u00E7\\u00F5es visuais com clareza.\",\"Participa\\u00E7\\u00E3o de pessoas surdas e comunica\\u00E7\\u00E3o acess\\u00EDvel.\",\"Ideias da comunidade para espa\\u00E7os culturais mais inclusivos.\"]",
                            Texto = "Convite fictício para o piloto: uma palestra cultural sobre acessibilidade e deficiência visual aconteceria em 21 de novembro de 2026, às 14h, em um espaço comunitário a definir no Parque Doroteia. A proposta é dar visibilidade a experiências que costumam passar despercebidas e abrir espaço para perguntas, escuta e troca de ideias. A programação prevê linguagem clara, descrição dos materiais visuais e discussão de recursos de comunicação acessível. Este anúncio não corresponde a um evento real. Assuntos da conversa: #CulturaAcessível #OuvirParaTransformar #DoroteiaParticipa",
                            Titulo = "Uma conversa para enxergar a acessibilidade de outro jeito",
                            UsuarioId = 1003L
                        },
                        new
                        {
                            Id = 1003L,
                            Assuntos = "[]",
                            Texto = "Encontrou uma barreira no bairro? Publique seu relato no feed e explique como ela interfere no seu caminho. A comunidade pode complementar as informações e discutir alternativas. Não é preciso ter uma solução pronta para participar. #SuaVozImporta #AJAComRespeito #BairroParaTodos",
                            Titulo = "Sua experiência pode ser o começo de uma mudança",
                            UsuarioId = 1001L
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
                        },
                        new
                        {
                            Id = 1001L,
                            Biografia = "Perfil fictício da moderação para demonstrar a organização da comunidade.",
                            Email = "moderacao@aja.example",
                            Nome = "Moderação AJA · Demonstração",
                            SenhaHash = ""
                        },
                        new
                        {
                            Id = 1002L,
                            Biografia = "Perfil fictício, sem vínculo oficial com a Prefeitura de São Paulo.",
                            Email = "prefeitura@aja.example",
                            Nome = "Prefeitura de São Paulo · Demonstração",
                            SenhaHash = ""
                        },
                        new
                        {
                            Id = 1003L,
                            Biografia = "Perfil fictício, sem vínculo com órgãos públicos de cultura.",
                            Email = "cultura@aja.example",
                            Nome = "Cultura SP · Demonstração",
                            SenhaHash = ""
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

                    b.HasOne("Aja.HelperApp.Api.Models.Usuario", "Usuario")
                        .WithMany("Topicos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_top_usuarios");

                    b.Navigation("Postagem");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Postagem", b =>
                {
                    b.Navigation("Topicos");
                });

            modelBuilder.Entity("Aja.HelperApp.Api.Models.Usuario", b =>
                {
                    b.Navigation("Postagens");

                    b.Navigation("Topicos");
                });
#pragma warning restore 612, 618
        }
    }
}
