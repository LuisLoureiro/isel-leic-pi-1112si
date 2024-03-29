﻿using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;
using PI.WebGarten.Pipeline;

namespace HttpServer
{
    /// <summary>
    /// Verifica se tem header "Authentication": 
	///		se sim verifica se as credenciais estão correctas;
	///			se não 401;
	///			se sim cria GenericPrincipal, adiciona as respectivas Roles e passa ao próximo filtro;
	///		se não passa ao próximo filtro;
    /// </summary>
    public class AuthenticationFilter : IHttpFilter
    {
        private readonly string _name;
        private IHttpFilter _nextFilter;

        public AuthenticationFilter(String name)
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
            var auth = requestInfo.Context.Request.Headers["Authorization"];

            if (auth == null)
            {
                if (requestInfo.Context.Request.RawUrl.Equals("/login"))
                    return NotAuthorized();

                requestInfo.User = new GenericPrincipal(new GenericIdentity("", "Basic"), new[]{Roles.Anonimo});
            }
            else
            {
                auth = auth.Replace("Basic ", "");
                string[] userPasswd = Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(':');
                string user = userPasswd[0];
                string password = userPasswd[1];

                // Verificar se as credenciais fornecidas estão correctas
                try 
                {
                    if (!User.IsCorrect(user, password))
                        return NotAuthorized();
                }
                catch (InvalidOperationException e)
                {
                    return new HttpResponse(HttpStatusCode.Unauthorized, new TextContent(e.Message));
                }

                requestInfo.User = new GenericPrincipal(new GenericIdentity(user, "Basic"), User.GetRoles(user));
                
                if (requestInfo.Context.Request.RawUrl.Equals("/login"))
                    return new HttpResponse(HttpStatusCode.Redirect).WithHeader("Location", "/");
            }

            return _nextFilter.Process(requestInfo);
        }

        private static HttpResponse NotAuthorized()
        {
            var resp = new HttpResponse(HttpStatusCode.Unauthorized, new Handler.NotAuthorized());

            resp.WithHeader("WWW-Authenticate", "Basic realm=\"LI51N-G08\"");
            return resp;
        }
    }
}
