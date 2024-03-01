using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GenerateurDFUSafir.Models
{
    public class AddSousProjet
    {
        public List<SelectListItem> ProjetSuppl { get; set; }
        public string Service { get; set; }
        public int ADDNewSSprojet { get; set; }
        public int ProjetEnCours { get; set; }
        public Projet view { get; set; }
        public string CodeValidation { get; set; }
        public AddSousProjet()
        {

        }
        public AddSousProjet(string service)
        {
            Service = service;
            ProjetSuppl = new List<SelectListItem>();
            RetDInformation prj = new RetDInformation(service);
            List<SOUSPROJET> projetobligatoire = prj.ListSousProjetNonObligatoire();
            ProjetSuppl.Add(new SelectListItem { Text = "Nouveau Projet", Value = "00" });
            foreach (var p in projetobligatoire)
            {
                if ((p.IDSOUSPROJET % 100) == 0)
                {
                    long idprojet = p.IDSOUSPROJET / 100;
                    ProjetSuppl.Add(new SelectListItem { Text = p.NomSousProjet + p.TitreSousProjet, Value = idprojet.ToString() });
                }
            }
            CodeValidation = "";
        }
        public void Init(int? sousprojet)
        {
            RetDInformation prj = new RetDInformation(Service);
            List<SOUSPROJET> projetobligatoire = prj.ListSousProjetNonObligatoire();
            SOUSPROJET tmp;
            if (sousprojet != null  && sousprojet !=0)
            {
                tmp = projetobligatoire.Where(p => ((int)(p.IDSOUSPROJET / 100)).Equals((int)sousprojet)).First();
                ADDNewSSprojet = (int)tmp.IDSOUSPROJET /100;
            }
            else
            {
                tmp = new SOUSPROJET();
                tmp.NomSousProjet = "Nouveau Projet";
                tmp.Service= Service;
                tmp.TitreSousProjet = "Description";
                tmp.Affichage=false;
                tmp.DateFinProjet = DateTime.Now;
                tmp.Commentaire = "";
                ADDNewSSprojet = 0;
            }
            view = new Projet();
            view.NomSousProjet = tmp.NomSousProjet.Trim();
            view.Service = tmp.Service.Trim();
            view.TitreSousProjet = tmp.TitreSousProjet.Trim();
            if (tmp.Commentaire == null) { tmp.Commentaire = ""; }
            view.Commentaire = tmp.Commentaire.Trim();
            view.Affichage = tmp.Affichage;
            view.IDSOUSPROJET = tmp.IDSOUSPROJET;
            view.ID = tmp.ID;
            if (tmp.DateFinProjet== null)
            {
                view.DateFinProjet = DateTime.Now;
            }
            else
            {
                view.DateFinProjet = (DateTime)tmp.DateFinProjet;
            }
            
        }
        public bool  UpdateOrSaveProjet()
        {
            RetDInformation prj = new RetDInformation(Service);
            if (ADDNewSSprojet != 0)
            {
                
                List<SOUSPROJET> projetobligatoire = prj.ListSousProjetNonObligatoire();
                SOUSPROJET tmp;
                tmp = projetobligatoire.Where(p => ((int)(p.IDSOUSPROJET / 100)).Equals((int)ADDNewSSprojet)).First();
                if (tmp.NomSousProjet.Trim() == view.NomSousProjet && tmp.Service.Trim() == Service)
                {
                    tmp.TitreSousProjet = view.TitreSousProjet;
                    tmp.Service = Service;
                    tmp.Commentaire = view.Commentaire;
                    tmp.Affichage = view.Affichage;
                    tmp.DateFinProjet = view.DateFinProjet;
                    prj.UpdateSSProjet(tmp);
                    return true; 
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //ajouter un nouveau projet
                // le projet n'hesiste pas 
                // IL faut attribuer un idprojet 
                // 0-9900 pour projet generique
                // > 70000 pour les autres
                long idproj = 0;
                if (view.Affichage)
                {
                    
                    List<SOUSPROJET> projetobligatoire = prj.ListALLSousProjetALLService().Where(a=>a.Affichage == true).OrderBy(p=>p.IDSOUSPROJET).ToList();
                     idproj = projetobligatoire.Last().IDSOUSPROJET;
                    
                }
                else
                {
                    
                    List<SOUSPROJET> projetNonobligatoire = prj.ListALLSousProjetALLService().Where(a => a.Affichage == false).OrderBy(p => p.IDSOUSPROJET).ToList();
                    idproj = projetNonobligatoire.Last().IDSOUSPROJET;

                }
                // on incremente de 100 pour laisser de la place si un jour on gerer des item des les projets
                idproj += 100;
                SOUSPROJET tmp = new SOUSPROJET();
                tmp.IDSOUSPROJET = idproj;
                tmp.Service = Service;
                tmp.NomSousProjet = view.NomSousProjet;
                tmp.TitreSousProjet = view.TitreSousProjet;
                tmp.Commentaire = view.Commentaire;
                tmp.Affichage = view.Affichage;
                tmp.DateFinProjet = view.DateFinProjet;
                prj.CreateSSProjet(tmp);

                return true;
            }
        }
    }
}