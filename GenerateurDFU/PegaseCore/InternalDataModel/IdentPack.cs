using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    public class IdentPack
    {
        // Variables
        #region Variables
     
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le commentaire lié au pack
        /// </summary>
        public String Commentaire
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/CommentaireFiche", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/CommentaireFiche", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: Commentaire

        /// <summary>
        /// Le numéro de série du pack
        /// </summary>
        public String PackSerialNumber
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/NumSeriePack", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/NumSeriePack", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: PackSerialNumber

        /// <summary>
        /// La date de génération d'un pack
        /// </summary>
        public String PackDateGen
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/DateGen", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/DateGen", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: PackDateGen

        /// <summary>
        /// La date de programmation du pack
        /// </summary>
        public String PackDateProg
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/DateProg", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/DateProg", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: PackDateProg

        /// <summary>
        /// La version du Firmwar pour le MT
        /// </summary>
        public String FirmwarMT
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/FirmwMT", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/FirmwMT", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: FirmwarMT

        /// <summary>
        /// La version du Firmwar pour le MO
        /// </summary>
        public String FirmwarMO
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/IdentPack/FirmwMO", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/IdentPack/FirmwMO", "", "", XML_ATTRIBUTE.VALUE, "");
            }
        } // endProperty: FirmwarMO

        /// <summary>
        /// Numéro de fiche
        /// </summary>
        public String NumFiche
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentAffaire/NumFichePerso", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
	            {
                    Result = "";
	            }
                return Result;
            }
            private set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlIdentification/Ident/IdentAffaire/NumFichePerso", "", "", XML_ATTRIBUTE.VALUE, "");
            }
        } // endProperty: NumFiche

        #endregion

        // Constructeur
        #region Constructeur

        public IdentPack( )
        {
            
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: IdentPack
}
