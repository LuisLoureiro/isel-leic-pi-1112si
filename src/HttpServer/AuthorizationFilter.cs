using System;
using System.Net;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;
using PI.WebGarten.Pipeline;

namespace HttpServer
{
    /// <summary>
    /// Verifica se, para o recurso pedido, o utilizador tem as permissões necessárias.
    /// </summary>
    public class AuthorizationFilter : IHttpFilter
    {
        private readonly string _name;
        private IHttpFilter _nextFilter;

        public AuthorizationFilter(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public void SetNextFilter(IHttpFilter nextFilter)
        {
            _nextFilter = nextFilter;
        }

        public HttpResponse Process(RequestInfo requestInfo)
        {
            var principal = requestInfo.User;
            string path = requestInfo.Context.Request.RawUrl;

            // Verificar os recursos exclusivos de utilizadores autenticados.
            if ((path.Contains("props") || path.Contains("edit") || path.Contains("new")) && principal.IsInRole(Roles.Anonimo))
                return Forbidden();

            // Verificar os recursos exclusivos de utilizadores coordenadores.
            if ((path.Contains("props") && path.Contains("edit")) && principal.IsInRole(Roles.Coordenador))
                return Forbidden();

            // Verificar os recursos exclusivos de utilizadores não coordenadores.
            if (path.Contains("accept") && principal.IsInRole(Roles.Utilizador))
                return Forbidden();

            // Se passou nas verificações anteriores significa que pode aceder ao recurso.

            return _nextFilter.Process(requestInfo);
        }

        private static HttpResponse Forbidden()
        {
            return new HttpResponse(HttpStatusCode.Forbidden, new TextContent("Access Forbidden"));
        }
    }
}
