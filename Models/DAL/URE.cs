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
    
    public partial class URE
    {
        public long ID { get; set; }
        public long NMR_CLE { get; set; }
        public string NMR_SERIE { get; set; }
        public string DOSSIER { get; set; }
        public Nullable<short> VERSION { get; set; }
        public string COMMENTAIRE { get; set; }
        public Nullable<long> CODE_ID { get; set; }
        public Nullable<long> CODE_ID2 { get; set; }
        public string REFERENCE { get; set; }
        public string MASQUE_BP { get; set; }
        public string TEMPS_HM { get; set; }
        public Nullable<short> GAMME_FREQ { get; set; }
        public Nullable<short> CANAL { get; set; }
        public Nullable<short> OPTION1 { get; set; }
        public Nullable<System.DateTime> DATE_PROG { get; set; }
        public string DATE { get; set; }
        public string Erreur { get; set; }
        public string COMPL_REF { get; set; }
    }
}
