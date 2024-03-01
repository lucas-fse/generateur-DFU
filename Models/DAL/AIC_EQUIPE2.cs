using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class AIC_EQUIPE
    {
        public string IDString
        {
            get
            {
                return ID.ToString();
            }
        }
        public string DateString
        {
            get
            {
                if (Date != null)
                {
                    return ((DateTime)Date).ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }
        public string DateResolutionString
        {
            get
            {
                if (DateResolution != null)
                {
                    return ((DateTime)DateResolution).ToString("yyyy-MM-dd");
                }
                else
                {
                    return "";
                }
            }
        }
        public bool Check
        {
            get
            {
                if (Status == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}