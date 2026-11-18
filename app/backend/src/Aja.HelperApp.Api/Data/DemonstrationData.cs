using Aja.HelperApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aja.HelperApp.Api.Data;

internal static class DemonstrationData
{
    public const string DemoSenhaHash = "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==";

    public static void Configure(ModelBuilder modelBuilder)
    {
        var nomes = new[]
        {
            "Lucas Oliveira",
            "Mariana Costa",
            "Pedro Alves",
            "Juliana Souza"
        };

        var biografias = new[]
        {
            "Sou Lucas, entregador de bicicleta e estudante de logística à noite. Conheço muitas ruas do Doroteia pelas entregas e quero colaborar com relatos sobre calçadas e travessias. Não tenho deficiência; aprendo com os vizinhos que usam os mesmos caminhos de outras formas. Nos fins de semana, jogo basquete com meus amigos. Perfil fictício para demonstração.",
            "Sou Mariana, costureira e moradora do bairro desde criança. Tenho baixa visão e uso letras maiores para trabalhar com o celular. Gosto de cinema com audiodescrição, de trocar receitas e de passear com minha família. Quero participar de propostas de sinalização legível e de espaços culturais que acolham públicos diferentes. Perfil fictício para demonstração.",
            "Sou Pedro, aposentado do comércio e avô de três crianças. Não tenho deficiência e costumo ajudar na horta comunitária. Depois de acompanhar um amigo surdo a um serviço público, percebi como um atendimento apenas por voz pode excluir. Estou aqui para aprender, compartilhar ideias e apoiar iniciativas construídas com os moradores. Perfil fictício para demonstração.",
            "Sou Juliana, estudante de arquitetura e voluntária em atividades culturais do bairro. Não tenho deficiência; meu interesse por acessibilidade nasceu das conversas com colegas e moradores. Gosto de desenhar, andar de ônibus pela cidade e descobrir bibliotecas. Acredito em projetos que escutem as pessoas antes de propor mudanças. Perfil fictício para demonstração."
        };

        var assuntos = new[]
        {
            "calçadas livres de obstáculos",
            "sinalização visual nos serviços públicos",
            "orientação tátil nas travessias",
            "comunicação acessível a pessoas surdas",
            "acesso seguro às praças",
            "participação de pessoas cegas nas melhorias",
            "informações de transporte em formato visual",
            "continuidade do piso tátil",
            "atendimento inclusivo",
            "travessias acessíveis"
        };

        modelBuilder.Entity<Usuario>()
            .HasData(new Usuario
            {
                Id = 1,
                Nome = "Ana Ribeiro",
                Email = "ana@aja.example",
                SenhaHash = DemoSenhaHash,
                Biografia = "Sou Ana, professora de música e moradora do Parque Doroteia há 18 anos. Sou cega e me desloco de bengala para dar aulas e encontrar amigos. Gosto de samba, de cozinhar aos domingos e de conversar sobre caminhos que permitam a qualquer pessoa circular com autonomia. Quero contribuir com minhas experiências e ouvir as dos vizinhos. Perfil fictício para demonstração."
            },
            new Usuario
            {
                Id = 2,
                Nome = "Bruno Santos",
                Email = "bruno@aja.example",
                SenhaHash = DemoSenhaHash,
                Biografia = "Sou Bruno, designer gráfico, surdo e usuário de Libras. Cresci na Zona Sul e moro no Doroteia com minha companheira e nossos dois gatos. Adoro fotografia e futebol de várzea. Participo das conversas sobre legendas, avisos visuais e atendimento acessível, porque informação clara aproxima a comunidade. Perfil fictício para demonstração."
            },
            new Usuario
            {
                Id = 3,
                Nome = "Carla Lima",
                Email = "carla@aja.example",
                SenhaHash = DemoSenhaHash,
                Biografia = "Sou Carla, técnica de enfermagem e mãe de uma adolescente. Não tenho deficiência e comecei a reparar mais nas barreiras do bairro ao acompanhar uma vizinha com baixa visão. Entre os plantões, gosto de cuidar das plantas e caminhar na praça. Quero ouvir quem enfrenta dificuldades e ajudar a mobilizar melhorias sem falar no lugar dessas pessoas. Perfil fictício para demonstração."
            });

        modelBuilder.Entity<Postagem>()
            .HasData(new Postagem
            {
                Id = 1,
                Texto = "Caso demonstrativo: uma travessia no Parque Doroteia precisa de recursos de orientação para pessoas cegas.",
                UsuarioId = 1,
                CriadaEm = new DateTime(2026, 11, 12, 9, 0, 0, DateTimeKind.Utc)
            },
            new Postagem
            {
                Id = 2,
                Texto = "Caso demonstrativo: informações de atendimento em um espaço público do Parque Doroteia poderiam contar com comunicação visual acessível a pessoas surdas.",
                UsuarioId = 2,
                CriadaEm = new DateTime(2026, 11, 12, 8, 0, 0, DateTimeKind.Utc)
            },
            new Postagem
            {
                Id = 3,
                Texto = "Caso demonstrativo: obstáculos na calçada dificultam a circulação e a orientação de moradores do Parque Doroteia.",
                ImagemUrl = "https://placehold.co/1200x800/png?text=Imagem+demonstrativa",
                UsuarioId = 3,
                CriadaEm = new DateTime(2026, 11, 12, 7, 0, 0, DateTimeKind.Utc)
            });

        for (var i = 0; i < nomes.Length; i++)
        {
            modelBuilder.Entity<Usuario>()
                .HasData(new Usuario
                {
                    Id = i + 4,
                    Nome = nomes[i],
                    Email = $"demo{i + 4}@aja.example",
                    SenhaHash = DemoSenhaHash,
                    Biografia = biografias[i]
                });
        }

        for (var id = 4; id <= 20; id++)
        {
            modelBuilder.Entity<Postagem>()
                .HasData(new Postagem
                {
                    Id = id,
                    UsuarioId = (id % 7) + 1,
                    Texto = $"Caso demonstrativo {id}: discussão sobre {assuntos[(id - 4) % assuntos.Length]} no Parque Doroteia, São Paulo.",
                    ImagemUrl = id % 3 == 0 ? $"https://placehold.co/900x600/png?text=Caso+demonstrativo+{id}" : null,
                    CriadaEm = new DateTime(2026, 11, 12, 10, 0, 0, DateTimeKind.Utc).AddMinutes(id * 15)
                });
        }

        modelBuilder.Entity<Topico>()
            .HasData(new Topico
            {
                Id = 1,
                Assuntos = ["deficiência visual", "travessia", "orientação acessível"],
                UsuarioId = 1,
                Titulo = "Uma travessia que acolha todo mundo",
                Interesses = 24,
                Texto = "Quem caminha pelo bairro sabe como uma orientação clara faz diferença. Vamos reunir experiências sobre travessias e entender o que ajuda pessoas cegas a circular com autonomia? Conte sua experiência e ajude a construir uma proposta com quem vive esse desafio. #DoroteiaAcessível #SuaVozTransforma",
                PostagemId = 1
            },
            new Topico
            {
                Id = 2,
                Assuntos = ["deficiência auditiva", "comunicação visual", "serviços públicos"],
                UsuarioId = 2,
                Titulo = "Informação também precisa ser acessível",
                Interesses = 18,
                Texto = "Avisos só por alto-falante deixam muita gente de fora. Que tal conversar sobre painéis visuais, legendas e outras formas de comunicação nos espaços públicos? Pessoas surdas precisam participar dessa conversa desde o começo. #ComunicaçãoParaTodos #DoroteiaInclui",
                PostagemId = 2
            },
            new Topico
            {
                Id = 3,
                Assuntos = ["calçadas", "mobilidade", "Parque Doroteia"],
                UsuarioId = 3,
                Titulo = "Calçada livre, bairro mais acolhedor",
                Interesses = 32,
                Texto = "Um obstáculo pequeno pode virar uma grande barreira para quem usa bengala. Compartilhe observações sobre os caminhos do bairro e explique como isso afeta sua rotina. Vamos organizar os relatos sem expor dados pessoais de ninguém. #CaminhoLivre #JuntosPeloDoroteia",
                PostagemId = 3
            },
            new Topico
            {
                Id = 4,
                UsuarioId = 1001,
                Titulo = "Boas-vindas à conversa do bairro!",
                Interesses = 45,
                Texto = "Aqui, cada experiência importa. Apoiar uma melhoria não significa concordar com todas as soluções propostas. Ouça quem encontra barreiras no dia a dia, traga evidências e mantenha o respeito. Estes são exemplos fictícios para experimentar o AJA. #Participe #AcessibilidadeÉDireito",
                Assuntos = ["acessibilidade", "participação cidadã", "São Paulo"]
            });

        InstitutionalDemonstrationData.Configure(modelBuilder);
    }
}
