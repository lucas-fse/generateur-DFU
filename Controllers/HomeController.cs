using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                string rawId = Request.UserHostName;
                string info = Dns.GetHostEntry(Request.UserHostAddress).HostName.ToString();
            }
            catch { }

            KPI kpi = new KPI();

            return View(kpi);
            
        }
        private List<OrdreFabrication> InfoProduction;
        private void RefreshDataProduction()
        {
            try
            {
                InfoProduction = InfoAllProduction.InfoAllOfProduction();
            }
            catch (Exception e)
            {

            }
        }
        public ActionResult About()
        {
            //ControleFinalScan tmp = new ControleFinalScan();
            //tmp.GetOFSearch("F2314508");
            ViewBag.Message = "OptiZEN";
            AndroidAppList datas = new AndroidAppList();
            return View(datas);
        }

        private List<OrdreFabricationBiDir> OrdreFabrications = new List<OrdreFabricationBiDir>();
        private InfoOrdreFabricationBidir InfoOrdreFabrications = new InfoOrdreFabricationBidir();
        private InfoLogistique InfoLogistique;
        private void RefreshData ()
        {
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    OrdreFabrications = data.ListOfBidirX3().OrderBy(p => p.DateLivraison).ToList();
                    InfoOrdreFabrications = new InfoOrdreFabricationBidir();
                    InfoOrdreFabrications.OrdreFabrications = OrdreFabrications;
                }
            }
            catch (Exception e)
            {

            }
        }

        [ActionName("GetKPI")]
        public ActionResult getKPI()
        {

            AccueilKPI data = new AccueilKPI();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Supervision()
        {
            RefreshData();
            return View(OrdreFabrications);
        }
        
        public ActionResult Synthese()
        {
            RefreshData();
           return View(InfoOrdreFabrications);
        }

        public ActionResult Accueil()
        {
            GestionTraitementOFs vue = new GestionTraitementOFs();
            return View(vue);
        }

        private void RefreshDataLogistique()
        {
            try
            {
                if (true)
                {
                    InfoLogistique = new InfoLogistique();
                    InfoLogistique.MiseAJourData();
                }
            }
            catch (Exception e)
            {

            }
        }
        public ActionResult Logistique()
        {
            RefreshDataLogistique();
            return View(InfoLogistique);
        }
        public ActionResult Operateurs()
        {
            return View();
        }
        public ActionResult GammeUD()
        {
            return View();
        }

    }
}