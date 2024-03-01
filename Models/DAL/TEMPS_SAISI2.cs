using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class TEMPS_SAISI
    {
        public void Init(short annee,short semaine, int projet,short days1,short days2, short days3, short days4, short days5, short days6, short days7)
        {
            this.Annee = annee;
            this.Semaine = semaine;
            this.SOUSPROJET_ID = projet;
            this.Days1 = days1;
            this.Days2 = days2;
            this.Days3 = days3;
            this.Days4 = days4;
            this.Days5 = days5;
            this.Days6 = days6;
            this.Days7 = days4;
        }
    }
}