using System.Collections.Generic;
using System.Linq;
using HttpServer.Controller;
using HttpServer.Model.Entities;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    public class FucsView : HtmlDoc
    {
        private CurricularUnit fuc;

        public FucsView(IEnumerable<CurricularUnit> fucs)
            : base("Lista de Fucs",
                   H1(Text("Lista de Fichas de Unidades Curriculares")),
                   Ul(
                       fucs.Select(fuc => Li(A(ResolveUri.For(fuc), fuc.Name))).ToArray()
                       )
                )
        {
        }

        public FucsView(CurricularUnit fuc)
            : base(fuc.Name,
                    H1(Text(fuc.Name + " (" + fuc.Key + ")")),
                    H2(Text("Caracterização da Unidade Curricular")),
                    Ul(
                        Li(Text(string.Format("Nome: {0}", fuc.Name))),
                        Li(Text(string.Format("Enquadramento: {0}, {1}", fuc.Mandatory ? "Obrigatória" : "Opcional", fuc.Semester))),
                        Li(Text(string.Format("Créditos: {0}", fuc.Ects))),
                        Li(Text("Pré Requisitos: ")),
                            Ul(fuc.Precedence.Select(f => Li(A(ResolveUri.For(f), f.Key))).ToArray())
                    ),
                    H2(Text("Objectvios")),
                    P(Text(fuc.Objectives)),
                    H2(Text("Resultados da Aprendizagem")),
                    P(Text(fuc.Results)),
                    H2(Text("Avaliação dos Resultados da Aprendizagem")),
                    P(Text(fuc.Assessment)),
                    H2(Text("Programa Resumido")),
                    P(Text(fuc.Program))
                )
        {
        }
    }
}