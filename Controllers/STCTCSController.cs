using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class STCTCSController : Controller
    {
        

        public ActionResult Index(int nbJours = 0)
        {
            GestionSTCOperateur gestion = new GestionSTCOperateur();
            gestion.GestionSTCOperateur_update(nbJours);

            return View(gestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexPost(GestionSTCOperateur app, int nbJours = 0)
        {
             app.SavePlanning();
            return RedirectToAction("Index", new { nbJours = nbJours });
        }

        public ActionResult GestionSTC(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "STCTCS");
            }
            else
            {
                DataOperateursSTC ope = new DataOperateursSTC();
                    ope.DataOperateursSTCinit((long)id,0,0);
                return View(ope);
            }
        }

        public ActionResult Edit(long? idstc)
        {

            return RedirectToAction("GestionFPS", "STCTCS", new { idstc = idstc,idtcs= 0, statusdemande = 0 });
        }
 
        public ActionResult GestionFPS(int? idstc,int? idtcs, int? statusdemande)
        {
            FPSEncours fps = new FPSEncours();            
            if (idstc != null && idstc != 0)
            {
                fps.NameSTC = (int)idstc;
            }
            else
            {
                fps.NameSTC = (int)0;
            }
            if (idtcs != null && idtcs != 0)
            {
                fps.NameTCS = (int)idtcs;
            }
            else
            {
                fps.NameTCS = (int)0;
            }
            if (statusdemande != null && statusdemande != 0)
            {
                fps.StatusViewFps = (int)statusdemande;
            }
            else
            {
                fps.StatusViewFps = (int)0;
            }
            fps.TRACAFPsUpdate();
            return View(fps);
        }
        public ActionResult GestionFPSFull()
        {
            FPSEncours fps = new FPSEncours();
            
                fps.NameSTC = (int)-1;
            
            
                fps.StatusViewFps = (int)3;
            
            fps.TRACAFPsUpdate();
            return View(fps);
        }
        public ActionResult GestionMono(int? idstc, int? idtcs, int? statusdemande)
        {
            MonoEncours ligne = new MonoEncours();
            if (idstc != null && idstc != 0)
            {
                ligne.NameSTC = (int)idstc;
            }
            else
            {
                ligne.NameSTC = (int)0;
            }
            if (idtcs != null && idtcs != 0)
            {
                ligne.NameTCS = (int)idtcs;
            }
            else
            {
                ligne.NameTCS = (int)0;
            }
            if (statusdemande != null && statusdemande != 0)
            {
                ligne.StatusViewMono = (int)statusdemande;
            }
            else
            {
                ligne.StatusViewMono = (int)0;
            }
            ligne.TRACAFPsUpdate();
            return View(ligne);
        }
        public ActionResult GestionCmd(int? idstc, int? idtcs, int? idstatus)
        {
            CmdEnCours data = new CmdEnCours();
            if (idstc != null &&  idstc != 0)
            {
                data.NameSTC = (int)idstc;
            }
            else
            {
                data.NameSTC = (int)0;
            }
            if (idtcs != null &&  idtcs != 0)
            {
                data.NameTCS = (int)idtcs;
            }
            else
            {
                data.NameTCS = (int)0;
            }
            if (idstatus != null && idstatus != 0)
            {
                data.StatusCmd = (int)idstatus;
            }
            else
            {
                data.StatusCmd = (int)0;
            }
            data.CmdEncours();
            return View(data);
        }
       
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGestionFPS(FPSEncours ope)
        {
            FPSEncours encours = new FPSEncours();
            
            encours.TRACAFPsUpdate();
            if (ope.NameSTC != 0)
            {
                if (ope.FormTRACACMD != null)
                {
                    //foreach (var t in ope.TRACACMDsfiltres)
                    //{
                    List<TRACACMD> dsfirst = encours.TRACACMDs.Where(p => ope.FormTRACACMD.IndexCMD.Equals(p.IndexCMD)).ToList();
                    string commande = "";
                    if (dsfirst.Count != 0)
                    {
                        commande = dsfirst.First().SOHNUM.Trim();
                    }
                    if (string.IsNullOrWhiteSpace(commande))
                    {
                        commande = "Non definie";
                    }
                    List<TRACACMD> ds = encours.TRACACMDs.Where(p => ope.FormTRACACMD.IndexCMD.Equals(p.IndexCMD) || p.SOHNUM.Contains(commande)).ToList();
                   
                    foreach (var d in ds)
                    {
                        if (ope.FormTRACACMD.DelaiEstimation != null)
                        {
                            d.DelaiEstimation = (short)ope.FormTRACACMD.DelaiEstimation;
                        }
                        if (ope.FormTRACACMD.DateEffectiveClient != null)
                        {
                            d.DateEffectiveClient = ope.FormTRACACMD.DateEffectiveClient;
                        }
                        else if (ope.FormTRACACMD.EXTDLVDAT != null)
                        {
                            d.DateEffectiveClient = d.EXTDLVDAT;
                        }
                        if (ope.FormTRACACMD.StatusFPSnullable != null)
                        {
                            d.StatusFPS = (short)ope.FormTRACACMD.StatusFPSnullable;
                        }
                        //if (d.StatusFPS == 0)
                        //{
                        //    d.STATUS = ope.FormTRACACMD.StatusFPS;
                        //}
                        if ((d.StatusFPS == 4) || (d.StatusFPS == 5))
                        {
                            d.DateCloture = DateTime.Now;
                        }
                        else
                        {
                            d.DateCloture = null;
                        }
                        var query = encours.GetOperateursSTC.Where(o => o.ID == ope.FormTRACACMD.STCEnChargeInt);
                        if (query != null && query.Count() == 1)
                        {
                            d.STcEnCharge = encours.GetOperateursSTC.Where(o => o.ID == ope.FormTRACACMD.STCEnChargeInt).First().INITIAL;
                        }
                        else
                        {
                            //d.STcEnCharge = encours.GetOperateursSTC.Where(o => o.ID == ope.NameSTC).First().INITIAL;
                            // d.STcEnCharge = "null";
                        }
                        if (!String.IsNullOrWhiteSpace(ope.CommentaireSpe) && ope.CommentaireSpe.Length<500)
                        {                            
                            d.Commentaire = ope.CommentaireSpe;
                        }
                        else if (ope.FormTRACACMD.AjoutCommentaire != 0)
                        {
                            string tmp = ope.ListCommentaireFicheDate.Where(p => p.Value == ope.FormTRACACMD.AjoutCommentaire.ToString()).First().Text;
                            tmp = tmp.Replace("%Date", DateTime.Now.ToString("dd/MM/yyyy"));
                            d.Commentaire += tmp + "\r\n";
                        if (d.Commentaire.Length > 500) { d.Commentaire= d.Commentaire.Substring(0, 500); }
                        }
                        encours.UpdatetracaFPS(d,false);
                    }
                }
            }
            return RedirectToAction("GestionFPS", "STCTCS", new { idstc = ope.NameSTC, idtcs = ope.NameTCS, statusdemande = ope.StatusViewFps });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGestionMono(MonoEncours ope)
        {
            MonoEncours encours = new MonoEncours();

            encours.TRACAFPsUpdate();
            if (ope.NameSTC != 0)
            {
                if (ope.FormTRACACMD != null)
                {
                    //foreach (var t in ope.TRACACMDsfiltres)
                    //{
                    List<TRACACMDMONO> ds = encours.TRACACMDs.Where(p => ope.FormTRACACMD.IndexCmd.Equals(p.IndexCmd)).ToList();

                    foreach (var d in ds)
                    {
                        //if (ope.FormTRACACMD.DelaiEstimation != null)
                        //{
                        //    d.DelaiEstimation = (short)ope.FormTRACACMD.DelaiEstimation;
                        //}
                        if (ope.FormTRACACMD.DateEffectiveClient != null)
                        {
                            d.DateEffectiveClient = ope.FormTRACACMD.DateEffectiveClient;
                        }
                        else if (ope.FormTRACACMD.EXTDLVDAT_0 != null)
                        {
                            d.DateEffectiveClient = d.EXTDLVDAT_0;
                        }
                        if (ope.FormTRACACMD.STATUSnullable != null)
                        {
                            d.STATUSFPS = (short)ope.FormTRACACMD.STATUSnullable;
                        }
                        if (d.STATUSFPS == 0)
                        {
                            d.STATUSFPS = ope.FormTRACACMD.STATUSFPS;
                        }
                        if ((d.STATUSFPS == 4) || (d.STATUSFPS == 5))
                        {
                            d.DateCloture = DateTime.Now;
                        }
                        else
                        {
                            d.DateCloture = null;
                        }
                        var query = encours.GetOperateursSTC.Where(o => o.ID == ope.FormTRACACMD.STCEnChargeInt);
                        if (query != null && query.Count() == 1)
                        {
                            d.STcEnCharge = encours.GetOperateursSTC.Where(o => o.ID == ope.FormTRACACMD.STCEnChargeInt).First().INITIAL;
                        }
                        else
                        {
                            //d.STcEnCharge = encours.GetOperateursSTC.Where(o => o.ID == ope.NameSTC).First().INITIAL;
                            // d.STcEnCharge = "null";
                        }
                        if (!String.IsNullOrWhiteSpace(ope.CommentaireSpe) && ope.CommentaireSpe.Length < 500)
                        {
                            d.Commentaire = ope.CommentaireSpe;
                        }
                        else if (ope.FormTRACACMD.AjoutCommentaire != 0)
                        {
                            string tmp = ope.ListCommentaireFicheDate.Where(p => p.Value == ope.FormTRACACMD.AjoutCommentaire.ToString()).First().Text;
                            tmp = tmp.Replace("%Date", DateTime.Now.ToString("dd/MM/yyyy"));
                            d.Commentaire += tmp + "\r\n";
                            if (d.Commentaire.Length > 500) { d.Commentaire = d.Commentaire.Substring(0, 500); }
                        }
                        encours.UpdatetracaFPS(d);
                    }

                    //}
                }
            }
            return RedirectToAction("GestionMono", "STCTCS", new { idstc = ope.NameSTC, idtcs = ope.NameTCS, statusdemande = ope.StatusViewMono });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGestionCmd(CmdEnCours ope)
        {
           
            if ((ope.NameSTC != 0) || (ope.NameTCS != 0))
            {
                int statusdemande = 0;
                if (ope.FormTRACACMD != null)
                {
                    if (ope.FormTRACACMD.Statusnullable != null)
                    {
                        statusdemande = (short)ope.FormTRACACMD.Statusnullable;
                    }

                    if (ope.FormTRACACMD.TypeData.Equals(0))
                    {
                        FPSEncours encours = new FPSEncours();
                        encours.TRACAFPsUpdate();
                        TRACACMD ds = encours.TRACACMDs.Where(p => ope.FormTRACACMD.SOHNUM.Equals(p.SOHNUM) && ope.FormTRACACMD.SOPLIN.Equals(p.SOPLIN)).ToList().First();
                        ds.StatusFPS = (short)statusdemande;
                        encours.UpdatetracaFPS(ds,false);                        
                    }
                    else
                    {
                        MonoEncours encours = new MonoEncours();
                        encours.TRACAFPsUpdate();
                        TRACACMDMONO ds = encours.TRACACMDs.Where(p => ope.FormTRACACMD.SOHNUM.Equals(p.SOHNUM) && ope.FormTRACACMD.SOPLIN.Equals(p.SOPLIN)).ToList().First();
                        ds.STATUSFPS = (short)statusdemande;
                        encours.UpdatetracaFPS(ds);
                    }
                }
                
            }
            return RedirectToAction("GestionCmd", "STCTCS", new { idstc = ope.NameSTC, idtcs = ope.NameTCS, idStatus = ope.StatusCmd });
        }
        [HttpPost, ActionName("EditStatus")]
        public ActionResult EditStatus(FPSEncours ope)
        {
            FPSEncours encours = new FPSEncours();
            encours.TRACAFPsUpdate();
            if (ope.FormTRACACMD.StatusNullable != null)
            {
                List<TRACACMD> dsfirst = encours.TRACACMDs.Where(p => ope.FormTRACACMD.IndexCMD.Equals(p.IndexCMD)).ToList();
                string commande = "";
                string ficheparam = "";
                if (dsfirst.Count != 0)
                {
                    commande = dsfirst.First().SOHNUM.Trim();
                    ficheparam = dsfirst.First().VAL_FPARAM_0.Trim();
                }
                if (string.IsNullOrWhiteSpace(commande))
                {
                    commande = "Non definie";
                }
                List<TRACACMD> ds = encours.TRACACMDs.Where(p => ope.FormTRACACMD.IndexCMD.Equals(p.IndexCMD) || (p.SOHNUM.Contains(commande) && p.VAL_FPARAM_0.Contains(ficheparam))).ToList();

                foreach (var d in ds)
                {
                    d.GestionStatus = ope.FormTRACACMD.StatusNullable;
                    d.StatusFPS = 0;
                    encours.UpdatetracaFPS(d, true); ;
                }

            }
            return RedirectToAction("GestionFPS", "STCTCS", new { idstc = ope.NameSTC, idtcs = ope.NameTCS, statusdemande = ope.StatusViewFps });
        }

    }
}