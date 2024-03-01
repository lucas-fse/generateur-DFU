using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// L' information de base Gain/Offset issus des options logiciels
    /// </summary>
    public class ConfigGainOffsetESAna
    {
        // Variables
        #region Variables

        private XElement _gain;
        private XElement _offest;
        private XElement _carte;
        private XElement _voie;
        private XElement _typeUI;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La configuration Gain / Offset est-elle sur une entrée ?
        /// </summary>
        public Boolean IsEntree
        {
            get;
            set;
        } // endProperty: IsEntree

        /// <summary>
        /// Le gain
        /// </summary>
        public float Gain
        {
            get
            {
                String SV = this._gain.Attribute(XML_ATTRIBUTE.VALUE).Value;
                float Result = Tools.ConvertFromStringIEEE_2Float(SV);

                return Result;
            }
            set
            {
                String SV;
                SV = Tools.ConvertFromfloat_2StringIEEE(value);
                this._gain.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Gain

        /// <summary>
        /// L'Offset
        /// </summary>
        public float Offset
        {
            get
            {
                String SV = this._offest.Attribute(XML_ATTRIBUTE.VALUE).Value;
                float Result = Tools.ConvertFromStringIEEE_2Float(SV);

                return Result;
            }
            set
            {
                String SV;
                SV = Tools.ConvertFromfloat_2StringIEEE(value);
                this._offest.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Offset

        /// <summary>
        /// Le numéro de la carte
        /// </summary>
        public Byte NumCarte
        {
            get
            {
                String SV;
                Byte Result;

                SV = this._carte.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Result = Tools.ConvertASCCI2Byte(SV);

                return Result;
            }
            set
            {
                String SV = Tools.ConvertByteASCIIByte(value);
                this._carte.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: NumCarte

        /// <summary>
        /// Le numéro de la voie
        /// </summary>
        public Byte NumVoie
        {
            get
            {
                String SV;
                Byte Result;

                SV = this._voie.Attribute(XML_ATTRIBUTE.VALUE).Value;
                Result = Tools.ConvertASCCI2Byte(SV);

                return Result;
            }
            set
            {
                String SV;
                SV = Tools.ConvertByteASCIIByte(value);
                this._voie.Attribute(XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: NumVoie

        #endregion

        // Constructeur
        #region Constructeur

        public ConfigGainOffsetESAna(XElement gain, XElement offset, XElement carte, XElement voie, XElement typeUI)
        {
            this._gain = gain;
            this._offest = offset;
            this._carte = carte;
            this._voie = voie;
            this._typeUI = typeUI;
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ConfigGainOffsetESAna
    
	
}
