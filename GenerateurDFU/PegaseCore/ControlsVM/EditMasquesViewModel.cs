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
using JAY.XMLCore;
using JAY.FileCore;
using System.Configuration;
using System.Collections.Specialized;

namespace JAY.PegaseCore.ControlsVM
{
    public class EditMasquesViewModel:Mvvm.ViewModelBase
    {
        #region Constantes

        private const String AUXILIAIRE1 = "A13";
        public const String COMMAND_ACCELEROMETRE = "ACCELEROMETRE";
        public const String COMMAND_ARRET_URGENCE = "ARRET_URGENCE";
        public const String COMMAND_CHECKSTYLE = "CHECK";
        public const String COMMAND_DEFAULTSTYLE = "DEFAULT";
        public const String COMMAND_CADENASSTYLE = "CADENAS";
        public const String COMMAND_CHECKSTYLE_PLUS = "CHECKPLUS";
        public const String COMMAND_AXE_GROUPE = "AXE_GROUPE";

        public const String CHECK_STYLEDEM = "ToggleButtonStyleEtiquetteCheckDem";
        public const String CHECK_STYLEPLUS = "ToggleButtonStyleEtiquetteCheckPlus";
        public const String CHECK_STYLE = "ToggleButtonStyleEtiquetteCheck";
        public const String DEFAULT_STYLE_RED = "ToggleButtonStyleEtiquette";
        public const String DEFAULT_STYLE_GREY = "ToggleButtonStyleEtiquette2";
        public const String CADENAS_STYLE = "ToggleButtonStyleEtiquetteCheckCadenas";
        public const String AXE_STYLE = "ToggleButtonStyleAxe2";
        public const String AXE_STYLE_V= "ToggleButtonStyleAxeV2";
        public const String AXE_STYLE_DEFAULT = "ToggleButtonStyleAxe";
        public const String AXE_STYLE_DEFAULT_V = "ToggleButtonStyleAxeV";

        #endregion

        #region Variables
        private UInt32 _mask32;
        private UInt16 _mask16;

        private ObservableCollection<String> _listVariables;
        private Int16 _currentvariable;

        private Boolean _selectonetoggle;

        private Boolean _isEtatAccelerometre;
        private Visibility _isliste;
        private Boolean _setAllOrgane;

        // Int32
        private Boolean _etatAccelerometre;             // Bit0
        private Boolean _etatArretUrgence;              // Bit0
        private Boolean _etatBtnMarche;                 // Bit1
        private Boolean _etatBtnNav1;                   // Bit2
        private Boolean _etatBtnNav2;                   // Bit3
        private Boolean _etatBtn01;                     // Bit4
        private Boolean _etatBtn02;                     // Bit5
        private Boolean _etatBtn03;                     // Bit6
        private Boolean _etatBtn04;                     // Bit7
        private Boolean _etatBtn05;                     // Bit8
        private Boolean _etatBtn06;                     // Bit9
        private Boolean _etatBtn07;                     // Bit10
        private Boolean _etatBtn08;                     // Bit11
        private Boolean _etatBtn09;                     // Bit12
        private Boolean _etatBtn10;                     // Bit13
        private Boolean _etatBtnJoystick01;             // Bit14
        private Boolean _etatBtnJoystick02;             // Bit15
        private Boolean _etatBtnJoystick03;             // Bit16 -> non utilisé
        private Boolean _etatBtnAuxiliaire01;           // Bit17
        private Boolean _etatBtnAuxiliaire02;           // Bit18
        private Boolean _etatBtnAuxiliaire03;           // Bit19
        private Boolean _etatToggle01;                  // Bit20
        private Boolean _etatToggle02;                  // Bit21
        private Boolean _etatToggle03;                  // Bit22
        private Boolean _etatToggle04;                  // Bit23
        private Boolean _etatToggle05;                  // Bit24
        private Boolean _etatToggle06;                  // Bit25
        private Boolean _etatToggle07;                  // Bit26
        private Boolean _etatToggle08;                  // Bit27
        private Boolean _etatToggle09;                  // Bit28
        private Boolean _etatToggle10;                  // Bit29
        private Boolean _etatToggle11;                  // Bit30
        private Boolean _etatToggle12;                  // Bit31

        private Boolean _testToggle00 = true;
        private Boolean _testToggle01 = true;                  // Bit20
        private Boolean _testToggle02 = true;                  // Bit21
        private Boolean _testToggle03 = true;                  // Bit22
        private Boolean _testToggle04 = true;                  // Bit23
        private Boolean _testToggle05 = true;                  // Bit24
        private Boolean _testToggle06 = true;                  // Bit25
        private Boolean _testToggle07 = true;                  // Bit26
        private Boolean _testToggle08 = true;                  // Bit27
        private Boolean _testToggle09 = true;                  // Bit28
        private Boolean _testToggle10 = true;                  // Bit29
        private Boolean _testToggle11 = true;                  // Bit30
        private Boolean _testToggle12 = true;                  // Bit31

        // Int16
        private Boolean _etatOrganeAxe01;               // Bit0 -> Horizontal
        private Boolean _etatOrganeAxe02;               // Bit1 -> Vertical
        private Boolean _etatOrganeAxe03;               // Bit2 -> Horizontal
        private Boolean _etatOrganeAxe04;               // Bit3 -> Vertical
        private Boolean _etatOrganeAxe05;               // Bit4 -> Horizontal
        private Boolean _etatOrganeAxe06;               // Bit5
        private Boolean _etatOrganeCom12_01;            // Bit6
        private Boolean _etatOrganeCom12_02;            // Bit7
        //private Boolean _etatOrganeAxeAna01;            // Bit8  -> Horizontal
        //private Boolean _etatOrganeAxeAna02;            // Bit9  -> Vertical
        //private Boolean _etatOrganeAxeAna03;            // Bit10 -> Horizontal
        //private Boolean _etatOrganeAxeAna04;            // Bit11 -> Vertical
        //private Boolean _etatOrganeAxeAna05;            // Bit12 -> Horizontal
        //private Boolean _etatOrganeAxeAna06;            // Bit13 -> Vertical
        private Boolean _etatPotar01;                   // Bit14
        private Boolean _etatPotar02;                   // Bit15


        private UIElement _currentView;
        private String _moName;

        private Visibility _mask32Visibility;
        private Visibility _mask16Visibility;
        private Visibility _auxVisibility;

        private Style _toggleStyle;
        private Style _toggleStyleBP;
        private Style _toggleStyletoggle;

        private Style _toggleButtonStyleAxe;
        private Style _toggleButtonStyleAxeV;
        private Visibility _isVisibleAide;

        private string _NomFenetreEditMasque = "";


        #endregion

        #region Propriétés


        public String NomFenetreEditMasque
        {
            get
            {
                return LanguageSupport.Get().GetText(_NomFenetreEditMasque);
            }
            set
            {
                _NomFenetreEditMasque = value;
            }
        }

        /// <summary>
        /// Le libellé sélectionné tout
        /// </summary>
        public String LibelSelectAll
        {
            get
            {
                String Result;
                if (ToggleStyleBP == (Style)Application.Current.Resources[DEFAULT_STYLE_RED])
                {
                    Result = "/EDIT_VARIABLE/BTN_SELECT_ALL";
                }
                else
                {
                    Result = "/EDIT_VARIABLE/BTN_DESELECT_ALL";
                }

                return Result;
            }
        } // endProperty: LibelSelectAll

        /// <summary>
        /// Le libellé désélectionné tout
        /// </summary>
        public String LibelDeSelectAll
        {
            get
            {
                String Result;
                if (ToggleStyleBP == (Style)Application.Current.Resources[DEFAULT_STYLE_RED])
                {
                    Result = "/EDIT_VARIABLE/BTN_DESELECT_ALL";
                }
                else
                {
                    Result = "/EDIT_VARIABLE/BTN_SELECT_ALL";
                }

                return Result;
            }
        } // endProperty: LibelSelectAll


        public ObservableCollection<String> ListVariables
        {
            get
            {
                return this._listVariables;
            }
            set
            {
                this._listVariables = value;
                RaisePropertyChanged("ListVariables");
            }
        }

        public Int16 CurrentVariable
        {
            get
            {
                return this._currentvariable;
            }
            set
            {
                this._currentvariable = value;
                RaisePropertyChanged("CurrentVariable");
            }
        }

        /// <summary>
        /// Le bit0 correspond-il à l'accéléromètre, ou à l'arrêt d'urgence
        /// </summary>
        public Boolean IsEtatAccelerometre
        {
            get
            {
                return this._isEtatAccelerometre;
            }
            set
            {
                this._isEtatAccelerometre = value;
                RaisePropertyChanged("IsEtatAccelerometre");
            }
        } // endProperty: IsEtatAccelerometre

        public Boolean IsAxeGroupe
        {
            get; set;
        }

        public Visibility Isliste
        {
            get
            {
                return this._isliste;
            }
            set
            {
                this._isliste = value;
                RaisePropertyChanged("isListe");
            }
        } // endProperty: IsEtatAccelerometre
   

        /// <summary>
        /// Le style des boutons utilisés pour le masque
        /// </summary>
        public Style ToggleStyle
        {
            get
            {
                if (this._toggleStyle == null)
                {
                    this._toggleStyle = (Style)Application.Current.Resources[DEFAULT_STYLE_RED];
                }
                return this._toggleStyle;
            }
            set
            {
                this._toggleStyle = value;
                RaisePropertyChanged("ToggleStyleBP");
                RaisePropertyChanged("LibelDeSelectAll");
                RaisePropertyChanged("LibelSelectAll");
                
            }
        } // endProperty: ToggleStyle


        public Style ToggleStyleBP
            {
            get
            {
                if (this._toggleStyleBP == null)
                {
                    this._toggleStyleBP = (Style)Application.Current.Resources[DEFAULT_STYLE_RED];
                }
                return this._toggleStyleBP;
            }
            set
            {
                this._toggleStyleBP = value;
                RaisePropertyChanged("ToggleStyleBP");
                RaisePropertyChanged("LibelDeSelectAll");
                RaisePropertyChanged("LibelSelectAll");
            }
        }
        public Style ToggleStyleToggle1
        { get {return ToggleStyleToggleEnable(1); } set{ ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle2
        { get { return ToggleStyleToggleEnable(2); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle3
        { get { return ToggleStyleToggleEnable(3); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle4
        { get { return ToggleStyleToggleEnable(4); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle5
        { get { return ToggleStyleToggleEnable(5); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle6
        { get { return ToggleStyleToggleEnable(6); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle7
        { get { return ToggleStyleToggleEnable(7); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle8
        { get { return ToggleStyleToggleEnable(8); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle9
        { get { return ToggleStyleToggleEnable(9); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle10
        { get { return ToggleStyleToggleEnable(10); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle11
        { get { return ToggleStyleToggleEnable(11); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggle12
        { get { return ToggleStyleToggleEnable(12); } set { ToggleStyleToggle = value; } }
        public Style ToggleStyleToggleMarche
        { get { return ToggleStyleToggleEnable(0); } set { ToggleStyleToggle = value; } }

        public Style ToggleStyleToggle
        {
            get
            {
                if (this._toggleStyletoggle == null)
                {
                    this._toggleStyletoggle = (Style)Application.Current.Resources[DEFAULT_STYLE_RED];
                }
                return this._toggleStyletoggle;
            }
            set
            {
                this._toggleStyletoggle = value;
                RaisePropertyChanged("ToggleStyleToggle");
                RaisePropertyChanged("LibelDeSelectAll");
                RaisePropertyChanged("LibelSelectAll");
            }
        }

        private Style ToggleStyleToggleEnable(int nmrOrgane)
        {
            Style result = this._toggleStyletoggle;
            switch (nmrOrgane)
            {

                case 0:
                    if (_testToggle00 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 1:
                    if (_testToggle01 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 2:
                    if (_testToggle02 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 3:
                    if (_testToggle03 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 4:
                    if (_testToggle04 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 5:
                    if (_testToggle05 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 6:
                    if (_testToggle06 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 7:
                    if (_testToggle07 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 8:
                    if (_testToggle08 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 9:
                    if (_testToggle09 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 10:
                    if (_testToggle10 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 11:
                    if (_testToggle11 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
                case 12:
                    if (_testToggle12 == false)
                    {
                        result = (Style)Application.Current.Resources[DEFAULT_STYLE_GREY];
                    }
                    break;
            }
            return result;
        }

        public Style ToggleButtonStyleAxe
        {
            get
            {
                if (this._toggleButtonStyleAxe == null)
                {
                    this._toggleButtonStyleAxe = (Style)Application.Current.Resources[AXE_STYLE_DEFAULT];
                }
                return this._toggleButtonStyleAxe;
            }
            set
            {
                this._toggleButtonStyleAxe = value;
                RaisePropertyChanged("ToggleButtonStyleAxe");
                RaisePropertyChanged("LibelDeSelectAll");
                RaisePropertyChanged("LibelSelectAll");
            }
        }
        public Style ToggleButtonStyleAxeV
        {
            get
            {
                if (this._toggleButtonStyleAxeV == null)
                {
                    this._toggleButtonStyleAxeV = (Style)Application.Current.Resources[AXE_STYLE_DEFAULT_V];
                }
                return this._toggleButtonStyleAxeV;
            }
            set
            {
                this._toggleButtonStyleAxeV = value;
                RaisePropertyChanged("ToggleButtonStyleAxeV");
                RaisePropertyChanged("LibelDeSelectAll");
                RaisePropertyChanged("LibelSelectAll");
            }
        }
        /// <summary>
        /// La visibilité pour les auxiliaires
        /// </summary>
        public Visibility AuxVisibility
        {
            get
            {
                return this._auxVisibility;
            }
            set
            {
                this._auxVisibility = value;
                RaisePropertyChanged("AuxVisibility");
            }
        } // endProperty: AuxVisibility

        /// <summary>
        /// La visibilité pour le Mask32
        /// </summary>
        public Visibility Mask32Visibility
        {
            get
            {
                return this._mask32Visibility;
            }
            set
            {
                this._mask32Visibility = value;
                RaisePropertyChanged("Mask32Visibility");
            }
        } // endProperty: Mask32Visibility

        /// <summary>
        /// La visibilité pour le Mask16
        /// </summary>
        public Visibility Mask16Visibility
        {
            get
            {
                return this._mask16Visibility;
            }
            set
            {
                this._mask16Visibility = value;
                RaisePropertyChanged("Mask16Visibility");
            }
        } // endProperty: Mask16Visibility

        /// <summary>
        /// La vue permettant d'éditer le masque
        /// </summary>
        public UIElement CurrentView
        {
            get
            {
                return this._currentView;
            }
            private set
            {
                this._currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        } // endProperty: CurrentView

        /// <summary>
        /// Le masque 32 bits
        /// </summary>
        public UInt32 Mask32
        {
            get
            {
                return this._mask32;
            }
            set
            {
                this._mask32 = value;
                RaisePropertyChanged("Mask32");
            }
        } // endProperty: Mask1

        /// <summary>
        /// Le masque 16 bits
        /// </summary>
        public UInt16 Mask16
        {
            get
            {
                return this._mask16;
            }
            set
            {
                this._mask16 = value;
                RaisePropertyChanged("Mask16");
            }
        } // endProperty: Mask2

        /// <summary>
        /// Etat accelerometre (actif / non actif)
        /// </summary>
        public Boolean EtatAccelerometre
        {
            get
            {
                return this._etatAccelerometre;
            }
            set
            {
                this._etatAccelerometre = value;
                RaisePropertyChanged("EtatAccelerometre");
            }
        } // endProperty: EtatAccelerometre

        /// <summary>
        /// Etat de l'arrêt d'urgence (actif / non actif)
        /// </summary>
        public Boolean EtatArretUrgence
        {
            get
            {
                return this._etatArretUrgence;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatArretUrgence = value;
                RaisePropertyChanged("EtatArretUrgence");
            }
        } // endProperty: EtatArretUrgence

        /// <summary>
        /// Etat du bouton marche (actif / non actif)
        /// </summary>
        public Boolean EtatBtnMarche
        {
            get
            {
                //if (_testToggle00 == true)
                //{
                //    return this._etatBtnMarche;
                //}
                //else
                //{
                //    return false;
                //}
                return this._etatBtnMarche;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtnMarche = value;
                }
                else
                {
                    this._etatBtnMarche = true;
                }
                RaisePropertyChanged("EtatBtnMarche");
            }
        } // endProperty: EtatBtnMarche

        /// <summary>
        /// Etat du bouton de navigation 1 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnNav1
        {
            get
            {
                return this._etatBtnNav1;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtnNav1 = value;
                }
                else
                {
                    this._etatBtnNav1 = true;
                }
                RaisePropertyChanged("EtatBtnNav1");
            }
        } // endProperty: EtatBtnNav1

        /// <summary>
        /// L'etat du bouton de navigation 2
        /// </summary>
        public Boolean EtatBtnNav2
        {
            get
            {
                return this._etatBtnNav2;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtnNav2 = value;
                }
                else
                {
                    this._etatBtnNav2 = true;
                }
                RaisePropertyChanged("EtatBtnNav2");
            }
        } // endProperty: EtatBtnNav2

        /// <summary>
        /// Etat du bouton 01 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn01
        {
            get
            {
                return this._etatBtn01;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn01 = value;
                    RaisePropertyChanged("EtatBtn01");
                }
                else
                {
                    this._etatBtn01 = true;
                }
            }
        } // endProperty: EtatBtn01

        /// <summary>
        /// Etat du bouton 02 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn02
        {
            get
            {
                return this._etatBtn02;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn02 = value;
                }
                else
                {
                    this._etatBtn02 = true;
                }
                RaisePropertyChanged("EtatBtn02");
            }
        } // endProperty: EtatBtn02

        /// <summary>
        /// Etat du bouton 03 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn03
        {
            get
            {
                return this._etatBtn03;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn03 = value;
                }
                else
                {
                    this._etatBtn03 = true;
                }
                RaisePropertyChanged("EtatBtn03");
            }
        } // endProperty: EtatBtn03

        /// <summary>
        /// Etat du bouton 04 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn04
        {
            get
            {
                return this._etatBtn04;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn04 = value;
                }
                else
                {
                    this._etatBtn04 = true;
                }
                RaisePropertyChanged("EtatBtn04");
            }
        } // endProperty: EtatBtn04

        /// <summary>
        /// Etat du bouton 05 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn05
        {
            get
            {
                return this._etatBtn05;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn05 = value;
                }
                else
                {
                    this._etatBtn05 = true;
                }
                RaisePropertyChanged("EtatBtn05");
            }
        } // endProperty: EtatBtn05

        /// <summary>
        /// Etat du bouton 06 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn06
        {
            get
            {
                return this._etatBtn06;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn06 = value;
                }
                else
                {
                    this._etatBtn06 = true;
                }
                RaisePropertyChanged("EtatBtn06");
            }
        } // endProperty: EtatBtn06

        /// <summary>
        /// Etat du bouton 07 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn07
        {
            get
            {
                return this._etatBtn07;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn07 = value;
                }
                else
                {
                    this._etatBtn07 = true;
                }
                RaisePropertyChanged("EtatBtn07");
            }
        } // endProperty: EtatBtn07

        /// <summary>
        /// Etat du bouton 08 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn08
        {
            get
            {
                return this._etatBtn08;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn08 = value;
                }
                else
                {
                    this._etatBtn08 = true;
                }
                RaisePropertyChanged("EtatBtn08");
            }
        } // endProperty: EtatBtn08

        /// <summary>
        /// Etat du bouton 09 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn09
        {
            get
            {
                return this._etatBtn09;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn09 = value;
                }
                else
                {
                    this._etatBtn09 = true;
                }
                RaisePropertyChanged("EtatBtn09");
            }
        } // endProperty: EtatBtn09

        /// <summary>
        /// Etat du bouton 10 (actif / non actif)
        /// </summary>
        public Boolean EtatBtn10
        {
            get
            {
                return this._etatBtn10;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                if (_setAllOrgane)
                {
                    this._etatBtn10 = value;
                }
                else
                {
                    this._etatBtn10 = true;
                }
                RaisePropertyChanged("EtatBtn10");
            }
        } // endProperty: EtatBtn10

        /// <summary>
        /// Etat du bouton joystick 01 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnJoystick01
        {
            get
            {
                return this._etatBtnJoystick01;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnJoystick01 = value;
                RaisePropertyChanged("EtatBtnJoystick01");
            }
        } // endProperty: EtatBtnJoystick01

        /// <summary>
        /// Etat du bouton joystick 02 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnJoystick02
        {
            get
            {
                return this._etatBtnJoystick02;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnJoystick02 = value;
                RaisePropertyChanged("EtatBtnJoystick02");
            }
        } // endProperty: EtatBtnJoystick02

        /// <summary>
        /// Etat du bouton joystick 03 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnJoystick03
        {
            get
            {
                return this._etatBtnJoystick03;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnJoystick03 = value;
                RaisePropertyChanged("EtatBtnJoystick03");
            }
        } // endProperty: EtatBtnJoystick03

        /// <summary>
        /// Etat du bouton auxiliaire 01 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnAuxiliaire01
        {
            get
            {
                return this._etatBtnAuxiliaire01;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnAuxiliaire01 = value;
                RaisePropertyChanged("EtatBtnAuxiliaire01");
            }
        } // endProperty: EtatBtnAuxiliaire01

        /// <summary>
        /// Etat du bouton auxiliaire 02 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnAuxiliaire02
        {
            get
            {
                return this._etatBtnAuxiliaire02;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnAuxiliaire02 = value;
                RaisePropertyChanged("EtatBtnAuxiliaire02");
            }
        } // endProperty: EtatBtnAuxiliaire02

        /// <summary>
        /// Etat du bouton auxiliaire 03 (actif / non actif)
        /// </summary>
        public Boolean EtatBtnAuxiliaire03
        {
            get
            {
                return this._etatBtnAuxiliaire03;
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatBtnAuxiliaire03 = value;
                RaisePropertyChanged("EtatBtnAuxiliaire03");
            }
        } // endProperty: EtatBtnAuxiliaire03


        /// <summary>
        /// Etat du toggle 01 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle01
        {
            get
            {
                if (_testToggle01 == true)
                {
                    return this._etatToggle01;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle01 = value;
                RaisePropertyChanged("EtatToggle01");
            }
        } // endProperty: EtatToggle01

        /// <summary>
        /// L'état du toggle 02 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle02
        {
            get
            {
                if (_testToggle02 == true)
                {
                    return this._etatToggle02;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle02 = value;
                RaisePropertyChanged("EtatToggle02");
            }
        } // endProperty: EtatToggle02

        /// <summary>
        /// Etat du toggle 03
        /// </summary>
        public Boolean EtatToggle03
        {
            get
            {
                if (_testToggle03 == true)
                {
                    return this._etatToggle03;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle03 = value;
                RaisePropertyChanged("EtatToggle03");
            }
        } // endProperty: EtatToggle03

        /// <summary>
        /// Etat du toggle 04 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle04
        {
            get
            {
                if (_testToggle04 == true)
                {
                    return this._etatToggle04;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle04 = value;
                RaisePropertyChanged("EtatToggle04");
            }
        } // endProperty: EtatToggle04

        /// <summary>
        /// Etat du toggle 05
        /// </summary>
        public Boolean EtatToggle05
        {
            get
            {
                if (_testToggle05 == true)
                {
                    return this._etatToggle05;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle05 = value;
                RaisePropertyChanged("EtatToggle05");
            }
        } // endProperty: EtatToggle05

        /// <summary>
        /// Etat du toggle 06 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle06
        {
            get
            {
                if (_testToggle06 == true)
                {
                    return this._etatToggle06;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle06 = value;
                RaisePropertyChanged("EtatToggle06");
            }
        } // endProperty: EtatToggle06

        /// <summary>
        /// Etat du toggle 07
        /// </summary>
        public Boolean EtatToggle07
        {
            get
            {
                if (_testToggle07 == true)
                {
                    return this._etatToggle07;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle07 = value;
                RaisePropertyChanged("EtatToggle07");
            }
        } // endProperty: EtatToggle07

        /// <summary>
        /// Etat du toggle 08 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle08
        {
            get
            {
                if (_testToggle08 == true)
                {
                    return this._etatToggle08;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle08 = value;
                RaisePropertyChanged("EtatToggle08");
            }
        } // endProperty: EtatToggle08

        /// <summary>
        /// Etat du toggle 09 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle09
        {
            get
            {
                if (_testToggle09 == true)
                {
                    return this._etatToggle09;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle09 = value;
                RaisePropertyChanged("EtatToggle09");
            }
        } // endProperty: EtatToggle09

        /// <summary>
        /// Etat du toggle 10
        /// </summary>
        public Boolean EtatToggle10
        {
            get
            {
                if (_testToggle10 == true)
                {
                    return this._etatToggle10;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle10 = value;
                RaisePropertyChanged("EtatToggle10");
            }
        } // endProperty: EtatToggle10

        /// <summary>
        /// Etat du toggle 11 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle11
        {
            get
            {
                if (_testToggle11 == true)
                {
                    return this._etatToggle11;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle11 = value;
                RaisePropertyChanged("EtatToggle11");
            }
        } // endProperty: EtatToggle11

        /// <summary>
        /// Etat du toggle 12 (actif / non actif)
        /// </summary>
        public Boolean EtatToggle12
        {
            get
            {
                if (_testToggle12 == true)
                {
                    return this._etatToggle12;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_selectonetoggle)
                {
                    RasToggle();
                }
                this._etatToggle12 = value;
                RaisePropertyChanged("EtatToggle12");
            }
        } // endProperty: EtatToggle12

        /// <summary>
        /// Etat de l'axe à cran 01 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeAxe01
        {
            get
            {
                return this._etatOrganeAxe01;
            }
            set
            {
                this._etatOrganeAxe01 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe02 = value;
                    RaisePropertyChanged("EtatOrganeAxe02");
                }
                RaisePropertyChanged("EtatOrganeAxe01");
            }
        } // endProperty: EtatOrganeAxeACran01

        /// <summary>
        /// Etat de l'axe à cran 02 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeAxe02
        {
            get
            {
                return this._etatOrganeAxe02;
            }
            set
            {
                this._etatOrganeAxe02 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe01 = value;
                    RaisePropertyChanged("EtatOrganeAxe01");
                }
                RaisePropertyChanged("EtatOrganeAxe02");
            }
        } // endProperty: EtatOrganeAxeACran02

        /// <summary>
        /// Etat organe axe à cran 03 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeAxe03
        {
            get
            {
                return this._etatOrganeAxe03;
            }
            set
            {
                this._etatOrganeAxe03 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe04 = value;
                    RaisePropertyChanged("EtatOrganeAxe04");
                }
                RaisePropertyChanged("EtatOrganeAxe03");
            }
        } // endProperty: EtatOrganeAxeACran03

        /// <summary>
        /// Etat organe axe à cran 04 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeAxe04
        {
            get
            {
                return this._etatOrganeAxe04;
            }
            set
            {
                this._etatOrganeAxe04 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe03 = value;
                    RaisePropertyChanged("EtatOrganeAxe03");
                }
                RaisePropertyChanged("EtatOrganeAxe04");
            }
        } // endProperty: EtatOrganeAxeACran04

        /// <summary>
        /// EtatOrganeAxe05
        /// </summary>
        public Boolean EtatOrganeAxe05
        {
            get
            {
                return this._etatOrganeAxe05;
            }
            set
            {
                this._etatOrganeAxe05 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe06 = value;
                    RaisePropertyChanged("EtatOrganeAxe06");
                }
                RaisePropertyChanged("EtatOrganeAxe05");
            }
        } // endProperty: EtatOrganeAxeACran05

        /// <summary>
        /// Etat organe axe à cran 06
        /// </summary>
        public Boolean EtatOrganeAxe06
        {
            get
            {
                return this._etatOrganeAxe06;
            }
            set
            {
                this._etatOrganeAxe06 = value;
                if (this.IsAxeGroupe == true)
                {
                    this._etatOrganeAxe05 = value;
                    RaisePropertyChanged("EtatOrganeAxe05");
                }
                RaisePropertyChanged("EtatOrganeAxe06");
            }
        } // endProperty: EtatOrganeAxeACran06

        /// <summary>
        /// Etat de l'organe Com12 01 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeCom12_01
        {
            get
            {
                return this._etatOrganeCom12_01;
            }
            set
            {
                this._etatOrganeCom12_01 = value;
                RaisePropertyChanged("EtatOrganeCom12_01");
            }
        } // endProperty: EtatOrganeCom12

        /// <summary>
        /// Etat de l'organe Com12 02 (actif / non actif)
        /// </summary>
        public Boolean EtatOrganeCom12_02
        {
            get
            {
                return this._etatOrganeCom12_02;
            }
            set
            {
                this._etatOrganeCom12_02 = value;
                RaisePropertyChanged("EtatOrganeCom12_02");
            }
        } // endProperty: EtatOrganeCom12_02

        /// <summary>
        /// Etat potar (actif / non actif)
        /// </summary>
        public Boolean EtatPotar01
        {
            get
            {
                return this._etatPotar01;
            }
            set
            {
                this._etatPotar01 = value;
                RaisePropertyChanged("EtatPotar01");
            }
        } // endProperty: EtatPotar01

        /// <summary>
        /// Etat potar 02 (actif / non actif)
        /// </summary>
        public Boolean EtatPotar02
        {
            get
            {
                return this._etatPotar02;
            }
            set
            {
                this._etatPotar02 = value;
                RaisePropertyChanged("EtatPotar02");
            }
        } // endProperty: EtatPotar02

        /// <summary>
        /// Le nom du MO
        /// </summary>
        public String MOName
        {
            get
            {
                return this._moName;
            }
            set
            {
                this._moName = value;
                RaisePropertyChanged("MOName");
            }
        } // endProperty: MOName

        #endregion

        public EditMasquesViewModel()
        {
            // Initialiser les valeurs
            if (!IsInDesignMode)
            {
                this.IsEtatAccelerometre = true;
            }

            // Construire les commandes
            this.CreateCommandSelectAll();
            this.CreateCommandDeselectAll();
            this.CreateCommandAide();

            this.IsVisibleAide = Visibility.Hidden;

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

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
        
        /// <summary>
        /// Assigne les paramètres de l'analyses
        /// </summary>
        public void SetParam ( String Param )
        {
            String[] Params;
            // 1 - Délimiter les paramètres
            Params = Param.Split(new Char[] { '|' });

            // 2 - Traiter chacun des paramètres et configurer le viewModel en fonction
            foreach (var parametre in Params)
            {
                if (parametre.Contains("EDIT_VARIABLE/"))
                {
                    NomFenetreEditMasque = parametre;
                }
                else
                {
                    switch (parametre)
                    {
                        case COMMAND_CHECKSTYLE_PLUS:
                            this.ToggleStyleBP = (Style)Application.Current.Resources[CHECK_STYLEDEM];
                            this.ToggleStyleToggle = (Style)Application.Current.Resources[CHECK_STYLEPLUS];
                            this.ToggleButtonStyleAxe = (Style)Application.Current.Resources[AXE_STYLE];
                            this.ToggleButtonStyleAxeV = (Style)Application.Current.Resources[AXE_STYLE_V];
                            this.IsAxeGroupe = true;
                            break;
                        case COMMAND_CHECKSTYLE:
                            this.ToggleStyleBP = (Style)Application.Current.Resources[CHECK_STYLE];
                            this.ToggleStyleToggle = (Style)Application.Current.Resources[CHECK_STYLE];
                            this.IsAxeGroupe = false;
                            break;
                        case COMMAND_DEFAULTSTYLE:
                            this.ToggleStyleBP = (Style)Application.Current.Resources[DEFAULT_STYLE_RED];
                            this.ToggleStyleToggle = (Style)Application.Current.Resources[DEFAULT_STYLE_RED];
                            this.IsAxeGroupe = false;
                            break;
                        case COMMAND_CADENASSTYLE:
                            this.ToggleStyleBP = (Style)Application.Current.Resources[CADENAS_STYLE];
                            this.ToggleStyleToggle = (Style)Application.Current.Resources[CADENAS_STYLE];
                            this.IsAxeGroupe = false;
                            break;
                        case COMMAND_ACCELEROMETRE:
                            this.IsEtatAccelerometre = true;
                            break;
                        case COMMAND_ARRET_URGENCE:
                            this.IsEtatAccelerometre = false;
                            break;
                        case COMMAND_AXE_GROUPE:
                            this.IsAxeGroupe = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        } // endMethod: SetParam

        /// <summary>
        /// Decode le Mask32 et initialise les booléens
        /// </summary>
        public void DecodeMask ( UInt32 Mask32 )
        {
            this.Mask32 = Mask32;
            this.Mask16 = 0;
            this.DecodeMask();
            this.Mask32Visibility = Visibility.Visible;
            this.Mask16Visibility = Visibility.Hidden;
        } // endMethod: DecodeMask

        /// <summary>
        /// Décoder les deux masques et initialiser les booléens
        /// </summary>
        public void DecodeMask ( UInt32 Mask32, UInt16 Mask16 )
        {
            this.Mask32 = Mask32;
            this.Mask16 = Mask16;
            this.DecodeMask();
            this.Mask32Visibility = Visibility.Visible;
            this.Mask16Visibility = Visibility.Visible;
        } // endMethod: DecodeMask

        /// <summary>
        /// Décoder les deux masques et initialiser les booléens
        /// </summary>
        private void DecodeMask ( )
        {
            // Décoder le masque 32 bit
            Boolean[] M32 = new Boolean[32];
            for (int i = 0; i < 32; i++)
            {
                UInt32 M = 1;
                M = M << i;

                if ((M | this.Mask32) == this.Mask32)
                {
                    M32[i] = true;
                }
                else
                {
                    M32[i] = false;
                }
            }

            // Assigner les valeurs des bits aux variables correspondantes
            if (this.IsEtatAccelerometre)
            {
                this.EtatAccelerometre = M32[0];
            }
            else
            {
                this.EtatArretUrgence = M32[0];
            }
            this.EtatBtnMarche = M32[1];
            this.EtatBtnNav1 = M32[2];
            this.EtatBtnNav2 = M32[3];
            this.EtatBtn01 = M32[4];
            this.EtatBtn02 = M32[5];
            this.EtatBtn03 = M32[6];
            this.EtatBtn04 = M32[7];
            this.EtatBtn05 = M32[8];
            this.EtatBtn06 = M32[9];
            this.EtatBtn07 = M32[10];
            this.EtatBtn08 = M32[11];
            this.EtatBtn09 = M32[12];
            this.EtatBtn10 = M32[13];
            this.EtatBtnJoystick01 = M32[14];
            this.EtatBtnJoystick02 = M32[15];
            this.EtatBtnJoystick03 = M32[16];
            this.EtatBtnAuxiliaire01 = M32[17];
            this.EtatBtnAuxiliaire02 = M32[18];
            this.EtatBtnAuxiliaire03 = M32[19];
            this.EtatToggle01 = M32[20];
            this.EtatToggle02 = M32[21];
            this.EtatToggle03 = M32[22];
            this.EtatToggle04 = M32[23];
            this.EtatToggle05 = M32[24];
            this.EtatToggle06 = M32[25];
            this.EtatToggle07 = M32[26];
            this.EtatToggle08 = M32[27];
            this.EtatToggle09 = M32[28];
            this.EtatToggle10 = M32[29];
            this.EtatToggle11 = M32[30];
            this.EtatToggle12 = M32[31];

            // Décoder le masque 16 bit
            Boolean[] M16 = new Boolean[16];
            Int32 M16_32 = (Int32)this.Mask16;
            for (Int32 i = 0; i < 16; i++)
            {
                Int32 M = 1;
                M = M << i;

                if ((M | M16_32) == M16_32)
                {
                    M16[i] = true;
                }
                else
                {
                    M16[i] = false;
                }
            }

            // Assigner les variables des bits aux variables correspondantes
            this.EtatOrganeAxe01 = M16[0];
            this.EtatOrganeAxe02 = M16[1];
            this.EtatOrganeAxe03 = M16[2];
            this.EtatOrganeAxe04 = M16[3];
            this.EtatOrganeAxe05 = M16[4];
            this.EtatOrganeAxe06 = M16[5];
            this.EtatOrganeCom12_01 = M16[6];
            this.EtatOrganeCom12_02 = M16[7];
            //this.EtatOrganeAxeAna01 = M16[8];
            //this.EtatOrganeAxeAna02 = M16[9];
            //this.EtatOrganeAxeAna03 = M16[10];
            //this.EtatOrganeAxeAna04 = M16[11];
            //this.EtatOrganeAxeAna05 = M16[12];
            //this.EtatOrganeAxeAna06 = M16[13];
            this.EtatPotar01 = M16[14];
            this.EtatPotar02 = M16[15];
        } // endMethod: DecodeMask

        private void RasToggle()
        {
            this._etatBtnMarche = true;
            this._etatBtnNav1 = true;
            this._etatBtnNav2 = true;
            this._etatBtn01 = true;
            this._etatBtn02 = true;
            this._etatBtn03 = true;
            this._etatBtn04 = true;
            this._etatBtn05 = true;
            this._etatBtn06 = true;
            this._etatBtn07 = true;
            this._etatBtn08 = true;
            this._etatBtn09 = true;
            this._etatBtn10 = true;
            this._etatBtnJoystick01 = true;
            this._etatBtnJoystick02 = true;
            this._etatBtnJoystick03 = true;
            this._etatBtnAuxiliaire01 = true;
            this._etatBtnAuxiliaire02 = true;
            this._etatBtnAuxiliaire03 = true;
            this._etatToggle01 = true;
            this._etatToggle02 = true;
            this._etatToggle03 = true;
            this._etatToggle04 = true;
            this._etatToggle05 = true;
            this._etatToggle06 = true;
            this._etatToggle07 = true;
            this._etatToggle08 = true;
            this._etatToggle09 = true;
            this._etatToggle10 = true;
            this._etatToggle11 = true;
            this._etatToggle12 = true;
            RaisePropertyChanged("EtatBtn01");
            RaisePropertyChanged("EtatBtn02");
            RaisePropertyChanged("EtatBtn03");
            RaisePropertyChanged("EtatBtn04");
            RaisePropertyChanged("EtatBtn05");
            RaisePropertyChanged("EtatBtn06");
            RaisePropertyChanged("EtatBtn07");
            RaisePropertyChanged("EtatBtn08");
            RaisePropertyChanged("EtatBtn09");
            RaisePropertyChanged("EtatBtn10");
            RaisePropertyChanged("EtatBtnNav1");
            RaisePropertyChanged("EtatBtnNav2");
            RaisePropertyChanged("EtatBtnJoystick01");
            RaisePropertyChanged("EtatBtnJoystick02");
            RaisePropertyChanged("EtatBtnJoystick03");
            RaisePropertyChanged("EtatBtnMarche");
            RaisePropertyChanged("EtatToggle01");
            RaisePropertyChanged("EtatToggle02");
            RaisePropertyChanged("EtatToggle03");
            RaisePropertyChanged("EtatToggle04");
            RaisePropertyChanged("EtatToggle05");
            RaisePropertyChanged("EtatToggle06");
            RaisePropertyChanged("EtatToggle07");
            RaisePropertyChanged("EtatToggle08");
            RaisePropertyChanged("EtatToggle09");
            RaisePropertyChanged("EtatToggle10");
            RaisePropertyChanged("EtatToggle11");
            RaisePropertyChanged("EtatToggle12");
            RaisePropertyChanged("EtatBtnAuxiliaire01");
            RaisePropertyChanged("EtatBtnAuxiliaire02");
            RaisePropertyChanged("EtatBtnAuxiliaire03");

        }

        /// <summary>
        /// Encoder les deux masques en fonction de la valeur des booléens organes
        /// </summary>
        public void EncodMask ( out UInt32 Mask32, out UInt16 Mask16 )
        {
            Boolean[] M32 = new Boolean[32];
            Boolean[] M16 = new Boolean[16];

            // Initialiser le Mask32
            if (this.IsEtatAccelerometre)
            {
                M32[0] = this.EtatAccelerometre;
            }
            else
            {
                M32[0] = this.EtatArretUrgence;
            }
            
            M32[1] = this.EtatBtnMarche;
            M32[2] = this.EtatBtnNav1;
            M32[3] = this.EtatBtnNav2;
            M32[4] = this.EtatBtn01;
            M32[5] = this.EtatBtn02;
            M32[6] = this.EtatBtn03;
            M32[7] = this.EtatBtn04;
            M32[8] = this.EtatBtn05;
            M32[9] = this.EtatBtn06;
            M32[10] = this.EtatBtn07;
            M32[11] = this.EtatBtn08;
            M32[12] = this.EtatBtn09;
            M32[13] = this.EtatBtn10;
            M32[14] = this.EtatBtnJoystick01;
            M32[15] = this.EtatBtnJoystick02;
            M32[16] = this.EtatBtnJoystick03;
            M32[17] = this.EtatBtnAuxiliaire01;
            M32[18] = this.EtatBtnAuxiliaire02;
            M32[19] = this.EtatBtnAuxiliaire03;
            M32[20] = this.EtatToggle01 || !_testToggle01;
            M32[21] = this.EtatToggle02 || !_testToggle02; 
            M32[22] = this.EtatToggle03 || !_testToggle03;
            M32[23] = this.EtatToggle04 || !_testToggle04;
            M32[24] = this.EtatToggle05 || !_testToggle05;
            M32[25] = this.EtatToggle06 || !_testToggle06;
            M32[26] = this.EtatToggle07 || !_testToggle07;
            M32[27] = this.EtatToggle08 || !_testToggle08;
            M32[28] = this.EtatToggle09 || !_testToggle09;
            M32[29] = this.EtatToggle10 || !_testToggle10;
            M32[30] = this.EtatToggle11 || !_testToggle11;
            M32[31] = this.EtatToggle12 || !_testToggle12;

            // Calculer le mask32
            UInt32 M;
            Mask32 = 0;

            for (int i = 0; i < 32; i++)
            {
                if (M32[i] == true)
                {
                    M = 1;
                    M = M << i;
                    Mask32 += M;
                }
            }

            // Initialiser le Mask16
            M16[0] = this.EtatOrganeAxe01;
            M16[1] = this.EtatOrganeAxe02;
            M16[2] = this.EtatOrganeAxe03;
            M16[3] = this.EtatOrganeAxe04;
            M16[4] = this.EtatOrganeAxe05;
            M16[5] = this.EtatOrganeAxe06;
            M16[6] = this.EtatOrganeCom12_01;
            M16[7] = this.EtatOrganeCom12_02;
            M16[8] = this.EtatOrganeAxe01;
            M16[9] = this.EtatOrganeAxe02;
            M16[10] = this.EtatOrganeAxe03;
            M16[11] = this.EtatOrganeAxe04;
            M16[12] = this.EtatOrganeAxe05;
            M16[13] = this.EtatOrganeAxe06;
            M16[14] = this.EtatPotar01;
            M16[15] = this.EtatPotar02;
            Mask16 = 0;

            for (int i = 0; i < 16; i++)
            {
                if (M16[i] == true)
                {
                    M = 1;
                    M = M << i;
                    Mask16 += (UInt16)M;
                }
            }
        } // endMethod: EncodMask


        public void Configtoggle(String Memologique, bool Configurable)
        {
            switch (Memologique)
                {
                case "M":
                    _testToggle00 = Configurable;
                    break;
                case "A1":
                    _testToggle01 = Configurable;
                    break;
                case "A2":
                    _testToggle02 = Configurable;
                    break;
                case "A3":
                    _testToggle03 = Configurable;
                    break;
                case "A4":
                    _testToggle04 = Configurable;
                    break;
                case "A5":
                    _testToggle05 = Configurable;
                    break;
                case "A6":
                    _testToggle06 = Configurable;
                    break;
                case "A7":
                    _testToggle07 = Configurable;
                    break;
                case "A8":
                    _testToggle08 = Configurable;
                    break;
                case "A9":
                    _testToggle09 = Configurable;
                    break;
                case "A10":
                    _testToggle10 = Configurable;
                    break;
                case "A11":
                    _testToggle11 = Configurable;
                    break;
                case "A12":
                    _testToggle12 = Configurable;
                    break;
                default:
                    break;

            }

        }

        /// <summary>
        /// Le type du MO
        /// </summary>
        public void SetMOType ( MO MO )
        {
            // Définir le type de MO
            switch (MO.TypeMO)
            {
                case MO.BETA2:
                    this.MOName = MO.BETA2;
                    this.CurrentView = new JAY.PegaseCore.Controls.B2();
                    break;
                case MO.BETA6:
                    this.MOName = MO.BETA6;
                    this.CurrentView = new JAY.PegaseCore.Controls.B6();
                    break;
                case MO.GAMA6:
                    this.MOName = MO.GAMA6;
                    this.CurrentView = new JAY.PegaseCore.Controls.G6();
                    break;
                case MO.GAMA10:
                    this.MOName = MO.GAMA10;
                    this.CurrentView = new JAY.PegaseCore.Controls.G1();
                    break;
                case MO.MOKA2:
                    this.MOName = MO.MOKA2;
                    this.CurrentView = new JAY.PegaseCore.Controls.M2();
                    break;
                case MO.MOKA3:
                    this.MOName = MO.MOKA3;
                    this.CurrentView = new JAY.PegaseCore.Controls.M3();
                    break;
                case MO.MOKA6:
                    this.MOName = MO.MOKA6;
                    this.CurrentView = new JAY.PegaseCore.Controls.M6();
                    break;
                case MO.PIKA1:
                    this.MOName = MO.PIKA1;
                    this.CurrentView = new JAY.PegaseCore.Controls.P1();
                    break;
                case MO.PIKA2:
                    this.MOName = MO.PIKA2;
                    this.CurrentView = new JAY.PegaseCore.Controls.P2();
                    break;
                default:
                    break;
            }

            // Initialiser l'affichage en fonction du type de matériel
            var Query = from organe in MO.OrganesCommandes
                        where organe.NomOrganeMO == AUXILIAIRE1
                        select organe;

            if (Query.Count() > 0)
            {
                this.AuxVisibility = Visibility.Visible;
            }
            else
            {
                this.AuxVisibility = Visibility.Hidden;
            }
        } // endMethod: SetMOType


        public Boolean SelectOneToggle
        {
            get
            {
                return this._selectonetoggle;
            }
            set
            {
                this._selectonetoggle = value;
            }
        }

        public Boolean SetAllOrgane
        {
            get
            {
                return this._setAllOrgane;
            }
            set
            {
                this._setAllOrgane = value;
            }
        }

        #endregion

        #region Commands

        #region CommandSelectAll
        /// <summary>
        /// La commande SelectAll
        /// </summary>
        public ICommand CommandSelectAll
        {
            get;
            internal set;
        } // endProperty: CommandSelectAll

        public ICommand CommandAide
        {
            get;
            internal set;
        }

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandSelectAll()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandSelectAll = new Mvvm.Command.RelayCommand(ExecuteCommandSelectAll, CanExecuteCommandSelectAll);
        } // endMethod: CreateCommandSelectAll

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandAide()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandAide = new Mvvm.Command.RelayCommand(ExecuteCommandAide, CanCommandAide);
        } // endMethod: CreateCommandSelectAll

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandSelectAll()
        {
            this.Mask32 = (UInt32)0xFFFFFFFF;
            this.Mask16 = (UInt16)0xFFFF;
            this.DecodeMask();
        } // endMethod: ExecuteCommandSelectAll

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandSelectAll()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandSelectAll 

        public void ExecuteCommandAide()
        {
            JAY.PegaseCore.Controls.EditAide EditMasquEditAide = new JAY.PegaseCore.Controls.EditAide();
            JAY.PegaseCore.ControlsVM.EditAideViewModel EditAideVM = EditMasquEditAide.DataContext as JAY.PegaseCore.ControlsVM.EditAideViewModel;
            EditMasquEditAide.ShowDialog();
        }

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanCommandAide()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandSelectAll 
        #endregion
        public Visibility IsVisibleAide
        {
            get { return _isVisibleAide; }
            set {
                _isVisibleAide = value;
                RaisePropertyChanged("IsVisibleAide");
            }
        }


        #region CommandDeselectAll
        /// <summary>
        /// La commande DeselectAll
        /// </summary>
        public ICommand CommandDeselectAll
        {
            get;
            internal set;
        } // endProperty: CommandDeselectAll

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandDeselectAll()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandDeselectAll = new Mvvm.Command.RelayCommand(ExecuteCommandDeselectAll, CanExecuteCommandDeselectAll);
        } // endMethod: CreateCommandDeselectAll

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandDeselectAll()
        {
            this.Mask32 = 0;
            this.Mask16 = 0;
            this.DecodeMask();
        } // endMethod: ExecuteCommandDeselectAll

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandDeselectAll()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandDeselectAll 

        


        /// <summary>
        /// init des testtoggle correspondant à la capacité d'un toggle a etre configurer
        /// </summary>
        public void InitTestToggle()
        {
            _testToggle01 = true;
            _testToggle02 = true;
            _testToggle03 = true;
            _testToggle04 = true;
            _testToggle05 = true;
            _testToggle06 = true;
            _testToggle07 = true;
            _testToggle08 = true;
            _testToggle09 = true;
            _testToggle10 = true;
            _testToggle11 = true;
            _testToggle12 = true;
        }

        #endregion

        #endregion
    }
}
