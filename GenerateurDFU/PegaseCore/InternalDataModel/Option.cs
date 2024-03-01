using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Description d'une option (IR / Auxiliaire)
    /// </summary>
    public class Option
    {
        // Variables
        #region Variables

        private Int32 _id;
        private String _nom;
        private String _description;
        private Boolean _presence;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Id de l'option
        /// </summary>
        public Int32 Id
        {
            get
            {
                return this._id;
            }
            private set
            {
                this._id = value;
            }
        } // endProperty: Id

        /// <summary>
        /// Le nom de l'option
        /// </summary>
        public String Nom
        {
            get
            {
                return this._nom;
            }
            private set
            {
                this._nom = value;
            }
        } // endProperty: Nom

        /// <summary>
        /// La description de l'option
        /// </summary>
        public String Description
        {
            get
            {
                return this._description;
            }
            private set
            {
                this._description = value;
            }
        } // endProperty: Description

        /// <summary>
        /// La présence de l'option
        /// </summary>
        public Boolean Presence
        {
            get
            {
                return this._presence;
            }
            private set
            {
                this._presence = value;
            }
        } // endProperty: Presence

        #endregion

        // Constructeur
        #region Constructeur

        public Option(XElement xOption)
        {
            XMLProcessing XProcess = new XMLProcessing();
            XProcess.OpenXML(xOption);
            String Value;

            // Id
            Value = XProcess.GetValue("IdOption", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != "")
            {
                this.Id = Convert.ToInt32(Value);
            }

            // Nom
            this.Nom = XProcess.GetValue("NomOption", "", "", XML_ATTRIBUTE.VALUE);

            // Présence
            Value = XProcess.GetValue("PresenceOption", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != "1")
            {
                this.Presence = false;
            }
            else
            {
                this.Presence = true;
            }

            // Description
            this.Description = XProcess.GetValue("DescriptionOption", "", "", XML_ATTRIBUTE.VALUE);
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Option
}
