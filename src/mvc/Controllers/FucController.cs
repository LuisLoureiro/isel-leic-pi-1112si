using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    public class FucController : Controller
    {
        //
        // GET: /Fuc/

        public ActionResult Index()
        {
            return View(RepositoryLocator.Get<string, CurricularUnit>().GetAll());
        }

        public ActionResult Details(string id)
        {
            var fuc = RepositoryLocator.Get<string, CurricularUnit>().GetById(id);
            return View(fuc);
        }

        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(CurricularUnit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model, User.Identity.Name);
            RepositoryLocator.Get<long, Proposal>().Insert(prop);
            
            return RedirectToAction("Details", "Prop", new { Id = prop.Key });
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            string owner = User.Identity.Name;
            IEnumerable<Proposal> proposta = RepositoryLocator.Get<long, Proposal>().GetAll().Where(
                prop => prop.Info.Key.Equals(id) && prop.Owner.Equals(owner));

            return proposta.Count() == 0
                       ? (ActionResult) View(RepositoryLocator.Get<string, CurricularUnit>().GetById(id))
                       : RedirectToAction("Details", "Prop", new {Id = proposta.First().Key});
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CurricularUnit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model, User.Identity.Name);
            RepositoryLocator.Get<long, Proposal>().Insert(prop);

            return RedirectToAction("Details", "Prop", new { Id = prop.Key });
        }
    }
}
