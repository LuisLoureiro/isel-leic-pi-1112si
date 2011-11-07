using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class FucController : Controller
    {
        //
        // GET: /Uc/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string acr)
        {
            //TODO
            return View();
        }

        public ActionResult New()
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult New(FucModel model)
        {
            //TODO
            var prop = new PropModel();
            return RedirectToAction("Details", "Prop", prop.Id);
        }

        public ActionResult Edit(string acr)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FucModel model)
        {
            //TODO
            var prop = new PropModel();
            return RedirectToAction("Details", "Prop", prop.Id);
        }
    }
}
