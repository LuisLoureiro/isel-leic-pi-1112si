using System;
using System.Net;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;
using PI.WebGarten.Pipeline;

namespace HttpServer
{
    /// <summary>
    /// Verifica se o recurso pedido precisa de Roles:
	///		se sim executa IsInRole do User de RequestInfo passando a Role;
	///			se não 403 Forbidden;
	///			se sim passa ao próximo filtro;
	///		se não passa ao próximo filtro;
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

            if (principal != null)
            {
                string path = requestInfo.Context.Request.Url.AbsolutePath;

                // Verificar os recursos exclusivos de utilizador coordenador.
                if (path.EndsWith("accept") && !principal.IsInRole("coordenador"))
                    return Forbidden();

                // Verificar os recursos exclusivos de utilizador não coordenador.
                if (path.Contains("prop") && path.EndsWith("edit") && !principal.IsInRole("utilizador"))
                    return Forbidden();

                // Se passou nas verificações anteriores significa que pode aceder a qualquer recurso.
            }

            return _nextFilter.Process(requestInfo);
        }

        private static HttpResponse Forbidden()
        {
            return new HttpResponse(HttpStatusCode.Forbidden, new TextContent("Access Forbidden"));
        }
    }
}
