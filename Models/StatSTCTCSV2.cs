using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    /// <summary>
    /// recuperation des données pour la generation des 
    /// KPI du service STC
    /// </summary>
    /// 
    
    public class StatSTCTCSV2
    {
        public void getSetKpiStc(DateTime date,int? nbSemaine)
        {
            if (nbSemaine == null) { nbSemaine = 6; }
            PEGASE_CHECKFPSEntities1 db = new PEGASE_CHECKFPSEntities1();
            for (int s = 0; s > nbSemaine; s++)
            {
                int semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(-s * 7), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                DateTime firstDayOfWeek = getPremierJourSemaine(semaine, date.Year);
                DateTime lastDayOfWeek = getDernierJourSemaine(semaine, date.Year);

                var query = db.TRACACMD.Where(p => p.CREDAT_0 > firstDayOfWeek && p.CREDAT_0 < lastDayOfWeek);
            }
        }
        private static DateTime getPremierJourSemaine(int numeroSemaine, int annee)
        {
            DateTime temp = new DateTime(annee, 1, 1);
            int compteurSemaine = 1;

            //D'abord, on va se caler sur le premier jeudi de l'année
            //Le premier jeudi dans l'année représente la première semaine

            while (temp.DayOfWeek != DayOfWeek.Thursday)
            {
                temp = temp.AddDays(1);
            }

            //Maintenant, on revient sur le lundi précédent
            while (temp.DayOfWeek != DayOfWeek.Monday)
                temp = temp.AddDays(-1);

            //On va avancer de 7 jours en 7 jours pour trouver notre semaine
            while (compteurSemaine < numeroSemaine)
            {
                temp = temp.AddDays(7);
                compteurSemaine++;
            }

            return temp;
        }
        private static DateTime getDernierJourSemaine(int numeroSemaine, int annee)
        {
            DateTime temp = new DateTime(annee, 1, 1);
            int compteurSemaine = 1;

            //D'abord, on va se caler sur le premier jeudi de l'année
            //Le premier jeudi dans l'année représente la première semaine

            while (temp.DayOfWeek != DayOfWeek.Thursday)
            {
                temp = temp.AddDays(1);
            }

            //Maintenant, on revient sur le lundi précédent
            while (temp.DayOfWeek != DayOfWeek.Sunday)
                temp = temp.AddDays(1);

            //On va avancer de 7 jours en 7 jours pour trouver notre semaine
            while (compteurSemaine < numeroSemaine)
            {
                temp = temp.AddDays(7);
                compteurSemaine++;
            }

            return temp;
        }

    }

    public class DataSTCV2
    {
        public int NbATraiterEntree { get; set; }
        public int NbAttenteValidFps { get; set; }
        public int NbAttenteValidPlastron { get; set; }
        public int NbEnAttente { get; set; }
        public int NbTermine { get; set; }
        public List<TRACACMD> ListCmd { get; set; }
    }
}