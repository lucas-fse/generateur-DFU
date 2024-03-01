using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;
using System.Xml.Linq;
using JAY.PegaseCore.Helper;
using JAY.PegaseCore;


namespace JAY.PegaseCore.Rapport
{
    /// <summary>
    /// Classe de construction d'un rapport à partir d'un template Word
    /// </summary>
    public class RapportBuilder
    {
        // Variables constantes
        #region Constantes

        private const String DEFAULT_VALUE = "-";
        private const String FIELD_CHAR = "fldChar";
        private const String FIELD_SIMPLE = "fldSimple";
        private const String FIELD_NAME = "instrText";
        private const String TEXT = "t";
        private const String ATTRIBUTE_FIELD = "w:instr";
        private const String TAG_MERGEFIELD = "MERGEFIELD";
        private const String TAG_MERGEFORMAT = "\\* MERGEFORMAT";

        private const String MODE = "mode";
        private const String CARTE = "btnrelais";
        private const String MODE_CODE_SHAPE = "_M";
        private const String CARTE_CODE_SHAPE = "";

        #endregion

        // Variables
        #region Variables

        private RapportData _rapportData;
        private Package _wordPackage;
        private String _reportLanguage;
        private Int32 _compteur;
        private XElement _rapportCablageRow;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La ligne de cablage du rapport
        /// </summary>
        public XElement RapportCablageRow
        {
            get
            {
                return this._rapportCablageRow;
            }
            private set
            {
                this._rapportCablageRow = value;
            }
        } // endProperty: RapportCablageRow

        /// <summary>
        /// La langue utilisée pour le rapport
        /// </summary>
        public String ReportLanguage
        {
            get
            {
                return this._reportLanguage;
            }
            private set
            {
                this._reportLanguage = value;
            }
        } // endProperty: ReportLanguage

        /// <summary>
        /// Les données utilisées pour le rapport
        /// </summary>
        public RapportData Data
        {
            get
            {
                if (this._rapportData == null)
                {
                    this._rapportData = new RapportData();
                }

                return this._rapportData;
            }
        } // endProperty: Data

        #endregion

        // Constructeur
        #region Constructeur

        public RapportBuilder()
        {
            // Initialiser tous les blocs de construction du rapport

            // 1 - Rapport cablage Value
            String FileValue;
            FileCore.FilePackage package = FileCore.FilePackage.GetMaterielData();
            Stream stream = package.GetPartStream(new Uri(DefaultValues.Get().RapportCablageRow, UriKind.Relative));
            if (stream != null)
            {
                using (StreamReader SR = new StreamReader(stream))
                {
                    FileValue = SR.ReadToEnd();
                    XElement element = XElement.Parse(FileValue);
                    this.RapportCablageRow = element;
                }
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Rechercher le texte de remplacement d'une balise
        /// </summary>
        public XNode Findstring ( string CurrentText)
        {
            XNode Result, Data;
            String Text = CurrentText;
            String Balise;
            int num = 0;

            Data = new XText(Text);

            if (CurrentText.Length > 4)
            {
                Int32 pos = CurrentText.LastIndexOf("$BALISE");
                if (pos > -1)
                {
                    CurrentText = CurrentText.Substring(pos + "$BALISE".Length, 4);
                    num = XMLCore.Tools.ConvertASCIIToInt16(CurrentText);
                }

                Balise = String.Format("$BALISE{0:0000}", num);

                switch (num)
                {
                    case 0001:
                        Data = this.Data.Fields[RapportData.NUMFICHE];
                        break;
                    case 0010:
                        Data = this.Data.Fields[RapportData.MO_REF];
                        break;
                    case 0011:
                        Data = this.Data.Fields[RapportData.MO_NUM_SERIE];
                        break;
                    case 0012:
                        Data = this.Data.Fields[RapportData.MO_CODE_ID];
                        break;
                    case 0013:
                        Data = this.Data.Fields[RapportData.MO_AUX];
                        break;
                    case 0014:
                        Data = new XText(" "); //this.Data.Fields[RapportData.MO_AUX];
                        break;
                    case 0015:
                        Data = this.Data.Fields[RapportData.MO_IR_STUP];
                        break;
                    case 0016:
                        Data = this.Data.Fields[RapportData.MO_ACCEL];
                        break;
                    case 0017:
                        Data = this.Data.Fields[RapportData.MO_VIBREUR];
                        break;
                    case 0018:
                        Data = this.Data.Fields[RapportData.MO_BUZZER];
                        break;
                    case 0019:
                        Data =  new XText(" "); //this.Data.Fields[RapportData.MO_IR_STUP];
                        break;
                    case 0020:
                        Data = this.Data.Fields[RapportData.MO_SBC_BEHAVIOR];
                        break;
                    case 0030:
                        Data = this.Data.Fields[RapportData.SIM_REF];
                        break;
                    case 0032:
                        Data = this.Data.Fields[RapportData.SIM_NUM_SERIE];
                        break;
                    case 0033:
                        Data = this.Data.Fields[RapportData.MT_FREQ];
                        break;
                    case 0034:
                        Data = this.Data.Fields[RapportData.SIM_ARRET_PASSIF];
                        break;
                    case 0035:
                        Data = this.Data.Fields[RapportData.SIM_STANDBY];
                        break;
                    case 0036:
                        Data = this.Data.Fields[RapportData.SIM_OPTION_IR];
                        break;
                    case 0037:
                        Data = this.Data.Fields[RapportData.SIM_MODES_OPT];
                        break;
                    case 0038:
                        Data = this.Data.Fields[RapportData.SIM_SYNCHRO_OPT];
                        break;
                    case 0039:
                        Data = this.Data.Fields[RapportData.SIM_DEADMAN_OPT];
                        break;
                    case 0051:
                        Data = this.Data.Fields[RapportData.MT_REFERENCE];
                        break;
                    case 0052:
                        Data = this.Data.Fields[RapportData.MT_NUMERO_SERIE];
                        break;
                    case 0053:
                        Data = this.Data.Fields[RapportData.MT_CODE_ID];
                        break;
                    case 0054:
                        Data = this.Data.Fields[RapportData.MT_TYPE_CABLE];
                        break;
                    case 0055:
                        Data = this.Data.Fields[RapportData.MT_KLAXON];
                        break;
                    case 0057:
                        if (this.Data.Fields.TryGetValue("NomCarte02", out Data))
                        {
                            if (PegaseData.Instance.ModuleT.TypeMT == MT.ALTO)
                            {
                                if (Data is XText)
                                {
                                    Text = "Slot A - " + ((XText)Data).Value;
                                    ((XText)Data).Value = Text;
                                }
                                else
                                {
                                    Data = new XText(" ");
                                }
                            }
                        }
                        else
                        {
                            Data = new XText(" ");
                        }
                        break;
                    case 0058:
                        if (this.Data.Fields.TryGetValue("NomCarte03", out Data))
                        {
                            if (PegaseData.Instance.ModuleT.TypeMT == MT.ALTO)
                            {
                                if (Data is XText)
                                {
                                    Text = "Slot B - " + ((XText)Data).Value;
                                    ((XText)Data).Value = Text;
                                }
                                else
                                {
                                    Data = new XText(" ");
                                }
                            }
                        }
                        else
                        {
                            Data = new XText(" ");
                        }
                        break;
                    case 0059:
                        if (this.Data.Fields.TryGetValue("NomCarte04", out Data))
                        {
                            if (PegaseData.Instance.ModuleT.TypeMT == MT.ALTO)
                            {
                                if (Data is XText)
                                {
                                    Text = "Slot C - " + ((XText)Data).Value;
                                    ((XText)Data).Value = Text;
                                }
                                else
                                {
                                    Data = new XText(" ");
                                }
                            }
                        }
                        else
                        {
                            Data = new XText(" ");
                        }
                        break;
                    case 0061:
                        ObservableCollection<ImgDefinition> ImgMO;
                        ImgDefinition ImgMoPrincipal;
                        DecodConfigRefIndusAppl.Get().GetRapportImg(PegaseData.Instance.MOperateur.RefIndus.Substring(0, 2), PegaseData.Instance.RefErpMO, out ImgMoPrincipal, out ImgMO);

                        // Construction de la section pour l'image principal
                        XElement imagesMO = new XElement("div");
                        XAttribute MOAttrib = new XAttribute("id", "imagesmo");
                        imagesMO.Add(MOAttrib);

                        XElement imagesboitier = new XElement("div");
                        MOAttrib = new XAttribute("id", "imagesboitier");
                        imagesboitier.Add(MOAttrib);

                        XElement image1 = XElement.Parse(this.Data.BuildBaliseImgRapport(ImgMoPrincipal.Image1, 0, 0));

                        imagesboitier.Add(image1);
                        imagesMO.Add(imagesboitier);

                        // Construction des images secondaires
                        XElement imagesacc = new XElement("div");
                        MOAttrib = new XAttribute("id", "imagesacc");
                        imagesacc.Add(MOAttrib);

                        if (ImgMO.Count > 0)
                        {
                            for (int i = 0; i < ImgMO.Count && i < 2; i++)
                            {
                                XElement imgAcc = XElement.Parse(this.Data.BuildBaliseImgRapport(ImgMO[i].Image1, 0, 0));
                                imagesacc.Add(imgAcc);
                            }
                        }
                        else
                        {
                            imagesacc.Add(new XText(" "));
                        }

                        imagesMO.Add(imagesacc);

                        Data = imagesMO;
                        break;
                    case 0062:
                        System.Windows.MessageBox.Show("Image MO 0062");
                        break;
                    case 0063:
                        System.Windows.MessageBox.Show("Image MO 0063");
                        break;
                    case 0064:
                        // Images MT nouveau rapport
                        ObservableCollection<ImgDefinition> ImgMT;
                        ImgDefinition ImgPrincipal;
                        DecodConfigRefIndusAppl.Get().GetRapportImg(PegaseData.Instance.ModuleT.RefIndus.Substring(0, 2), PegaseData.Instance.RefErpMT, out ImgPrincipal, out ImgMT);
                        XElement imagesMT = new XElement("div");
                        if (ImgPrincipal.Image1 != "")
                        {
                            XElement img1 = XElement.Parse(this.Data.BuildBaliseImgRapport(ImgPrincipal.Image1, 0, 0));
                            imagesMT.Add(img1);
                        }
                        if (ImgMT.Count > 0)
                        {
                            foreach (var img in ImgMT)
                            {
                                XElement img2 = XElement.Parse(this.Data.BuildBaliseImgRapport(img.Image2, 0, 0));
                                imagesMT.Add(img2);
                            }
                        }
                        Data = imagesMT;
                        break;
                    case 0072:
                        Data = this.Data.Fields[RapportData.TABLE_BOUTON1];
                        break;
                    case 0070:
                        Data = this.Data.Fields[RapportData.TABLE_BOUTON2];
                        break;
                    case 0100:
                        Data = this.Data.Fields[RapportData.TABLE_CABLAGE];
                        break;
                    case 0216:
                        Data = this.Data.Fields[RapportData.MT_CODE_CONFIGURATEUR];
                        break;
                    case 0217:
                        Data = this.Data.Fields[RapportData.MO_CODE_CONFIGURATEUR];
                        break;
                    case 0218:
                        Data = this.Data.Fields[RapportData.SIM_CODE_CONFIGURATEUR];
                        break;
                    case 0220:
                        Data = this.Data.Fields[RapportData.COMMENTAIRE_PRINCIPALE];
                        break;
                    default:
                        Boolean IsValueExist;
                        IsValueExist = this.Data.Fields.TryGetValue(String.Format("$BALISE{0:0000}", num), out Data);

                        if (!IsValueExist)
	                    {
                            Data = new XText(" ");
	                    }
                        break;
                }

                if (Data is XText)
                {
                    XText XT = Data as XText;
                    // Remplacer la balise par le texte
                    Text = Text.Replace(Balise, XT.Value);
                    XT.Value = Text;
                    Data = XT;
                }
            }

            Result = Data;

            return Result;
        } // endMethod: Findstring

        /// <summary>
        /// Remplacer les balises du noeud enfant
        /// </summary>
        public void CompleteChildNode ( XNode Node )
        {
            XText tmp;
            if (Node != null)
            {
                if (Node is XText)
                {
                    tmp = (XText)Node;
                    XNode Content = this.Findstring(tmp.Value);
                    if (Content != null)
                    {
                        try
                        {
                            Node.ReplaceWith(Content);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        Node.ReplaceWith(new XText(" "));
                    }
                }
            }
        } // endMethod: CompleteChildNode



        public void BuildNewDoc(String FileName, String langue)
        {
            String CurrentLanguage = LanguageSupport.Get().LanguageName;
            LanguageSupport.Get().InitialiseLanguage(langue, DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);

            XElement Data = new XElement("params");
            XElement Langue = this.Data.InitNewLangue(langue);
            Data = this.Data.InitNewTradReport(Data, langue);
            Data = this.Data.InitNewParamSimple(Data, langue);
            Data = this.Data.InitNewData(Data,langue);
            Data = this.Data.InitNewTableauMode(Data, langue);
            
            Data = this.Data.InitNewMode(Data,langue);
            //XElement Alarme = this.Data.InitNewAlarme();
            Data = this.Data.InitNewMt(Data, langue);
            Data = this.Data.InitNewMo(Data, langue);
            Data = this.Data.InitNewAlarm(Data, langue);
            Data = this.Data.InitNewIO(Data, langue);
            Data = this.Data.initNewNetwork(Data, langue);
            Data = this.Data.initNewPlastron(Data, langue);

            try
            {
                //using (FileStream stream = File.Open(FileName, System.IO.FileMode.Create))
                //{
                //    //fichier existe 

                //    XDocument Rapport = new XDocument();//.Load(stream);
                //    XElement NiveauRoot = new XElement("Rapport");
                //    NiveauRoot.Add(Langue);
                //    NiveauRoot.Add(Data);
                //    //NiveauRoot.Add(Mode);
                //    //NiveauRoot.Add(Alarme);
                //    Rapport.Add(NiveauRoot);
                //    Rapport.Save(stream);
                    
                //}
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, false))
                {
                    XDocument Rapport = new XDocument();//.Load(stream);
                    XElement NiveauRoot = new XElement("Rapport");
                    NiveauRoot.Add(Langue);
                    NiveauRoot.Add(Data);
                    //NiveauRoot.Add(Mode);
                    //NiveauRoot.Add(Alarme);
                    Rapport.Add(NiveauRoot);
                    string XmlString = Rapport.ToString();
                    Xml xml = new Xml(XmlString);
                    var actual = xml.LinearizeXml;
                    actual = "function xmlR(){\r\n var xml = '" + actual + "'\r\n return xml;\r\n}";
                    file.WriteLine(actual);
                }
            }
            catch
            {

            }
            LanguageSupport.Get().InitialiseLanguage(CurrentLanguage, DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);
        }
        /// <summary>
        /// Initialiser les données
        /// </summary>
        public void BuildDoc ( String FileName, String Langue )
        {
            // Initialiser les données
            String CurrentLanguage = LanguageSupport.Get().LanguageName;
            LanguageSupport.Get().InitialiseLanguage(Langue, DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);
            this.Data.InitData(Langue);

            XDocument doc = XDocument.Load(FileName);
            XElement root = doc.Root;

            // Construire les tables
            foreach (XElement child in root.Descendants("div"))
            {
                // rechercher une div avec un type 'section'
                if (child.Attribute("type") != null)
                {
                    if (child.Attribute("type").Value == "section")
                    {
                        // rechercher une div avec un type 'table'
                        foreach (XElement div in child.Descendants("div"))
                        {
                            if (div.Attribute("type") != null)
                            {
                                if (div.Attribute("type").Value == "table")
                                {
                                    if (PegaseData.Instance.ModuleT.Cartes.Count > 1)
                                    {
                                        // Duppliquer autant de fois que nécessaire
                                        for (int i = 1; i < PegaseData.Instance.ModuleT.Cartes.Count; i++)
                                        {
                                            XElement NewTable = new XElement(div);
                                            NewTable = this.CreateCableTable(NewTable, PegaseData.Instance.ModuleT.Cartes[i]);
                                            child.Add(NewTable);
                                        }
                                    }
                                    if (PegaseData.Instance.ModuleT.Cartes.Count > 0)
                                    {
                                        XElement Table;
                                        Table = this.CreateCableTable(div, PegaseData.Instance.ModuleT.Cartes[0]);
                                        div.ReplaceWith(Table);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            // Remplacer les balises
            List<XNode> nodes = root.DescendantNodes().ToList<XNode>();

            for (int i = 0; i < nodes.Count; i++)
            {
                this.CompleteChildNode(nodes[i]);
            }

            // Mettre à jour le nom des images

            String HTMLContent = root.ToString();

            using (StreamWriter SW = new StreamWriter(File.Open(FileName, FileMode.Create), Encoding.UTF8))
            {
                SW.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd_\"> ");
                SW.WriteLine(SW.NewLine);
                SW.Write(HTMLContent);
                SW.Flush();
            }

            LanguageSupport.Get().InitialiseLanguage(CurrentLanguage, DefaultValues.Get().IDialogLanguageFolder, PegaseData.Instance.CurrentPackage);
        } // endMethod: InitData

        /// <summary>
        /// Le noeud source
        /// </summary>
        public XElement CreateCableTable ( XElement sourceNode, Carte carte )
        {
            XElement Result, tableNode;
            this._compteur = 0;
            Int32 NbRow;

            ObservableCollection<CablageRow> cablageRows;
            tableNode = sourceNode.Descendants("table").First();

            NbRow = this._rapportData.InitCarteMode(carte, out cablageRows);

            foreach (var row in cablageRows)
            {
                XElement newRow = row.BuildRow(this.RapportCablageRow);
                tableNode.Add(newRow);
            }

            String XML = sourceNode.ToString();
            XML = XML.Replace("$BALISE0100", carte.Nom);

            Result = XElement.Parse(XML);
            return Result;
        } // endMethod: CreateCableTable

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: RapportBuilder
}
