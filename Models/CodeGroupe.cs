using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class CodeGroupe
    {
        public string Groupe { get; set; }

        public string SouSGroupe { get; set; }

        public int Code { get; set; }

        public string Erreur { get; set; }

        public bool ischecked{get; set; }

        public string Couleur { get; set; }

        public CodeGroupe(string _Groupe, string _SousGroupe, int _Code, string _Erreur, string _Couleur)
        {
            Groupe = _Groupe;
            SouSGroupe = _SousGroupe;
            Code = _Code;
            Erreur = _Erreur;
            ischecked = false;
            Couleur = _Couleur;
        }

        public CodeGroupe()
        {

        }
    }
}