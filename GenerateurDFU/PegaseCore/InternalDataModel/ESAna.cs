using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.PegaseCore
{
    public enum TypeUI
    {
        TUI_0a10V = 0,
        TUI_m10ap10V=1,
        TUI_vref=2,
        TUI_0a20mA=3,
        TUI_4a20mA=4,
        TUI_undefined = 5
    }

    public enum TypeIO
    {
        Input = 0,
        Output = 1
    }

    /// <summary>
    /// Les entrées sorties analogiques
    /// </summary>
    public class ESAna : ES
    {
        // Variables
        #region Variables

        private XMLCore.XMLProcessing _section1;        // Section Eana ou Sana
        //private XMLCore.XMLProcessing _section2;      // Section PilotageES
        private XMLCore.XMLProcessing _section3;        // Section Affectation ou Reglage
        private XMLCore.XMLProcessing _section4;        // Section ReglageSecurite
        private ParamsGainOffset _currentEAnaModes;     // Le mode EAna en cours de validité pour l'ES
        private Single _lienAnaValMin;
        private Single _lienAnaValMax;
        private String _lienAnaUnite;
        private String _lienAnaType;
        private String _lienAnaName;
        private TypeUI _uiType = TypeUI.TUI_undefined;

        // en sécurité
        private String _valeurInitialePercent;
        private String _valeurEnSecuritePercent;

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
        /// La liste des mode EAna disponible
        /// </summary>
        public ObservableCollection<ParamsGainOffset> EAnaModes
        {
            get
            {
                ObservableCollection<ParamsGainOffset> Result = new ObservableCollection<ParamsGainOffset>();
                String TypeUIES = this.IsNewTypeUI;
                if (TypeUIES != "")
                {
                    // Gérer les types UI des nouvelles fiches
                    foreach (var param in PegaseCore.RefGainOffset.Get().GetParamsByMaterial(TypeUIES))
                    {
                        Result.Add(param);
                    }
                }
                else
                {
                    // Gérer les types UI des anciennes fiches
                    String MT = PegaseCore.PegaseData.Instance.ModuleT.TypeMT.ToUpper();
                    foreach (var param in PegaseCore.RefGainOffset.Get().GetParamsByMaterial(MT))
                    {
                        Result.Add(param);
                    } 
                }

                return Result;
            }
        } // endProperty: EAnaModes

        /// <summary>
        /// Valeur initiale (mise en sécurité)
        /// </summary>
        public String ValeurInitialePercent
        {
            get
            {
                return this._valeurInitialePercent;
            }
            set
            {
                this._valeurInitialePercent = this.ValidatePercentInitial(value, this._valeurEnSecuritePercent);
                if (this._valeurInitialePercent != "")
                {
                    double vs = (Convert.ToDouble(this._valeurInitialePercent) * 1023) / 100;
                    base.ValeurInitiale = ((Int32)vs).ToString(); 
                }
                else
                {
                    base.ValeurInitiale = "";
                }
            }
        } // endProperty: ValeurInitiale

        /// <summary>
        /// Valeur en sécurité
        /// </summary>
        public String ValeurEnSecuritePercent
        {
            get
            {
                return this._valeurEnSecuritePercent;
            }
            set
            {
                this._valeurEnSecuritePercent = this.ValidatePercent(value, this._valeurEnSecuritePercent);
                if (this._valeurEnSecuritePercent != "")
                {
                    double vs = (Convert.ToDouble(this._valeurEnSecuritePercent) * 1023) / 100; // *(double)Gain;
                    base.ValeurEnSecurite = ((Int32)vs).ToString(); 
                }
                else
                {
                    base.ValeurEnSecurite = "";
                }
            }
        } // endProperty: ValeurEnSecurite

        /// <summary>
        /// Le type IO
        /// </summary>
        public TypeIO IOType
        {
            get;
            set;
        } // endProperty: IOType

        /// <summary>
        /// Le mode EAna en cours de sélection
        /// </summary>
        public ParamsGainOffset CurrentEAnaModes
        {
            get
            {
                return this._currentEAnaModes;
            }
            set
            {
                this._currentEAnaModes = value;
                if (value != null)
                {
                    this.LienAnaName = value.Name;
                    this.LienAnaType = value.Type;
                    this.LienAnaUnite = value.Unit;
                    this.LienAnaValMax = value.Max;
                    this.LienAnaValMin = value.Min;

                    if (this.IOType == TypeIO.Input)
                    {
                        this.Gain = value.GainE;
                        this.Offset = value.OffsetE;
                    }
                    else
                    {
                        this.Gain = value.GainS;
                        this.Offset = value.OffsetS;
                    }
                    this.UIType = (TypeUI)value.TypeUI;
                    // Pour tous les retours d'info associés, remettre les bornes de la calibration callées sur le nouveau type sélectionné
                    this.TypeUIChanged();
                }
            }
        } // endProperty: CurrentEAnaModes

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// la valeur minimum renvoyée (en courant ou en tension)
        /// </summary>
        public Single LienAnaValMin
        {
            get
            {
                return this._lienAnaValMin;
            }
            set
            {
                this._lienAnaValMin = value;
            }
        } // endProperty: LienAnaValMin

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// la valeur maximum renvoyée (en courant ou en tension)
        /// </summary>
        public Single LienAnaValMax
        {
            get
            {
                return this._lienAnaValMax;
            }
            set
            {
                this._lienAnaValMax = value;
            }
        } // endProperty: LienAnaValMax

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// l'unité de la valeur (en courant ou en tension)
        /// </summary>
        public String LienAnaUnite
        {
            get
            {
                return this._lienAnaUnite;
            }
            set
            {
                this._lienAnaUnite = value;
            }
        } // endProperty: LienAnaUnite

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// le type de l'entrée courant (intensité) ou en tension
        /// </summary>
        public String LienAnaType
        {
            get
            {
                return this._lienAnaType;
            }
            set
            {
                this._lienAnaType = value;
            }
        } // endProperty: LienAnaType

        /// <summary>
        /// Le nom décrit dans le lien Ana
        /// </summary>
        public String LienAnaName
        {
            get
            {
                return this._lienAnaName;
            }
            set
            {
                this._lienAnaName = value;
            }
        } // endProperty: LienAnaName

        /// <summary>
        /// Id de l'entrée ou de la sortie Ana
        /// </summary>
        public Int32 IdESAna
        {
            get
            {
                return base.ID;
            }
        } // endProperty: IdESAna

        /// <summary>
        /// La position de la carte dans le MT
        /// </summary>
        public Int32 PositionCarte
        {
            get
            {
                return base.IDCarte;
            }
            set
            {
                base.IDCarte = value;
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
                return (PegaseCore.FamilleDeDonnees_e)base.IndiceFamille;
            }
            set
            {
                base.IndiceFamille = (Int32)value;
            }
        } // endProperty: Famille

        /// <summary>
        /// L'indice de la donnée
        /// </summary>
        public Int32 Indice
        {
            get
            {
                return base.IndiceES;
            }
            set
            {
                base.IndiceES = value;
            }
        } // endProperty: Indice

        /// <summary>
        /// Le gain de l'entrée ou de la sortie
        /// </summary>
        public Single Gain
        {
            get
            {
                Single Value = 1;
                String SV;

                if (this._section3.GetNodesByCode("Gain").FirstOrDefault() != null)
                {
                    SV = this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    SV = JAY.XMLCore.Tools.FixFloatStringSeparator(SV);
                    Value = Convert.ToSingle(SV); 
                }
                return Value;
            }
            set
            {
                CultureInfo culture = CultureInfo.InvariantCulture;
                String SV = value.ToString(culture);
                if (this._section3.GetNodesByCode("Gain").FirstOrDefault() != null)
                {
                    string test = this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    this._section3.GetNodesByCode("Gain").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;
                }
            }
        } // endProperty: Gain

        /// <summary>
        /// L'offset de l'entrée ou de la sortie
        /// </summary>
        public Single Offset
        {
            get
            {
                Single Value = 0;
                String SV;

                if (this._section3.GetNodesByCode("Offset").FirstOrDefault() != null)
                {
                    SV = this._section3.GetNodesByCode("Offset").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    SV = JAY.XMLCore.Tools.FixFloatStringSeparator(SV);
                    Value = Convert.ToSingle(SV); 
                }
                return Value;
            }
            set
            {
                CultureInfo culture = CultureInfo.InvariantCulture;
                String SV = value.ToString(culture);
                if (this._section3.GetNodesByCode("Offset").FirstOrDefault() != null)
                {
                    this._section3.GetNodesByCode("Offset").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;
                }
            }
        } // endProperty: Offset

        /// <summary>
        /// Le type de fonctionnement
        /// </summary>
        public TypeUI UIType
        {
            get
            {
                if (this._uiType == TypeUI.TUI_undefined)
                {
                    String SV = "";
                    Int32 Value = 0;

                    if (this._section3 != null && this._section3.GetNodesByCode("TypeUI") != null)
                    {
                        if (this._section3.GetNodesByCode("TypeUI").Count > 0)
                        {
                            SV = this._section3.GetNodesByCode("TypeUI").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                        }
                        try
                        {
                            Value = Convert.ToInt32(SV);
                        }
                        catch
                        {
                            Value = 0;
                        }
                    }

                    this._uiType = (TypeUI)Value;
                }
                return this._uiType;
            }
            set
            {
                if (this._section3 != null && this._section3.GetNodesByCode("TypeUI") != null)
                {
                    this._uiType = value;
                    if (this._section3.GetNodesByCode("TypeUI").Count > 0)
                    {
                        string test = ((Int32)value).ToString();
                        this._section3.GetNodesByCode("TypeUI").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = ((Int32)value).ToString();
                    }
                }
                // Initialiser les gains et offsets en fonction du type UI
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
                        Value = 0;
                        break;
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
                        Value = 100;
                        break;
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

        /// <summary>
        /// Le type UI est-il géré suivant l'ancienne méthode, ou bien la nouvelle ?
        /// Si ancienne méthode, renvoyer "", sinon la lettre désignant le bloc de type disponible (A, B, C...)
        /// </summary>
        public String IsNewTypeUI
        {
            get
            {
                String Result = "";
                ObservableCollection<XElement> LCaracteristique = this._section1.GetNodeByPath("Caracteristique");
                // Vérifier si le type UI de nouvelle génération est renseigné ?
                if (LCaracteristique != null)
                {
                    Result = LCaracteristique.First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value.ToUpper();
                }

                return Result;
            }
        } // endProperty: IsNewTypeUI

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Constructeur pour une entrée analogique
        /// </summary>
        public ESAna(XElement Configuration, XElement PilotageES, XElement Affectation)
            : base(Configuration, PilotageES, null)
        {
            this.Type = TypeES.EAna;
            this.TypeName = "XML_CONFIG/EANA";
            // Initialiser les 4 sections du xml
            this._section1 = new XMLCore.XMLProcessing();
            //this._section2 = new XMLCore.XMLProcessing();
            this._section3 = new XMLCore.XMLProcessing();
            this._section4 = new XMLCore.XMLProcessing();
            // Ouvrir les XElements correspondant
            this._section1.OpenXML(Configuration);
            //this._section2.OpenXML(PilotageES);
            this._section3.OpenXML(Affectation);

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// Constructeur pour une sortie analogique
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="PilotageES"></param>
        /// <param name="Reglage"></param>
        /// <param name="ReglageSecurite"></param>
        public ESAna(XElement Configuration, XElement PilotageES, XElement Reglage, XElement ReglageSecurite) 
            : base(Configuration, PilotageES, ReglageSecurite)
        {
            this.Type = TypeES.SAna;
            this.TypeName = "XML_CONFIG/SANA";
            // Initialiser les 4 sections du xml
            this._section1 = new XMLCore.XMLProcessing();
            //this._section2 = new XMLCore.XMLProcessing();
            this._section3 = new XMLCore.XMLProcessing();
            this._section4 = new XMLCore.XMLProcessing();
            // Ouvrir les XElements correspondant
            this._section1.OpenXML(Configuration);
            //this._section2.OpenXML(PilotageES);
            this._section3.OpenXML(Reglage);
            this._section4.OpenXML(ReglageSecurite);
            // initialiser les valeur en sécurité en %
            try
            {
                this._valeurEnSecuritePercent = ((Int32)((Convert.ToDouble(base.ValeurEnSecurite) / 1023) * 100)).ToString();
            }
            catch
            {
                this._valeurEnSecuritePercent = "0";
                base.ValeurEnSecurite = "0";
            }
            try
            {
                this._valeurInitialePercent = ((Int32)((Convert.ToDouble(base.ValeurEnSecurite) / 1023) * 100)).ToString();
            }
            catch
            {
                this._valeurInitialePercent = "0";
                base.ValeurEnSecurite = "0";
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
                RaisePropertyChanged("LabelValInitiale");
                RaisePropertyChanged("LabelValSecurite");
                RaisePropertyChanged("LabelCyclicRatio");
                RaisePropertyChanged("LabelFrequence");
            }

            // Faut-il mettre à jour la visibilité du paramètre en cours d'édition

        } // endMethod: ReceiveMessage

        /// <summary>
        /// Le mode en cours des entrées / sorties analogiques
        /// </summary>
        public void InitCurrentEAnaModes ( )
        {
            var Query = from eana in this.EAnaModes
                        where eana.TypeUI == (Int32)this.UIType
                        select eana;

            this.CurrentEAnaModes = Query.FirstOrDefault();
            if (this.CurrentEAnaModes == null)
            {
                if (this.EAnaModes.Count > 0)
                {
                    this.CurrentEAnaModes = this.EAnaModes[0]; 
                }
            }
        } // endMethod: CurrentEAnaModes
        
        /// <summary>
        /// Valider la valeur NewValue en tant que pourcentage
        /// </summary>
        public String ValidatePercent ( String NewValue, String OldValue )
        {
            String Result;

            if (NewValue.Trim() != "")
            {
                try
                {
                    Int32 value = Convert.ToInt32(NewValue);
                    if (value < 0)
                    {
                        value = 0;
                    }
                    else if (value > 100)
                    {
                        value = 100;
                    }
                    Result = value.ToString();
                }
                catch
                {
                    Result = OldValue;
                } 
            }
            else
            {
                Result = "";
            }
            
            return Result;
        } // endMethod: ValidatePercent

        /// <summary>
        /// Valider la valeur NewValue en tant que pourcentage
        /// </summary>
        public String ValidatePercentInitial(String NewValue, String OldValue)
        {
            String Result;

            try
            {
                Int32 value = Convert.ToInt32(NewValue);
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                Result = value.ToString();
            }
            catch
            {
                Result = OldValue;
            }

            return Result;
        } // endMethod: ValidatePercent

        /// <summary>
        /// Le type UI a changé
        /// </summary>
        private void TypeUIChanged ( )
        {
            if (PegaseData.Instance.OLogiciels != null)
            {
                var Query = from ri in PegaseData.Instance.OLogiciels.Informations
                            where ri.EAna != null && ri.EAna.IdESAna == this.IdESAna
                            select ri;

                if (Query.Count() > 0)
                {
                    foreach (var ri in Query)
                    {
                        ri.TPtA_X = this.CurrentEAnaModes.Min;
                        ri.TPtB_X = this.CurrentEAnaModes.Max;
                    }
                }
            }
        } // endMethod: TypeUIChanged

        /// <summary>
        /// Fermer proprement les données
        /// </summary>
        public void Dispose ( )
        {
            this._section1.Close();
            //this._section2.Close();
            this._section3.Close();
            this._section4.Close();
        } // endMethod: Dispose

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ESAna
}
