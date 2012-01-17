using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mvc.Models;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    [Authorize]
    public class PropController : Controller
    {
        public ActionResult Index(int page = 0, int pageSize = -1, bool partial = false, string orderBy = null)
        {
            bool redirect = false;

            if (page <= 0)
            {
                page = 1;
                redirect = true;
            }

            if ((pageSize < 0))
            {
                pageSize = 3;
                redirect = true;
            }

            ViewBag.IdSort = string.IsNullOrEmpty(orderBy) ? "Id desc" : "";
            ViewBag.AcrSort = "Acr desc".Equals(orderBy) ? "Acr" : "Acr desc";
            ViewBag.NameSort = "Name desc".Equals(orderBy) ? "Name" : "Name desc";
            ViewBag.EctsSort = "Ects desc".Equals(orderBy) ? "Ects" : "Ects desc";
            ViewBag.OwnerSort = "Owner desc".Equals(orderBy) ? "Owner" : "Owner desc";
            ViewBag.PageSize = pageSize;
            ViewBag.OrderBy = orderBy;

            var elems = RepositoryLocator.Get<long, Proposal>()
                .GetAll()
                .Where(p => User.IsInRole("Admin")
                                ? p.State.Equals(AbstractEntity<long>.Status.Pending)
                                : p.Owner.Equals(User.Identity.Name));

            switch(orderBy)
            {
                case "Id desc":
                    elems = elems.OrderByDescending(f => f.Key);
                    break;
                case "Acr":
                    elems = elems.OrderBy(f => f.Info.Key);
                    break;
                case "Acr desc":
                    elems = elems.OrderByDescending(f => f.Info.Key);
                    break;
                case "Name":
                    elems = elems.OrderBy(f => f.Info.Name);
                    break;
                case "Name desc":
                    elems = elems.OrderByDescending(f => f.Info.Name);
                    break;
                case "Ects":
                    elems = elems.OrderBy(f => f.Info.Ects);
                    break;
                case "Ects desc":
                    elems = elems.OrderByDescending(f => f.Info.Ects);
                    break;
                case "Owner":
                    elems = elems.OrderBy(f => f.Owner);
                    break;
                case "Owner desc":
                    elems = elems.OrderByDescending(f => f.Owner);
                    break;
                default:
                    elems = elems.OrderBy(f => f.Key);
                    break;
            }


            var viewModel = new TableViewModel
                                {
                                    Items = pageSize > 0
                                                ? elems.Skip((page - 1)*pageSize) //Salta os elementos iniciais que não interessam
                                                       .Take(pageSize)           //Retorna apenas o numero de elementos que pretendemos
                                                : elems,
                                    PagingInfo = new PagingInfo
                                                     {
                                                         CurrentPage = page,
                                                         ItemsPerPage = pageSize,
                                                         TotalItems = elems.Count()
                                                     }
                                };

            int total = viewModel.PagingInfo.TotalPages;
            if (page > total)
            {
                page = total;
                redirect = true;
            }

            if (redirect)
                return RedirectToAction("Index", new {page, pageSize});

            return partial ? (ActionResult)PartialView("ProposalsTableContent", viewModel.Items as IEnumerable<Proposal>)
                           : View(viewModel);
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
                return new HttpStatusCodeResult(403, "Only the owner or admin users are able to view this proposal.");

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

            if (!proposal.State.Equals(AbstractEntity<long>.Status.Pending))
                return new HttpStatusCodeResult(403, "It's not possible to edit a proposal that's already accepted or canceled.");
            
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

            if (!proposal.State.Equals(AbstractEntity<long>.Status.Pending))
                return new HttpStatusCodeResult(403, "It's not possible to edit a proposal that's already accepted or canceled.");
            
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

            if (!proposal.State.Equals(AbstractEntity<long>.Status.Pending))
                return new HttpStatusCodeResult(403, "It's not possible to accept a proposal that's already accepted or canceled.");
            
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

                if (!proposal.State.Equals(AbstractEntity<long>.Status.Pending))
                    return new HttpStatusCodeResult(403, "It's not possible to cancel a proposal that's already accepted or canceled.");

                proposal.UpdateStatus(AbstractEntity<long>.Status.Canceled);
            }
            TempData["message"] = "Proposta cancelada com sucesso.";

            return RedirectToAction("Index", "Prop");
        }
    }
}
