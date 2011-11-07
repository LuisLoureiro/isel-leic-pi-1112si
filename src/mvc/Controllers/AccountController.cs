using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            //TODO mostrar perfil do user que se encontra autenticado

            return View();
        }
        
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(UserModel model)
        {
            return View();
        }

        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Account");

            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Account");

            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Activate(String hash)
        {
            //TODO verificar activação do user e redireccionar
            var user = new UserModel();

            if (true)
                return LogOn(user);

            return View();
        }
    }
}
