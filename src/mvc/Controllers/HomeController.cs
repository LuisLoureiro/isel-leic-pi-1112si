using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
using mvc.Models.Entities;
using mvc.Models.Repository;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(String search)
        {
            if(!String.IsNullOrEmpty(search))
            {
                var res = new List<IEnumerable>
                              {
                                  RepositoryLocator.Get<string, CurricularUnit>().GetAll()
                                      .Where(
                                          f =>
                                          (f.Key.ToUpper().Contains(search.ToUpper()) ||
                                           f.Name.ToUpper().Contains(search.ToUpper()) ||
                                           f.Assessment.ToUpper().Contains(search.ToUpper()) ||
                                           f.Program.ToUpper().Contains(search.ToUpper()) ||
                                           f.Objectives.ToUpper().Contains(search.ToUpper()) ||
                                           f.Results.ToUpper().Contains(search.ToUpper()))),
                              };

                if(Request.IsAuthenticated)
                    res.Add(RepositoryLocator.Get<long, Proposal>().GetAll()
                        .Where(f =>
                            (f.Info.Key.ToUpper().Contains(search.ToUpper()) ||
                             f.Info.Name.ToUpper().Contains(search.ToUpper()) ||
                             f.Info.Assessment.ToUpper().Contains(search.ToUpper()) ||
                             f.Info.Program.ToUpper().Contains(search.ToUpper()) ||
                             f.Info.Objectives.ToUpper().Contains(search.ToUpper()) ||
                             f.Info.Results.ToUpper().Contains(search.ToUpper())) &&
                            f.Owner.Equals(User.Identity.Name)));


                ViewBag.SearchString = search;

                return View(res);
            }

            

            return View();
        }
    }
}
