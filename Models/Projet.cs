using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class Projet
    {
        public Projet()
        {
           
        }

        public long ID { get; set; }

        public long IDSOUSPROJET { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "Référence")]
        public string NomSousProjet { get; set; }
        [Required]
        [Display(Name = "Projet générique/transverse")]
        public bool Affichage { get; set; }
    
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fin de projet")]
        public DateTime DateFinProjet { get; set; }
        [Required]
        [StringLength(40)]
        [Display(Name = "Description")]
        public string TitreSousProjet { get; set; }
        [StringLength(40)]
        [Display(Name = "Commentaire")]
        public string Commentaire { get; set; }

        [StringLength(5)]
        [Display(Name = "Service (RD, MKT)")]
        public string Service { get; set; }
    }
}