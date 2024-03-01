using DALPEGASE;
using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models;
using GDAL = GenerateurDFUSafir.Models.DAL ;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class OFSafirController : Controller
    {
        // GET: Supervision
        public ActionResult Index()
        {
            RefreshData();
            return View(OrdreFabricationsBiDir);
        }
        public ActionResult Edit(string id)
        {
            //RefreshData();
            RefreshData();
            OrdreFabricationBiDir of = new OrdreFabricationBiDir();
            of = OrdreFabricationsBiDir.Where(p => p.Nmr.Equals(id)).First();
            return View(of);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RefreshData();
            ORDRE_FABRICATION_GENERE of = new ORDRE_FABRICATION_GENERE();
            OrdreFabricationBiDir of2 = new OrdreFabricationBiDir();

            TracabiliteProd traca = new TracabiliteProd();

            List<OrdreFabricationBiDir> ofs = OrdreFabricationsBiDir.Where(p => p.Nmr == id).ToList();
            if (ofs != null && ofs.Count > 0)
            {
                //string[] fieldsToBind = new string[] { "RefComPack", "RefComMO", "RefIndusMO", "Fps", "RefComSim", "RefOptionlogSim", "RefComMT", "RefIndusMT", "RefOptionMaterielMT", "RefOptionMaterielMO" };
                string[] fieldsToBind = new string[] { "RefComPack", "RefComMO", "RefIndusMO", "RefOptionMaterielMO", "RefComMT", "RefIndusMT", "RefOptionMaterielMT", "RefComSim", "RefOptionlogSim", "Fps", "Version", "CommandeSynchro" };
                of2 = ofs.First();
                //of2 = new OrdreFabricationBiDir();
                if (TryUpdateModel(of2, fieldsToBind))
                {
                    try
                    {
                        of.COMMANDE_SYNCHRO = of2.CommandeSynchro;
                        of.VERSION_LOG = of2.Version;
                        of.DATE_GENERATION = null;
                        of.GENERE = false;
                        of.REF_COMMERCIALE_PACK = of2.RefComPack;
                        of.OPTIONS_LOGICIELLES = of2.RefOptionlogSim;
                        of.OPTION_MATERIEL_MO = of2.RefOptionMaterielMO;
                        of.OPTION_MATERIEL_MT = of2.RefOptionMaterielMT;
                        of.REF_COMMERCIALE_MO = of2.RefComMO;
                        of.REF_INDUSTRIELLE_MO = of2.RefIndusMO;
                        of.REF_COMMERCIALE_MT = of2.RefComMT;
                        of.REF_INDUSTRIELLE_MT = of2.RefIndusMT;
                        of.REF_COMMERCIALE_SIM = of2.RefComSim;
                        of.REF_FICHE_PERSO = of2.Fps;
                        of.MODIF_MANUEL = true;
                        of.NUM_OF = of2.Nmr;
                        of.NB_PACK = of2.Qtr;
                        traca.SaveDb2(of);
                        return RedirectToAction("Index");
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                OrdreFabricationBiDir tmp = OrdreFabricationsBiDir.Where(p => p.Nmr == id).First();
                if (tmp != null)
                {
                    ORDRE_FABRICATION_GENERE of3 = new ORDRE_FABRICATION_GENERE();
                    of3.COMMANDE_SYNCHRO = tmp.CommandeSynchro;
                    of3.VERSION_LOG = tmp.Version;
                    of3.DATE_GENERATION = null;
                    of3.GENERE = false;
                    of3.REF_COMMERCIALE_PACK = tmp.RefComPack;
                    of3.OPTIONS_LOGICIELLES = tmp.RefOptionlogSim;
                    of3.OPTION_MATERIEL_MO = tmp.RefOptionMaterielMO;
                    of3.OPTION_MATERIEL_MT = tmp.RefOptionMaterielMT;
                    of3.REF_COMMERCIALE_MO = tmp.RefComMO;
                    of3.REF_INDUSTRIELLE_MO = tmp.RefIndusMO;
                    of3.REF_COMMERCIALE_MT = tmp.RefComMT;
                    of3.REF_INDUSTRIELLE_MT = tmp.RefIndusMT;
                    of3.REF_COMMERCIALE_SIM = tmp.RefComSim;
                    of3.REF_FICHE_PERSO = tmp.Fps;
                    of3.MODIF_MANUEL = true;
                    of3.NUM_OF = tmp.Nmr;
                    of3.NB_PACK = tmp.Qtr;
                    traca.SaveDb2(of3);
                }

            }
            return RedirectToAction("Index");
        }
        public ActionResult Reinitialisation(string id)
        {
            TracabiliteProd traca = new TracabiliteProd();
            List<ORDRE_FABRICATION_GENERE> list = traca.OFGenereExistedb2(id, false, false);
            // 
            OfX3 data = new OfX3();
            List<OrdreFabricationBiDir> ofs = data.ListOfBidirX3().Where(p => p.Nmr == id).ToList();
            if ((list != null && list.Count() > 0) && (ofs != null && ofs.Count() > 0))
            {

                OrdreFabricationBiDir of_orig = ofs.First();
                ORDRE_FABRICATION_GENERE of = list.First();
                of.COMMANDE_SYNCHRO = of_orig.CommandeSynchro;
                of.VERSION_LOG = of_orig.Version;
                of.DATE_GENERATION = null;
                of.GENERE = false;
                of.REF_COMMERCIALE_PACK = of_orig.RefComPack;
                of.OPTIONS_LOGICIELLES = of_orig.RefOptionlogSim;
                of.OPTION_MATERIEL_MO = of_orig.RefOptionMaterielMO;
                of.OPTION_MATERIEL_MT = of_orig.RefOptionMaterielMT;
                of.REF_COMMERCIALE_MO = of_orig.RefComMO;
                of.REF_INDUSTRIELLE_MO = of_orig.RefIndusMO;
                of.REF_COMMERCIALE_MT = of_orig.RefComMT;
                of.REF_INDUSTRIELLE_MT = of_orig.RefIndusMT;
                of.REF_COMMERCIALE_SIM = of_orig.RefComSim;
                of.REF_FICHE_PERSO = of_orig.Fps;
                of.MODIF_MANUEL = false;
                of.NUM_OF = of_orig.Nmr;
                of.NB_PACK = of_orig.Qtr;
                traca.SaveDb2(of);
            }
            RefreshData();
            return RedirectToAction("Index");
        }
        private List<OrdreFabricationBiDir> OrdreFabricationsBiDir = new List<OrdreFabricationBiDir>();
        private InfoNonConformite nonConformiteInfo = new InfoNonConformite();
        private InfoOrdreFabricationBidir InfoOrdreFabrications = new InfoOrdreFabricationBidir();
        private void RefreshData()
        {
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    OrdreFabricationsBiDir = data.ListOfBidirX3().Where(i => !string.IsNullOrWhiteSpace(i.Nmr)).OrderBy(p => p.DateLivraison).ToList();
                    MiseAJourUpdated(ref OrdreFabricationsBiDir);
                    InfoOrdreFabrications = new InfoOrdreFabricationBidir();
                    InfoOrdreFabrications.OrdreFabrications = OrdreFabricationsBiDir;
                }
            }
            catch (Exception e)
            {

            }
        }
        private void MiseAJourUpdated(ref List<OrdreFabricationBiDir> listof)
        {
            TracabiliteProd traca = new TracabiliteProd();
            foreach (var item in listof)
            {
                List<ORDRE_FABRICATION_GENERE> list = traca.OFGenereExistedb2(item.Nmr, false, false);
                if (list != null && list.Count > 0)
                {
                    ORDRE_FABRICATION_GENERE tmp = list.First();
                    if (tmp.GENERE != null)
                    {
                        item.DataGenerated = (bool)tmp.GENERE;
                    }
                    if (tmp.MODIF_MANUEL)
                    {
                        item.CommandeSynchro = tmp.COMMANDE_SYNCHRO;
                        item.Version = tmp.VERSION_LOG;
                        item.RefComPack = tmp.REF_COMMERCIALE_PACK;
                        item.RefOptionlogSim = tmp.OPTIONS_LOGICIELLES;
                        item.RefOptionMaterielMO = tmp.OPTION_MATERIEL_MO;
                        item.RefOptionMaterielMT = tmp.OPTION_MATERIEL_MT;
                        item.RefComMO = tmp.REF_COMMERCIALE_MO;
                        item.RefIndusMO = tmp.REF_INDUSTRIELLE_MO;
                        item.RefComMT = tmp.REF_COMMERCIALE_MT;
                        item.RefIndusMT = tmp.REF_INDUSTRIELLE_MT;
                        item.RefComSim = tmp.REF_COMMERCIALE_SIM;
                        item.Fps = tmp.REF_FICHE_PERSO;
                        item.Nmr = tmp.NUM_OF;
                    }
                    else
                    {
                        if (tmp.NB_MO == 0)
                        {
                            item.RefOptionMaterielMO = tmp.OPTION_MATERIEL_MO;
                            item.RefComMO = tmp.REF_COMMERCIALE_MO;
                            item.RefIndusMO = tmp.REF_INDUSTRIELLE_MO;
                        }
                        if (tmp.NB_MT == 0)
                        {
                            item.RefOptionMaterielMT = tmp.OPTION_MATERIEL_MT;
                            item.RefComMT = tmp.REF_COMMERCIALE_MT;
                            item.RefIndusMT = tmp.REF_INDUSTRIELLE_MT;
                        }
                        if (tmp.NB_SIM == 0)
                        {
                            item.RefOptionlogSim = tmp.OPTIONS_LOGICIELLES;
                            item.RefComSim = tmp.REF_COMMERCIALE_SIM;
                        }
                    }
                }
            }
        }

        public ActionResult PNC(int? typedata)
        {
            nonConformiteInfo = new InfoNonConformite();
            nonConformiteInfo.TypeData = typedata;
            return View(nonConformiteInfo);
        }
        [HttpPost, ActionName("GestionPNC")]
        [ValidateAntiForgeryToken]
        public ActionResult GestionPNC(InfoNonConformite nc)
        {
            uint id_demande = UInt32.Parse(Request.Form["ID"]);
            string tmp = Request.Form["Datatype"];
            uint typedatademande = UInt32.Parse(Request.Form["Datatype"]);
            int status_demande = Int32.Parse(Request.Form["item.StatusNCnullable"]);
            GestionNonConformite gnc = new GestionNonConformite();
            gnc.ChangeSatus(id_demande, status_demande);

            return RedirectToAction("PNC", "OFSafir", new { typedata = typedatademande });
        }
        [HttpPost, ActionName("GestionTypePNC")]
        [ValidateAntiForgeryToken]
        public ActionResult GestionTypePNC(InfoNonConformite typedata)
        {
            uint typedataDemande = UInt32.Parse(Request.Form["InfoStatusPNCnullable"]);

            return RedirectToAction("PNC", "OFSafir", new { typedata = typedataDemande });
            //return RedirectToAction("PNC");
        }
        

    }
}