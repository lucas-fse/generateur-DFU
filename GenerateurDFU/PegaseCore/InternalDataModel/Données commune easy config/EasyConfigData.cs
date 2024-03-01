using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using JAY.PegaseCore.InternalDataModel;


namespace JAY.PegaseCore
{
    /// <summary>
    /// L'ensemble des données 'Easy config', éditées par le plugin du même nom
    /// et partagées par le plugin 'Rapport'
    /// </summary>
    /// 
    public enum TypeBusEasyConfig
    {
        AUCUN = 0,
        MODBUS,
        CANOPEN,
        PROFIBUSV1,
        DEVICENET,
        ETHERCAT,
        PROFINET,
        ETHRNETIP,
        POWERLINK,
        CCLINK
    }
    public class EasyConfigData : Mvvm.ViewModelBase
    {
        // Variables singleton
        private static EasyConfigData _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private ObservableCollection<EC_Mode> _modes;
        private ObservableCollection<EC_Colonne> _colonnes;
        private ObservableCollection<EC_Colonne> _colonnesBtn;
        private ObservableCollection<EC_VarNetwork> _varNetworks;
        private ObservableCollection<VariableE> _listVarBoolOutput;
        private ObservableCollection<VariableE> _listVarBoolInput;
        private ObservableCollection<VariableE> _listVarNumOutput;
        private ObservableCollection<VariableE> _listVarNumInput;

        private ObservableCollection<ComplementaryData> _maskmodes;


        private UInt32 _offsetmodbus; // offset du modbus
        private UInt32 _canopennodeid; // nodeid canopen TPDO
        private UInt32 _powerlinknodenumber;
        private TypeBusEasyConfig _typebuseasyconfig;

        #endregion

        // Propriétés
        #region Propriétés

        public ObservableCollection<VariableE> ListVarBoolOutputInit
        {
            set
            {
                this._listVarBoolOutput = null;
            }
        }
        /// <summary>
        /// Variables booléennes utilisables sur le système (sorties)
        /// </summary>
        public ObservableCollection<VariableE> ListVarBoolOutput
        {
            get
            {
                if (this._listVarBoolOutput == null && PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.STORS != null)
                {
                    this._listVarBoolOutput = new ObservableCollection<VariableE>();

                    VariableE v = new VariableE("", "----", typeof(Boolean), "");
                    this._listVarBoolOutput.Add(v);

                    // Ajouter les relais, sorties TOR
                    var QueryRelais = from relais in PegaseData.Instance.ModuleT.STORS
                                      where (relais.MnemoLogique.Contains("RELAI") || relais.MnemoLogique.Contains("SORTIE_TOR")) && relais.IsPWM == false
                                      select relais;

                    if (QueryRelais.Count() > 0)
                    {
                        foreach (var relais in QueryRelais)
                        {
                            VariableE va = new VariableE(relais.MnemoLogique, relais.MnemoClient, typeof(Boolean), "I");
                            this._listVarBoolOutput.Add(va);
                        }
                    }

                    // Ajouter les entrées TOR
                    if (PegaseData.Instance.ModuleT != null)
                    {
                        foreach (var etor in PegaseData.Instance.ModuleT.ETORS)
                        {
                            VariableE va = new VariableE(etor.MnemoLogique, etor.MnemoClient, typeof(Boolean), "O");
                            this._listVarBoolOutput.Add(va);
                        }
                    }

                    // Ajouter les boutons
                    if (PegaseData.Instance.MOperateur.OrganesCommandes != null)
                    {
                        if (this.Modes.Count > 0)// && this.Modes[0].OrganAnaUsed.Count > 0)
                        {
                            var QueryButton = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                                              where organ.MnemoHardFamilleMO != "AXE"
                                              select organ;

                            foreach (OrganCommand organ in QueryButton)
                            {
                                String Mnemo;
                                String Affichage;
                                VariableE va;
                                String Etat = "";

                                if (organ.MnemoHardFamilleMO == "BT")
                                {
                                    // Bouton
                                    for (int i = 0; i < organ.NbPosOrgane; i++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                Etat = "PAS_D_APPUI";
                                                break;
                                            case 1:
                                                Etat = "APPUI_SIMPLE";
                                                break;
                                            case 2:
                                                Etat = "APPUI_DOUBLE";
                                                break;
                                            default:
                                                break;
                                        }

                                        Mnemo = String.Format("{0} == {1} ? VRAI : FAUX", organ.Mnemologique, Etat);
                                        Affichage = String.Format("{0} - Pos{1:00} : {2}", organ.NomOrganeMO, i, organ.MnemoClient);
                                        va = new VariableE(Mnemo, Affichage, typeof(Boolean), "I");
                                        this._listVarBoolOutput.Add(va);
                                    }
                                }
                                else if ((organ.MnemoHardFamilleMO == "AC") && (organ.NbPosOrgane < 15 && organ.NbPosOrgane > 6))
                                {
                                    // Axe à cran
                                    // Limiter au cran -4 / +4
                                    // for (int i = 0; i < organ.NbPosOrgane; i++)
                                    for (int i = 2; i < organ.NbPosOrgane - 2; i++)
                                    {
                                        switch (i + (4 - organ.NbCran))
                                        {
                                            case 0:
                                                Etat = "CRAN_MOINS_SIX";
                                                break;
                                            case 1:
                                                Etat = "CRAN_MOINS_CINQ";
                                                break;
                                            case 2:
                                                Etat = "CRAN_MOINS_QUATRE";
                                                break;
                                            case 3:
                                                Etat = "CRAN_MOINS_TROIS";
                                                break;
                                            case 4:
                                                Etat = "CRAN_MOINS_DEUX";
                                                break;
                                            case 5:
                                                Etat = "CRAN_MOINS_UN";
                                                break;
                                            case 6:
                                                Etat = "CRAN_REPOS";
                                                break;
                                            case 7:
                                                Etat = "CRAN_PLUS_UN";
                                                break;
                                            case 8:
                                                Etat = "CRAN_PLUS_DEUX";
                                                break;
                                            case 9:
                                                Etat = "CRAN_PLUS_TROIS";
                                                break;
                                            case 10:
                                                Etat = "CRAN_PLUS_QUATRE";
                                                break;
                                            case 11:
                                                Etat = "CRAN_PLUS_CINQ";
                                                break;
                                            case 12:
                                                Etat = "CRAN_PLUS_SIX";
                                                break;
                                            default:
                                                Etat = "";
                                                break;
                                        }
                                        Mnemo = String.Format("{0} == {1} ? VRAI : FAUX", organ.Mnemologique, Etat);
                                        Affichage = String.Format("{0} - Pos{1:00} : {2}", organ.NomOrganeMO, i + (4 - organ.NbCran) - 6, organ.MnemoClient);
                                        va = new VariableE(Mnemo, Affichage, typeof(Boolean), "I");
                                        this._listVarBoolOutput.Add(va);
                                    }
                                }
                                else if (organ.MnemoHardFamilleMO == "CO" && !organ.NomOrganeMO.Contains("Seln"))
                                {
                                    if ((organ.NbPosOrgane >= 2) && (organ.ReferenceOrgan.Equals("D30400A")))
                                    {
                                        // Commutateur 12
                                        for (int i = 0; i < organ.NbPosOrgane; i++)
                                        {
                                            Mnemo = String.Format("{0} == POSITION_{1:00} ? VRAI : FAUX", organ.Mnemologique, i);
                                            Affichage = String.Format("{0} - Pos{1:00} : {2}", organ.NomOrganeMO, i + 1, organ.MnemoClient);
                                            va = new VariableE(Mnemo, Affichage, typeof(Boolean), "I");
                                            this._listVarBoolOutput.Add(va);
                                        }
                                    }
                                    else
                                    {
                                        // Commutateur 2 et 3 position
                                        if (organ.NbPosOrgane == 2)
                                        {
                                            for (int i = 0; i < organ.NbPosOrgane; i++)
                                            {
                                                switch (i)
                                                {
                                                    case 0:
                                                        Etat = "POSITION_1";
                                                        break;
                                                    case 1:
                                                        Etat = "POSITION_2";
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                Mnemo = String.Format("{0} == {1} ? VRAI : FAUX", organ.Mnemologique, Etat);
                                                Affichage = String.Format("{0} - Pos{1:00} : {2}", organ.NomOrganeMO, i + 1, organ.MnemoClient);
                                                va = new VariableE(Mnemo, Affichage, typeof(Boolean), "I");
                                                this._listVarBoolOutput.Add(va);
                                            }
                                        }
                                        else if (organ.NbPosOrgane == 3)
                                        {
                                            for (int i = 0; i < organ.NbPosOrgane; i++)
                                            {
                                                switch (i)
                                                {
                                                    case 0:
                                                        Etat = "POSITION_0";
                                                        break;
                                                    case 1:
                                                        Etat = "POSITION_1";
                                                        break;
                                                    case 2:
                                                        Etat = "POSITION_2";
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                Mnemo = String.Format("{0} == {1} ? VRAI : FAUX", organ.Mnemologique, Etat);
                                                Affichage = String.Format("{0} - Pos{1:00} : {2}", organ.NomOrganeMO, i, organ.MnemoClient);
                                                va = new VariableE(Mnemo, Affichage, typeof(Boolean), "I");
                                                this._listVarBoolOutput.Add(va);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Ajouter les variables internes et systèmes
                    var Query = from variable in PegaseData.Instance.Variables
                                where variable.IO == "" && variable.VarType == typeof(Boolean)
                                select variable;

                    if (Query.Count() > 0)
                    {
                        foreach (var variable in Query)
                        {
                            this._listVarBoolOutput.Add(variable);
                        }
                    }
                    else
                    {
                        this._listVarBoolOutput = null;
                    }
                }

                return this._listVarBoolOutput;
            }
            set
            {
                this._listVarBoolOutput = value;
            }
        } // endProperty: ListVarBoolOutput

        /// <summary>
        /// Variable booléennes utilisables sur le système (entrées)
        /// </summary>
        public ObservableCollection<VariableE> ListVarBoolInput
        {
            get
            {
                if (this._listVarBoolInput == null && PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.STORS != null)
                {
                    this._listVarBoolInput = new ObservableCollection<VariableE>();
                    // Ajout des relais
                    VariableE v = new VariableE("", "----", typeof(Boolean), "");
                    this._listVarBoolInput.Add(v);

                    var QueryRelais = from relais in PegaseData.Instance.ModuleT.STORS
                                      where (relais.MnemoLogique.Contains("RELAI")) && relais.IsPWM == false
                                      select relais;

                    if (QueryRelais.Count() > 0)
                    {
                        foreach (var relais in QueryRelais)
                        {
                            VariableE va = new VariableE(relais.MnemoLogique, relais.MnemoClient, typeof(Boolean), "I");
                            this._listVarBoolInput.Add(va);
                        }
                    }

                    // Ajout des sorties TOR
                    var QueryTOR = from STOR in PegaseData.Instance.ModuleT.STORS
                                   where !STOR.MnemoLogique.Contains("RELAI") && STOR.IsPWM == false
                                   select STOR;

                    if (QueryTOR.Count() > 0)
                    {
                        foreach (var STOR in QueryTOR)
                        {
                            VariableE va = new VariableE(STOR.MnemoLogique, STOR.MnemoClient, typeof(Boolean), "I");
                            this._listVarBoolInput.Add(va);
                        }
                    }

                    // Ajout des variables

                    var QueryVar = from variable in this.ListVarBoolOutput
                                   where variable.Name.Contains("VARIABLE")
                                   select variable;

                    if (QueryVar.Count() > 0)
                    {
                        foreach (var variable in QueryVar)
                        {
                            this._listVarBoolInput.Add(variable);
                        }
                    }

                }

                return this._listVarBoolInput;
            }
        } // endProperty: ListVarBoolInput

        public ObservableCollection<VariableE> ListVarNumOutputInit
        {
            set
            {
                this._listVarNumOutput = null;
            }
        }
        /// <summary>
        /// Variables numériques utilisables sur le système
        /// </summary>
        public ObservableCollection<VariableE> ListVarNumOutput
        {
            get
            {
                if (this._listVarNumOutput == null && PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.EAnas != null)
                {
                    this._listVarNumOutput = new ObservableCollection<VariableE>();

                    // Ligne 'variable vide'
                    VariableE v = new VariableE("", "----", typeof(Double), "");
                    this._listVarNumOutput.Add(v);

                    // axe analogique

                    foreach (var organ in PegaseData.Instance.MOperateur.OrganesCommandes)
                    {
                        if (organ.MnemoHardFamilleMO == "AXE")
                        {
                            v = new VariableE(organ.Mnemologique, organ.MnemoClient, typeof(Double), "o", organ.MnemoHardFamilleMO);
                            
                            this._listVarNumOutput.Add(v);
                        }
                        if (organ.MnemoHardFamilleMO == "AC")
                        {
                            v = new VariableE(organ.Mnemologique, organ.MnemoClient, typeof(Double), "o", organ.MnemoHardFamilleMO);
                            this._listVarNumOutput.Add(v);
                        }
                    }

                    // entrée analogique
                    foreach (var eana in PegaseData.Instance.ModuleT.EAnas)
                    {
                        VariableE va = new VariableE(eana.MnemoLogique, eana.MnemoClient, typeof(Double), "I");
                        this._listVarNumOutput.Add(va);
                    }

                    // sortie analogique
                    foreach (var sana in PegaseData.Instance.ModuleT.SAnas)
                    {
                        VariableE vsa = new VariableE(sana.MnemoLogique, sana.MnemoClient, typeof(Double), "O");
                        this._listVarNumOutput.Add(vsa);
                    }

                    // PWM
                    var QueryRelais = from relais in PegaseData.Instance.ModuleT.STORS
                                      where (relais.MnemoLogique.Contains("RELAI") || relais.MnemoLogique.Contains("SORTIE_TOR")) && relais.IsPWM == true
                                      select relais;

                    if (QueryRelais.Count() > 0)
                    {
                        foreach (var relais in QueryRelais)
                        {
                            VariableE va = new VariableE(relais.MnemoLogique, relais.MnemoClient, typeof(Boolean), "I");
                            this._listVarNumOutput.Add(va);
                        }
                    }

                    // selecteur
                    for (int i = 0; i < Constantes.MAX_SELECTEUR; i++)
                    {
                        VariableE vaSe = new VariableE(String.Format("SELECTEUR_{0:00}", i + 1), String.Format("SELECTOR_{0:00}", i + 1), typeof(Double), "O");
                        this._listVarNumOutput.Add(vaSe);
                    }

                    // commutateur 12 positions
                    // Ajouter les boutons
                    if (PegaseData.Instance.MOperateur.OrganesCommandes != null)
                    {
                        if (this.Modes.Count > 0)// && this.Modes[0].OrganAnaUsed.Count > 0)
                        {
                            var QueryButton = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                                              where organ.MnemoHardFamilleMO != "AXE"
                                              select organ;

                            foreach (OrganCommand organ in QueryButton)
                            {
                                if (organ.MnemoHardFamilleMO == "CO" && !organ.NomOrganeMO.Contains("Seln"))
                                {
                                    if ((organ.NbPosOrgane >= 2) && (organ.ReferenceOrgan.Equals("D30400A")))
                                    {
                                            VariableE va = new VariableE(String.Format("{0}", organ.Mnemologique), String.Format("{0}", organ.Mnemologique), typeof(Boolean), "O");
                                            this._listVarNumOutput.Add(va);
                                    }
                                }
                            }
                        }
                    }

                    // variable interne
                    var Query = from variable in PegaseData.Instance.Variables
                                where variable.IO == "" && variable.VarType == typeof(Double)
                                select variable;

                    if (Query.Count() > 0)
                    {
                        foreach (var variable in Query)
                        {
                            this._listVarNumOutput.Add(variable);
                        }
                    }
                    else
                    {
                        this._listVarNumOutput = null;
                    }
                }

                return this._listVarNumOutput;
            }
        } // endProperty: VarNum

        /// <summary>
        /// Variables numériques utilisables sur le système
        /// </summary>
        public ObservableCollection<VariableE> ListVarNumInput
        {
            get
            {
                if (this._listVarNumInput == null && PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.SAnas != null)
                {
                    this._listVarNumInput = new ObservableCollection<VariableE>();

                    // Ligne vide
                    VariableE v = new VariableE("", "----", typeof(Double), "");
                    this._listVarNumInput.Add(v);

                    // sortie analogique
                    foreach (var sana in PegaseData.Instance.ModuleT.SAnas)
                    {
                        VariableE va = new VariableE(sana.MnemoLogique, sana.MnemoClient, typeof(Double), "O");
                        this._listVarNumInput.Add(va);
                    }

                    // PWM
                    var QueryRelais = from relais in PegaseData.Instance.ModuleT.STORS
                                      where (relais.MnemoLogique.Contains("RELAI") || relais.MnemoLogique.Contains("SORTIE_TOR")) && relais.IsPWM == true
                                      select relais;

                    if (QueryRelais.Count() > 0)
                    {
                        foreach (var relais in QueryRelais)
                        {
                            VariableE va = new VariableE(relais.MnemoLogique, relais.MnemoClient, typeof(Boolean), "I");
                            this._listVarNumInput.Add(va);
                        }
                    }

                    // variable interne
                    var Query = from variable in this.ListVarNumOutput
                                where variable.Name.Contains("VARIABLE")
                                select variable;

                    if (Query.Count() > 0)
                    {
                        foreach (var variable in Query)
                        {
                            this._listVarNumInput.Add(variable);
                        }
                    }
                    else
                    {
                        this._listVarNumInput = null;
                    }
                }

                return this._listVarNumInput;
            }
        } // endProperty: ListVarNumInput

        /// <summary>
        /// Les variables réseau
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarNetworks
        {
            get
            {
                if (this._varNetworks == null)
                {
                    this._varNetworks = new ObservableCollection<EC_VarNetwork>();
                    this.InitDataSystem();
                    this.InitBoolInput();
                    this.InitBoolOutput();
                    this.InitNumericalInput();
                    this.InitNumericalOutput();
                }

                return this._varNetworks;
            }
            set
            {
                this._varNetworks = value;
                RaisePropertyChanged("VarNetworks");
            }
        } // endProperty: VarNetworks

        
            /// <summary>
        /// La liste des variables fixe
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarDataSys
        {
            get
            {
                ObservableCollection<EC_VarNetwork> Result = new ObservableCollection<EC_VarNetwork>();

                var Query = from variable in this.VarNetworks
                            where variable.Type == typeVarNetwork.DataSystem
                            select variable;

                if (Query.Count() > 0)
                {
                    foreach (var variable in Query)
                    {
                        Result.Add(variable);
                    }
                }

                return Result;
            }
        } // endProperty: VarIBool


        /// <summary>
        /// La liste des variables booléennes
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarIBool
        {
            get
            {
                ObservableCollection<EC_VarNetwork> Result = new ObservableCollection<EC_VarNetwork>();

                var Query = from variable in this.VarNetworks
                            where variable.Type == typeVarNetwork.IBoolNetwork
                            select variable;

                if (Query.Count() > 0)
                {
                    foreach (var variable in Query)
                    {
                        Result.Add(variable);
                    }
                }
                return Result;
            }
        } // endProperty: VarIBool

        /// <summary>
        /// La liste des variables booléennes
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarOBool
        {
            get
            {
                ObservableCollection<EC_VarNetwork> Result = new ObservableCollection<EC_VarNetwork>();

                var Query = from variable in this.VarNetworks
                            where variable.Type == typeVarNetwork.OBoolNetwork
                            select variable;

                if (Query.Count() > 0)
                {
                    foreach (var variable in Query)
                    {
                        Result.Add(variable);
                    }
                }

                return Result;
            }
        } // endProperty: VarOBool

        public void CloseData()
        {
            this._colonnes = null;
            this._colonnesBtn = null;
            //EasyConfigData._instance = null;
            this._modes = null;
            this._colonnes = null;
            this._colonnesBtn = null;
            this._varNetworks = null;
            this._listVarBoolOutput = null;
            this._listVarBoolInput = null;
            this._listVarNumOutput = null;
            this._listVarNumInput = null;
            
    }

        /// <summary>
        /// La liste des variables numériques
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarINum
        {
            get
            {
                ObservableCollection<EC_VarNetwork> Result = new ObservableCollection<EC_VarNetwork>();

                var Query = from variable in this.VarNetworks
                            where variable.Type == typeVarNetwork.INumNetwork
                            select variable;

                if (Query.Count() > 0)
                {
                    foreach (var variable in Query)
                    {
                        Result.Add(variable);
                    }
                }

                return Result;
            }
        } // endProperty: VarINum

        /// <summary>
        /// La liste des variables numériques
        /// </summary>
        public ObservableCollection<EC_VarNetwork> VarONum
        {
            get
            {
                ObservableCollection<EC_VarNetwork> Result = new ObservableCollection<EC_VarNetwork>();

                var Query = from variable in this.VarNetworks
                            where variable.Type == typeVarNetwork.ONumNetwork
                            select variable;

                if (Query.Count() > 0)
                {
                    foreach (var variable in Query)
                    {
                        Result.Add(variable);
                    }
                }

                return Result;
            }
        } // endProperty: VarONum



        /// <summary>
        /// La liste des modes surchargés 
        /// </summary>
        public ObservableCollection<EC_Mode> Modes
        {
            get
            {
                if (this._modes == null)
                {
                    if (IsInDesignMode)
                    {
                        this.InitDesignMode();
                    }
                    else
                    {
                        this.InitModes();
                    }
                    
                }
                return this._modes;
            }
            set
            {
                this._modes = value;
                RaisePropertyChanged("Modes");
            }
        } // endProperty: Modes

        /// <summary>
        /// La liste des modes surchargés 
        /// </summary>
        public ObservableCollection<ComplementaryData> MaskModes
        {
            get
            {
                if (this._maskmodes == null)
                {
                    this.InitMaskModes();
                }

                return this._maskmodes;
            }
            set
            {
                this._maskmodes = value;
                RaisePropertyChanged("MaskModes");
            }
        } // endProperty: Modes
        /// <summary>
        /// La légende des colonnes
        /// </summary>
        public ObservableCollection<EC_Colonne> Colonnes
        {
            get
            {
                if (this._colonnes == null)
                {
                    if (IsInDesignMode)
                    {
                        this.InitDesignColonnes();
                    }
                    else
                    {
                        this.InitColonnes();
                    }
                }
                return this._colonnes;
            }
            set
            {
                this._colonnes = value;
                RaisePropertyChanged("Colonnes");
            }
        } // endProperty: Colonnes

        /// <summary>
        /// Les colonnes décrivant des boutons pour les interverrouillages boutons
        /// </summary>
        public ObservableCollection<EC_Colonne> ColonnesBtn
        {
            get
            {
                if (this._colonnesBtn == null)
                {
                    this.InitColonnesBtn();
                }

                return this._colonnesBtn;
            }
            set
            {
                this._colonnesBtn = value;
                RaisePropertyChanged("ColonnesBtn");
            }
        } // endProperty: ColonnesBtn

        #endregion

        // Constructeur
        #region Constructeur

        public EasyConfigData()
        {

            this.Load();
            this.BlockOutputFromEquation();
            this.BlockOutputFromAllMode();
            
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static EasyConfigData Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new EasyConfigData();
                  

                return _instance;
            }
        }

        /// <summary>
        /// Bloque les sorties utiliser dans le mapping dans le mode 'All mode'
        /// </summary>
        public void BlockOutputFromAllMode()
        {
            // on debloque les soetie
            this.DeblockOutputFromAllMode();
            List<Int32> blockVerrou = new List<Int32>();
            // puis on reevalue les sorties
            // blocage des sortie utiliser dans le mode 32 (tt les modes)
            foreach (var mode in this.Modes)
            {
                if (mode.RefMode.Position == 32)
                {
                    // All mode
                    foreach (var effecteur in mode.Effecteurs)
                    {
                        foreach (var position in effecteur.Positions)
                        {
                            // Evaluer les positions et bloquer si nécessaire
                            for (int i = 0; i < position.Outputs.Count; i++)
                            {
                                if (position.Outputs[i].IsUsed)
                                {
                                    this.BlockOutputFromAllMode(i);
                                    blockVerrou.Add(i);
                                }
                            }
                        }
                    }
                    foreach (var verrou in mode.Verrouillages)
                    {
                        for (int i = 0; i < blockVerrou.Count; i++)
                        {
                            verrou.Outputs[blockVerrou[i]].SetIsEnable(false);
                        }
                    }
                }
            }
        } // endMethod: BlockOutputFromAllMode

        /// <summary>
        /// Bloque les sorties utiliser dans les équations manuel
        /// </summary>
        public void BlockOutputFromEquation()
        {
            // Pour le mode universel -> bloquer l'utilisation de la sortie dans tous les modes
            if (PegaseData.Instance.ParamHorsMode != null && PegaseData.Instance.ParamHorsMode.Formules != null)
            {
                foreach (var formule in PegaseData.Instance.ParamHorsMode.Formules)
                {
                    if (formule.FormuleType == TypeFormule.USER)
                    {
                        foreach (var equation in formule.Equations)
                        {
                            this.SearchOutputFromEquation(equation.TextEquation);
                        }
                    }
                }
            }

            // Pour tous les autres modes d'exploitation -> bloquer l'utilisation de la sortie dans le mode considéré et le mode universelle
            if (PegaseData.Instance.OLogiciels != null && PegaseData.Instance.OLogiciels.ModesExploitation != null)
            {
                foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
                {
                    if (mode.Formules != null)
                    {
                        foreach (var formule in mode.Formules)
                        {
                            if (formule.FormuleType == TypeFormule.USER)
                            {
                                foreach (var equation in formule.Equations)
                                {
                                    var QueryMode = from m in this.Modes
                                                    where m.RefMode.Position == mode.Position
                                                    select m;

                                    if (QueryMode.Count() > 0)
                                    {
                                        this.SearchOutputFromEquation(equation.TextEquation, this.Modes[0]);     // -> Bloquer dans le mode universel
                                        this.SearchOutputFromEquation(equation.TextEquation, QueryMode.First()); // -> Bloquer dans le mode en cours
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } // endMethod: BlockOutputFromEquation

        /// <summary>
        /// Recherche une sortie TOR dans l'équation, si elle est trouvé, elle est bloqué en utilisation
        /// </summary>
        public void SearchOutputFromEquation(String EquationTxt)
        {

            // on bloque que les sorties affecte dans une equations.
            Regex outputregex = new Regex("^([0-9_a-zA-Z ]*) := ");
            MatchCollection outputmatches = outputregex.Matches(EquationTxt);
            String result = "";
            if ((outputmatches != null) && (outputmatches.Count > 0))
            {
                result = outputmatches[0].Groups[1].Value;
            }

            String[] parts = result.Split(new Char[] { ' ' });

            foreach (var item in parts)
            {
                if (this.Modes[0].Effecteurs.Count > 0)
                {
                    for (int i = 0; i < this.Modes[0].Effecteurs[0].Positions[0].Outputs.Count; i++)
                    {
                        if (this.Modes[0].Effecteurs[0].Positions[0].Outputs[i].STOR.MnemoLogique == item)
                        {
                            // Bloquer la sortie dans tous les modes
                            foreach (var mode in this.Modes)
                            {
                                foreach (var effecteur in mode.Effecteurs)
                                {
                                    foreach (var position in effecteur.Positions)
                                    {
                                        position.Outputs[i].IsEquationDisable = true;
                                        position.Outputs[i].IsEnable = false;
                                    }
                                }
                                foreach (var verrou in mode.Verrouillages)
                                {
                                    verrou.Outputs[i].IsEquationDisable = true;
                                    verrou.Outputs[i].IsEnable = false;
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
        } // endMethod: SearchOutputFromEquation

        /// <summary>
        /// Recherche une sortie TOR dans l'équation, si elle est trouvé, elle est bloqué en utilisation dans le mode fourni
        /// </summary>
        public void SearchOutputFromEquation(String EquationTxt, EC_Mode mode)
        {
            // on bloque que les sorties affecte dans une equations.
            Regex outputregex = new Regex("^([0-9_a-zA-Z ]*) := ");
            MatchCollection outputmatches = outputregex.Matches(EquationTxt);
            String result = "";
            if ((outputmatches != null) && (outputmatches.Count > 0))
            {
                result = outputmatches[0].Groups[1].Value;
            }

            String[] parts = result.Split(new Char[] { ' ' });

            foreach (var item in parts)
            {
                for (int i = 0; i < this.Modes[0].Effecteurs[0].Positions[0].Outputs.Count; i++)
                {
                    if (this.Modes[0].Effecteurs[0].Positions[0].Outputs[i].STOR.MnemoLogique == item)
                    {
                        // Bloquer la sortie dans tous les modes
                        foreach (var effecteur in mode.Effecteurs)
                        {
                            foreach (var position in effecteur.Positions)
                            {
                                position.Outputs[i].IsEquationDisable = true;
                                position.Outputs[i].IsEnable = false;
                            }
                        }
                        foreach (var verrou in mode.Verrouillages)
                        {
                            verrou.Outputs[i].IsEquationDisable = true;
                            verrou.Outputs[i].IsEnable = false;
                        }
                    }
                }
            }
        } // endMethod: SearchOutputFromEquation

        /// <summary>
        /// Débloque la sortie spécifié pour tous les modes hors 'All mode'
        /// </summary>
        public void DeblockOutputFromAllMode()
        {
            foreach (var mode in this.Modes)
            {
                if (mode.RefMode.Position < 32)
                {
                    foreach (var effecteur in mode.Effecteurs)
                    {
                        foreach (var position in effecteur.Positions)
                        {
                            foreach (var output in position.Outputs)
                            {
                                output.SetIsEnable(true);
                            }
                        }
                    }
                }
                foreach (var verrou in mode.Verrouillages)
                {
                    foreach (var output in verrou.Outputs)
                    {
                        output.SetIsEnable(true);
                    }
                }
            }
        } // endMethod: DeblockOutputFromAllMode

        /// <summary>
        /// Bloque les sorties depuis le mode 'All mode'
        /// </summary>
        public void BlockOutputFromAllMode(Int32 NumOutput)
        {
            foreach (var mode in this.Modes)
            {
                if (mode.RefMode.Position < 32)
                {
                    foreach (var effecteur in mode.Effecteurs)
                    {
                        foreach (var position in effecteur.Positions)
                        {
                            position.Outputs[NumOutput].SetIsEnable(false);
                        }
                    }
                }
            }
        } // endMethod: BlockOutputFromAllMode

        /// <summary>
        /// Initialiser les modes
        /// </summary>
        public void InitModes()
        {
            ObservableCollection<EC_Mode> modes = new ObservableCollection<EC_Mode>();
            ObservableCollection<ComplementaryData> maskmodes = new ObservableCollection<ComplementaryData>();

            if (PegaseData.Instance.OLogiciels != null)
            {
                // Ajouter le mode universel

                // le mode universel est disponible tt Client dans le plugin easy config
                //if (DefaultValues.Get().UserMode == DefaultValues.EXPERT)
                //{
                EC_Mode modeUniversel = new EC_Mode(new ModeExploitation(32));
                modeUniversel.RefMode.ModeLabel = new SelecteurLabel("UniversalMode");
                modeUniversel.RefMode.ModeLabel.LibelSelecteur = LanguageSupport.Get().GetText("EASYCONF/ALL_MODE");
                modes.Add(modeUniversel);
                //}

                // Ajouter tous les modes
                foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
                {
                    EC_Mode m = new EC_Mode(mode);
                    modes.Add(m);
                }

                // configurer le mask de changement de modes
            }
            this._modes = modes;
        } // endMethod: InitModes

        public void InitMaskModes()
        {
            ObservableCollection<ComplementaryData> maskmodes = new ObservableCollection<ComplementaryData>();

            ComplementaryData mask = new ComplementaryData();
            maskmodes.Add(mask);
            this._maskmodes = maskmodes;
        } // endMethod: InitMaskModes

        /// <summary>
        /// Initialiser les modes dans le cadre d'un design
        /// </summary>
        public void InitDesignMode()
        {
            ObservableCollection<EC_Mode> modes = new ObservableCollection<EC_Mode>();
            ModeExploitation mode0 = new ModeExploitation(0);
            mode0.ModeLabel = new SelecteurLabel("sellabel001");
            mode0.ModeLabel.LibelSelecteur = "Mode 1";
            ModeExploitation mode1 = new ModeExploitation(1);
            mode1.ModeLabel = new SelecteurLabel("sellabel002");
            mode1.ModeLabel.LibelSelecteur = "Mode 2";

            modes.Add(new EC_Mode(mode0));
            modes.Add(new EC_Mode(mode1));

            this._modes = modes;
        } // endMethod: InitDesignMode
        #endregion

        /// <summary>
        /// Initialiser les colonnes
        /// </summary>
        public void InitColonnes()
        {
            ObservableCollection<EC_Colonne> colonnes = new ObservableCollection<EC_Colonne>();

            if (PegaseData.Instance.ModuleT != null)
            {
                foreach (var stor in PegaseData.Instance.ModuleT.STORS)
                {
                    EC_Colonne Colonne = new EC_Colonne(stor.MnemoClient);
                    if (stor.IsPWM)
                    {
                        Colonne.IsEnable = false;
                    }
                    else
                    {
                        Colonne.IsEnable = true;
                    }

                    colonnes.Add(Colonne);
                }
            }

            this.Colonnes = colonnes;
        } // endMethod: InitColonnes

        /// <summary>
        /// Initialiser le design des colonnes
        /// </summary>
        public void InitDesignColonnes()
        {
            ObservableCollection<EC_Colonne> colonnes = new ObservableCollection<EC_Colonne>();

            EC_Colonne Colonne = new EC_Colonne("RELAIS_01");
            Colonne.IsEnable = true;
            colonnes.Add(Colonne);
            colonnes.Add(new EC_Colonne("RELAIS_02"));

            this.Colonnes = colonnes;
        } // endMethod: InitDesignColonnes

        /// <summary>
        /// Initialiser les boutons des colonnes
        /// </summary>
        public void InitColonnesBtn()
        {
            ObservableCollection<EC_Colonne> colonnes = new ObservableCollection<EC_Colonne>();

            if (PegaseData.Instance.MOperateur != null)
            {
                foreach (var organ in PegaseData.Instance.MOperateur.OrganesCommandes)
                {
                    // modif CF 
                    if ((organ.MnemoHardFamilleMO == "BT") || ((organ.MnemoHardFamilleMO == "CO") && (organ.ReferenceOrgan != "VIDE") && (!organ.ReferenceOrgan.Contains("DESCRIPT_"))) )
                    {
                        colonnes.Add(new EC_Colonne(organ.MnemoClient,organ.Mnemologique));
                    }
                }
            }

            this.ColonnesBtn = colonnes;
        } // endMethod: InitColonnesBtn

        public void InitDataSystem()
        {
            // recherche des variables fixe du reseau DS
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IO == "DS"
                        select variable;

            String RegexText = JAY.PegaseCore.Helper.ConfigurationReader.Instance.GetValue("_(\\d{1,2})$");
            Regex regex = new Regex(RegexText);
            String result;
            Int32 NumBit8 = 0, Adresse = 0, Adresse16 = 0, Adresse32 = 0, adresseSub=0;
            Int32 NumBit16 = 0;
            Int32 NumBit16Mem = -1;
            Int32 Memoire_type = 1;
            String ModBus;String ModBusOffset;
            // Adresse CANOpen SDO
            String CANOpenSDO;String CANOpenSDOOffset;
            // Adresse CANOpen RPDO
            String CANOpenRPDO;String CANOpenRPDOOffset;
            //profibus
            String Profibus;
            //ethercat
            String Ethercat;
            String EthercatIndex;
            //Devicenet
            String Devicenet;
            //Profinet
            String Profinet;
            //EthernetIp
            String EthernetIp;
            //Powerlink
            String Powerlink;
            // type vraibale;
            String TypeVariable;
            String ModBus_bis;

            uint offsetmodbus;
            offsetmodbus = this.OffsetModbus;
            uint nodeid;
            nodeid = this.CanOpenNodeID;

            foreach (var variable in Query)
            {
                EC_VarNetwork vn = new EC_VarNetwork(this.ListVarBoolInput);
                
                vn.Type = typeVarNetwork.DataSystem;
                // traduction de la variable
                if (variable.Name != "")
                {
                    vn.VariableNetwork = LanguageSupport.Get().GetText("VARRESEAUX/" + variable.Name);
                    if (vn.VariableNetwork == null)
                    {
                        vn.VariableNetwork = variable.Name;
                    }
                }
                vn.Name = variable.Name;
                vn.Adresse = vn.VariableNetwork;
                MatchCollection matches = regex.Matches(variable.UserName);
                if ((matches !=null) && ( matches.Count > 0))
                {
                    NumBit16 = Convert.ToByte(matches[0].Groups[1].Value);
                }
                else
                {
                    NumBit16=0;
                }
                if (NumBit16 < NumBit16Mem)
                {
                    Adresse16++;
                    adresseSub += 1;
                    Adresse++;
                    NumBit16Mem = NumBit16;
                }
                // Calcul des adresses
                // Incrément

                if (variable.VarType == typeof(UInt32))
                {
                    if (Memoire_type != 1)
                    {
                        Adresse32 += 1;
                        adresseSub = 0;
                    }
                    TypeVariable = "Int32";
                    ModBus = String.Format("0x{0:X4} + Offset Modbus", Adresse16);
                    ModBus_bis = String.Format("0x{0:X4}", Adresse16);
                    ModBusOffset = String.Format("0x{0:X4}", Adresse16 + offsetmodbus);

                    // Adresse CANOpen SDO
                    CANOpenSDO = String.Format("0x6404 sub{0:X2} - sub{1:X2}", Adresse+1,Adresse+2);
                    // Adresse CANOpen sdo offset
                    Profibus = string.Format("0x{0:X4} 0x{1:X4}", Adresse16, Adresse16 + 1);
                    //ethercat
                    Ethercat = String.Format("0x{0:X4} ", Adresse32 + 0x2064);
                    EthercatIndex = String.Format("0x{0:X4} ", Adresse32 + 0x2064);

                    Adresse16 += 2;
                    Adresse32 += 1;
                    adresseSub = 0;
                    Adresse+=2;
                    NumBit16Mem = 0;
                    Memoire_type = 1;
                }
                else if (variable.VarType == typeof(UInt16))
                {
                    if (Memoire_type != 2)
                    {
                        Adresse32 += 1;
                        adresseSub = 0;
                    }
                    TypeVariable = "UInt16";
                    ModBus = String.Format("0x{0:X4} + Offset Modbus", Adresse16);
                    ModBus_bis = String.Format("0x{0:X4}", Adresse16);
                    ModBusOffset = String.Format("0x{0:X4}", Adresse16 + offsetmodbus);
                    // Adresse CANOpen SDO
                    CANOpenSDO = String.Format("0x6404 sub{0:X2}", Adresse+1);
                   
                    
                    Profibus = Profibus = string.Format("0x{0:X4}", Adresse16);
                    Ethercat = String.Format("0x{0:X4} Sub{1}", Adresse32 + 0x2064, adresseSub+1);
                    EthercatIndex = String.Format("0x{0:X4} Sub{1} ", Adresse32 + 0x2064, adresseSub+1);

                    Adresse16 += 1;
                    adresseSub += 1;
                    Adresse++;
                    NumBit16Mem = 0;

                    Memoire_type = 2;
                }
                else if (variable.VarType == typeof(Boolean))
                {
                    if (Memoire_type != 3)
                    {
                        Adresse32 += 1;
                        adresseSub = 0;
                    }
                    TypeVariable = "Boolean";
                    ModBus = String.Format("0x{0:X4} bit {1} + Offset Modbus", Adresse16, NumBit16);
                    ModBus_bis   = String.Format("0x{0:X4} bit {1}", Adresse16, NumBit16);
                    ModBusOffset = String.Format("0x{0:X4} bit {1}", Adresse16 + offsetmodbus, NumBit16);
                    // Adresse CANOpen SDO
                    CANOpenSDO = String.Format("0x6404 sub{0:X2} bit {1}", Adresse+1, NumBit16);
                    
                    

                    //profibus
                    Profibus = Profibus = string.Format("0x{0:X4} bit {1}", Adresse16, NumBit16);
                    Ethercat = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse32 + 0x2064, adresseSub + 1, NumBit16);
                    EthercatIndex = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse32 + 0x2064, adresseSub + 1, NumBit16);
                    if (NumBit16 < NumBit16Mem)
                    {
                        Adresse16++;
                        adresseSub += 1;
                        Adresse++;
                    }

                    NumBit16Mem = NumBit16;
                    Memoire_type = 3;
                }
                else
                {
                    ModBus = "";
                    ModBus_bis = "";
                    ModBusOffset = "";
                    CANOpenSDO = "";
                    CANOpenRPDO = "";
                    Profibus = "";
                    Ethercat = "";
                    TypeVariable = "";
                }
                vn.AdrMODBus = ModBus;
                vn.AdrMODBus_bis = ModBus_bis;
                vn.AdrMODBusOffset = ModBusOffset;
                vn.AdrCANSDO = "SDO : "+CANOpenSDO;
                vn.AdrCANRPDO_bis = CANOpenSDO;
                vn.AdrCANRPDO = "TPDO : "+"-";
                vn.AdrCANRPDO_bis = "-";
                vn.AdrCANSDOOffset = "SDO : "+"-";
                vn.AdrCANSDOOffset_bis = "";
                vn.AdrCANRPDOOffset = "TPDO : " + "-";
                vn.AdrCANRPDOOffset_bis = "-";
                vn.AdrProfibus = Profibus;
                vn.AdrEthercat = Ethercat;
                vn.TypeVariable = TypeVariable;
                this.VarNetworks.Add(vn);
            }
        }
        /// <summary>
        /// Initialiser les entrées booléennes
        /// </summary>
        public void InitBoolInput()
        {
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IO == "MBI" && variable.VarType == typeof(Boolean) && variable.Name.Contains("MODBUS")
                        select variable;

            Int32 NumBit = 1;
            Int32 NumBit8 = 0, Adresse = 0, Adresse16 = 0;
            Int32 NumBit16 = 0;

            uint offsetmodbus;
            offsetmodbus = this.OffsetModbus;
            // si offset est different de 0 alors il y a un decolage de 50 entre read et write
            if (offsetmodbus != 0)
            {
                offsetmodbus = offsetmodbus + 50;
            }
            uint nodeid;
            nodeid = this.CanOpenNodeID;

            String ModBus; String ModBusOffset;
            // Adresse CANOpen SDO
            String CANOpenSDO; String CANOpenSDOOffset;
            // Adresse CANOpen RPDO
            String CANOpenRPDO; String CANOpenRPDOOffset;

            //profibus
            String Profibus;
            //ethercat
            String Ethercat;
            String EthercatIndex;
            //Devicenet
            String Devicenet;
            //Profinet
            String Profinet;
            //EthernetIp
            String EthernetIp;
            //Powerlink
            String Powerlink;
            String ModBus_bis;
            String CANOpenRPDO_bis;

            foreach (var variable in Query)
            {
                EC_VarNetwork vn = new EC_VarNetwork(this.ListVarBoolInput);
                vn.Adresse = String.Format("IBoolNetwork.{0:00}", NumBit);
                vn.Type = typeVarNetwork.IBoolNetwork;
                vn.VariableNetwork = variable.Name;

                // Calcul des adresses
                // ModBus
                ModBus = String.Format("0x{0:X4} bit {1} + Offset Modbus", Adresse16, NumBit16);
                ModBus_bis = String.Format("0x{0:X4} bit {1} ", Adresse16, NumBit16);
                ModBusOffset = String.Format("0x{0:X4} bit {1}", Adresse16 + offsetmodbus, NumBit16);
                // CAN SDO
                 CANOpenSDO = String.Format("0x6200sub{0} bit {1}", Adresse + 1, NumBit8);
                // CAN RPDO
                 CANOpenRPDO = String.Format("0x200+Node-Id bit {0}", NumBit);
                CANOpenRPDO_bis  = String.Format("0x200 bit {0}", NumBit);
                CANOpenRPDOOffset = String.Format("0x{0:X4} bit {1}", 0x200 + nodeid, NumBit);
                //profibus
                 Profibus = string.Format("0x{0:X4} bit {1}", Adresse16, NumBit);
                //Ethercat
                 Ethercat = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse + 0x20C8, NumBit16+1, NumBit16);
                 EthercatIndex = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse + 0x20C8, NumBit16+1, NumBit16);

                vn.AdrMODBus = ModBus;
                vn.AdrMODBus_bis = ModBus_bis;
                vn.AdrMODBusOffset = ModBusOffset;
                vn.AdrCANSDO = "SDO : " + CANOpenSDO;
                vn.AdrCANSDO_bis =CANOpenSDO;
                vn.AdrCANRPDO = "RPDO : " + CANOpenRPDO;
                vn.AdrCANRPDO_bis = CANOpenRPDO_bis;
                vn.AdrCANSDOOffset = "SDO : " + "-";
                vn.AdrCANSDOOffset_bis = "";
                vn.AdrCANRPDOOffset = "RPDO : " + CANOpenRPDOOffset;
                vn.AdrCANRPDOOffset_bis = CANOpenRPDOOffset;
                vn.AdrProfibus = Profibus;
                vn.AdrEthercat = Ethercat;
                vn.TypeVariable = "Boolean";

                // Incrément
                NumBit8++;
                if (NumBit8 > 7)
                {
                    NumBit8 = 0;
                    Adresse++;
                }

                NumBit16++;
                if (NumBit16 > 15)
                {
                    NumBit16 = 0;
                    Adresse16++;
                }

                NumBit++;
                this.VarNetworks.Add(vn);
            }
        } // endMethod: InitBoolInput

        /// <summary>
        /// Initialiser les sorties booléennes
        /// </summary>
        public void InitBoolOutput()
        {
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IO == "MBO" && variable.VarType == typeof(Boolean) && variable.Name.Contains("MODBUS")
                        select variable;

            Int32 NumBit = 1;
            Int32 NumBit8 = 0, Adresse = 0, Adresse16 = 0, NumBit16 = 0;

            String ModBus; String ModBusOffset;
            // Adresse CANOpen SDO
            String CANOpenSDO; String CANOpenSDOOffset;
            // Adresse CANOpen RPDO
            String CANOpenRPDO; String CANOpenRPDOOffset;
            //profibus
            String Profibus;
            //ethercat
            String Ethercat;
            String EthercatIndex;
            //Devicenet
            String Devicenet;
            //Profinet
            String Profinet;
            //EthernetIp
            String EthernetIp;
            //Powerlink
            String Powerlink;
            String ModBus_bis;
            String CANOpenRPDO_bis;

            uint offsetmodbus;
            offsetmodbus = this.OffsetModbus;
            uint nodeid;
            nodeid = this.CanOpenNodeID;

            this.ListVarBoolOutputInit = null;

            foreach (var variable in Query)
            {
                EC_VarNetwork vn = new EC_VarNetwork(this.ListVarBoolOutput);
                vn.Adresse = String.Format("OBoolNetwork.{0:00}", NumBit);
                vn.Type = typeVarNetwork.OBoolNetwork;
                vn.VariableNetwork = variable.Name;

                // Calcul des adresses
                // ModBus
                 ModBus = String.Format("0x{0:X4} bit {1} + Offset Modbus", Adresse16 + 14, NumBit16);
                ModBus_bis = String.Format("0x{0:X4} bit {1}", Adresse16 + 14, NumBit16);
                ModBusOffset = String.Format("0x{0:X4} bit {1}", Adresse16 + 14 + offsetmodbus, NumBit16);
                // CAN SDO
                 CANOpenSDO = String.Format("0x6000sub{0} bit {1}", Adresse + 1, NumBit8);
                // CAN RPDO
                 CANOpenRPDO = String.Format("0x180+Node-Id bit {0}", NumBit);
                CANOpenRPDO_bis = String.Format("0x180 bit {0}", NumBit);
                CANOpenRPDOOffset = String.Format("0x{0:X4} bit {1}", 0x180 + nodeid, NumBit);
                //profibus
                 Profibus = String.Format("0x{0:X4} bit {1}", Adresse16, NumBit16);
                 //Ethercat
                 Ethercat = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse + 0x2069, NumBit16 + 1, NumBit16);
                 EthercatIndex = String.Format("0x{0:X4} Sub{1} Bit{2}", Adresse + 0x2069, NumBit16 + 1, NumBit16);


                vn.AdrMODBus = ModBus;
                vn.AdrMODBus_bis = ModBus_bis;
                vn.AdrMODBusOffset = ModBusOffset;
                vn.AdrCANSDO = "SDO : " + CANOpenSDO;
                vn.AdrCANSDO_bis = CANOpenSDO;
                vn.AdrCANRPDO = "TPDO : " + CANOpenRPDO;
                vn.AdrCANRPDO_bis = CANOpenRPDO_bis;
                vn.AdrCANSDOOffset = "SDO : " + "-";
                vn.AdrCANSDOOffset_bis = "";
                vn.AdrCANRPDOOffset = "TPDO : " + CANOpenRPDOOffset;
                vn.AdrCANRPDOOffset_bis = CANOpenRPDO_bis;
                vn.AdrProfibus = Profibus;
                vn.AdrEthercat = Ethercat;
                vn.TypeVariable = "Boolean";
                // Incrément
                NumBit8++;
                if (NumBit8 > 7)
                {
                    NumBit8 = 0;
                    Adresse++;
                }

                NumBit16++;
                if (NumBit16 > 15)
                {
                    NumBit16 = 0;
                    Adresse16++;
                }

                NumBit++;

                this.VarNetworks.Add(vn);
            }
        } // endMethod: InitBoolOutput

        /// <summary>
        /// Initialiser les entrées numériques
        /// </summary>
        public void InitNumericalInput()
        {
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IO == "MBI" && variable.VarType == typeof(Double) && variable.Name.Contains("MODBUS")
                        select variable;

            Int32 Num = 1;
            Int32 NumOctet = 1;
            Int32 Adresse = 0x300;
            Int32 Adresse16 = 0x24;
            Int32 NumBit16 = 0;

            String ModBus; String ModBusOffset;
            // Adresse CANOpen SDO
            String CANOpenSDO; String CANOpenSDOOffset;
            // Adresse CANOpen RPDO
            String CANOpenRPDO; String CANOpenRPDOOffset;
            //profibus
            String Profibus;
            //ethercat
            String Ethercat;
            String EthercatIndex;
            //Devicenet
            String Devicenet;
            //Profinet
            String Profinet;
            //EthernetIp
            String EthernetIp;
            //Powerlink
            String Powerlink;

            uint offsetmodbus;
            offsetmodbus = this.OffsetModbus;
            // si offset est different de 0 alors il y a un decolage de 50 entre read et write
            if (offsetmodbus != 0)
            {
                offsetmodbus = offsetmodbus + 50;
            }
            uint nodeid;
            nodeid = this.CanOpenNodeID;

            foreach (var variable in Query)
            {
                EC_VarNetwork vn = new EC_VarNetwork(this.ListVarNumInput);
                vn.Adresse = String.Format("INumNetwork.{0:00}", Num);
                vn.Type = typeVarNetwork.INumNetwork;
                vn.VariableNetwork = variable.Name;

                // Adresse MODBus
                ModBus = String.Format("0x{0:X4} + Offset Modbus", Num + 3);
                 ModBusOffset = String.Format("0x{0:X4}", Num + offsetmodbus + 3);
                // Adresse CANOpen SDO
                 CANOpenSDO = String.Format("0x6411 sub{0:X2}", Num);
                // Adresse CANOpen RPDO
                 CANOpenRPDO = String.Format("0x{0:X4}+Node-Id Octet {1}", Adresse, NumOctet);
                 CANOpenRPDOOffset = String.Format("0x{0:X4} Octet {1}", Adresse+nodeid, NumOctet);

                 Profibus = String.Format("0x{0:X4} Octet {1}", Adresse16, Adresse16+1);
                 //Ethercat
                 Ethercat = String.Format("0x{0:X4} Sub{1}", 0x20C9, NumBit16 + 1);
                 EthercatIndex = String.Format("0x{0:X4} Sub{1}", 0x20C9, NumBit16 + 1);

                vn.AdrMODBus = ModBus;
                vn.AdrMODBus_bis = ModBus;
                vn.AdrMODBusOffset = ModBusOffset;
                vn.AdrCANSDO = "SDO : " + CANOpenSDO;
                vn.AdrCANSDO_bis = CANOpenSDO;
                vn.AdrCANRPDO = "RPDO : " + CANOpenRPDO;
                vn.AdrCANRPDO_bis = CANOpenRPDO;
                vn.AdrCANSDOOffset = "SDO : " + "-";
                vn.AdrCANSDOOffset_bis = "";
                vn.AdrCANRPDOOffset = "RPDO : " + CANOpenRPDOOffset;
                vn.AdrCANRPDOOffset_bis = CANOpenRPDOOffset;
                vn.AdrProfibus = Profibus;
                vn.AdrEthercat = Ethercat;
                vn.TypeVariable = "Int16" ;
                NumOctet += 2;
                Adresse16 += 2;
                NumBit16 += 1;

                if (NumOctet > 7)
                {
                    NumOctet = 1;
                    Adresse += 0x100;
                    if (Adresse > 0x500)
                    {
                        Adresse = 0x8000;
                    }
                }

                Num++;
                this.VarNetworks.Add(vn);
            }
        } // endMethod: InitNumericalInput

        /// <summary>
        /// Initialiser les sorties numeriques
        /// </summary>
        public void InitNumericalOutput()
        {
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IO == "MBO" && variable.VarType == typeof(Double) && variable.Name.Contains("MODBUS")
                        select variable;

            Int32 Num = 1;
            Int32 NumOctet = 1;
            Int32 Adresse = 0x280;
            Int32 Adresse16 = 0x08;
            Int32 NumBit16 = 0;
            String ModBus; String ModBusOffset;
            // Adresse CANOpen SDO
            String CANOpenSDO; String CANOpenSDOOffset;
            // Adresse CANOpen RPDO
            String CANOpenRPDO; String CANOpenRPDOOffset;
            //profibus
            String Profibus;
            //ethercat
            String Ethercat;
            String EthercatIndex;
            //Devicenet
            String Devicenet;
            //Profinet
            String Profinet;
            //EthernetIp
            String EthernetIp;
            //Powerlink
            String Powerlink;
            uint offsetmodbus;
            offsetmodbus = this.OffsetModbus;
            // si offset est different de 0 pas de decalage sur le read
            uint nodeid;
            nodeid = this.CanOpenNodeID;
            this.ListVarNumOutputInit = null;
            foreach (var variable in Query)
            {
               
                EC_VarNetwork vn = new EC_VarNetwork(this.ListVarNumOutput);
                vn.Adresse = String.Format("ONumNetwork.{0:00}", Num);
                vn.Type = typeVarNetwork.ONumNetwork;
                vn.VariableNetwork = variable.Name;

                // Adresse MODBus
                ModBus = String.Format("0x{0:X4} + Offset Modbus", Num + 0x11);
                 ModBusOffset = String.Format("0x{0:X4}", Num + offsetmodbus + 0x11);
                // Adresse CANOpen SDO
                 CANOpenSDO = String.Format("0x6401 sub{0:X2}", Num);
                // Adresse CANOpen RPDO
                 CANOpenRPDO = String.Format("0x{0:X4}+Node-Id Octet {1}", Adresse, NumOctet);
                 CANOpenRPDOOffset = String.Format("0x{0:X4} Octet {1}", Adresse + nodeid, NumOctet);
                // adresse profibus
                 Profibus = String.Format("0x{0:X4}",Adresse16);
                 //Ethercat
                 Ethercat = String.Format("0x{0:X4} Sub{1}", 0x206A, NumBit16 + 1);
                 EthercatIndex = String.Format("0x{0:X4} Sub{1}", 0x206A, NumBit16 + 1);

                vn.AdrMODBus = ModBus;
                vn.AdrMODBus_bis = ModBus;
                vn.AdrMODBusOffset = ModBusOffset;
                vn.AdrCANSDO = "SDO : " + CANOpenSDO;
                vn.AdrCANSDO_bis = CANOpenSDO;
                vn.AdrCANRPDO = "RPDO : " + CANOpenRPDO;
                vn.AdrCANRPDO_bis = CANOpenRPDO;
                vn.AdrCANSDOOffset = "SDO : " + "-";
                vn.AdrCANSDOOffset_bis = "";
                vn.AdrCANRPDOOffset = "RPDO : " + CANOpenRPDOOffset;
                vn.AdrCANRPDOOffset_bis = CANOpenRPDOOffset;
                vn.AdrProfibus = Profibus;
                vn.AdrEthercat = Ethercat;
                vn.TypeVariable = "Int16" ;

                NumOctet += 2;
                NumBit16 += 1;
                Adresse16 += 2;
                if (NumOctet > 7)
                {
                    NumOctet = 1;
                    Adresse += 0x100;
                    if (Adresse > 0x500)
                    {
                        Adresse = 0x8000;
                    }
                }

                Num++;
                this.VarNetworks.Add(vn);
            }
        } // endMethod: InitNumericalOutput

        /// Les offset modbus
        /// </summary>
        public UInt32 OffsetModbus
        {
            get
            {
                return this._offsetmodbus;
            }
            set
            {
                this._offsetmodbus = value;
            }
        } // endProperty: offset modbus

        /// <summary>
        /// Les offset modbus
        /// </summary>
        public UInt32 CanOpenNodeID
        {
            get
            {
                return this._canopennodeid;
            }
            set
            {
                this._canopennodeid = value;
            }
        } // endProperty: offset modbus

        public UInt32 PowerlinkNodenumber
        {
            get
            {
                return this._powerlinknodenumber;
            }
            set
            {
                this._powerlinknodenumber = value;
            }
        } // endProperty: offset modbus


        public TypeBusEasyConfig TypeBusEasyConfig
        {
            get
            {
                return this._typebuseasyconfig;
            }
            set
            {
                this._typebuseasyconfig = value;
            }
        } // endProperty: offset modbus
        /// <summary>
        /// Initialiser les données du SBC à partir du XML
        /// </summary>
        public Int32 InitNetwork()
        {
            
            // Masque sur support de charge
            UInt32 UIntValue;
            Int32 IntValue;
            String Value;
            VariableEditable VE;


            VE = GestionVariableSimple.Instance.GetVariableByName("OffsetModbus");
            try
            {
                UIntValue = Convert.ToUInt32(VE.RefElementValue);
                if (UIntValue == 0xFFFF)
                {
                    UIntValue = 0;
                }
            }
            catch
            {
                UIntValue = 0;
            }
            this.OffsetModbus = UIntValue;

            VE = GestionVariableSimple.Instance.GetVariableByName("COAdresse");
            try
            {
                UIntValue = Convert.ToUInt32(VE.RefElementValue);
                if (UIntValue == 0xFFFF)
                {
                    UIntValue = 0;
                }
            }
            catch
            {
                UIntValue = 0;
            }
            this.CanOpenNodeID = UIntValue;

            VE = GestionVariableSimple.Instance.GetVariableByName("PBVitesse");
            try
            {
                UIntValue = Convert.ToUInt32(VE.RefElementValue);
                if (UIntValue == 0xFFFF)
                {
                    UIntValue = 0;
                }
            }
            catch
            {
                UIntValue = 0;
            }
            this.PowerlinkNodenumber = UIntValue;

            VE = GestionVariableSimple.Instance.GetVariableByName("CRProfibus");
            TypeBusEasyConfig typebus;
            try
            {
                if (VE.RefElementValue == null)
                {
                    IntValue = -1;
                }
                else
                {
                    IntValue = Convert.ToInt32(VE.RefElementValue);
                }

                if ((IntValue < 0) || (IntValue > 200))
                {
                    VE = GestionVariableSimple.Instance.GetVariableByName("CanOpen");
                    IntValue = Convert.ToInt32(VE.RefElementValue);
                    if (IntValue == 1)
                    {
                        typebus = TypeBusEasyConfig.CANOPEN;
                    }
                    else
                    {
                        VE = GestionVariableSimple.Instance.GetVariableByName("ModBus");
                        IntValue = Convert.ToInt32(VE.RefElementValue);
                        if (IntValue == 1)
                        {
                            typebus = TypeBusEasyConfig.MODBUS;
                        }
                        else
                        {
                            typebus = TypeBusEasyConfig.AUCUN;
                        }
                    }
                }
                else
                {
                    switch (IntValue)
                    {
                        case 2:
                            typebus = TypeBusEasyConfig.PROFIBUSV1;
                            break;
                        case 4:
                            typebus = TypeBusEasyConfig.DEVICENET;
                            break;
                        case 8:
                            typebus = TypeBusEasyConfig.ETHERCAT;
                            break;
                        case 9:
                            typebus = TypeBusEasyConfig.PROFINET;
                            break;
                        case 13:
                            typebus = TypeBusEasyConfig.ETHRNETIP;
                            break;
                        case 15:
                            typebus = TypeBusEasyConfig.POWERLINK;
                            break;
                        case 16:
                            typebus = TypeBusEasyConfig.MODBUS;
                            break;
                        default:
                            typebus = TypeBusEasyConfig.AUCUN;
                            break;
                    }
                }
            }
            catch
            {
                UIntValue = 0;
                typebus = typebus = TypeBusEasyConfig.AUCUN;

            }
            this.TypeBusEasyConfig = typebus;
            return 0;
        }

        /// <summary>
        /// Charger les données précédentes
        /// </summary>
        public void Load()
        {
            if (PegaseData.Instance.CurrentPackage != null && PegaseData.Instance.XMLRoot != null)
            {
                PegaseData.Instance.CurrentPackage.OpenPackage();

                // Ourvrir la configuration boutons / relais

                if (PegaseData.Instance.XMLRoot.Descendants("EasyConfigData").Count() > 0)
                {
                    XElement EasyConfigData = PegaseData.Instance.XMLRoot.Descendants("EasyConfigData").First();

                    if (EasyConfigData.Descendants("BoutonRelais").Count() > 0)
                    {
                        XElement RootBR = EasyConfigData.Descendants("BoutonRelais").First();

                        foreach (XElement item in RootBR.Descendants("Mode"))
                        {
                            Int32 Pos = Convert.ToInt32(item.Attribute("Position").Value);

                            var Query = from mode in this.Modes
                                        where mode.RefMode.Position == Pos
                                        select mode;

                            if (Query.Count() > 0)
                            {
                                Query.First().LoadConfig(item);
                            }
                        }
                    }

                    // Ouvrir la configuration axe

                    if (EasyConfigData.Descendants("Axe").Count() > 0)
                    {
                        XElement RootAxe = EasyConfigData.Descendants("Axe").First();

                        foreach (XElement item in RootAxe.Descendants("Mode"))
                        {
                            Int32 Pos = Convert.ToInt32(item.Attribute("Position").Value);

                            var Query = from mode in this.Modes
                                        where mode.RefMode.Position == Pos
                                        select mode;

                            if (Query.Count() > 0)
                            {
                                Query.First().LoadAxe(item);
                            }
                        }
                    }

                    // Ouvrir la configuration verrouBtn

                    if (EasyConfigData.Descendants("VerrouBtn").Count() > 0)
                    {
                        XElement RootVerrouBtn = EasyConfigData.Descendants("VerrouBtn").First();

                        foreach (XElement item in RootVerrouBtn.Descendants("Mode"))
                        {
                            Int32 Pos = Convert.ToInt32(item.Attribute("Position").Value);

                            var Query = from mode in this.Modes
                                        where mode.RefMode.Position == Pos
                                        select mode;

                            if (Query.Count() > 0)
                            {
                                Query.First().LoadVerrouBtn(item);
                            }
                        }
                    }

                    // Ouvrir la configuration verrou

                    if (EasyConfigData.Descendants("Verrou").Count() > 0)
                    {
                        XElement RootVerrou = EasyConfigData.Descendants("Verrou").First();

                        foreach (XElement item in RootVerrou.Descendants("Mode"))
                        {
                            Int32 Pos = Convert.ToInt32(item.Attribute("Position").Value);

                            var Query = from mode in this.Modes
                                        where mode.RefMode.Position == Pos
                                        select mode;

                            if (Query.Count() > 0)
                            {
                                Query.First().LoadVerrou(item);
                            }
                        }
                    }

                    // Ouvrir la configuration Réseau

                    if (EasyConfigData.Descendants("Reseau").Count() > 0)
                    {
                        XElement RootReseau = EasyConfigData.Descendants("Reseau").FirstOrDefault();

                        if (RootReseau != null)
                        {
                            this.LoadReseau(RootReseau);
                        }
                    }
                }

                PegaseData.Instance.CurrentPackage.ClosePackage();
            }
            // generer les equations
           // BuildEquations();
        } // endMethod: Load

        /// <summary>
        /// Charger les données réseau
        /// </summary>
        public void LoadReseau(XElement rootReseau)
        {
            foreach (var xvariable in rootReseau.Descendants("Variable"))
            {
                String networkVar = xvariable.Attribute("NetworkVar").Value;
                String internalVar = xvariable.Attribute("InternalVar").Value;

                var Query = from variable in this.VarNetworks
                            where variable.VariableNetwork == networkVar
                            select variable;

                if (Query.Count() > 0)
                {
                    Query.First().VariableInternal = internalVar;
                }
            }
        } // endMethod: LoadReseau

        /// <summary>
        /// Sauvegarder les données
        /// </summary>
        public Boolean Save()
        {
            JAY.FileCore.iDialogPackage package = PegaseData.Instance.CurrentPackage;
            Boolean Result = false;

            if (package != null && PegaseData.Instance.XMLRoot != null)
            {
                XDocument zConfig_dat = new XDocument();
                XDocument zConfigAxe_dat = new XDocument();
                XDocument zConfigVerrou_dat = new XDocument();
                XDocument zConfigVerrouBtn_dat = new XDocument();

                XElement RootC = new XElement("BoutonRelais");
                XAttribute attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "BoutonRelais");
                RootC.Add(attribCode);

                XElement RootAxe = new XElement("Axe");
                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Axe");
                RootAxe.Add(attribCode);

                XElement RootVerrou = new XElement("Verrou");
                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Verrou");
                RootVerrou.Add(attribCode);

                XElement RootVerrouBtn = new XElement("VerrouBtn");
                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "VerrouBtn");
                RootVerrouBtn.Add(attribCode);

                XElement RootReseau = new XElement("Reseau");
                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Reseau");
                RootReseau.Add(attribCode);

                zConfig_dat.AddFirst(RootC);
                zConfigAxe_dat.AddFirst(RootAxe);
                zConfigVerrou_dat.AddFirst(RootVerrou);
                zConfigVerrouBtn_dat.AddFirst(RootVerrouBtn);

                // 1 - fabriquer le xml
                foreach (var mode in Modes)
                {
                    // Config
                    XElement XModeConfig = new XElement(String.Format("Mode", mode.RefMode.Position));
                    attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Mode");
                    XModeConfig.Add(attribCode);
                    XAttribute Xattrib = new XAttribute("Position", mode.RefMode.Position);
                    XModeConfig.Add(Xattrib);

                    // 1.1 fabriquer les données de ce noeud
                    XModeConfig = mode.SaveConfig(XModeConfig);

                    // 2 - ConfigAxe
                    XElement XModeConfigAxe = new XElement(String.Format("Mode", mode.RefMode.Position));
                    attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Mode");
                    XModeConfigAxe.Add(attribCode);
                    XAttribute XattribConfigAxe = new XAttribute("Position", mode.RefMode.Position);
                    XModeConfigAxe.Add(XattribConfigAxe);

                    // 2.1 - fabriquer les données de ce noeud
                    XModeConfigAxe = mode.SaveAxe(XModeConfigAxe);

                    // 3 - ConfigVerrou
                    XElement XModeConfigVerrou = new XElement(String.Format("Mode", mode.RefMode.Position));
                    attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Mode");
                    XModeConfigVerrou.Add(attribCode);
                    XAttribute XattribVerrou = new XAttribute("Position", mode.RefMode.Position);
                    XModeConfigVerrou.Add(XattribVerrou);

                    // 3.1 - fabriquer les données de ce noeud
                    XModeConfigVerrou = mode.SaveVerrou(XModeConfigVerrou);

                    // 4 - ConfigVerrouBtn
                    XElement XModeConfigVerrouBtn = new XElement(String.Format("Mode", mode.RefMode.Position));
                    attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Mode");
                    XModeConfigVerrouBtn.Add(attribCode);
                    XAttribute XattribVerrouBtn = new XAttribute("Position", mode.RefMode.Position);
                    XModeConfigVerrouBtn.Add(XattribVerrouBtn);

                    // 4.1 - fabriquer les données de ce noeud
                    XModeConfigVerrouBtn = mode.SaveVerrouBtn(XModeConfigVerrouBtn);

                    // 5 ajouter le noeud à la collection
                    RootC.Add(XModeConfig);
                    RootAxe.Add(XModeConfigAxe);
                    RootVerrou.Add(XModeConfigVerrou);
                    RootVerrouBtn.Add(XModeConfigVerrouBtn);
                }

                RootReseau = this.SaveReseau(RootReseau);

                // 5 - enregistrer

                // 5.1 - si la section existe, la supprimer dans le fichier iDialog
                XElement EasyRoot;

                if (PegaseData.Instance.XMLRoot != null)
                {
                    if (PegaseData.Instance.XMLRoot.Descendants("EasyConfigData").Count() > 0)
                    {
                        EasyRoot = PegaseData.Instance.XMLRoot.Descendants("EasyConfigData").First();
                        EasyRoot.Remove();
                    }
                }

                // 5.2 - construire la section dans la version en cours du fichier iDialog
                EasyRoot = new XElement("EasyConfigData");
                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "EasyConfigData");
                EasyRoot.Add(attribCode);
                EasyRoot.Add(RootC);
                EasyRoot.Add(RootVerrou);
                EasyRoot.Add(RootVerrouBtn);
                EasyRoot.Add(RootAxe);
                EasyRoot.Add(RootReseau);

                PegaseData.Instance.XMLRoot.AddFirst(EasyRoot);
                
 //               this.MaskModes[0].SerialiseMaskM();
                Result = true;
            }

            return Result;
        } // endMethod: Save

        /// <summary>
        /// Sauver les données réseau
        /// </summary>
        public XElement SaveReseau(XElement rootReseau)
        {
            XElement Result = rootReseau;

            foreach (var variable in this.VarNetworks)
            {
                
                if (variable.CurrentVariable != null && variable.CurrentVariable.Name != "" && variable.Adresse!=null)
                {
                    XAttribute xattrib;
                    XElement XVariable = new XElement("Variable");
                    xattrib = new XAttribute("Adresse", variable.Adresse);
                    XVariable.Add(xattrib);
                    xattrib = new XAttribute("Type", variable.Type);
                    XVariable.Add(xattrib);
                    xattrib = new XAttribute("NetworkVar", variable.VariableNetwork);
                    XVariable.Add(xattrib);
                    xattrib = new XAttribute("InternalVar", variable.VariableInternal);
                    XVariable.Add(xattrib);

                    rootReseau.Add(XVariable);
                }
            }

            return rootReseau;
        } // endMethod: SaveReseau

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void BuildEquations()
        {
            // Supprimer les équations générées précédement avec easy config
            // Mode universel
            var QueryUniversal = from formule in PegaseData.Instance.ParamHorsMode.Formules
                                 where formule.FormuleType == TypeFormule.AUTO
                                 select formule;

            List<Formule> FormuleUniversal = QueryUniversal.ToList<Formule>();

            if (FormuleUniversal != null)
            {
                for (int i = FormuleUniversal.Count - 1; i >= 0; i--)
                {
                    PegaseData.Instance.ParamHorsMode.Formules.Remove(FormuleUniversal[i]);
                }
            }

            // Tous les modes equation de easy config
            foreach (var mode in this.Modes)
            {
                var QueryMode = from formule in mode.RefMode.Formules
                                where formule.FormuleType == TypeFormule.AUTO
                                select formule;

                List<Formule> Formules = QueryMode.ToList<Formule>();

                if (Formules != null)
                {
                    for (int i = Formules.Count - 1; i >= 0; i--)
                    {
                        mode.RefMode.Formules.Remove(Formules[i]);
                    }
                }
            }

            // Mode en sécurité equation de easy config
            var QuerySecu = from formule in PegaseData.Instance.OLogiciels.ModeSecurite.Formules
                            where formule.FormuleType == TypeFormule.AUTO
                            select formule;

            if (QuerySecu != null && QuerySecu.Count() > 0)
            {
                List<Formule> FormuleSecu = QuerySecu.ToList<Formule>();

                for (int i = FormuleSecu.Count - 1; i >= 0; i--)
                {
                    PegaseData.Instance.OLogiciels.ModeSecurite.Formules.Remove(FormuleSecu[i]);
                }
            }

            // Chercher le mode universel
            foreach (var mode in this.Modes)
            {
                if (mode.RefMode.Position == 32)
                {
                    // construire les équations réseau
                    foreach (VariableE variable in PegaseData.Instance.Variables)
                    {
                        variable.IsUsedByUniversalMode = false;
                    }
                    EquationBuilder.BuildAllNetworkEquation(mode);
                    // Créer les équations en sécurité
                    EquationBuilder.BuildSecurityEquation(mode);
                    break;
                }
                else
                {

                }
            }

            // Créer le dico des variables utilisables
            if (this.Modes[0].Effecteurs.Count > 0)
            {
                EquationBuilder.BuildAllEquations(this.Modes);
            }
        } // endMethod: BuildEquations
        // Messages
        #region Messages

        #endregion

    } // endClass: EasyConfigData
}
