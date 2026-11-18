using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aja.HelperApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class BiografiasDaComunidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1L,
                column: "usu_biografia",
                value: "Sou Ana, professora de música e moradora do Parque Doroteia há 18 anos. Sou cega e me desloco de bengala para dar aulas e encontrar amigos. Gosto de samba, de cozinhar aos domingos e de conversar sobre caminhos que permitam a qualquer pessoa circular com autonomia. Quero contribuir com minhas experiências e ouvir as dos vizinhos. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 2L,
                column: "usu_biografia",
                value: "Sou Bruno, designer gráfico, surdo e usuário de Libras. Cresci na Zona Sul e moro no Doroteia com minha companheira e nossos dois gatos. Adoro fotografia e futebol de várzea. Participo das conversas sobre legendas, avisos visuais e atendimento acessível, porque informação clara aproxima a comunidade. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 3L,
                column: "usu_biografia",
                value: "Sou Carla, técnica de enfermagem e mãe de uma adolescente. Não tenho deficiência e comecei a reparar mais nas barreiras do bairro ao acompanhar uma vizinha com baixa visão. Entre os plantões, gosto de cuidar das plantas e caminhar na praça. Quero ouvir quem enfrenta dificuldades e ajudar a mobilizar melhorias sem falar no lugar dessas pessoas. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 4L,
                column: "usu_biografia",
                value: "Sou Lucas, entregador de bicicleta e estudante de logística à noite. Conheço muitas ruas do Doroteia pelas entregas e quero colaborar com relatos sobre calçadas e travessias. Não tenho deficiência; aprendo com os vizinhos que usam os mesmos caminhos de outras formas. Nos fins de semana, jogo basquete com meus amigos. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 5L,
                column: "usu_biografia",
                value: "Sou Mariana, costureira e moradora do bairro desde criança. Tenho baixa visão e uso letras maiores para trabalhar com o celular. Gosto de cinema com audiodescrição, de trocar receitas e de passear com minha família. Quero participar de propostas de sinalização legível e de espaços culturais que acolham públicos diferentes. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 6L,
                column: "usu_biografia",
                value: "Sou Pedro, aposentado do comércio e avô de três crianças. Não tenho deficiência e costumo ajudar na horta comunitária. Depois de acompanhar um amigo surdo a um serviço público, percebi como um atendimento apenas por voz pode excluir. Estou aqui para aprender, compartilhar ideias e apoiar iniciativas construídas com os moradores. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 7L,
                column: "usu_biografia",
                value: "Sou Juliana, estudante de arquitetura e voluntária em atividades culturais do bairro. Não tenho deficiência; meu interesse por acessibilidade nasceu das conversas com colegas e moradores. Gosto de desenhar, andar de ônibus pela cidade e descobrir bibliotecas. Acredito em projetos que escutem as pessoas antes de propor mudanças. Perfil fictício para demonstração.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1001L,
                column: "usu_biografia",
                value: "Espaço fictício da equipe de moderação do AJA. Neste protótipo, acolhemos novos participantes, incentivamos o diálogo respeitoso e ajudamos a organizar relatos sobre o Parque Doroteia. Nosso papel é favorecer a escuta e a participação dos moradores. Não somos um canal oficial de atendimento público.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1002L,
                column: "usu_biografia",
                value: "Perfil institucional fictício criado para simular o diálogo entre moradores e poder público. Reúne propostas demonstrativas de mobilidade, acessibilidade e melhorias urbanas discutidas no Parque Doroteia. Não representa a Prefeitura de São Paulo, não recebe solicitações oficiais e não anuncia obras ou serviços reais.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1003L,
                column: "usu_biografia",
                value: "Perfil cultural fictício para demonstrar a divulgação de encontros e atividades inclusivas no Parque Doroteia. As pautas incluem leitura, música, audiodescrição, Libras e participação dos moradores na vida cultural. Não possui vínculo com órgãos públicos e os eventos apresentados no protótipo não são reais.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1L,
                column: "usu_biografia",
                value: "Perfil fictício de uma moradora cega do Parque Doroteia, Zona Sul de São Paulo.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 2L,
                column: "usu_biografia",
                value: "Perfil fictício de um morador surdo interessado em comunicação acessível no Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 3L,
                column: "usu_biografia",
                value: "Perfil fictício de uma moradora que participa de melhorias de acessibilidade no Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 4L,
                column: "usu_biografia",
                value: "Perfil fictício do Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 5L,
                column: "usu_biografia",
                value: "Perfil fictício do Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 6L,
                column: "usu_biografia",
                value: "Perfil fictício do Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 7L,
                column: "usu_biografia",
                value: "Perfil fictício do Parque Doroteia.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1001L,
                column: "usu_biografia",
                value: "Perfil fictício da moderação para demonstrar a organização da comunidade.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1002L,
                column: "usu_biografia",
                value: "Perfil fictício, sem vínculo oficial com a Prefeitura de São Paulo.");

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1003L,
                column: "usu_biografia",
                value: "Perfil fictício, sem vínculo com órgãos públicos de cultura.");
        }
    }
}
