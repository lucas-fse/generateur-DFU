using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class OPERATEURS
    {
        public string PourcentageComplet
        {
            get
            {
                int nbnotValidWeek = 0;
                int semaineStart = 25;
                int SemaineEnCours = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                int AnneeeEnCours = CultureInfo.CurrentCulture.Calendar.GetYear(DateTime.Now);
                int nbsweekok = TEMPS_SEMAINE.Where(s => s.Annee == AnneeeEnCours && s.Semaine >= semaineStart && s.Complete == true).Count();
                int nbwweektobeok = 0;
                if ((SemaineEnCours- semaineStart)<0) { nbwweektobeok = SemaineEnCours; }
                else { nbwweektobeok = SemaineEnCours - semaineStart; }
                if ((nbwweektobeok- nbsweekok)<=0)
                {
                    return 100.ToString();
                }
                else if ((nbwweektobeok - nbsweekok) <= 1)
                {
                    return 80.ToString();
                }
                else if ((nbwweektobeok - nbsweekok) <= 2)
                {
                    return 50.ToString();
                }
                else 
                {
                    return 10.ToString();
                }
            }
        }

    }
}