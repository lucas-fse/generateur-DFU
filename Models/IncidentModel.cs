using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class IncidentModel
    {
        public int ID { get; set; }
        public string NomPrenom{get{return "";} }

        public IEnumerable<SelectListItem> ListItemIncident
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = "Suggestion d'amélioration", Value="0"},
                    new SelectListItem {Text = "Incident", Value="1"}                    
                };
            }
        }
        public IEnumerable<SelectListItem> ListItemLieu
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = "Production", Value="0"},
                    new SelectListItem {Text = "R&D", Value="1"},
                    new SelectListItem {Text = "Salle de réunion", Value="2"},
                    new SelectListItem {Text = "Commercial", Value="3"},
                    new SelectListItem {Text = "Administratif", Value="4"}
                };
            }
        }
        public string DéclarationTxt { get; set; }
    }
}