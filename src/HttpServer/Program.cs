using HttpServer.Controller;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;
using HttpServer.Model.Repository;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;
using System.Diagnostics;

namespace HttpServer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Init();

            var host = new HttpListenerBasedHost("http://localhost:8080/");

            host.Add(DefaultMethodBasedCommandFactory.GetCommandsFor(
                typeof (RootController),
                typeof (FucController)));

            host.OpenAndWaitForever();
        }

        private static void Init()
        {
            var ucRepo = RepositoryLocator.Get<string, CurricularUnit>();

            ucRepo.Add(typeof(CurricularUnit), new CurricularUnitMapper());

            CurricularUnit uc = new CurricularUnit("Programação na Internet", "PI", true, (0x01 << 5), 6);// Faltam os pré-requisitos
            uc.Objectives = "Esta unidade curricular representa um dos pontos de consolidação e interligação de diversos temas "+
                "abordados em unidades curriculares anteriores, concretizadas no desenvolvimento de aplicações Web."+
                "Tem como objectivos fornecer as competências necessárias à realização de aplicações Web de pequena e média complexidade,"+
                " enfatizando os aspectos de geração, distribuição e actualização da interface com o utilizador.";
            uc.Results = "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                "1. Identificar os principais elementos constituintes de aplicações Web com interface gráfica para Web browser.\n" +
                "2. Conhecer e utilizar as principais normas associados à componente de cliente (browser).\n" +
                "3. Compreender o problema da manutenção de estado em aplicações Web e distinguir os diversos tipos soluções existentes.\n" +
                "4. Compreender, utilizar e estender uma tecnologia de servidor, com significativa adopção industrial, para criação de"+
                " aplicações Web.\n" +
                "5. Implementar aplicações Web de pequena e média complexidade, que incluem funcionalidades de autenticação, " +
                "manutenção de estado de conversação, visualização e edição de dados.";
            uc.Assessment = "Os resultados da aprendizagem (1), (2), (3) e (4) são avaliados individualmente através de teste escrito." +
                "Durante as aulas práticas e na discussão final dos trabalhos práticos, realizada em grupo, são avaliados os resultados" +
                " da aprendizagem (2), (4) e (5).";
            uc.Program = "A arquitectura da World Wide Web. Identificação de recursos, sintaxe e semântica de URLs. "+
                "Distribuição de conteúdos web (protocolo HTTP). Infra-estruturas de suporte à criação da interface com o utilizador "+
                "em aplicações web e respectivo modelo de programação. Componente de cliente (browser): descrição, formatação visual, "+
                "manipulação programática e actualizações totais e parciais da interface gráfica. Componente de servidor: distribuição "+
                "de conteúdos estáticos; geração dinâmica de conteúdos; modelo de programação no servidor; manutenção de estado "+
                "(de visualização, de sessão e de aplicação); intercepção de pedidos. Consequências da distribuição no modelo de programação. "+
                "Mecanismos de cache. Concretização dos temas estudados através do desenvolvimento de aplicações web com suporte de dados "+
                "XML e/ou bases de dados relacionais.";
            ucRepo.Insert(uc);
        }
    }
}