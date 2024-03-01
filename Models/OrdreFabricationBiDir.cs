using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class OrdreFabricationBiDir
    {
        Dictionary<string, string> StatusOF = new Dictionary<string, string>() { { "0", "Non_généré" }, { "1", "En_Attente" }, { "2", "En_étude" }, { "3", "Edité" }, { "4", "En_cours" }, { "5", "Soldé" }, { "6", "Calcul" } };

        public string ID { get; set; }
        [StringLength(8)]
        [Display(Name = "OF")]
        public String Nmr { get; set; }
        public int Qtr { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "Référence commercial du pack")]
        public string RefComPack { get; set; }
        public string Fpa { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Référence de la fiche FPS")]
        public string Fps { get; set; }
        //public int QtrSim { get; set; }
        //[Required]
        [StringLength(8)]
        [Display(Name = "Référence commercial de la SIM")]
        public string RefComSim { get; set; }
        //[Required]
        [StringLength(10)]
        [Display(Name = "Options logicielles de la SIM")]
        public string RefOptionlogSim { get; set; }
        //public int QtrMT { get; set; }
        // [Required]
        [StringLength(8)]
        [Display(Name = "Référence commercial du MT")]
        public string RefComMT { get; set; }
        //[Required]
        [StringLength(8)]
        [Display(Name = "Référence industrielle du MT")]
        public string RefIndusMT { get; set; }
        //[Required]
        [StringLength(20)]
        [Display(Name = "Référence matériel du MT")]
        public string RefOptionMaterielMT { get; set; }
        //public int QtrMO { get; set; }
        //[Required]
        [StringLength(8)]
        [Display(Name = "Référence commercial du MO")]
        public string RefComMO { get; set; }
        //[Required]
        [StringLength(8)]
        [Display(Name = "Référence industrielle du MO")]
        public string RefIndusMO { get; set; }
        //[Required]
        [StringLength(20)]
        [Display(Name = "Référence matériel du MO")]
        public string RefOptionMaterielMO { get; set; }

        [Display(Name = "Commande synchro")]
        public string CommandeSynchro { get; set; }
        
        [Display(Name = "Version logicielle")]
        public string Version { get; set; }


        public bool DataGenerated{get;set;}
        public DateTime DateLivraison { get; set; }
        public string DateLivraisonAff { get { return DateLivraison.ToString("dd/MM/yyyy"); } }
        public int StatusOf { get; set; }
        public string StatusImg
        {
            get
            {
                if (StatusOf.Equals(0))
                {
                    return "/image/agenou.png";
                }
                else if (StatusOf.Equals(3))
                {
                    return "/image/debout.png";
                }
                else if (StatusOf.Equals(4))
                {
                    return "/image/court.png";
                }
                else
                {
                    return "/image/agenou.png";
                }
                
                
            }
        }
        public string InfoBulle
        {
            get
            {
                string value = "" ;
                StatusOF.TryGetValue(StatusOf.ToString(), out value);
                
                return value;
            }
        }
        public string Adress_client { get; set; }
        public string Code_client { get; set; }
        public string Pays_client { get; set; }
        
        public OrdreFabricationBiDir()
        {

        }
        
        public OrdreFabricationBiDir (string _ID,
        String _Nmr,
        int _Qtr ,
        string _RefComPack ,
        string _Fpa ,
        string _Fps ,
        int _QtrSim ,
        string _RefComSim ,
        string _RefOptionlogSim ,
        int _QtrMT ,
        string _RefComMT ,
        string _RefIndusMT ,
        string _RefOptionMaterielMT,
        int _QtrMO ,
        string _RefComMO ,
        string _RefIndusMO ,
        string _RefOptionMaterielMO ,
        string _CommandeSynchro,
        string _version,
        DateTime _DateLivraison,
        int _StatusOf,
        string _Code_client,
        string _Adress_client,
        string _Pays_client)
        {
            ID = _ID;
            Nmr = _Nmr;
            Qtr =_Qtr;
            RefComPack= _RefComPack;
            Fpa= _Fpa;
            Fps =_Fps;
            //if (!string.IsNullOrWhiteSpace(RefComSim))
            //{ 
            //    QtrSim = 1; 
            //}
            //else
            //{
            //    QtrSim = 0;
            //}
            RefComSim= _RefComSim;
            RefOptionlogSim= _RefOptionlogSim;
            RefComMT = _RefComMT;
            //if (!string.IsNullOrWhiteSpace(RefComMT))
            //{
            //    QtrMT = 1;
            //}
            //else
            //{
            //    QtrMT = 0;
            //}
            RefIndusMT= _RefIndusMT;
            RefOptionMaterielMT = _RefOptionMaterielMT;
            RefComMO = _RefComMO;
            //if (!string.IsNullOrWhiteSpace(RefComMO))
            //{
            //    QtrMO = 1;
            //}
            //else
            //{
            //    QtrMO = 0;
            //}
            RefIndusMO= _RefIndusMO;
            RefOptionMaterielMO = _RefOptionMaterielMO;
            CommandeSynchro = _CommandeSynchro;
            Version = _version;
            //DataGenerated = _genere;
            DateLivraison = _DateLivraison;
            StatusOf = _StatusOf;
            Code_client = _Code_client;
            Adress_client = _Adress_client;
            Pays_client = _Pays_client;
        }
    }
}