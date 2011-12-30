using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using mvc.Crypto;
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

        // Por quest�es de seguran�a, o valor do cookie � uma entrada numa tabela com os valores necess�rios
        //  para a aplica��o. Desta forma, evita-se que algu�m, que consiga interceptar os cookies, consiga
        //  aceder a informa��es internas e que s� utilizadores registados tenham acesso.
        // Para que seja f�cil manter um registo dos que j� fizeram logoff ou que ainda estejam ligados!
        private static readonly IDictionary<string, string> CookieValueToUsername = new Dictionary<string, string>();
        
        public static void SignOut()
        {
            string cookieName = ValidateCookieConfiguration();

            HttpContext current = HttpContext.Current;

            // Esta ac��o apenas retira o cookie da colec��o no lado do servidor!
            CookieValueToUsername.Remove(current.Request.Cookies[cookieName].Value);
            // Para retirar no lado do cliente, adiciona-se um novo cookie � resposta, com nome igual ao que se quer remover e
            //  com data de expira��o anterior � data actual.
            // Esta ac��o faz com que o browser actualize a informa��o que tinha sobre o referido cookie e,
            //  ao verificar que o cookie j� expirou, o elimine e n�o o volte a usar em futuros pedidos.
            current.Response.Cookies.Add(new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1d) });
        }

        public static void RedirectToLoginPage()
        {
            string loginPage = ValidateLoginConfiguration();
            
            HttpContext current = HttpContext.Current;
            current.Response.Redirect(string.Format("{0}?returnUrl={1}", loginPage, 
                current.Server.UrlEncode(current.Request.RawUrl)));
        }

        public static void SetAuthCookie(string username, bool persistentCookie)
        {
            string cookieName = ValidateCookieConfiguration();

            // A forma como o valor do cookie est� a ser gerado faz com que para o mesmo utilizador
            //  seja sempre gerado o mesmo valor de cookie!!
            string value = MD5Crypto.GenerateMD5(username);
            CookieValueToUsername[value] = username;

            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(cookieName) { HttpOnly = true, Value = value });
            // Ao n�o se afectar o valor da propriedade Domain, indica-se que o cookie � para
            //  ser associado ao dom�nio corrente.
            // N�o se afecta a propriedade Expires para que sejam non-persistent cookies,
            //  t�m tempo de dura��o igual � dura��o da sess�o, quando o utilizador
            //  fechar a janela do browser, o cookie expira.
        }

        internal static DefaultUser GetUserFor(string value)
        {
            try
            {
                return MvcNotMembershipProvider.GetUser(CookieValueToUsername[value]);
            }
            catch (Exception)
            {
                // Utilizador ou cookie inexistente
                return null;
            }
        }

        internal static HttpApplication ValidateApplication(object app, out string cookieName, out string loginPage)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            var application = app as HttpApplication;
            if (application == null)
                throw new ApplicationException("Typeof Object must be HttpApplication.");

            if (!application.Request.Browser.Cookies)
                throw new ApplicationException("This application does not support cookieless requests.");

            cookieName = ValidateCookieConfiguration();
            loginPage = ValidateLoginConfiguration();

            return application;
        }

        private static string ValidateCookieConfiguration()
        {
            string cookieName = ConfigurationManager.AppSettings[COOKIE_KEY];
            if (cookieName == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", COOKIE_KEY));
            return cookieName;
        }

        private static string ValidateLoginConfiguration()
        {
            string loginPage = ConfigurationManager.AppSettings[LOGINPAGE_KEY];
            if (loginPage == null)
                throw new ApplicationException(string.Format(
                    "To use this module it's necessary to have the key '{0}' in appSettings section of Web.config.", LOGINPAGE_KEY));
            return loginPage;
        }
    }

    public class AppAuthentication : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(AuthenticateRequest);
            // O atributo Authorize actua ao n�vel do MvcHandler, por isso adiciona-se
            //  o module ao HttpApplication Event que � executado depois de executado o Handler.
            context.PostRequestHandlerExecute += new EventHandler(PostRequestHandlerExecute);
        }

        private static void AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName, loginPage;
            HttpApplication app = AppFormsAuthentication.ValidateApplication(sender, out cookieName, out loginPage);

            HttpRequest request = app.Request;

            // Se o pedido transportar cookies verificar se algum corresponde a um
            //  utilizador da aplica��o.
            string userCookie;
            if (request.Cookies.Count > 0 && !string.IsNullOrEmpty(
                    userCookie = request.Cookies.AllKeys
                                    .Where(str => str.ToUpper().Equals(cookieName.ToUpper()))
                                    .FirstOrDefault()))
            {
                DefaultUser user = AppFormsAuthentication.GetUserFor(request.Cookies[userCookie].Value);
                
                if (user != null)
                {
                    app.Context.User = new System.Security.Principal.GenericPrincipal(
                        new System.Security.Principal.GenericIdentity(user.Number, "AppForms"),
                        Roles.GetRolesForUser(user.Number));
                }
            }
        }

        private static void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            // Verificar o status code da response: se 401 fazer redire��o!
            string cookieName, loginPage;
            HttpApplication app = AppFormsAuthentication.ValidateApplication(sender, out cookieName, out loginPage);

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