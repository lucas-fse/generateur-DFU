using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class GestionOp
    {
        public List<OPERATEURS> Operateurs { get; set; }
        //public int IdMax { get; set; }
        public string JsonListOperateurs { get; set; }
        //Filtres pour l'affichage des salariés
        public List<string> Service
        {
            get
            {
                List<string> _Service = new List<string>() { "---", "Production", "R&D", "STC", "TCS", "Marketing" };
                return _Service;
            }
        }
        public List<string> SousService
        {
            get
            {
                List<string> _Service = new List<string>() { "---", "FAB", "LOG", "ADM", };
                return _Service;
            }
        }
        public string ServiceSelectionne { get; set; }
        public GestionOp()
        {
            ServiceSelectionne = "ALL";
            //Initialisation de l'attribut List Opérateurs
            Operateurs = new List<OPERATEURS>();

            //Récupération de la Base de Données
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            pEGASE_PROD2Entities.Configuration.ProxyCreationEnabled = false;
            //Requête sur la BD pour récupérer la liste de salarié du service "Prod"
            List<OPERATEURS> ListeCompleteOperateurs = pEGASE_PROD2Entities.OPERATEURS.OrderBy(o => o.NOM).ToList();


            foreach (OPERATEURS o in ListeCompleteOperateurs)
            {
                //Test pour n'avoir que les opérateurs avec des contratcs en cour dans la liste
                DateTime now = DateTime.Now;
                if (o.FINCONTRAT > now || o.FINCONTRAT == null)
                {
                    Operateurs.Add(o);
                }
            }

            foreach (OPERATEURS o in Operateurs)
            {
                //Si le lien vers le photo de profil existe, il est corrigé pour bien pointé sur le fichier
                if (!string.IsNullOrEmpty(o.PATHA))
                {
                    o.PATHA = "../operateurs/" + o.PATHA;
                }
                if (!string.IsNullOrEmpty(o.PATHB))
                {
                    o.PATHB = "../operateurs/" + o.PATHB;
                }
            }

            ToJson();
        }

        public GestionOp(string Service,string sousservice,int pole)
        {
            ServiceSelectionne = Service;
            //Initialisation de l'attribut List Opérateurs
            Operateurs = new List<OPERATEURS>();
            List<OPERATEURS> ListeCompleteOperateurs = new List<OPERATEURS>();

            //Récupération de la Base de Données
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            pEGASE_PROD2Entities.Configuration.ProxyCreationEnabled = false;
            //Requête sur la BD pour récupérer la liste de salarié du service "Prod"

            if (Service == "ALL")
            {
                ListeCompleteOperateurs = pEGASE_PROD2Entities.OPERATEURS.OrderBy(o => o.NOM).ToList();
            }
            else
            {   
                if (!String.IsNullOrWhiteSpace(sousservice))
                {
                    ListeCompleteOperateurs = pEGASE_PROD2Entities.OPERATEURS.Where(o => o.SERVICE.Contains(Service) && o.SousService.Contains(sousservice)).OrderBy(o => o.NOM).ToList();
                }
                else
                {
                    ListeCompleteOperateurs = pEGASE_PROD2Entities.OPERATEURS.Where(o => o.SERVICE.Contains(Service)).OrderBy(o => o.NOM).ToList();
                }
            }

            foreach (OPERATEURS o in ListeCompleteOperateurs)
            {
                //Test pour n'avoir que les opérateurs avec des contratcs en cour dans la liste
                DateTime now = DateTime.Now;
                if ((o.FINCONTRAT > now || o.FINCONTRAT == null) && ((o.POLE != null &&  o.POLE ==pole)|| (pole == 1 || pole == 0)))
                {
                    Operateurs.Add(o);
                }
            }

            foreach (OPERATEURS o in Operateurs)
            {
                //Si le lien vers le photo de profil existe, il est corrigé pour bien pointé sur le fichier
                if (!string.IsNullOrEmpty(o.PATHA))
                {
                    o.PATHA = "../operateurs/" + o.PATHA;
                }
                if (!string.IsNullOrEmpty(o.PATHB))
                {
                    o.PATHB = "../operateurs/" + o.PATHB;
                }
            }
            ToJson();
        }

        public void ToJson()
        {
            JsonListOperateurs = JsonConvert.SerializeObject(Operateurs, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
        public void AjoutOp(OPERATEURS op)
        {

        }

    }
}