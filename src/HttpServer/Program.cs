using System;
using HttpServer.Controller;
using HttpServer.Model;
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
            //var repo = RepositoryLocator.Get();

            //Repositório forçado para prosseguir com o projecto
            IRepository<string, CurricularUnit> repo = new Repository<string, CurricularUnit>();

            repo.Add(typeof (CurricularUnit), new CurricularUnitMapper());
            //repo.Add<UInt32, Proposal>(typeof (Proposal), new ProposalMapper());

            //repo.Insert(new CurricularUnit("UC TESTE", "UCT", false, 0x01, 6));
        }
    }
}