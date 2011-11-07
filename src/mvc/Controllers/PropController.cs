using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

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
        public ActionResult Edit(PropModel model)
        {
            //TODO
            var prop = new PropModel();
            return RedirectToAction("Details", "Prop", prop.Id);
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
