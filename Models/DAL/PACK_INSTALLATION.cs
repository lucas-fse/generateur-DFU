//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenerateurDFUSafir.Models.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PACK_INSTALLATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PACK_INSTALLATION()
        {
            this.FICHE_PERSO = new HashSet<FICHE_PERSO>();
        }
    
        public long ID { get; set; }
        public string NUM_COMMANDE_CLIENT { get; set; }
        public string NUM_AFFAIRE { get; set; }
        public Nullable<long> ID_CLIENT { get; set; }
    
        public virtual CLIENT CLIENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FICHE_PERSO> FICHE_PERSO { get; set; }
    }
}
