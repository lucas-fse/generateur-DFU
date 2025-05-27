using GenerateurDFUSafir.DAL;

using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Util;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.WebPages;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace GenerateurDFUSafir.Controllers
{
    public class LogistiqueController : Controller
    {
        public static bool CaptureOCR()
        {
            InfoAmelioration vue = new InfoAmelioration();
            return true;//View("CaptureOCR");
        }

        public ActionResult Index()
        {

            Logistique logistique = new Logistique();
            return View(logistique);
        }

        public ActionResult PbCommandesFournisseur()
        {
            PbCommandesFournisseur pbCommandesFournisseur = new PbCommandesFournisseur();
            return View(pbCommandesFournisseur);
        }
        public ActionResult PbReceptionsFournisseur()
        {

            PbLivraisonsFournisseur pbReceptionsFournisseur = new PbLivraisonsFournisseur();
            return View(pbReceptionsFournisseur);
        }
        public ActionResult PbLivraisonsClient()
        {

            PbLivraisonsClient pbLivraisonsClient = new PbLivraisonsClient();
            return View(pbLivraisonsClient);
        }

        public ActionResult Reassort()
        {

            Reassort reassort = new Reassort();
            return View(reassort);
        }

        [HttpPost]
        public ActionResult FormulaireDeclarationProbleme (FormulaireProblemeDto data)
        {
            if (data == null)
            {
                Session["Message"] = "Aucune donnée soumise";
                Session["MessageType"] = "error";
            }
            else
            {
                string date = data.SaisieDate;
                string typeProbleme = data.TypeProbleme;
                string natureProbleme = data.NatureProbleme;
                string probleme = data.Probleme;
                string resolution = data.Resolu;
                DateTime dateValue;
                DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);

                if (typeProbleme == "CommandeFournisseur")
                {
                    PbCommandesFournisseur obj = new PbCommandesFournisseur();
                    PB_COMMANDES_FOURNISSEUR newObj = new PB_COMMANDES_FOURNISSEUR();
                    newObj.Probleme = probleme;
                    newObj.NaturePB = natureProbleme;
                    newObj.Date = dateValue;
                    if (resolution=="Oui")
                    {
                        newObj.Resolution = true;
                    }
                    else
                    {
                        newObj.Resolution = false;
                    }

                    int res = obj.AddPbCommandesFournisseur(newObj);
                    return RedirectToAction("PbCommandesFournisseur", "Logistique");
                }

                if (typeProbleme == "LivraisonFournisseur")
                {
                    string nCommande = data.NumCommande;
                    PbLivraisonsFournisseur obj = new PbLivraisonsFournisseur();
                    PB_LIVRAISONS_FOURNISSEUR newObj = new PB_LIVRAISONS_FOURNISSEUR();
                    newObj.Probleme = probleme;
                    newObj.NaturePB = natureProbleme;
                    newObj.Date = dateValue;
                    newObj.Ncommande = nCommande;
                    if (resolution == "Oui")
                    {
                        newObj.Resolution = true;
                    }
                    else
                    {
                        newObj.Resolution = false;
                    }

                    int res = obj.AddPbLivraisonsFournisseur(newObj);
                    return RedirectToAction("PbReceptionsFournisseur", "Logistique");
                }

                if (typeProbleme == "LivraisonClient")
                {

                    string nCommande = data.NumCommande;
                    string service = data.Service;
                    PbLivraisonsClient obj = new PbLivraisonsClient();
                    PB_LIVRAISONS_CLIENT newObj = new PB_LIVRAISONS_CLIENT();
                    newObj.Probleme = probleme;
                    newObj.NaturePB = natureProbleme;
                    newObj.Date = dateValue;
                    newObj.Ncommande = nCommande;
                    newObj.Service = service;
                    if (resolution == "Oui")
                    {
                        newObj.Resolution = true;
                    }
                    else
                    {
                        newObj.Resolution = false;
                    }

                    int res = obj.AddPbLivraisonsClient(newObj);
                    Session["Message"] = "Problème enregistré avec succès";
                    Session["MessageType"] = "success";
                    return RedirectToAction("PbLivraisonsClient", "Logistique");
                }
            }
            return RedirectToAction("Index");
        }
    }
}