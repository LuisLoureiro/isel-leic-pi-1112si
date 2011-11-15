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

            // Os cookies criados vão com o número do utilizador
            return View(MvcNotMembershipProvider.GetUser(
                            Convert.ToInt32(User.Identity.Name)));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(AccountUser updatedUser)
        {
            if (!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            try
            {
                MvcNotMembershipProvider.UpdateUser(updatedUser);
                TempData["message"] = "Alteração efectuada com sucesso!";
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Alteração não foi concretizada. "+e.Message);
            }

            return View(updatedUser);
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
                    ModelState.AddModelError("", "O nome de utilizador ou password estão incorrectos");
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
                    TempData["message"] = "Registo criado com sucesso!";

                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError("", "Utilizador não foi registado. " + e.Message);
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

        [Authorize]
        [HttpPost]
        public ActionResult Remove()
        {
            if (!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            string user = User.Identity.Name;

            FormsAuthentication.SignOut();
            MvcNotMembershipProvider.DeleteUser(Convert.ToInt32(user));
            Roles.RemoveUserFromRoles(user, Roles.GetRolesForUser(user));

            return RedirectToAction("Index", "Home");
        }

        // TODO Change password
    }
}
