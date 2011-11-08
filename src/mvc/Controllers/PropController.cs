using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HttpServer.Model.Entities;
using mvc.Models;
using mvc.Models.Entities;

namespace mvc.Controllers
{
    public class PropController : Controller
    {
        //
        // GET: /Prop/

        public ActionResult Index()
        {
            //TODO
            return View();
        }

        public ActionResult Details(int id)
        {
            //TODO - passar à vista um modelo
            return View();
        }

        public ActionResult Edit(int id)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Proposal model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model.Info, null);

            return RedirectToAction("Details", "Prop", prop.Key);
        }

        [HttpPost]
        public ActionResult Accept(int id)
        {
            //TODO
            return RedirectToAction("Index", "Prop");
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            //TODO
            return RedirectToAction("Index", "Prop");
        }
    }
}
