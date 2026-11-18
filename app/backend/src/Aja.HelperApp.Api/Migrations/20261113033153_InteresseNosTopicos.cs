using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aja.HelperApp.Api.Migrations
{
    public partial class InteresseNosTopicos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "top_interesses",
                table: "top_topicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1L,
                column: "top_interesses",
                value: 24);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 2L,
                column: "top_interesses",
                value: 18);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 3L,
                column: "top_interesses",
                value: 32);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 4L,
                column: "top_interesses",
                value: 45);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1001L,
                columns: new[] { "top_assuntos", "top_interesses" },
                values: new object[] { "[\"Ouvir pessoas com defici\\u00EAncia visual antes de escolher as interven\\u00E7\\u00F5es.;Mapear obst\\u00E1culos e a continuidade dos percursos de pedestres.;Avaliar orienta\\u00E7\\u00E3o t\\u00E1til e sinaliza\\u00E7\\u00E3o com profissionais habilitados.;Preservar faixas livres de circula\\u00E7\\u00E3o e acessos aos servi\\u00E7os.\",\"Encaminhar o dossi\\u00EA para an\\u00E1lise do \\u00F3rg\\u00E3o competente.\"]", 86 });

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1002L,
                columns: new[] { "top_assuntos", "top_interesses" },
                values: new object[] { "[\"Autonomia de pessoas cegas nos espa\\u00E7os de uso p\\u00FAblico.;Barreiras de orienta\\u00E7\\u00E3o que passam despercebidas no cotidiano.;Como descrever informa\\u00E7\\u00F5es visuais com clareza.\",\"Participa\\u00E7\\u00E3o de pessoas surdas e comunica\\u00E7\\u00E3o acess\\u00EDvel.\",\"Ideias da comunidade para espa\\u00E7os culturais mais inclusivos.\"]", 53 });

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1003L,
                column: "top_interesses",
                value: 12);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "top_interesses",
                table: "top_topicos");

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1001L,
                column: "top_assuntos",
                value: "[\"Ouvir pessoas com defici\\u00EAncia visual antes de escolher as interven\\u00E7\\u00F5es.\",\"Mapear obst\\u00E1culos e a continuidade dos percursos de pedestres.\",\"Avaliar orienta\\u00E7\\u00E3o t\\u00E1til e sinaliza\\u00E7\\u00E3o com profissionais habilitados.\",\"Preservar faixas livres de circula\\u00E7\\u00E3o e acessos aos servi\\u00E7os.\",\"Encaminhar o dossi\\u00EA para an\\u00E1lise do \\u00F3rg\\u00E3o competente.\"]");

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1002L,
                column: "top_assuntos",
                value: "[\"Autonomia de pessoas cegas nos espa\\u00E7os de uso p\\u00FAblico.\",\"Barreiras de orienta\\u00E7\\u00E3o que passam despercebidas no cotidiano.\",\"Como descrever informa\\u00E7\\u00F5es visuais com clareza.\",\"Participa\\u00E7\\u00E3o de pessoas surdas e comunica\\u00E7\\u00E3o acess\\u00EDvel.\",\"Ideias da comunidade para espa\\u00E7os culturais mais inclusivos.\"]");
        }
    }
}
