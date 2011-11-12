using System.Web.Mvc;
using mvc.Models.Entities;
using mvc.Models.Reposiroty;

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
