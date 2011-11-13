using System;
using System.Web.Mvc;
using System.Web.Security;
using mvc.Models;
using mvc.Models.Entities;

namespace mvc.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [Authorize]
        public ActionResult Index()
        {
            if(!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();
            
            return View(Request.RequestContext.HttpContext.User.Identity.Name);
        }
        
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOn model, string returnUrl)
        {
            if (ModelState.IsValid) // TODO ver situação dos utilizadores que ainda não estão activados!!
            {
                try
                {
                    if (MvcNotMembershipProvider.ValidateUser(model.Username, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username.ToString(), false);

                        return returnUrl == null ? (ActionResult)RedirectToAction("Index", "Account") : Redirect(returnUrl);
                    }
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // Chegar aqui indica que a submissão do formulário não foi aceite, devido à não validação da informação nele presente.
            // O conteúdo da resposta vai conter o formulário pré-preenchido, através da passagem à vista do parâmetro 'model'
            //  com a informação submetida.
            // As propriedades da classe LogOn estão decoradas com atributos por forma a indicar a informação detalhada
            //  dos erros de validação.
            return View(model);
        }

        public ActionResult Register()
        {
            // TODO será melhor enviar uma excepção, para nossa precaução??
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Account");

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MvcNotMembershipProvider.CreateUser(model);

                    FormsAuthentication.SetAuthCookie(model.Number.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Activate(String hash)
        {
            if (!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            var user = new DefaultUser();
            
            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            // TODO será melhor enviar uma excepção, para nossa precaução??
            if (!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // TODO Change password
    }
}
