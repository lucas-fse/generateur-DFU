using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class Utilitaire
    {

        // Cette méthode permet de générer une couleur à partir d'une chaine de caractère.
        public static string colorHash(string str)
        {
            MD5 md5 = MD5.Create();

            byte[] a = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));
            string R = ((a[0] + a[1] + a[2] + a[3] + a[4] + a[5]) % 225).ToString("X");
            string G = ((a[6] + a[7] + a[8] + a[9] + a[10]) % 225).ToString("X");
            string B = ((a[11] + a[12] + a[13] + a[14] + a[15]) % 225).ToString("X");

            if (R.Length < 2) R = "0" + R;
            if (G.Length < 2) G = "0" + G;
            if (B.Length < 2) B = "0" + B;

            return "#" + R + G + B;
        }
    }
}