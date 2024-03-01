using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class CONTROLE_QUALITE
    {
        public string DateString
        {
            get
            {
                if (Date != null)
                {
                    return ((DateTime)Date).ToString("ddMMyyyy");
                }
                else
                {
                    return "UNDEFINED";
                }
            }
        }
        public string TYPE_CAUSEString
        {
            get
            {
                if (ID_TYPE_CAUSE != null)
                {
                    return TYPE_CAUSE.Valeur;
                }
                else
                {
                    return "";
                }
            }
        }
        public string TYPE_ANOMALIEString
        {
            get
            {
                if (ID_TYPE_ANOMALIE != null)
                {
                    return TYPE_ANOMALIE.Valeur;
                }
                else
                {
                    return "";
                }
            }
        }
       
        public  CONTROLE_QUALITE()
        {
            NMROF = "";
         ITEMREF = "";
        ITEMDESCRIPT = "";
         Conforme = 0;
         Description = "";
       UrlImage = "";
    }
    }
}