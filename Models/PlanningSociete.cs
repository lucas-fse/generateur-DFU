using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class PlanningSociete
    {
        public List<LignePlanning> Planning { get; set; }
        public List<string> PlanningValues { get; set; }

        public List<string> ServicesList { get; set; }

        public PlanningSociete(int numsemaine, string filtre)
        {

            Planning = new List<LignePlanning>();
            ServicesList = new List<string>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            PlanningValues = db.PLANNING_VALUE.Select(i => i.Status.Trim()).ToList();

            DateTime now = DateTime.Now;

            int annee = now.Year;

            var query = db.OPERATEURS.Where(i => i.PRESTAT == null && (i.FINCONTRAT == null || i.FINCONTRAT > now)).OrderBy(i => i.NOM);

            if(query != null && query.Count() > 0)
            {
                foreach(OPERATEURS op in query.ToList())
                {
                    List<PLANNING_OPERATEUR> po = op.PLANNING_OPERATEUR.Where(j => j.Semaine == numsemaine && j.Annee == annee).ToList();

                    // Si il n'y a pas d'entrée dans la base pour la semaine donnée on l'a rajoute
                    if(po.Count() == 0) {
                        PLANNING_OPERATEUR np = new PLANNING_OPERATEUR();
                        np.Semaine = numsemaine;
                        np.Annee = annee;
                        np.ID_STC = op.ID;


                        np.LundiMatin = 2;
                        np.LundiApresmidi = 2;
                        np.MardiMatin = 2;
                        np.MardiApresmidi = 2;
                        np.MercrediMatin = 2;
                        np.MercrediApresmidi = 2;
                        np.JeudiMatin = 2;
                        np.JeudiApresmidi = 2;
                        np.VendrediMatin = 2;
                        np.VendrediApresmidi = 2;
                        

                        db.PLANNING_OPERATEUR.Add(np);
                        op.PLANNING_OPERATEUR.Add(np);
                    }
                }

                db.SaveChanges();

                foreach(OPERATEURS i in query.ToList())
                {
                    LignePlanning l = new LignePlanning();

                    l.Nom = i.NOM;
                    l.Prenom = i.PRENOM;
                    l.Service = i.SERVICE.Trim();

                    l.Matin = new List<int>();
                    l.ApresMidi = new List<int>();

                    PLANNING_OPERATEUR po = db.PLANNING_OPERATEUR.Where(j => j.ID_STC == i.ID && j.Semaine == numsemaine && j.Annee == annee).First();

                    l.Matin.Add((int) po.LundiMatin);
                    l.Matin.Add((int) po.MardiMatin);
                    l.Matin.Add((int) po.MercrediMatin);
                    l.Matin.Add((int) po.JeudiMatin);
                    l.Matin.Add((int) po.VendrediMatin);

                    l.ApresMidi.Add((int) po.LundiApresmidi);
                    l.ApresMidi.Add((int)po.MardiApresmidi);
                    l.ApresMidi.Add((int)po.MercrediApresmidi);
                    l.ApresMidi.Add((int)po.JeudiApresmidi);
                    l.ApresMidi.Add((int)po.VendrediApresmidi);

                    Planning.Add(l);

                    if(!ServicesList.Contains(l.Service))
                    {
                        ServicesList.Add(l.Service);
                    }
                }

            }

        }

    }

    public class LignePlanning
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public string Service { get; set; }

        public List<int> Matin { get; set; }
        public List<int> ApresMidi { get; set; }

    }
}