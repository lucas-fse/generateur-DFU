using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    public class PluginsMenu : Mvvm.ViewModelBase
    {
        #region Variables

        private Boolean _isActivated;
        private FontWeight _fontWeight;
        private Boolean _isSelected;
        private Brush _textColor;
        private Brush _background;
        protected String _fieldMenu;

        #endregion

        #region Propriétés

        /// <summary>
        /// Le champ menu
        /// </summary>
        public String FieldMenu
        {
            get
            {
                return this._fieldMenu;
            }
            set
            {
                this._fieldMenu = value;
                RaisePropertyChanged("FieldMenu");
            }
        } // endProperty: FieldMenu

        /// <summary>
        /// L'élément de menu est en cours de sélection
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                if (this._isSelected)
                {
                    this.fontWeight = FontWeights.Bold;
                    this.TextColor = new SolidColorBrush(Color.FromArgb(255, 00, 00, 0)) ;// Brushes.Black;
                    this.Background = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150)); 
                }
                else
                {
                    this.fontWeight = FontWeights.Normal;
                    this.TextColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    this.Background = new SolidColorBrush(Color.FromArgb(00, 00, 00, 0));
                }
                RaisePropertyChanged("IsSelected");
            }
        } // endProperty: IsSelected


        /// <summary>
        /// Le menu est-il disponible?
        /// </summary>
        public Boolean IsActivated
        {
            get
            {
                return this._isActivated;
            }
            set
            {
                this._isActivated = value;
                RaisePropertyChanged("IsActivated");
            }
        } // endProperty: IsActivated

        /// <summary>
        /// Le graissage des charactères
        /// </summary>
        public FontWeight fontWeight
        {
            get
            {
                return this._fontWeight;
            }
            set
            {
                this._fontWeight = value;
                RaisePropertyChanged("fontWeight");
            }
        } // endProperty: fontWeight

        /// <summary>
        /// La couleur du texte de l'élément de menu
        /// </summary>
        public Brush TextColor
        {
            get
            {
                return this._textColor;
            }
            set
            {
                this._textColor = value;
                RaisePropertyChanged("TextColor");
            }
        } // endProperty: TextColor

        public Brush Background
        {
            get
            {
                return this._background;
            }
            set
            {
                this._background = value;
                RaisePropertyChanged("Background");
            }
        }
        #endregion
    }
}
