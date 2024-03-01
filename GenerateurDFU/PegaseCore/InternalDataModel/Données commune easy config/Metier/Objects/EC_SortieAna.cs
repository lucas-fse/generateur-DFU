using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JAY.PegaseCore;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Class de présentation des sorties analogiques
    /// </summary>
    public class EC_SortieAna : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private ESAna _sana;
        private ESTOR _sPWM;
        private Boolean _isAssociated;
        private EC_OrganAna _linkedOrganAna;
        private Int32 _lineNumber;

        private Double _borneMax;
        private Double _borneMin;
        private Double _valZero;
        private Boolean _courbePositive;
        private String _lastTypeUI;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le dernier type UI de la sortie
        /// </summary>
        public String LastTypeUI
        {
            get
            {
                return this._lastTypeUI;
            }
            set
            {
                this._lastTypeUI = value;
                RaisePropertyChanged("LastTypeUI");
            }
        } // endProperty: LastTypeUI

        /// <summary>
        /// La courbe est-elle positive ?
        /// </summary>
        public Boolean IsCourbePositive
        {
            get
            {
                return this._courbePositive;
            }
            set
            {
                this._courbePositive = value;
                RaisePropertyChanged("IsCourbePositive");
            }
        } // endProperty: IsCourbePositive

        /// <summary>
        /// La borne max (V ou mA) atteinte sur la sortie analogique
        /// </summary>
        public Double BorneMax
        {
            get
            {
                return this._borneMax;
            }
            set
            {
                this._borneMax = value;
                Double vzero = (this._borneMax - this._borneMin) / 2;
                this.ValZero = vzero;
                RaisePropertyChanged("BorneMax");
            }
        } // endProperty: BorneMax

        /// <summary>
        /// La borne min (V ou mA) atteinte sur la sortie analogique
        /// </summary>
        public Double BorneMin
        {
            get
            {
                return this._borneMin;
            }
            set
            {
                this._borneMin = value;

                Double vzero = (this._borneMax - this._borneMin) / 2;
                this.ValZero = vzero;
                RaisePropertyChanged("BorneMin");
            }
        } // endProperty: BorneMin

        /// <summary>
        /// La valeur de sortie lorsque le joystick est au neutre
        /// </summary>
        public Double ValZero
        {
            get
            {
                return this._valZero;
            }
            set
            {
                this._valZero = value;
                RaisePropertyChanged("ValZero");
            }
        } // endProperty: ValZero

        /// <summary>
        /// Le numéro de la ligne dans une collection
        /// </summary>
        public Int32 LineNumber
        {
            get
            {
                return this._lineNumber;
            }
            set
            {
                this._lineNumber = value;
                RaisePropertyChanged("LineNumber");
            }
        } // endProperty: LineNumber

        /// <summary>
        /// L'organe analogique lié à cette sortie analogique
        /// </summary>
        public EC_OrganAna LinkedOrganAna
        {
            get
            {
                return this._linkedOrganAna;
            }
            set
            {
                this._linkedOrganAna = value;
                RaisePropertyChanged("LinkedOrganAna");
            }
        } // endProperty: LinkedOrganAna;

        /// <summary>
        /// La sortie analogique est-elle liée avec un organe ana ?
        /// </summary>
        public Boolean IsAssociated
        {
            get
            {
                return this._isAssociated;
            }
            set
            {
                this._isAssociated = value;
                if (this.LinkedOrganAna != null)
                {
                    this.LinkedOrganAna.IsUsed = value;
                    this.LinkedOrganAna.LinkedOAna = this;
                }
                RaisePropertyChanged("IsAssociated");
            }
        } // endProperty: IsAssociated

        /// <summary>
        /// Le lien avec la sortie analogique / PWM
        /// </summary>
        public ESAna SAna
        {
            get
            {
                return this._sana;
            }
            set
            {
                this._sana = value;
                RaisePropertyChanged("SAna");
            }
        } // endProperty: SAna

        /// <summary>
        /// La sortie PWM
        /// </summary>
        public ESTOR SPWM
        {
            get
            {
                return this._sPWM;
            }
            set
            {
                this._sPWM = value;
                RaisePropertyChanged("SPWM");
            }
        } // endProperty: SPWM

        #endregion

        // Constructeur
        #region Constructeur

        public EC_SortieAna()
        {
            this.IsAssociated = false;
        }

        public EC_SortieAna(ESAna sAna)
        {
            this.IsAssociated = false;
            this.SAna = sAna;

            this.BorneMax = (Int32)this.SAna.ValUIMax;
            this.BorneMin = (Int32)this.SAna.ValUIMin;
            this.ValZero = (this.BorneMax + this.BorneMin) / 2;

            this.IsCourbePositive = true;
        }

        public EC_SortieAna(ESTOR sTor)
        {
            this.IsAssociated = false;
            this.SPWM = sTor;

            this.BorneMax = 90; // pourcent
            this.BorneMin = 1;  // pourcent
            this.ValZero = 0;
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_SortieAna
}
