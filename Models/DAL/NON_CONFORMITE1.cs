using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial  class NON_CONFORMITE
    {
        public short? StatusNCnullable { get; set; }
        public IEnumerable<SelectListItem> ListStatus
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                var query = _db.STATUS_PNC;

                if (query.Count() > 0)
                {
                    foreach (var t in query.ToList())
                    {
                        // status 5  clos non validé (cmd supprimée ) 6 check periodique
                        if(Status==0)
                        {
                            if (t.IDSTATUS == 0 || t.IDSTATUS == 2)
                            {
                                result.Add(new SelectListItem { Text = t.STATUS, Value = t.IDSTATUS.ToString() });
                            }
                        }
                        else
                        {
                            result.Add(new SelectListItem { Text = t.STATUS, Value = t.IDSTATUS.ToString() });
                        }
                            
                        
                    }
                }
                return result;
            }
        }
    }
}