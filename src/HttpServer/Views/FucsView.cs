using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using HttpServer.Controller;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    public class FucsView : HtmlDoc
    {
        public FucsView(IEnumerable<CurricularUnit> fucs)
            : base("Lista de Fucs",
                   H1(Text("Lista de Fichas de Unidades Curriculares")),
                   Ul(
                       fucs.Select(fuc => Li(A(ResolveUri.For(fuc), fuc.Name))).ToArray()
                       ),
                    A(ResolveUri.ForRoot(), "Página Inicial")
                )
        {}

        public FucsView(CurricularUnit fuc, IPrincipal principal)
            : base(fuc.Name,
                   H1(Text(fuc.Name + " (" + fuc.Key + ")")),
                   H2(Text("Caracterização da Unidade Curricular")),
                   Ul(
                       Li(Text(string.Format("Nome: {0}", fuc.Name))),
                       Li(
                           Text(string.Format("Enquadramento: {0}, Semestres: {1}",
                                              fuc.Mandatory ? "Obrigatória" : "Opcional", fuc.SemesterToText()))),
                       Li(Text(string.Format("Créditos: {0}", fuc.Ects))),
                       Li(Text("Pré Requisitos: ")),
                       Ul(fuc.Precedence.Select(f => Li(A(ResolveUri.For(f), f.Key))).ToArray())
                       ),
                   H2(Text("Objectivos")),
                   P(Text(fuc.Objectives)),
                   H2(Text("Resultados da Aprendizagem")),
                   P(Text(fuc.Results)),
                   H2(Text("Avaliação dos Resultados da Aprendizagem")),
                   P(Text(fuc.Assessment)),
                   H2(Text("Programa Resumido")),
                   P(Text(fuc.Program)),
                   GetLinks(fuc, principal), Br(),
                   A(ResolveUri.ForFucs(), "Voltar à Listagem")
                )
        {}

        private static IWritable GetLinks(CurricularUnit fuc, IPrincipal principal)
        {
            if (principal == null || principal.IsInRole(Roles.Anonimo))
                return Br();

            var prop = RepositoryLocator.Get<long, Proposal>().GetAll()
                        .Where(p => p.Owner.Equals(principal.Identity.Name))
                        .FirstOrDefault(p => p.Info.Key.Equals(fuc.Key));

            return (prop == null ? A(ResolveUri.ForEdit(fuc), "Criar proposta") : A(ResolveUri.For(prop), "Editar Proposta"));
        }
    }
}