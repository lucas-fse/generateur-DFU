using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class QRQC
    {
        public string DateOuvertureString
        {
            get
            {
                return (DateOuverture).ToString("yyyy-MM-dd");
            }
        }
        public string DateClotureString
        {
            get
            {
                if (DateCloture != null)
                {
                    return ((DateTime)DateCloture).ToString("yyyy-MM-dd");
                }
                else
                {
                    return "";
                }
            }
        }
        public string DateSuivisString
        {
            get
            {
                if (DateSuivis != null)
                {
                    return ((DateTime)DateSuivis).ToString("yyyy-MM-dd");
                }
                else
                {
                    return "";
                }
            }
        }
    }
}