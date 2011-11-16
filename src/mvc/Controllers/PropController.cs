using System.Web.Mvc;
using mvc.Models.Entities;
using mvc.Models.Repository;

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

        [Authorize]
        public ActionResult Edit(long id)
        {
            return View(RepositoryLocator.Get<long, Proposal>().GetById(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Proposal model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prop = new Proposal(model.Info, User.Identity.Name);

            return RedirectToAction("Details", "Prop", prop.Key);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Accept(int id)
        {
            return RedirectToAction("Index", "Prop");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Cancel(int id)
        {
            //TODO
            return RedirectToAction("Index", "Prop");
        }
    }
}
