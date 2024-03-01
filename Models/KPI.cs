using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class KPI
    {
        public double OTD { get; set; }
        public double OTR { get; set; }
        public int FRC { get; set; }
        public int CA { get; set; }
        public int SD { get; set; }

        public  KPI ()
        {
            OTD = 0;
            OTR = 0;
            CA = 0;
            FRC = 0;
            SD = 0;
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            DateTime now =  DateTime.Now.AddDays(-1);
            int semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = now.Year;
            List <KPI_PROD>  listkpi = _db.KPI_PROD.Where(p => (int)p.Annee == annee && (short)p.Semaine == semaine).ToList();
            FRC = _db.FRC.Where(d => d.Datetime.Year >= annee && d.Valide == true).ToList().Count();
            SD = _db.ACCIDENT.Where(a => a.Date.Year >= annee && a.Type < 4).ToList().Count();
            _db.Dispose();
            if (listkpi!= null && listkpi.Count>0)
            {
                OTD = listkpi.First().OTDByWeek;
                OTR = listkpi.First().OTRByWeek;
                CA = listkpi.First().CA;
            }
        }
    }
}