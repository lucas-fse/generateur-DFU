using DALPEGASE;
using GenerateurDFUSafir.Models.DAL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class AccueilKPI
    {
        public double OtdMois { get; set; }
        public double OtdSemaine { get; set; }
        public double OtrMois { get; set; }
        public double OtrSemaine { get; set; }

        public double ObjectifOtd { get; set; }
        public double ObjectifOtr { get; set; }

        public int FrcMois { get; set; }
        public int FrcAnnee { get; set; }
        public int ObjectifFrc { get; set; }
        public int CaMois { get; set; }

        public int CaMoisPrec { get; set; }
        public int CaAnnee { get; set; }
        public int ObjectifCa { get; set; }

        public int NbJourSansAccident { get; set; }
        public int NbActesDangereux { get; set; }
        public int NbSituationDangereuse { get; set; }
        public int NbPresqueAccident { get; set; }
        public int NbSoinsBenins { get; set; }


        public AccueilKPI()
        {
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            // OTD / OTR
            DateTime now = DateTime.Now;
            int semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = now.Year;

            SemaineAnnee[] numsemaines = new SemaineAnnee[4];
            
            for(int i = 0; i < 4; i++)
            {
                if ((--semaine) <= 0)
                {
                    semaine = 52;
                    annee--;
                }

                numsemaines[i] = new SemaineAnnee(semaine, annee);
                
            }

            SemaineAnnee s1 = numsemaines[0];
            SemaineAnnee s2 = numsemaines[1];
            SemaineAnnee s3 = numsemaines[2];
            SemaineAnnee s4 = numsemaines[3];

            var query = db.KPI_PROD.Where(p =>
                s1.Annee == p.Annee && s1.Semaine == p.Semaine
             || s2.Annee == p.Annee && s2.Semaine == p.Semaine
             || s3.Annee == p.Annee && s3.Semaine == p.Semaine
             || s4.Annee == p.Annee && s4.Semaine == p.Semaine);

            if (query!= null && query.Count()>0)
            {
                List<KPI_PROD> list_kpi = query.ToList();
                double avg_otd = 0;
                double avg_otr = 0;

                foreach(var kpi in list_kpi) {
                    avg_otd += kpi.OTDByWeek;
                    avg_otr += kpi.OTRByWeek;
                }

                this.OtdMois = avg_otd / 4;
                this.OtrMois = avg_otr / 4;

                KPI_PROD last_week = list_kpi[3];
                this.ObjectifOtd = (double) last_week.ObjectifOTD;
                this.ObjectifOtr = (double) last_week.ObjectifOTR;

                this.OtrSemaine = last_week.OTRByWeek;
                this.OtdSemaine = last_week.OTDByWeek;
            }


            // FRC
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            this.FrcMois = db.FRC.Where(p => p.Datetime >= firstDayOfMonth && p.Valide == true).Count();

            DateTime firstDayOfYear = new DateTime(now.Year, 1, 1);
            this.FrcAnnee = db.FRC.Where(p => p.Datetime >= firstDayOfYear && p.Valide == true).Count();

            var query2 = db.DATA_GENERIQUE.Where(p => p.ID == 3);
            if(query2 != null && query2.Count() == 1)
            {
                this.ObjectifFrc = (int) query2.First().Value1;
            }

            // CA
            DateTime lastMonth = now.AddMonths(-1);

            var query3 = db.KPI_CA.Where(p => p.Mois == now.Month && p.Annee == now.Year);
            if(query3 != null && query3.Count() == 1)
            {
                this.CaMois = (int) query3.First().CA;
            }

            var query5 = db.KPI_CA.Where(p => p.Mois == lastMonth.Month && p.Annee == lastMonth.Year);
            if (query5 != null && query5.Count() == 1)
            {
                this.CaMoisPrec = (int)query5.First().CA;
            }

            int somme = 0;
            var query6 = db.KPI_CA.Where(p => p.Annee == now.Year);
            if (query6 != null && query6.Count() > 0)
            {
                 foreach(var ca in query6.ToList()) {
                    somme += (int) ca.CA;
                 }
            }

            this.CaAnnee = somme;

            // Accident
            var query4 = db.DATA_GENERIQUE.Where(p => p.ID == 2);
            if (query4 != null && query4.Count() == 1)
            {
                this.NbJourSansAccident = (int) query4.First().Value1;
            }

            this.NbActesDangereux = db.ACCIDENT.Where(p => p.Type == 1 && p.Date > firstDayOfYear).Count();
            this.NbSituationDangereuse = db.ACCIDENT.Where(p => p.Type == 2 && p.Date > firstDayOfYear).Count();
            this.NbPresqueAccident = db.ACCIDENT.Where(p => p.Type == 3 && p.Date > firstDayOfYear).Count();
            this.NbSoinsBenins = db.ACCIDENT.Where(p => p.Type == 4 && p.Date > firstDayOfYear).Count();


        }



    }

    class SemaineAnnee
    {
        public int Semaine { get; set; }
        public int Annee { get; set; }

        public SemaineAnnee(int semaine, int annee)
        {
            this.Semaine = semaine;
            this.Annee = annee;
        }
    };
}