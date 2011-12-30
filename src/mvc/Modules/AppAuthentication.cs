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
    /// Classe responsável por fazer a gestão dos cookies.
    /// Implementa alguns métodos que a classe FormsAuthentication disponibiliza.
    /// </summary>
    public class AppFormsAuthentication
    {
        private const string COOKIE_KEY = "AppAuthentication.Cookie";
        private const string LOGINPAGE_KEY = "AppAuthentication.LoginPage";

        // Por questões de segurança, o valor do cookie é uma entrada numa tabela com os valores necessários
        //  para a aplicação. Desta forma, evita-se que alguém, que consiga interceptar os cookies, consiga
        //  aceder a informações internas e que só utilizadores registados tenham acesso.
        // Para que seja fácil manter um registo dos que já fizeram logoff ou que ainda estejam ligados!
        private static readonly IDictionary<string, string> CookieValueToUsername = new Dictionary<string, string>();
        
        public static void SignOut()
        {
            string cookieName = ValidateCookieConfiguration();

            HttpContext current = HttpContext.Current;

            // Esta acção apenas retira o cookie da colecção no lado do servidor!
            CookieValueToUsername.Remove(current.Request.Cookies[cookieName].Value);
            // Para retirar no lado do cliente, adiciona-se um novo cookie à resposta, com nome igual ao que se quer remover e
            //  com data de expiração anterior à data actual.
            // Esta acção faz com que o browser actualize a informação que tinha sobre o referido cookie e,
            //  ao verificar que o cookie já expirou, o elimine e não o volte a usar em futuros pedidos.
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

            // A forma como o valor do cookie está a ser gerado faz com que para o mesmo utilizador
            //  seja sempre gerado o mesmo valor de cookie!!
            string value = MD5Crypto.GenerateMD5(username);
            CookieValueToUsername[value] = username;

            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(cookieName) { HttpOnly = true, Value = value });
            // Ao não se afectar o valor da propriedade Domain, indica-se que o cookie é para
            //  ser associado ao domínio corrente.
            // Não se afecta a propriedade Expires para que sejam non-persistent cookies,
            //  têm tempo de duração igual à duração da sessão, quando o utilizador
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
            // O atributo Authorize actua ao nível do MvcHandler, por isso adiciona-se
            //  o module ao HttpApplication Event que é executado depois de executado o Handler.
            context.PostRequestHandlerExecute += new EventHandler(PostRequestHandlerExecute);
        }

        private static void AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName, loginPage;
            HttpApplication app = AppFormsAuthentication.ValidateApplication(sender, out cookieName, out loginPage);

            HttpRequest request = app.Request;

            // Se o pedido transportar cookies verificar se algum corresponde a um
            //  utilizador da aplicação.
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
            // Verificar o status code da response: se 401 fazer redireção!
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