using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class StructLabel
    {
        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string LABELNAME = "LABELNAME";
        public const string NMRSERIE1 = "NSE";
        public const string NMRSERIE2 = "NSE1";

        public const string ITMREF = "ITMREF";
        public const string LOCALISATION  = "Localisation";
        public const string NUM = "NUM";
        // <summary>
        /// 
        /// </summary>
        public const string CODE_PRODUIT1 = "CODE_PRODUIT";
        public const string OF1 = "OF";
        public const string CDE1 = "CDE";
        public const string CODE_PRODUIT2 = "CODE_PRODUIT1";
        public const string OF2 = "OF1";
        public const string CDE2 = "CDE1";
        public const string SERIAL = "SERIAL";
        public const string NC = "NC";
        public const string AUTRE = "AUTRE";
        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string DATE = "DATE";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string BANDE_FREQ = "BANDE_FREQ";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string FCC_ID = "FCC_ID";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string FOURNISSEUR = "FOURNISSEUR";


        /// <summary>
        /// Ligne LABELNAME
        /// </summary>
        public const string REF_ENSEMBLE = "REF_ENSEMBLE";

        /// <summary>
        /// Ligne REFCOMMO
        /// </summary>
        public const string REF_RECEPTEUR = "REF_RECEPTEUR";

        /// <summary>
        /// Ligne REFCOMMT
        /// </summary>
        public const string COMP_REF_RECEPTEUR = "COMP_REF_RECEPTEUR";

        /// <summary>
        /// Ligne REFCOMMO
        /// </summary>
        public const string NUM_SERIE_RECEPTEUR = "NUM_SERIE_RECEPTEUR";

        /// <summary>
        /// Ligne REFCOMMT
        /// </summary>
        public const string DOSSIER_RECEPTEUR = "DOSSIER_RECEPTEUR";

        /// <summary>
        /// Ligne REFCOMSIM
        /// </summary>
        public const string CODEIDENT_RECEPTEUR = "CODEIDENT_RECEPTEUR";

        /// <summary>
        /// Ligne REFCOMPACK
        /// </summary>
        public const string ARRET_PASSIF = "ARRET_PASSIF";

        /// <summary>
        /// Ligne NUMMO
        /// </summary>
        public const string ALIMENTATION = "ALIMENTATION";

        /// <summary>
        /// Ligne NUMMT
        /// </summary>
        public const string PONT_COUPLE_TXT = "PONT_COUPLE_TXT";

        /// <summary>
        /// Ligne NUMSIM
        /// </summary>
        public const string CANAL = "CANAL";

        /// <summary>
        /// Ligne CMDCLIENT
        /// </summary>
        public const string CMDCLIENT = "CMDCLIENT";

        /// <summary>
        /// Ligne DATE
        /// </summary>
        public const string REF_EMETTEUR = "REF_EMETTEUR";

        /// <summary>
        /// Ligne FPERSO
        /// </summary>
        public const string COMP_REF_EMETTEUR = "COMP_REF_EMETTEUR";

        /// <summary>
        /// Ligne NUMSERIE
        /// </summary>
        public const string NUM_SERIE_EMETTEUR = "NUM_SERIE_EMETTEUR";


        public const string DOSSIER_EMETTEUR = "DOSSIER_EMETTEUR";
        /// <summary>
        /// Ligne FREQ
        /// </summary>
        public const string CODEIDENT_EMETTEUR = "CODEIDENT_EMETTEUR";

        /// <summary>
        /// Ligne TENSION
        /// </summary>
        public const string CODEIDENT_EMETTEUR_2 = "CODEIDENT_EMETTEUR_2";

        /// <summary>
        /// Ligne CODEIDENT
        /// </summary>
        public const string OPTION_TEXTE = "OPTION_TEXTE";

        /// <summary>
        /// Ligne FIRMWAREMO
        /// </summary>
        public const string NUM_CLE = "NUM_CLE";

        /// <summary>
        /// Ligne FIRMWAREMT
        /// </summary>
        public const string REF_CLE = "REF_CLE";

        /// <summary>
        /// Ligne CODEPIN0
        /// </summary>
        public const string HOMME_MORT = "HOMME_MORT";

        /// <summary>
        /// Ligne CODEPIN1
        /// </summary>
        public const string FICHE_PERSO = "FICHE_PERSO";

        /// <summary>
        /// Ligne CODEPIN2
        /// </summary>
        public const string AR_OF = "AR_OF";
        /// <summary>
        /// Ligne CODEPIN3
        /// </summary>
        public const string PRINTER = "PRINTER";

        /// <summary>
        /// Ligne PRINTER
        /// </summary>
        public const string QUANTITY = "QUANTITY";

        /// <summary>
        /// Initializes a new instance of the <see cref="StructLabel"/> class.
        /// </summary>
        public StructLabel()
        {

        }


        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string CMDSYNCH = "CMD_SYNCHRONISEE";
        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string OPTIONS_LOGICIELLES = "OPTIONS_LOGICIELLES";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string OPTIONS_MATERIEL_MO = "OPTIONS_MATERIEL_MO";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string OPTIONS_MATERIEL_MT = "OPTIONS_MATERIEL_MT";

        /// <summary>
        /// Les options logicielles
        /// </summary>
        public const string COMMANDE_SYNCHRO = "COMMANDE_SYNCHRO";


        /// <summary>
        /// Ligne REFCOMMO
        /// </summary>
        public const string REFCOMMO = "REFCOMMO";

        /// <summary>
        /// Ligne REFCOMMT
        /// </summary>
        public const string REFCOMMT = "REFCOMMT";

        /// <summary>
        /// Ligne REFCOMMO
        /// </summary>
        public const string REFINDUSMO = "REFINDUSMO";

        /// <summary>
        /// Ligne REFCOMMT
        /// </summary>
        public const string REFINDUSMT = "REFINDUSMT";

        /// <summary>
        /// Ligne REFCOMSIM
        /// </summary>
        public const string REFCOMSIM = "REFCOMSIM";

        /// <summary>
        /// Ligne REFCOMPACK
        /// </summary>
        public const string REFCOMPACK = "REFCOMPACK";

        /// <summary>
        /// Ligne NUMMO
        /// </summary>
        public const string NUMMO = "NUMMO";

        /// <summary>
        /// Ligne NUMMT
        /// </summary>
        public const string NUMMT = "NUMMT";

        /// <summary>
        /// Ligne NUMSIM
        /// </summary>
        public const string NUMSIM = "NUMSIM";
        /// <summary>
        /// Ligne FPERSO
        /// </summary>
        public const string FPERSO = "FPERSO";

        /// <summary>
        /// Ligne NUMSERIE
        /// </summary>
        public const string NUMSERIE = "NUMSERIE";

        public const string NUMSERIEOF = "NUMSERIEOF";
        /// <summary>
        /// Ligne FREQ
        /// </summary>
        public const string FREQ = "FREQ";

        /// <summary>
        /// Ligne TENSION
        /// </summary>
        public const string TENSION = "TENSION";

        /// <summary>
        /// Ligne CODEIDENT
        /// </summary>
        public const string CODEIDENT = "CODEIDENT";

        /// <summary>
        /// Ligne FIRMWAREMO
        /// </summary>
        public const string FIRMWAREMO = "FIRMWAREMO";

        /// <summary>
        /// Ligne FIRMWAREMT
        /// </summary>
        public const string FIRMWAREMT = "FIRMWAREMT";

        /// <summary>
        /// Ligne CODEPIN0
        /// </summary>
        public const string CODEPIN0 = "CODEPIN0";

        /// <summary>
        /// Ligne CODEPIN1
        /// </summary>
        public const string CODEPIN1 = "CODEPIN1";

        /// <summary>
        /// Ligne CODEPIN2
        /// </summary>
        public const string CODEPIN2 = "CODEPIN2";
        /// <summary>
        /// Ligne CODEPIN3
        /// </summary>
        public const string CODEPIN3 = "CODEPIN3";

       

        /// <summary>
        /// Ligne LABELQUANTITY
        /// </summary>
        public const string LABELQUANTITY = "LABELQUANTITY";
    }

}