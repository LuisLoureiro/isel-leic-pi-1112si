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
    }
}