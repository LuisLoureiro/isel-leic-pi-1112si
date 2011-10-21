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
                        P(Text("Nome: "), InputText("name")),
                        P(Text("Acrónimo: "), InputText("acr")),
                        P(Text("ECTS: "), InputText("ects"))
                    )
            )
        {
        }
    }
}
