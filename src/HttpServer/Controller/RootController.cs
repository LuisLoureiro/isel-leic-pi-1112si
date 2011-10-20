using System.Collections.Generic;
using System.Net;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;

namespace HttpServer.Controller
{
    public class RootController
    {
        [HttpCmd(HttpMethod.Get, "/")]
        public HttpResponse Get()
        {
            return new HttpResponse(HttpStatusCode.OK, new RootView());
        }

        [HttpCmd(HttpMethod.Get, "/login")]
        public HttpResponse Login()
        {
            return new HttpResponse(HttpStatusCode.Unauthorized, new RootView()).WithHeader("WWW-Authenticate", "Basic");
            //O filtro deve verificar a resposta a este pedido, 
            //se as credencias estiverem correctas responde com o codigo 301 e adiciona o cabeçalho Location : "/"
        }
    }
}