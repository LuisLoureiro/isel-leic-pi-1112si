using System.Security.Principal;
using HttpServer.Controller;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    class RootView : HtmlDoc
    {
        public RootView(IPrincipal principal) 
            : base("LI51N-G08", 
                    H1(Text(string.Format("Bem vindo{0}!", principal.Identity.IsAuthenticated ? " "+principal.Identity.Name : ""))),
                    Ul(
                        Li(A(ResolveUri.ForFucs(), "Fichas das Unidades Curriculares")),
                        Li(A(ResolveUri.ForLogin(), "Login")),
                        principal.Identity.IsAuthenticated ? 
                            Li(A(ResolveUri.ForProposals(), principal.IsInRole(Roles.Utilizador) ? 
                                                                "Propostas criadas por si" : 
                                                                "Todas as propostas")) :
                            Br()
                    )
            )
        {
        }
    }
}
