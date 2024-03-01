using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class OFView
    {
        public string numOF { get; set; }
        public string numCommande { get; set; }
        public string refIndu { get; set; }
        public DateTime? dateExpe { get; set; }
        public bool stock { get; set; }
        public DateTime? dateDebut { get; set; }
        public int quantite { get; set; }
        public int specs { get; set; }
        public double duree { get; set; }
        public String poste { get; set; }
        public String pole { get; set; }
        public String couleur { get; set; }
        public bool rupture { get; set; }
        public int rang { get; set; }
        public int etat { get; set; }
        public string Description { get; set; }
    }
}