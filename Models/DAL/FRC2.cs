using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class FRC
    {
        public string DatetimeString { get { return Datetime.ToString("dd/MM/yyyy"); } }
        public int jourDeclare
        {
            get
            {
                return Datetime.Day;
            }
        }
        public string Descriptions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Description))
                {
                    return "";
                }
                else
                {
                    return Description;
                }
            }
        }
        public string Commentaires
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Commentaire))
                {
                    return "";
                }
                else
                {
                    return Commentaire;
                }
            }
        }
        public string Actions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Action))
                {
                    return "";
                }
                else
                {
                    return Action;
                }
            }
        }
    }
}