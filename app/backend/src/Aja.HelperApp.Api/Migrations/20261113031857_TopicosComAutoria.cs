using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace Aja.HelperApp.Api.Migrations
{
    public partial class TopicosComAutoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "top_texto",
                table: "top_topicos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "top_titulo",
                table: "top_topicos",
                type: "varchar(180)",
                maxLength: 180,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "top_usuario_id",
                table: "top_topicos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1L,
                columns: new[] { "top_texto", "top_titulo", "top_usuario_id" },
                values: new object[] { "Quem caminha pelo bairro sabe como uma orientação clara faz diferença. Vamos reunir experiências sobre travessias e entender o que ajuda pessoas cegas a circular com autonomia? Conte sua experiência e ajude a construir uma proposta com quem vive esse desafio. #DoroteiaAcessível #SuaVozTransforma", "Uma travessia que acolha todo mundo", 1L });

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 2L,
                columns: new[] { "top_texto", "top_titulo", "top_usuario_id" },
                values: new object[] { "Avisos só por alto-falante deixam muita gente de fora. Que tal conversar sobre painéis visuais, legendas e outras formas de comunicação nos espaços públicos? Pessoas surdas precisam participar dessa conversa desde o começo. #ComunicaçãoParaTodos #DoroteiaInclui", "Informação também precisa ser acessível", 2L });

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 3L,
                columns: new[] { "top_texto", "top_titulo", "top_usuario_id" },
                values: new object[] { "Um obstáculo pequeno pode virar uma grande barreira para quem usa bengala. Compartilhe observações sobre os caminhos do bairro e explique como isso afeta sua rotina. Vamos organizar os relatos sem expor dados pessoais de ninguém. #CaminhoLivre #JuntosPeloDoroteia", "Calçada livre, bairro mais acolhedor", 3L });

            migrationBuilder.UpdateData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 4L,
                columns: new[] { "top_texto", "top_titulo", "top_usuario_id" },
                values: new object[] { "Aqui, cada experiência importa. Apoiar uma melhoria não significa concordar com todas as soluções propostas. Ouça quem encontra barreiras no dia a dia, traga evidências e mantenha o respeito. Estes são exemplos fictícios para experimentar o AJA. #Participe #AcessibilidadeÉDireito", "Boas-vindas à conversa do bairro!", 1001L });

            migrationBuilder.InsertData(
                table: "usu_usuarios",
                columns: new[] { "usu_id", "usu_biografia", "usu_email", "usu_nome", "usu_senha_hash" },
                values: new object[,]
                {
                    { 1001L, "Perfil fictício da moderação para demonstrar a organização da comunidade.", "moderacao@aja.example", "Moderação AJA · Demonstração", "" },
                    { 1002L, "Perfil fictício, sem vínculo oficial com a Prefeitura de São Paulo.", "prefeitura@aja.example", "Prefeitura de São Paulo · Demonstração", "" },
                    { 1003L, "Perfil fictício, sem vínculo com órgãos públicos de cultura.", "cultura@aja.example", "Cultura SP · Demonstração", "" }
                });

            migrationBuilder.InsertData(
                table: "top_topicos",
                columns: new[] { "top_id", "top_assuntos", "top_postagem_id", "top_texto", "top_titulo", "top_usuario_id" },
                values: new object[,]
                {
                    { 1001L, "[\"Ouvir pessoas com defici\\u00EAncia visual antes de escolher as interven\\u00E7\\u00F5es.\",\"Mapear obst\\u00E1culos e a continuidade dos percursos de pedestres.\",\"Avaliar orienta\\u00E7\\u00E3o t\\u00E1til e sinaliza\\u00E7\\u00E3o com profissionais habilitados.\",\"Preservar faixas livres de circula\\u00E7\\u00E3o e acessos aos servi\\u00E7os.\",\"Encaminhar o dossi\\u00EA para an\\u00E1lise do \\u00F3rg\\u00E3o competente.\"]", null, "Exemplo de mobilização: está aberto um abaixo-assinado demonstrativo para propor guias de orientação e percursos acessíveis no entorno do fórum que atende o bairro. A ideia é ouvir pessoas cegas, reunir relatos e encaminhar uma proposta documentada aos responsáveis. Apoie a discussão, conte quais trajetos merecem atenção e compartilhe este tópico com a comunidade. Nenhuma obra foi aprovada e nenhuma coleta oficial de assinaturas está acontecendo neste protótipo. Premissas da proposta: #DoroteiaAcessível #CaminharComAutonomia #JuntosPelaInclusão", "Vamos construir caminhos mais acessíveis no Doroteia?", 1002L },
                    { 1002L, "[\"Autonomia de pessoas cegas nos espa\\u00E7os de uso p\\u00FAblico.\",\"Barreiras de orienta\\u00E7\\u00E3o que passam despercebidas no cotidiano.\",\"Como descrever informa\\u00E7\\u00F5es visuais com clareza.\",\"Participa\\u00E7\\u00E3o de pessoas surdas e comunica\\u00E7\\u00E3o acess\\u00EDvel.\",\"Ideias da comunidade para espa\\u00E7os culturais mais inclusivos.\"]", null, "Convite fictício para o piloto: uma palestra cultural sobre acessibilidade e deficiência visual aconteceria em 21 de novembro de 2026, às 14h, em um espaço comunitário a definir no Parque Doroteia. A proposta é dar visibilidade a experiências que costumam passar despercebidas e abrir espaço para perguntas, escuta e troca de ideias. A programação prevê linguagem clara, descrição dos materiais visuais e discussão de recursos de comunicação acessível. Este anúncio não corresponde a um evento real. Assuntos da conversa: #CulturaAcessível #OuvirParaTransformar #DoroteiaParticipa", "Uma conversa para enxergar a acessibilidade de outro jeito", 1003L },
                    { 1003L, "[]", null, "Encontrou uma barreira no bairro? Publique seu relato no feed e explique como ela interfere no seu caminho. A comunidade pode complementar as informações e discutir alternativas. Não é preciso ter uma solução pronta para participar. #SuaVozImporta #AJAComRespeito #BairroParaTodos", "Sua experiência pode ser o começo de uma mudança", 1001L }
                });

            migrationBuilder.Sql("UPDATE top_topicos SET top_texto = '' WHERE top_texto IS NULL;");

            migrationBuilder.AlterColumn<string>(
                name: "top_texto",
                table: "top_topicos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql("UPDATE top_topicos AS t LEFT JOIN pos_postagens AS p ON p.pos_id = t.top_postagem_id LEFT JOIN usu_usuarios AS u ON u.usu_id = t.top_usuario_id SET t.top_usuario_id = COALESCE(p.pos_usuario_id, 1001) WHERE u.usu_id IS NULL;");

            migrationBuilder.CreateIndex(
                name: "IX_top_topicos_top_usuario_id",
                table: "top_topicos",
                column: "top_usuario_id");

            migrationBuilder.AddForeignKey(
                name: "fk_top_usuarios",
                table: "top_topicos",
                column: "top_usuario_id",
                principalTable: "usu_usuarios",
                principalColumn: "usu_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_top_usuarios",
                table: "top_topicos");

            migrationBuilder.DropIndex(
                name: "IX_top_topicos_top_usuario_id",
                table: "top_topicos");

            migrationBuilder.DeleteData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1001L);

            migrationBuilder.DeleteData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1002L);

            migrationBuilder.DeleteData(
                table: "top_topicos",
                keyColumn: "top_id",
                keyValue: 1003L);

            migrationBuilder.DeleteData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1001L);

            migrationBuilder.DeleteData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1002L);

            migrationBuilder.DeleteData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1003L);

            migrationBuilder.DropColumn(
                name: "top_texto",
                table: "top_topicos");

            migrationBuilder.DropColumn(
                name: "top_titulo",
                table: "top_topicos");

            migrationBuilder.DropColumn(
                name: "top_usuario_id",
                table: "top_topicos");
        }
    }
}
