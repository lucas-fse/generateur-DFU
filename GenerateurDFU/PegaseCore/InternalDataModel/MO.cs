using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;
using JAY;
using System.Text.RegularExpressions;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe contenant toutes les données d'un MO ainsi que son fonctionnement
    /// </summary>
    public class MO
    {
        #region Constantes

        public const String B = "B";
        public const String G = "G";
        public const String M = "M";
        public const String P = "P";
        public const String B2 = "B2";
        public const String B6 = "B6";
        public const String G6 = "G6";
        public const String G1 = "G1";
        public const String P0 = "P0";
        public const String P1 = "P1";
        public const String P2 = "P2";
        public const String M1 = "M1";
        public const String M2 = "M2";
        public const String M3 = "M3";
        public const String M6 = "M6";
        public const String BETA2 = "Beta 2";
        public const String BETA6 = "Beta 6";
        public const String GAMA6 = "Gama 6";
        public const String GAMA10 = "Gama 10";
        public const String PIKA0 = "Pika 0";
        public const String PIKA1 = "Pika 1";
        public const String PIKA2 = "Pika 2";
        public const String MOKA1 = "Moka 1";
        public const String MOKA2 = "Moka 2";
        public const String MOKA3 = "Moka 3";
        public const String MOKA6 = "Moka 6";
        public const String BETA = "Beta";
        public const String GAMA = "Gama";
        public const String PIKA = "Pika";
        public const String MOKA = "Moka";

        #endregion

        // Variables
        #region Variables

        private XElement _xmlRoot;
        private XMLProcessing _xmlProcessing;
        private ObservableCollection<OrganCommand> _organesCommandes;
        private Int32 _presenceIR;
        private Int32 _presenceAccelerometre;
        private Int32 _presenceVibreur;
        private ObservableCollection<Option> _options;
        private Int32 _presenceBuzzer;
        private Int32 _presenceAuxiliaire;
        private Int32 _presenceSafetyButton;
        private Int32 _presenceliaisonfilaire;
        private Int32 _presenceBAUGris;
        private Int32 _presenceGalva;
        #endregion

        // Propriétés
        #region Propriétés


        public Int32 Presenceliaisonfilaire
        {
            get
            {
                return _presenceliaisonfilaire;
            }
            set
            {
                _presenceliaisonfilaire = value;
            }
        }
        public Int32 PresenceBAUGris
        {
            get
            {
                return _presenceBAUGris;
            }
            set
            {
                _presenceBAUGris = value;
            }
        }
        public Int32 PresenceGalva
        {
            get
            {
                return _presenceGalva;
            }
            set
            {
                _presenceGalva = value;
            }
        }
        /// <summary>
        /// Au moins un bouton auxiliaire est-il présent?
        /// </summary>
        public Int32 PresenceAuxiliaire
        {
            get
            {
                return this._presenceAuxiliaire;
            }
            set
            {
                this._presenceAuxiliaire = value;
            }
        } // endProperty: PresenceAuxiliaire

        public Int32 PresenceSafetyButton
        {
            get
            {
                return this._presenceSafetyButton;
            }
            set
            {
                this._presenceSafetyButton = value;
            }
        } // endProperty: PresenceAuxiliaire

        /// <summary>
        /// La référence industrielle du produit
        /// </summary>
        public String RefIndus
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/IdentificationProduit/MO/ConfigMat/IdentificationERP/RefIndus", "", "", XML_ATTRIBUTE.VALUE);
                Result = XMLCore.Tools.ConvertFromASCII2Text(Result);

                return Result;
            }
        } // endProperty: RefIndus

        /// <summary>
        /// La fréquence du MO
        /// </summary>
        public String Frequence
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/FreqMO", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
        } // endProperty: Frequence

        /// <summary>
        /// Les options matérielles liées au MO
        /// </summary>
        public ObservableCollection<Option> OptionsMateriel
        {
            get
            {
                if (this._options == null)
                {
                    this._options = new ObservableCollection<Option>();
                }
                return this._options;
            }
            set
            {
                this._options = value;
            }
        } // endProperty: Options

        /// <summary>
        /// La liste des organes de commandes
        /// </summary>
        public ObservableCollection<OrganCommand> OrganesCommandes
        {
            get
            {
                if (this._organesCommandes == null)
                {
                    this._organesCommandes = new ObservableCollection<OrganCommand>();
                }
                return this._organesCommandes;
            }
            private set
            {
                this._organesCommandes = value;
            }
        } // endProperty: OrganesCommandes

        /// <summary>
        /// Le numéro de série
        /// </summary>
        public String SerialNumber
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("DonneesDeFab/NumeroDeSerie", "", "", XML_ATTRIBUTE.VALUE);
                SV = Tools.ConvertFromASCII2Text(SV);

                return SV;
            }
        } // endProperty: SerialNumber

        /// <summary>
        /// L'ID unique
        /// </summary>
        public String UniqueID
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("DonneesDeFab/CodeIdNatifMO", "", "", XML_ATTRIBUTE.VALUE);
                Int32 conv;
                try
                {
                    conv = Convert.ToInt32(SV, 16);
                }
                catch
                {
                    conv = 0;
                }
                SV = String.Format("{0:x5}", conv);

                return SV;
            }
        } // endProperty: UniqueID


        /// <summary>
        /// Le groupe d'appartenance du MO (Beta, Gama, Pika, Moka)
        /// </summary>
        public String GroupType
        {
            get;
            set;
        } // endProperty: GroupType

        /// <summary>
        /// Le type de MO
        /// </summary>
        public String TypeMO
        {
            get
            {
                string SV = "";
                String SV1 = PegaseData.Instance.XMLFile.GetValue("XmlTracabilite/MORef", "", "", XML_ATTRIBUTE.VALUE);
                String SV2 = this._xmlProcessing.GetValue("ConfigMat/IdentificationERP/RefIndus", "", "", XML_ATTRIBUTE.VALUE);
                SV2 = Tools.ConvertFromASCII2Text(SV2);

                if (SV1.Length > 2)
                {
                    SV1 = SV1.Substring(0, 2);
                    _typeMOShort = SV1;
                    switch (SV1)
                    {
                        case B2:
                            SV = BETA2;
                            this.GroupType = BETA;
                            
                            break;
                        case B6:
                            SV = BETA6;
                            this.GroupType = BETA;
                            break;
                        case G6:
                            SV = GAMA6;
                            this.GroupType = GAMA;
                            break;
                        case G1:
                            SV = GAMA10;
                            this.GroupType = GAMA;
                            break;
                        case P0:
                            SV = PIKA0;
                            this.GroupType = PIKA;
                            break;
                        case P1:
                            SV = PIKA1;
                            this.GroupType = PIKA;
                            break;
                        case P2:
                            SV = PIKA2;
                            this.GroupType = PIKA;
                            break;
                        case M1:
                            SV = MOKA2;
                            this.GroupType = MOKA;
                            break;
                        case M2:
                            SV = MOKA2;
                            this.GroupType = MOKA;
                            break;
                        case M3:
                            SV = MOKA3;
                            this.GroupType = MOKA;
                            break;
                        case M6:
                            SV = MOKA6;
                            this.GroupType = MOKA;
                            break;
                        default:
                            SV = LanguageSupport.Get().COMMON_FIELD_UNKNOW;
                            break;
                    }
                }
                else if(SV2.Length > 2)
                {
                    SV2 = SV2.Substring(0, 2);
                    _typeMOShort = SV2;
                    switch (SV2)
                    {
                        case B2:
                            SV = BETA2;
                            this.GroupType = BETA;
                            break;
                        case B6:
                            SV = BETA6;
                            this.GroupType = BETA;
                            break;
                        case G6:
                            SV = GAMA6;
                            this.GroupType = GAMA;
                            break;
                        case G1:
                            SV = GAMA10;
                            this.GroupType = GAMA;
                            break;
                        case P0:
                            SV = PIKA0;
                            this.GroupType = PIKA;
                            break;
                        case P1:
                            SV = PIKA1;
                            this.GroupType = PIKA;
                            break;
                        case P2:
                            SV = PIKA2;
                            this.GroupType = PIKA;
                            break;
                        case M1:
                            SV = MOKA2;
                            this.GroupType = MOKA;
                            break;
                        case M2:
                            SV = MOKA2;
                            this.GroupType = MOKA;
                            break;
                        case M3:
                            SV = MOKA3;
                            this.GroupType = MOKA;
                            break;
                        case M6:
                            SV = MOKA6;
                            this.GroupType = MOKA;
                            break;
                        default:
                            SV = LanguageSupport.Get().COMMON_FIELD_UNKNOW;
                            break;
                    }
                }
                else
                {
                    SV =  PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/TypeMO", "", "", XML_ATTRIBUTE.VALUE);
                    if (SV != null)
                    {
                        if (SV.Length > 1)
                        {
                            _typeMOShort = SV;
                            SV = SV.Substring(0, 1);
                            switch (SV)
                            {
                                case "B":
                                    SV = BETA2;
                                    this.GroupType = BETA;
                                    break;
                                case "G":
                                    SV = GAMA6;
                                    this.GroupType = GAMA;
                                    break;
                                case "M":
                                    SV = MOKA2;
                                    this.GroupType = MOKA;
                                    break;
                                case "P":
                                    SV = PIKA1;
                                    this.GroupType = PIKA;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                return SV;
            }
        } // endProperty: TypeMO

        private string _typeMOShort = "";
        public String TypeMOShort
        {
            get
            {
                string miseajour = TypeMO;
                return _typeMOShort;
            }
            private set
            {
                _typeMOShort = value;
            }
        }
        /// <summary>
        /// Présence de l'IR retenue
        /// </summary>
        public Int32 PresenceIR
        {
            get
            {
                return this._presenceIR;
            }
            set
            {
                this._presenceIR = value;
            }
        } // endProperty: IsIR

        /// <summary>
        /// Présence du vibreur
        /// </summary>
        public Int32 PresenceVibreur
        {
            get
            {
                return this._presenceVibreur;
            }
            set
            {
                this._presenceVibreur = value;
            }
        } // endProperty: PresenceVibreur

        /// <summary>
        /// Presence de l'auxiliaire
        /// </summary>
        public Int32 PresenceAccelerometre
        {
            get
            {
                return this._presenceAccelerometre;
            }
            set
            {
                this._presenceAccelerometre = value;
            }
        } // endProperty: PresenceAuxiliaire

        /// <summary>
        /// La présence du buzzer
        /// </summary>
        public Int32 PresenceBuzzer
        {
            get
            {
                return this._presenceBuzzer;
            }
            set
            {
                this._presenceBuzzer = value;
            }
        } // endProperty: PresenceBuzzer

        /// <summary>
        /// L'option IR est-elle activée dans le logiciel?
        /// </summary>
        public Int32 IsIRActivated
        {
            get
            {
                return PegaseCore.PegaseData.Instance.OLogiciels.GestionIR;
            }
            set
            {
                PegaseCore.PegaseData.Instance.OLogiciels.GestionIR = value;
            }
        } // endProperty: IsIRActivated

        #endregion

        // Constructeur
        #region Constructeur

        public MO(XElement mo)
        {
            this._xmlRoot = mo;
            this._xmlProcessing = new XMLProcessing();
            this._xmlProcessing.OpenXML(this._xmlRoot);
            String Init = this.TypeMO;

            this.InitMO();
            this.InitOptions();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Effacer les données proprement
        /// </summary>
        public void Dispose ( )
        {
            this._xmlProcessing.Close();
            this._xmlRoot = null;
        } // endMethod: Dispose

        /// <summary>
        /// Initialiser les données des options
        /// </summary>
        public void InitOptions()
        {
            ObservableCollection<XElement> options = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamCartesOptions/CartesOptions/Options");

            this.OptionsMateriel.Clear();
            if (options != null)
            {
                foreach (var option in options)
                {
                    Option O = new Option(option);
                    if (O.Presence)
                    {
                        this.OptionsMateriel.Add(O);
                    }
                }
            }
        } // endMethod: InitOptions 

        /// <summary>
        /// Initialiser les données du MO à partir du fichier principal de données
        /// </summary>
        public void InitMO ()
        {
            // Initialiser la liste des organes
            IEnumerable<XElement> organes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/OrganeCommande/DicoOrganeMO/configHardMO");

            // afin de définir les bonnes valeurs pour "organ.DescriptionOrgane"

            String previousLanguage = LanguageSupport.Get().LanguageName;
            LanguageSupport.Get().InitialiseLanguage("Francais", DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);
            
            if (organes != null)
            {
                // Passer en langue française le temps de l'initialisation du MO
                foreach (XElement organe in organes)
                {
                    XMLProcessing XProcess = new XMLProcessing();
                    XProcess.OpenXML(organe);

                    OrganCommand OG = new OrganCommand();

                    OG.Mnemologique = XProcess.GetValue("MnemoLogique", "", "", XML_ATTRIBUTE.VALUE);

                    if (OG.Mnemologique != null)
                    {
                        OG.NomOrganeMO = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.VALUE);
                        if (OG.NomOrganeMO != "")
                        {
                            String Value = XProcess.GetValue("IndiceOrganeMo", "", "", XML_ATTRIBUTE.VALUE);
                            try
                            {
                                OG.IndiceOrganeMO = Convert.ToInt32(Value);
                            }
                            catch
                            {
                                OG.IndiceOrganeMO = 0;
                            }
                            if (XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.ZONEORGANEMO) != null)
                            {
                                OG.ZoneOrganeMO = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.ZONEORGANEMO);
                            }
                            else
                            {
                                OG.ZoneOrganeMO = "INCONNUE";
                            }
                            OG.MnemoHardFamilleMO = XProcess.GetValue("MnemoHardFamilleMo", "", "", XML_ATTRIBUTE.VALUE);
                            OG.MnemoHardOrganeMO = XProcess.GetValue("MnemoHardOrganeMo", "", "", XML_ATTRIBUTE.VALUE);
                            OG.MnemoClient = XProcess.GetValue("MnemoHardOrganeMo", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
                            OG.ReferenceOrgan = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.DESCRIPTION);
                            
                            Value = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.POSITION);
                            try
                            {
                                OG.Position = Convert.ToInt32(Value);
                            }
                            catch
                            {
                                OG.Position = 0;
                            }

                            if (OG.MnemoClient == "Plage_MnemoHardOrganeMo")
                            {
                                OG.MnemoClient = OG.NomOrganeMO;
                            }

                            Value = XProcess.GetValue("NbPosOrgane", "", "", XML_ATTRIBUTE.VALUE);
                            if (Value != null)
                            {
                                OG.NbPosOrgane = Convert.ToInt32(Value);
                            }
                            else
                            {
                                if (OG.MnemoHardFamilleMO == "AC")
                                {
                                    OG.NbPosOrgane = 13;
                                }
                            }
                            Value = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.TYPEPLD);
                            if ((Value != null) && Value.Equals("SingleSafety"))
                            {
                                OG.TypePld = OrganCommand.TYPE_PLD.SingleSafety;
                            }
                            else if ((Value != null) && Value.Equals("DoubleSafety"))
                            {
                                OG.TypePld = OrganCommand.TYPE_PLD.DoubleSafety;
                            }
                            else
                            {
                                OG.TypePld = OrganCommand.TYPE_PLD.None;
                            }
                            // Orientation
                            Value = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.MIN);
                            Int32 orientation = 0;
                            try
                            {
                                orientation = Convert.ToInt32(Value);
                            }
                            catch
                            {
                                orientation = 0;
                            }

                            OG.OrientationOrgane = orientation;

                            // Verrouillage en croix
                            Value = XProcess.GetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.MAX);
                            if (Value != null)
                            {
                                if (Value == "1")
                                {
                                    OG.VerrouillageEnCroix = true;
                                }
                                else
                                {
                                    OG.VerrouillageEnCroix = false;
                                }
                            }
                            else
                            {
                                OG.VerrouillageEnCroix = false;
                            }
                            this.OrganesCommandes.Add(OG);
                        }
                        // Corriger la valeur de la référence pour les anciennes fiches
                        if (OG.MnemoHardFamilleMO != "AC" && OG.MnemoHardFamilleMO != "AXE")
                        {
                            if (OG.ReferenceOrgan == "Descrip_NomOrganeMo")
                            {
                                // ne pas corrigé
                                //OG.ReferenceOrgan = "VIDE";
                            }
                        }
                        if (OG.ReferenceOrgan != null && OG.ReferenceOrgan != "")
                        {
                            OG.DescriptionOrgan = LanguageSupport.Get().GetToolTip("/CHECK_MATERIEL/" + OG.ReferenceOrgan);
                        }
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Il n'existe pas de définition d'organe de commande dans le fichier...");
            }

            // Repasser dans la langue précédente
            LanguageSupport.Get().InitialiseLanguage(previousLanguage, DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);

            // Initialiser la présence de certains matériels tels que le vibreur, l'IR et l'auxiliaire
            Int32 V;

            String SV = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/InfraRouge", "", "", XML_ATTRIBUTE.VALUE);
            if (SV != null)
            {
                if (SV.Length > 2 && SV.Substring(0, 2) == "0x")
                {
                    V = Convert.ToInt32(SV, 16);
                }
                else
                {
                    V = Convert.ToInt32(SV);
                }
            }
            else
            {
                V = 0;
            }

            //String PlageValeur = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/InfraRouge", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
            //PlageValeur = LanguageSupport.Get().GetToolTip("XMLTranslation/"+PlageValeur);
            //SV = Tools.GetCorrespondantStringValue(PlageValeur, V);

            this._presenceIR = V;

            // Initialiser la présence du vibreur
            //SV = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/Vibreur", "", "", XML_ATTRIBUTE.VALUE);
            //if (SV != null)
            //{
            //    if (SV.Length > 2 && SV.Substring(0, 2) == "0x")
            //    {
            //        V = Convert.ToInt32(SV, 16);
            //    }
            //    else
            //    {
            //        V = Convert.ToInt32(SV);
            //    }
            //}
            //else
            //{
            //    V = 0;
            //}
            ////PlageValeur = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/Vibreur", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
            ////PlageValeur = LanguageSupport.Get().GetToolTip("XMLTranslation/" + PlageValeur);
            ////SV = Tools.GetCorrespondantStringValue(PlageValeur, V);
            //this._presenceVibreur = V;

            // Initialiser la présence de l'acceleromètre
            SV = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/Accelerometre", "", "", XML_ATTRIBUTE.VALUE);
            if (SV != null)
            {
                if (SV.Length > 2 && SV.Substring(0, 2) == "0x")
                {
                    V = Convert.ToInt32(SV, 16);
                }
                else
                {
                    V = Convert.ToInt32(SV);
                }
            }
            else
            {
                V = 0;
            }
            String PlageValeur = this._xmlProcessing.GetValue("ConfigMat/OptionsMaterielles/Accelerometre", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
            PlageValeur = LanguageSupport.Get().GetToolTip("XMLTranslation/" + PlageValeur);
            SV = Tools.GetCorrespondantStringValue(PlageValeur, V);

            this._presenceAccelerometre = V;

            // Initialiser la présence du buzzer et de l'auxiliaire
            ObservableCollection<XElement> carteOptions = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamCartesOptions/CartesOptions/Options");
            if (carteOptions != null && carteOptions.Count > 0)
            {
                foreach (var carteOption in carteOptions)
                {
                    IEnumerable<XElement> Options = carteOption.Descendants();
                    XMLProcessing process = new XMLProcessing();
                    process.OpenXML(carteOption);

                    String NameOption = process.GetValue("NomOption", "", "", XML_ATTRIBUTE.VALUE);

                    if (NameOption != null)
                    {
                        if (NameOption.ToUpper() == "BUZZER")
                        {
                            this.PresenceBuzzer = this.GetPresenceOptionValue(process);
                        }
                        else if (NameOption.ToUpper() == "AUXILIARY BUTTON")
                        {
                            this.PresenceAuxiliaire = this.GetPresenceOptionValue(process);
                        }
                        else if (NameOption.ToUpper() == "VIBRATOR")
                        {
                            this._presenceVibreur = this.GetPresenceOptionValue(process);
                        }
                        else if (NameOption.ToUpper() == "LINKBYWIRE")
                        {
                            this._presenceliaisonfilaire = this.GetPresenceOptionValue(process);
                        }
                        else if (NameOption.ToUpper() == "SAFETY BUTTON")
                        {
                            this._presenceSafetyButton = this.GetPresenceOptionValue(process);
                        }
                    }
                }
            }
            String RefMO = PegaseData.Instance.XMLFile.GetValue("XmlTracabilite/MORef", "", "", XML_ATTRIBUTE.VALUE);
            try
            {
                Regex pattern_galva = new Regex(PegaseCore.Helper.ConfigurationReader.Instance.GetValue("OptionMoGalva"));
                Regex pattern_baugris = new Regex(PegaseCore.Helper.ConfigurationReader.Instance.GetValue("OptionMoBauGris"));
                if (pattern_baugris.IsMatch(RefMO))
                {
                 this._presenceBAUGris = 1;
                }
                else
                {
                    this._presenceBAUGris = 0;
                }
                if (pattern_galva.IsMatch(RefMO))
                {
                    this._presenceGalva = 1;
                }
                else
                {
                    this._presenceGalva = 0;
                }
            }
            catch
            {

            }
        } // endMethod: InitMO
        
        /// <summary>
        /// Lit l'option contenu dans process 
        /// </summary>
        public Int32 GetPresenceOptionValue ( XMLProcessing process )
        {
            Int32 Result = 0;
            String SV;

            SV = process.GetValue("PresenceOption", "", "", XML_ATTRIBUTE.VALUE);
            if (SV != null)
            {
                if (SV.Length > 2 && SV.Substring(0, 2) == "0x")
                {
                    Result = Convert.ToInt32(SV, 16);
                }
                else
                {
                    Result = Convert.ToInt32(SV);
                }
            }
            else
            {
                Result = 0;
            }

            return Result;
        } // endMethod: GetPresenceOptionValue

        /// <summary>
        /// Enregistrer les données dans le fichier initial
        /// </summary>
        public void SerialiseMO ( )
        {
            // Sauvegarde des organes
            IEnumerable<XElement> organes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/OrganeCommande/DicoOrganeMO/configHardMO");
            
            if (organes != null)
            {
                List<XElement> organesList = organes.ToList();
                Int32 i = 0;

                foreach (PegaseCore.OrganCommand organe in this.OrganesCommandes)
                {
                    XMLProcessing XProcess = new XMLProcessing();
                    XProcess.OpenXML(organesList[i]);

                    XProcess.SetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.VALUE, organe.NomOrganeMO);
                    XProcess.SetValue("IndiceOrganeMo", "", "", XML_ATTRIBUTE.VALUE, organe.IndiceOrganeMO.ToString());
                    XProcess.SetValue("MnemoHardFamilleMo", "", "", XML_ATTRIBUTE.VALUE, organe.MnemoHardFamilleMO);
                    XProcess.SetValue("MnemoHardOrganeMo", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, organe.MnemoClient);
                    XProcess.SetValue("MnemoLogique", "", "", XML_ATTRIBUTE.VALUE, organe.Mnemologique);
                    if (!XProcess.SetValue("NbPosOrgane", "", "", XML_ATTRIBUTE.VALUE, organe.NbPosOrgane.ToString()))
                    {
                        // Créer la ligne
                        XElement node = XProcess.GetNodeByPath("Mnemologique").First();
                        XElement XnbPosOrgane = XElement.Parse(String.Format("<Variable code=\"NbPosOrgane\" description=\"Descrip_NomOrganeMo\" commentaire=\"Com_NbPosOrgane\" offsetAbsolu=\"-1\" offsetRelatif=\"-1\" taille=\"30\" type=\"String\" count=\"-1\" min=\"\" max=\"\" commentairePlageValeur=\"\" plagevaleur=\"Plage_NbPosOrgane\" valeur=\"{0}\" indice=\"-1\" unite=\"\" countmax=\"-1\" idEAna=\"-1\" Num=\"24\" />", organe.NbPosOrgane));
                        node.AddAfterSelf(XnbPosOrgane);
                    }

                    XProcess.SetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.DESCRIPTION, organe.ReferenceOrgan);
                    XProcess.SetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.MIN, organe.OrientationOrgane.ToString());
                    String v = "0";
                    if (organe.VerrouillageEnCroix)
                    {
                        v = "1";
                    }
                    XProcess.SetValue("NomOrganeMo", "", "", XML_ATTRIBUTE.MAX, v);
                    i++;
                }
            }

            // sauvegarde de la présence IR
            IEnumerable<XElement> XIR = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/InfraRouge");
            if (XIR != null)
            {
                XElement PIR = XIR.First();
                //PIR.Attribute(XML_ATTRIBUTE.VALUE).Value = this.PresenceIR.ToString();
            }

            // Sauvegarde de la présence vibreur
            IEnumerable<XElement> XVIB = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/Vibreur");
            if (XVIB != null)
            {
                XElement PVIB = XVIB.First();
                //PVIB.Attribute(XML_ATTRIBUTE.VALUE).Value = this.PresenceVibreur.ToString();
            }

            // Sauvegarde de la présence auxiliaire
            IEnumerable<XElement> XAUX = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/EntreesAuxMO");
            if (XAUX != null)
            {
                XElement PAUX = XAUX.First();
                //PAUX.Attribute(XML_ATTRIBUTE.VALUE).Value = this.PresenceVibreur.ToString();
            }

        } // endMethod: SerialiseMO

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: MO
}
