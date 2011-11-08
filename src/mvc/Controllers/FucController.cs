using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
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

        public ActionResult Details(string id)
        {
            var fuc = RepositoryLocator.Get<string, CurricularUnit>().GetById(id);
            return View(fuc);
        }

        public ActionResult New()
        {
            //var emptyFuc = new CurricularUnit();
            return View();
        }

        [HttpPost]
        public ActionResult New(CurricularUnit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model, null);
            //TODO colocar no repositório
            
            return RedirectToAction("Details", "Prop", prop.Key);
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

            var prop = new Proposal(model, null);

            return RedirectToAction("Details", "Prop", prop.Key);
        }
    }
}
