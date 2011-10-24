using System.Net;
using System.Security.Principal;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;

namespace HttpServer.Controller
{
    public class RootController
    {
        [HttpCmd(HttpMethod.Get, "/")]
        public HttpResponse Get(IPrincipal principal)
        {
            return new HttpResponse(HttpStatusCode.OK, new RootView(principal));
        }

        [HttpCmd(HttpMethod.Get, "/login")]
        public HttpResponse Login()
        {
            return new HttpResponse(HttpStatusCode.Unauthorized)
                        .WithHeader("WWW-Authenticate", "Basic realm=\"LI51N-G08\"");
        }
    }
}