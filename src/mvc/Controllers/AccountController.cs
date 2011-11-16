using System;
using System.Web.Mvc;
using System.Web.Security;
using mvc.Models;

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
            return View(MvcNotMembershipProvider.GetUser(User.Identity.Name));
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
            if (ModelState.IsValid)
            {
                try
                {
                    if (MvcNotMembershipProvider.ValidateUser(model.Username, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);

                        return returnUrl == null ? (ActionResult)RedirectToAction("Index", "Account") : Redirect(returnUrl);
                    }
                    ModelState.AddModelError("", "Password incorrecta.");
                }
                catch(InvalidOperationException e)
                {
                    ModelState.AddModelError("", e.Message + "\r\n Check your email box for activation.");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
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
                    // TODO enviar email
                    TempData["message"] = "Registo criado com sucesso! Verifique a sua caixa de correio electrónico.";

                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError("", "Utilizador não foi registado. " + e.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Activate(string hash)
        {
            try
            {
                MvcNotMembershipProvider.ActivateUser(hash);
                TempData["message"] = "Utilizador activado com sucesso!";
                return RedirectToAction("LogOn");
            }
            catch (ArgumentException e)
            {
                TempData["exception"] = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult LogOff()
        {
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
            MvcNotMembershipProvider.DeleteUser(user);
            Roles.RemoveUserFromRoles(user, Roles.GetRolesForUser(user));

            return RedirectToAction("Index", "Home");
        }
    }
}
