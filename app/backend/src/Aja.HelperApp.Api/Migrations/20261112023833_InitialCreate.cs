using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace Aja.HelperApp.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usu_usuarios",
                columns: table => new
                {
                    usu_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usu_nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usu_biografia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usu_usuarios", x => x.usu_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pos_postagens",
                columns: table => new
                {
                    pos_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pos_texto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pos_imagem_url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pos_usuario_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pos_postagens", x => x.pos_id);
                    table.ForeignKey(
                        name: "fk_pos_usuarios",
                        column: x => x.pos_usuario_id,
                        principalTable: "usu_usuarios",
                        principalColumn: "usu_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "top_topicos",
                columns: table => new
                {
                    top_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    top_assuntos = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    top_postagem_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_top_topicos", x => x.top_id);
                    table.ForeignKey(
                        name: "fk_top_postagens",
                        column: x => x.top_postagem_id,
                        principalTable: "pos_postagens",
                        principalColumn: "pos_id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "top_topicos",
                columns: new[] { "top_id", "top_assuntos", "top_postagem_id" },
                values: new object[] { 4L, "[\"acessibilidade\",\"participa\\u00E7\\u00E3o cidad\\u00E3\",\"S\\u00E3o Paulo\"]", null });

            migrationBuilder.InsertData(
                table: "usu_usuarios",
                columns: new[] { "usu_id", "usu_biografia", "usu_nome" },
                values: new object[,]
                {
                    { 1L, "Perfil fictício de uma moradora cega do Parque Doroteia, Zona Sul de São Paulo.", "Ana Ribeiro" },
                    { 2L, "Perfil fictício de um morador surdo interessado em comunicação acessível no Parque Doroteia.", "Bruno Santos" },
                    { 3L, "Perfil fictício de uma moradora que participa de melhorias de acessibilidade no Parque Doroteia.", "Carla Lima" }
                });

            migrationBuilder.InsertData(
                table: "pos_postagens",
                columns: new[] { "pos_id", "pos_imagem_url", "pos_texto", "pos_usuario_id" },
                values: new object[,]
                {
                    { 1L, null, "Caso demonstrativo: uma travessia no Parque Doroteia precisa de recursos de orientação para pessoas cegas.", 1L },
                    { 2L, null, "Caso demonstrativo: informações de atendimento em um espaço público do Parque Doroteia poderiam contar com comunicação visual acessível a pessoas surdas.", 2L },
                    { 3L, "https://placehold.co/1200x800/png?text=Imagem+demonstrativa", "Caso demonstrativo: obstáculos na calçada dificultam a circulação e a orientação de moradores do Parque Doroteia.", 3L }
                });

            migrationBuilder.InsertData(
                table: "top_topicos",
                columns: new[] { "top_id", "top_assuntos", "top_postagem_id" },
                values: new object[,]
                {
                    { 1L, "[\"defici\\u00EAncia visual\",\"travessia\",\"orienta\\u00E7\\u00E3o acess\\u00EDvel\"]", 1L },
                    { 2L, "[\"defici\\u00EAncia auditiva\",\"comunica\\u00E7\\u00E3o visual\",\"servi\\u00E7os p\\u00FAblicos\"]", 2L },
                    { 3L, "[\"cal\\u00E7adas\",\"mobilidade\",\"Parque Doroteia\"]", 3L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_pos_usuario_id",
                table: "pos_postagens",
                column: "pos_usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_top_postagem_id",
                table: "top_topicos",
                column: "top_postagem_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "top_topicos");

            migrationBuilder.DropTable(
                name: "pos_postagens");

            migrationBuilder.DropTable(
                name: "usu_usuarios");
        }
    }
}
