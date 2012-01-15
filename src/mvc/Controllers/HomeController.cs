using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private static Func<CurricularUnit, bool> SearchUc(string search)
        {
            return (f => (f.Key.ToUpper().Contains(search.ToUpper()) ||
                          f.Name.ToUpper().Contains(search.ToUpper()) ||
                          f.Assessment.ToUpper().Contains(search.ToUpper()) ||
                          f.Program.ToUpper().Contains(search.ToUpper()) ||
                          f.Objectives.ToUpper().Contains(search.ToUpper()) ||
                          f.Results.ToUpper().Contains(search.ToUpper())));
        }

        private static Func<Proposal, bool> SearchProp(string search, string username)
        {
            return (p => (p.State.Equals(AbstractEntity<long>.Status.Pending) &&
                          (p.Info.Key.ToUpper().Contains(search.ToUpper()) ||
                           p.Info.Name.ToUpper().Contains(search.ToUpper()) ||
                           p.Info.Assessment.ToUpper().Contains(search.ToUpper()) ||
                           p.Info.Program.ToUpper().Contains(search.ToUpper()) ||
                           p.Info.Objectives.ToUpper().Contains(search.ToUpper()) ||
                           p.Info.Results.ToUpper().Contains(search.ToUpper())) &&
                           p.Owner.Equals(username)));

        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AjaxSearch(string search)
        {
            var res = new List<IEnumerable<KeyValuePair<string, string>>>();
            if(!String.IsNullOrEmpty(search))
            {
                search = Server.HtmlEncode(search);
                var fucs = new LinkedList<KeyValuePair<string, string>>(
                                    RepositoryLocator.Get<string, CurricularUnit>().GetAll()
                                        .Where(SearchUc(search))
                                        .Select(f => new KeyValuePair<string, string>("/fuc/details/" + f.Key, f.Name)));
                fucs.AddFirst(new KeyValuePair<string, string>("Fichas de Unidades Curriculares", null));
                res.Add(fucs);

                if (Request.IsAuthenticated)
                {
                    var props = new LinkedList<KeyValuePair<string, string>>(
                                        RepositoryLocator.Get<long, Proposal>().GetAll()
                                            .Where(SearchProp(search, User.Identity.Name))
                                            .Select(p => new KeyValuePair<string, string>("/prop/details/" + p.Key, p.Key.ToString())));
                    props.AddFirst(new KeyValuePair<string, string>("Propostas", null));
                    res.Add(props);
                }
            }
            return PartialView(res);
        }

        public ActionResult Search(String search)
        {
            if(!String.IsNullOrEmpty(search))
            {
                search = Server.HtmlEncode(search);
                var res = new List<IEnumerable>
                              {
                                  RepositoryLocator.Get<string, CurricularUnit>().GetAll()
                                      .Where(SearchUc(search))
                              };

                if(Request.IsAuthenticated)
                    res.Add(RepositoryLocator.Get<long, Proposal>().GetAll()
                        .Where(SearchProp(search, User.Identity.Name)));


                ViewBag.SearchString = search;

                return View(res);
            }
            return View();
        }
    }
}
