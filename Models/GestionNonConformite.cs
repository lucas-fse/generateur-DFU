using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GenerateurDFUSafir.Models
{
    public class GestionNonConformite
    {

        public string AddNonConformite(NON_CONFORMITE newnc)
        {
            string result = "";
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                OPERATEURS op = _db.OPERATEURS.Where(o => o.ID == newnc.OperateursID).FirstOrDefault();
                DateTime now = DateTime.Now;
                string recherche = "PNC" + now.ToString("yy") + now.ToString("MM");
                List<NON_CONFORMITE> Listnc = _db.NON_CONFORMITE.Where(p => p.NmrChronoS.StartsWith(recherche)).ToList();
                int cpt = Listnc.Count() + 1;
                NON_CONFORMITE nc = new NON_CONFORMITE();
                nc.Item = newnc.Item;                
                nc.Qtr = newnc.Qtr;                
                nc.Datetime = now;
                nc.DescriptionItem = newnc.DescriptionItem;
                nc.DescriptionUser = newnc.DescriptionUser;
                nc.OperateursID = op.ID;
                nc.NmrOF = newnc.NmrOF;
                nc.Status = 0;
                string tmp = now.ToString("yy") + now.ToString("MM") + cpt.ToString("0000");
                nc.NmrChrono = Convert.ToInt64(now.ToString("yy") + now.ToString("MM") + cpt.ToString("0000"));
                nc.NmrChronoS = "PNC" + nc.NmrChrono.ToString();
                _db.NON_CONFORMITE.Add(nc);
                _db.SaveChanges();
                result = nc.NmrChronoS;
            }
            catch(Exception e)
            {
                result = "";
            }
            return result;
        }
        public bool ChangeSatus(UInt32 id,int status)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            List<NON_CONFORMITE> nc = _db.NON_CONFORMITE.Where(p => p.ID == id).ToList();
            if (nc != null && nc.Count ==1)
            {
                NON_CONFORMITE tmp = _db.NON_CONFORMITE.Where(p => p.ID == id).First();
                tmp.Status = status;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}