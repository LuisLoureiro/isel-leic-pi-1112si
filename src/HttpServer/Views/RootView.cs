using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Controller;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;

namespace HttpServer.Views
{
    class RootView : HtmlDoc
    {
        public RootView() 
            : base("LI51N-G08", 
                    H1(Text("Bem vindo!")),
                    Ul(
                        Li(A(ResolveUri.ForFucs(), "Fichas das Unidades Curriculares"))
                    )
            )
        {
        }
    }
}
