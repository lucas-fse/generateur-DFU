using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public class OffreX3
    {
        // liste des offre X3
        public List<Offre> GetListOffreX3(DateTime date)
        {
            if (date== null) { date = DateTime.Now; }
            DateTime FirstDayofWeeknow = new DateTime(date.Year, date.Month, date.Day);
            DateTime LastDayofWeeknow = FirstDayofWeeknow;
            while (FirstDayofWeeknow.DayOfWeek != DayOfWeek.Monday)
            {
                FirstDayofWeeknow = FirstDayofWeeknow.AddDays(-1);
            }
            LastDayofWeeknow = FirstDayofWeeknow.AddDays(7);
            x160Entities _db = new x160Entities();
            PEGASE_PROD2Entities2 _db2 = new PEGASE_PROD2Entities2();
            var query = _db.SQUOTE.Where(p => p.ZDATREL1_0 > FirstDayofWeeknow && p.ZDATREL1_0 < LastDayofWeeknow && p.ZREL1OK_0 == 0);
            if (query!= null && query.Count()>0)
            {
                List<SQUOTE> ListOffre = query.ToList();
            }
            return null;
        } 
        
    }
    public class Offre
    {
        public string Sdhnum { get; set; }
        public string RaisonSocial { get; set; }
        public DateTime DateRelance { get; set; }
        public bool IsRelance { get; set; }
    }
}