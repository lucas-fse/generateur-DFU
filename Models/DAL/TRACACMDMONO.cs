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
    
    public partial class TRACACMDMONO
    {
        public long IndexCmd { get; set; }
        public string SOHNUM { get; set; }
        public string SOPLIN { get; set; }
        public string STAT { get; set; }
        public string BPCORD { get; set; }
        public string BPAADD { get; set; }
        public string STCORIG { get; set; }
        public short STATUSFPS { get; set; }
        public Nullable<System.DateTime> DateVerif { get; set; }
        public Nullable<System.DateTime> CREDAT_0 { get; set; }
        public Nullable<System.DateTime> SHIDAT { get; set; }
        public Nullable<System.DateTime> EXTDLVDAT_0 { get; set; }
        public Nullable<System.DateTime> DEMDLVDAT_0 { get; set; }
        public Nullable<System.DateTime> ORDDDAT_0 { get; set; }
        public string NomClient { get; set; }
        public Nullable<System.DateTime> DateCloture { get; set; }
        public string Commentaire { get; set; }
        public Nullable<System.DateTime> DateEffectiveClient { get; set; }
        public string STcEnCharge { get; set; }
        public string ZFPCONTROL_0 { get; set; }
        public string ZCOMCONT_0 { get; set; }
        public Nullable<short> QTY { get; set; }
        public string PRODUIT { get; set; }
        public string VAL_FPARAM_0 { get; set; }
        public string CODE_IDs { get; set; }
        public string CPLREF { get; set; }
        public string RASSOTX { get; set; }
        public string RASSORX { get; set; }
        public string CONF_BTS { get; set; }
        public string NMR_CLE { get; set; }
        public string CPLTCLE { get; set; }
        public string CANAL1 { get; set; }
        public string CANAL2 { get; set; }
        public string APASS { get; set; }
        public string ERRFICHE { get; set; }
        public string TYPEANALYSE { get; set; }
        public string TCSORIG { get; set; }
        public Nullable<short> GestionStatus { get; set; }
    
        public virtual STATUSFPS STATUSFPS1 { get; set; }
        public virtual STATUSRELANCE STATUSRELANCE { get; set; }
    }
}
