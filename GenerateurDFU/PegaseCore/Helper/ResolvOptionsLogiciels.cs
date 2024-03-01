using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JAY.PegaseCore;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Linq;

namespace JAY.PegaseCore.Helper
{
    public class ValeurOptionLogiciel
    {
        /// <summary>
        /// Le nom de l'option logicielle
        /// </summary>
        public String NameOption
        {
            get;
            set;
        } // endProperty: NameOption

        /// <summary>
        /// Le code ERP de l'option
        /// </summary>
        public String CodeERP
        {
            get;
            set;
        } // endProperty: CodeERP

        /// <summary>
        /// Le code ERP de l'option
        /// </summary>
        public String CodeEmbarque
        {
            get;
            set;
        } // endProperty: CodeEmbarque

        /// <summary>
        /// Le libellé de la valeur de l'option logicielle
        /// </summary>
        public String Libelle
        {
            get;
            set;
        } // endProperty: Libelle
    }

    /// <summary>
    /// Résoudre les options logiciels à partir de la référence industrielle
    /// de la Sim
    /// </summary>
    public static class ResolvOptionsLogiciels
    {
        #region Propriétés static



        public static String NmrSimTraceur
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Le dictionnaire des options et de leurs valeurs
        /// Ces options doivent être chargées depuis un produit connecté (client),
        /// ou depuis un fichier XML (prod)
        /// </summary>
        public static Dictionary<String, Int32> ListOptions
        {
            get;
            private set;
        } // endProperty: ListOptions
        public static Dictionary<String, Int32> ListBlocageMateriel
        {
            get;
            private set;
        } // endProperty: ListOptions 
        /// <summary>
        /// La liste de toutes les valeurs des options logicielles (requis pour le rapport)
        /// </summary>
        public static ObservableCollection<ValeurOptionLogiciel> ListValeurOptionsLogiciel
        {
            get;
            private set;
        } // endProperty: ListValeurOptionsLogiciel
        public static ObservableCollection<ValeurOptionLogiciel> ListValeurBlocageMateriel
        {
            get;
            private set;
        } // endProperty: ListValeurOptionsLogiciel
        
        /// <summary>
        /// Dictionnaire des codes embarqués en focntion des codes ERP
        /// </summary>
        public static Dictionary<String, String> CodeERPCodeEmbarque
        {
            get;
            set;
        } // endProperty: Dictionnary<String, String> CodeERPCodeEmbarque

        #endregion

        #region Constructeur statique

        // constructeur statique initialisant les valeurs par défaut
        static ResolvOptionsLogiciels()
        {
            InitDictionaryCodeERPCodeEmbarque();
        }

        #endregion

        #region Méthodes static

        /// <summary>
        /// initialise le dictionnaire des codes embarqués en focntion des codes ERP
        /// </summary>
        public static void InitDictionaryCodeERPCodeEmbarque( )
        {
            XElement ConfigOptions = LoadConfigOptions();
            Dictionary<String, String> CErpCEmbarque = new Dictionary<String,String>();
            ObservableCollection<ValeurOptionLogiciel> listValeurOptionsLogiciel = new ObservableCollection<ValeurOptionLogiciel>();

            IEnumerable<XElement> sim = from row in ConfigOptions.Descendants("Produit")
                                        where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == "SIM"
                                        select row;

            var queryOption = from row in sim.Descendants()
                              where row.Name == "Option" 
                              select row;

            List<XElement> ListOptions = queryOption.ToList<XElement>();

            foreach (var option in ListOptions)
            {
                var queryValeur = from node in option.Descendants()
                                  where node.Name == "Valeur"
                                  select node;

                foreach (var valeur in queryValeur)
                {
                    String Option = option.Attribute("valeur").Value;
                    String CodeERP = valeur.Attribute("Code").Value;
                    String CodeEmbarq = valeur.Attribute("Embarque").Value;
                    CErpCEmbarque.Add(Option + "_" + CodeERP, CodeEmbarq);

                    ValeurOptionLogiciel VOL = new ValeurOptionLogiciel();
                    VOL.CodeEmbarque = CodeEmbarq;
                    VOL.CodeERP = CodeERP;
                    VOL.NameOption = Option;
                    VOL.Libelle = valeur.Attribute("Libelle").Value;
                    listValeurOptionsLogiciel.Add(VOL);
                }
            }

            CodeERPCodeEmbarque = CErpCEmbarque;
            ResolvOptionsLogiciels.ListValeurOptionsLogiciel = listValeurOptionsLogiciel;
        } // endMethod: InitDictionaryCodeERPCodeEmbarque

        public static void InitDictionaryBlocageByProduct(ConnectedProduct product)
        {
            XElement ConfigOptions = LoadConfigOptions();
            Dictionary<String, String> CErpCEmbarque = new Dictionary<String, String>();
            ObservableCollection<ValeurOptionLogiciel> listValeurBlocageMateriel = new ObservableCollection<ValeurOptionLogiciel>();

            IEnumerable<XElement> mt = from row in ConfigOptions.Descendants("Produit")
                                        where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == product.ProductVidPid.Name
                                        select row;

            var queryOption = from row in mt.Descendants()
                              where row.Name == "Option"
                              select row;

            List<XElement> ListOptions = queryOption.ToList<XElement>();

            foreach (var option in ListOptions)
            { 
                var queryValeur = from node in option.Descendants()
                                  where node.Name == "Valeur"
                                  select node;
                foreach (var valeur in queryValeur)
                {
                    String Option = option.Attribute("valeur").Value;
                    String CodeERP = valeur.Attribute("Code").Value;
                    String CodeEmbarq = valeur.Attribute("Embarque").Value;
                    CErpCEmbarque.Add(Option + "_" + CodeERP, CodeEmbarq);

                    ValeurOptionLogiciel VOL = new ValeurOptionLogiciel();
                    VOL.CodeEmbarque = CodeEmbarq;
                    VOL.CodeERP = CodeERP;
                    VOL.NameOption = Option;
                    VOL.Libelle = valeur.Attribute("Libelle").Value;
                    listValeurBlocageMateriel.Add(VOL);
                }
            }

            CodeERPCodeEmbarque = CErpCEmbarque;
            ResolvOptionsLogiciels.ListValeurBlocageMateriel = listValeurBlocageMateriel;
        }
        private static XElement _refFile = null;

        /// <summary>
        /// La configuration logiciel portée par le produit
        /// </summary>
        public static UInt32 LoadOptionsLogicielsFromProduct(ConnectedProduct product, XMLCore.XMLProcessing Technique)
        {
            // 0 - initialisation
            UInt32 Result = ErrorCode.NO_ERROR;
            Int32 Value = 0;
            String NmrSimMoValue = null;
            Dictionary<String, Int32> listOptions = new Dictionary<String, Int32>();
            Dictionary<String, Int32>  listBlocage = new Dictionary<String, Int32>();
            // 1 - charger le fichier de référence permettant de parser le champ SimConfig
            XElement refFile = ResolvOptionsLogiciels.LoadConfigOptions();

            // 2 - parser le fichier de référence option par option
            IEnumerable<XElement> sim = from row in refFile.Descendants("Produit")
                                        where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == "SIM"
                                        select row;

            foreach (XElement Option in sim.First().Descendants("Option"))
            {
                String Key = Option.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                // Charger le chemin de la valeur dans le XML Technique
                String XMLPath;
                if (product.TypeProduct == ProductType.MT || product.TypeProduct == ProductType.SIM)
                {
                    XMLPath = Option.Descendants("PathTechniqueSIM").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                }
                else
                {
                    XMLPath = Option.Descendants("PathTechniqueMO").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                }

                ObservableCollection<XElement> variables = Technique.GetNodeByPath(XMLPath);
                if (variables != null)
                {
                    String OffsetStr = variables.First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value;
                    if (OffsetStr != "")
                    {
                        UInt32 Offset = Convert.ToUInt32(OffsetStr);
                        UInt32 SizeOctet = 0;
                        String DataType = variables.First().Attribute(XMLCore.XML_ATTRIBUTE.TYPE).Value;
                        switch (DataType)
                        {
                            case "int8_t":
                            case "uint8_t":
                                SizeOctet = 1;
                                break;
                            case "int16_t":
                            case "uint16_t":
                                SizeOctet = 2;
                                break;
                            case "int32_t":
                            case "uint32_t":
                                SizeOctet = 4;
                                break;
                            default:
                                break;
                        }

                        String StrValue;
                        if (product.TypeProduct == ProductType.MT || product.TypeProduct == ProductType.SIM)
                        {
                            StrValue = product.GetEEPROMValue(Offset, SizeOctet, Hid.CIBLE_HID_e.CIBLE_SIM);
                        }
                        else
                        {
                            StrValue = product.GetEEPROMValue(Offset, SizeOctet, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                        }

                        Value = Convert.ToInt32(StrValue, 16);
                        // Si la valeur est non renseignée dans le produit, la gérer comme valeur par défaut (0)
                        if (Value == 255)
                        {
                            Value = 0;
                        }
                    }
                }

                // 3 - stocker les options logiciels dans les propriétés statiques de la classe
                listOptions.Add(Key, Value);
            }
            // recuperation du num serie sim dans la zone de data des MO uniquement
            if (product.TypeProduct == ProductType.MO)
            {
                try
                {
                    ObservableCollection<XElement> variablessim = Technique.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/NmrSimTraceur");
                    if (variablessim != null)
                    {
                        String OffsetStr = variablessim.First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value;
                        if (OffsetStr != "")
                        {
                            UInt32 Offset = Convert.ToUInt32(OffsetStr);
                            String DataType = variablessim.First().Attribute(XMLCore.XML_ATTRIBUTE.TAILLE).Value;
                            UInt32 SizeOctet = Convert.ToUInt32(DataType);
                            NmrSimMoValue = product.GetEEPROMASCIIValue(Offset, SizeOctet, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                        }
                    }
                }
                catch
                {
                    // mise a jour de la fiche requise pour permettre la tracabilité
                }
            }
            ResolvOptionsLogiciels.ListOptions = listOptions;
            ResolvOptionsLogiciels.NmrSimTraceur = NmrSimMoValue;

            IEnumerable<XElement> leproduit = from row in refFile.Descendants("Produit")
                                              where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == product.ProductVidPid.Name
                                              select row;

            foreach (XElement Option in leproduit.First().Descendants("Option"))
            {
                String Key = Option.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                // Charger le chemin de la valeur dans le XML Technique
                String XMLPath;
                if (product.TypeProduct == ProductType.MT )
                {
                    XMLPath = Option.Descendants("PathTechniqueMT").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                }
                else
                {
                    XMLPath = Option.Descendants("PathTechniqueMO").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                }

                ObservableCollection<XElement> variables = Technique.GetNodeByPath(XMLPath);
                if (variables != null)
                {
                    String OffsetStr = variables.First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value;
                    if (OffsetStr != "")
                    {
                        UInt32 Offset = Convert.ToUInt32(OffsetStr);
                        UInt32 SizeOctet = 0;
                        String DataType = variables.First().Attribute(XMLCore.XML_ATTRIBUTE.TYPE).Value;
                        switch (DataType)
                        {
                            case "int8_t":
                            case "uint8_t":
                                SizeOctet = 1;
                                break;
                            case "int16_t":
                            case "uint16_t":
                                SizeOctet = 2;
                                break;
                            case "int32_t":
                            case "uint32_t":
                                SizeOctet = 4;
                                break;
                            default:
                                break;
                        }

                        String StrValue;
                        StrValue = product.GetEEPROMValue(Offset, SizeOctet, Hid.CIBLE_HID_e.CIBLE_EEP_0);
                        Value = Convert.ToInt32(StrValue, 16);
                        // Si la valeur est non renseignée dans le produit, la gérer comme valeur par défaut (0)
                        if (Value == 255)
                        {
                            Value = 0;
                        }
                    }
                }

                // 3 - stocker les options logiciels dans les propriétés statiques de la classe
                listBlocage.Add(Key, Value);
            }
            ResolvOptionsLogiciels.ListBlocageMateriel = listBlocage;
            return Result;
        } // endMethod: MAJOptionsLogiciels
        
        /// <summary>
        /// Sauver les options logiciels précédemment chargées dans le Xml (XMLProcessing)
        /// </summary>
        public static UInt32 SaveOptionsLogicielInFile ( XMLCore.XMLProcessing Technique )
        {
            UInt32 Result = PegaseCore.ErrorCode.NO_ERROR;
            
            // 1 - charger le fichier de référence
            XElement refFile = ResolvOptionsLogiciels.LoadConfigOptions();

            // 2 - pour chacun des paramètres sauvegardé, trouver le chemin d'enregistrement
            IEnumerable<XElement> simQuery = from row in refFile.Descendants("Produit")
                                        where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == "SIM"
                                        select row;

            XElement sim = simQuery.First();

            foreach (var option in ResolvOptionsLogiciels.ListOptions)
            {
                String OptionName = option.Key;
                var optionQuery = from row in refFile.Descendants("Option")
                                  where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == OptionName
                                  select row;

                if (optionQuery.Count() > 0)
                {
                    // 3 - enregistrer la valeur dans les paramètres applicatifs
                    XElement XOption = optionQuery.First();
                    String TPath = XOption.Descendants("PathTechniqueMO").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

                    ObservableCollection<XElement> Nodes = Technique.GetNodeByPath(TPath);

                    if (Nodes != null)
                    {
                        XElement Dest = Nodes.First();
                        Dest.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = option.Value.ToString();
                    }

                    // 4 - enregistrer les valeurs des paramètres complémentaires en fonction de la valeur de l'option
                    // s'ils existent
                    // 4.1 - trouver la correspondance de la valeur dans l'embarqué
                    IEnumerable<XElement> valeurQuery = from row in XOption.Descendants("Valeur")
                                                        where row.Attribute(XMLCore.XML_ATTRIBUTE.EMBARQUE).Value == option.Value.ToString()
                                                        select row;

                    // Si la valeur n'existe pas dans le descriptif XML, prendre la première valeur de la collection
                    if (valeurQuery.Count() == 0)
                    {
                        valeurQuery = new ObservableCollection<XElement>();
                        valeurQuery = XOption.Descendants("Valeur");
                    }

                    if (valeurQuery.Count() > 0)
                    {
                        XElement Valeur = valeurQuery.First();

                        var actionQuery = from row in Valeur.Descendants("Action")
                                            select row;
                        // Pour toutes les actions décrites, effectuer les actions requises
                        foreach (var action in actionQuery)
                        {
                            // Valeur de l'action
                            String ActStrValue = "";
                            if (action.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
                            {
                                ActStrValue = action.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                            }

                            String ActionPath = action.Attribute(XMLCore.XML_ATTRIBUTE.XMLPath).Value;
                            String FinalValue = "";
                            XElement XAction = null;
                            if (Technique.GetNodeByPath(ActionPath) != null)
                            {
                                XAction = Technique.GetNodeByPath(ActionPath).First();
                            }
                            else
                            {
                                if (ActionPath == "XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels")
                                {
                                    XElement XNbModes = Technique.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModes").First();
                                    XElement XNbModesReel = new XElement(XNbModes);
                                    XNbModes.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = "NbModesReels";
                                    XNbModes.AddAfterSelf(XNbModesReel);
                                    XAction = XNbModesReel;
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Incorrect file -> contact support");
                                    Result = PegaseCore.ErrorCode.IDIALOG_INTEGRITY_ERROR;
                                    return Result;
                                }
                            }
                            UInt32 CurrentValue;
                            String CurrentActionValue = XAction.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value.Trim();

                            // Si l'attribut valeur est présent
                            if (CurrentActionValue != "")
                            {
                                try
                                {
                                    if (CurrentActionValue.Length > 2 && CurrentActionValue.Substring(0,2) == "0x")
                                    {
                                        CurrentActionValue = CurrentActionValue.Remove(0, 2);
                                        CurrentValue = Convert.ToUInt32(CurrentActionValue, 16);
                                    }
                                    else
                                    {
                                        CurrentValue = Convert.ToUInt32(CurrentActionValue);
                                    }
                                }
                                catch
                                {
                                    CurrentValue = 0xFF;
                                }

                                if (ActStrValue.Contains("/"))
                                {
                                    // 1 - cas d'une plage de valeur -> vérifier que la valeur entrée est dans la plage. Si ce n'est pas le cas
                                    // utiliser la valeur min, prévenir l'utilisateur
                                    String[] PlageVal = ActStrValue.Split(new Char[] { '/' });
                                    Boolean IsInRange = false;

                                    foreach (String item in PlageVal)
                                    {
                                        Int32 IntItem = XMLCore.Tools.ConvertASCIIToInt32(item);
                                        if (CurrentValue == IntItem)
                                        {
                                            IsInRange = true;
                                        }
                                    }

                                    if (IsInRange)
                                    {
                                        FinalValue = CurrentActionValue;
                                    }
                                    else
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                        FinalValue = PlageVal[0];
                                    }
                                }
                                else if (ActStrValue.Contains("min"))
                                {
                                    // 2 - cas d'une liste de possibilité -> vérifier la valeur présente dans la fiche de paramétrage
                                    // si elle est absente, prévenir l'utilisateur, la remplacer par la valeur par défaut (0 = désactivé)
                                    UInt32 Min, Max;
                                    ResolvOptionsLogiciels.GetMinMax(out Min, out Max, ActStrValue);

                                    if (CurrentValue < Min || CurrentValue > Max)
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                        CurrentValue = Min;
                                    }
                                    FinalValue = CurrentValue.ToString();
                                }
                                else
                                {
                                    // 3 - cas d'une valeur unique -> enregistrer la valeur suivant le chemin
                                    FinalValue = ActStrValue;
                                    Int32 IntActStrValue = XMLCore.Tools.ConvertASCIIToInt32(ActStrValue);
                                    if (CurrentValue != IntActStrValue)
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                    }
                                }
                            }

                            // Si le chemin de lecture de la valeur est différent du chemin d'écriture, charger le chemin d'écriture
                            if (action.Attribute(XMLCore.XML_ATTRIBUTE.DESTPATH) != null)
                            {
                                ActionPath = action.Attribute(XMLCore.XML_ATTRIBUTE.DESTPATH).Value;
                                XAction = Technique.GetNodeByPath(ActionPath).First();
                            }
                            XAction.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = FinalValue;
                        }
                    }
                } //
            }

            return Result;
        } // endMethod: SaveOptionsLogicielInFile
        

        public static UInt32 SaveAndCheckBlocagelogInFile(ConnectedProduct produit, XMLCore.XMLProcessing Technique)
        {
            //ListValeurBlocageMateriel
            UInt32 Result = PegaseCore.ErrorCode.NO_ERROR;

            // 1 - charger le fichier de référence
            XElement refFile = ResolvOptionsLogiciels.LoadConfigOptions();

            // 2 - pour chacun des paramètres sauvegardé, trouver le chemin d'enregistrement
            IEnumerable<XElement> Query = from row in refFile.Descendants("Produit")
                                             where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == produit.ProductVidPid.Name
                                             select row;

            XElement queryproduit = Query.First();

            foreach (var option in ResolvOptionsLogiciels.ListBlocageMateriel)
            {
                
                String OptionName = option.Key;
                var optionQuery = from row in queryproduit.Descendants("Option")
                                  where row.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == OptionName
                                  select row;

                if (optionQuery.Count() > 0)
                {
                    // 3 - enregistrer la valeur dans les paramètres applicatifs
                    //XElement XOption = optionQuery.First();
                    //String TPath="";
                    //if (produit.IsMT)
                    //{
                    //     TPath = XOption.Descendants("PathTechniqueMT").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    //}
                    //else
                    //{
                    //     TPath = XOption.Descendants("PathTechniqueMO").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    //}
                    //// option materiel dans le produit
                    //ObservableCollection<XElement> Nodes = Technique.GetNodeByPath(TPath);
                    //string embarque = Nodes.First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    string embarque = option.Value.ToString();

                    if (Query.Count() > 0)
                    {
                        XElement Valeur = Query.First();
                        var valeurquery = from valeur in Valeur.Descendants("Valeur")
                                          where valeur.Attribute(XMLCore.XML_ATTRIBUTE.EMBARQUE).Value == embarque
                                          select valeur;
                        if (valeurquery== null || valeurquery.Count()==0)
                        {
                            valeurquery = from valeur in Valeur.Descendants("Valeur")
                                          where valeur.Attribute(XMLCore.XML_ATTRIBUTE.EMBARQUE).Value == "0"
                                          select valeur;
                        }
                        XElement Action = valeurquery.First();
                        var actionQuery = from row in Action.Descendants("Action")
                                          select row;
                        // Pour toutes les actions décrites, effectuer les actions requises
                        foreach (var action in actionQuery)
                        {
                            // Valeur de l'action
                            String ActStrValue = "";
                            if (action.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
                            {
                                ActStrValue = action.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                            }

                            String ActionPath = action.Attribute(XMLCore.XML_ATTRIBUTE.XMLPath).Value;
                            String FinalValue = "";
                            XElement XAction = null;
                            if (Technique.GetNodeByPath(ActionPath) != null)
                            {
                                XAction = Technique.GetNodeByPath(ActionPath).First();
                            }
                            else
                            {
                                if (ActionPath == "XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/TypeDeSecurite/NivSecu/NivSecuPLD/")
                                {
                                    XElement XNbModesReel = XElement.Parse("<Variable code='NivSecuPLD' valeur='0' />");


                                    XAction = XNbModesReel;
                                }
                                else if (ActionPath == "XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels")
                                {
                                    XElement XNbModes = Technique.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModes").First();
                                    XElement XNbModesReel = new XElement(XNbModes);
                                    XNbModes.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = "NbModesReels";
                                    XNbModes.AddAfterSelf(XNbModesReel);
                                    XAction = XNbModesReel;
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Incorrect file -> contact support");
                                    Result = PegaseCore.ErrorCode.IDIALOG_INTEGRITY_ERROR;
                                    return Result;
                                }
                            }
                            UInt32 CurrentValue;
                            String CurrentActionValue = XAction.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value.Trim();

                            // Si l'attribut valeur est présent
                            if (CurrentActionValue != "")
                            {
                                try
                                {
                                    if (CurrentActionValue.Length > 2 && CurrentActionValue.Substring(0, 2) == "0x")
                                    {
                                        CurrentActionValue = CurrentActionValue.Remove(0, 2);
                                        CurrentValue = Convert.ToUInt32(CurrentActionValue, 16);
                                    }
                                    else
                                    {
                                        CurrentValue = Convert.ToUInt32(CurrentActionValue);
                                    }
                                }
                                catch
                                {
                                    CurrentValue = 0xFF;
                                }

                                if (ActStrValue.Contains("/"))
                                {
                                    // 1 - cas d'une plage de valeur -> vérifier que la valeur entrée est dans la plage. Si ce n'est pas le cas
                                    // utiliser la valeur min, prévenir l'utilisateur
                                    String[] PlageVal = ActStrValue.Split(new Char[] { '/' });
                                    Boolean IsInRange = false;

                                    foreach (String item in PlageVal)
                                    {
                                        Int32 IntItem = XMLCore.Tools.ConvertASCIIToInt32(item);
                                        if (CurrentValue == IntItem)
                                        {
                                            IsInRange = true;
                                        }
                                    }

                                    if (IsInRange)
                                    {
                                        FinalValue = CurrentActionValue;
                                    }
                                    else
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                        FinalValue = PlageVal[0];
                                    }
                                }
                                else if (ActStrValue.Contains("min"))
                                {
                                    // 2 - cas d'une liste de possibilité -> vérifier la valeur présente dans la fiche de paramétrage
                                    // si elle est absente, prévenir l'utilisateur, la remplacer par la valeur par défaut (0 = désactivé)
                                    UInt32 Min, Max;
                                    ResolvOptionsLogiciels.GetMinMax(out Min, out Max, ActStrValue);

                                    if (CurrentValue < Min || CurrentValue > Max)
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                        CurrentValue = Min;
                                    }
                                    FinalValue = CurrentValue.ToString();
                                }
                                else
                                {
                                    // 3 - cas d'une valeur unique -> enregistrer la valeur suivant le chemin
                                    FinalValue = ActStrValue;
                                    Int32 IntActStrValue = XMLCore.Tools.ConvertASCIIToInt32(ActStrValue);
                                    if (CurrentValue != IntActStrValue)
                                    {
                                        String message = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/OPTION_LOG_INCONNUE"), LanguageSupport.Get().GetText("/MAJMOMT/" + option.Key));
                                        String messagetitre = String.Format(LanguageSupport.Get().GetText("/MAJMOMT/INFORMATION"));
                                        System.Windows.MessageBox.Show(message, messagetitre);
                                    }
                                }
                            }

                            // Si le chemin de lecture de la valeur est différent du chemin d'écriture, charger le chemin d'écriture
                            if (action.Attribute(XMLCore.XML_ATTRIBUTE.DESTPATH) != null)
                            {
                                ActionPath = action.Attribute(XMLCore.XML_ATTRIBUTE.DESTPATH).Value;
                                if (Technique.GetNodeByPath(ActionPath) != null)
                                {
                                    XAction = Technique.GetNodeByPath(ActionPath).First();
                                }
                            }
                            XAction.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = FinalValue;
                        }
                    }
                } //
            }
            return Result;
        }
        /// <summary>
        /// Acquérir les valeurs Min Max contenue dans range
        /// </summary>
        private static void GetMinMax ( out UInt32 Min, out UInt32 Max, String range )
        {
            String[] Param = range.Split(new Char[] { ';' });
            String[] mins = Param[0].Split(new Char[] { ':' });
            String[] maxs = Param[1].Split(new Char[] { ':' });

            Min = Convert.ToUInt32(mins[1]);
            Max = Convert.ToUInt32(maxs[1]);
        } // endMethod: GetMinMax

        /// <summary>
        /// Charger le fichier de référence
        /// </summary>
        private static XElement LoadConfigOptions ( )
        {
            XElement Result = null;
            // Si le fichier n'a jamais été ouvert, l'ouvrir.
            // Sinon, renvoyer le fichier conservé en mémoire
            if (_refFile == null)
            {
                FileCore.FilePackage package = null;
                // Ouvrir le package

                package = FileCore.FilePackage.GetParamData();

                if (package != null)
                {
                    // Ouvrir la partie
                    Uri partUri = new Uri("/ConfigOptions.xml", UriKind.Relative);
                    Stream partStream = package.GetPartStream(partUri);

                    if (partStream != null)
                    {
                        XDocument doc = XDocument.Load(partStream);
                        Result = doc.Root;
                    }
                    else
                    {
                        Result = null;
                    }
                    _refFile = Result;
                    package.ClosePackage();
                }
            }
            else
            {
                Result = _refFile;
            }

            return Result;
        } // endMethod: LoadConfigOptions

        #endregion
    }
}
