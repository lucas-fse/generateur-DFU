using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La class de gestion des données d'un MT
    /// </summary>
    public class MT
    {
        #region Constantes

        public const String A1 = "A";
        public const String E1 = "E";
        public const String T1 = "T";
        public const String N1 = "N";
        public const String ALTO = "Alto";
        public const String ELIO = "Elio";
        public const String TIMO = "Timo";
        public const String NEMO = "Nemo";

        #endregion

        // Variables
        #region Variables

        private XElement _xmlRoot;
        private XMLProcessing _xmlTechProcessing;
        private XMLProcessing _xmlMetProcessing;
        private ObservableCollection<Carte> _cartes;
        private ObservableCollection<ESAna> _eAnas;
        private ObservableCollection<ESAna> _sAnas;
        private ObservableCollection<ESTOR> _eTors;
        private ObservableCollection<ESTOR> _sTors;
        private ObservableCollection<ESDivers> _esDivers;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Présence du Klaxon
        /// </summary>
        public String Klaxon
        {
            get
            {
                String Result = this._xmlTechProcessing.GetValue("XmlMetier/HorsMode/DescriptionKlaxon/PresenceKlaxon", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
        } // endProperty: Klaxon

        /// <summary>
        /// La référence industrielle du MT
        /// </summary>
        public String RefIndus
        {
            get
            {
                String Result = this._xmlTechProcessing.GetValue("ConfigMat/IdentificationERP/IdentERP", "", "", XML_ATTRIBUTE.VALUE);
                Result = XMLCore.Tools.ConvertFromASCII2Text(Result);

                return Result;
            }
        } // endProperty: RefIndus

        /// <summary>
        /// La fréquence du MT
        /// </summary>
        public String Frequence
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/FreqMT", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
        } // endProperty: Frequence

        /// <summary>
        /// La tension du MT
        /// </summary>
        public String Tension
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/TensionMT", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
        } // endProperty: Tension

        /// <summary>
        /// Le Type de cable
        /// </summary>
        public String TypeCable
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/TypeSortieCable", "", "", XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }
                return Result;
            }
        } // endProperty: TypeCable

        /// <summary>
        /// Le numéro de série du MT
        /// </summary>
        public String SerialNumber
        {
            get
            {
                String SV = this._xmlTechProcessing.GetValue("DonneesDeFab/NumeroDeSerie", "", "", XML_ATTRIBUTE.VALUE);
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
                String SV = this._xmlTechProcessing.GetValue("DonneesDeFab/CodeIdNatifMT", "", "", XML_ATTRIBUTE.VALUE);
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
        /// Le type du MT (Elio / Alto)
        /// </summary>
        public String TypeMT
        {
            get
            {
                String SV = this._xmlTechProcessing.GetValue("ConfigMat/IdentificationERP/IdentERP", "", "", XML_ATTRIBUTE.VALUE);
                SV = Tools.ConvertFromASCII2Text(SV);

                if (SV.Length > 1)
                {
                    SV = SV.Substring(0, 1);
                }
                else
                {
                    SV = PegaseData.Instance.XMLFile.GetValue("XmlIdentification/Ident/IdentProduit/TypeMT", "", "", XML_ATTRIBUTE.VALUE);
                    if (SV != null)
                    {
                        if (SV.Length > 1)
                        {
                            SV = SV.Substring(0, 1);
                        }
                    }
                }
                
                switch (SV)
                {
                    case A1:
                        SV = ALTO;
                        break;
                    case E1:
                        SV = ELIO;
                        break;
                    case T1:
                        SV = TIMO;
                        break;
                    case N1:
                        SV = NEMO;
                        break;
                    default:
                        SV = LanguageSupport.Get().COMMON_FIELD_UNKNOW;
                        break;
                }

                return SV;
            }
        } // endProperty: TypeMT

        #region OptionMatériel

        /// <summary>
        /// L'ensemble des cartes constituant le MT
        /// </summary>
        public ObservableCollection<Carte> Cartes
        {
            get
            {
                if (this._cartes == null)
                {
                    this._cartes = new ObservableCollection<Carte>();
                }

                return this._cartes;
            }
        } // endProperty: Cartes

        #endregion

        #region IO

        /// <summary>
        /// La liste des entrées sorties divers
        /// </summary>
        public ObservableCollection<ESDivers> ListESDivers
        {
            get
            {
                if (this._esDivers == null)
                {
                    this._esDivers = new ObservableCollection<ESDivers>();
                }

                return this._esDivers;
            }
        } // endProperty: ESDivers

        /// <summary>
        /// La collection des entrées analogiques
        /// </summary>
        public ObservableCollection<ESAna> EAnas
        {
            get
            {
                if (this._eAnas == null)
                {
                    this._eAnas = new ObservableCollection<ESAna>();
                }

                return this._eAnas;
            }
        } // endProperty: EAnas

        /// <summary>
        /// La collection des sorties analogiques
        /// </summary>
        public ObservableCollection<ESAna> SAnas
        {
            get
            {
                if (this._sAnas == null)
                {
                    this._sAnas = new ObservableCollection<ESAna>();
                }
                return this._sAnas;
            }
        } // endProperty: SAnas

        /// <summary>
        /// Les entrées TORs
        /// </summary>
        public ObservableCollection<ESTOR> ETORS
        {
            get
            {
                if (this._eTors == null)
                {
                    this._eTors = new ObservableCollection<ESTOR>();
                }
                return this._eTors;
            }
        } // endProperty: ETORS

        /// <summary>
        /// Les sorties TORs
        /// </summary>
        public ObservableCollection<ESTOR> STORS
        {
            get
            {
                if (this._sTors == null)
                {
                    this._sTors = new ObservableCollection<ESTOR>();
                }
                return this._sTors;
            }
        } // endProperty: STORS

        #endregion

        #endregion

        // Constructeur
        #region Constructeur

        public MT(XElement mt)
        {
            this._xmlRoot = mt;
            // Initialisation de la partie technique de la recherche d'informations
            this._xmlTechProcessing = new XMLProcessing();
            this._xmlTechProcessing.OpenXML(this._xmlRoot);
            // Initialisation de la partie métier de la recherche d'informations
            this._xmlMetProcessing = new XMLProcessing();
            this._xmlMetProcessing.OpenXML(PegaseData.Instance.MetierRoot);
            // Initialisation des cartes
            this.InitCartes();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Mettre à jour les données Timo pour le PWM
        /// </summary>
        public void MAJSTorTimo4PWM ( ESTOR currentSTOR )
        {
            Int32 F = currentSTOR.PWMFrequence;
            Int32 currentSTORNum;
            Int32 BorneMin, BorneMax;

            if (currentSTOR.MnemoHardware.Length == 2)
	        {
                currentSTORNum = Convert.ToInt32(currentSTOR.MnemoHardware.Substring(1, 1));
                if (currentSTORNum >=1 && currentSTORNum <= 4)
                {
                    BorneMin = 1;
                    BorneMax = 4;
                }
                else
                {
                    BorneMin = 5;
                    BorneMax = 6;
                }

                for (int i = BorneMin; i <= BorneMax; i++)
                {
                    String STORName = String.Format("o{0}", i);

                    var Query = from STor in PegaseData.Instance.ModuleT.STORS
                                where STor.MnemoHardware == STORName
                                select STor;

                    if (Query.Count()>0)
                    {
                        Query.First().SetFrequence(F);
                    }
                }
	        }
        } // endMethod: MAJSTorTimo4PWM

        /// <summary>
        /// Initialiser les données des entrées / sorties
        /// </summary>
        public void InitES ( )
        {
            // Initialisation de la collection EAnas
            this.InitEAna();
            // Initialisation de la collection SAnas
            this.InitSAna();
            // Initialisation de la collection ETORs
            this.InitETORs();
            // Initialisation de la collection STORs
            this.InitSTORs();
            // Initialiser la collection des entrées / sorties divers
            this.InitESDivers();
        } // endMethod: InitES

        /// <summary>
        /// Initialiser les modes UI en cours pour chacune des entrées / sorties Ana
        /// </summary>
        public Int32 InitUIESAna ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            try
            {
                foreach (var item in this.EAnas)
                {
                    item.InitCurrentEAnaModes();
                }

                foreach (var item in this.SAnas)
                {
                    item.InitCurrentEAnaModes();
                }
            }
            catch
            {
                Result = XML_ERROR.ERROR_XML_INTEGRITY;
            }

            return Result;
        } // endMethod: InitUIEAna
        
        /// <summary>
        /// Initialiser la liste des entrées / sorties divers
        /// </summary>
        public Int32 InitESDivers( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> XESDivers = this._xmlMetProcessing.GetNodeByPath("ESDivers/ES/Configuration");
            if (XESDivers != null && XESDivers.Count > 0)
            {
                foreach (var item in XESDivers)
                {
                    ESDivers es = new ESDivers(item);
                    this.ListESDivers.Add(es);
                } 
            }

            return Result;
        } // endMethod: InitESDivers

        /// <summary>
        /// Initialiser la liste des entrées analogiques
        /// </summary>
        private Int32 InitEAna ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;
            ObservableCollection<XElement> Eana;
            ObservableCollection<XElement> PilotageES;
            ObservableCollection<XElement> Affectation;

            this._eAnas = new ObservableCollection<ESAna>();

            // lister tous les composants des entrées Ana
            Eana = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/EntreesAna/Eana");
            PilotageES = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/EntreesAna/PilotageES");
            Affectation = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/EntreesAna/Affectation");

            // vérifier que le nombre de données est équivalente pour chacune des collections
            this._eAnas = new ObservableCollection<ESAna>();

            if (Eana != null && PilotageES != null && Affectation != null)
            {
                if (Eana.Count == PilotageES.Count && PilotageES.Count == Affectation.Count)
                {
                    // créer toutes les entrées analogiques
                    for (int i = 0; i < Eana.Count; i++)
                    {
                        ESAna esAna = new ESAna(Eana[i], PilotageES[i], Affectation[i]);
                        esAna.IOType = TypeIO.Input;
                        if (esAna.ID != -1)
                        {
                            this._eAnas.Add(esAna);
                        }
                    }
                }
                else
                {
                    Result = XML_ERROR.ERROR_XML_INTEGRITY;
                }
            }

            return Result;
        } // endMethod: InitEAna
        
        /// <summary>
        /// Initialiser les données des cartes
        /// </summary>
        public void InitCartes ( )
        {
            ObservableCollection<XElement> cartes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamCartesOptions/CartesOptions/Cartes");

            if (cartes != null)
            {
                this.Cartes.Clear();
                foreach (var carte in cartes)
                {
                    Carte C = new Carte(carte);
                    if (C.IsInstalled)
                    {
                        this.Cartes.Add(C); 
                    }
                }
            }
        } // endMethod: InitCartes

        /// <summary>
        /// Enregistre les données du MT
        /// </summary>
        public void SerialiseMT ( )
        {
            this.SaveEAnas();
            this.SaveSAnas();
            this.SaveETORS();
            this.SaveSTORS();
        } // endMethod: SerialiseMT

        /// <summary>
        /// Sauvegarder les entrées EAnas
        /// </summary>
        private void SaveEAnas ( )
        {
            IEnumerable<XElement> eanas = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/EntreesAna/Eana");
            IEnumerable<XElement> pilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/EntreesAna/PilotageES");
            IEnumerable<XElement> affectation = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/EntreesAna/Affectation");
            if (eanas != null && pilotageES != null && affectation != null)
            {
                List<XElement> eanasList = eanas.ToList();
                List<XElement> pilotageESList = pilotageES.ToList();
                List<XElement> affectationList = affectation.ToList();

                for (int i = 0; i < this.EAnas.Count; i++)
                {
                    // Mettre à jour les gains et offsets en fonction du type UI
                    ParamsGainOffset PGO;
                    if (String.IsNullOrEmpty(this.EAnas[i].IsNewTypeUI))
                    {
                         PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndTypeUI(this.TypeMT.ToUpper(), this.EAnas[i].UIType);
                    }
                    else
                    {
                         PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndTypeUI(this.EAnas[i].IsNewTypeUI, this.EAnas[i].UIType);
                    }
                    if (PGO != null)
                    {
                        this.EAnas[i].Gain = PGO.GainE;
                        this.EAnas[i].Offset = PGO.OffsetE;
                    }

                    // EAna
                    XMLProcessing XProcessEana = new XMLProcessing();
                    XProcessEana.OpenXML(eanasList[i]);
                    XProcessEana.SetValue("MnemoHardware", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, this.EAnas[i].MnemoClient);

                    // PilotageES

                    // Affectation
                    XMLProcessing XProccesAffectation = new XMLProcessing();
                    XProccesAffectation.OpenXML(affectationList[i]);
                    XProccesAffectation.SetValue("Gain", "", "", XML_ATTRIBUTE.VALUE, this.EAnas[i].Gain.ToString(CultureInfo.InvariantCulture));
                    XProccesAffectation.SetValue("Offset", "", "", XML_ATTRIBUTE.VALUE, this.EAnas[i].Offset.ToString(CultureInfo.InvariantCulture));
                    XProccesAffectation.SetValue("TypeUI", "", "", XML_ATTRIBUTE.VALUE, ((Int32)this.EAnas[i].UIType).ToString(CultureInfo.InvariantCulture));
                }
            }
        } // endMethod: SaveEAnas

        /// <summary>
        /// Enregistrer les données TOR
        /// </summary>
        public void SaveETORS ( )
        {
            IEnumerable<XElement> etors = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/Configuration");
            IEnumerable<XElement> pilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/PilotageES");

            if (etors != null && pilotageES != null)
            {
                List<XElement> etorsList = etors.ToList<XElement>();
                List<XElement> pilotageESList = pilotageES.ToList<XElement>();

                if (etorsList.Count == pilotageESList.Count)
                {
                    for (int i = 0; i < this.ETORS.Count; i++)
                    {
                        // ETOR :-> Seul Mnemohardware est modifiable. Seul cette donnée est sauvegardée...
                        XMLProcessing XProcessETOR = new XMLProcessing();
                        XProcessETOR.OpenXML(etorsList[i]);

                        XProcessETOR.SetValue("MnemoHardware", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, this.ETORS[i].MnemoClient);
                        // PilotageES
                    }
                }
            }
        } // endMethod: SaveETORS
        
        /// <summary>
        /// Sauvegarder les sorties TORs
        /// </summary>
        public void SaveSTORS ( )
        {
            IEnumerable<XElement> stors = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/Configuration");
            IEnumerable<XElement> pilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/PilotageES");
            IEnumerable<XElement> reglage = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/Reglage");
            IEnumerable<XElement> reglageSecurite = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/ReglageSecurite");

            if (stors != null && pilotageES != null && reglage != null && reglageSecurite != null)
            {
                List<XElement> storsList = stors.ToList<XElement>();
                List<XElement> pilotageESList = pilotageES.ToList<XElement>();
                List<XElement> reglageList = reglage.ToList<XElement>();
                List<XElement> reglageSecuriteList = reglageSecurite.ToList<XElement>();

                // Sauvegarde de Mnemohardware uniquement, la seule donnée modifiable
                for (int i = 0; i < this.STORS.Count; i++)
                {
                    XMLProcessing XProccessSTors = new XMLProcessing();
                    XProccessSTors.OpenXML(storsList[i]);

                    XProccessSTors.SetValue("MnemoHardware", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, this.STORS[i].MnemoClient);
                }

                // Sauvegarde des valeurs en sécurité
                for (int i = 0; i < this.STORS.Count; i++)
                {
                    XMLProcessing XProcessSTors = new XMLProcessing();
                    XProcessSTors.OpenXML(reglageSecuriteList[i]);

                    XProcessSTors.SetValue("ValeurInitiale", "", "", XML_ATTRIBUTE.VALUE, this.STORS[i].ValeurInitiale);
                    
                    if (this.STORS[i].ValeurEnSecurite == "0" || this.STORS[i].ValeurEnSecurite == "1" )
                    {
                        XProcessSTors.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "1");
                        XProcessSTors.SetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE, this.STORS[i].ValeurEnSecurite);
                    }
                    else
                    {
                        XProcessSTors.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "0");
                        XProcessSTors.SetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "0");
                    }

                    // Sauvegarder le PWM en cas de besoin
                    if (this.TypeMT == MT.TIMO)
                    {
                        if (this.STORS[i].IsPWM)
                        {
                            this.STORS[i].SavePWM();
                        }
                    }
                }
            }
        } // endMethod: SaveSTORS
        
        /// <summary>
        /// Sauvegarder les sorties Anas
        /// </summary>
        public void SaveSAnas ( )
        {
            IEnumerable<XElement> sanas = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/SortiesAna/Configuration");
            IEnumerable<XElement> pilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/SortiesAna/PilotageES");
            IEnumerable<XElement> reglage = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/SortiesAna/Reglage");
            IEnumerable<XElement> reglageSecurite = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParamAppliES/SortiesAna/ReglageSecurite");

            if (sanas != null && pilotageES != null && reglage != null && reglageSecurite != null)
            {
                List<XElement> sanaList = sanas.ToList<XElement>();
                List<XElement> pilotageESList = pilotageES.ToList<XElement>();
                List<XElement> reglageList = reglage.ToList<XElement>();
                List<XElement> reglageSecuriteList = reglageSecurite.ToList<XElement>();

                // Sauvegarde de Mnemohardware uniquement, la seule donnée modifiable
                for (int i = 0; i < this.SAnas.Count; i++)
                {
                    XMLProcessing XProccess = new XMLProcessing();
                    XProccess.OpenXML(sanaList[i]);

                    XProccess.SetValue("MnemoHardware", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, this.SAnas[i].MnemoClient);
                    ParamsGainOffset PGO = null;

                    if (String.IsNullOrEmpty(this.SAnas[i].IsNewTypeUI))
                    {
                        PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndTypeUI(this.TypeMT.ToUpper(), this.SAnas[i].UIType);
                    }
                    else
                    {
                        PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndTypeUI(this.SAnas[i].IsNewTypeUI, this.SAnas[i].UIType);
                    }
                    if (PGO != null)
                    {
                        this.SAnas[i].Gain = PGO.GainS;
                        this.SAnas[i].Offset = PGO.OffsetS;
                    }
                }

                // Sauvegarde des modifications de la section réglages
                for (int i = 0; i < this.SAnas.Count; i++)
                {
                    XMLProcessing XProcess = new XMLProcessing();
                    XProcess.OpenXML(reglageList[i]);

                    XProcess.SetValue("TypeUI", "", "", XML_ATTRIBUTE.VALUE, ((Int32)this.SAnas[i].UIType).ToString(CultureInfo.InvariantCulture));
                }

                // Sauvegarde des réglages en sécurité
                for (int i = 0; i < this.SAnas.Count; i++)
                {
                    XMLProcessing XProcess = new XMLProcessing();
                    XProcess.OpenXML(reglageSecuriteList[i]);

                    
                    if (SAnas[i].ValeurInitialePercent != null)
                    {
                        if (SAnas[i].ValeurInitialePercent != "")
                        {
                            XProcess.SetValue("ValeurInitiale", "", "", XML_ATTRIBUTE.VALUE, this.SAnas[i].ValeurInitiale.ToString(CultureInfo.InvariantCulture));
                            XProcess.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "1");
                        }
                        else
                        {
                            XProcess.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "0");
                        }
                    }
                    if (SAnas[i].ValeurEnSecuritePercent != null)
                    {
                        if (SAnas[i].ValeurEnSecuritePercent != "")
                        {
                            XProcess.SetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE, this.SAnas[i].ValeurEnSecurite.ToString(CultureInfo.InvariantCulture));
                            XProcess.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "1");
                        }
                        else
                        {
                            XProcess.SetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "-1");
                            XProcess.SetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE, "0");
                        }
                    }
                }
            }
        } // endMethod: SaveSAnas

        /// <summary>
        /// Initialiser le mappage des sorties analogiques
        /// </summary>
        public Int32 InitSAna ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> SAna;
            ObservableCollection<XElement> PilotageES;
            ObservableCollection<XElement> Reglage;
            ObservableCollection<XElement> ReglageSecurite;

            this._sAnas = new ObservableCollection<ESAna>();

            // lister tous les composants des entrées Ana
            SAna = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/SortiesAna/Configuration");
            PilotageES = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/SortiesAna/PilotageES");
            Reglage = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/SortiesAna/Reglage");
            ReglageSecurite = this._xmlMetProcessing.GetNodeByPath("ParamAppliES/SortiesAna/ReglageSecurite");

            // vérifier que le nombre de données est équivalente pour chacune des collections
            this._sAnas = new ObservableCollection<ESAna>();

            if (SAna != null && PilotageES != null && Reglage != null && ReglageSecurite != null)
            {
                if (SAna.Count == PilotageES.Count && PilotageES.Count == Reglage.Count && Reglage.Count == ReglageSecurite.Count)
                {
                    // créer toutes les entrées analogiques
                    for (int i = 0; i < SAna.Count; i++)
                    {
                        ESAna esAna = new ESAna(SAna[i], PilotageES[i], Reglage[i], ReglageSecurite[i]);
                        esAna.IOType = TypeIO.Output;
                        if (esAna.ID != -1)
                        {
                            this._sAnas.Add(esAna); 
                        }
                    }
                }
                else
                {
                    Result = XML_ERROR.ERROR_XML_INTEGRITY;
                }
            }

            // Chargement des réglages en sécurité
            for (int i = 0; i < this.SAnas.Count; i++)
            {
                XMLProcessing XProcess = new XMLProcessing();
                XProcess.OpenXML(ReglageSecurite[i]);

                this.SAnas[i].ValeurInitiale = XProcess.GetValue("ValeurInitiale", "", "", XML_ATTRIBUTE.VALUE);
                this.SAnas[i].ValeurEnSecurite = XProcess.GetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE);

                Double val;

                try
                {
                    val = Convert.ToDouble(this.SAnas[i].ValeurInitiale);
                    val = (val * 100) / 1023;
                }
                catch
                {
                    val = 0;
                }
                this.SAnas[i].ValeurInitialePercent = (Math.Round(val)).ToString(CultureInfo.InvariantCulture);

                try
                {
                    val = Convert.ToDouble(this.SAnas[i].ValeurEnSecurite);
                    val = (val * 100) / 1023;
                }
                catch
                {
                    val = 0;
                }
                if (val >= 0)
                {
                    this.SAnas[i].ValeurEnSecuritePercent = (Math.Round(val)).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.SAnas[i].ValeurEnSecuritePercent = "";
                }
            }

            return Result;
        } // endMethod: InitSAna
        
        /// <summary>
        /// Initialisation des sorties TORs
        /// </summary>
        public void InitSTORs( )
        {
            ObservableCollection<XElement> Configurations, PilotageES, Reglage, ReglageSecurite;

            Configurations = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/Configuration");
            PilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/PilotageES");
            Reglage = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/Reglage");
            ReglageSecurite = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/SortiesTOR/ReglageSecurite");

            if (Configurations != null && PilotageES != null && Reglage != null && ReglageSecurite != null)
            {
                if (Configurations.Count == PilotageES.Count && PilotageES.Count == Reglage.Count && Reglage.Count == ReglageSecurite.Count)
                {
                    this.STORS.Clear();
                    for (int i = 0; i < Configurations.Count; i++)
                    {
                        ESTOR STor = new ESTOR(Configurations[i], PilotageES[i], Reglage[i], ReglageSecurite[i]);
                        if (STor.ID != -1)
                        {
                            this.STORS.Add(STor);
                        }
                    }
                    if (this.TypeMT == MT.TIMO)
                    {
                        for (int i = 0; i < this.STORS.Count; i++)
                        {
                            if (this.STORS[i].PWMFrequence > 0)
                            {
                                this.MAJSTorTimo4PWM(this.STORS[i]); 
                            }
                        }
                    }
                } 
            }
        } // endMethod: InitSTORs
        
        /// <summary>
        /// Initialisation des entrées TORs
        /// </summary>
        public void InitETORs( )
        {
            ObservableCollection<XElement> Configurations, PilotageES, Affectation;

            Configurations = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/Configuration");
            PilotageES = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/PilotageES");
            Affectation = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ESTOR/EntreesTOR/Affectation");

            if (Configurations != null && PilotageES != null /*&& Affectation != null*/)
            {
                if (Configurations.Count == PilotageES.Count /*&& PilotageES.Count == Affectation.Count*/)
                {
                    for (int i = 0; i < Configurations.Count; i++)
                    {
                        ESTOR ETor = new ESTOR(Configurations[i], PilotageES[i], null /*Affectation[i]*/);
                        if (ETor.ID !=-1)
                        {
                            this.ETORS.Add(ETor); 
                        }
                    }
                } 
            }
        } // endMethod: InitETORs

        /// <summary>
        /// Fermer les données proprement
        /// </summary>
        public void Dispose ( )
        {
            // les explorateurs de xml
            this._xmlTechProcessing.Close();
            this._xmlTechProcessing = null;

            this._xmlMetProcessing.Close();
            this._xmlMetProcessing = null;

            // les cartes
            if (this._cartes != null)
            {
                foreach (var carte in this._cartes)
                {
                    carte.Dispose();
                }
                this._cartes = null;
            }
            
            // les entrées analogiques
            if (this._eAnas != null)
            {
                foreach (var eanas in this._eAnas)
                {
                    eanas.Dispose();
                }
                this._eAnas = null; 
            }

            // les sorties analogiques
            if (this._sAnas != null)
            {
                foreach (var sanas in this._sAnas)
                {
                    sanas.Dispose();
                }
                this._sAnas = null; 
            }

            this._xmlRoot = null;

        } // endMethod: Dispose

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: MT
}
