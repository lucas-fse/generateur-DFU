using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace JAY.PegaseCore
{
    public enum TypePin
    {
        Admin = 0,
        Apprentissage = 1,
        RunPP = 2,
        Run = 3
    }

    /// <summary>
    /// Les données d'un masque des organes pour un utilisateur
    /// </summary>
    public class MaskUserMode
    {
        // Variables
        #region Variables
        
        private UInt16 _mask16;
        private UInt32 _mask32;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le masque 32 bits des organes pour le mode
        /// </summary>
        public UInt32 Mask32
        {
            get
            {
                return this._mask32;
            }
            set
            {
                this._mask32 = value;
            }
        } // endProperty: Mask32

        /// <summary>
        /// Le masque 16 bits des organes pour le mode
        /// </summary>
        public UInt16 Mask16
        {
            get
            {
                return this._mask16;
            }
            set
            {
                this._mask16 = value;
            }
        } // endProperty: Mask16

        #endregion

        // Constructeur
        #region Constructeur

        public MaskUserMode(UInt32 Mask32, UInt16 Mask16)
        {
            this.Mask32 = Mask32;
            this.Mask16 = Mask16;
        }

        public MaskUserMode(XElement Mask32, XElement Mask16)
        {
            // Définir le masque 32 bits
            if (Mask32.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
            {
                String M32 = Mask32.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                try
                {
                    this.Mask32 = Convert.ToUInt32(M32);
                    //this.Mask32 = XMLCore.Tools.Bitfield32ToInt32(M32);
                }
                catch
                {
                    this.Mask32 = (UInt32)0xFFFFFFFF;
                }
            }
            else
            {
                this.Mask32 = (UInt32)0xFFFFFFFF;
            }
            // Définir le masque 16 bits
            if (Mask16.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
            {
                String M16 = Mask16.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                try
                {
                    this.Mask16 = Convert.ToUInt16(M16);
                    //this.Mask16 = XMLCore.Tools.Bitfield16ToInt16(M16);
                }
                catch
                {
                    this.Mask16 = (UInt16)0xFFFF;
                }
            }
            else
            {
                this.Mask16 = (UInt16)0xFFFF;
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: MaskUserMode
}
