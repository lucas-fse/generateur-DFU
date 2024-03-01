using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Le comportement des sorties / relais dans le cadre de la configuration facile
    /// </summary>
    public class EC_Output : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private Boolean _isUsed = false;
        private ESTOR _stor;
        private OrganCommand _organ;
        private EC_Position _parentPosition = null;
        private Boolean _isEnable;
        private Boolean _isEquationDisable;
        private EC_Mode _parentMode;
         

        #endregion

        // Propriétés
        #region Propriétés
        
        /// <summary>
        /// Le mode parent de l'effecteur
        /// </summary>
        public EC_Mode ParentMode
        {
            get
            {
                return this._parentMode;
            }
            set
            {
                this._parentMode = value;
                RaisePropertyChanged("ParentMode");
            }
        } // endProperty: ParentMode

        /// <summary>
        /// La position parente de la sortie
        /// </summary>
        public EC_Position ParentPosition
        {
            get
            {
                return this._parentPosition;
            }
            set
            {
                this._parentPosition = value;
                RaisePropertyChanged("ParentPosition");
            }
        } // endProperty: ParentPosition

        /// <summary>
        /// La sortie TOR en lien avec cette sortie
        /// </summary>
        public ESTOR STOR
        {
            get
            {
                return this._stor;
            }
            set
            {
                this._stor = value;
                RaisePropertyChanged("STOR");
            }
        } // endProperty: STOR

        /// <summary>
        /// L'Organe lié à la sortie
        /// </summary>
        public OrganCommand Organ
        {
            get
            {
                return this._organ;
            }
            set
            {
                this._organ = value;
                RaisePropertyChanged("Organ");
            }
        } // endProperty: Organ

        /// <summary>
        /// La sortie est-elle utilisée dans ce contexte ?
        /// </summary>
        public Boolean IsUsed
        {
            get
            {
                return this._isUsed;
            }
            set
            {
                this._isUsed = value;
                if (this.ParentPosition != null)
                {
                    this.ParentPosition.EvalIsChecked();
                }
                if (this.ParentMode != null)
                {
                    // Si on change un check dans 'all mode' évaluer le blocage des contrôles sur l'ensemble
                    if (this.ParentMode.RefMode.Position == 32)
                    {
                        Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_MAJVIEW));
                    }
                }
                RaisePropertyChanged("IsUsed");
            }
        } // endProperty: IsUsed

        /// <summary>
        /// La sortie est-elle disponible dans ce contexte ?
        /// </summary>
        public Boolean IsEnable
        {
            get
            {
                return this._isEnable;
            }
            set
            {
                // Si la sortie n'est pas bloquée par les équations...
                if (!this.IsEquationDisable)
                {
                    if (PegaseData.Instance.ModuleT.TypeMT == MT.TIMO)
                    {
                        if (this.STOR != null)
                        {
                            if (this.STOR.IsPWM)
                            {
                                this._isEnable = false;
                            }
                            else
                            {
                                this._isEnable = value;
                            }
                        }
                        else
                        {
                            this._isEnable = value;
                        }
                    }
                    else
                    {
                        this._isEnable = value;
                    }
                    if (!this._isEnable)
                    {
                        this.IsUsed = false;
                    }
                }
                else
                {
                    // Sinon, bloqué la sortie
                    this._isEnable = false;
                }
                RaisePropertyChanged("IsEnable"); 
            }
        } // endProperty: IsEnable

        /// <summary>
        /// La sortie est-elle bloqué manuellement par les équations ?
        /// </summary>
        public Boolean IsEquationDisable
        {
            get
            {
                return this._isEquationDisable;
            }
            set
            {
                this._isEquationDisable = value;
                if (this._isEquationDisable)
                {
                    this.SetIsEnable(false);
                    this.SetIsUsed(false);
                }
                RaisePropertyChanged("IsEquationDisable");
            }
        } // endProperty: IsEquationDisable

        #endregion

        // Constructeur
        #region Constructeur

        public EC_Output(ESTOR STor)
        {
            this.STOR = STor;
            this.IsEquationDisable = false;
            this.SetIsEnable(true);
        }

        public EC_Output(OrganCommand organ)
        {
            this.Organ = organ;
            this.IsEquationDisable = false;
            this.SetIsEnable(true);
            this.SetIsUsed(false);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Mettre à jour la valeur IsUsed, sans provoquer un rafraichissement de l'affichage
        /// Le rafraichissement de l'affichage doit être effectué manuellement
        /// </summary>
        public void SetIsUsed ( Boolean Value )
        {
            this._isUsed = Value;
            if (this.ParentPosition != null)
            {
                this.ParentPosition.EvalIsChecked();
            }
            RaisePropertyChanged("IsUsed");
        } // endMethod: SetIsUsed
        
        /// <summary>
        /// Mettre à jour la valeur IsEnable, sans provoquer un rafraichissement de l'affichage
        /// Le rafraichissement de l'affichage doit être provoqué manuellement
        /// </summary>
        public void SetIsEnable ( Boolean Value )
        {
            if (!this.IsEquationDisable)
            {
                if (PegaseData.Instance.ModuleT.TypeMT == MT.TIMO)
                {
                    if (this.STOR != null)
                    {
                        if (this.STOR.IsPWM)
                        {
                            this._isEnable = false;
                        }
                        else
                        {
                            this._isEnable = Value;
                        }
                    }
                    else
                    {
                        this._isEnable = Value;
                    }
                }
                else
                {
                    this._isEnable = Value;
                }
                if (!this._isEnable)
                {
                    this.SetIsUsed(false);
                }
            }
            else
            {
                // Sinon, bloqué la sortie
                this._isEnable = false;
                this.SetIsUsed(false);
                
            }
            RaisePropertyChanged("IsEnable"); 
        } // endMethod: SetIsEnable

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_Output
}
