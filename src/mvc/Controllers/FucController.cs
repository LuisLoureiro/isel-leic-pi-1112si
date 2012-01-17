using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mvc.Models;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    public class FucController : Controller
    {
        public ActionResult Index(int page = 0, int pageSize = -1, bool partial = false, string orderBy = null)
        {
            bool redirect = false;

            if (page <= 0)
            {
                page = 1;
                redirect = true;
            }

            if (pageSize < 0)
            {
                pageSize = 3;
                redirect = true;
            }

            ViewBag.AcrSort = string.IsNullOrEmpty(orderBy) ? "Acr desc" : "";
            ViewBag.NameSort = "Name desc".Equals(orderBy) ? "Name" : "Name desc";
            ViewBag.EctsSort = "Ects desc".Equals(orderBy) ? "Ects" : "Ects desc";
            ViewBag.PageSize = pageSize;
            ViewBag.OrderBy = orderBy;

            var elems = RepositoryLocator.Get<string, CurricularUnit>().GetAll();

            switch (orderBy)
            {
                case "Acr desc":
                    elems = elems.OrderByDescending(f => f.Key);
                    break;
                case "Name":
                    elems = elems.OrderBy(f => f.Name);
                    break;
                case "Name desc":
                    elems = elems.OrderByDescending(f => f.Name);
                    break;
                case "Ects":
                    elems = elems.OrderBy(f => f.Ects);
                    break;
                case "Ects desc":
                    elems = elems.OrderByDescending(f => f.Ects);
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

            return partial ? (ActionResult) PartialView("CurricularUnitsTableContent", viewModel.Items as IEnumerable<CurricularUnit>)
                           : View(viewModel);
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

            return !proposta.Any()
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
