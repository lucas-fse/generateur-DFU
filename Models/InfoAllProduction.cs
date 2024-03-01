using GenerateurDFUSafir.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public static  class InfoAllProduction
    {
        public static List<OrdreFabrication>  InfoAllOfProduction()
        {
            OfX3 ofs = new OfX3();
            List<OrdreFabrication> Listofs  = ofs.ListOfAllProductionX3();

            return Listofs;
        }
        public static List<OrdreFabrication> InfoAllOfProduction(DateTime date)
        {
            OfX3 ofs = new OfX3();
            List<OrdreFabrication> Listofs = ofs.ListOfAllProductionX3Bis(date);

            return Listofs;
        }
        
    }
}