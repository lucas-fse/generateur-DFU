using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.DAL.BDDLocal
{
    /// <summary>
    /// Le viewModel de la fenetre Importe Window
    /// </summary>
    public class ViewModelImportWindow : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private String _title;
        private Int32 _progress;
        private Int32 _minProgressValue;
        private Int32 _maxProgressValue;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La valeur maximum pour la barre de progrès
        /// </summary>
        public Int32 MaxProgressValue
        {
            get
            {
                return this._maxProgressValue;
            }
            set
            {
                this._maxProgressValue = value;
                RaisePropertyChanged("MaxProgressValue");
            }
        } // endProperty: MaxProgressValue

        /// <summary>
        /// La valeur minimum pour la barre de progrès
        /// </summary>
        public Int32 MinProgressValue
        {
            get
            {
                return this._minProgressValue;
            }
            set
            {
                this._minProgressValue = value;
                RaisePropertyChanged("MinProgressValue");
            }
        } // endProperty: MinProgressValue

        /// <summary>
        /// La progression de l'action en cours
        /// </summary>
        public Int32 Progress
        {
            get
            {
                return this._progress;
            }
            set
            {
                this._progress = value;
                RaisePropertyChanged("Progress");
            }
        } // endProperty: Progress

        /// <summary>
        /// Le titre de la fenêtre
        /// </summary>
        public String Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        } // endProperty: Title

        #endregion

        // Constructeur
        #region Constructeur

        public ViewModelImportWindow()
        {
            this.Title = "iDialog file importer 1.0";
            this.MaxProgressValue = 100;
            this.MinProgressValue = 0;
            this.Progress = 0;
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ViewModelImportWindow
}
