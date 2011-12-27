using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Security;
using mvc.Models;

namespace mvc.Modules
{
    /// <summary>
    /// Classe respons�vel por fazer a gest�o dos cookies.
    /// Implementa alguns m�todos que a classe FormsAuthentication disponibiliza.
    /// </summary>
    public class AppFormsAuthentication
    {
        private const string COOKIE_KEY = "AppAuthentication.Cookie";
        private const string LOGINPAGE_KEY = "AppAuthentication.LoginPage";
        
        public static void SignOut()
        {
            string cookieName = ConfigurationManager.AppSettings[COOKIE_KEY];
            if (cookieName == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", COOKIE_KEY));

            // Esta ac��o apenas retira o cookie da colec��o no lado do servidor!
            HttpContext.Current.Request.Cookies.Remove(cookieName);
            // Para retirar no lado do cliente, altera-se a data de expira��o para uma data anterior � actual.
            // Esta ac��o faz com que o browser, ao verificar que o cookie j� expirou, o elimine e n�o o volte a usar.
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookieName)
                                                         {
                                                             Expires = DateTime.Now.AddDays(-1d)
                                                         });
        }

        public static void RedirectToLoginPage()
        {
            string loginPage = ConfigurationManager.AppSettings[LOGINPAGE_KEY];
            if (loginPage == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", LOGINPAGE_KEY));
            
            HttpContext.Current.Response.Redirect(loginPage);
        }

        public static void SetAuthCookie(string username, bool persistentCookie)
        {
            
        }
    }

    public class AppAuthentication : IHttpModule
    {
        private const string COOKIE_KEY = "AppAuthentication.Cookie";
        private const string LOGINPAGE_KEY = "AppAuthentication.LoginPage";

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(AuthenticateRequest);
            context.PostAuthenticateRequest += new EventHandler(PostAuthenticateRequest);
        }

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app == null)
                throw new ApplicationException("Typeof Object must be HttpApplication");

            string cookieName = ConfigurationManager.AppSettings[COOKIE_KEY];
            if (cookieName == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", COOKIE_KEY));

            HttpRequest request = app.Request;

            // Se o pedido transportar cookies verificar se corresponde a alguma entrada registada
            if (request.Cookies.Count > 0 && request.Cookies[cookieName] != null)
            {
                AccountUser user;
                try
                {
                    // O value tem que vir encriptado!!
                    // Se calhar tem que ser verificada a propriedade Expires
                    user = MvcNotMembershipProvider.GetUser(request.Cookies[cookieName].Value);
                }
                catch (ArgumentException)
                {
                    // Utilizador inexistente
                    return;
                }
                if (user != null)
                {
                    app.Context.User = new System.Security.Principal.GenericPrincipal(
                        new System.Security.Principal.GenericIdentity(user.Number, "Forms"),
                        Roles.GetRolesForUser(user.Number));
                }
            }
        }

        private void PostAuthenticateRequest(object sender, EventArgs e)
        {
            // Verificar o status code da response
            // if 401 fazer redire��o!
            HttpApplication app = sender as HttpApplication;
            if (app == null)
                throw new ApplicationException("Typeof Object must be HttpApplication");

            string loginPage = ConfigurationManager.AppSettings[LOGINPAGE_KEY];
            if (loginPage == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", LOGINPAGE_KEY));

            if (app.Context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                AppFormsAuthentication.RedirectToLoginPage();
            }
        }

        public void Dispose()
        {
            
        }
    }
}