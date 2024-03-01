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
    public class SelecteurLabel : LabelBase
    {
        // Variables
        #region Variables

        private XElement _element;
        private XMLCore.XMLProcessing _xmlProcessing;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le texte est-il éditable ?
        /// </summary>
        public Boolean IsEditableText
        {
            get
            {
                Boolean Result;

                if (this.LibelSelecteur == Constantes.DIRECT_TO_BMP)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                }
                return Result;
            }
        } // endProperty: IsDirectToBMP

        /// <summary>
        /// L'identifiant du sélecteur
        /// </summary>
        public Int32 IdSelecteur
        {
            get
            {
                return base.IdLabel;
            }
            set
            {
                base.IdLabel = value;
                RaisePropertyChanged("IdSelecteur");
            }
        } // endProperty: IdSelecteur

        /// <summary>
        /// L'identifiant du libellé du sélecteur
        /// </summary>
        public String IdentLibelSelecteur
        {
            get
            {
                return base.IdentLabel;
            }
            set
            {
                base.IdentLabel = value;
                RaisePropertyChanged("IdentLibelSelecteur");
            }
        } // endProperty: IdentLibelSelecteur

        /// <summary>
        /// Numéro du libellé du sélecteur
        /// </summary>
        public Int32 NumLibelSelecteur
        {
            get
            {
                return base.NumLabel;
            }
            set
            {
                base.NumLabel = value;
                RaisePropertyChanged("NumLibelSelecteur");
            }
        } // endProperty: NumLibelSelecteur

        /// <summary>
        /// Libellé des sélecteurs
        /// </summary>
        public String LibelSelecteur
        {
            get
            {
                return base.Label;
            }
            set
            {
                base.Label = value;
                RaisePropertyChanged("LibelSelecteur");
            }
        } // endProperty: LibelSelecteur

        /// <summary>
        /// true si la police est en gras, false sinon
        /// </summary>
        public Boolean PoliceGrasSelecteur
        {
            get
            {
                return base.PoliceGras;
            }
            set
            {
                base.PoliceGras = value;
                RaisePropertyChanged("PoliceGrasSelecteur");
                RaisePropertyChanged("LibelSelecteur");
            }
        } // endProperty: PoliceGrasSelecteur

        /// <summary>
        /// Le nom du fichier transitoire pour la création de la bitmap
        /// </summary>
        public String NomFichierBitmapSelecteur
        {
            get
            {
                return base.NomFichierBitmap;
            }
            set
            {
                base.NomFichierBitmap = value;
                RaisePropertyChanged("NomFichierBitmapSelecteur");
            }
        } // endProperty: NomFichierBitmapSelecteur

        #endregion

        // Constructeur
        #region Constructeur

        public SelecteurLabel(XElement Element)
        {
            this.InitFromXml(Element);
        }

        // construire un sélecteur par défaut avec un IDSelecteur spécifique
        public SelecteurLabel(String IdentLibelSelecteur)
        {
            this.IdentLibelSelecteur = IdentLibelSelecteur;
            this.LibelSelecteur = "----";
            this.PoliceGrasSelecteur = false;
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser le libellé du sélecteur
        /// </summary>
        public void InitFromXml(XElement Element)
        {
            // 0 - Initialisation
            this._element = Element;
            this._xmlProcessing = new XMLCore.XMLProcessing();
            this._xmlProcessing.OpenXML(Element);

            // 1 - IdSelecteur
            String SV;

            //if (this._element.Attribute(XMLCore.XML_ATTRIBUTE.NUM) != null)
            //{
            //    SV = this._element.Attribute(XMLCore.XML_ATTRIBUTE.NUM).Value;
            //}
            //else
            //{
            //    SV = "";
            //}

            //Int32 Id;

            //if (SV.Count() > 0)
            //{
            //    Id = Convert.ToInt32(SV);
            //}
            //else
            //{
            //    Id = 0;
            //}
            //this.IdSelecteur = Id;

            // 2 - IdentSelecteur
            this.IdentLibelSelecteur = this._xmlProcessing.GetNodesByCode("IdentLibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 3 - NumLibelSelecteur
            SV = this._xmlProcessing.GetNodesByCode("NumLibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.NumLibelSelecteur = Convert.ToInt32(SV);
            
            // 4 - LibelSelecteur
            this.LibelSelecteur = this._xmlProcessing.GetNodesByCode("LibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 5 - PoliceGrasSelecteur
            Boolean b;

            SV = this._xmlProcessing.GetNodesByCode("PoliceGrasSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            if (Convert.ToInt32(SV) == 1)
            {
                b = true;
            }
            else
            {
                b = false;
            }
            this.PoliceGrasSelecteur = b;

            // 6 - NomFichierBitmapSelecteur
            this.NomFichierBitmapSelecteur = this._xmlProcessing.GetNodesByCode("NomFichierBitmapSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

        } // endMethod: InitFromXml
        
        /// <summary>
        /// Remplir l'élément xml transmis en vue de la sérialisation
        /// </summary>
        public void SerialiseLibelSelecteur ( XElement libelselecteur )
        {
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(libelselecteur);
            String SV;

            // 1 - IdSelecteur
            //this._element.Attribute(XMLCore.XML_ATTRIBUTE.NUM).Value = this.IdSelecteur.ToString();

            // 2 - IdentSelecteur
            XProcess.GetNodesByCode("IdentLibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.IdentLibelSelecteur;

            // 3 - NumLibelSelecteur
            XProcess.GetNodesByCode("NumLibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NumLibelSelecteur.ToString();

            // 4 - LibelSelecteur
            XProcess.GetNodesByCode("LibelSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.LibelSelecteur;

            // 5 - PoliceGrasSelecteur
            if (this.PoliceGrasSelecteur)
            {
                SV = "1";
            }
            else
            {
                SV = "0";
            }

            XProcess.GetNodesByCode("PoliceGrasSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;

            // 6 - NomFichierBitmapSelecteur
            if (this.NomFichierBitmapSelecteur != null)
            {
                XProcess.GetNodesByCode("NomFichierBitmapSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NomFichierBitmapSelecteur; 
            }
            else
            {
                XProcess.GetNodesByCode("NomFichierBitmapSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "";
            }

        } // endMethod: SerialiseLibelSelecteur

        /// <summary>
        /// Supprimer le libellé sélecteur
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