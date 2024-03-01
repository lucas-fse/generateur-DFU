using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Libellé d'un sélecteur
    /// </summary>
    public class InformationLabel : LabelBase
    {
        // Variables
        #region Variables

        private XElement _element;
        private XMLCore.XMLProcessing _xmlProcessing;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'identifiant du sélecteur
        /// </summary>
        public Int32 IdSelecteur
        {
            get
            {
                return base.IdLabel;
            }
            private set
            {
                base.IdLabel = value;
                RaisePropertyChanged("IdSelecteur");
            }
        } // endProperty: IdSelecteur

        /// <summary>
        /// L'identifiant du libellé du sélecteur
        /// </summary>
        public String IdentLibelInformation
        {
            get
            {
                return base.IdentLabel;
            }
            set
            {
                base.IdentLabel = value;
                RaisePropertyChanged("IdentLibelInformation");
            }
        } // endProperty: IdentLibelSelecteur

        /// <summary>
        /// Numéro du libellé du sélecteur
        /// </summary>
        public Int32 NumLibelInformation
        {
            get
            {
                return base.NumLabel;
            }
            set
            {
                base.NumLabel = value;
                RaisePropertyChanged("NumLibelInformation");
            }
        } // endProperty: NumLibelSelecteur

        /// <summary>
        /// Libellé des sélecteurs
        /// </summary>
        public String LibelInformation
        {
            get
            {
                return base.Label;
            }
            set
            {
                base.Label = value;
                if (base.Label != Constantes.DIRECT_TO_BMP)
                {
                    this.NomFichierBitmapInformation = "";
                }
                RaisePropertyChanged("LibelInformation");
            }
        } // endProperty: LibelSelecteur

        /// <summary>
        /// true si la police est en gras, false sinon
        /// </summary>
        public Boolean PoliceGrasInformation
        {
            get
            {
                return base.PoliceGras;
            }
            set
            {
                base.PoliceGras = value;
                RaisePropertyChanged("PoliceGrasInformation");
            }
        } // endProperty: PoliceGrasSelecteur

        /// <summary>
        /// Le nom du fichier transitoire pour la création de la bitmap
        /// </summary>
        public String NomFichierBitmapInformation
        {
            get
            {
                return base.NomFichierBitmap;
            }
            set
            {
                base.NomFichierBitmap = value;
                RaisePropertyChanged("NomFichierBitmapInformation");
            }
        } // endProperty: NomFichierBitmapSelecteur

        #endregion

        // Constructeur
        #region Constructeur

        public InformationLabel(XElement Element)
        {
            this._element = Element;
            this._xmlProcessing = new XMLCore.XMLProcessing();
            this._xmlProcessing.OpenXML(Element);
            this.InitFromXML();
        }

        public InformationLabel(String IDLabel)
        {
            this.IdentLibelInformation = IDLabel;
            this.LibelInformation = "----";
            this.PoliceGrasInformation = false;
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser depuis le XML
        /// </summary>
        private void InitFromXML ( )
        {
            // L'ID du libellé d'information
            String SV;

            if (this._element.Attribute(XMLCore.XML_ATTRIBUTE.NUM) != null)
            {
                SV = this._element.Attribute(XMLCore.XML_ATTRIBUTE.NUM).Value;
            }
            else
            {
                SV = "0";
            }

            this.IdSelecteur = Convert.ToInt32(SV);

            // L'IdentLibelInformation
            this.IdentLibelInformation = this._xmlProcessing.GetNodesByCode("IdentLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // NumLibelInformation
            SV = this._xmlProcessing.GetNodesByCode("NumLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            try
            {
                this.NumLibelInformation = Convert.ToInt32(SV);
            }
            catch
            {
                SV = "0";
            }

            // LibelInformation
            this.LibelInformation = this._xmlProcessing.GetNodesByCode("LibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // PoliceGrasInformation
            Boolean b;

            SV = this._xmlProcessing.GetNodesByCode("PoliceGrasInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            if (Convert.ToInt32(SV) == 1)
            {
                b = true;
            }
            else
            {
                b = false;
            }

            this.PoliceGras = b;

            // Nom du fichier
            this.NomFichierBitmapInformation = this._xmlProcessing.GetNodesByCode("NomFichierBitmapInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
        } // endMethod: InitFromXML

        
        /// <summary>
        /// Sérialise les données du libellé dans le XML
        /// </summary>
        public void SerialiseXML ( XElement node )
        {
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(node);

            // IdentLibelInformation
            XProcess.GetNodesByCode("IdentLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.IdentLibelInformation;

            // Num
            XProcess.GetNodesByCode("NumLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NumLibelInformation.ToString();

            // LibelInformation
            XProcess.GetNodesByCode("LibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.LibelInformation;

            // Envoyer un message spécifiant que le libellé change
            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_VALUE_CHANGED));

            // PoliceGrasInformation
            String Value;

            if (this.PoliceGrasInformation)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            XProcess.GetNodesByCode("PoliceGrasInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = Value;
            // Nom fichier bitmap
            if (this.NomFichierBitmapInformation == null)
            {
                this.NomFichierBitmapInformation = ""; 
            }
            XProcess.GetNodesByCode("NomFichierBitmapInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NomFichierBitmapInformation;
        } // endMethod: SerialiseXML

        /// <summary>
        /// Supprimer le libellé
        /// </summary>
        public void Remove ( )
        {
            this._xmlProcessing.RootNode.Remove();
        } // endMethod: Remove

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: LibelSelecteur
}
