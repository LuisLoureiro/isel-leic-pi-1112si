using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using mvc.Models.Entities;
using mvc.Models.Mappers;
using mvc.Models.Repository;

namespace mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitRepositories();
        }

        protected void InitRepositories()
        {
            // 
            // Adicionar UCs em memória e criar os repositórios com os respectivos Mappers
            var ucRepo = RepositoryLocator.Get<string, CurricularUnit>();
            ucRepo.Add(new CurricularUnitMapper());

            var propRepo = RepositoryLocator.Get<long, Proposal>();
            propRepo.Add(new ProposalMapper());

            IEnumerable<CurricularUnit> ucs = GetUCs();
            ucRepo.Insert(ucs);

            propRepo.Insert(new Proposal(
                                new CurricularUnit("Matemática I", "M1", true, 1, 6)
                                                {
                                                   Assessment = "Avaliação Matemática",
                                                   Objectives = "Aprender Matemática",
                                                   Program = "Conceitos de Matemática",
                                                   Results = "Gostar de Matemática"
                                                }, 
                                                "1"));

        }

        private static IEnumerable<CurricularUnit> GetUCs()
        {
            CurricularUnit[] listUCs = new CurricularUnit[6];

            listUCs[0] = new CurricularUnit("Ambientes Virtuais de Execução", "AVE", true, (0x01 << 3), 6)
                             {
                                 Objectives =
                                     "Conhecer os requisitos e o funcionamento dos ambientes virtuais de execução. " +
                                     "Saber desenvolver aplicações para ambientes virtuais de execução.",
                                 Results =
                                     "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de: \n" +
                                     "1. Compreender os requisitos dos ambientes virtuais de execução no suporte à programação de " +
                                     "aplicações e componentes.\n" +
                                     "2. Realizar componentes e aplicações para ambientes virtuais de execução.\n" +
                                     "3. Compreender o funcionamento dos serviços disponibilizados pelos ambientes virtuais de execução.",
                                 Assessment =
                                     "Os resultados da aprendizagem (1) e (3) são avaliados através do teste escrito. " +
                                     "Trabalhos individuais realizados durante o semestre e discussão individual dos trabalhos a realizar " +
                                     "no final do semestre avaliam os resultados de aprendizagem de (1) a (3).",
                                 Program =
                                     "Problemas do código não controlado no que respeita a: robustez, interoperabilidade, portabilidade " +
                                     "e segurança. Caracterização e requisitos dos ambientes virtuais de execução. Construção de componentes: " +
                                     "representação intermédia; metadata; código seguro. Carregamento dinâmico de componentes: verificação; compilação JIT. " +
                                     "Sistema comum de tipos: tipos, objectos e valores. Serviços de suporte à execução: gestão automática de memória; " +
                                     "excepções; introspecção. Identificação e distribuição de componentes. Domínios de aplicação."
                             };

            listUCs[1] = new CurricularUnit("Redes de Computadores", "RCp", true, (0x01 << 3), 6)
                             {
                                 Objectives =
                                     "A disciplina tem como objectivo dotar os alunos com os conhecimentos base na área de redes " +
                                     "de computadores. Pretende-se que os alunos compreendam as tecnologias, arquitecturas e aplicações de redes " +
                                     "de computadores, com especial ênfase em redes locais e na família de protocolos TCP/IP.",
                                 Results =
                                     "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                                     "1. Compreender os princípios base de redes de computadores, i.e. conceitos fundamentais como a " +
                                     "organização em camadas, aspectos de comunicação de dados e protocolos teóricos.\n" +
                                     "2. Utilizar redes locais, com realce para a Ethernet, compreendendo os pormenores do seu funcionamento.\n" +
                                     "3. Explicar e utilizar a família de protocolos TCP/IP, com especial ênfase no protocolo de rede IP e nos " +
                                     "protocolos de transporte TCP e UDP, endereçamento, sistema de nomes de domínios, atribuição dinâmica de " +
                                     "endereços, protocolos de transferência de ficheiros, correio electrónico.",
                                 Assessment =
                                     "Os resultados de aprendizagem são avaliados individualmente através de testes escritos e de " +
                                     "fichas realizadas durante o semestre. Durante o acompanhamento dos trabalhos de grupo realizados nas aulas " +
                                     "práticas são avaliados os resultados de aprendizagem.",
                                 Program =
                                     "Comunicações digitais. Modelo OSI. Comunicação ponto a ponto e multiponto. Redes de comunicação " +
                                     "de dados: canais de transmissão, transmissão série e paralela, sincronismo, erros de transmissão e topologias " +
                                     "de rede. Protocolos teóricos de recuperação de erros e controlo de fluxo.Introdução às redes locais, ênfase na " +
                                     "rede Ethernet, normas 802.x, controlo de acesso ao meio, encapsulamento. Conceitos sobre interligação de redes, " +
                                     "especialmente sobre repetidores e comutadores.Família de protocolos TCP/IP. Modelo TCP/IP (vs. OSI). Protocolo IP," +
                                     " formato dos datagramas, fragmentação, encaminhamento, máscaras de rede, protocolo controlo de erros ICMP. Protocolo" +
                                     " UDP e TCP, conceito de ligação, controlo de fluxo e de congestão. Nível de aplicação: resolução de nomes, " +
                                     "transferência e acesso a ficheiros, correio electrónico, atribuição dinâmica de endereços."
                             };

            listUCs[2] = new CurricularUnit("Programação na Internet", "PI", true, (0x01 << 4), 6)
                             {
                                 Objectives =
                                     "Esta unidade curricular representa um dos pontos de consolidação e interligação de diversos temas " +
                                     "abordados em unidades curriculares anteriores, concretizadas no desenvolvimento de aplicações Web." +
                                     "Tem como objectivos fornecer as competências necessárias à realização de aplicações Web de pequena e média complexidade," +
                                     " enfatizando os aspectos de geração, distribuição e actualização da interface com o utilizador.",
                                 Results =
                                     "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                                     "1. Identificar os principais elementos constituintes de aplicações Web com interface gráfica para Web browser.\n" +
                                     "2. Conhecer e utilizar as principais normas associados à componente de cliente (browser).\n" +
                                     "3. Compreender o problema da manutenção de estado em aplicações Web e distinguir os diversos tipos soluções existentes.\n" +
                                     "4. Compreender, utilizar e estender uma tecnologia de servidor, com significativa adopção industrial, para criação de" +
                                     " aplicações Web.\n" +
                                     "5. Implementar aplicações Web de pequena e média complexidade, que incluem funcionalidades de autenticação, " +
                                     "manutenção de estado de conversação, visualização e edição de dados.",
                                 Assessment =
                                     "Os resultados da aprendizagem (1), (2), (3) e (4) são avaliados individualmente através de teste escrito." +
                                     "Durante as aulas práticas e na discussão final dos trabalhos práticos, realizada em grupo, são avaliados os resultados" +
                                     " da aprendizagem (2), (4) e (5).",
                                 Program =
                                     "A arquitectura da World Wide Web. Identificação de recursos, sintaxe e semântica de URLs. " +
                                     "Distribuição de conteúdos web (protocolo HTTP). Infra-estruturas de suporte à criação da interface com o utilizador " +
                                     "em aplicações web e respectivo modelo de programação. Componente de cliente (browser): descrição, formatação visual, " +
                                     "manipulação programática e actualizações totais e parciais da interface gráfica. Componente de servidor: distribuição " +
                                     "de conteúdos estáticos; geração dinâmica de conteúdos; modelo de programação no servidor; manutenção de estado " +
                                     "(de visualização, de sessão e de aplicação); intercepção de pedidos. Consequências da distribuição no modelo de programação. " +
                                     "Mecanismos de cache. Concretização dos temas estudados através do desenvolvimento de aplicações web com suporte de dados " +
                                     "XML e/ou bases de dados relacionais."
                             };
            listUCs[2].AddPrecedence(listUCs[0]); listUCs[2].AddPrecedence(listUCs[1]);

            listUCs[3] = new CurricularUnit("Álgebra Linear e Geometria Analítica", "ALGA", true, (0x01 << 0), 6)
                             {
                                 Objectives =
                                     "Nesta unidade curricular são abordados temas de Álgebra Linear e Geometria Analítica, em particular, matrizes, sistemas de equações lineares, determinantes, espaços vectoriais, aplicações lineares, valores e vectores próprios, espaços euclidianos e geometria analítica do 1º e 2º graus. Pretende-se com este conteúdo programático - que corresponde ao currículo normal das Cadeiras com esta temática - cobrir importantes áreas da Matemática necessárias não só para as Cadeiras de Matemática a juzante desta mas também para várias Cadeiras de Engenharia",
                                 Results =
                                     "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                                     "1. Efectuar cálculos com matrizes e determinantes.\n" +
                                     "2. Discutir e resolver sistemas de equações lineares, pelo algoritmo de Gauss-Jordan.\n" +
                                     "3. Reconhecer os conceitos de espaço vectorial e aplicação linear e utilizá-los na resolução de problemas destes domínios.\n" +
                                     "4. Determinar valores e vectores próprios dos endomorfismos e a existência de uma representação diagonal, reconhecendo a sua importância em problemas de Engenharia e de Física.\n" +
                                     "5. Apreender os conceitos fundamentais dos espaços euclidianos e unitários, nomeadamente os produtos interno, externo, misto e duplo produto externo e respectivas aplicações em Geometria" +
                                     "6. Compreender a noção de espaço afim e de referencial e aplicar os conceitos abordados nesta unidade curricular na resolução de problemas de geometria analítica do 1º (plano e recta) e 2º grau (quádricas)" +
                                     "7. Identificar e utilizar os conceitos abordados na resolução de problemas de Engenharia no âmbito de cada Curso",
                                 Assessment =
                                     "Os resultados da aprendizagem são avaliados por questionários realizados durante o Semestre e por dois testes ou, em alternativa, por exame final. As classificações obtidas nos questionários propostos, quando realizados, constituem 10% da avaliação final. Caso não sejam realizados os questionários propostos, a classificação final é a média aritmética das notas dos testes ou a nota do exame final (numa escala 0-20)",
                                 Program =
                                     "0.	Noções introdutórias de Álgebra 1.	Matrizes 2.	Determinantes 3.	Espaços vectoriais 4.	Aplicações Lineares 5. Valores e vectores próprios 6.	Espaços euclidianos 7.	Geometria analítica do 1º e 2º grau"
                             };
            listUCs[4] = new CurricularUnit("Comunicações", "Com", true, (0x01 << 2), 6)
            {
                Objectives =
                    "Dotar os alunos de conhecimentos das técnicas usadas na comunicação digital. Tratamento das modulações utilizadas - codificação de canal estrita. Análise do comportamento do canal através de modelos probabilísticos. Tratamento dos mecanismos de detecção e correcção de erros e adaptação destes em função da qualidade de serviço.",
                Results =
                    "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                    "1. Dominar os conceitos de espectro, largura de banda, filtragem e de probabilidade de erro de transmissão.\n" +
                    "2. Interpretar e avaliar o funcionamento de sistemas de comunicação digital em banda base e banda canal.\n" +
                    "3. Formular as características de um sistema de comunicação digital, em função da qualidade e do tipo de serviço.\n" +
                    "4. Escolher e adaptar códigos detectores e correctores de erros para determinada aplicação.\n",
                Assessment =
                    "Os resultados da aprendizagem (1)(2)(3)(4) são avaliados individualmente através de avaliação escrita e de discussão individual dos trabalhos práticos. A avaliação escrita é realizada, em alternativa, através de dois testes parciais ou de um teste global. A avaliação prática consiste na realização de trabalhos práticos de laboratório e respectiva discussão individual.",
                Program =
                    "Conceitos básicos de sinal e sistema. Espectro, largura de banda e resposta em frequência. Digitalização de sinal: amostragem e quantização. Ritmo de transmissão e capacidade de canal. Teorema da codificação de canal. Modulações digitais em banda base e banda canal. Modulações binárias e M-árias. Análise do seu funcionamento na presença de ruído. Uso de modelos probabilísticos para caracterização de canal. Probabilidade de erro e qualidade de serviço. Codificação de canal. Códigos detectores e correctores de erros. Códigos lineares de bloco. Códigos de Hamming. Códigos cíclicos e CRC (Cyclic Redundancy Check)."
            };

            listUCs[1].AddPrecedence(listUCs[4]);

            listUCs[5] = new CurricularUnit("Segurança Informática", "SI", false, (0x01 << 4), 6)
            {
                Objectives =
                    "Realizar uma introdução à teoria e prática da segurança informática, apresentando os principais tipos de ameaças à segurança de sistemas informáticos e descrevendo as técnicas existentes para a sua minimização. Fornecer a base conceptual e prática necessária ao estudo dos aspectos de segurança em contextos específicos, nomeadamente: sistemas distribuídos; sistemas de informação e redes de comunicação de dados.",
                Results =
                    "Os estudantes que terminam com sucesso esta unidade curricular serão capazes de:\n" +
                    "1. Compreender os principais tipos de ameaças à segurança de sistemas informáticos.\n" +
                    "2. Compreender, escolher e utilizar mecanismos e protocolos criptográficos, incluindo os aspectos da gestão de chaves.\n" +
                    "3. Compreender, escolher e utilizar modelos e mecanismos para o controlo de acesso.\n" +
                    "4. Identificar vulnerabilidades existentes em programas e usar técnicas adequadas à sua correcção.\n",
                Assessment =
                    "Os resultados da aprendizagem são avaliados individualmente através de teste escrito e de discussão final dos trabalhos práticos.",
                Program =
                    "Esquemas e protocolos criptográficos e métodos de gestão de chaves: esquemas de cifra simétrica e assimétrica, esquemas MAC e de assinatura digital; protocolos e autenticação e estabelecimento de chaves; infra-estruturas de chave pública. Modelos e mecanismos para controlo de acessos: monitor de referências e \"Trusted Computer Base\";modelos e mecanismos de autorização clássicos (matriz de controlo de acessos, listas de controlo de acessos e \"capabilities\", modelos baseados em reticulados e modelo de Clark-Wilson); modelos RBAC (\"Role Based Access Control\"); modelos e mecanismos baseados na identidade do código; modelos baseados no paradigma \"Trust Management\". Escrita de código seguro: vulnerabilidades típicas, técnicas de protecção e metodologias de desenho e desenvolvimento."
            };

            return listUCs;
        }
    }
}