﻿using GenerateurDFUSafir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class RechercheDeveloppementController : Controller
    {
        RetDInformation InformationUser;
        // GET: RechercheDeveloppement
        public ActionResult Index()
        {
            RetDInformation data = new RetDInformation("RD");
            data.RefreshDataUser();
            return View(data);
        }


        // GET: RechercheDeveloppement/Create
        [HttpGet, ActionName("Saisi")]
        public ActionResult SaisiEditGet(int ID, int? semaine, int? annee)
        {
            SaisiModel model = new SaisiModel("RD");
            
            if ((semaine != null) && (annee!= null))
            {
                model.Init(ID, (int)semaine, (int)annee);
            }
            else 
            { 
                model.Init(ID);
            }
            return View(model);
        }
        
        // POST: RechercheDeveloppement/Create

        [HttpPost, ActionName("Saisi")]
        public ActionResult SaisiEditPost(string submitButton, SaisiModel saisimanuel)
        {
            bool savedata = false;
            int semainedemande = -1;
            
            Regex filtreSemaine = new Regex("^S[0-9]{1}");
            if (submitButton == null)
            {
                //ajout d'une ligne projet
                savedata = true;
                if (saisimanuel.ADDNewSSprojet != 00)
                {
                    // on ajout le projet
                    semainedemande = saisimanuel.SemaineEnCours;
                }
            }
            else if (submitButton.Equals("Save"))
            {                                                           
                savedata = true;
                semainedemande = saisimanuel.SemaineEnCours;
            }
            else if (filtreSemaine.IsMatch(submitButton))
            {
                savedata = true;
                try
                {
                    //semaine demande par l'utilisateur de la saisi
                    semainedemande = Convert.ToInt32( submitButton.Substring(1, 2));
                }
                catch
                {

                }
            }
            SaisiModel model = new SaisiModel("RD");
            model.Init((short)saisimanuel.ID, semainedemande, saisimanuel.Annee);
            if (savedata == true)
            {
                model.AddNewWeek(saisimanuel);
            }
            // mettre a jour la base de données correspondant à la complitude de saisie des temps
            model.CheckTimeByWeek((short)saisimanuel.ID, saisimanuel.SemaineEnCours,saisimanuel.Annee);
            return RedirectToAction("Saisi", new { ID = (short)saisimanuel.ID, semaine = semainedemande, annee = saisimanuel.Annee } );
        }


        // GET: RechercheDeveloppement/Create
        [HttpGet, ActionName("Projet")]
        public ActionResult ProjetEditGet(int? nmrsousprojet)
        {
           
            AddSousProjet model = new AddSousProjet("RD");
            model.Init(nmrsousprojet);
            return View(model);
        }

        // POST: RechercheDeveloppement/Create

        [HttpPost, ActionName("Projet")]
        public ActionResult ProjetEditPost(string submitButton, AddSousProjet sousprojet)
        {
            bool savedata = false;
            int semainedemande = -1;
            
            if (submitButton == null)
            {
                //ajout d'une ligne projet
                savedata = false;
                if (sousprojet != null)
                {
                    //AddSousProjet model = new AddSousProjet("RD");
                    //model.Init(sousprojet.ADDNewSSprojet);
                    return RedirectToAction("Projet", new { nmrsousprojet = (short)sousprojet.ADDNewSSprojet });
                    //return View(model);
                }
                return RedirectToAction("Index");
            }
            else if (sousprojet.CodeValidation != null && sousprojet.CodeValidation.Equals("RD1"))
            {
                if (submitButton.ToUpper().Equals("ADD PROJET"))
                {
                    savedata = true;
                    sousprojet.Service = "RD";
                    sousprojet.UpdateOrSaveProjet();

                }
                else if (submitButton.ToUpper().Equals("SAVE PROJET"))
                {
                    savedata = true;
                    sousprojet.Service = "RD";
                    sousprojet.UpdateOrSaveProjet();
                }
                return RedirectToAction("Index");
            }
            else
            {
                int tmp = sousprojet.ADDNewSSprojet;
                sousprojet = new AddSousProjet("RD");
                sousprojet.Init(tmp) ;
                return View(sousprojet);
            }
        }
    }
}
