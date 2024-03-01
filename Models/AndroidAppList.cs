using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class AndroidAppList
    {

        public List <LIENS_OUTILS> AppList { get; set; }

        public AndroidAppList() { 

            PEGASE_GENERIQUEEntities db = new PEGASE_GENERIQUEEntities();

            AppList = db.LIENS_OUTILS.ToList();
        }
    }

}