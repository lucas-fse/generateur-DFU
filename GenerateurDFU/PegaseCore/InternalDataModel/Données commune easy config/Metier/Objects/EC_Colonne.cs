using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe contenant les données d'entête de colonne
    /// </summary>
    public class EC_Colonne : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables
        private String _name;
        private String _mmnemologique;
        private Boolean _isEnable;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La colonne est-elle disponible
        /// </summary>
        public Boolean IsEnable
        {
            get
            {
                return this._isEnable;
            }
            set
            {
                this._isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        } // endProperty: IsEnable

        /// <summary>
        /// Le nom de la colonne
        /// </summary>
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        } // endProperty: Name
        /// <summary>
        /// Le nom de la mnemologique
        /// </summary>
        public String Mmnemologique
        {
            get
            {
                return this._mmnemologique;
            }
            set
            {
                this._mmnemologique = value;
                
            }
        } // endProperty: Name
        #endregion

        // Constructeur
        #region Constructeur

        public EC_Colonne(String name)
        {
            this.Name = name;
        }
        public EC_Colonne(String name, string mmnemologique)
        {
            this.Name = name;
            this.Mmnemologique = mmnemologique;
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_Colonne
}
