using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateurDFUSafir.Models;

namespace GenerateurDFUSafir.Controllers
{
    public class TestController : Controller
    {
        // GET: Diagnostique
        public ActionResult Index()
        {
            TestOutilProduction test = TestOutilProduction.GetInstance();
            return View(test);
        }

        public ActionResult Output()
        {
            TestOutilProductionVue tt = new TestOutilProductionVue();
            return Json(tt.RequestResult(), JsonRequestBehavior.AllowGet);
        }

        public void  Start()
        {
            TestOutilProduction test = TestOutilProduction.GetInstance();
            test.TestOutilProductionExecute();
            //return View(test);
            
        }

       
    }
}