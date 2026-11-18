using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Data;

internal static class InstitutionalDemonstrationData
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasData(new Usuario
            {
                Id = 1001,
                Nome = "Moderação AJA · Demonstração",
                Email = "moderacao@aja.example",
                Biografia = "Espaço fictício da equipe de moderação do AJA. Neste protótipo, acolhemos novos participantes, incentivamos o diálogo respeitoso e ajudamos a organizar relatos sobre o Parque Doroteia. Nosso papel é favorecer a escuta e a participação dos moradores. Não somos um canal oficial de atendimento público."
            },
            new Usuario
            {
                Id = 1002,
                Nome = "Prefeitura de São Paulo · Demonstração",
                Email = "prefeitura@aja.example",
                Biografia = "Perfil institucional fictício criado para simular o diálogo entre moradores e poder público. Reúne propostas demonstrativas de mobilidade, acessibilidade e melhorias urbanas discutidas no Parque Doroteia. Não representa a Prefeitura de São Paulo, não recebe solicitações oficiais e não anuncia obras ou serviços reais."
            },
            new Usuario
            {
                Id = 1003,
                Nome = "Cultura SP · Demonstração",
                Email = "cultura@aja.example",
                Biografia = "Perfil cultural fictício para demonstrar a divulgação de encontros e atividades inclusivas no Parque Doroteia. As pautas incluem leitura, música, audiodescrição, Libras e participação dos moradores na vida cultural. Não possui vínculo com órgãos públicos e os eventos apresentados no protótipo não são reais."
            });

        modelBuilder.Entity<Topico>()
            .HasData(new Topico
            {
                Id = 1001,
                UsuarioId = 1002,
                Titulo = "Vamos construir caminhos mais acessíveis no Doroteia?",
                Interesses = 86,
                Texto = "Exemplo de mobilização: está aberto um abaixo-assinado demonstrativo para propor guias de orientação e percursos acessíveis no entorno do fórum que atende o bairro. A ideia é ouvir pessoas cegas, reunir relatos e encaminhar uma proposta documentada aos responsáveis. Apoie a discussão, conte quais trajetos merecem atenção e compartilhe este tópico com a comunidade. Nenhuma obra foi aprovada e nenhuma coleta oficial de assinaturas está acontecendo neste protótipo. Premissas da proposta: #DoroteiaAcessível #CaminharComAutonomia #JuntosPelaInclusão",
                Assuntos = ["Ouvir pessoas com deficiência visual antes de escolher as intervenções.;Mapear obstáculos e a continuidade dos percursos de pedestres.;Avaliar orientação tátil e sinalização com profissionais habilitados.;Preservar faixas livres de circulação e acessos aos serviços.", "Encaminhar o dossiê para análise do órgão competente."]
            },
            new Topico
            {
                Id = 1002,
                UsuarioId = 1003,
                Titulo = "Uma conversa para enxergar a acessibilidade de outro jeito",
                Interesses = 53,
                Texto = "Convite fictício para o piloto: uma palestra cultural sobre acessibilidade e deficiência visual aconteceria em 21 de novembro de 2026, às 14h, em um espaço comunitário a definir no Parque Doroteia. A proposta é dar visibilidade a experiências que costumam passar despercebidas e abrir espaço para perguntas, escuta e troca de ideias. A programação prevê linguagem clara, descrição dos materiais visuais e discussão de recursos de comunicação acessível. Este anúncio não corresponde a um evento real. Assuntos da conversa: #CulturaAcessível #OuvirParaTransformar #DoroteiaParticipa",
                Assuntos = ["Autonomia de pessoas cegas nos espaços de uso público.;Barreiras de orientação que passam despercebidas no cotidiano.;Como descrever informações visuais com clareza.", "Participação de pessoas surdas e comunicação acessível.", "Ideias da comunidade para espaços culturais mais inclusivos."]
            },
            new Topico
            {
                Id = 1003,
                UsuarioId = 1001,
                Titulo = "Sua experiência pode ser o começo de uma mudança",
                Interesses = 12,
                Texto = "Encontrou uma barreira no bairro? Publique seu relato no feed e explique como ela interfere no seu caminho. A comunidade pode complementar as informações e discutir alternativas. Não é preciso ter uma solução pronta para participar. #SuaVozImporta #AJAComRespeito #BairroParaTodos",
                Assuntos = []
            });
    }
}
