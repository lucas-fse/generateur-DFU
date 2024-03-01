using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Form = System.Windows.Forms;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Xml;
using System.Xml.Linq;
using JAY.PegaseCore;
using System.Configuration;
using System.Collections.Specialized;

namespace JAY.Affichage.ViewModel
{
    public class CalibrateAxisViewModel:Mvvm.ViewModelBase
    {
        #region Constantes

        #endregion

        #region Variables

        private Double _borneMax;
        private Double _borneMin;
        private Double _valZero;
        private EC_SortieAna _sortie;
        private Boolean _courbePositive;

        #endregion

        #region Propriétés

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
                this.EvalValZero();
                RaisePropertyChanged("IsCourbePositive");
                RaisePropertyChanged("IsCourbeNegative");
                RaisePropertyChanged("IsCourbePositiveVisibility");
                RaisePropertyChanged("IsCourbeNegativeVisibility");
            }
        } // endProperty: IsCourbePositive

        /// <summary>
        /// La visibilité des contrôles liés au choix 'courbe positive'
        /// </summary>
        public Visibility IsCourbePositiveVisibility
        {
            get
            {
                Visibility Result = Visibility.Hidden;

                if (IsCourbePositive)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: IsCourbePositiveVisibility

        /// <summary>
        /// La visibilité des contrôles liés au choix 'courbe négative'
        /// </summary>
        public Visibility IsCourbeNegativeVisibility
        {
            get
            {
                Visibility Result = Visibility.Hidden;

                if (IsCourbeNegative)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: IsCourbeNegativeVisibility

        /// <summary>
        /// La courbe est-elle négative ?
        /// </summary>
        public Boolean IsCourbeNegative
        {
            get
            {
                return !this.IsCourbePositive;
            }
        } // endProperty: IsCourbeNegative

        /// <summary>
        /// Unite de la la sortie analogique lié (V ou mA)
        /// </summary>
        public String Unite
        {
            get
            {
                String Result = "";

                if (this.Sortie != null)
                {
                    if (this.Sortie.SAna != null)
                    {
                        if (this.Sortie.SAna.UIType != TypeUI.TUI_vref)
                        {
                            Result = this.Sortie.SAna.UIUnit;
                        }
                        else
                        {
                            Result = "% VRef";
                        }
                    }
                    else
                    {
                        Result = "%";
                    }
                }

                return Result;
            }
        } // endProperty: Unite

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
                this.EvalValZero();
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
                this.EvalValZero();

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
        } // endProperty: ValZero

        /// <summary>
        /// La sortie associée à l'axe
        /// </summary>
        public EC_SortieAna Sortie
        {
            get
            {
                return this._sortie;
            }
            set
            {
                this._sortie = value;

                RaisePropertyChanged("Sortie");
                RaisePropertyChanged("Unite");
                RaisePropertyChanged("IsCourbePositive");
                RaisePropertyChanged("IsCourbeNegative");
            }
        } // endProperty: Sortie

        #endregion

        public CalibrateAxisViewModel()
        {
            // Initialiser les valeurs
            if (!IsInDesignMode)
            {
                RaisePropertyChanged("IsCourbePositive");
                RaisePropertyChanged("IsCourbeNegative");
            }
            else
            {

            }

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }
        
        /// <summary>
        /// Evaluer la valeur Zero
        /// </summary>
        public void EvalValZero ( )
        {
            Double vzero;

            if (this.IsCourbeNegative)
            {
                vzero = (this._borneMax + this._borneMin) / 2;
            }
            else
            {
                vzero = this._borneMin;
            }

            this._valZero = vzero;
        } // endMethod: EvalValZero

        #region Méthodes

        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("InfoText");
            }
        } // endMethod: ReceiveMessage

        #endregion

        #region Commands

        #endregion
    }
}
