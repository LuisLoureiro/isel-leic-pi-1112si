using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
using mvc.Models.Entities;

namespace mvc.Controllers
{
    public class FucController : Controller
    {
        //
        // GET: /Fuc/

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Details(string acr)
        {
            //TODO
            var fuc = new CurricularUnit();
            return View(fuc);
        }

        public ActionResult New()
        {
            var emptyFuc = new CurricularUnit();
            return View(emptyFuc);
        }

        [HttpPost]
        public ActionResult New(CurricularUnit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model);
            //TODO colocar no repositório
            
            return RedirectToAction("Details", "Prop", prop.Id);
        }

        public ActionResult Edit(string acr)
        {
            //TODO
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CurricularUnit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model);

            return RedirectToAction("Details", "Prop", prop.Id);
        }
    }
}
