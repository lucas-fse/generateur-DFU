using GenerateurDFUSafir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class PlanningController : Controller
    {
        // GET: PlanningSociete
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("getPlanningSocieteApi")]
        public ActionResult GetPlanningSocieteApi(int numsemaine, string filtre)
        {
            PlanningSociete planningSociete = new PlanningSociete(numsemaine, filtre);

            return Json(new { Planning = planningSociete.Planning, KeyValues = planningSociete.PlanningValues, Services = planningSociete.ServicesList } , JsonRequestBehavior.AllowGet);
        }
    }
}