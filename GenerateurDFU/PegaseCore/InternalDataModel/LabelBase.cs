using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Form = System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mvvm = GalaSoft.MvvmLight;
using System.IO;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Class de base permettant de gérer des labels RI ou sélecteur
    /// </summary>
    public class LabelBase : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private Boolean _policeGras;
        private String _identLabel;
        private Int32 _idLabel;
        private String _label;
        private String _nomFichierBitmap;
        private Int32 _numLabel;
        private Boolean _isBitmap;
        private Boolean _isEditable;
        private Visibility _labelIsVisible;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le label est-il visible ?
        /// </summary>
        public Visibility LabelIsVisible
        {
            get
            {
                return this._labelIsVisible;
            }
            set
            {
                this._labelIsVisible = value;
                RaisePropertyChanged("LabelIsVisible");
            }
        } // endProperty: LabelIsVisible

        /// <summary>
        /// La visibilité de l'édition bitmap
        /// </summary>
        public Visibility BitmapVisibility
        {
            get
            {
                Visibility Result = Visibility.Visible;

                if (!this.IsBitmap)
                {
                    Result = Visibility.Collapsed;
                }

                return Result;
            }
        } // endProperty: BitmapVisibility

        /// <summary>
        /// La visibilité de l'édition de texte
        /// </summary>
        public Visibility TextVisibility
        {
            get
            {
                Visibility Result = Visibility.Visible;
                if (this.IsBitmap)
                {
                    Result = Visibility.Collapsed;
                }

                return Result;
            }
        } // endProperty: TextVisibility

        /// <summary>
        /// Le libellé est-il à éditer sous forme de bitmap
        /// </summary>
        public Boolean IsBitmap
        {
            get
            {
                return this._isBitmap;
            }
            set
            {
                this._isBitmap = value;
                RaisePropertyChanged("IsBitmap");
                RaisePropertyChanged("TextVisibility");
                RaisePropertyChanged("BitmapVisibility");
            }
        } // endProperty: IsBitmap

        /// <summary>
        /// Le libellé est-il éditable?
        /// </summary>
        public Boolean IsEditable
        {
            get
            {
                return this._isEditable;
            }
            set
            {
                this._isEditable = value;
                RaisePropertyChanged("IsEditable");
            }
        } // endProperty: IsEditable

        /// <summary>
        /// L'identifiant du libellé
        /// </summary>
        public Int32 IdLabel
        {
            get
            {
                return this._idLabel;
            }
            set
            {
                this._idLabel = value;
                RaisePropertyChanged("IdLabel");
            }
        } // endProperty: IdLabel

        /// <summary>
        /// L'identifiant du libellé du sélecteur
        /// </summary>
        public String IdentLabel
        {
            get
            {
                return this._identLabel;
            }
            set
            {
                this._identLabel = value;
                RaisePropertyChanged("IdentLabel");
            }
        } // endProperty: IdentLabel

        /// <summary>
        /// Numéro du libellé du sélecteur
        /// </summary>
        public Int32 NumLabel
        {
            get
            {
                return this._numLabel;
            }
            set
            {
                this._numLabel = value;
                RaisePropertyChanged("NumLabel");
            }
        } // endProperty: NumLabel

        /// <summary>
        /// Libellé des sélecteurs
        /// </summary>
        public String Label
        {
            get
            {
                return this._label;
            }
            set
            {
                this._label = value;
                if (this._label == Constantes.DIRECT_TO_BMP)
                {
                    this.IsBitmap = true;
                }
                else
                {
                    this.NomFichierBitmap = "";
                    this.IsBitmap = false;
                }
                RaisePropertyChanged("Label");
            }
        } // endProperty: Label

        /// <summary>
        /// true si la police est en gras, false sinon
        /// </summary>
        public Boolean PoliceGras
        {
            get
            {
                return this._policeGras;
            }
            set
            {
                this._policeGras = value;
                RaisePropertyChanged("PoliceGras");
                RaisePropertyChanged("Label");
            }
        } // endProperty: PoliceGras

        /// <summary>
        /// Le nom du fichier transitoire pour la création de la bitmap
        /// </summary>
        public String NomFichierBitmap
        {
            get
            {
                return this._nomFichierBitmap;
            }
            set
            {
                this._nomFichierBitmap = value;
                RaisePropertyChanged("NomFichierBitmap");
            }
        } // endProperty: NomFichierBitmap

        #endregion

        // Constructeur
        #region Constructeur

        public LabelBase()
        {
            this.CreateCommandBrowse();
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandBrowse
        /// <summary>
        /// La commande Browse
        /// </summary>
        public ICommand CommandBrowse
        {
            get;
            internal set;
        } // endProperty: CommandBrowse

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandBrowse()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandBrowse = new Mvvm.Command.RelayCommand(ExecuteCommandBrowse, CanExecuteCommandBrowse);
        } // endMethod: CreateCommandBrowse

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandBrowse()
        {
            // Aller charger une image
            Form.OpenFileDialog OFD = new Form.OpenFileDialog();
            OFD.Filter = LanguageSupport.Get().GetText("FILTER/BITMAP_FILTER");

            if (OFD.ShowDialog() == Form.DialogResult.OK)
            {
                System.Drawing.Bitmap Result = null;
                try
                {
                    Uri ImageUri = new Uri(OFD.FileName, UriKind.Relative);
                   
                    System.IO.Stream BitmapStream = PegaseData.Instance.CurrentPackage.GetPartStream(ImageUri);
                    if (BitmapStream == null && File.Exists(OFD.FileName))
                    {
                        using (BitmapStream = File.OpenRead(OFD.FileName))
                        {
                            Result = new System.Drawing.Bitmap(BitmapStream);
                        }
                    }
                }
                catch
                {
                    // image invalide
                    Result = null;
                    MessageBox.Show(LanguageSupport.Get().GetText("FILTER/INVALID_FORMAT"));
                }
                if (Result != null)
                {
                    this.NomFichierBitmap = OFD.FileName;
                    this.Label = Constantes.DIRECT_TO_BMP;
                }
            }
        } // endMethod: ExecuteCommandBrowse

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandBrowse()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandBrowse

        #endregion

        #endregion
    } // endClass: Label
}
