using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class DataGammeUD
    {
        public string NMROF { get; set; }
        public string Item { get; set; }
        public int Qtr { get; set; }
        public string ItemAff
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Item))
                {
                    return "";
                }
                else
                {
                    return "Référence : " + Item;
                }
            }
        }
        public string QtrAff 
        {
            get
            {
                if (Qtr == 0)
                {
                    return "";
                }
                else
                {
                    return "Quantité : " + Qtr.ToString();
                }
            }
        }
    }
}