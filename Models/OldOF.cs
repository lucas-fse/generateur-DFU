using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{    
    public class OldOF
    {
        public List<StatusOldOf> ListOldOF { get; set; }
        public OldOF()
        {
            ListOldOF = new List<StatusOldOf>();
            PEGASE_CONTROLEntities db = new PEGASE_CONTROLEntities();

            //var query = db.HISTORIQUE_CONTROL.Where (i=>i.IsPack == true).OrderByDescending(d => d.Date).Select(p=>p.NumOF).GroupBy(p=>p);
            var query = db.HISTORIQUE_CONTROL.Where(o => o.IsPack == true).GroupBy(u => u.NumOF).Select(i => new { i.Key , Datea = i.Max(p => p.Date) }).OrderByDescending(d => d.Datea); ;

            if (query != null && query.Count() > 0)
            {
                foreach (var of in query.ToList())
                {

                    StatusOldOf s = new StatusOldOf();
                    s.NmrOf = of.Key;
                    s.date = (DateTime)of.Datea;
                    ListOldOF.Add(s);
                    if (ListOldOF.Count() > 15) { break; }
                }
            }
            // recuperation status des of
            
            foreach (var of in ListOldOF)
            {
                DataTable rawResult1 = null;
                ModelOF1.RequeteStatusOF(of.NmrOf, ref rawResult1);
                // liste des contenu de l'of
                if (rawResult1 != null && rawResult1.Rows != null && rawResult1.Rows.Count == 1)
                {
                    of.StatusOf = Convert.ToInt32(rawResult1.Rows[0]["MFGTRKFLG_0"].ToString());
                    of.StatusOfString = rawResult1.Rows[0]["Statut_OF"].ToString();
                }
            }
            ListOldOF = ListOldOF.OrderBy(p =>  p.StatusOf).ThenByDescending(o=>o.date).Take(5).ToList();
        }
    }
    public class StatusOldOf
    {
        public string NmrOf { get; set; }
        public DateTime date { get; set; }
        public int StatusOf { get; set; }
        public string StatusOfString { get; set; }
    }
}