
//using GenerateurDFUSafir.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class GestionTraitementOFs
    {
        // liste des opérateurs de production
        public List<DataOperateurProd> DataOperateurs;
        // liste des alea pouvant être declare
        

        public GestionTraitementOFs()
        {
            DataOperateurs = GestionOperateursProd.GestionTraitementOperateurs("PRODONLY");
        }
    }
}