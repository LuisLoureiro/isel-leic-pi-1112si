using System;
using System.Web.Helpers;
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
            if (ModelState.IsValid)
            {
                try
                {
                    MvcNotMembershipProvider.UpdateUser(updatedUser);
                    TempData["message"] = "Alteração efectuada com sucesso!";
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Alteração não foi concretizada. " + e.Message);
                }
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
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string hash = MvcNotMembershipProvider.CreateUser(model);

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "ISEL.LEIC.PI.LI51NG08@gmail.com";
                WebMail.Password = "iselleicpi";
                WebMail.From = "ISEL.LEIC.PI.LI51NG08@gmail.com";
                WebMail.Send(model.Email.Trim(), "Activação de Acesso", string.Format("Para activar o seu acesso siga o seguinte link: {0}/Account/Activate/?hash={1} ", Request.Url.ToString().Replace(Request.Path, ""), hash));

                TempData["message"] = "Registo criado com sucesso! Verifique a sua caixa de correio electrónico.";

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("", "Utilizador não foi registado. " + e.Message);
                return View(model);
            }catch (Exception e)
            {
                TempData["message"] = "Não foi possivel enviar o email\n" + e;
                return View(model);
            }
        }

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
        public ActionResult Remove(string id)
        {
            if (!Request.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            if (!User.Identity.Name.Equals(id) && !User.IsInRole("admin"))
                return new HttpStatusCodeResult(403, "Impossivel remover uma conta que não seja a sua");

            if (User.Identity.Name.Equals(id))
                FormsAuthentication.SignOut();

            MvcNotMembershipProvider.DeleteUser(id);
            Roles.RemoveUserFromRoles(id, Roles.GetRolesForUser(id));

            TempData["message"] = "Utilizador removido com sucesso!";

            return User.Identity.Name.Equals(id) 
                ? RedirectToAction("Index", "Home") 
                : RedirectToAction("Index", "Account");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UpdateRole(string id, string roles)
        {
            if (!Roles.RoleExists(roles))
            {
                return new HttpStatusCodeResult(500, "Something nonsense happened, the new role does not exist!");
            }
            // Se a actualização for para a mesma role, não se actualiza.
            if (!Roles.IsUserInRole(id, roles))
            {
                try
                {
                    Roles.RemoveUserFromRoles(id, Roles.GetRolesForUser(id));
                    Roles.AddUserToRole(id, roles);
                }
                catch (ArgumentException e)
                {
                    TempData["exception"] = e.Message;
                }
                
                TempData["message"] = string.Format("Regra de acesso do utilizador {0} alterada para {1} com sucesso!",
                                                    id, roles);
            }
            else
            {
                TempData["exception"] = "Não é possível alterar uma regra de acesso para a mesma regra!";
            }

            return RedirectToAction("Index");
        }
    }
}
