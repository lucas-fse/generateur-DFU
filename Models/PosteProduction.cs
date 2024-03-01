using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public static class PosteProduction
    {
        public static string[] DEFPOSTEBIDIR = new string[] { "A5/01", "C6/00", "P0/01", "P1/01", "P1/02", "P1/03", "P1/99", "P2/01", "P3/01", "A5/01" };
        public static string[] DEFPOSTEMONO = new string[] { "A5/00", "A1/00", "A2/00", "A3/00", "A4/00", "A6/00", "A5/00", "C1/00", "C5/00", "C3/00", "C3/01", "C3/02", "C3/99", "C4/00" };
        // la baie larysis est attribue au bidir et le banc radio au monodirectionnel
        //string[] DEFPOSTETEST = new string[] { "A5/00", "A5/01" };
        public static string[] DEFPOSTETEST = new string[] { };

        public static bool IsPosteIsInPole(string poste,int? pole)
        {
            if (poste == null)
            {
                return false;
            }
            if ((pole==2) && DEFPOSTEBIDIR.Contains(poste.Trim()))
            {
                return true;
            }
            else if ((pole == 3) && DEFPOSTEMONO.Contains(poste.Trim()))
            {
                return true;
            }
            else if (pole !=2 && pole != 3)
            {
                return true;
            }
            return false;
        }
    }
}