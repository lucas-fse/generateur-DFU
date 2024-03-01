using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les fichiers de traçabilité
    /// </summary>
    public class TraceRefFile
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom court du fichier d'origine qui a permit 
        /// </summary>
        public String ShortFileName
        {
            get;
            private set;
        } // endProperty: ShortFileName

        /// <summary>
        /// L'id du fichier de référence
        /// </summary>
        public String IDRefFile
        {
            get;
            private set;
        } // endProperty: IDPArt

        #endregion

        // Constructeur
        #region Constructeur

        public TraceRefFile(XElement component)
        {
            if (component.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
            {
                String Value = component.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                Int32 Pos = Value.IndexOf('_');
                this.ShortFileName = Value.Substring(Pos+1);
            }

            if (component.Attribute(XMLCore.XML_ATTRIBUTE.NUM) != null)
            {
                this.IDRefFile = component.Attribute(XMLCore.XML_ATTRIBUTE.NUM).Value;
            }
            else if (component.Attribute("Num") != null)
            {
                this.IDRefFile = component.Attribute("Num").Value;
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: TraceRefFile
}