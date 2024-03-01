using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class DataOperateursSTC
    {
        public long ID { get; set; }
        public String Nom { get; set; }

        public String Prenom { get; set; }
        public String Initial { get; set; }
        public bool Animateur { get; set; }
        public DateTime? Anniversaire { get; set; }
        public string Photo { get; set; }

        public FluxCommandeSemaineGraphPerso fluxCommande { get; set; }
        public CommandeEnCoursGraphPerso commandeEnCours { get; set; }
        public EvolutionCommandeGraphPerso evolutionCommande { get; set; }
        public PLANNING_OPERATEUR planning_op { get; set; } /* tous les créneaux pour un operateur */

        public DataOperateursSTC()
        {
            planning_op = new PLANNING_OPERATEUR();
        }
        public void DataOperateursSTCinit(long idStc,int planningAnnee,int PlanningSemaine)
        {
            this.ID = idStc;
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            OPERATEURS op = db.OPERATEURS.Where(p=> p.ID == idStc).First();
            Prenom = op.PRENOM;
            Nom = op.NOM;
            Photo = op.PATHB;
            Initial = op.INITIAL;				 
            fluxCommande = new FluxCommandeSemaineGraphPerso(idStc);
            commandeEnCours = new CommandeEnCoursGraphPerso(idStc);
            evolutionCommande = new EvolutionCommandeGraphPerso(idStc);
            var query = db.PLANNING_OPERATEUR.Where(p => p.ID_STC == idStc && p.Annee == planningAnnee && p.Semaine == PlanningSemaine);
            if (query != null && query .Count()>0)
            {
                planning_op = query.First();
            }
            else
            {
                PLANNING_OPERATEUR tmp = new PLANNING_OPERATEUR();
                tmp.ID_STC = idStc;
                tmp.Annee = planningAnnee;
                tmp.Semaine = PlanningSemaine;
                tmp.LundiMatin = 2;
                tmp.LundiApresmidi = 2;
                tmp.MardiMatin = 2;
                tmp.MardiApresmidi = 2;
                tmp.MercrediMatin = 2;
                tmp.MercrediApresmidi = 2;
                tmp.JeudiMatin = 2;
                tmp.JeudiApresmidi = 2;
                tmp.VendrediMatin = 2;
                tmp.VendrediApresmidi = 2;
                tmp.SamediMatin = 1;
                tmp.SamediApresmidi = 1;
                tmp.DimancheMatin = 1;
                tmp.DimancheApresmidi = 1;
                db.PLANNING_OPERATEUR.Add(tmp);
                db.SaveChanges();
                planning_op = tmp;
            }
            

            
        }
       

        //public PLANNING_VALUE planning_value { get; set; }
    }


    public class FluxCommandeSemaineGraphPerso
    {

        public List<double> nb { get; set; }
        public List<double> ca { get; set; }

        public FluxCommandeSemaineGraphPerso(long idStc)
        {
            nb = new List<double>() { 0, 0, 0 };
            ca = new List<double>() { 0, 0, 0 };

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year && i.ID_STC == idStc);

            if (query != null && query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");

                KPI_STC kpi_stc = query.First();


                nb[0] = (double)kpi_stc.Value1;
                nb[1] = (double)(kpi_stc.Value3 + kpi_stc.Value4 + kpi_stc.Value5 + kpi_stc.Value6);
                nb[2] = (double)kpi_stc.Value2;

                ca[0] = (double)kpi_stc.Value10;
                ca[1] = (double)kpi_stc.Value11;
                ca[2] = (double)kpi_stc.Value12;
            }
        }

    }

    public class CommandeEnCoursGraphPerso
    {
        public List<double> nbCommandes { get; set; }

        public CommandeEnCoursGraphPerso(long idStc)
        {

            nbCommandes = new List<double>() { 0, 0, 0 };

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year && i.ID_STC == idStc);

            if (query != null && query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");

                KPI_STC s = query.First();
                nbCommandes[0] = (double)s.Value1;
                nbCommandes[1] = (double)s.Value5;
                nbCommandes[2] = (double)s.Value4;
            }
        }
    }

    public class EvolutionCommandeGraphPerso
    {
        public List<int> NumSemaines { get; set; }
        public List<double> datasEntree { get; set; }
        public List<double> datasEncours { get; set; }
        public List<double> datasTermine { get; set; }

        public EvolutionCommandeGraphPerso(long idStc)
        {
            datasEntree = new List<double>();
            datasEncours = new List<double>();
            datasTermine = new List<double>();

            NumSemaines = new List<int>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;

            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = now.Year;

            // On récupère les 6 dernières semaines au fur et à mesure
            for (int i = 0; i < 6; i++)
            {
                var query = db.KPI_STC.Where(j => j.Semaine == numsemaine && j.Annee == annee && j.ID_STC == idStc);

                if (query != null && query.Count() > 0)
                {
                    KPI_STC s = query.First();


                    datasEntree.Insert(0, s.Value1 != null ? (double)s.Value1 : 0);
                    datasEncours.Insert(0, (double) (s.Value3 + s.Value4 + s.Value5 + s.Value6));
                    datasTermine.Insert(0, s.Value2 != null ? (double)s.Value2 : 0);

                }

                NumSemaines.Insert(0, numsemaine);

                numsemaine--;
                if (numsemaine < 0)
                {
                    numsemaine = 52;
                    annee--;
                }
            }


        }
    }
}