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
    
    public partial class CONTROLE_QUALITE
    {
        public long ID { get; set; }
        public Nullable<long> ID_OPERATEUR { get; set; }
        public string NMROF { get; set; }
        public string ITEMREF { get; set; }
        public string ITEMDESCRIPT { get; set; }
        public Nullable<int> Conforme { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<long> ID_TYPE_CAUSE { get; set; }
        public Nullable<long> ID_TYPE_ANOMALIE { get; set; }
        public Nullable<System.DateTime> DateModif { get; set; }
    
        public virtual OPERATEURS OPERATEURS { get; set; }
        public virtual TYPE_ANOMALIE TYPE_ANOMALIE { get; set; }
        public virtual TYPE_CAUSE TYPE_CAUSE { get; set; }
    }
}
