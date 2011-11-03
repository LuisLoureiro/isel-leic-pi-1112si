using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class UcController : Controller
    {
        //
        // GET: /Uc/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string acr)
        {
            //Aceder ao repositório e devolver a FUC com aquele acrónimo
            //Hardcoded só para visualizar qualquer coisa...
            return View(new UcModel
                            {
                                Assessment = "Aqui vai a aprendizagem",
                                Ects = 6,
                                Mandatory = true,
                                Name = "TESTE",
                                Objectives = "Estes são os objectivos",
                                Program = "Este é o programa",
                                Results = "E isto os resultados",
                                Semester = 5
                            } );
        }

    }
}
