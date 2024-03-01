using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les entrées sorties analogiques
    /// </summary>
    public class ESTOR : ES
    {
        // Variables
        #region Variables

        private XMLCore.XMLProcessing _section1;        // Section Eana ou Sana
        private XMLCore.XMLProcessing _section2;        // Section PilotageES
        private XMLCore.XMLProcessing _section3;        // Section Affectation ou Reglage
        private XMLCore.XMLProcessing _section4;        // Section ReglageSecurite
        private Boolean                  _isPWM;        // La ESTOR est-elle en mode PWM ?

        private ObservableCollection<String> _listEtatInitial;
        private ObservableCollection<String> _listEtatSecurite;

        private Int32 _pwmFrequence;
        private Int32 _pwmCyclique;
        private String _pwmFrequencePath;
        private String _pwmCycliquePath;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Libellé pour la traduction de Fréquence
        /// </summary>
        public String LabelFrequence
        {
            get
            {
                return "CHECK_MATERIEL/FREQUENCY";
            }
        } // endProperty: LabelFrequence

        /// <summary>
        /// Libellé pour la traduction de rapport cyclique
        /// </summary>
        public String LabelCyclicRatio
        {
            get
            {
                return "CHECK_MATERIEL/RAPPORT_CYCLIQUE";
            }
        } // endProperty: LabelCyclicRatio

        /// <summary>
        /// Libellé pour l'onglet Valeur initiale
        /// </summary>
        public String LabelValInitiale
        {
            get
            {
                return "CHECK_MATERIEL/VAL_INITIALE";
            }
        } // endProperty: LabelValInitiale

        /// <summary>
        /// Libellé pour l'onglet Valeur en sécurité
        /// </summary>
        public String LabelValSecurite
        {
            get
            {
                return "CHECK_MATERIEL/VAL_SECURITE";
            }
        } // endProperty: LabelValSecurite

        /// <summary>
        /// La fréquence PWM dans le cas du TIMO, si PWM est sélectionné
        /// </summary>
        public Int32 PWMFrequence
        {
            get
            {
                return this._pwmFrequence;
            }
            set
            {
                if (value > 0 && value <= 1000)
                {
                    this._pwmFrequence = value;
                    PegaseData.Instance.XMLFile.SetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, value.ToString());
                    PegaseData.Instance.ModuleT.MAJSTorTimo4PWM(this);
                }
            }
        } // endProperty: PWMFrequence

        /// <summary>
        /// Le type de Template
        /// </summary>
        public String TemplateType
        {
            get
            {
                String Result = "";
                // Si c'est une sortie sur un Timo
                if (PegaseData.Instance.ModuleT.TypeMT == MT.TIMO && this.Type == TypeES.STor)
                {
                    if (this.IsPWM)
                    {
                        Result = MT.TIMO + "PWM";
                    }
                    else
                    {
                        Result = MT.TIMO;
                    }
                }

                return Result;
            }
        } // endProperty: MTType

        /// <summary>
        /// Le rapport cyclique du PWM pour le Timo
        /// </summary>
        public Int32 PWMCyclique
        {
            get
            {
                return this._pwmCyclique;
            }
            set
            {
                if (value >= 1 && value <=90)
                {
                    this._pwmCyclique = value;
                    PegaseData.Instance.XMLFile.SetValue(this._pwmCycliquePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, (value * 10).ToString()); 
                }
            }
        } // endProperty: PWMCyclique

        /// <summary>
        /// L'ESTOR est-elle en PWM ?
        /// </summary>
        public Boolean IsPWM
        {
            get
            {
                return this._isPWM;
            }
            set
            {
                this._isPWM = value;
                if (!this._isPWM)
                {
                    PegaseData.Instance.XMLFile.SetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, "0");
                }
                else
                {
                    if (this.PWMFrequence == 0)
                    {
                        this.PWMFrequence = 1;
                    }
                    PegaseData.Instance.XMLFile.SetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, this.PWMFrequence.ToString());
                }
                RaisePropertyChanged("IsPWM");
                RaisePropertyChanged("TemplateType");
            }
        } // endProperty: IsPWM

        /// <summary>
        /// La liste des états de sécurité
        /// </summary>
        public ObservableCollection<String> ListEtatSecurite
        {
            get
            {
                if (this._listEtatSecurite == null)
                {
                    this._listEtatSecurite = new ObservableCollection<String>();
                    this._listEtatSecurite.Add("ON ");
                    this._listEtatSecurite.Add("OFF");
                    this._listEtatSecurite.Add(" - ");
                }

                return this._listEtatSecurite;
            }
        } // endProperty: ListEtatSecurite

        /// <summary>
        /// La liste des états initiaux disponibles
        /// </summary>
        public ObservableCollection<String> ListEtatInitial
        {
            get
            {
                if (this._listEtatInitial == null)
                {
                    this._listEtatInitial = new ObservableCollection<String>();
                    this._listEtatInitial.Add("ON ");
                    this._listEtatInitial.Add("OFF");
                }

                return this._listEtatInitial;
            }
        } // endProperty: ListEtatInitial

        /// <summary>
        /// Id de l'entrée ou de la sortie Ana
        /// </summary>
        public String IdESAna
        {
            get
            {
                String Result = "";
                IEnumerable<XElement> ids = this._section1.GetNodeByPath("Id");
                if (ids.Count() > 0)
                {
                    XElement XE = ids.First();
                    Result = XE.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                }
                return Result;
            }
        } // endProperty: IdESAna

        /// <summary>
        /// La position de la carte dans le 
        /// </summary>
        public String PositionCarte
        {
            get
            {
                return this._section1.GetNodeByPath("IdCarte").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        } // endProperty: IdCarte

        /// <summary>
        /// Le fonctionnement prévu
        /// </summary>
        public String Fonctionnement
        {
            get
            {
                return this._section1.GetNodeByPath("Fonctionnement").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        } // endProperty: Fonctionnement

        /// <summary>
        /// La famille de la donnée
        /// </summary>
        public PegaseCore.FamilleDeDonnees_e Famille
        {
            get
            {
                Int32 Value;
                String SV = this._section2.GetNodeByPath("IndiceFamille").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                try
                {
                    Value = Convert.ToInt32(SV);
                }
                catch
                {
                    Value = 0;
                }
                return (PegaseCore.FamilleDeDonnees_e)Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        } // endProperty: Famille

        /// <summary>
        /// L'indice de la donnée
        /// </summary>
        public Int32 Indice
        {
            get
            {
                Int32 Value;
                String SV = this._section2.GetNodeByPath("IndiceES").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                try
                {
                    Value = Convert.ToInt32(SV);
                }
                catch
                {
                    Value = 0;
                }
                return Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        } // endProperty: Indice

        /// <summary>
        /// Le gain de l'entrée ou de la sortie
        /// </summary>
        public Single Gain
        {
            get
            {
                Single Value;
                String SV;

                SV = this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                SV = JAY.XMLCore.Tools.FixFloatStringSeparator(SV);
                Value = Convert.ToSingle(SV);
                return Value;
            }
            set
            {
                String SV = value.ToString();
                this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Gain

        /// <summary>
        /// L'offset de l'entrée ou de la sortie
        /// </summary>
        public Single Offset
        {
            get
            {
                Single Value;
                String SV;

                SV = this._section3.GetNodesByCode("Offset").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                SV = JAY.XMLCore.Tools.FixFloatStringSeparator(SV);
                Value = Convert.ToSingle(SV);
                return Value;
            }
            set
            {
                String SV = value.ToString();
                this._section3.GetNodesByCode("Offset").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;
            }
        } // endProperty: Offset

        /// <summary>
        /// Le type de fonctionnement
        /// </summary>
        public TypeUI UIType
        {
            get
            {
                String SV;
                Int32 Value;

                SV = this._section3.GetNodesByCode("TypeUI").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                try
                {
                    Value = Convert.ToInt32(SV);
                }
                catch
                {
                    Value = 0;
                }
                return (TypeUI)Value;
            }
            set
            {
                this._section3.GetNodesByCode("TypeUI").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = ((Int32)value).ToString();
            }
        } // endProperty: TypeUI

        /// <summary>
        /// L'unité de la grandeur manipulée
        /// </summary>
        public String Unité
        {
            get
            {
                String SV;

                SV = this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.UNIT).Value;
                return SV;
            }
            set
            {
                this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.UNIT).Value = value;
            }
        } // endProperty: Unité

        /// <summary>
        /// L'unité de U ou de I
        /// </summary>
        public String UIUnit
        {
            get
            {
                String Value;

                switch (this.UIType)
                {
                    case TypeUI.TUI_0a10V:
                    case TypeUI.TUI_m10ap10V:
                    case TypeUI.TUI_vref:
                        Value = "V";
                        break;
                    case TypeUI.TUI_0a20mA:
                    case TypeUI.TUI_4a20mA:
                        Value = "mA";
                        break;
                    default:
                        Value = "";
                        break;
                }
                return Value;
            }
        } // endProperty: UIUnit

        /// <summary>
        /// La constante de transformation du courant en valeur numérique
        /// permettant de calculer le gain et l'offset
        /// </summary>
        public Single ValCsteUIMax
        {
            get
            {
                Single Result = 0f;

                switch (this.UIType)
                {
                    case TypeUI.TUI_0a10V:
                        Result = 32732f;
                        break;
                    case TypeUI.TUI_m10ap10V:
                        break;
                    case TypeUI.TUI_vref:
                        break;
                    case TypeUI.TUI_0a20mA:
                        break;
                    case TypeUI.TUI_4a20mA:
                        Result = 25820f;
                        break;
                    default:
                        Result = 0f;
                        break;
                }

                return Result;
            }
        } // endProperty: ValCsteUIMax

        /// <summary>
        /// La constante de transformation du courant en valeur numérique
        /// permettant de calculer le gain et l'offset
        /// </summary>
        public Single ValCsteUIMin
        {
            get
            {
                Single Result = 0f;

                switch (this.UIType)
                {
                    case TypeUI.TUI_0a10V:
                        Result = 0f;
                        break;
                    case TypeUI.TUI_m10ap10V:
                        break;
                    case TypeUI.TUI_vref:
                        break;
                    case TypeUI.TUI_0a20mA:
                        break;
                    case TypeUI.TUI_4a20mA:
                        Result = 5156f;
                        break;
                    default:
                        Result = 0f;
                        break;
                }

                return Result;
            }
        } // endProperty: ValCsteUIMin

        /// <summary>
        /// La valeur minimal de tension ou d'intensité
        /// </summary>
        public Single ValUIMin
        {
            get
            {
                Single Value;

                switch (this.UIType)
                {
                    case TypeUI.TUI_m10ap10V:
                        Value = -10f;
                        break;
                    case TypeUI.TUI_0a10V:
                    case TypeUI.TUI_vref:
                    case TypeUI.TUI_0a20mA:
                        Value = 0f;
                        break;
                    case TypeUI.TUI_4a20mA:
                        Value = 4f;
                        break;
                    default:
                        Value = 0f;
                        break;
                }
                return Value;
            }
        } // endProperty: ValUIMin

        /// <summary>
        /// La valeur maximal de tension ou d'intensité
        /// </summary>
        public Single ValUIMax
        {
            get
            {
                Single Value;

                switch (this.UIType)
                {
                    case TypeUI.TUI_0a10V:
                    case TypeUI.TUI_m10ap10V:
                        Value = 10f;
                        break;
                    case TypeUI.TUI_vref:
                    case TypeUI.TUI_0a20mA:
                    case TypeUI.TUI_4a20mA:
                        Value = 20f;
                        break;
                    default:
                        Value = 10f;
                        break;
                }
                return Value;
            }
        } // endProperty: ValUIMax

        /// <summary>
        /// La valeur numérique minimale : ValNumMin = ValUIMin * gain + offset
        /// </summary>
        public Single ValNumMin
        {
            get
            {
                // calculer la valeur avec le gain et l'offset
                Single Value;
                Value = this.Gain * this.ValCsteUIMin + this.Offset;
                Value = (Single)Math.Ceiling(Value);
                return Value;
            }
            set
            {
                // calculer le gain et l'offset en fonction de la valeur
                Single G;
                Single O;

                G = (this.ValNumMax - value)/(this.ValCsteUIMax - this.ValCsteUIMin);
                O = value - this.ValCsteUIMin * G;

                this.Gain = G;
                this.Offset = O;
            }
        } // endProperty: ValNumMin

        /// <summary>
        /// La valeur numérique maximale : ValNumMin = ValUIMax * gain + offset
        /// </summary>
        public Single ValNumMax
        {
            get
            {
                // calculer la valeur avec le gain et l'offset
                Single Value;
                Value = this.Gain * this.ValCsteUIMax + this.Offset;
                Value = (Single)Math.Ceiling(Value);
                return Value;
            }
            set
            {
                // calculer le gain et l'offset en fonction de la valeur
                Single G;
                Single O;

                G = (value - this.ValNumMin) / (this.ValCsteUIMax - this.ValCsteUIMin);
                O = value - this.ValCsteUIMax * G;

                this.Gain = G;
                this.Offset = O;
            }
        } // endProperty: ValNumMax

        /// <summary>
        /// Le nom complet de l'entrée / sortie
        /// </summary>
        public String FullName
        {
            get
            {
                String Result;
                Result = String.Format("{0} - {1} - {2}", this.MnemoBornier, this.MnemoHardware, this.PositionCarte);

                return Result;
            }
        } // endProperty: FullName

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Constructeur pour une entrée TOR
        /// </summary>
        public ESTOR(XElement Configuration, XElement PilotageES, XElement Affectation) 
            : base(Configuration, PilotageES, null)
        {
            this.Type = TypeES.ETor;
            this.TypeName = "XML_CONFIG/ETOR";

            // Initialiser les 4 sections du xml
            this._section1 = new XMLCore.XMLProcessing();
            this._section2 = new XMLCore.XMLProcessing();
            this._section3 = new XMLCore.XMLProcessing();
            this._section4 = new XMLCore.XMLProcessing();
            // Ouvrir les XElements correspondant
            if (Configuration != null)
            {
                this._section1.OpenXML(Configuration);
            }
            if (PilotageES != null)
            {
                this._section2.OpenXML(PilotageES);
            }
            if (Affectation != null)
            {
                this._section3.OpenXML(Affectation);
            }

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// Constructeur pour une sortie TOR
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="PilotageES"></param>
        /// <param name="Reglage"></param>
        /// <param name="ReglageSecurite"></param>
        public ESTOR(XElement Configuration, XElement PilotageES, XElement Reglage, XElement ReglageSecurite) 
            : base(Configuration, PilotageES, ReglageSecurite)
        {
            this.Type = TypeES.STor;
            this.TypeName = "XML_CONFIG/STOR";
            // Initialiser les 4 sections du xml
            this._section1 = new XMLCore.XMLProcessing();
            this._section2 = new XMLCore.XMLProcessing();
            this._section3 = new XMLCore.XMLProcessing();
            this._section4 = new XMLCore.XMLProcessing();
            // Ouvrir les XElements correspondant
            this._section1.OpenXML(Configuration);
            this._section2.OpenXML(PilotageES);
            this._section3.OpenXML(Reglage);
            this._section4.OpenXML(ReglageSecurite);
            // Cas du Timo, lié les sorties PWM correspondantes
            if (PegaseData.Instance.ModuleT.TypeMT == MT.TIMO)
            {
                this.InitPWM();
            }

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("FolderName");
                RaisePropertyChanged("LabelValInitiale");
                RaisePropertyChanged("LabelValSecurite");
                RaisePropertyChanged("LabelCyclicRatio");
                RaisePropertyChanged("LabelFrequence");
            }

            // Faut-il mettre à jour la visibilité du paramètre en cours d'édition

        } // endMethod: ReceiveMessage

        /// <summary>
        /// Fermer proprement les données
        /// </summary>
        public new void Dispose ( )
        {
            this._section1.Close();
            this._section2.Close();
            this._section3.Close();
            this._section4.Close();
        } // endMethod: Dispose
        
        /// <summary>
        /// Fixer la fréquence sans mise à jour en boucle de l'ensemble des fréquences
        /// </summary>
        public void SetFrequence ( Int32 F )
        {
            this._pwmFrequence = F;
            RaisePropertyChanged("PWMFrequence");
        } // endMethod: SetFrequence

        /// <summary>
        /// Initialiser les données PWM en cas de besoin
        /// </summary>
        public void InitPWM ( )
        {
            Int32 STorNum;
            if (this.MnemoHardware.Length == 2)
            {
                if (this.MnemoHardware.Substring(0,1).ToLower() == "o")
                {
                    STorNum = Convert.ToInt32(this.MnemoHardware.Substring(1));
                    if (STorNum > 0 && STorNum < 7)
                    {
                        this._pwmFrequencePath = String.Format("XmlTechnique/ParametresApplicatifs/ParametresFixesMT/TableCaracES/TableFrequencePWM/Sortie{0}/Frequence{0}", STorNum);
                        this._pwmCycliquePath = String.Format("XmlTechnique/ParametresApplicatifs/ParametresFixesMT/TableCaracES/TableFrequencePWM/Sortie{0}/RapportCyclique{0}", STorNum);

                        // Fréquence
                        try
                        {
                            this._pwmFrequence = Convert.ToInt32(PegaseData.Instance.XMLFile.GetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE));
                            if (this._pwmFrequence > 0)
                            {
                                this._isPWM = true;
                            }
                            else
                            {
                                this._isPWM = false;
                            }
                        }
                        catch
                        {
                            this._pwmFrequence = 0;
                            this._isPWM = false;
                        }

                        // Rapport cyclique
                        try
                        {
                            this._pwmCyclique = Convert.ToInt32(PegaseData.Instance.XMLFile.GetValue(this._pwmCycliquePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE));
                            this._pwmCyclique /= 10;
                            if (this._pwmCyclique < 1)
                            {
                                this._pwmCyclique = 1;
                            }
                        }
                        catch
                        {
                            this._pwmCyclique = 1;
                        }
                    }
                }
            }
        } // endMethod: InitPWM

        /// <summary>
        /// Sauver le PWM
        /// </summary>
        public void SavePWM ( )
        {
            if (this.IsPWM)
            {
                PegaseData.Instance.XMLFile.SetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, this._pwmFrequence.ToString()); 
            }
            else
            {
                PegaseData.Instance.XMLFile.SetValue(this._pwmFrequencePath, "", "", XMLCore.XML_ATTRIBUTE.VALUE, "0");
            }
        } // endMethod: SavePWM

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ESAna
}
