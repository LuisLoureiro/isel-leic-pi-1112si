using System.Collections;
using HttpServer.Controller;
using HttpServer.Model.Entities;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    public class ProposalView : HtmlDoc
    {
        public ProposalView(CurricularUnit fuc)
            : base("Edição - " + fuc.Name,
                    Form(HttpMethod.Post, ResolveUri.For(fuc) + "/edit", 
                        Fieldset(
                            Legend(string.Format("Editar {0}", fuc.Name)),
                            Div("clearfix",
                                Label("Nome: "), 
                                Div("input", 
                                    InputText("name", fuc.Name)
                                )
                            ),
                            Div("clearfix",
                                Label("Acrónimo: "), 
                                Div("input", 
                                    InputText("acr", fuc.Key)
                                )
                            ),
                            Div("clearfix",
                                Label("ECTS: "), 
                                Div("input", 
                                    InputText("ects", fuc.Ects.ToString())
                                )
                            ),
                            Div("clearfix",
                                Label("Aprendizagem: "),
                                Div("input",
                                    InputText("ects", fuc.Assessment)
                                )
                            ),
                            Div("clearfix",
                                Label("Resultados: "),
                                Div("input",
                                    InputText("ects", fuc.Results)
                                )
                            ),
                            Div("clearfix",
                                Label("Objectivo: "),
                                Div("input",
                                    InputText("ects", fuc.Objectives)
                                )
                            ),
                            Div("clearfix",
                                Label("Programa: "),
                                Div("input",
                                    InputText("ects", fuc.Program)
                                )
                            ),
                            Div("clearfix",
                                Label("Programa: "),
                                Div("input",
                                    Ul("inputs-list", GenerateMandatoryRadioButtons(fuc))
                                )
                            ),
                            Div("clearfix",
                                Label("Semestre(s): "),
                                Div("input",
                                    Ul("inputs-list", GenerateSemesterCheckBoxes(fuc))
                                )
                            )
                        )
                    )
            )
        {
        }

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

            ret[0] = Li(InputRadioButton("tipoObrig", "obrigatoria", fuc.Mandatory), Text("Obrigatória"));
            ret[1] = Li(InputRadioButton("tipoObrig", "opcional", !fuc.Mandatory), Text("Opcional"));

            return ret;
        }
    }
}
