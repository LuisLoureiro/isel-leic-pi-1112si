﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using HttpServer.Controller;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    public class ProposalView : HtmlDoc
    {
        public ProposalView(IEnumerable<Proposal> props)
            : base("Lista de propostas", 
                    H1(Text("Lista de Propostas a Unidades Curriculares")),
                    Ul(
                       props.Select(prop => Li(A(ResolveUri.For(prop), string.Format("Id: {0}; UC: {1}",prop.Key.ToString(), prop.Info.Name)))).ToArray()
                    ),
                    A(ResolveUri.ForRoot(), "Página Inicial")
                )
        {}

        public ProposalView(Proposal proposal, IPrincipal principal) 
            : base("Proposta " + proposal.Key, new FucsView(proposal.Info),
                    Form(HttpMethod.Post, ResolveUri.ForProposalCancel(proposal.Key), InputSubmit("Cancelar Proposta")),
                    principal.IsInRole(Roles.Coordenador) 
                        ? Form(HttpMethod.Post, ResolveUri.ForProposalAccept(proposal.Key), InputSubmit("Aceitar Proposta"))
                        : Text(""),
                    proposal.Owner.Equals(principal.Identity.Name) ? 
                        Form(HttpMethod.Get, ResolveUri.ForEdit(proposal), InputSubmit("Editar Proposta"))
                        : Text("")
            )
        {}

        public ProposalView(CurricularUnit fuc, string action)
            : base("Edição - " + fuc.Name,
                    Form(HttpMethod.Post, action, 
                        Fieldset(
                            Legend(string.Format("Editar {0}", fuc.Name)),
                            Div("clearfix",Label("Nome: "), Div("input", InputText("name", fuc.Name))),
                            Div("clearfix",Label("Acrónimo: "), Div("input", (InputText("acr", fuc.Key) as HtmlElem).WithAttr("readonly","readonly"))),
                            Div("clearfix",Label("ECTS: "), Div("input", InputText("ects", fuc.Ects.ToString()))),
                            Div("clearfix",Label("Aprendizagem: "), Div("input", InputText("assessment", fuc.Assessment))),
                            Div("clearfix",Label("Resultados: "),Div("input",InputText("results", fuc.Results))),
                            Div("clearfix",Label("Objectivo: "),Div("input",InputText("objectives", fuc.Objectives))),
                            Div("clearfix",Label("Programa: "),Div("input",InputText("program", fuc.Program))),
                            Div("clearfix",Label("Obrigatoriedade: "),GenerateMandatoryRadioButtons(fuc)),
                            Div("clearfix",Label("Semestre(s): "),GenerateSemesterCheckBoxes(fuc)),
                            Div("clearfix",Label("Pré Requisito(s): "), GeneratePrecedencesCheckBoxes(fuc)),
                            Div("clearfix",Div("input", InputSubmit("Submeter")))
                        )
                    )
            )
        {}

        public ProposalView()
        : base("Criar nova Unidade Curricular", 
                Form(HttpMethod.Post, ResolveUri.ForNewFuc(), 
                    Fieldset(
                        Legend("Editar"),
                        Div("clearfix",Label("Nome: "), Div("input", InputText("name"))),
                        Div("clearfix",Label("Acrónimo: "), Div("input", InputText("acr"))),
                        Div("clearfix",Label("ECTS: "), Div("input", InputText("ects"))),
                        Div("clearfix",Label("Aprendizagem: "),Div("input",InputText("assessment"))),
                        Div("clearfix",Label("Resultados: "), Div("input", InputText("results"))),
                        Div("clearfix",Label("Objectivo: "), Div("input", InputText("objectives"))),
                        Div("clearfix",Label("Programa: "), Div("input", InputText("program"))),
                        Div("clearfix",Label("Obrigatoriedade: "), GenerateMandatoryRadioButtons(null)),
                        Div("clearfix", Label("Semestre(s): "), GenerateSemesterCheckBoxes(null)),
                        Div("clearfix",Label("Pré Requisito(s): "), GeneratePrecedencesCheckBoxes(null)),
                        Div("clearfix",Div("input", InputSubmit("Submeter")))
                    )
                )
            )
        {}

        private static IWritable GenerateSemesterCheckBoxes(CurricularUnit fuc)
        {
            var quant = CurricularUnit.Maxsemesters;

            var ret = new IWritable[quant];

            var semester = fuc == null ? 0 : fuc.Semester;

            for (int i = 0; i != quant; i++)
                ret[i] = Li(InputCheckBox("" + i, "" + i, (semester & (0x01 << i)) == (0x01 << i)), Text(string.Format(" {0}º Semestre", i+1)) );

            return Div("input", Ul("inputs-list", ret));
        }

        private static IWritable GenerateMandatoryRadioButtons(CurricularUnit fuc)
        {
            var ret = new IWritable[2];

            ret[0] = Li(InputRadioButton("tipoObrig", "obrigatoria", (fuc == null ? true : fuc.Mandatory)), Text("Obrigatória"));
            ret[1] = Li(InputRadioButton("tipoObrig", "opcional", (fuc == null ? false : !fuc.Mandatory)), Text("Opcional"));

            return Div("input", Ul("inputs-list", ret));
        }

        private static IWritable GeneratePrecedencesCheckBoxes(CurricularUnit uc)
        {
            var ucs = RepositoryLocator.Get<string, CurricularUnit>().GetAll();

            if (uc != null)
                ucs = ucs.Where(c => !c.Key.Equals(uc.Key)); //Para não considerar a própria UC

            var ret = new IWritable[ucs.Count()];

            int i = 0;
            foreach(var cUnit in ucs)
                ret[i++] = Li(InputCheckBox(cUnit.Key, cUnit.Key, (uc != null && uc.Precedence.Contains(cUnit))), Text(string.Format(" {0}", cUnit.Key)));
            
            return Div("input", Ul("inputs-list", ret));
        }
    }
}
