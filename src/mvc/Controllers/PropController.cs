using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    [Authorize]
    public class PropController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Proposal> proposals = RepositoryLocator.Get<long, Proposal>().GetAll();
            return User.IsInRole("admin")
                       ? View(proposals.Where(prop => prop.State.Equals(AbstractEntity<long>.Status.Pending)))
                       : View(proposals.Where(prop => prop.Owner.Equals(User.Identity.Name)));
        }

        public ActionResult Details(long id)
        {
            Proposal proposal = RepositoryLocator.Get<long, Proposal>().GetById(id);
            if (proposal == null)
            {
                TempData["exception"] = "Não existe nenhuma proposta com o identificador indicado.";
                return RedirectToAction("Index", "Prop");
            }
            
            if (!(User.Identity.Name.Equals(proposal.Owner) || User.IsInRole("admin")))
                return new HttpStatusCodeResult(403, "Only the owner or admin users are able to cancel this proposal.");

            return View(proposal);
        }

        public ActionResult Edit(long id)
        {
            Proposal proposal = RepositoryLocator.Get<long, Proposal>().GetById(id);
            if (proposal == null)
            {
                TempData["exception"] = "Não existe nenhuma proposta com o identificador indicado.";
                return RedirectToAction("Index", "Prop");
            }
            
            if (!proposal.Owner.Equals(User.Identity.Name))
                return new HttpStatusCodeResult(403, "You are not the owner of this proposal.");

            return View(proposal);
        }

        [HttpPost]
        public ActionResult Edit(Proposal model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Proposal proposal = RepositoryLocator.Get<long, Proposal>().GetById(model.Key);
            if (proposal == null)
            {
                TempData["exception"] = "Não existe nenhuma proposta com o identificador indicado.";
                return RedirectToAction("Index", "Prop");
            }
            
            if (!proposal.Owner.Equals(User.Identity.Name))
                return new HttpStatusCodeResult(403, "You are not the owner of this proposal.");

            proposal.Info = model.Info;
            TempData["message"] = "Proposta editada com sucesso.";

            return RedirectToAction("Details", "Prop", new { Id = proposal.Key });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Accept(int id)
        {
            Proposal proposal = RepositoryLocator.Get<long, Proposal>().GetById(id);
            if (proposal == null)
            {
                TempData["exception"] = "Não existe nenhuma proposta com o identificador indicado.";
                return RedirectToAction("Index", "Prop");
            }
            
            proposal.UpdateStatus(AbstractEntity<long>.Status.Accepted);
            if (RepositoryLocator.Get<string, CurricularUnit>().GetById(proposal.Info.Key) != null)
                RepositoryLocator.Get<string, CurricularUnit>().Update(proposal.Info);
            else
                RepositoryLocator.Get<string, CurricularUnit>().Insert(proposal.Info);
            
            TempData["message"] = "Ficha de unidade curricular criada com sucesso.";

            return RedirectToAction("Details", "Fuc", new { Id = proposal.Info.Key });
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            Proposal proposal = RepositoryLocator.Get<long, Proposal>().GetById(id);
            if (proposal == null)
                TempData["exception"] = "Não existe nenhuma proposta com o identificador indicado.";
            else
            {
                if (!(proposal.Owner.Equals(User.Identity.Name) || User.IsInRole("admin")))
                    return new HttpStatusCodeResult(403, "Only the owner or admin users are able to cancel this proposal.");

                proposal.UpdateStatus(AbstractEntity<long>.Status.Canceled);
            }
            TempData["message"] = "Proposta cancelada com sucesso.";

            return RedirectToAction("Index", "Prop");
        }
    }
}
