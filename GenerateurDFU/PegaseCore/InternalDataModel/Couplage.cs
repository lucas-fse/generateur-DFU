using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using JAY.XMLCore;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    public class MTCouplage : Mvvm.ViewModelBase , ICloneable
    {
        private String _codeNatifMT;
        private Int32 _canalRadioMT;
        private Visibility _isVisible;
        private Boolean _isEnable;

        #region Propriété
        
        /// <summary>
        /// Le label pour le code identité natif du MT
        /// </summary>
        public String LabelCodeNatifMT
        {
            get
            {
                return "EDIT_VARIABLE/CODE_ID_MT";
            }
        } // endProperty: LabelCodeNatifMT

        /// <summary>
        /// AFaire: Ajouter une description de la propriété
        /// </summary>
        public String LabelCodeNatifMO
        {
            get
            {
                return "EDIT_VARIABLE/CODE_ID_MO";
            }
        } // endProperty: LabelCodeNatifMO

        /// <summary>
        /// Le label pour le canal radio
        /// </summary>
        public String LabelCanal
        {
            get
            {
                return "EDIT_VARIABLE/CANAL";
            }
        } // endProperty: LabelCanal

        /// <summary>
        /// La ligne est-elle utilisée ?
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
            }
        } // endProperty: IsUsed

        /// <summary>
        /// La ligne est-elle visible
        /// </summary>
        public Visibility IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
            }
        } // endProperty: IsVisible

        /// <summary>
        /// Le code natif du MT couplé
        /// </summary>
        public String CodeNatifMT
        {
            get
            {
                return this._codeNatifMT;
            }
            set
            {
                Regex regex = new Regex(Couplage.REGEX_HEXA);

                if (regex.IsMatch(value.ToUpper()))
                {
                    this._codeNatifMT = value.ToUpper();
                }
                
                RaisePropertyChanged("CodeNatifMT");
            }
        } // endProperty: CodeNatifMT

        /// <summary>
        /// Canal radio MT
        /// </summary>
        public Int32 CanalRadioMT
        {
            get
            {
                return this._canalRadioMT;
            }
            set
            {
                Int32 Value, ValueMax;
                // Déterminer la borne max du nombre de canaux
                if (PegaseData.Instance.ModuleT != null)
                {
                    if (PegaseData.Instance.ModuleT.Frequence == "433 MHz")
                    {
                        ValueMax = 64;
                    }
                    else if (PegaseData.Instance.ModuleT.Frequence == "869 MHz")
                    {
                        ValueMax = 12;
                    }
                    else
                    {
                        ValueMax = 32;
                    } 
                }
                else
                {
                    ValueMax = 64;
                }
                // Valider la valeur
                if (value <= ValueMax)
                {
                    Value = value;
                }
                else
                {
                    Value = this._canalRadioMT;
                }
                this._canalRadioMT = Value;
            }
        } // endProperty: CanalRadioMT 

        #endregion

        #region Constructeur

        public MTCouplage(String codeNatifMT, Int32 canalRadioMT)
        {
            this.CodeNatifMT = codeNatifMT;
            this.CanalRadioMT = canalRadioMT;

            this.CreateCommandIsEnable();
        }

        #endregion

        #region Méthodes
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Commandes

        #region CommandIsEnable
        /// <summary>
        /// La commande IsEnable
        /// </summary>
        public ICommand CommandIsEnable
        {
            get;
            internal set;
        } // endProperty: CommandIsEnable

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandIsEnable()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandIsEnable = new Mvvm.Command.RelayCommand(ExecuteCommandIsEnable, CanExecuteCommandIsEnable);
        } // endMethod: CreateCommandIsEnable

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandIsEnable()
        {
            if (this.IsEnable)
            {
                // ajouter une ligne si possible
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_ADD_NEW_LINE));
            }
            else
            {
                // supprimer la ligne
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_DEL_LINE));
            }

            RaisePropertyChanged("IsEnable");
        } // endMethod: ExecuteCommandIsEnable

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandIsEnable()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandIsEnable 
        #endregion

        #endregion
    }

    public class MOCouplage : Mvvm.ViewModelBase, ICloneable
    {
        private String _codeNatifMO;
        private Int32 _canalRadioMO;
        private Visibility _isVisible;
        private Boolean _isEnable;

        #region Propriété

        /// <summary>
        /// Le label pour le code identité du MO
        /// </summary>
        public String LabelCodeNatifMO
        {
            get
            {
                return "EDIT_VARIABLE/CODE_ID_MO";
            }
        } // endProperty: LabelCodeNatifMO

        /// <summary>
        /// Le label pour le canal radio
        /// </summary>
        public String LabelCanal
        {
            get
            {
                return "EDIT_VARIABLE/CANAL";
            }
        } // endProperty: LabelCanal

        /// <summary>
        /// La ligne est-elle utilisée ?
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
            }
        } // endProperty: IsUsed

        /// <summary>
        /// La ligne est-elle visible
        /// </summary>
        public Visibility IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
            }
        } // endProperty: IsVisible

        /// <summary>
        /// Le code natif du MO couplé
        /// </summary>
        public String CodeNatifMO
        {
            get
            {
                return this._codeNatifMO;
            }
            set
            {
                Regex regex = new Regex(Couplage.REGEX_HEXA);

                if (regex.IsMatch(value.ToUpper()))
                {
                    this._codeNatifMO = value.ToUpper();
                }

                RaisePropertyChanged("CodeNatifMO");
            }
        } // endProperty: CodeNatifMO

        /// <summary>
        /// Canal radio MO
        /// </summary>
        public Int32 CanalRadioMO
        {
            get
            {
                return this._canalRadioMO;
            }
            set
            {
                Int32 Value, ValueMax;
                // Déterminer la borne max du nombre de canaux
                if (PegaseData.Instance.MOperateur != null)
                {
                    if (PegaseData.Instance.MOperateur.Frequence == "433 MHz")
                    {
                        ValueMax = 64;
                    }
                    else if (PegaseData.Instance.MOperateur.Frequence == "869 MHz")
                    {
                        ValueMax = 12;
                    }
                    else
                    {
                        ValueMax = 32;
                    }
                }
                else
                {
                    ValueMax = 64;
                }
                // Valider la valeur
                if (value <= ValueMax)
                {
                    Value = value;
                }
                else
                {
                    Value = this._canalRadioMO;
                }
                this._canalRadioMO = Value;
            }
        } // endProperty: CanalRadioMO

        #endregion

        #region Constructeur

        public MOCouplage(String codeNatifMO, Int32 canalRadioMO)
        {
            this.CodeNatifMO = codeNatifMO;
            this.CanalRadioMO = canalRadioMO;

            this.CreateCommandIsEnable();
        }

        #endregion

        #region Méthodes
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Commandes

        #region CommandIsEnable
        /// <summary>
        /// La commande IsEnable
        /// </summary>
        public ICommand CommandIsEnable
        {
            get;
            internal set;
        } // endProperty: CommandIsEnable

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandIsEnable()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandIsEnable = new Mvvm.Command.RelayCommand(ExecuteCommandIsEnable, CanExecuteCommandIsEnable);
        } // endMethod: CreateCommandIsEnable

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandIsEnable()
        {
            if (this.IsEnable)
            {
                // ajouter une ligne si possible
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_ADD_NEW_LINE));
            }
            else
            {
                // supprimer la ligne
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_DEL_LINE));
            }

            RaisePropertyChanged("IsEnable");
        } // endMethod: ExecuteCommandIsEnable

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandIsEnable()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandIsEnable 
        #endregion

        #endregion
    }

    /// <summary>
    /// Données de couplage
    /// </summary>
    public class Couplage
    {
        public const String REGEX_HEXA = "^[A-F0-9]+$";

        // Variables
        #region Variables

        private Int32 _modeCouplage;
        private Int32 _periodeEcouteCanal;
        private UInt32 _masqueRetourMT1;
        private Int32 _nbModeSolo;
        private Int32 _masterSlave;
        private Int32 _modeLibCouplage;
        private ObservableCollection<MTCouplage> _mtCouplage;
        private ObservableCollection<MOCouplage> _moCouplage;
        private List<int> _ModeCouplageList;

        #endregion

        // Propriétés
        #region Propriétés

        public List<int> ModeCouplageList
        {
            get
            {
                int liaionactive = 0;
                if (_ModeCouplageList != null)
                {
                    _ModeCouplageList.Clear();
                }
                else
                {
                    _ModeCouplageList = new List<int>();
                }
                String XValue = PegaseData.Instance.GestionLiaisonFilaire.ChoixLiaisonFilaire;
                if (XValue != null && XValue != "")
                {
                    liaionactive = Tools.ConvertASCIIToInt32(XValue);
                }
                else
                {
                    liaionactive = 0;
                }
                _ModeCouplageList.Add(0);
                if (liaionactive == 0)
                {
                    _ModeCouplageList.Add(1);
                    _ModeCouplageList.Add(2);
                    _ModeCouplageList.Add(3);
                    _ModeCouplageList.Add(4);
                    _ModeCouplageList.Add(5);
                    if (ListmodePickAndCtrlAutorise.Count > 0)
                    {
                        _ModeCouplageList.Add(6);
                    }
                }
                else
                {
                    _ModeCouplageList.Add(5);
                    //if (ListmodePickAndCtrlAutorise.Count > 0)
                    //{
                    //    _ModeCouplageList.Add(6);
                    //}
                }
                _ModeCouplageList.Sort();
                return _ModeCouplageList;
            }
            set
            {
                _ModeCouplageList = value;
            }

        }

        /// <summary>
        /// La collection des MO couplés
        /// </summary>
        public ObservableCollection<MOCouplage> MOCouplage
        {
            get
            {
                if (this._moCouplage == null)
                {
                    this._moCouplage = new ObservableCollection<MOCouplage>();
                }

                return this._moCouplage;
            }
            set
            {
                this._moCouplage = value;
            }
        } // endProperty: MOCouplage

        /// <summary>
        /// La collection des MT couplés
        /// </summary>
        public ObservableCollection<MTCouplage> MTCouplage
        {
            get
            {
                if (this._mtCouplage == null)
                {
                    this._mtCouplage = new ObservableCollection<MTCouplage>();
                }

                return this._mtCouplage;
            }
            set
            {
                this._mtCouplage = value;
            }
        } // endProperty: MTCouplage



        /// <summary>
        /// Mode de libération du couplage
        /// </summary>
        public Int32 ModeLibCouplage
        {
            get
            {
                return this._modeLibCouplage;
            }
            set
            {
                this._modeLibCouplage = value;
            }
        } // endProperty: ModeLibCouplage

        /// <summary>
        /// mode Master / Slave
        /// </summary>
        public Int32 MasterSlave
        {
            get
            {
                return this._masterSlave;
            }
            set
            {
                this._masterSlave = value;
            }
        } // endProperty: MasterSlave

        /// <summary>
        /// calcul masque pont couple
        /// </summary>
        public Int32? CalculDuMasque
        {
            get;
            set;
        } // endProperty: MasterSlave

        /// <summary>
        /// Le nombre de mode solo
        /// </summary>
        public Int32 NbModeSolo
        {
            get
            {
                return this._nbModeSolo;
            }
            set
            {
                this._nbModeSolo = value;
            }
        } // endProperty: NbModeSolo

        /// <summary>
        /// Le nombre de mode solo
        /// </summary>
        public Int32 NbMaxMo
        {
            get;
            set;
        } // endProperty: NbModeSolo
        public Int32 NbModeAssoCouplagePCTRL
        {
            get;
            set;
        } // endProperty: 
        public Int32 NbModeAssoCouplageNB
        {
            get;
            set;
        } // endProperty: 

        /// <summary>
        /// Le masque pour MT1
        /// </summary>
        public UInt32 MasqueRetourMT1
        {
            get
            {
                return this._masqueRetourMT1;
            }
            set
            {
                this._masqueRetourMT1 = value;
            }
        } // endProperty: MasqueRetourMT1

        /// <summary>
        /// La période de scrutation du canal
        /// </summary>
        public Int32 PeriodeEcouteCanal
        {
            get
            {
                return this._periodeEcouteCanal;
            }
            set
            {
                this._periodeEcouteCanal = value;
            }
        } // endProperty: PeriodeEcouteCanal

        public bool PresenceEntree1PandC
            {
                get;set;
            }

        /// <summary>
        /// Le mode de couplage
        /// </summary>
        public Int32 ModeCouplage
        {
            get
            {
                return this._modeCouplage;
            }
            set
            {
                this._modeCouplage = value;
            }
        } // endProperty: ModeCouplage

        private List<int> _listmodePickAndCtrlAutorise;
        public List<int> ListmodePickAndCtrlAutorise
        {
            get
            {
                int ModeInfraRouge = 0;
                if (_listmodePickAndCtrlAutorise != null)
                {
                    _listmodePickAndCtrlAutorise.Clear();
                }
                else
                {
                    _listmodePickAndCtrlAutorise = new List<int>();
                }

                String XValue1 = PegaseData.Instance.GestionInfraRouge.ChoixInfraRouge;
                if (XValue1 != null && XValue1 != "")
                {
                    ModeInfraRouge = Tools.ConvertASCIIToInt32(XValue1);
                }
                else
                {
                    ModeInfraRouge = 0;
                }
                int liaionactive = 0;
                String XValue = PegaseData.Instance.GestionLiaisonFilaire.ChoixLiaisonFilaire; 
                if (XValue != null && XValue != "")
                {
                    liaionactive = Tools.ConvertASCIIToInt32(XValue);
                }
                else
                {
                    liaionactive = 0;
                }
                if ((this.PresenceEntree1PandC) && (liaionactive == 0) && (ModeInfraRouge == 0))
                {
                    _listmodePickAndCtrlAutorise.Add(6);
                }
                if ((liaionactive == 0) && (ModeInfraRouge == 0))
                {
                    _listmodePickAndCtrlAutorise.Add(8);
                }
                _listmodePickAndCtrlAutorise.Sort();
                return _listmodePickAndCtrlAutorise;
            }
            set
            {
                _listmodePickAndCtrlAutorise = value;
            }
        }
        #endregion

        // Constructeur
        #region Constructeur

        public Couplage()
        {
            this.InitCouplage();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser les données de couplage
        /// </summary>
        public void InitCouplage()
        {
            XMLProcessing XProcess = PegaseData.Instance.XMLFile;
            XMLProcessing Xreffile = new XMLProcessing();
            Xreffile.OpenXML(DefaultValues.Get().RefFileName);

            String Value;

            // Le mode de couplage
            this.MTCouplage.Clear();
            this.MOCouplage.Clear();

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/ModeCouplage", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    Int32 tmp = Convert.ToInt32(Value);
                    if (tmp ==6)
                    {
                        this.ModeCouplage = 6;
                        this.NbModeAssoCouplagePCTRL = 6;
                    }
                    else if (tmp == 7)
                    {
                        this.ModeCouplage = 6;
                        this.NbModeAssoCouplagePCTRL = 7;
                    }
                    else if (tmp == 8)
                    {
                        this.NbModeAssoCouplagePCTRL = 8;
                        this.ModeCouplage = 6;
                    }
                    else
                    {
                        this.ModeCouplage = tmp;
                    }

                    
                }
                catch
                {
                    this.ModeCouplage = 0;
                }
            }
            else
            {
                this.ModeCouplage = 0;
            }

            // La période de scrutation du canal

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/PeriodeEcouteCanal", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.PeriodeEcouteCanal = Convert.ToInt32(Value);
                }
                catch
                {
                    this.PeriodeEcouteCanal = 0;
                }
            }
            else
            {
                this.PeriodeEcouteCanal = 0;
            }
            // presence entree 1 sur MT

            
            ObservableCollection<XElement> ListeEntreeTOR = XProcess.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/Configuration/MnemoLogique");
            this.PresenceEntree1PandC = false;
            if (ListeEntreeTOR != null && ListeEntreeTOR.Count > 0)
            {
                foreach (var input in ListeEntreeTOR)
                {try
                    {
                        string test = input.Attribute(XML_ATTRIBUTE.VALUE).Value.ToString();

                        if (test == "ENTREE_TOR_01")
                        {
                            this.PresenceEntree1PandC = true;
                        }
                    }
                    catch
                    { }
                }
            }
            

            // Masque de retour MT1

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/MasqueRetourMT1", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.MasqueRetourMT1 = Convert.ToUInt32(Value);
                }
                catch
                {
                    this.MasqueRetourMT1 = 0xFFFFFFFF;
                }
            }
            else
            {
                this.MasqueRetourMT1 = 0xFFFFFFFF;
            }

            // Nombre de mode solo

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresFixesMO/SectionSelecteurs/ConfigModeExploit/NbModesSolo", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.NbModeSolo = Convert.ToInt32(Value);
                }
                catch
                {
                    this.NbModeSolo = 0;
                }
            }
            else
            {
                this.NbModeSolo = 0;
            }

            // CalculDuMasque

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/CalculDuMasque", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.CalculDuMasque = Convert.ToInt32(Value);
                }
                catch
                {
                    this.CalculDuMasque = 0;
                }
            }
            else
            {
                this.CalculDuMasque = null;
            }

            // Master / Slave

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/Master", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.MasterSlave = Convert.ToInt32(Value);
                }
                catch
                {
                    this.MasterSlave = 0;
                }
            }
            else
            {
                this.MasterSlave = 0;
            }

            // Mode de libération du couplage

            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/ModeLibCouplage", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.ModeLibCouplage = Convert.ToInt32(Value);
                }
                catch
                {

                    this.ModeLibCouplage = 0;
                }
            }
            else
            {
                this.ModeLibCouplage = 0;
            }
            // nb mo max
            Value = XProcess.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/NbMaxMO", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.NbMaxMo = Convert.ToInt32(Value);
                }
                catch
                {

                    this.NbMaxMo = 0;
                }
            }
            else
            {
                this.NbMaxMo = 0;
            }
            Value = XProcess.GetValue("XmlMetier/HorsMode/CouplageMetier/NbCouplagePick/", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null)
            {
                try
                {
                    this.NbModeAssoCouplageNB = Convert.ToInt32(Value);
                }
                catch
                {

                    this.NbModeAssoCouplageNB = 0;
                }
            }
            else
            {
                this.NbModeAssoCouplageNB = 0;
            }

            // Liste des Codes natifs MT
            ObservableCollection<XElement> ListCodeNatifMT = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif/CodeNatifMT");
            ObservableCollection<XElement> ListCanalRadioMT = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal/CanalRadioMT");
            ObservableCollection<MTCouplage> MTsCouplage = new ObservableCollection<MTCouplage>();
            if (ListCodeNatifMT == null)
            {
                ListCodeNatifMT = Xreffile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif/CodeNatifMT");
            }
            if (ListCanalRadioMT == null)
            {
                ListCanalRadioMT = Xreffile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal/CanalRadioMT");
            }

            if (ListCodeNatifMT != null && ListCanalRadioMT != null)
            {
                if (ListCanalRadioMT.Count == ListCodeNatifMT.Count)
                {
                    for (int i = 0; i < ListCanalRadioMT.Count; i++)
                    {
                        String CodeMTNatif = ListCodeNatifMT[i].Attribute(XML_ATTRIBUTE.VALUE).Value;
                        Value = ListCanalRadioMT[i].Attribute(XML_ATTRIBUTE.VALUE).Value;
                        Int32 NumCanal;

                        try
                        {
                            NumCanal = Convert.ToInt32(Value);
                        }
                        catch
                        {
                            NumCanal = 1;
                        }

                        if (CodeMTNatif.Length > 2)
                        {
                            if (CodeMTNatif.Contains("0x"))
                            {
                                CodeMTNatif = CodeMTNatif.Substring(2);
                            }
                        }
                        else
                        {
                            CodeMTNatif = "00000";
                        }

                        MTCouplage MTC = new MTCouplage(CodeMTNatif, NumCanal);
                        if (CodeMTNatif == "0")
                        {
                            MTC.IsVisible = Visibility.Visible;
                            MTC.IsEnable = false;
                            MTsCouplage.Add(MTC);
                            break;
                        }
                        else
                        {
                            MTC.IsVisible = Visibility.Visible;
                            MTC.IsEnable = true;
                        }

                        MTsCouplage.Add(MTC);
                    }
                    this.MTCouplage = MTsCouplage;
                } 
            }

            // Lire la table des MO (ID Natif + canaux radio)
            ObservableCollection<XElement> ListeCodeNatifMO = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableIDNatif/CodeNatifMO");
            ObservableCollection<XElement> ListeCanalMO = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableCanal/CanalRadioMO");
            ObservableCollection<MOCouplage> MOsCouplage = new ObservableCollection<MOCouplage>();

            if (ListeCodeNatifMO != null && ListeCanalMO != null)
            {
                if (ListeCodeNatifMO.Count == ListeCanalMO.Count)
                {
                    for (int i = 0; i < ListeCodeNatifMO.Count; i++)
                    {
                        String CodeMONatif = ListeCodeNatifMO[i].Attribute(XML_ATTRIBUTE.VALUE).Value;
                        Value = ListeCanalMO[i].Attribute(XML_ATTRIBUTE.VALUE).Value;
                        Int32 NumCanal;

                        try
                        {
                            NumCanal = Convert.ToInt32(Value);
                        }
                        catch
                        {
                            NumCanal = 1;
                        }

                        if (CodeMONatif.Length > 2)
                        {
                            if (CodeMONatif.Contains("0x"))
                            {
                                CodeMONatif = CodeMONatif.Substring(2);
                            }
                        }
                        else
                        {
                            CodeMONatif = "00000";
                        }

                        MOCouplage MOC = new MOCouplage(CodeMONatif, NumCanal);
                        if (CodeMONatif == "0")
                        {
                            MOC.IsVisible = Visibility.Visible;
                            MOC.IsEnable = false;
                            MOsCouplage.Add(MOC);
                            break;
                        }
                        else
                        {
                            MOC.IsVisible = Visibility.Visible;
                            MOC.IsEnable = true;
                        }

                        MOsCouplage.Add(MOC);
                    }
                    this.MOCouplage = MOsCouplage;
                } 
            }
        } // endMethod: InitCouplage
        private List<int> _listCmdSynchrAutorise;
        public List<int> ListCmdSynchrAutorise
        {
            get
            {
                if (_listCmdSynchrAutorise != null)
                {
                    _listCmdSynchrAutorise.Clear();
                }
                else
                {
                    _listCmdSynchrAutorise = new List<int>();
                }

                int nbmodes = PegaseData.Instance.OLogiciels.NbModes;
                _listCmdSynchrAutorise.Add(0);
                if (nbmodes <=1)
                {
                     for( int i = 2; i<33; i++)
                    {
                        _listCmdSynchrAutorise.Add(i);
                    }
                }
                _listCmdSynchrAutorise.Sort();
                return _listCmdSynchrAutorise;
            }
            set
            {
                _listCmdSynchrAutorise = value;
            }
        }
        /// <summary>
        /// Enregistrer les données de couplage
        /// </summary>
        public void Save ( )
        {
            XMLProcessing XProcess = PegaseData.Instance.XMLFile;
            XMLProcessing Xreffile = new XMLProcessing();
            Xreffile.OpenXML(DefaultValues.Get().RefFileName);
            string tmp = "";
            // Le mode de couplage
            if ((this.ModeCouplage == 6) && (this.NbModeAssoCouplagePCTRL ==6))
            {
                tmp = "6";
            }
            else if ((this.ModeCouplage == 6) && (this.NbModeAssoCouplagePCTRL == 7))
            {
                tmp = "7";
            }
            else if ((this.ModeCouplage == 6)  && (this.NbModeAssoCouplagePCTRL == 8))
            {
                tmp = "8";
            }
            else
            {
                tmp = this.ModeCouplage.ToString();
            }
            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/ModeCouplage", "", "", XML_ATTRIBUTE.VALUE, tmp);

            // La période de scrutation du canal

            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/PeriodeEcouteCanal", "", "", XML_ATTRIBUTE.VALUE, this.PeriodeEcouteCanal.ToString());

            // Masque de retour MT1

            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/MasqueRetourMT1", "", "", XML_ATTRIBUTE.VALUE, this.MasqueRetourMT1.ToString());

            // Nombre de mode solo

            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresFixesMO/SectionSelecteurs/ConfigModeExploit/NbModesSolo", "", "", XML_ATTRIBUTE.VALUE, this.NbModeSolo.ToString());

            // Master / Slave
            
                XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/Master", "", "", XML_ATTRIBUTE.VALUE, this.MasterSlave.ToString());

            // masque retour 
            if (this.CalculDuMasque != null)
            {
                XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/CalculDuMasque", "", "", XML_ATTRIBUTE.VALUE, this.CalculDuMasque.ToString());
            }


            // Mode de libération du couplage

            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/ModeLibCouplage", "", "", XML_ATTRIBUTE.VALUE, this.ModeLibCouplage.ToString());

            //NbMaxMo
            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/NbMaxMO", "", "", XML_ATTRIBUTE.VALUE, this.NbMaxMo.ToString());
            XProcess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/NbMaxMO", "", "", XML_ATTRIBUTE.TYPE, "uint8_t");
            try
            {
                // Liste des Codes natifs MT
                XElement ListCodeNatifMTRoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif/").First();
                XElement ListCanalRadioMTRoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal/").First();
                if (ListCodeNatifMTRoot.Descendants("Variable").Count() == 0)
                {
                    ListCodeNatifMTRoot = Xreffile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif/").First();
                }
                if (ListCanalRadioMTRoot.Descendants("Variable").Count() == 0)
                {
                    ListCanalRadioMTRoot = Xreffile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal/").First();
                }
                ObservableCollection<MTCouplage> MTsCouplage = new ObservableCollection<MTCouplage>();
                XElement CodeNatifMTRef = ListCodeNatifMTRoot.Descendants("Variable").First();
                XElement CanalMTRef = ListCanalRadioMTRoot.Descendants("Variable").First();

                Int32 TailleCodeNatifMT = Convert.ToInt32(CodeNatifMTRef.Attribute(XML_ATTRIBUTE.TAILLE).Value);
                Int32 TailleCanalMT = Convert.ToInt32(CanalMTRef.Attribute(XML_ATTRIBUTE.TAILLE).Value);
                Int32 AdrAbsoluCodeNatifMT = Convert.ToInt32(CodeNatifMTRef.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value);
                Int32 AdrAbsoluCanalMT = Convert.ToInt32(CanalMTRef.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value);
                Int32 AdrRelatifCodeNatifMT = Convert.ToInt32(CodeNatifMTRef.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value);
                Int32 AdrRelatifCanalMT = Convert.ToInt32(CanalMTRef.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value);
                 ListCodeNatifMTRoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif/").First();
                 ListCanalRadioMTRoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal/").First();

                if (this.MTCouplage.Count > 0)
                {
                    ListCodeNatifMTRoot.Descendants().Remove();
                    ListCanalRadioMTRoot.Descendants().Remove();

                    for (int i = 0; i < this.MTCouplage.Count; i++)
                    {
                        XElement newCodeNatifMT = this.DuppliqueLine(CodeNatifMTRef, ref AdrAbsoluCodeNatifMT, ref AdrRelatifCodeNatifMT, TailleCodeNatifMT, "0x" + this.MTCouplage[i].CodeNatifMT);
                        XElement newCanalMT = this.DuppliqueLine(CanalMTRef, ref AdrAbsoluCanalMT, ref AdrRelatifCanalMT, TailleCanalMT, this.MTCouplage[i].CanalRadioMT.ToString());

                        ListCodeNatifMTRoot.Add(newCodeNatifMT);
                        ListCanalRadioMTRoot.Add(newCanalMT);
                    }
                }

                // ajout pour gestion du pitch and catch 2.0
                try
                {
                    XElement BaliseCouplage = XProcess.GetNodeByPath("XmlMetier/HorsMode/CouplageMetier/").First();
                    if (BaliseCouplage != null)
                    {
                        XElement XNbModeEnCouplagePCTRL = ListCodeNatifMTRoot.Descendants("Variable").First();
                        XProcess.SetValue("XmlMetier/HorsMode/CouplageMetier/NbCouplagePick", "", "", XML_ATTRIBUTE.VALUE, this.NbModeAssoCouplageNB.ToString());
                    }
                    else
                    {

                    }
                }
                catch
                {
                    XElement BaliseCouplage = new XElement("SousBloc");
                    XAttribute code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "CouplageMetier");
                    BaliseCouplage.Add(code);
                    XAttribute descript = new XAttribute(XMLCore.XML_ATTRIBUTE.DESCRIPTION, "Descript_CouplageMetier");
                    BaliseCouplage.Add(descript);
                    XAttribute offsetabsolu = new XAttribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU, "-1");
                    BaliseCouplage.Add(offsetabsolu);
                    XAttribute offsetrelatif = new XAttribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF, "-1");
                    BaliseCouplage.Add(offsetrelatif);
                    XAttribute num = new XAttribute(XMLCore.XML_ATTRIBUTE.NUM, "-2");
                    BaliseCouplage.Add(num);
                    XElement NbCouplage = new XElement("Variable");
                    XAttribute code1 = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "NbCouplagePick");
                    NbCouplage.Add(code1);
                    XAttribute descript1 = new XAttribute(XMLCore.XML_ATTRIBUTE.DESCRIPTION, "Descript_NbCouplagePick");
                    NbCouplage.Add(descript1);
                    XAttribute offsetabsolu1 = new XAttribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU, "-1");
                    NbCouplage.Add(offsetabsolu1);
                    XAttribute offsetrelatif1 = new XAttribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF, "-1");
                    NbCouplage.Add(offsetrelatif1);
                    XAttribute num1 = new XAttribute(XMLCore.XML_ATTRIBUTE.VALUE, NbModeAssoCouplagePCTRL);
                    NbCouplage.Add(num1);
                    BaliseCouplage.Add(NbCouplage);
                    XElement balisehorsmode = XProcess.GetNodeByPath("XmlMetier/HorsMode/").First();
                    balisehorsmode.Add(BaliseCouplage);
                }
            }
            catch
            {
                
            }

            try
            {
                // Liste des codes natifs MO
                // Initialisation
                XElement ListCodeNatifMORoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableIDNatif/").First();
                XElement ListCanalMORoot = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableCanal/").First();
                XElement CodeNatifMORef = ListCodeNatifMORoot.Descendants("Variable").First();
                XElement CanalMORef = ListCanalMORoot.Descendants("Variable").First();
                Int32 TailleCodeNatifMO = Convert.ToInt32(CodeNatifMORef.Attribute(XML_ATTRIBUTE.TAILLE).Value);
                Int32 TailleCanalMO = Convert.ToInt32(CanalMORef.Attribute(XML_ATTRIBUTE.TAILLE).Value);
                Int32 AdrAbsoluCodeNatifMO = Convert.ToInt32(CodeNatifMORef.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value);
                Int32 AdrAbsoluCanalMO = Convert.ToInt32(CanalMORef.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value);
                Int32 AdrRelatifCodeNatifMO = Convert.ToInt32(CodeNatifMORef.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value);
                Int32 AdrRelatifCanalMO = Convert.ToInt32(CanalMORef.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value);

                // Vider la liste
                if (this.MOCouplage.Count > 0)
                {
                    ListCodeNatifMORoot.Descendants().Remove();
                    ListCanalMORoot.Descendants().Remove();

                    for (int i = 0; i < this.MOCouplage.Count; i++)
                    {
                        XElement newCodeNatifMO = this.DuppliqueLine(CodeNatifMORef, ref AdrAbsoluCodeNatifMO, ref AdrRelatifCodeNatifMO, TailleCodeNatifMO, "0x" + this.MOCouplage[i].CodeNatifMO);
                        XElement newCanalMO = this.DuppliqueLine(CanalMORef, ref AdrAbsoluCanalMO, ref AdrRelatifCanalMO, TailleCanalMO, this.MOCouplage[i].CanalRadioMO.ToString());

                        ListCodeNatifMORoot.Add(newCodeNatifMO);
                        ListCanalMORoot.Add(newCanalMO);
                    }
                }
            }
            catch
            {

            }
        } // endMethod: Save

        /// <summary>
        /// Duppliquer le XElement fourni en incrémentant les offsets absolus et relatifs spécifiés
        /// </summary>
        public XElement DuppliqueLine( XElement Master, ref Int32 OffsetAbsolu, ref Int32 OffsetRelatif, Int32 Taille, String Value )
        {
            XElement Result;

            Result = new XElement(Master);
            Result.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value = OffsetAbsolu.ToString();
            Result.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value = OffsetRelatif.ToString();
            Result.Attribute(XML_ATTRIBUTE.VALUE).Value = Value;

            OffsetAbsolu += Taille;
            OffsetRelatif += Taille;

            return Result;
        } // endMethod: DuppliqueLine

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Couplage	
}