using System.Web.Mvc;
using mvc.Models.Entities;
using mvc.Models.Reposiroty;

namespace mvc.Controllers
{
    public class PropController : Controller
    {

        public ActionResult Index()
        {
            return View(RepositoryLocator.Get<long, Proposal>().GetAll());
        }

        public ActionResult Details(long id)
        {
            return View(RepositoryLocator.Get<long, Proposal>().GetById(id));
        }

        public ActionResult Edit(long id)
        {
            return View(RepositoryLocator.Get<long, Proposal>().GetById(id));
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
