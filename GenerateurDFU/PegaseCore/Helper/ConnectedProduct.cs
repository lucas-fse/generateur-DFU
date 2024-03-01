using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;
using System.ComponentModel;
using System.Windows;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Un produit connecté
    /// </summary>
    public class ConnectedProduct : Mvvm.ViewModelBase
    {
        // constantes
        #region Constantes

        public const UInt32 NB_OCTET_CODEID = 4;
        public const UInt32 NB_OCTET_PARAMIDIALOG = 5;
        public const UInt32 ADRESSE_CODE_MARCHE_MT = 0x8C;
        public const UInt32 ADRESSE_CODE_MARCHE_MO = 0x24C;
        public const UInt32 NB_OCTET_CODE_MARCHE = 4;
        public const UInt32 ADRESSE_SERIAL_MT = 0x90;
        public const UInt32 ADRESSE_SERIAL_MO = 0x250;
        public const UInt32 ADRESSE_SERIAL_SIM_MT = 0x28;
        public const UInt32 ADRESSE_SERIAL_SIM_MO = 0x41E;
        public const UInt32 NB_OCTET_SERIAL_MT = 11;
        public const UInt32 NB_OCTET_SERIAL_MO = 11;
        public const UInt32 NB_OCTET_SERIAL_SIM = 10;
        public const UInt32 ADRESSE_MARCHE_SIM = 0x48;
        public const UInt32 ADRESSE_REFCOMMERCIALE_SIM = 0x49;
        public const UInt32 NB_OCTET_REFMARCHESIM = 1;
        public const UInt32 NB_OCTET_REFCOMSIM = 6;
        public const UInt32 ADRESSE_PLD_MT = 0x11C;
        public const UInt32 ADRESSE_PLD_MO = 0x2DC;
        public const UInt32 NB_OCTET_PLD_MT = 0x1;
        public const UInt32 NB_OCTET_PLD_MO = 0x1;

        #endregion

        // Variables
        #region Variables
        private ObservableCollection<UInt32> _mo_NombreManoeuvre;
        private UInt32 _mo_NombreChoc;
        private UInt32 _mo_NombreAlarme;
        private UInt32 _mo_TempsAbsolu;

        private UInt32 _mt_SecuMaitre_NbManoeuvre;
        private UInt32 _mt_SecuMaitre_TempsActif;
        private UInt32 _mt_SecuEsclave_NbManoeuvre;
        private UInt32 _mt_SecuEsclave_TempsActif;
        private ObservableCollection<UInt32> _mt_Relais_NbManoeuvre;
        private ObservableCollection<UInt32> _mt_Relais_TempsActif;

        private VidPid _productVidPid;

        private String _firmwareVersion;
        protected Hid.Hid _hidPort;
        protected ProductType _typeProduct = ProductType.Unknown;
        private String _codeId;
        private String _codeIdMO;
        private String _codeIdMT;
        private String _paramIDialog;
        private Boolean _isNotInMajFirmware = true;
        private Int32 _memorySize;
        private DateTime _utcProduct;
        private DateTime _readUTCDateTime;
        private Int32 _progressValue;

        private Visibility _fonctionnalityVisibility;
        private Visibility _MTOnlyVisibility;
        private Visibility _MOOnlyVisibility;
        private Visibility _simErrorReadVisibility;

        private String _marketCodeMT;
        private String _serialNumberMT;
        private String _serialNumberSIM;
        private String _refCommercialSIM;
        private String _RefMarcheSim;
        private String _marketCodeMO;
        private String _serialNumberMO;

        #endregion

        // Propriétés
        #region Propriétés

            public bool IsMT
        {
            get;set;
        }
        public bool IsMO
        {
            get; set;
        }
        /// <summary>
        /// La visibilité pour les experts uniquement
        /// </summary>
        public Visibility ExpertOnlyVisibility
        {
            get
            {
                Visibility Result = Visibility.Hidden;

                if (DefaultValues.Get().UserMode == DefaultValues.EXPERT)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: ExpertVisibility

        /// <summary>
        /// Le code marché du MT
        /// </summary>
        public String MarketCodeMT
        {
            get
            {
                return this._marketCodeMT;
            }
            set
            {
                this._marketCodeMT = value;
                RaisePropertyChanged("MarketCodeMT");
            }
        } // endProperty: MarketCodeMT

        /// <summary>
        /// Le numéro de série du MT
        /// </summary>
        public String SerialNumberMT
        {
            get
            {
                return this._serialNumberMT;
            }
            set
            {
                this._serialNumberMT = value;
                RaisePropertyChanged("SerialNumberMT");
            }
        } // endProperty: SerialNumberMT

        /// <summary>
        /// Le numéro de série de la SIM
        /// </summary>
        public String SerialNumberSIM
        {
            get
            {
                return this._serialNumberSIM;
            }
            set
            {
                this._serialNumberSIM = value;
                RaisePropertyChanged("SerialNumberSIM");
            }
        } // endProperty: SerialNumberSIM

        /// <summary>
        /// La référence commerciale de la SIM
        /// </summary>
        public String RefCommercialSIM
        {
            get
            {
                return this._refCommercialSIM;
            }
            set
            {
                this._refCommercialSIM = value;
                RaisePropertyChanged("RefCommercialSIM");
            }
        } // endProperty: RefCommercialSIM
          /// <summary>
          /// La référence commerciale de la SIM
          /// </summary>
        public String RefMarcheSim
        {
            get
            {
                return this._RefMarcheSim;
            }
            set
            {
                this._RefMarcheSim = value;
                RaisePropertyChanged("RefMarcheSim");
            }
        } // endProperty: RefCommercialSIM
        
        /// <summary>
        /// Le code marché du MO
        /// </summary>
        public String MarketCodeMO
        {
            get
            {
                return this._marketCodeMO;
            }
            set
            {
                this._marketCodeMO = value;
                RaisePropertyChanged("MarketCodeMO");
            }
        } // endProperty: MarketCodeMO

        /// <summary>
        /// Le numéro de série du MO
        /// </summary>
        public String SerialNumberMO
        {
            get
            {
                return this._serialNumberMO;
            }
            set
            {
                this._serialNumberMO = value;
                RaisePropertyChanged("SerialNumberMO");
            }
        } // endProperty: SerialNumberMO

        public UInt32 TypePldLevel
        { get; set; }

        /// <summary>
        /// Erreur de lecture de la sim
        /// </summary>
        public Visibility SimErrorReadVisibility
        {
            get
            {
                if (this.TypeProduct == ProductType.MT && this.MemorySize == 0)
                {
                    this._simErrorReadVisibility = Visibility.Visible;
                }
                else
                {
                    this._simErrorReadVisibility = Visibility.Hidden;
                }

                return this._simErrorReadVisibility;
            }
        } // endProperty: SimErrorReadVisibility

        /// <summary>
        /// La visibilité des affichages spécifiques du MT
        /// </summary>
        public Visibility MTOnlyVisibility
        {
            get
            {
                if (this.TypeProduct == ProductType.MT)
                {
                    this._MTOnlyVisibility = Visibility.Visible;
                }
                else
                {
                    this._MTOnlyVisibility = Visibility.Collapsed;
                }
                return this._MTOnlyVisibility;
            }
        } // endProperty: MTOnlyVisibility

        /// <summary>
        /// La visibilité des affichages spécifiques du MO
        /// </summary>
        public Visibility MOOnlyVisibility
        {
            get
            {
                if (this.TypeProduct == ProductType.MO)
                {
                    this._MOOnlyVisibility = Visibility.Visible;
                }
                else
                {
                    this._MOOnlyVisibility = Visibility.Collapsed;
                }
                return this._MOOnlyVisibility;
            }
        } // endProperty: MOOnlyVisibility

        /// <summary>
        /// La visibilité des fonctionnalités
        /// </summary>
        public Visibility FonctionnalityVisibility
        {
            get
            {
                if (this.TypeProduct == ProductType.MO || this.TypeProduct == ProductType.MT)
                {
                    this._fonctionnalityVisibility = Visibility.Visible;
                }
                else
                {
                    this._fonctionnalityVisibility = Visibility.Collapsed;
                }

                return this._fonctionnalityVisibility;
            }
        } // endProperty: FonctionnalityVisibility

        /// <summary>
        /// La valeur de progrès lors d'action longue
        /// </summary>
        public Int32 ProgressValue
        {
            get
            {
                return this._progressValue;
            }
            set
            {
                this._progressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        } // endProperty: ProgressValue

        /// <summary>
        /// MO : Le nombre de manoeuvres de chacun des organes
        /// </summary>
        public ObservableCollection<UInt32> MO_NombreManoeuvre
        {
            get
            {
                return this._mo_NombreManoeuvre;
            }
            set
            {
                this._mo_NombreManoeuvre = value;
                RaisePropertyChanged("MO_NombreManoeuvre");
            }
        } // endProperty: MO_NombreManoeuvre

        /// <summary>
        /// MO : Le nombre de choc
        /// </summary>
        public UInt32 MO_NombreChoc
        {
            get
            {
                return this._mo_NombreChoc;
            }
            set
            {
                this._mo_NombreChoc = value;
                RaisePropertyChanged("MO_NombreChoc");
            }
        } // endProperty: MO_NombreChoc

        /// <summary>
        /// MO : Le nombre d'alarmes
        /// </summary>
        public UInt32 MO_NombreAlarme
        {
            get
            {
                return this._mo_NombreAlarme;
            }
            set
            {
                this._mo_NombreAlarme = value;
                RaisePropertyChanged("MO_NombreAlarme");
            }
        } // endProperty: MO_NombreAlarme

        /// <summary>
        /// MO : Temps absolu de fonctionnement
        /// </summary>
        public UInt32 MO_TempsAbsolu
        {
            get
            {
                return this._mo_TempsAbsolu;
            }
            set
            {
                this._mo_TempsAbsolu = value;
                RaisePropertyChanged("MO_TempsAbsolu");
            }
        } // endProperty: MO_TempsAbsolu

        /// <summary>
        /// MT : Le nombre de manoeuvre sur sécu maitre
        /// </summary>
        public UInt32 MT_SecuMaitre_NbManoeuvre
        {
            get
            {
                return this._mt_SecuMaitre_NbManoeuvre;
            }
            set
            {
                this._mt_SecuMaitre_NbManoeuvre = value;
                RaisePropertyChanged("MT_SecuMaitre_NbManoeuvre");
            }
        } // endProperty: MT_SecuMaitre_NbManoeuvre

        /// <summary>
        /// MT : Temps actif sur sécu maitre
        /// </summary>
        public UInt32 MT_SecuMaitre_TempsActif
        {
            get
            {
                return this._mt_SecuMaitre_TempsActif;
            }
            set
            {
                this._mt_SecuMaitre_TempsActif = value;
                RaisePropertyChanged("MT_SecuMaitre_TempsActif");
            }
        } // endProperty: MT_SecuMaitre_TempsActif

        /// <summary>
        /// MT : Nombre de manoeuvre sur le sécu esclave
        /// </summary>
        public UInt32 MT_SecuEsclave_NbManoeuvre
        {
            get
            {
                return this._mt_SecuEsclave_NbManoeuvre;
            }
            set
            {
                this._mt_SecuEsclave_NbManoeuvre = value;
                RaisePropertyChanged("MT_SecuEsclave_NbManoeuvre");
            }
        } // endProperty: MT_SecuEsclave_NbManoeuvre

        /// <summary>
        /// MT : Temps actif du sécu esclave
        /// </summary>
        public UInt32 MT_SecuEsclave_TempsActif
        {
            get
            {
                return this._mt_SecuEsclave_TempsActif;
            }
            set
            {
                this._mt_SecuEsclave_TempsActif = value;
                RaisePropertyChanged("MT_SecuEsclave_TempsActif");
            }
        } // endProperty: MT_SecuEsclave_TempsActif

        /// <summary>
        /// MT : Le nombre de manoeuvre sur les relais
        /// </summary>
        public ObservableCollection<UInt32> MT_Relais_NbManoeuvre
        {
            get
            {
                return this._mt_Relais_NbManoeuvre;
            }
            set
            {
                this._mt_Relais_NbManoeuvre = value;
                RaisePropertyChanged("MT_Relais_NbManoeuvre");
            }
        } // endProperty: MT_Relais_NbManoeuvre

        /// <summary>
        /// MT : Le temps actif de chacun des relais
        /// </summary>
        public ObservableCollection<UInt32> MT_Relais_TempsActif
        {
            get
            {
                return this._mt_Relais_TempsActif;
            }
            set
            {
                this._mt_Relais_TempsActif = value;
                RaisePropertyChanged("MT_Relais_TempsActif");
            }
        } // endProperty: MT_Relais_TempsActif

        /// <summary>
        /// La date UTC du produit
        /// </summary>
        public DateTime UTCProduct
        {
            get
            {
                return this._utcProduct;
            }
        } // endProperty: UTCProduct

        /// <summary>
        /// La date et heure du produit recalculée depuis la dernière lecture dans le produit
        /// </summary>
        public DateTime CurrentUTCProduct
        {
            get
            {
                DateTime Result = this.UTCProduct + (DateTime.UtcNow - this._readUTCDateTime);

                return Result;
            }
        } // endProperty: CurrentUTCProduct

        /// <summary>
        /// La date et heure du produit recalculée depuis la dernière lecture du produit
        /// </summary>
        public DateTime CurrentLocalDateTime
        {
            get
            {
                DateTime Result = this.LocalDateProduct + (DateTime.UtcNow - this._readUTCDateTime);

                return Result;
            }
        } // endProperty: CurrentLocalDateTime

        /// <summary>
        /// La date local du produit
        /// </summary>
        public DateTime LocalDateProduct
        {
            get
            {
                return this.UTCProduct.ToLocalTime();
            }
        } // endProperty: LocalDateProduct

        /// <summary>
        /// La taille mémoire
        /// </summary>
        public Int32 MemorySize
        {
            get
            {
                return this._memorySize;
            }
            set
            {
                this._memorySize = value;
                RaisePropertyChanged("MemorySize");
            }
        } // endProperty: MemorySize

        /// <summary>
        /// Le produit est en cours de mise à jour pour le firmware
        /// </summary>
        public Boolean IsNotInMajFirmeware
        {
            get
            {
                return this._isNotInMajFirmware;
            }
            set
            {
                this._isNotInMajFirmware = value;
                RaisePropertyChanged("IsInMajFirmeware");
            }
        } // endProperty: IsInFlash

        /// <summary>
        /// Le code Id du produit connecté
        /// </summary>
        public String CodeID
        {
            get
            {
                return this._codeId;
            }
            protected set
            {
                this._codeId = value;
            }
        } // endProperty: CodeID

        /// <summary>
        /// Le code IDMO du produit
        /// il s'agit de son code ID (cas d'un MO)
        /// du code ID du produit apparié (MT)
        /// ou il est vide (Chargeur...)
        /// </summary>
        public String CodeIDMO
        {
            get
            {
                return this._codeIdMO;
            }
            protected set
            {
                this._codeIdMO = value;
            }
        } // endProperty: CodeIDMO

        /// <summary>
        /// Le code IDMT du produit
        /// il s'agit de son code ID (cas d'un MT)
        /// du code ID du produit apparié (MO)
        /// ou il est vide (Chargeur...)
        /// </summary>
        public String CodeIDMT
        {
            get
            {
                return this._codeIdMT;
            }
            protected set
            {
                this._codeIdMT = value;
            }
        } // endProperty: CodeIDMT

        /// <summary>
        /// le paramètre de traçabilité iDialog
        /// pour un MO ou un MT il est égal au numéro de série de la SIM
        /// présente sur le système.
        /// Permet de vérifier également que la fiche est cohérente avec le matériel
        /// </summary>
        public String ParamIDialog
        {
            get
            {
                return this._paramIDialog;
            }
            private set
            {
                this._paramIDialog = value;
            }
        } // endProperty: ParamIDialog

        /// <summary>
        /// Décrit le type de produit connecté
        /// </summary>
        public ProductType TypeProduct
        {
            get
            {
                return this._typeProduct;
            }
            set
            {
                this._typeProduct = value;
                RaisePropertyChanged("TypeProduct");
                RaisePropertyChanged("FonctionnalityVisibility");
                RaisePropertyChanged("MTOnlyVisibility");
            }
        } // endProperty: TypeProduct

        /// <summary>
        /// traduction pour le bouton mise à jour du firmware
        /// </summary>
        public String MAJ_FirmWare
        {
            get
            {
                return "/MAJMOMT/MAJ_FIRM";
            }
        } // endProperty: MAJ_FirmWare

        /// <summary>
        /// traduction pour le numéro de série 
        /// </summary>
        public String LabelSerialNumber
        {
            get
            {
                return "/MAJMOMT/SERIALNUMBER";
            }
        } // endProperty: LabelSerialNumber

        /// <summary>
        /// Le libellé pour le marché
        /// </summary>
        public String LabelMarket
        {
            get
            {
                return "/MAJMOMT/MARKET";
            }
        } // endProperty: LabelMarket

        /// <summary>
        /// Le libellé pour la référence
        /// </summary>
        public String LabelReference
        {
            get
            {
                return "/MAJMOMT/REFERENCE";
            }
        } // endProperty: LabelReference

        /// <summary>
        /// Le libellé pour le code ID
        /// </summary>
        public String LabelCodeId
        {
            get
            {
                return "/MAJMOMT/CODEID";
            }
        } // endProperty: LabelCodeId

        /// <summary>
        /// Le label du transceiver
        /// </summary>
        public String LabelTransceiver
        {
            get
            {
                return "/CHECK_MATERIEL/MT";
            }
        } // endProperty: LabelTransceiver

        /// <summary>
        /// Le label du module opérateur
        /// </summary>
        public String LabelModuleOperateur
        {
            get
            {
                return "/CHECK_MATERIEL/MO";
            }
        } // endProperty: LabelModuleOperateur

        /// <summary>
        /// Le libellé pour le journal
        /// </summary>
        public String LabelJournal
        {
            get
            {
                return "/MAJMOMT/JOURNAL";
            }
        } // endProperty: LabelJournal

        public string LabelReadInfo
        {
            get
            {
                return "/MAJMOMT/READINFO";
            }
        }
        /// <summary>
        /// Le libellé pour le compteur
        /// </summary>
        public String LabelCompteur
        {
            get
            {
                return "/MAJMOMT/COMPTEUR";
            }
        } // endProperty: LabelCompteur

        /// <summary>
        /// Le libellé pour la SIM
        /// </summary>
        public String LabelSIM
        {
            get
            {
                return "/MAJMOMT/SIM";
            }
        } // endProperty: LabelSIM

        /// <summary>
        /// Le libellé pour l'erreur SIM
        /// </summary>
        public String LabelErrorSim
        {
            get
            {
                return "/MAJMOMT/SIMERROR";
            }
        } // endProperty: LabelErrorSim
        
        public String MAJDATE
        {
            get
            {
                return "MAJMOMT/MAJDATE";
            }
        } // endProperty: MAJDATE
        /// <summary>
        /// Le VidPid du produit
        /// </summary>
        public VidPid ProductVidPid
        {
            get
            {
                return this._productVidPid;
            }
            private set
            {
                this._productVidPid = value;
                switch (this._productVidPid.Name.ToUpper())
                {
                    case Product.BETA:
                    case Product.GAMA:
                    case Product.PIKA:
                    case Product.MOKA:
                        this._typeProduct = ProductType.MO;
                        break;
                    case Product.ALTO:
                    case Product.ELIO:
                    case Product.TIMO:
                    case Product.NEMO:
                        this._typeProduct = ProductType.MT;
                        break;
                    case Product.SBC:
                        this._typeProduct = ProductType.SBC;
                        break;
                    default:
                        this._typeProduct = ProductType.Unknown;
                        break;
                }
            }
        } // endProperty: ProductVidPid

        /// <summary>
        /// La version du firmware
        /// </summary>
        public String FirmwareVersion
        {
            get
            {
                return this._firmwareVersion;
            }
            private set
            {
                this._firmwareVersion = value;
            }
        } // endProperty: FirmwareVersion
        public string HardwareConfig { get; private set; }

        #endregion

        // Constructeur 
        #region Constructeur

        /// <summary>
        /// Le constructeur ne connect pas le produit
        /// </summary>
        /// <param name="vidPid"></param>
        public ConnectedProduct(VidPid vidPid)
        {
            //System.Windows.MessageBox.Show("3.0");
            this.ProductVidPid = vidPid;
            //System.Windows.MessageBox.Show("3.1");
            this.Connect();
            String Result = "";
            String Result_hard = "";
            //System.Windows.MessageBox.Show("3.2");
            //System.Windows.MessageBox.Show("3.3");
            this._hidPort.GetFirmwareVersion(Hid.CIBLE_HID_e.CIBLE_CPU_0, ref Result);
            this._hidPort.GetHarwareInformation(ref Result_hard);

            // Charger l'heure du produit
            this.GetUTCTime();

            // définir la taille mémoire
            Hid.HidDll.TARGET_TYPE_s target;
            switch (this.TypeProduct)
	        {
		        case ProductType.Unknown:
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_UNKNOWN;
                    break;
                case ProductType.MO:
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_MO;
                    break;
                case ProductType.MT:
                    // Charger l'heure du produit
                    this.GetUTCTime();
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_MT;
                    break;
                case ProductType.SIM:
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_MT;
                    break;
                case ProductType.SBC:
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_UNKNOWN;
                    break;
                default:
                    target = Hid.HidDll.TARGET_TYPE_s.TARGET_UNKNOWN;
                    break;
	        }

            if (target != Hid.HidDll.TARGET_TYPE_s.TARGET_UNKNOWN)
            {
                Int32 MemSize = this._hidPort.TestMemorySize(target);
                switch (MemSize)
                {
                    case 1:
                        this.MemorySize = 32;
                        break;
                    case 2:
                        this.MemorySize = 128;
                        break;
                    default:
                        break;
                }
            }

            //System.Windows.MessageBox.Show("3.4");
            this.FirmwareVersion = Result;
            this.HardwareConfig = Result_hard;

            //System.Windows.MessageBox.Show("3.5");
            this.GetProductRef();
            //System.Windows.MessageBox.Show("3.6");
            this.InitCodeIDModuleAs();
            //System.Windows.MessageBox.Show("3.7");
            this.Disconnect();
            //System.Windows.MessageBox.Show("3.8");
            this.IsNotInMajFirmeware = true;
            //System.Windows.MessageBox.Show("3.9");
            this.CreateCommandMAJFirmawreClick();
            this.CreateCommandLectureJournal();
            this.CreateCommandReadInfo();
            this.CreateCommandCounter();
            this.CreateCommandMAJDateProduct();
            //System.Windows.MessageBox.Show("3.10");
            Mvvm.Messaging.Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
            //System.Windows.MessageBox.Show("3.11");
            RaisePropertyChanged("SimErrorReadVisibility");
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Mettre à jour la date et l'heure pour le produit
        /// </summary>
        public void MAJCurrentDateTime ( )
        {
            RaisePropertyChanged("CurrentUTCProduct");
            RaisePropertyChanged("CurrentLocalDateTime");
        } // endMethod: MAJCurrentDateTime

        /// <summary>
        /// Reception des messages
        /// </summary>
        /// <param name="message">
        /// Le message de commande
        /// </param>
        public void ReceiveMessage(CommandMessage message)
        {
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("MAJ_FirmWare");
                RaisePropertyChanged("LabelCompteur");
                RaisePropertyChanged("LabelJournal");
                RaisePropertyChanged("LabelModuleOperateur");
                RaisePropertyChanged("LabelTransceiver");
                RaisePropertyChanged("LabelSerialNumber");
                RaisePropertyChanged("LabelReference");
                RaisePropertyChanged("LabelMarket");
                RaisePropertyChanged("LabelCodeId");
                RaisePropertyChanged("LabelSIM");
                RaisePropertyChanged("LabelErrorSim");
            }
        }

        /// <summary>
        /// Connecter le produit
        /// </summary>
        public Boolean Connect ( )
        {
            Boolean Result = false;

            this._hidPort = new Hid.Hid();
            Result = this._hidPort.Connecter(this.ProductVidPid.Vid, this.ProductVidPid.Pid);

            return Result;
        } // endMethod: Connect
        
        /// <summary>
        /// Déconnecter le produit
        /// </summary>
        public Boolean Disconnect ( )
        {
            Boolean Result = false;

            if (this._hidPort != null)
            {
                this._hidPort.Deconnecter();
                Result = true;
            }
            
            return Result;
        } // endMethod: Disconnect
        
        /// <summary>
        /// Acquérir le point d'entré HID lié à ce produit
        /// </summary>
        public Hid.Hid GetHID ( )
        {
            Hid.Hid Result = this._hidPort;
            
            return Result;
        } // endMethod: GetHID

        /// <summary>
        /// Mettre à jour le firmware en utilisant le fichier indiqué
        /// </summary>
        public virtual Boolean MAJFirmware ( String Filename, bool InHid )
        {
            Boolean Result = false;

            if (this._hidPort == null)
            {
                this._hidPort = new Hid.Hid();
            }

            // si le produit n'est pas connecter, le connecter
            if (this.Connect() || !InHid)
            {
                // passer le produit en mode DFU
                //System.Windows.MessageBox.Show("Enter DFU -> Before");
                if (InHid)
                {
                    try
                    {
                        this._hidPort.EnterDfuMode();
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message, "ConnectedProduct");
                        throw;
                    }
                }
                
                //if (this._hidPort.EnterDfuMode())
                //{
                    // Uploader le fichier
                    //System.Windows.MessageBox.Show("Enter DFU -> After");
                    TimeHelper.Wait(DefaultValues.Get().WaitFlashTimeMs);
                    Pegase.Dfu.DFU dfu;
                    Pegase.Dfu.DFU.Error Err; 
                    try
                    {
                        //System.Windows.MessageBox.Show("Enter DFU -> True");
                        dfu = new Pegase.Dfu.DFU();
                        Helper.TimeHelper.Wait(DefaultValues.Get().WaitFlashTimeMs);
                        //System.Windows.MessageBox.Show("DFU -> OK");
                        Err = dfu.Connecter();
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message, "ConnectedProduct");
                        throw;
                    }
                    //System.Windows.MessageBox.Show(String.Format("Code Erreur {0}", Err));

                    if (Err == Pegase.Dfu.DFU.Error.NO_ERROR)
                    {
                        Err = dfu.Telecharger(Filename, true);
                        //System.Windows.MessageBox.Show("Téléchargé -> OK");
                        Result = true;
                        
                        dfu.Deconnecter();
                    }
                    else
                    {
                        switch (Err)
                        {
                            case Pegase.Dfu.DFU.Error.ERR_MORETHANONEDEVICECONNECTED:
                                System.Windows.MessageBox.Show("ERR_MORETHANONEDEVICECONNECTED");
                                break;
                            case Pegase.Dfu.DFU.Error.ERR_NODEVICECONNECTED:
                                System.Windows.MessageBox.Show("ERR_NODEVICECONNECTED");
                                break;
                            case Pegase.Dfu.DFU.Error.ERR_NODOWNLOADCAPABILITY:
                                System.Windows.MessageBox.Show("ERR_NODOWNLOADCAPABILITY");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_NO_ERROR:
                                System.Windows.MessageBox.Show("STDFU_NO_ERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_MEMORY:
                                System.Windows.MessageBox.Show("STDFU_MEMORY");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_BADPARAMETER:
                                System.Windows.MessageBox.Show("STDFU_BADPARAMETER");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_NOTIMPLEMENTED:
                                System.Windows.MessageBox.Show("STDFU_NOTIMPLEMENTED");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ENUMFINISHED:
                                System.Windows.MessageBox.Show("STDFU_ENUMFINISHED");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_OPENDRIVERERROR:
                                System.Windows.MessageBox.Show("STDFU_OPENDRIVERERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ERRORDESCRIPTORBUILDING:
                                System.Windows.MessageBox.Show("STDFU_ERRORDESCRIPTORBUILDING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_PIPECREATIONERROR:
                                System.Windows.MessageBox.Show("STDFU_PIPECREATIONERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_PIPERESETERROR:
                                System.Windows.MessageBox.Show("STDFU_PIPERESETERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_PIPEABORTERROR:
                                System.Windows.MessageBox.Show("STDFU_PIPEABORTERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_STRINGDESCRIPTORERROR:
                                System.Windows.MessageBox.Show("STDFU_STRINGDESCRIPTORERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_DRIVERISCLOSED:
                                System.Windows.MessageBox.Show("STDFU_DRIVERISCLOSED");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_VENDOR_RQ_PB:
                                System.Windows.MessageBox.Show("STDFU_VENDOR_RQ_PB");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ERRORWHILEREADING:
                                System.Windows.MessageBox.Show("STDFU_ERRORWHILEREADING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ERRORBEFOREREADING:
                                System.Windows.MessageBox.Show("STDFU_ERRORBEFOREREADING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ERRORWHILEWRITING:
                                System.Windows.MessageBox.Show("STDFU_ERRORWHILEWRITING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_ERRORBEFOREWRITING:
                                System.Windows.MessageBox.Show("STDFU_ERRORBEFOREWRITING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_DEVICERESETERROR:
                                System.Windows.MessageBox.Show("STDFU_DEVICERESETERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_CANTUSEUNPLUGEVENT:
                                System.Windows.MessageBox.Show("STDFU_CANTUSEUNPLUGEVENT");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_INCORRECTBUFFERSIZE:
                                System.Windows.MessageBox.Show("STDFU_INCORRECTBUFFERSIZE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_DESCRIPTORNOTFOUND:
                                System.Windows.MessageBox.Show("STDFU_DESCRIPTORNOTFOUND");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_PIPESARECLOSED:
                                System.Windows.MessageBox.Show("STDFU_PIPESARECLOSED");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_PIPESAREOPEN:
                                System.Windows.MessageBox.Show("STDFU_PIPESAREOPEN");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFU_TIMEOUTWAITINGFORRESET:
                                System.Windows.MessageBox.Show("STDFU_TIMEOUTWAITINGFORRESET");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_ERROR_OFFSET:
                                System.Windows.MessageBox.Show("STDFUPRT_ERROR_OFFSET");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_UNABLETOLAUNCHDFUTHREAD:
                                System.Windows.MessageBox.Show("STDFUPRT_UNABLETOLAUNCHDFUTHREAD");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_DFUALREADYRUNNING:
                                System.Windows.MessageBox.Show("STDFUPRT_DFUALREADYRUNNING");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_BADPARAMETER:
                                System.Windows.MessageBox.Show("STDFUPRT_BADPARAMETER");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_BADFIRMWARESTATEMACHINE:
                                System.Windows.MessageBox.Show("STDFUPRT_BADFIRMWARESTATEMACHINE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_UNEXPECTEDERROR:
                                System.Windows.MessageBox.Show("STDFUPRT_UNEXPECTEDERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_DFUERROR:
                                System.Windows.MessageBox.Show("STDFUPRT_DFUERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_RETRYERROR:
                                System.Windows.MessageBox.Show("STDFUPRT_RETRYERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUPRT_UNSUPPORTEDFEATURE:
                                System.Windows.MessageBox.Show("STDFUPRT_UNSUPPORTEDFEATURE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_ERROR_OFFSET:
                                System.Windows.MessageBox.Show("STDFUFILES_ERROR_OFFSET");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_BADSUFFIX:
                                System.Windows.MessageBox.Show("STDFUFILES_BADSUFFIX");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_UNABLETOOPENFILE:
                                System.Windows.MessageBox.Show("STDFUFILES_UNABLETOOPENFILE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_UNABLETOOPENTEMPFILE:
                                System.Windows.MessageBox.Show("STDFUFILES_UNABLETOOPENTEMPFILE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_BADFORMAT:
                                System.Windows.MessageBox.Show("STDFUFILES_BADFORMAT");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_BADADDRESSRANGE:
                                System.Windows.MessageBox.Show("STDFUFILES_BADADDRESSRANGE");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_BADPARAMETER:
                                System.Windows.MessageBox.Show("STDFUFILES_BADPARAMETER");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_UNEXPECTEDERROR:
                                System.Windows.MessageBox.Show("STDFUFILES_UNEXPECTEDERROR");
                                break;
                            case Pegase.Dfu.DFU.Error.STDFUFILES_FILEGENERALERROR:
                                System.Windows.MessageBox.Show("STDFUFILES_FILEGENERALERROR");
                                break;
                            default:
                                break;
                        }
                    }
                //}

                this.Disconnect();
            }
            
            return Result;
        } // endMethod: MAJFirmware
        
        /// <summary>
        /// Récupérer le code ID du produit connecté
        /// </summary>
        private String GetCodeID ( )
        {
            String Result;
            Byte[] buffer = new Byte[4];
            UInt32 adresse = 0x0000;

            if (this._typeProduct == ProductType.MO)
            {
                adresse = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("AdrCodeIDMO"), 16);
            }
            else if (this._typeProduct == ProductType.MT)
            {
                adresse = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("AdrCodeIDMT"), 16);
            }

            Result = this.GetEEPROMValue(adresse, NB_OCTET_CODEID, Hid.CIBLE_HID_e.CIBLE_EEP_0);

            return Result;
        } // endMethod: GetCodeID
        
        /// <summary>
        /// Acquérir les références du produit
        /// </summary>
        public void GetProductRef ( )
        {
            this.CodeID = this.GetCodeID();

            if (this.TypeProduct == ProductType.MT)
            {
                this.IsMT = true;
                this.IsMO = false;
                this.CodeIDMT = this.CodeID;
                this.MarketCodeMT = this.ConvertToMarket(this.GetEEPROMValue(ADRESSE_CODE_MARCHE_MT, NB_OCTET_CODE_MARCHE, Hid.CIBLE_HID_e.CIBLE_EEP_0));
                this.SerialNumberMT = this.GetEEPROMASCIIValue(ADRESSE_SERIAL_MT, NB_OCTET_SERIAL_MT, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.RefMarcheSim = this.GetEEPROMASCIIValue(ADRESSE_MARCHE_SIM, NB_OCTET_REFMARCHESIM, Hid.CIBLE_HID_e.CIBLE_SIM);
                this.RefCommercialSIM = this.GetEEPROMASCIIValue(ADRESSE_REFCOMMERCIALE_SIM, NB_OCTET_REFCOMSIM, Hid.CIBLE_HID_e.CIBLE_SIM);
                this.SerialNumberSIM = this.GetEEPROMASCIIValue(ADRESSE_SERIAL_SIM_MT, NB_OCTET_SERIAL_SIM, Hid.CIBLE_HID_e.CIBLE_SIM);
                string tmp = this.GetEEPROMValue(ADRESSE_PLD_MT, NB_OCTET_PLD_MT, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.TypePldLevel = Convert.ToUInt32(this.GetEEPROMValue(ADRESSE_PLD_MT, NB_OCTET_PLD_MT, Hid.CIBLE_HID_e.CIBLE_EEP_0),16);
            }
            else
            if (this.TypeProduct == ProductType.MO)
            {
                this.IsMT = false;
                this.IsMO = true;
                this.CodeIDMO = this.CodeID;
                this.MarketCodeMO = this.ConvertToMarket(this.GetEEPROMValue(ADRESSE_CODE_MARCHE_MO, NB_OCTET_CODE_MARCHE, Hid.CIBLE_HID_e.CIBLE_EEP_0));
                this.SerialNumberMO = this.GetEEPROMASCIIValue(ADRESSE_SERIAL_MO, NB_OCTET_SERIAL_MO, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.SerialNumberSIM = this.GetEEPROMASCIIValue(ADRESSE_SERIAL_SIM_MO, NB_OCTET_SERIAL_SIM, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.TypePldLevel = Convert.ToUInt32(this.GetEEPROMValue(ADRESSE_PLD_MT, NB_OCTET_PLD_MT, Hid.CIBLE_HID_e.CIBLE_EEP_0),16);
            }
        } // endMethod: GetProductRef

        /// <summary>
        /// Convertir une chaine de charactère en code marché
        /// </summary>
        public String ConvertToMarket ( String HexaString )
        {
            String Result;
            Int32 Value = Convert.ToInt32(HexaString, 16);
            Value = Value + 25;

            if ((Value < 99) && (Value>0))
            {
                Result = "" + Convert.ToChar(Value); 
            }
            else
            {
                Result = "-";
            }
 
            return Result;
        } // endMethod: ConvertToMarket

        /// <summary>
        /// Lire une valeur dans l'EEPROM
        /// </summary>
        /// <returns>
        /// Renvoie une valeur Hexadécimal sous forme d'une chaine de caractères
        /// </returns>
        public String GetEEPROMValue ( UInt32 adresse, uint NbOctet, Hid.CIBLE_HID_e cible )
        {
            String Result;
            Byte[] buffer = new Byte[NbOctet];

            // Connecter le produit
            this.Connect();
            //
            this._hidPort.LireBloc(cible, buffer, adresse, NbOctet);
            // Déconnecter après la lecture
            this.Disconnect();

            Result = "";

            for (Int32 i = (Int32)NbOctet - 1; i >= 0; i--)
            {
                String tmp = Convert.ToString(buffer[i], 16);
                if (tmp.Length == 1)
                {
                    tmp = "0" + tmp;
                }
                Result += tmp;
            }

            return Result;
        } // endMethod: GetEEPROMValue

        /// <summary>
        /// Lire une valeur ASCII dans l'EEPROM
        /// </summary>
        public String GetEEPROMASCIIValue(UInt32 adresse, uint NbOctet, Hid.CIBLE_HID_e cible)
        {
            String Result;
            Byte[] buffer = new Byte[NbOctet];

            // Connecter le produit
            this.Connect();
            //
            this._hidPort.LireBloc(cible, buffer, adresse, NbOctet);
            // Déconnecter après la lecture
            this.Disconnect();

            Result = "";

            for (Int32 i = 0; i < NbOctet; i++)
            {
                if (buffer[i] < 0xFF)
                {
                    Result += (Char)buffer[i];
                }
                else
                {
                    break;
                }
            }

            return Result;
        } // endMethod: GetEEPROMValue

        /// <summary>
        /// Supprimer les données d'un produit à l'exception des 16 premiers octets réservés à la prod (banc de test)
        /// </summary>
        public Boolean EraseProduct( )
        {
            Boolean Result = false;
            Byte[] EraseBuffer = new Byte[32736];

            // 1 - Initialiser le buffer
            for (int i = 0; i < EraseBuffer.Length; i++)
            {
                EraseBuffer[i] = 0xFF;
            }

            // 2 - Charger le contenu du buffer dans les cibles
            this.Connect();
            Result = this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, EraseBuffer, 0, 8);
            Result = this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, EraseBuffer, 0, 8);
            Result = this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, EraseBuffer, 0x7600, 512 + 512 + 2048);
            Result = this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, EraseBuffer, 0x7600, 512 + 512 + 2048);

            
            
            if ((JAY.DefaultValues.Get().OptimisationProgMo == true) && (this.IsMO == true))
            {
                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, EraseBuffer, 32, 400);
            }
            else if ((JAY.DefaultValues.Get().OptimisationProgMt == true) && (this.IsMT == true))
            {
                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, EraseBuffer, 32, 400);
            }
            else
            {
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, EraseBuffer, 32, 32736);
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, EraseBuffer, 32, 32736);
            }
            Result = this.Disconnect();

            return Result;
        } // endMethod: EraseProduct
        
        /// <summary>
        /// Inscrire B0B0B0 dans le produit, à l'adresse 0
        /// </summary>
        public Boolean SetB0B0B0 ( )
        {
            Boolean Result = false;
            Byte[] OKBuffer = new Byte[4];
            Byte[] CopyBuffer = new Byte[4];


            // 1 - Initialiser le buffer
            for (int i = 0; i < 4; i++)
            {
                OKBuffer[i] = 0xB0;
            }
            CopyBuffer[3] = 0xFA;
            CopyBuffer[2] = 0xB0;
            CopyBuffer[1] = 0xFE;
            CopyBuffer[0] = 0xED;

            // 2 - Charger les valeurs à l'adresse 0
            this.Connect();


            if ((JAY.DefaultValues.Get().OptimisationProgMo) && (this.IsMO == true))
                {
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, OKBuffer, 0x0, 4);
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, CopyBuffer, 0x0, 4);
                }
                else if ((JAY.DefaultValues.Get().OptimisationProgMt)  && (this.IsMT == true))
                {
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, CopyBuffer, 0x0, 4);
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, CopyBuffer, 0x0, 4);
                }
                else
                {
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, OKBuffer, 0x0, 4);
                    this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, OKBuffer, 0x0, 4);
                }
            this.Disconnect();

            return Result;
        } // endMethod: SetB0B0B0
        
        /// <summary>
        /// Ecrire les valeurs contenues dans buffer
        /// </summary>
        public void SetValue ( Hid.CIBLE_HID_e cible, Byte[] buffer, UInt32 adresse )
        {
            this.Connect();
            this._hidPort.EcrireBloc(cible, buffer, adresse, (UInt32)buffer.Count());
            this.Disconnect();
        } // endMethod: SetValue

        /// <summary>
        /// Fixer la date du produit
        /// </summary>
        public Boolean SetDate ( )
        {
            Boolean Result;
            System.Threading.Thread.Sleep(1000);

            DateTime Origine = new DateTime(Constantes.RefYear, Constantes.RefMounth, Constantes.RefDay);
            Origine = Origine.ToUniversalTime();
            DateTime Now = DateTime.UtcNow;
            TimeSpan Ellapsed = Now - Origine;
            UInt32 date = (UInt32)Ellapsed.TotalSeconds;

            Result = this.Connect();
            if (!Result)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(String.Format("Erreur de connection au produit"), "Attention");
            }
            System.Threading.Thread.Sleep(1000);
            Result = this._hidPort.SetDate(date);
            if (!Result)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(String.Format("Erreur de mise a l'heure"), "Attention");
            }
            System.Threading.Thread.Sleep(1000);
            Result = this.Disconnect();
            if (!Result)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(String.Format("Erreur de deconnection"), "Attention");
            }
            this._utcProduct = Now;
            this._readUTCDateTime = DateTime.UtcNow;

            return Result;
        } // endMethod: SetDate
        public bool CheckTime()
        {
            DateTime Origine = new DateTime(Constantes.RefYear, Constantes.RefMounth, Constantes.RefDay);
            Origine = Origine.ToUniversalTime();
            DateTime Now = DateTime.UtcNow;
            TimeSpan Ellapsed = Now - Origine;
            UInt32 date = (UInt32)Ellapsed.TotalSeconds;
            UInt32 TimeInSec = GetTime();
            UInt32 result = 100;
            if (TimeInSec> date)
            {
                result = TimeInSec - date;
            }
            else
            {
                result = date - TimeInSec;
            }
            if (result >10)
            { return false; }
            return true;
        }
        public UInt32 GetTime()
        {
            this.Connect();
            System.Threading.Thread.Sleep(10000);
            UInt32 TimeInSec = this._hidPort.GetDate();
            System.Threading.Thread.Sleep(10000);
            this.Disconnect();
            return TimeInSec;
        }
        /// <summary>
        /// Retourner le temps UTC du timer du produit
        /// </summary>
        public DateTime GetUTCTime ( )
        {
            UInt32 TimeInSec = this._hidPort.GetDate();

            DateTime time = new DateTime(Constantes.RefYear, Constantes.RefMounth, Constantes.RefDay);
            time = time.ToUniversalTime();
            Int32 heures, minutes, seconde;

            heures = (Int32)(TimeInSec / 3600);
            minutes = (Int32)((TimeInSec - (UInt32)heures * 3600) / 60);
            seconde = (Int32)((TimeInSec - (UInt32)heures * 3600 - (UInt32)minutes * 60));

            TimeSpan timeS = new TimeSpan(heures, minutes, seconde);
            time += timeS;
            this._utcProduct = time;
            this._readUTCDateTime = DateTime.UtcNow;

            return time;
        } // endMethod: GetUTCTime

        /// <summary>
        /// Récupérer les codes ID des produits associés
        /// </summary>
        private void InitCodeIDModuleAs ( )
        {
            Byte[] buffer = new Byte[4];
            UInt32 adresse = 0x0000;
            UInt32 adresseParamIDialog;

            adresseParamIDialog = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("AdrParamIDialog"), 16);

            if (this.TypeProduct == ProductType.MO)
            {
                // initialisation dans le cas d'un MO
                this.CodeIDMO = this.CodeID;
                adresse = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("AdrCodeIDMTdansMO"), 16);
                this.CodeIDMT = this.GetEEPROMValue(adresse, NB_OCTET_CODEID, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.ParamIDialog = this.GetEEPROMASCIIValue(adresseParamIDialog, NB_OCTET_PARAMIDIALOG, Hid.CIBLE_HID_e.CIBLE_EEP_0);
            }
            else if(this.TypeProduct == ProductType.MT)
            {
                // initialisation dans le cas d'un MT
                this.CodeIDMT = this.CodeID;
                adresse = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("AdrCodeIDMOdansMT"), 16);
                this.CodeIDMO = this.GetEEPROMValue(adresse, NB_OCTET_CODEID, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                this.ParamIDialog = this.GetEEPROMASCIIValue(adresseParamIDialog, NB_OCTET_PARAMIDIALOG, Hid.CIBLE_HID_e.CIBLE_EEP_0);
            }
            else
            {
                // initialisation dans un autre cas
                this.CodeIDMO = "";
                this.CodeIDMT = "";
                return;
            }
        } // endMethod: GetCodeIDModuleAs
        
        /// <summary>
        /// Acquérir les compteurs de ce produit
        /// </summary>
        public void GetCounters ( BackgroundWorker worker )
        {
            String AdrStr, Value;
            UInt32 Adresse, value;

            this.ProgressValue = 0;
            if (this.TypeProduct == ProductType.MO)
            {
                // Récupérer l'adresse de base de la section MO
                try
                {
                    AdrStr = ConfigurationReader.Instance.GetValue("AdrCounterMO");
                    Adresse = Convert.ToUInt32(AdrStr, 16);
                }
                catch
                {
                    Adresse = 0x7670;
                }
                // Lire la table des compteurs par organ
                this._mo_NombreManoeuvre = new ObservableCollection<UInt32>();

                for (int i = 0; i < 48; i++)
                {
                    Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_1);
                    value = Convert.ToUInt32(Value, 16);
                    Adresse += 4;
                    this._mo_NombreManoeuvre.Add(value);
                    this.ProgressValue = (i * 100) / 48;
                    worker.WorkerReportsProgress = true;
                    worker.ReportProgress(this.ProgressValue);
                }
                this.MO_NombreManoeuvre = this._mo_NombreManoeuvre;
                // Nombre de choc
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MO_NombreChoc = value;
                // Nombre d'alarme
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_1);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MO_NombreAlarme = value;
                // Temps absolu
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_1);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MO_TempsAbsolu = value;
            }
            else if (this.TypeProduct == ProductType.MT)
            {
                // Récupérer l'adresse de base de la section MT
                try
                {
                    AdrStr = ConfigurationReader.Instance.GetValue("AdrCounterMT");
                    Adresse = Convert.ToUInt32(AdrStr, 16);
                }
                catch
                {
                    Adresse = 0x7610;
                }
                // Lire nombre de manoeuvre sécu maitre
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MT_SecuMaitre_NbManoeuvre = value;
                // Lire le temps d'activité sécu maitre
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MT_SecuMaitre_TempsActif = value;
                // Lire le nombre de manoeuvre sécu esclave
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_1);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MT_SecuEsclave_NbManoeuvre = value;
                // Lire le temps d'activité esclave sécu esclave
                Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_1);
                value = Convert.ToUInt32(Value, 16);
                Adresse += 4;
                this.MT_SecuEsclave_TempsActif = value;
                this.MT_Relais_NbManoeuvre = new ObservableCollection<UInt32>();
                this.MT_Relais_TempsActif = new ObservableCollection<UInt32>();
                for (int i = 0; i < 38; i++)
                {
                    Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                    value = Convert.ToUInt32(Value, 16);
                    Adresse += 4;
                    this.MT_Relais_NbManoeuvre.Add(value);
                    Value = this.GetEEPROMValue(Adresse, 4, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                    value = Convert.ToUInt32(Value, 16);
                    Adresse += 4;
                    this.MT_Relais_TempsActif.Add(value);
                    this.ProgressValue = (i * 100) / 38;
                    worker.WorkerReportsProgress = true;
                    worker.ReportProgress(this.ProgressValue);
                }
            }
        } // endMethod: GetCounters

        /// <summary>
        /// Assigné la valeur assurant que la mise à jour des produits c'est bien passé
        /// </summary>
        public Boolean SetFlagsConfigOK( )
        {
            Boolean Result = false;

            // Ne le faire que si le produit connecté est un MO
            if (this.TypeProduct == ProductType.MO)
            {
                this._hidPort.Connecter(this.ProductVidPid.Vid, this.ProductVidPid.Pid);
                UInt32 adresse = 0x0000;    // L'adresse d'écriture
                UInt32 Flags = 0xB0B0B0B0;
                Byte[] buffer = new Byte[4];

                String StrValue = Convert.ToString(Flags, 16);
                int j = 3;

                for (int i = 0; i < StrValue.Length - 1; i += 2)
                {
                    buffer[j] = Convert.ToByte(StrValue.Substring(i, 2), 16);
                    j -= 1;
                }

                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, buffer, adresse, 4);
                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, buffer, adresse, 4);
            }

            return Result;
        } // endMethod: SetFlagsConfigOK

        /// <summary>
        /// Placer les flags obligeant une mise à jour depuis le MO
        /// </summary>
        public Boolean SetFlagsMAJFromMO(Boolean Param_Modifiable, Boolean Param_FixeMT, Boolean Param_FixeMo, Boolean Param_FixeMOExt1, Boolean Param_FixeMOExt2, Boolean Param_FixeMOExt3)
        {
            Boolean Result = false;

            // Ne le faire que si le produit connecté est un MO
            if (this.TypeProduct == ProductType.MO)
            {
                this._hidPort.Connecter(this.ProductVidPid.Vid, this.ProductVidPid.Pid);
                UInt32 adresse = 0x0000;    // L'adresse d'écriture
                UInt32 Masque, PModifiable, PFixeMT, PFixeMO, PFixeMOExt1,PFixeMOExt2,PFixeMOExt3, Value;
                Byte[] buffer = new Byte[4];

                Masque = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MARQUEUR_PARAMETRAGE_USB"), 16);
                PModifiable = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_MODIFIABLES"), 16);
                PFixeMT = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_FIXES_MT"), 16);
                PFixeMO = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_FIXES_MO"), 16);
                PFixeMOExt1 = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_FIXES_MO_EXT1"), 16);
                PFixeMOExt2 = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_FIXES_MO_EXT2"), 16);
                PFixeMOExt3 = Convert.ToUInt32(ConfigurationReader.Instance.GetValue("MASQUE_ZONE_PARAM_FIXES_MO_EXT3"), 16);

                Value = Masque;
                if (Param_Modifiable)
                {
                    Value |= PModifiable;
                }
                if (Param_FixeMT)
                {
                    Value |= PFixeMT;
                }
                if (Param_FixeMo)
                {
                    Value |= PFixeMO;
                }
                if (Param_FixeMOExt1)
                {
                    Value |= PFixeMOExt1;
                }
                if (Param_FixeMOExt2)
                {
                    Value |= PFixeMOExt2;
                }
                if (Param_FixeMOExt3)
                {
                    Value |= PFixeMOExt3;
                }

                String StrValue = Convert.ToString(Value, 16);
                int j = 3;

                for (int i = 0; i < StrValue.Length - 1; i+=2)
                {
                    buffer[j] = Convert.ToByte(StrValue.Substring(i, 2), 16);
                    j -= 1;
                }

                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, buffer, adresse, 4);
                this._hidPort.EcrireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_1, buffer, adresse, 4);
            }

            return Result;
        } // endMethod: SetFlagsMAJFromMO

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandCounter
        /// <summary>
        /// La commande Counter
        /// </summary>
        public ICommand CommandCounter
        {
            get;
            internal set;
        } // endProperty: CommandCounter

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCounter()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCounter = new Mvvm.Command.RelayCommand(ExecuteCommandCounter, CanExecuteCommandCounter);
        } // endMethod: CreateCommandCounter

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCounter()
        {
            //Envoyer un Message à ViewModelMajMoMt pour que la lecture des compteurs soit lancée
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_LIRECOUNTER));
        } // endMethod: ExecuteCommandCounter

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCounter()
        {
            Boolean Result = false;

            if (DefaultValues.Get().UserMode == DefaultValues.EXPERT)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandCounter 
        #endregion


        #region CommandLectureJournal
        /// <summary>
        /// La commande LectureJournal
        /// </summary>
        public ICommand CommandLectureJournal
        {
            get;
            internal set;
        } // endProperty: CommandLectureJournal
        public ICommand CommandReadInfo
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandLectureJournal()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandLectureJournal = new Mvvm.Command.RelayCommand(ExecuteCommandLectureJournal, CanExecuteCommandLectureJournal);
        } // endMethod: CreateCommandLectureJournal
        
        private void CreateCommandReadInfo()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandReadInfo = new Mvvm.Command.RelayCommand(ExecuteCommandReadInfo, CanExecuteCommandReadInfo);
        }
        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandLectureJournal()
        {
            //Envoyer un Message à ViewModelMajMoMt pour que la mise à jour soit lancée
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_LIREJOURNAL));
        } // endMethod: ExecuteCommandLectureJournal
        public void ExecuteCommandReadInfo()
        {
            //Envoyer un Message à ViewModelMajMoMt pour que la mise à jour soit lancée
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_READINFO));
        }
        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandLectureJournal()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandLectureJournal 
        public Boolean CanExecuteCommandReadInfo()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        }
        #endregion

        #region CommandMAJFirmawreClick
        /// <summary>
        /// La commande MAJFirmawreClick
        /// </summary>
        public ICommand CommandMAJFirmawreClick
        {
            get;
            internal set;
        } // endProperty: CommandMAJFirmawreClick

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandMAJFirmawreClick()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            
            CommandMAJFirmawreClick = new Mvvm.Command.RelayCommand(ExecuteCommandMAJFirmawreClick, CanExecuteCommandMAJFirmawreClick);
        } // endMethod: CreateCommandMAJFirmawreClick

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandMAJFirmawreClick()
        {
            //Envoyer un Message à ViewModelMajMoMt pour que la mise à jour soit lancée
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_MAJFIRMWARE));
        } // endMethod: ExecuteCommandMAJFirmawreClick

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandMAJFirmawreClick()
        {
            return this.IsNotInMajFirmeware;
        } // endMethod: CanExecuteCommandMAJFirmawreClick

        #endregion
        #region CommandMAJDateProduct
        /// <summary>
        /// La commande MAJDateProduct
        /// </summary>
        public ICommand CommandMAJDateProduct
        {
            get;
            internal set;
        } // endProperty: CommandMAJDateProduct

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandMAJDateProduct()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandMAJDateProduct = new Mvvm.Command.RelayCommand(ExecuteCommandMAJDateProduct, CanExecuteCommandMAJDateProduct);
        } // endMethod: CreateCommandMAJDateProduct

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandMAJDateProduct()
        {
            // Pour tous les MT connectés
            if (this != null)
            {
                this.SetDate();
            }
        } // endMethod: ExecuteCommandMAJDateProduct

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandMAJDateProduct()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandMAJDateProduct 
        #endregion
        #endregion
    } // endClass: ConnectedProduct
}
