using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public static class EtiquetteBac
    {
       public static List<ITEM_LOCALISATION> GetEtiquetteBac(string itemref)
        {
            PEGASE_STAMPEntities db = new PEGASE_STAMPEntities();
            return db.ITEM_LOCALISATION.Where(i => i.ITEMREF.StartsWith(itemref)).ToList();
        }
        public static ITEM_LOCALISATION SetEtiquetteBac(string nameID, string nameRef, string namechar, string nameqtr)
        {
            long? id = null;
            PEGASE_STAMPEntities db = new PEGASE_STAMPEntities();
            try
            {
                id = Convert.ToInt64(nameID);
            }
            catch
            {
                id = null;
            }
            int qtr = 0;
            try { qtr = Convert.ToInt32(nameqtr); } catch { }
            ITEM_LOCALISATION et = null;
            if (id!= null&& id!=0)
            {
                 et = db.ITEM_LOCALISATION.Where(p => p.ID == id).First();
                et.ITEMREF = nameRef;
                et.Chariot = namechar;
                et.QtrByBox = qtr;
                db.SaveChanges();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(nameRef) && !string.IsNullOrWhiteSpace(namechar))
                {
                    et = new ITEM_LOCALISATION();
                    et.ITEMREF = nameRef;
                    et.Chariot = namechar;
                    et.QtrByBox = qtr;
                    db.ITEM_LOCALISATION.Add(et);
                    db.SaveChanges();
                }
            }
            return et;
        }
        public static List<ITEM_LOCALISATION> GetAllList()
        {
            PEGASE_STAMPEntities db = new PEGASE_STAMPEntities();
            return db.ITEM_LOCALISATION.ToList();
        }
    }
}