using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class InfoKPI
    {
        public List<string> ListOperateur { get; set; }
        public int OTRHebdomadaire { get; set; }
        public int OTRAnnuel { get; set; }
        public int OTDHebdomadaire { get;  set;  }
        public int OTDAnnuel { get; set; }

        public Dictionary <string, OTDOTRGraph> Dicokpi { get; set; }

        public InfoKPI()
        {
            MiseAJourOperateur();
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            DateTime now = DateTime.Now;
            int annee = now.Year;
            List<KPI_PROD>listkpi = _db.KPI_PROD.Where(p => (int)p.Annee == annee).ToList();
            long? ObjectifOTD = _db.DATA_GENERIQUE.Where(d => d.ID == 4).First().Value1;
            long? ObjectifOTR = _db.DATA_GENERIQUE.Where(d => d.ID == 4).First().Value2;
            Dicokpi = new Dictionary<string, OTDOTRGraph>();
            int nb_sem = listkpi.Count();
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            OTDOTRGraph OTD = new OTDOTRGraph();OTD.Name = "OTD"; OTD.Value = new double[nb_sem];OTD.Type = "bar"; OTD.backgroundColor = OTD.GetbackgroundColorValue(0); OTD.borderColor = OTD.GetborderColorValue(0);OTD.Id = "Pourcent";
            OTDOTRGraph OTR = new OTDOTRGraph(); OTR.Name = "OTR"; OTR.Value = new double[nb_sem]; OTR.Type = "bar"; OTR.backgroundColor = OTR.GetbackgroundColorValue(1); OTR.borderColor = OTR.GetborderColorValue(1); OTR.Id = "Pourcent";
            OTDOTRGraph objOTD = new OTDOTRGraph(); objOTD.Name = "Objectif OTD"; objOTD.Value = new double[nb_sem]; objOTD.Type = "line"; objOTD.backgroundColor = objOTD.GetbackgroundColorValue(2); objOTD.borderColor = objOTD.GetborderColorValue(2); objOTD.Id = "Pourcent";
            OTDOTRGraph objOTR = new OTDOTRGraph(); objOTR.Name = "Objectif OTR"; objOTR.Value = new double[nb_sem]; objOTR.Type = "line"; objOTR.backgroundColor = objOTR.GetbackgroundColorValue(3); objOTR.borderColor = objOTR.GetborderColorValue(3); objOTR.Id = "Pourcent";
            OTDOTRGraph CA = new OTDOTRGraph(); CA.Name = "Chiffre d'affaire"; CA.Value = new double[nb_sem]; CA.Type = "line"; CA.backgroundColor = CA.GetbackgroundColorValue(5); CA.borderColor = CA.GetborderColorValue(5); CA.Id = "CA";

            Dicokpi.Add("OTD", OTD);
            Dicokpi.Add("OTR", OTR);
            Dicokpi.Add("Objectif OTD", objOTD);
            Dicokpi.Add("Objectif OTR", objOTR);
            Dicokpi.Add("Chiffre d'affaire", CA);
            double oTRAnnuel = 0; double oTDAnnuel = 0; double oTRHebdomadaire = 0; double oTDHebdomadaire = 0;
            foreach (var sem in listkpi.OrderBy(s=> s.Semaine))
            {
                try
                {
                    if (sem.ObjectifOTD == null) sem.ObjectifOTD = 0;
                    if (sem.ObjectifOTR == null) sem.ObjectifOTR = 0;

                    oTRAnnuel += sem.OTRByWeek;
                    oTDAnnuel += sem.OTDByWeek;

                    OTD.Value[sem.Semaine - 1] = sem.OTDByWeek;
                    OTR.Value[sem.Semaine - 1] = sem.OTRByWeek;
                    objOTD.Value[sem.Semaine - 1] = (double)sem.ObjectifOTD;
                    objOTR.Value[sem.Semaine - 1] = (double)sem.ObjectifOTR;
                    CA.Value[sem.Semaine - 1] = sem.CA;
                    if (num_semaine >= sem.Semaine && num_semaine < (sem.Semaine + 4))
                    {
                        oTRHebdomadaire += sem.OTRByWeek;
                        oTDHebdomadaire += sem.OTDByWeek;
                    }
                }
                catch { }
            }
            oTDAnnuel = oTDAnnuel / listkpi.Count();
            oTRAnnuel = oTRAnnuel / listkpi.Count();
            oTDHebdomadaire = oTDHebdomadaire /  Math.Min( listkpi.Count(),4);
            oTRHebdomadaire = oTRHebdomadaire / Math.Min(listkpi.Count(), 4);
            this.OTDAnnuel = (int)oTDAnnuel;
            this.OTRAnnuel = (int)oTRAnnuel;
            this.OTDHebdomadaire = (int)oTDHebdomadaire;
            this.OTRHebdomadaire = (int)oTRHebdomadaire;
        }
        private void MiseAJourOperateur()
        {
            ListOperateur = new List<string>();
            string path = "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\operateurs\\MiniOperateurs\\";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fileEntries = di.GetFiles("*.png");
            foreach (var fileName in fileEntries)
            {
                ListOperateur.Add(@"/operateurs/MiniOperateurs/" + fileName);
            }
        }
    }
    public class OTDOTRGraph
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double[] Value { get; set; }
        public string Type { get; set; }
        public string GetbackgroundColorValue(int i)
        {
            return (backgroundColorValue[i % backgroundColorValue.Count()]);
        }
        public string GetborderColorValue(int i)
        {
            return (borderColorValue[i % borderColorValue.Count()]);
        }
        private string[] backgroundColorValue = new string[] {
            "rgba(255, 99, 132, 0.7)",
            "rgba(54, 162, 235, 0.7)",
            "rgba(255, 150, 0, 0.7)",
            "rgba(153, 102, 255, 0.7)",
            "rgba(75, 192, 192, 0.7)",
            "rgba(255, 0, 0, 0.7)",
            "rgba(0,255,255,0.7)",
            "rgba(0, 0, 0, 0.65)",
            "rgba(127, 0, 255, 0.8)",
            "rgba(0, 255, 0, 0.4)",
        };
        private string[] borderColorValue = new string[] {
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(255, 159, 64, 1)",
            "rgba(200, 200, 200, 1)",
            "rgba(0, 0, 0, 1)",
            "rgba(0, 255, 0, 1)",
            "rgba(0, 255, 0, 1)",
        };
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
    }
   
}