using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class ProduitMono
    {        
        public string NomProduit { get; set; }
        public bool ImprimerRapport { get; set;}
        public List<TYPEBPBYLIGNETYPE> ListOption = new List<TYPEBPBYLIGNETYPE>();   
    }
    public class TYPEBPBYLIGNETYPE
    {
        public string NomOption { get; set; }
        public bool AfficherNomOption { get; set; }
        public int optionMaskDebut { get; set; }
        public int optionMaskTaille { get; set; }
        public List<OptionDispo> Option { get; set; }
        public int ptxG { get; set; }
        public int ptxD { get; set; }
        public int pty { get; set; }
        public string Refimage { get; set; }
    }
    public class OptionDispo
    {
        public string Libelle { get; set; }
        public string valeur { get; set; }
        public string imageG { get; set; }
        public string imageD { get; set; }
        public string CarteHard { get; set; }
        public string Commentaire { get; set; }
    }
}