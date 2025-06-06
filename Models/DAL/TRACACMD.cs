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
    
    public partial class TRACACMD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRACACMD()
        {
            this.TRACA_CHANGE_GESTIONSTATUS = new HashSet<TRACA_CHANGE_GESTIONSTATUS>();
        }
    
        public long IndexCMD { get; set; }
        public string SOHNUM { get; set; }
        public string SOPLIN { get; set; }
        public string STAT { get; set; }
        public string BPCORD { get; set; }
        public string BPAADD { get; set; }
        public string CFGFLDALP2 { get; set; }
        public string CFGFLDALP3 { get; set; }
        public string CFGFLDALP4 { get; set; }
        public string VAL_FPARAM_0 { get; set; }
        public string ZFPS { get; set; }
        public string PLASTRON { get; set; }
        public string SYNCHRO { get; set; }
        public string ZVERSION { get; set; }
        public string ERRFICHE { get; set; }
        public Nullable<System.DateTime> DateVerif { get; set; }
        public Nullable<System.DateTime> CREDAT_0 { get; set; }
        public Nullable<System.DateTime> SHIDAT { get; set; }
        public Nullable<System.DateTime> EXTDLVDAT { get; set; }
        public Nullable<System.DateTime> DEMDLVDAT_0 { get; set; }
        public Nullable<System.DateTime> ORDDAT_0 { get; set; }
        public string STCORIG { get; set; }
        public Nullable<long> REFFICHE { get; set; }
        public bool IsMOInside { get; set; }
        public Nullable<int> TempsRealiseFPSHeure { get; set; }
        public string NomClient { get; set; }
        public short StatusFPS { get; set; }
        public short StatusPlastron { get; set; }
        public Nullable<short> DelaiEstimation { get; set; }
        public Nullable<System.DateTime> DateCloture { get; set; }
        public short Relance { get; set; }
        public string Commentaire { get; set; }
        public Nullable<System.DateTime> DateEffectiveClient { get; set; }
        public string STcEnCharge { get; set; }
        public string ZFPCONTROL_0 { get; set; }
        public string ZCOMCONT_0 { get; set; }
        public Nullable<short> QTY { get; set; }
        public string TCSORIG { get; set; }
        public string ITEMREF { get; set; }
        public string NomTech { get; set; }
        public string TelTech { get; set; }
        public string MobTech { get; set; }
        public string MailTech { get; set; }
        public Nullable<short> GestionStatus { get; set; }
        public Nullable<bool> CmdRecurrente { get; set; }
    
        public virtual DELAIFPS DELAIFPS { get; set; }
        public virtual STATUSFPS STATUSFPS1 { get; set; }
        public virtual STATUSFPS STATUSFPS2 { get; set; }
        public virtual STATUSRELANCE STATUSRELANCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRACA_CHANGE_GESTIONSTATUS> TRACA_CHANGE_GESTIONSTATUS { get; set; }
    }
}
