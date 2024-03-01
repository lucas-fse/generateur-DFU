using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Media;
using JAY.PegaseCore;
using System.Windows.Input;

namespace JAY.PegaseCore
{
    public enum TypeOrgaAna
    {
        Joystick = 0,
        EmulationJoystick = 1
    }

    public enum visibilityType
    {
        JoystickTrue = 0,
        JoystickFalse = 1,
        EmulationJoystickTrue = 2,
        EmulationJoystickFalse = 3
    }

    /// <summary>
    /// Class de base pour gérer un organe analogique
    /// </summary>
    public class EC_OrganAna : Mvvm.ViewModelBase
    {
        #region Constantes

        private const Int32 SEUIL_MIN = 0;
        private const Int32 SEUIL_MAX = 100;

        private const String AXANA1 = "AXANA1";
        private const String AXANA2 = "AXANA2";
        private const String AXANA3 = "AXANA3";
        private const String AXANA4 = "AXANA4";
        private const String AXANA5 = "AXANA5";
        private const String AXANA6 = "AXANA6";

        #endregion

        // Variables
        #region Variables

        private TypeOrgaAna _type;
        private Boolean _isUsed;
        private ObservableCollection<String> _listType;
        private ObservableCollection<OrganCommand> _listJoystick;
        private ObservableCollection<OrganCommand> _listButton;
        private ObservableCollection<String> _listButtonText;
        private Visibility _joystickHVisibility;
        private Visibility _joystickVVisibility;
        private Int32 _currentJoystick;
        private Int32 _incrementJoystick;
        private OrganCommand _bntIncrement;
        private OrganCommand _btnDecrement;
        private Int32 _lineNumber;
        private EC_SortieAna _linkedOAna;
        private Int32 _currentBtnIncrement;
        private Int32 _currentBtnDecrement;
        private Double _incrementPercent;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'élément de la liste de choix pour le bouton incrémenté
        /// </summary>
        public Int32 CurrentBtnIncrement
        {
            get
            {
                return this._currentBtnIncrement;
            }
            set
            {
                this._currentBtnIncrement = value;
                RaisePropertyChanged("CurrentBtnIncrement");
            }
        } // endProperty: CurrentBtnIncrement

        /// <summary>
        /// L'élément de la liste de choix pour le bouton décrémenté
        /// </summary>
        public Int32 CurrentBtnDecrement
        {
            get
            {
                return this._currentBtnDecrement;
            }
            set
            {
                this._currentBtnDecrement = value;
                RaisePropertyChanged("CurrentBtnDecrement");

            }
        } // endProperty: CurrentBtnDecrement

        /// <summary>
        /// La sortie analogique liée
        /// </summary>
        public EC_SortieAna LinkedOAna
        {
            get
            {
                return this._linkedOAna;
            }
            set
            {
                this._linkedOAna = value;
                RaisePropertyChanged("LinkedOAna");
            }
        } // endProperty: LinkedOAna

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
        /// La visibilité du joystick droit
        /// </summary>
        public Visibility JoystickDroiteVisibility
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Hidden;

                if (this.ListJoysticks.Count > 0 && this.CurrentJoystick > -1)
                {
                    if (this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA1 || this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA2)
                    {
                        Result = System.Windows.Visibility.Visible;
                    } 
                }

                return Result;
            }
        } // endProperty: JoystickDroiteVisibility

        /// <summary>
        /// La visibilité du joystick gauche
        /// </summary>
        public Visibility JoystickGaucheVisibility
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Hidden;

                if (this.ListJoysticks.Count > 0 && this.CurrentJoystick > -1)
                {
                    if (this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA3 || this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA4)
                    {
                        Result = System.Windows.Visibility.Visible;
                    }
                }

                return Result;
            }
        } // endProperty: JoystickGaucheVisibility

        /// <summary>
        /// La visibilité du joystick central
        /// </summary>
        public Visibility JoystickCentralVisibility
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Hidden;

                if (this.ListJoysticks.Count > 0 && this.CurrentJoystick > -1)
                {
                    if (this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA5 || this.ListJoysticks[this.CurrentJoystick].NomOrganeMO == AXANA6)
                    {
                        Result = System.Windows.Visibility.Visible;
                    }
                }

                return Result;
            }
        } // endProperty: JoystickCentralVisibility

        /// <summary>
        /// Le bouton utilisé pour l'incrémentation
        /// </summary>
        public OrganCommand BtnIncrement
        {
            get
            {
                return this._bntIncrement;
            }
            set
            {
                this._bntIncrement = value;
                RaisePropertyChanged("BtnIncrement");
            }
        } // endProperty: BtnIncrement

        /// <summary>
        /// Le bouton utilisé pour la décrémentation
        /// </summary>
        public OrganCommand BtnDecrement
        {
            get
            {
                return this._btnDecrement;
            }
            set
            {
                this._btnDecrement = value;
                RaisePropertyChanged("BtnDecrement");
            }
        } // endProperty: BtnDecrement

        /// <summary>
        /// La liste des joysticks
        /// </summary>
        public ObservableCollection<OrganCommand> ListJoysticks
        {
            get
            {
                return this._listJoystick;
            }
            set
            {
                this._listJoystick = value;
                RaisePropertyChanged("ListJoysticks");
            }
        } // endProperty: ListJoysticks

        /// <summary>
        /// La liste des boutons
        /// </summary>
        public ObservableCollection<OrganCommand> ListButtons
        {
            get
            {
                return this._listButton;
            }
            set
            {
                this._listButton = value;
                RaisePropertyChanged("ListButtons");
            }
        } // endProperty: ListButtons

        /// <summary>
        /// La liste des textes à utiliser lors du choix des boutons
        /// </summary>
        public ObservableCollection<String> ListTextButton
        {
            get
            {
                return this._listButtonText;
            }
            set
            {
                this._listButtonText = value;
                RaisePropertyChanged("ListTextButton");
            }
        } // endProperty: ListTextButton

        /// <summary>
        /// La liste des types accessibles
        /// </summary>
        public ObservableCollection<String> ListType
        {
            get
            {
                if (this._listType == null)
                {
                    this.InitListType();
                }

                return this._listType;
            }
            set
            {
                this._listType = value;
                RaisePropertyChanged("Type");
                RaisePropertyChanged("ListType");
            }
        } // endProperty: ListType

        /// <summary>
        /// La visibilité intégré
        /// </summary>
        public visibilityType Visibility
        {
            get
            {
                visibilityType Result;
                if (this.Type == TypeOrgaAna.Joystick)
	            {
                    if (this.IsUsed)
                    {
                        Result = visibilityType.JoystickTrue;
                    }
                    else
                    {
                        Result = visibilityType.JoystickFalse;
                    }
	            }
                else
                {
                    if (this.IsUsed)
                    {
                        Result = visibilityType.EmulationJoystickTrue;
                    }
                    else
                    {
                        Result = visibilityType.EmulationJoystickFalse;
                    }
                }

                return Result;
            }
        } // endProperty: Visibility

        /// <summary>
        /// Le joystick horizontal est-il visible
        /// </summary>
        public Visibility JoystickHVisibility
        {
            get
            {
                return this._joystickHVisibility;
            }
            set
            {
                this._joystickHVisibility = value;
                RaisePropertyChanged("JoystickHVisibility");
            }
        } // endProperty: JoystickHVisibility

        /// <summary>
        /// Le joystick vertical est-il visible
        /// </summary>
        public Visibility JoystickVVisibility
        {
            get
            {
                return this._joystickVVisibility;
            }
            set
            {
                this._joystickVVisibility = value;
                RaisePropertyChanged("JoystickVVisibility");
            }
        } // endProperty: JoystickVVisibility

        /// <summary>
        /// Le joystick en cours de sélection
        /// </summary>
        public Int32 CurrentJoystick
        {
            get
            {
                return this._currentJoystick;
            }
            set
            {
                this._currentJoystick = value;
                if (this.ListJoysticks.Count == 0)
                {
                    this._currentJoystick = -1;
                }
                if (this._currentJoystick > -1)
                {
                    String SNumAxe = this.ListJoysticks[value].Mnemologique;
                    Double NumAxe = Convert.ToDouble(SNumAxe.Substring(SNumAxe.Length - 1));
                    if (NumAxe % 2 == 0)
                    {
                        // pair -> axe vertical
                        this.JoystickHVisibility = System.Windows.Visibility.Hidden;
                        this.JoystickVVisibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        // impair -> axe horizontal
                        this.JoystickHVisibility = System.Windows.Visibility.Visible;
                        this.JoystickVVisibility = System.Windows.Visibility.Hidden;
                    }
                }
                RaisePropertyChanged("CurrentJoystick");
                RaisePropertyChanged("JoystickCentralVisibility");
                RaisePropertyChanged("JoystickDroiteVisibility");
                RaisePropertyChanged("JoystickGaucheVisibility");
            }
        } // endProperty: CurrentJoystick

        /// <summary>
        /// Le type en cours de sélection
        /// </summary>
        public Int32 CurrentType
        {
            get
            {
                return (Int32)this.Type;
            }
            set
            {
                if (this.ListJoysticks.Count == 0)
                {
                    this.Type = TypeOrgaAna.EmulationJoystick;
                }
                else
                {
                    this.Type = (TypeOrgaAna)value;
                }

                RaisePropertyChanged("CurrentType");
            }
        } // endProperty: CurrentType

        /// <summary>
        /// Le type de contrôle de la sortie analogique
        /// </summary>
        public TypeOrgaAna Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
                RaisePropertyChanged("Type");
                RaisePropertyChanged("Visibility");
            }
        } // endProperty: Type

        /// <summary>
        /// Cette organe est-il utilisé
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
                //this.CurrentJoystick = this._currentJoystick;
                RaisePropertyChanged("IsUsed");
                RaisePropertyChanged("Visibility");
                RaisePropertyChanged("CurrentJoystick");
                RaisePropertyChanged("CurrentType");
                RaisePropertyChanged("VisibilityListType");
                RaisePropertyChanged("JoystickHVisibility");
                RaisePropertyChanged("JoystickVVisibility");
            }
        } // endProperty: IsUsed

        /// <summary>
        /// La visibilité de la liste de choix du type
        /// </summary>
        public Visibility VisibilityListType
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Visible;

                if (this.IsUsed == false)
                {
                    Result = System.Windows.Visibility.Hidden;
                }
                else if (this.IsUsed && this.ListJoysticks.Count == 0)
                {
                    Result = System.Windows.Visibility.Hidden;
                }

                return Result;
            }
        } // endProperty: VisibilityListType

        /// <summary>
        /// L'incrément pour l'émulation joystick
        /// </summary>
        public Int32 IncrementJoystick
        {
            get
            {
                return this._incrementJoystick;
            }
            set
            {
                this._incrementJoystick = value;
                
                RaisePropertyChanged("IncrementJoystick");
            }
        } // endProperty: IncrementJoystick

        /// <summary>
        /// l'incrément en pourcent
        /// </summary>
        public Double IncrementPercent
        {
            get
            {
                return this._incrementPercent;
            }
            set
            {
                this._incrementPercent = Math.Round(value, 1);

                if (this._incrementPercent < 0)
                {
                    this._incrementPercent = 0;
                }
                else if(this._incrementPercent > 100)
                {
                    this._incrementPercent = 100;
                }
                Double v = (1023.0 * this._incrementPercent) / 100;
                this.IncrementJoystick = (Int32)v;

                RaisePropertyChanged("IncrementPercent");
            }
        } // endProperty: IncrementPercent

        #endregion

        // Constructeur
        #region Constructeur

        public EC_OrganAna()
        {
            if (!IsInDesignMode)
            {
                this.InitListType();
                this.InitListButton();
                this.InitListJoystick();
                if (this.ListJoysticks.Count == 0)
                {
                    this.Type = TypeOrgaAna.EmulationJoystick;
                    RaisePropertyChanged("CurrentType");
                }
                else
                {
                    this.Type = TypeOrgaAna.Joystick;
                }
            }

            this.CreateCommandDecremente();
            this.CreateCommandIncremente();
            this.CreateCommandCalibrateJoystick();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Acquérir le numéro d'ordre dans la liste en fonction de la valeur du bouton
        /// </summary>
        public Int32 GetListTextButtonPos(String label, out String MnemologiqueOrgane, out String Position)
        {
            Int32 Result = -1;

            String[] parts = label.Split(new Char[]{' '});

            if (parts != null && parts.Length > 2)
            {
                // Trouver le numéro du bouton dans la liste
                for (int i = 0; i < this.ListTextButton.Count; i++)
                {
                    if (this.ListTextButton[i].Contains(parts[0]) && this.ListTextButton[i].Contains(parts[parts.Length - 1]))
                    {
                        Result = i;
                        break;
                    }
                }

                // Trouver la mnemologique du bouton
                var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                            where organ.NomOrganeMO == parts[0]
                            select organ.Mnemologique;

                if (Query.Count() > 0)
                {
                    MnemologiqueOrgane = Query.First();
                }
                else
                {
                    MnemologiqueOrgane = "";
                }

                // Trouver la position du bouton
                switch (parts[parts.Length - 1])
                {
                    case "Pos00":
                        Position = "PAS_D_APPUI";
                        break;
                    case "Pos01":
                        Position = "APPUI_SIMPLE";
                        break;
                    case "Pos02":
                        Position = "APPUI_DOUBLE";
                        break;
                    default:
                        Position = "";
                        break;
                } 
            }
            else
            {
                Result = 0;
                MnemologiqueOrgane = "";
                Position = "";
            }
            if (Result < 0)
            {
                Result = 0;
                MnemologiqueOrgane = PegaseData.Instance.MOperateur.OrganesCommandes[0].Mnemologique;
                Position = "PAS_D_APPUI";
            }
            return Result;
        } // endMethod: GetListTextButtonPos

        /// <summary>
        /// Initialiser la liste des types
        /// </summary>
        public void InitListType ( )
        {
            this._listType = new ObservableCollection<String>();
            this._listType.Add("Joystick");
            this._listType.Add("Emulation joystick");
        } // endMethod: InitListType

        /// <summary>
        /// Initialiser la liste des joysticks disponibles
        /// </summary>
        public void InitListJoystick ( )
        {
            this._listJoystick = new ObservableCollection<OrganCommand>();

            var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organ.MnemoHardFamilleMO == "AXE"
                        select organ;

            foreach (var organ in Query)
            {
                this._listJoystick.Add(organ);
            }
        } // endMethod: InitListJoystick
        
        /// <summary>
        /// Initialiser la liste des boutons disponibles
        /// </summary>
        public void InitListButton ( )
        {
            this._listButton = new ObservableCollection<OrganCommand>();
            this._listButtonText = new ObservableCollection<String>();

            var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organ.MnemoHardFamilleMO == "BT"
                        select organ;

            foreach (var organ in Query)
            {
                for (int i = 0; i < organ.NbPosOrgane; i++)
                {
                    String text = String.Format("{0} - {1} : Pos{2:00}", organ.NomOrganeMO, organ.MnemoClient, i);
                    this._listButtonText.Add(text);
                }
                this._listButton.Add(organ);
            }
        } // endMethod: InitListButton

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandIncremente
        /// <summary>
        /// La commande Incremente
        /// </summary>
        public ICommand CommandIncremente
        {
            get;
            internal set;
        } // endProperty: CommandIncremente

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandIncremente()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandIncremente = new Mvvm.Command.RelayCommand(ExecuteCommandIncremente, CanExecuteCommandIncremente);
        } // endMethod: CreateCommandIncremente

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandIncremente()
        {
            this.IncrementPercent += 0.1;
        } // endMethod: ExecuteCommandIncremente

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandIncremente()
        {
            Boolean Result = false;

            if (this.IncrementPercent < SEUIL_MAX )
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandIncremente 
        #endregion

        #region CommandDecremente
        /// <summary>
        /// La commande Decremente
        /// </summary>
        public ICommand CommandDecremente
        {
            get;
            internal set;
        } // endProperty: CommandDecremente

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandDecremente()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandDecremente = new Mvvm.Command.RelayCommand(ExecuteCommandDecremente, CanExecuteCommandDecremente);
        } // endMethod: CreateCommandDecremente

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandDecremente()
        {
            this.IncrementPercent -= 0.1;
        } // endMethod: ExecuteCommandDecremente

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandDecremente()
        {
            Boolean Result = false;

            if (this.IncrementPercent > SEUIL_MIN )
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandDecremente 
        #endregion

        #region CommandCalibrateJoystick
        /// <summary>
        /// La commande CalibrateJoystick
        /// </summary>
        public ICommand CommandCalibrateJoystick
        {
            get;
            internal set;
        } // endProperty: CommandCalibrateJoystick

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCalibrateJoystick()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCalibrateJoystick = new Mvvm.Command.RelayCommand(ExecuteCommandCalibrateJoystick, CanExecuteCommandCalibrateJoystick);
        } // endMethod: CreateCommandCalibrateJoystick

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCalibrateJoystick()
        {
            // Configurer la boite de dialogue de calibration
            JAY.Affichage.View.CalibrAxeAnaView CAAV = new Affichage.View.CalibrAxeAnaView();
            JAY.Affichage.ViewModel.CalibrateAxisViewModel CAVM = CAAV.DataContext as JAY.Affichage.ViewModel.CalibrateAxisViewModel;
            CAVM.Sortie = this.LinkedOAna;
            CAVM.BorneMax = this.LinkedOAna.BorneMax;
            CAVM.BorneMin = this.LinkedOAna.BorneMin;
            CAVM.IsCourbePositive = this.LinkedOAna.IsCourbePositive;
            
            // Calibrer
            if (CAAV.ShowDialog() == true)
            {
                // mettre à jour les valeurs du contrôle
                this.LinkedOAna.BorneMax = CAVM.BorneMax;
                this.LinkedOAna.BorneMin = CAVM.BorneMin;
                this.LinkedOAna.ValZero = CAVM.ValZero;
                this.LinkedOAna.IsCourbePositive = CAVM.IsCourbePositive;
            }
        } // endMethod: ExecuteCommandCalibrateJoystick

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCalibrateJoystick()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandCalibrateJoystick 
        #endregion

        #endregion
    } // endClass: EC_OrganAna
}
