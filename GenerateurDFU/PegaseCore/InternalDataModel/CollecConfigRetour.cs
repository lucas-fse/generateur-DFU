using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe contenant une ligne de données de la collection CollecConfigRetour
    /// </summary>
    public class CollecConfigRetour
    {
        // Variables
        #region Variables

        private XMLProcessing _xmlProcessing;

        private XElement _familleES;
        private XElement _hauteurEnLigne;
        private XElement _IDLibelDesignation;
        private XElement _IDLibelMax;
        private XElement _IDLibelMin;
        private XElement _IDLibelUnit;
        private XElement _IndiceFamilleES;
        private XElement _NbDecimal;
        private XElement _Scaling_A;
        private XElement _Scaling_B;
        private XElement _TypeVariable;
        private XElement _ValMax;
        private XElement _ValMin;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La famille d'entrées / sorties à laquelle est liée le retour d'information
        /// </summary>
        public FamilleDeDonnees_e FamilleES
        {
            get
            {
                String SV = this._familleES.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                FamilleDeDonnees_e Result = (FamilleDeDonnees_e)Val;

                return Result;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte((Byte)value);
                this._familleES.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: FamilleES

        /// <summary>
        /// Indice de la famille d'entrées / sorties
        /// </summary>
        public Byte IndiceFamilleES
        {
            get
            {
                String SV = this._IndiceFamilleES.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._IndiceFamilleES.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: IndiceFamilleES

        /// <summary>
        /// Indice Libellé (dans la table d'offsets du Dico Infos)
        /// </summary>
        public Byte IDLibelDesignation
        {
            get
            {
                String SV = this._IDLibelDesignation.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._IDLibelDesignation.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: IDLibelDesignation

        /// <summary>
        /// L'ID du libellé des unités (dans la table d'offsets du Dico Infos)
        /// </summary>
        public Byte IDLibelUnite
        {
            get
            {
                String SV = this._IDLibelUnit.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._IDLibelUnit.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: IDLibelUnite

        /// <summary>
        /// Le facteur de mise à l'échelle A dans la formule Y = Ax + B
        /// </summary>
        public float Scaling_A
        {
            get
            {
                String SV = this._Scaling_A.Attribute(XML_ATTRIBUTE.VALUE).Value;
                float Val = Tools.ConvertFromStringIEEE_2Float(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertFromfloat_2StringIEEE(value);
                this._Scaling_A.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Scaling_A

        /// <summary>
        /// Le facteur de mise à l'échelle B dans la formule Y = Ax + B
        /// </summary>
        public float Scaling_B
        {
            get
            {
                String SV = this._Scaling_B.Attribute(XML_ATTRIBUTE.VALUE).Value;
                float Val = Tools.ConvertFromStringIEEE_2Float(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertFromfloat_2StringIEEE(value);
                this._Scaling_B.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Scaling_B

        /// <summary>
        /// Val Min avant mise à l'echelle (=0 pour TOR)
        /// </summary>
        public Int16 ValMin
        {
            get
            {
                String SV = this._ValMin.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Int16 Val = Tools.ConvertASCIIToInt16(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertInt16ToString(value);
                this._ValMin.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: ValMin

        /// <summary>
        /// Val Max avant mise à l'echelle (=1 pour TOR)
        /// </summary>
        public Int16 ValMax
        {
            get
            {
                String SV = this._ValMax.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Int16 Val = Tools.ConvertASCIIToInt16(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertInt16ToString(value);
                this._ValMax.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: ValMax

        /// <summary>
        /// Indice Libellé Val Min  (dans la table d'offsets du Dico Infos)
        /// </summary>
        public Byte IdLibelMin
        {
            get
            {
                String SV = this._IDLibelMin.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._IDLibelMin.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: IdLibelMin

        /// <summary>
        /// Indice Libellé Val MAX (dans la table d'offsets du Dico Infos)
        /// </summary>
        public Byte IdLibelMax
        {
            get
            {
                String SV = this._IDLibelMax.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._IDLibelMax.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: IdLibelMax

        /// <summary>
        /// Type Val Retour
        /// </summary>
        public Byte TypeVariable
        {
            get
            {
                String SV = this._TypeVariable.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._TypeVariable.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: TypeVariable

        /// <summary>
        /// Nb de decimales affichées
        /// </summary>
        public Byte NbDecimal
        {
            get
            {
                String SV = this._NbDecimal.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._NbDecimal.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: NbDecimal

        /// <summary>
        /// Hauteur affichage en Nb de ligne
        /// </summary>
        public Byte HauteurEnLigne
        {
            get
            {
                String SV = this._hauteurEnLigne.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Byte Val = Tools.ConvertASCCI2Byte(SV);
                return Val;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._hauteurEnLigne.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: HauteurEnLigne

        #endregion

        // Constructeur
        #region Constructeur

        public CollecConfigRetour(XElement config)
        {
            this._xmlProcessing = new XMLProcessing();
            this._xmlProcessing.OpenXML(config);

            this.InitCollecConfigRetour();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initaliser la configuration de retour
        /// </summary>
        private Int32 InitCollecConfigRetour ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            // Famille ES
            try
            {
                this._familleES = this._xmlProcessing.GetNodesByCode("FamilleES")[0];
                this._hauteurEnLigne = this._xmlProcessing.GetNodesByCode("HauteurEnLignes")[0];
                this._IDLibelDesignation = this._xmlProcessing.GetNodesByCode("LibelleDesignation")[0];
                this._IDLibelMax = this._xmlProcessing.GetNodesByCode("LibelleMax")[0];
                this._IDLibelMin = this._xmlProcessing.GetNodesByCode("LibelleMin")[0];
                this._IDLibelUnit = this._xmlProcessing.GetNodesByCode("LibelleUnite")[0];
                this._IndiceFamilleES = this._xmlProcessing.GetNodesByCode("IndiceFamilleES")[0];
                this._NbDecimal = this._xmlProcessing.GetNodesByCode("NbDecimales")[0];
                this._Scaling_A = this._xmlProcessing.GetNodesByCode("Scaling_a")[0];
                this._Scaling_B = this._xmlProcessing.GetNodesByCode("Scaling_b")[0];
                this._TypeVariable = this._xmlProcessing.GetNodesByCode("TypeVariable")[0];
                this._ValMax = this._xmlProcessing.GetNodesByCode("ValMax")[0];
                this._ValMin = this._xmlProcessing.GetNodesByCode("ValMin")[0];
            }
            catch
            {
                Result = XML_ERROR.ERROR_XML_INTEGRITY;
            }

            return Result;
        } // endMethod: InitCollecConfigRetour

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: CollecConfigRetour
}
