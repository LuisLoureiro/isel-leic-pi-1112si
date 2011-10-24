using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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
        public ProposalView(CurricularUnit fuc, string action)
            : base("Edição - " + fuc.Name,
                    Form(HttpMethod.Post, action, 
                        Fieldset(
                            Legend(string.Format("Editar {0}", fuc.Name)),
                            Div("clearfix",Label("Nome: "), Div("input", InputText("name", fuc.Name))),
                            Div("clearfix",Label("Acrónimo: "), Div("input", InputText("acr", fuc.Key))),
                            Div("clearfix",Label("ECTS: "), Div("input", InputText("ects", fuc.Ects.ToString()))),
                            Div("clearfix",Label("Aprendizagem: "), Div("input", InputText("assessment", fuc.Assessment))),
                            Div("clearfix",Label("Resultados: "),Div("input",InputText("results", fuc.Results))),
                            Div("clearfix",Label("Objectivo: "),Div("input",InputText("objectives", fuc.Objectives))),
                            Div("clearfix",Label("Programa: "),Div("input",InputText("program", fuc.Program))),
                            Div("clearfix",Label("Obrigatoriedade: "),Div("input",Ul("inputs-list", GenerateMandatoryRadioButtons(fuc)))),
                            Div("clearfix",Label("Semestre(s): "),Div("input",Ul("inputs-list", GenerateSemesterCheckBoxes(fuc)))),
                            // TODO quando é feito o submit esta informação não está a ir.
                            Div("clearfix",Label("Pré Requisito(s): "), Div("input", MultiSelect(4, GeneratePrecedencesListBox(fuc)))),
                            Div("clearfix",Div("input", InputSubmit("Submeter")))
                        )
                    )
            )
        {}

        public ProposalView()
        : base("Criar nova Unidade Curricular", 
                Form(HttpMethod.Post, ResolveUri.ForFucs() + "/new", 
                    Fieldset(
                        Legend("Editar"),
                        Div("clearfix",Label("Nome: "), Div("input", InputText("name"))),
                        Div("clearfix",Label("Acrónimo: "), Div("input", InputText("acr"))),
                        Div("clearfix",Label("ECTS: "), Div("input", InputText("ects"))),
                        Div("clearfix",Label("Aprendizagem: "),Div("input",InputText("assessment"))),
                        Div("clearfix",Label("Resultados: "), Div("input", InputText("results"))),
                        Div("clearfix",Label("Objectivo: "), Div("input", InputText("objectives"))),
                        Div("clearfix",Label("Programa: "), Div("input", InputText("program"))),
                        Div("clearfix",Label("Obrigatoriedade: "),Div("input",Ul("inputs-list", GenerateMandatoryRadioButtons(null)))),
                        Div("clearfix", Label("Semestre(s): "), Div("input", Ul("inputs-list"))),
                        Div("clearfix", Label("Pré Requisito(s): "), Div("input", MultiSelect(4, GeneratePrecedencesListBox(null)))),
                        Div("clearfix",Div("input", InputSubmit("Submeter")))
                    )
                )
            )
        {}

        private static IWritable[] GenerateSemesterCheckBoxes(CurricularUnit fuc)
        {
            var quant = CurricularUnit.Maxsemesters;

            var ret = new IWritable[quant];

            for (int i = 0; i < quant; i++)
                ret[i] = Li(InputCheckBox("" + i, "" + i, ((fuc.Semester) & (0x01 << i)) == (0x01 << i)), Text(string.Format(" {0}º Semestre", i+1)) );

            return ret;
        }

        private static IWritable[] GenerateMandatoryRadioButtons(CurricularUnit fuc)
        {
            var ret = new IWritable[2];

            ret[0] = Li(InputRadioButton("tipoObrig", "obrigatoria", (fuc == null ? true : fuc.Mandatory)), Text("Obrigatória"));
            ret[1] = Li(InputRadioButton("tipoObrig", "opcional", (fuc == null ? false : !fuc.Mandatory)), Text("Opcional"));

            return ret;
        }

        private static IWritable[] GeneratePrecedencesListBox(CurricularUnit uc)
        {
            var options = new List<IWritable>();
            var ucs = RepositoryLocator.Get<string, CurricularUnit>().GetAll();

            if (uc != null)
                ucs = ucs.Where(c => !c.Key.Equals(uc.Key));

            foreach(var cUnit in ucs)
            {
                var opt = Option(cUnit.Key, cUnit.Key) as HtmlElem;
                if (opt == null) 
                    throw new ServerException("Excepção gerada ao efectuar um cast de IWritable para HtmlElem sobre o elemento Option.");

                if (uc != null && uc.Precedence.Contains(cUnit))
                    opt.WithAttr("selected", "selected");

                options.Add(opt);
            }

            return options.ToArray();
        }
    }
}
