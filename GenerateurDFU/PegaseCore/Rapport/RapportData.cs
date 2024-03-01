using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.PegaseCore;
using JAY.PegaseCore.InternalDataModel;
using JAY.PegaseCore.Helper;
using JAY.XMLCore;
using System.Globalization;
using Drwg = System.Drawing;

namespace JAY.PegaseCore.Rapport
{
    public class CablageRow
    {
        /// <summary>
        /// Le nom de l'entrée sortie
        /// </summary>
        public String ES
        {
            get;
            set;
        } // endProperty: ES
        
        /// <summary>
        /// Le nom du bornier
        /// </summary>
        public String Bornier
        {
            get;
            set;
        } // endProperty: Bornier
        
        /// <summary>
        /// Le nom client de l'organe
        /// </summary>
        public String Organe
        {
            get;
            set;
        } // endProperty: Organe

        // Constructeur
        public CablageRow (String es, String bornier, String organe)
        {
            this.ES = es;
            this.Bornier = bornier;
            this.Organe = organe;
        }

        // Méthodes
        
        /// <summary>
        /// Construire une ligne grâce au template
        /// </summary>
        public XElement BuildRow ( XElement rowTemplate )
        {
            XElement Result;
            String outerText = rowTemplate.ToString();

            outerText = outerText.Replace("$BALISE0101", this.ES);
            outerText = outerText.Replace("$BALISE0102", this.Bornier);
            outerText = outerText.Replace("$BALISE0103", this.Organe);

            Result = XElement.Parse(outerText);

            return Result;
        } // endMethod: BuildRow
    }

    /// <summary>
    /// Données utilisées pour le rapport. Les données sont reconstruites chaque fois
    /// afin d'assurer 
    /// </summary>
    public class RapportData
    {
        // Constantes
        #region Constantes

        private const Int32 NB_MODE_MAX = 32;
        private const String VIDE = "-";
        private const String SCREEN_VIDE = " ";
        private const String SCREEN_INDETERMINE = "????";
        private const String FALSE = VIDE;
        private const String TRUE = "X";

        // Constantes MO
        public const String NUMFICHE = "NUM_FICHE";

        public const String MO_REF = "MO_REF";
        public const String MO_NUM_SERIE = "MO_NUM_SERIE";
        public const String MO_FREQ = "MO_FREQ";
        public const String MO_CODE_ID = "MO_CODE_ID";
        public const String MO_AUX = "MO_AUX";
        public const String MO_IR_STUP = "MO_IR_STUP";
        public const String MO_VIBREUR = "MO_VIBREUR";
        public const String MO_ACCEL = "MO_ACCEL";
        public const String MO_BUZZER = "MO_BUZZ";
        public const String MO_SBC_BEHAVIOR = "MO_SBC_BEHAVIOR";
        public const String MO_CODE_CONFIGURATEUR = "MO_CODE_CONFIG";

        // Constantes SIM
        public const String SIM_OPTION_IR = "OPTION_IR";
        public const String SIM_REF = "SIM_REF";
        public const String SIM_NUM_SERIE = "SIM_NUM_SERIE";
        public const String SIM_CANAL = "SIM_CANAL";
        public const String SIM_ARRET_PASSIF = "SIM_PASS_STOP";
        public const String SIM_STANDBY = "SIM_STANDBY";
        public const String SIM_MODES_OPT = "SIM_MODES_OPT";
        public const String SIM_SYNCHRO_OPT = "SIM_SYNCHRO_OPT";
        public const String SIM_DEADMAN_OPT = "SIM_DEADMAN_OPT";
        public const String SIM_CODE_CONFIGURATEUR = "SIM_CODE_CONFIG";
        public const String CHARGE_CODE_CONFIGURATEUR = "CHARGE_CODE_CONFIGURATEUR";

        // Constantes MT
        public const String MT_REFERENCE = "MT_REFERENCE";
        public const String MT_NUMERO_SERIE = "MT_NUMERO_SERIE";
        public const String MT_FREQ = "MT_FREQ";
        public const String MT_CODE_ID = "MT_CODE_ID";
        public const String MT_TYPE_CABLE = "MT_TYPE_CABLE";
        public const String MT_KLAXON = "MT_KLAXON";
        public const String MT_CODE_CONFIGURATEUR = "MT_CODE_CONFIG";

        public const String COMMENTAIRE_PRINCIPALE = "COM_PRINCIPALE";

        public const String TABLE_BOUTON1 = "TABLE_BOUTON1";
        public const String TABLE_BOUTON2 = "TABLE_BOUTON2";
        public const String TABLE_CABLAGE = "TABLE_CABLAGE";

        #endregion

        // Variables
        #region Variables

        private Dictionary<String, XNode> _fields;
        private String _langageName;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom du language en cours d'utilisation pour le rapport
        /// </summary>
        public String LanguageName
        {
            get
            {
                return this._langageName;
            }
            private set
            {
                this._langageName = value;
            }
        } // endProperty: LanguageName

        /// <summary>
        /// Le dictionnaire 
        /// </summary>
        public Dictionary<String, XNode> Fields
        {
            get
            {
                if (this._fields == null)
                {
                    this._fields = new Dictionary<String, XNode>();
                }
                return this._fields;
            }
            set
            {
                this._fields = value;
            }
        } // endProperty: Fields

        #endregion

        // Constructeur
        #region Constructeur

        public RapportData()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        public XElement InitNewLangue(String langue)
        {
            XElement Result = new XElement("LangueRapport");
            this.LanguageName = langue;
            Result.Value = langue;
            return Result;
        }
        public XElement InitNewParamSimple(XElement parent, string langue)
        {
            XElement paramgen = new XElement("PARAM_GENERAUX");

            XElement Result = new XElement("params");


            Result = new XElement("params");
            XAttribute item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            item = new XAttribute("type", "Type5");
            Result.Add(item);
            paramgen.Add(Result);

            Result = new XElement("params");
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/INTER_UTILISATEUR"));
            Result.Add(item);
            item = new XAttribute("type", "Type0");
            Result.Add(item);
            paramgen.Add(Result);

            string tmp = GestionVariableSimple.Instance.GetVariableByName("BitmapLogo").RefElementValue;
            tmp = tmp.Replace("\\", "/");
            Result = new XElement("params", tmp);
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ECRAN_DEM"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);


            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_NumLangue", langue));
            XText toto = new XText(LCM.GetLabel(GestionVariableSimple.Instance.GetVariableByName("NumLangue").RefElementValue));
            Result = new XElement("params", toto.Value.ToString());
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/LANGUE_MO"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            string value = GestionVariableSimple.Instance.GetVariableByName("CanalDemande").RefElementValue;
            if (value.Equals("0")|| (value.Equals("-1")))
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUCUN"));
            }
            else
            {
                Result = new XElement("params", GestionVariableSimple.Instance.GetVariableByName("CanalDemande").RefElementValue);
            }
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/CANAL"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            Result = new XElement("params");
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TEMPORISATIONS"));
            Result.Add(item);
            item = new XAttribute("type", "Type0");
            Result.Add(item);
            paramgen.Add(Result);


            string delai = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/delai", "", "", XML_ATTRIBUTE.VALUE);
            float time = Single.Parse(delai) / 10;
            delai = time.ToString("");
            delai = delai + " s";
            Result = new XElement("params", delai);
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XMLTranslation/Descrip_DelaiAP"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);


            string active = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XML_ATTRIBUTE.VALUE);
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ActivationMiseEnVeille", "Francais"));
            toto = new XText(LCM.GetLabel(active));

            string delaiMEV = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/delai", "", "", XML_ATTRIBUTE.VALUE);
            if (active.Equals("1"))
            {
                delaiMEV = delaiMEV + " s";
            }
            else
            {
                delaiMEV = toto.Value.ToString();
            }
            Result = new XElement("params", delaiMEV);
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XMLTranslation/Descrip_DelaiMEV"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);


            active = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE);
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ActivationAbsRadioMT", "Francais"));
            toto = new XText(LCM.GetLabel(active));
            string delaiMEVMT = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/DelaiAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE);
            if (active.Equals("1"))
            {
                delaiMEVMT = delaiMEVMT + " s";
            }
            else
            {
                delaiMEVMT = toto.Value.ToString();
            }
            Result = new XElement("params", delaiMEVMT);
            item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTranslation/DELAI_ABSRADIO_MT"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            Result = new XElement("params");
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/DROITSACCES"));
            Result.Add(item);
            item = new XAttribute("type", "Type0");
            Result.Add(item);
            paramgen.Add(Result);

            Int32 PassWordConfig = PegaseData.Instance.PassWordConfig;
            Int32 PassWordAssociation = PegaseData.Instance.PassWordAssociation;
            Int32 PassWordStartPP = PegaseData.Instance.PassWordStartPP;
            Int32 PassWordStart = PegaseData.Instance.PassWordStart;
            if (PassWordConfig.Equals(0))
            {
                PassWordAssociation = 0;
                PassWordStartPP = 0;
                PassWordStart = 0;
            }
            else if (PassWordAssociation.Equals(0))
            {
                PassWordStartPP = 0;
                PassWordStart = 0;
            }
            if (PassWordStart.Equals(0))
            {
                if (PassWordStartPP.Equals(0))
                {
                    PassWordStart = 0;
                    PassWordStartPP = 0;
                }
                else
                {
                    if (PegaseData.Instance.OLogiciels.ThereIsMaskInMode() || (PegaseData.Instance.MaskUserRun != 0xFFFFFFFF))
                    {
                        
                        PassWordStart = 0;
                    }
                    else
                    {
                        PassWordStart = PassWordStartPP;
                        PassWordStartPP = 0;
                    }
                    
                }
            }


            if (PassWordStart.Equals(0))
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUCUN"));
            }
            else
            {
                Result = new XElement("params", PassWordStart.ToString());
            }
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/DROITSACCESDEM"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            if (PassWordAssociation.Equals(0))
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUCUN"));
            }
            else
            {
                Result = new XElement("params", PassWordAssociation.ToString());
            }
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/DROITSACCESASSO"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            if (PassWordConfig.Equals(0))
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUCUN"));
            }
            else
            {
                Result = new XElement("params", PassWordConfig.ToString());
            }
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/DROITSACCESCONFIG"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            if (PassWordStartPP.Equals(0))
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUCUN"));
            }
            else if (PassWordStartPP < 0)
            {
                Result = new XElement("params", LanguageSupport.Get().GetTextRapport("----"));
            }
            else
            {
                Result = new XElement("params", PassWordStartPP.ToString());
            }
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/DROITSACCESDEMPP"));
            Result.Add(item);
            item = new XAttribute("type", "Type2");
            Result.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            Result.Add(item);
            paramgen.Add(Result);

            // ajout des parametres d'information 
            Result = new XElement("params");
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/RADIO"));
            Result.Add(item);
            item = new XAttribute("type", "Type0");
            Result.Add(item);
            paramgen.Add(Result);
            //canal fixe

            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_NumLangue", langue));
            Result = new XElement("params", LCM.GetLabel(GestionVariableSimple.Instance.GetVariableByName("NumLangue").RefElementValue));


            VariableEditable VE = GestionVariableSimple.Instance.GetVariableByName("CanalAuto");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_CanalAutoMt", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_CanalAutoMt", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }

            VE = GestionVariableSimple.Instance.GetVariableByName("PuissanceMTdemande");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_PuissanceMTdemande", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_PuissanceMTdemande", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("PuissanceMOdemande");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_PuissanceMOdemande", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_PuissanceMOdemande", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("DebitDemande");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_DebitDemande", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_DebitDemande", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            Result = new XElement("params");
            item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/AUTRES"));
            Result.Add(item);
            item = new XAttribute("type", "Type0");
            Result.Add(item);
            paramgen.Add(Result);
            VE = GestionVariableSimple.Instance.GetVariableByName("Mode");
            string  infra = PegaseData.Instance.GestionInfraRouge.ChoixInfraRouge;
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Mode", langue));
                Result = new XElement("params", LCM.GetLabel(infra));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_Mode", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("ModeAssociation");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ModeAssociation", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_ModeAssociation", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("CharIdent");
            if (VE != null)
            {
                Result = new XElement("params", VE.RefElementValue);
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_CharIdent", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("NumeroGroupeMT");
            if (VE != null)
            {
                if (VE.RefElementValue.Equals("0"))
                {
                    Result = new XElement("params", LanguageSupport.Get().GetTextRapport("XMLTranslation/UNDEFINED"));
                }
                else
                {
                    LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_NumeroGroupeMT", langue));
                    Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                }
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_NumeroGroupeMT", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            //PegaseData.Instance.GestionTemporisation.ActivationHommeMort;
            VE = GestionVariableSimple.Instance.GetVariableByNameCompl("Actif", "Descrip_ActifHomme");
            if (VE != null)
            {
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ActifHomme", langue));
                Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_ActifHomme", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("SeuilNseo");
            if (VE != null)
            {
                Result = new XElement("params", VE.RefElementValue);
                item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTRANSLATION/Descrip_SeuilNseo", langue));
                Result.Add(item);
                item = new XAttribute("type", "Type4");
                Result.Add(item);
                paramgen.Add(Result);
            }
            parent.Add(paramgen);
            return parent;
        }
        public XElement InitNewData(XElement parent, String langue)
        {

            XElement Result = new XElement("Description");
            XElement Text;
            int nbligne = 0;
            Text = this.Transform2Pargraphe(PegaseData.Instance.Commentaire.DescriptifGeneral, out nbligne);
            string template = DefaultValues.Get().RapportFolder + "template_description2.html";
            int nbpage = 0;
            if (nbligne > 45)
            {
                nbpage = ((nbligne - 40) / 45) + 1;
            }
            else
            {
                nbpage = 0;
            }
           
            XAttribute Xnbpage = new XAttribute("NBPAGES", nbpage.ToString());
            Result.Add(Xnbpage);
            if (File.Exists(template))
            {
                for (int i = 0; i < nbpage; i++)
                {
                    File.Copy(template, DefaultValues.Get().RapportFolder + "description2" + (i + 1).ToString() + ".html");
                }
            }

            Result.Add(Text);// = PegaseData.Instance.Commentaire.DescriptifGeneral;
            parent.Add(Result);

            Result = new XElement("Logo");
            string Value = GestionVariableSimple.Instance.GetVariableByName("BitmapLogo").RefElementValue;
            if (Value.Equals(""))
            {
                Result.Value = "";// "Images/JAYELECTRONIQUE.bmp";
            }
            else
            {
                Result.Value = this.BuildNewBaliseImgUserFile(GestionVariableSimple.Instance.GetVariableByName("BitmapLogo").RefElementValue, 0, 0);
            }
            parent.Add(Result);

            string imageMO = "";
            imageMO = PegaseData.Instance.RefErpMO.Substring(0, 2);
            if (PegaseData.Instance.RefErpMO.Contains("_X"))
            {
                imageMO += "A";
            }
            imageMO = "img/" + imageMO + ".png";
            Result = new XElement("IMAGEPRODUIT", imageMO);
            parent.Add(Result);

            string imageMT = "";
            imageMT = PegaseData.Instance.RefErpMT.Substring(0, 2);
            if (PegaseData.Instance.RefErpMT.Contains("_X"))
            {
                imageMT += "A";
            }
            imageMT = "img/" + imageMT + ".png";
            Result = new XElement("IMAGETRANCEIVER", imageMT);
            parent.Add(Result);

            Result = new XElement("NUMFICHE", PegaseData.Instance.IdentificationPack.NumFiche);
            parent.Add(Result);

            Result = new XElement("REF_PACK");
            Result.Value = PegaseData.Instance.RefPartNumber;
            parent.Add(Result);

            Result = new XElement("DATE_PROG");
            Result.Value = PegaseData.Instance.IdentificationPack.PackDateProg;
            parent.Add(Result);

            Result = new XElement("MT_REFERENCE");
            Result.Value = PegaseData.Instance.ModuleT.RefIndus;
            parent.Add(Result);

            Result = new XElement("MT_NUMERO_SERIE");
            Result.Value = PegaseData.Instance.ModuleT.SerialNumber;
            parent.Add(Result);

            Result = new XElement("MT_FREQ");
            Result.Value = PegaseData.Instance.ModuleT.Frequence;
            parent.Add(Result);

            Result = new XElement("MT_CODE_ID");
            Result.Value = PegaseData.Instance.ModuleT.UniqueID;
            parent.Add(Result);

            Result = new XElement("MT_TENSION");
            Result.Value = PegaseData.Instance.ModuleT.Tension;
            parent.Add(Result);

            Result = new XElement("MT_TYPE_CABLE");
            Result.Value = PegaseData.Instance.ModuleT.TypeCable;
            parent.Add(Result);

            // Result = new XElement("MT_CODE_CONFIGURATEUR");
            // Result.Value = PegaseData.Instance.RefErpMT;
            // parent.Add(Result);

            Result = new XElement("REF_COMPANY");
            Result.Value = PegaseData.Instance.RefCompany;
            parent.Add(Result);

            Result = new XElement("REF_CUSTOMER");
            Result.Value = PegaseData.Instance.RefCustomer;
            parent.Add(Result);

            Result = new XElement("REF_CUSTOMERCODE");
            Result.Value = PegaseData.Instance.RefCustomerCode;
            parent.Add(Result);

            CultureInfo ci = new CultureInfo(LanguageSupport.Get().LanguageTypeDe(this.LanguageName));
            Result = new XElement("REF_DATE");
            Result.Value = PegaseData.Instance.RefDate.ToString("d", ci);
            parent.Add(Result);

            Result = new XElement("REFINDICE");
            Result.Value = PegaseData.Instance.RefIndice;
            parent.Add(Result);

            Result = new XElement("REFPARTNUMBER");
            Result.Value = PegaseData.Instance.RefPartNumber;
            parent.Add(Result);
            
            Result = new XElement("CONTACTJAY");
            Result.Value = PegaseData.Instance.RefJAY;
            parent.Add(Result);

            // SIM

            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Mode", this.LanguageName));
            Int32 IntValue;
            try
            {
                IntValue = Convert.ToInt32(PegaseData.Instance.OLogiciels.GestionIR);
                if (IntValue < 0)
                {
                    IntValue = 0;
                }
            }
            catch
            {
                IntValue = 0;
            }
            String TradValue;
            TradValue = LCM.GetLabel(IntValue.ToString());
            Result = new XElement("SIM_OPTION_IR", TradValue);
            parent.Add(Result);

            Result = new XElement("SIM_REF", PegaseData.Instance.CarteSIM.RefIndus);
            parent.Add(Result);
            Result = new XElement("SIM_NUM_SERIE", PegaseData.Instance.CarteSIM.NumeroDeSerie);
            parent.Add(Result);
            Result = new XElement("SIM_CANAL", PegaseData.Instance.CarteSIM.CanalDemande);
            parent.Add(Result);


            // Arrêt passif
            String SIMPassStop = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            Int32 Temps = PegaseData.ConvertString2Value(SIMPassStop);
            Result = new XElement("SIM_ARRET_PASSIF", String.Format("{0}", Temps));
            parent.Add(Result);

            // Temps d'attente
            String IsOn = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            Int32 IIsOn = PegaseData.ConvertString2Value(IsOn);

            String SIMStandby;
            Int32 ISimStandby = 0;
            if (IIsOn != 0)
            {
                SIMStandby = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                ISimStandby = PegaseData.ConvertString2Value(SIMStandby) / 60;
                SIMStandby = String.Format("{0}", ISimStandby);
            }
            else
            {
                SIMStandby = VIDE;
            }
            Result = new XElement("SIM_STANDBY", SIMStandby);
            parent.Add(Result);

            // Mot de passe démarrage
            if (PegaseData.Instance.PassWordStart != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordStart);
            }
            else
            {
                Value = VIDE;
            }
            Result = new XElement("PW_START", Value);
            parent.Add(Result);


            // Mot de passe démarrage++
            if (PegaseData.Instance.PassWordStartPP != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordStartPP);
            }
            else
            {
                Value = VIDE;
            }
            Result = new XElement("PW_STARTPP", Value);
            parent.Add(Result);

            // Mot de passe association
            if (PegaseData.Instance.PassWordAssociation != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordAssociation);
            }
            else
            {
                Value = VIDE;
            }
            Result = new XElement("PW_ASSOCIATION", Value);
            parent.Add(Result);

            // Mot de passe configuration
            if (PegaseData.Instance.PassWordConfig != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordConfig);
            }
            else
            {
                Value = VIDE;
            }
            Result = new XElement("PW_CONFIG", Value);
            parent.Add(Result);
            // Option Mode -> identifier la valeur
            var QueryOMode = from omode in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                             where omode.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionExploit.ToString() && omode.NameOption == "ModeExploitation"
                             select omode.Libelle;

            if (QueryOMode.Count() > 0)
            {
                Result = new XElement("SIM_MODES_OPT", QueryOMode.First());
                parent.Add(Result);
            }
            else
            {
                this.Fields.Add(SIM_MODES_OPT, new XText("No Data"));
                Result = new XElement("SIM_MODES_OPT", "No Data");
                parent.Add(Result);
            }

            // Option Synchronisation -> identifier la valeur
            var QueryOSynchro = from osynchro in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                                where osynchro.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionCouplage.ToString() && osynchro.NameOption == "Couplage"
                                select osynchro.Libelle;

            if (QueryOSynchro.Count() > 0)
            {
                this.Fields.Add(SIM_SYNCHRO_OPT, new XText(QueryOSynchro.First()));
                Result = new XElement("SIM_SYNCHRO_OPT", QueryOSynchro.First());
                parent.Add(Result);
            }
            else
            {
                Result = new XElement("SIM_SYNCHRO_OPT", "No Data");
                parent.Add(Result);
            }

            // Option Homme Mort -> Identifier la valeur
            var QueryOHommeMort = from ohommemort in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                                  where ohommemort.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionHommeMort.ToString() && ohommemort.NameOption == "Homme mort"
                                  select ohommemort.Libelle;

            if (QueryOHommeMort.Count() > 0)
            {
                Result = new XElement("SIM_DEADMAN_OPT", QueryOHommeMort.First());
                parent.Add(Result);
            }
            else
            {
                Result = new XElement("SIM_DEADMAN_OPT", "No Data");
                parent.Add(Result);
            }

            // Ref ERP SIM
            Result = new XElement("SIM_CODE_CONFIGURATEUR", PegaseData.Instance.RefErpSIM);
            parent.Add(Result);
            // Ref ERP CHARGE
            Result = new XElement("CHARGE_CODE_CONFIGURATEUR", PegaseData.Instance.RefErpCHARGE);
            parent.Add(Result);

            Result = new XElement("MO_CODE_CONFIGURATEUR", PegaseData.Instance.RefErpMO);
            parent.Add(Result);
            Result = new XElement("MT_CODE_CONFIGURATEUR", PegaseData.Instance.RefErpMT);
            parent.Add(Result);

            Result = new XElement("MO_REF", PegaseData.Instance.MOperateur.RefIndus);
            parent.Add(Result);


            // MO

            Result = new XElement("MO_NUM_SERIE", PegaseData.Instance.MOperateur.SerialNumber);
            parent.Add(Result);
            Result = new XElement("MO_FREQ", PegaseData.Instance.MOperateur.Frequence);
            parent.Add(Result);
            // Unique ID
            // Int32 ID;
            // ID = XMLCore.Tools.ConvertASCIIToInt16(PegaseData.Instance.MOperateur.UniqueID);
            Result = new XElement("MO_CODE_ID", PegaseData.Instance.MOperateur.UniqueID);
            parent.Add(Result);
            // Acquérir les traductions présence / absence
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/PRESENCE_ABSENCE", this.LanguageName));
            int Value2;

            // Présence auxiliaire
            Value2 = PegaseData.Instance.MOperateur.PresenceAuxiliaire;
            TradValue = LCM.GetLabel(Value2.ToString());
            Result = new XElement("MO_AUX", TradValue);
            parent.Add(Result);
            // Présence IR
            Value2 = PegaseData.Instance.MOperateur.PresenceIR;
            TradValue = LCM.GetLabel(Value2.ToString());
            Result = new XElement("MO_IR_STUP", TradValue);
            parent.Add(Result);
            // Présence vibreur
            Value2 = PegaseData.Instance.MOperateur.PresenceVibreur;
            TradValue = LCM.GetLabel(Value2.ToString());
            Result = new XElement("MO_VIBREUR", TradValue);
            parent.Add(Result);
            // Présence acceleromètre
            Value2 = PegaseData.Instance.MOperateur.PresenceAccelerometre;
            TradValue = LCM.GetLabel(Value2.ToString());
            Result = new XElement("MO_ACCEL", TradValue);
            parent.Add(Result);
            // Présence buzzer
            Value2 = PegaseData.Instance.MOperateur.PresenceBuzzer;
            TradValue = LCM.GetLabel(Value2.ToString());
            Result = new XElement("MO_BUZZER", TradValue);
            parent.Add(Result);
            // Comportement sur support de charge
            Value2 = PegaseData.Instance.CurrentStartOnSBCMode;
            // LanguageSupport.Get().GetToolTip("XMLTranslation/Plage_NumLangue", "Anglais")
            switch (Value2)
            {
                case 0:
                    TradValue = LanguageSupport.Get().GetTextRapport("LIBEL_MO/COMBO_PAS_00");
                    break;
                case 1:
                    TradValue = LanguageSupport.Get().GetTextRapport("LIBEL_MO/COMBO_DEM_01");
                    break;
                case 2:
                    TradValue = LanguageSupport.Get().GetTextRapport("LIBEL_MO/COMBO_DEM_SECU_02");
                    break;
                default:
                    TradValue = LanguageSupport.Get().GetTextRapport("LIBEL_MO/COMBO_PAS_00");
                    break;
            }

            Result = new XElement("MO_SBC_BEHAVIOR", TradValue);
            parent.Add(Result);

            Result = new XElement("IDWEB", PegaseData.Instance.RefIdWeb);
            parent.Add(Result);

            Result = new XElement("CODEAPPLI", PegaseData.Instance.RefCodeAppli);
            parent.Add(Result);

            string name = "";
            if (Path.GetFileNameWithoutExtension(PegaseData.Instance.FileName).Length > 8)
            {
                name = Path.GetFileNameWithoutExtension(PegaseData.Instance.FileName).Substring(0, 8);
            }
            else
            {
                name = Path.GetFileNameWithoutExtension(PegaseData.Instance.FileName);
            }
            Result = new XElement("NMR_FICHE", name);
            parent.Add(Result);

            Result = new XElement("NOMPROJET", PegaseData.Instance.RefProjet);
            parent.Add(Result);

            return parent;
        }

        internal XElement InitNewTradReport(XElement parent, string langue)
        {
            string text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TN_RSA");
            XElement balise = new XElement("TN_RSA", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TVERSION");
            balise = new XElement("TVERSION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDATE_DE_REVISION");
            balise = new XElement("TDATE_DE_REVISION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TREF_PACK");
            balise = new XElement("TREF_PACK", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TPARTENAIRE_JAY");
            balise = new XElement("TPARTENAIRE_JAY", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCODE_PARTENAIRE");
            balise = new XElement("TCODE_PARTENAIRE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCONTACT_PARTENAIRE");
            balise = new XElement("TCONTACT_PARTENAIRE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCONTACT_JAY");
            balise = new XElement("TCONTACT_JAY", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TAPPROBATION_JAY");
            balise = new XElement("TAPPROBATION_JAY", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNOM_CLIENT");
            balise = new XElement("TNOM_CLIENT", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCODE_CLIENT");
            balise = new XElement("TCODE_CLIENT", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCONTACT_CLIENT");
            balise = new XElement("TCONTACT_CLIENT", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCAS_DE_REFERENCE");
            balise = new XElement("TCAS_DE_REFERENCE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TAPPROBATION_CLIENT");
            balise = new XElement("TAPPROBATION_CLIENT", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDEFINITION_DE_VOTRE_SOLUTION");
            balise = new XElement("TDEFINITION_DE_VOTRE_SOLUTION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDESCRIPTION_TITLE");
            balise = new XElement("TDESCRIPTION_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDESCRIPTION_TITLE2");
            balise = new XElement("TDESCRIPTION_TITLE2", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TINDICE");
            balise = new XElement("TINDICE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDATEREVISION");
            balise = new XElement("TDATEREVISION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TREFERENCEDOSSIER");
            balise = new XElement("TREFERENCEDOSSIER", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TOPTIONS");
            balise = new XElement("TOPTIONS", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMO_TITLE");
            balise = new XElement("TMO_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TFONCTIONS");
            balise = new XElement("TFONCTIONS", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TSCREEN");
            balise = new XElement("TSCREEN", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TVERRCROIX");
            balise = new XElement("TVERRCROIX", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TVOTREEMETTEUR");
            balise = new XElement("TVOTREEMETTEUR", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TPARAMECRAN");
            balise = new XElement("TPARAMECRAN", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNBMODES");
            balise = new XElement("TNBMODES", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMODESBC");
            balise = new XElement("TMODESBC", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMODEHSBC");
            balise = new XElement("TMODEHSBC", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMODE_TITLE");
            balise = new XElement("TMODE_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TEXPLMODE");
            balise = new XElement("TEXPLMODE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMT_TITLE1");
            balise = new XElement("TMT_TITLE1", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMT_TITLE2");
            balise = new XElement("TMT_TITLE2", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TINPUT_OUPUT_TITLE");
            balise = new XElement("TINPUT_OUPUT_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TPROGBASE");
            balise = new XElement("TPROGBASE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TALARM_TITLE");
            balise = new XElement("TALARM_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TSIGNATURE");
            balise = new XElement("TSIGNATURE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TVALIDATION");
            balise = new XElement("TVALIDATION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TRENSEIG");
            balise = new XElement("TRENSEIG", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TRENSEIG2");
            balise = new XElement("TRENSEIG2", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TRENSEIG3");
            balise = new XElement("TRENSEIG3", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TOUI");
            balise = new XElement("TOUI", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNON");
            balise = new XElement("TNON", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/PRINT_SCREEN");
            balise = new XElement("PRINT_SCREEN", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TBORNIER");
            balise = new XElement("TBORNIER", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TUTILISER");
            balise = new XElement("TUTILISER", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TFONCTION");
            balise = new XElement("TFONCTION", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TTEMPO");
            balise = new XElement("TTEMPO", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNOMALARM");
            balise = new XElement("TNOMALARM", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TIMAGES");
            balise = new XElement("TIMAGES", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCONDI");
            balise = new XElement("TCONDI", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TAUTOACQ");
            balise = new XElement("TAUTOACQ", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TVIBBUZ");
            balise = new XElement("TVIBBUZ", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TSYSTEM");
            balise = new XElement("TSYSTEM", text);
            parent.Add(balise);

            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TUSER");
            balise = new XElement("TUSER", text);
            parent.Add(balise);

            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TDATE");

            balise = new XElement("TDATE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TGENERAL_PARAM_TITLE");
            balise = new XElement("TGENERAL_PARAM_TITLE", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNOM_PROJET");
            balise = new XElement("TNOM_PROJET", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TNMR_FICHE");
            balise = new XElement("TNMR_FICHE", text);
            parent.Add(balise);

            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TID_WEB");
            balise = new XElement("TID_WEB", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCODE_APPL");
            balise = new XElement("TCODE_APPL", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TCONFIG_RESEAU");
            balise = new XElement("TCONFIG_RESEAU", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("EDIT_VARIABLE/PICTOCOCHE");
            balise = new XElement("TDEMM", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("EDIT_VARIABLE/PICTOCOCHEPLUS");
            balise = new XElement("TDEMM_PLUS", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("EDIT_VARIABLE/PICTOANTI");
            balise = new XElement("TANTI", text);
            parent.Add(balise);
            
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TALARM_NAME");
            balise = new XElement("TALARM_NAME", text);
            parent.Add(balise);
            text = LanguageSupport.Get().GetTextRapport("XML_CONFIG/TMODEIMG");
            balise = new XElement("TMODEIMG", text);
            parent.Add(balise);
            
            return parent;
        }

        public XElement InitNewTableauMode(XElement parent, String Langue)
        {
            XElement Tableau = new XElement("TABLEAUMODE");
            UInt16 i = 0;
            UInt32 masksbc = PegaseData.Instance.SBC_MasqueExploitation;
            UInt32 maskhorssbc = PegaseData.Instance.SBC_MasqueExploitHors;

            XElement Result = null;
            XAttribute image = null;

            foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
            {
                String ModeNum = String.Format("Mode {0:00} : ", mode.Position + 1);
                String ModeName = mode.ModeLabel.LibelSelecteur;
                Boolean IsCheckSBCMode = PegaseCore.Helper.BitHelper.ReadBit(masksbc, i);
                Boolean IsCheckNonSBCMode = PegaseCore.Helper.BitHelper.ReadBit(maskhorssbc, i);
                i++;

                Result = new XElement("Mode" + i.ToString(), this.BuildNewSelecteurLabel(mode.ModeLabel));
                image = new XAttribute("image", "true");
                Result.Add(image);
                Result.Add(new XAttribute("connexion", IsCheckSBCMode));
                Result.Add(new XAttribute("horsconnexion", IsCheckNonSBCMode));
                Tableau.Add(Result);
            }


            parent.Add(Tableau);
            XElement modeparams = new XElement("MODEPARAMS");
            Result = new XElement("params", PegaseData.Instance.ParamHorsMode.NavigationInc + "/" +
              PegaseData.Instance.ParamHorsMode.NavigationDec);
            Result.Add(new XAttribute("item", LanguageSupport.Get().GetTextRapport("LIBEL_MO/EDIT_SELECTEUR_MODE")));
            Result.Add(new XAttribute("type", "Type3"));
            Result.Add(new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE")));
            modeparams.Add(Result);

            Result = new XElement("params", this.BuildNewSelecteurLabel(PegaseData.Instance.ParamHorsMode.LibelleEquipement));
            Result.Add(new XAttribute("item", LanguageSupport.Get().GetTextRapport("CHECK_MATERIEL/TEQUIP_RAPPORT")));
            Result.Add(new XAttribute("type", "Type3"));
            Result.Add(new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE")));
            image = new XAttribute("image", "true");
            Result.Add(image);
            modeparams.Add(Result);

            parent.Add(modeparams);
            return parent;
        }

        public XElement InitNewIO(XElement parent, String Langue)
        { int i = 0;

            foreach (var carte in PegaseData.Instance.ModuleT.Cartes)
            {
                i++;
                XElement board = new XElement("TABLEAU");
                board.Add(new XAttribute("numero", i.ToString()));
                if (carte.Description != carte.Emplacement)
                {
                    board.Add(new XAttribute("nom", carte.Description + " - " + carte.Emplacement));
                }
                else
                {
                    board.Add(new XAttribute("nom", carte.Description));
                }
                //
                board.Add(new XElement("TITREUN", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TINPUT_OUPUT_TITLE")));
                board.Add(new XElement("TITREDEUX", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TBORNIER")));
                board.Add(new XElement("TITRETROIS", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TUTILISER")));
                board.Add(new XElement("TITREQUATRE", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TFONCTION")));

                // 3 - pour toutes les entrées / sorties

                // Eanas
                IEnumerable<ES> esCarte = from es in PegaseData.Instance.ModuleT.EAnas
                                          where es.Carte == carte.ID
                                          select es;

                if (esCarte.Count() > 0)
                {
                    foreach (var es in esCarte)
                    {
                        XElement io = new XElement("LIGNE");
                        io.Add(new XElement("DEUX", es.MnemoBornier));
                        io.Add(new XElement("UN", es.MnemoHardware));
                        if (es.MnemoHardware.Equals(es.MnemoClient))
                        {
                            io.Add(new XElement("QUATRIEME", " "));
                            io.Add(new XElement("TROISIEME", "false"));
                        }
                        else
                        {
                            io.Add(new XElement("QUATRIEME", es.MnemoClient));
                            io.Add(new XElement("TROISIEME", "true"));
                        }
                        board.Add(io);
                    }
                }

                // ETors
                esCarte = from es in PegaseData.Instance.ModuleT.ETORS
                          where es.Carte == carte.ID
                          select es;

                if (esCarte.Count() > 0)
                {
                    foreach (var es in esCarte)
                    {
                        XElement io = new XElement("LIGNE");
                        io.Add(new XElement("DEUX", es.MnemoBornier));
                        io.Add(new XElement("UN", es.MnemoHardware));
                        if (es.MnemoHardware.Equals(es.MnemoClient))
                        {
                            io.Add(new XElement("QUATRIEME", " "));
                            io.Add(new XElement("TROISIEME", "false"));
                        }
                        else
                        {
                            io.Add(new XElement("QUATRIEME", es.MnemoClient));
                            io.Add(new XElement("TROISIEME", "true"));
                        }
                        board.Add(io);
                    }
                }

                // SAnas
                esCarte = from es in PegaseData.Instance.ModuleT.SAnas
                          where es.Carte == carte.ID
                          select es;

                if (esCarte.Count() > 0)
                {
                    foreach (var es in esCarte)
                    {
                        XElement io = new XElement("LIGNE");
                        io.Add(new XElement("DEUX", es.MnemoBornier));
                        io.Add(new XElement("UN", es.MnemoHardware));
                        if (es.MnemoHardware.Equals(es.MnemoClient))
                        {
                            io.Add(new XElement("QUATRIEME", " "));
                            io.Add(new XElement("TROISIEME", "false"));
                        }
                        else
                        {
                            io.Add(new XElement("QUATRIEME", es.MnemoClient));
                            io.Add(new XElement("TROISIEME", "true"));
                        }
                        board.Add(io);
                    }
                }

                // STors
                esCarte = from es in PegaseData.Instance.ModuleT.STORS
                          where es.Carte == carte.ID
                          select es;

                if (esCarte.Count() > 0)
                {
                    foreach (var es in esCarte)
                    {
                        XElement io = new XElement("LIGNE");
                        io.Add(new XElement("DEUX", es.MnemoBornier));
                        io.Add(new XElement("UN", es.MnemoHardware));
                        if (es.MnemoHardware.Equals(es.MnemoClient))
                        {
                            io.Add(new XElement("QUATRIEME", " "));
                            io.Add(new XElement("TROISIEME", "false"));
                        }
                        else
                        {
                            io.Add(new XElement("QUATRIEME", es.MnemoClient));
                            io.Add(new XElement("TROISIEME", "true"));
                        }
                        board.Add(io);
                    }
                }
                parent.Add(board);
            }

            return parent;
        }

        public XElement InitNewMode(XElement parent, String Langue)
        {

            XElement Result = null;
            int nbmode = PegaseData.Instance.OLogiciels.ModesExploitation.Count();
            // creer les pages de modes en fonction du nb mode
            string template = DefaultValues.Get().RapportFolder + "template_mode.html";
            if (File.Exists(template))
            {
                for (int i = 0; i < nbmode; i++)
                {
                    File.Copy(template, DefaultValues.Get().RapportFolder + "mode" + (i + 1).ToString() + ".html");
                }
            }

            Result = new XElement("NbMode", nbmode);
            parent.Add(Result);


            // rajouter selecteur de modes d'exploitation


            // pour chaque mode
            foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
            {
                XAttribute image = null;

                XElement newmode = new XElement("Mode");
                XAttribute numero = new XAttribute("numero", mode.Position + 1);
                newmode.Add(numero);
                string tradValue = LanguageSupport.Get().GetTextRapport("REPORT/MODE");
                Result = new XElement("Affectation", tradValue + " : " + (mode.Position + 1).ToString());
                image = new XAttribute("image", "false");
                Result.Add(image);
                newmode.Add(Result);
                // libellé Equipement
                Result = new XElement("Equipement", this.BuildNewSelecteurLabel(PegaseData.Instance.ParamHorsMode.LibelleEquipement));
                image = new XAttribute("image", "true");
                Result.Add(image);
                newmode.Add(Result);
                // titre du mode
                Result = new XElement("Mode", this.BuildNewSelecteurLabel(mode.ModeLabel));
                image = new XAttribute("image", "true");
                Result.Add(image);
                XAttribute item = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("CHECK_MATERIEL/TMODE_RAPPORT"));
                Result.Add(item);
                newmode.Add(Result);





                /*  <></INFORMATION1>
                    <INFORMATION2></INFORMATION2>
                    <INFORMATION3></INFORMATION3>
                    <SELECTEUR1></SELECTEUR1>
                    <SELECTEUR2></SELECTEUR2>
                    <SELECTEUR3></SELECTEUR3>
                    <SELECTEUR4></SELECTEUR4>
                    <SELECTEUR5></SELECTEUR5>
                    <SELECTEUR6></SELECTEUR6> */



                // retour d'informations
                int cpt = 0;
                for (cpt = 0; cpt < 3; cpt++)
                {
                    // XElement newretour = new XElement("Retour" + cpt);
                    if (PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt] != null)
                    {
                        InformationLabel temp_RINUM = null;
                        InformationLabel temp_label = null;
                        InformationLabel temp_texte = null;
                        InformationLabel temp_unite = null;
                        InformationLabel temp_item1 = null;
                        if (PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].Information != null)
                        {
                            XElement retour = new XElement("Retour" + cpt);

                            Result = new XElement("Feedback", "");
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/RETOUR" + (cpt + 1).ToString()));
                            Result.Add(image);
                            retour.Add(Result);

                            InformationLabel temporaire = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].InfoLibelMax;
                            Result = new XElement("LibelMax", this.BuildNewInformationLabel(temporaire));
                            image = new XAttribute("image", "true");
                            Result.Add(image);

                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/VALMAX"));
                            Result.Add(image);
                            retour.Add(Result);
                            temporaire = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].InfoLibelMin;
                            temp_item1 = temporaire;
                            Result = new XElement("LibelMin", this.BuildNewInformationLabel(temporaire));

                            image = new XAttribute("image", "true");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/VALMIN"));
                            Result.Add(image);
                            retour.Add(Result);
                            temporaire = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].InfoLibelUnit;
                            Result = new XElement("LibelUnit", this.BuildNewInformationLabel(temporaire));
                            temp_unite = temporaire;

                            image = new XAttribute("image", "true");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/UNITE"));
                            Result.Add(image);
                            retour.Add(Result);
                            temporaire = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].InfoLibelRetour;
                            Result = new XElement("LibelRetour", this.BuildNewInformationLabel(temporaire));
                            temp_label = temporaire;

                            image = new XAttribute("image", "true");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/RI"));
                            Result.Add(image);
                            retour.Add(Result);
                            Double val = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].ValMin;
                            Result = new XElement("ValMin", val.ToString());
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/MO_EME_SMIN"));
                            Result.Add(image);
                            retour.Add(Result);
                            val = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].ValMax;
                            temp_texte = temporaire;
                            temp_texte = new InformationLabel(val.ToString());
                            temp_texte.PoliceGras = false;
                            temp_texte.IsBitmap = false;
                            temp_texte.Label = val.ToString();
                            Result = new XElement("ValMax", val.ToString());
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/MO_EME_SMAX"));
                            Result.Add(image);
                            retour.Add(Result);
                            string mnemologique = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].Information.MnemoLogique;
                            Result = new XElement("ValMax", mnemologique);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/RI_VAR"));
                            Result.Add(image);
                            retour.Add(Result);
                            int cpt2 = 0;
                            foreach (var libel in PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].LibelsVarNumerique)
                            {
                                temporaire = libel;
                                if (temporaire != null)
                                {
                                    Result = new XElement("LibelNum" + cpt2, this.BuildNewInformationLabel(temporaire));
                                    image = new XAttribute("image", "true");
                                    Result.Add(image);
                                    image = new XAttribute("titre", cpt2 + 1 + ": ");
                                    Result.Add(image);
                                    retour.Add(Result);
                                    if (temp_RINUM == null)
                                    {
                                        temp_RINUM = temporaire;
                                    }
                                }
                                cpt2++;
                            }
                            newmode.Add(retour);
                            // cree l'affichage de retour 
                            RIType tmp = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsInfoByPos[cpt].Information.TypeVariable;
                            if ((tmp == RIType.RIVARNUM) || (tmp == RIType.RIVARNUM_DISTANT))
                            {
                                if (temp_RINUM != null)
                                {
                                    Result = new XElement("INFORMATION" + (cpt + 1).ToString(), this.BuildNewInformationLabel(temp_RINUM));
                                    image = new XAttribute("image", "true");
                                    Result.Add(image);
                                    newmode.Add(Result);
                                }
                            }
                            else if ((tmp == RIType.RIBOOL) || (tmp == RIType.RIBOOL_DISTANT))
                            {
                                Result = new XElement("INFORMATION" + (cpt + 1).ToString(), this.BuildNewInformationLabelMulti(temp_label, temp_item1, null));
                                image = new XAttribute("image", "true");
                                Result.Add(image);
                                newmode.Add(Result);
                            }
                            else
                            {

                                Result = new XElement("INFORMATION" + (cpt + 1).ToString(), this.BuildNewInformationLabelMulti(temp_label, temp_texte, temp_unite));
                                image = new XAttribute("image", "true");
                                Result.Add(image);
                                newmode.Add(Result);
                            }
                        }

                    }
                    else
                    {
                        XElement retour = new XElement("Retour" + cpt);

                        Result = new XElement("Feedback", "");
                        image = new XAttribute("image", "false");
                        Result.Add(image);
                        image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/RETOUR" + (cpt + 1).ToString()));
                        Result.Add(image);
                        retour.Add(Result);
                        newmode.Add(retour);
                    }
                }
                // fin add retour
                //ajout des selecteur
                for (cpt = 0; cpt < 6; cpt++)
                {
                    if (PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt] != null)
                    {
                        XElement selecteur = new XElement("Selecteur" + cpt);

                        Result = new XElement("Selecteur", "");
                        image = new XAttribute("image", "false");
                        Result.Add(image);
                        image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/SELECTEUR" + (cpt + 1).ToString()));
                        Result.Add(image);
                        selecteur.Add(Result);
                        //newmode.Add(selecteur);

                        SelecteurLabel temporaireAff = null;
                        if (PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsSelecteurByPos[cpt] != null)
                        {

                            if (PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsSelecteurByPos[cpt].Count > 0)
                            {
                                int nbsel = 0;

                                foreach (var pos in PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].LabelsSelecteurByPos[cpt])
                                {
                                    SelecteurLabel temporaire = pos;

                                    if (temporaireAff == null)
                                    {
                                        temporaireAff = pos;
                                    }
                                    Result = new XElement("Pos" + nbsel, this.BuildNewSelecteurLabel(temporaire));
                                    image = new XAttribute("image", "true");
                                    Result.Add(image);
                                    image = new XAttribute("titre", nbsel + 1 + ": ");
                                    Result.Add(image);
                                    selecteur.Add(Result);
                                    nbsel++;
                                }
                            }
                        }
                        int type = (int)PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].SelecteurType;
                        if (type == 3)
                        {
                            string tmp2 = "";
                            tmp2 = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].BtDecrementer;
                            if (tmp2.Equals(""))
                            {
                                tmp2 = "__";
                            }
                            Result = new XElement("BtDecrementer", tmp2);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/BTN_DECR"));
                            Result.Add(image);
                            selecteur.Add(Result);

                            tmp2 = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].BtIncrementer;
                            if (tmp2.Equals(""))
                            {
                                tmp2 = "__";
                            }
                            Result = new XElement("BtIncrementer", tmp2);
                            image = new XAttribute("image", "false");
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/BTN_INCR"));
                            Result.Add(image);
                            selecteur.Add(Result);

                            tmp2 = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].BtPositionnerAValMax;
                            if (tmp2.Equals(""))
                            {
                                tmp2 = "__";
                            }
                            Result = new XElement("BtPositionnerAValMax", tmp2);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/BTN_AVALMAX"));
                            Result.Add(image);
                            selecteur.Add(Result);
                            tmp2 = PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].BtPositionnerAValMin;
                            if (tmp2.Equals(""))
                            {
                                tmp2 = "__";
                            }
                            Result = new XElement("BtPositionnerAValMin", tmp2);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/BTN_AVALMIN"));
                            Result.Add(image);
                            selecteur.Add(Result);

                            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ComportementAuxBornesHM", this.LanguageName));
                            string tmp = LCM.GetLabel(((int)PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].ComportementAuxBornes).ToString());
                            Result = new XElement("ComportementAuxBornes", tmp);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/COMPORT_BORNES"));
                            Result.Add(image);
                            selecteur.Add(Result);
                        }

                        if (type == 2)
                        {
                            Result = new XElement("OrganeARecopier", PegaseData.Instance.OLogiciels.ModesExploitation[mode.Position].Selecteurs[cpt].OrganeARecopier);
                            image = new XAttribute("image", "false");
                            Result.Add(image);
                            image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/ORGANE_RECOP"));
                            Result.Add(image);
                            selecteur.Add(Result);
                        }
                        newmode.Add(selecteur);
                        Result = new XElement("SELECTEUR" + cpt.ToString(), this.BuildNewSelecteurLabel(temporaireAff));
                        image = new XAttribute("image", "true");
                        Result.Add(image);
                        newmode.Add(Result);
                    }
                    else
                    {
                        XElement selecteur = new XElement("Selecteur" + cpt);

                        Result = new XElement("Selecteur", "");
                        image = new XAttribute("image", "false");
                        Result.Add(image);
                        image = new XAttribute("titre", LanguageSupport.Get().GetTextRapport("/LIBEL_MO/SELECTEUR" + (cpt + 1).ToString()));
                        Result.Add(image);
                        selecteur.Add(Result);
                        newmode.Add(selecteur);
                    }

                }

                parent.Add(newmode);
            }

            //XElement info = new XElement("libelMin", this.BuildNewSelecteurLabel(retour.));


            //    // 2.1 - Numéro du mode
            //    String NumMode = String.Format("Mode {0}", i++);
            //    Screen = Screen.Replace("$BALISE1001", NumMode);

            //    // 2.2 - Nom de l'équipement
            //    Screen = Screen.Replace("$BALISE1002", this.BuildSelecteurLabel(PegaseData.Instance.ParamHorsMode.LibelleEquipement));

            //    // 2.3 - Nom du mode
            //    Screen = Screen.Replace("$BALISE1003", this.BuildSelecteurLabel(mode.ModeLabel));

            //    // 2.4 - Retour d'information
            //    Int32 j = 1004;
            //    for (Int32 k = 0; k < 3; k++)
            //    {
            //        InfoByMode retourInfo = mode.LabelsInfoByPos[k];
            //        String LibelleInfo = " ";
            //        if (retourInfo != null)
            //        {
            //            if (retourInfo.Information.TypeVariable == RIType.RIANA)
            //            {
            //                LibelleInfo = String.Format("{0} {1} {2}", this.BuildInfoLabel(retourInfo.InfoLibelRetour), retourInfo.ValueDemo, this.BuildInfoLabel(retourInfo.InfoLibelUnit));
            //            }
            //            else if (retourInfo.Information.TypeVariable == RIType.RIBOOL)
            //            {
            //                LibelleInfo = String.Format("{0}", this.BuildInfoLabel(retourInfo.InfoLibelMin));
            //            }
            //            else if (retourInfo.Information.TypeVariable == RIType.RIVARNUM)
            //            {
            //                if (retourInfo.LibelsVarNumerique.Length > 0)
            //                {
            //                    LibelleInfo = String.Format("{0}", this.BuildInfoLabel(retourInfo.LibelsVarNumerique[0]));
            //                }
            //            }
            //        }

            //        Screen = Screen.Replace(String.Format("$BALISE{0:0000}", j++), LibelleInfo);
            //    }

            //    // 2.5 - Selecteur
            //    foreach (var selecteur in mode.LabelsSelecteurByPos)
            //    {
            //        String LibelSelecteur = " ";
            //        if (selecteur != null && selecteur.Count > 0)
            //        {
            //            LibelSelecteur = this.BuildSelecteurLabel(selecteur[0]);
            //        }
            //        Screen = Screen.Replace(String.Format("$BALISE{0:0000}", j++), LibelSelecteur);
            //    }

            //    // 3 - Création de la balise écran
            //    XElement XScreen = XElement.Parse(Screen);
            //    screenContener.Add(XScreen);
            //}

            //this.Fields.Add("$BALISE1000", screenContener);


            return parent;
        }

        public XElement InitNewMt(XElement parent, string langue)
        {
            XElement mt = new XElement("MT");
            string type = PegaseData.Instance.ModuleT.RefIndus.Substring(0, 1).ToUpper();
            String modelresult = "";
            switch (type)
            {
                case "E":
                    modelresult = "ELIO";
                    break;
                case "A":
                    modelresult = "ALTO";
                    break;
                case "T":
                    modelresult = "TIMO";
                    break;
                case "N":
                    modelresult = "NEMO";
                    break;
            }
            XAttribute model = new XAttribute("MODEL", modelresult);
            mt.Add(model);

            XElement cartes = new XElement("Boards");

            // Images MT nouveau rapport
            ObservableCollection<ImgDefinition> ImgMT;
            ImgDefinition ImgPrincipal;
            DecodConfigRefIndusAppl.Get().GetRapportImg(PegaseData.Instance.ModuleT.RefIndus.Substring(0, 2), PegaseData.Instance.RefErpMT, out ImgPrincipal, out ImgMT);
            int cpt = 0;
            if (ImgPrincipal.Image1 != "")
            {
                XElement carte = new XElement("Board" + cpt, "imagesrapport/" + ImgPrincipal.Image1);
                // XElement text = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + ImgPrincipal.Alias));
                XElement text = new XElement("TextBoard" + cpt, "");
                XAttribute image = new XAttribute("Image", "true");
                carte.Add(image);
                cartes.Add(carte);
                cartes.Add(text);
                cpt++;
            }
            if (ImgPrincipal.Image2 != "")
            {
                XElement carte = new XElement("Board" + cpt, "imagesrapport/" + ImgPrincipal.Image2);
                // XElement text = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + ImgPrincipal.Alias));
                XElement text = new XElement("TextBoard" + cpt, "");
                XAttribute image = new XAttribute("Image", "true");
                carte.Add(image);
                cartes.Add(carte);
                cartes.Add(text);
                cpt++;
            }
            if (ImgMT.Count > 0)
            {
                foreach (var img in ImgMT)
                {
                    if (img.Image2 != "")
                    {
                        XElement carte = new XElement("Board" + cpt, "imagesrapport/" + img.Image2);
                        XAttribute image = new XAttribute("Image", "true");
                        XElement text = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + img.Alias));

                        carte.Add(image);
                        cartes.Add(carte);
                        cartes.Add(text);
                        cpt++;
                    }
                }
            }


            mt.Add(cartes);
            // ajout  klaxon
            //ajout  description carte
            bool KlaxonPossible;
            bool klaxon = DecodConfigRefIndusAppl.Get().GetPresenceKlaxon(PegaseData.Instance.ModuleT.RefIndus.Substring(0, 2), PegaseData.Instance.RefErpMT, out KlaxonPossible);
            if (KlaxonPossible == true)
            {
                // on affiche l'info
                XElement XKlaxon = new XElement("params", klaxon.ToString());
                XAttribute item1 = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/RAPPORT_KLAXON"));
                XKlaxon.Add(item1);
                item1 = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
                XKlaxon.Add(item1);
                item1 = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
                XKlaxon.Add(item1);
                item1 = new XAttribute("type", "Type1");
                XKlaxon.Add(item1);
                mt.Add(XKlaxon);
            }
            // ajout protocole de com 

            VariableEditable VE = GestionVariableSimple.Instance.GetVariableByName("ModBus");
            String modbus = PegaseData.Instance.GestionModBus.ChoixModBus;
            String result = " ";
            if (VE != null)
            {
                if ((modbus != "") && (modbus != "0"))
                {
                    result = result + "ModBus-";
                }
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("CanOpen");
            if (VE != null)
            {
                if ((VE.RefElementValue != "") && (VE.RefElementValue != "0"))
                {
                    result = result + "CAN-";
                }
            }
            result = result.Substring(0, result.Length - 1);

            XElement protocole = new XElement("PROTOCOLE", result);
            XAttribute item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/RAPPORT_PROTOCOLE"));
            protocole.Add(item);
            item = new XAttribute("type", "Type2");
            protocole.Add(item);
            item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            protocole.Add(item);


            parent.Add(mt);

            return parent;
        }
        public XElement InitNewMo(XElement parent, string langue)
        {
            XElement mo = new XElement("MO");
            string type = PegaseData.Instance.MOperateur.RefIndus.Substring(0, 1).ToUpper();
            String modelresult = "";
            // PegaseData.Instance.MOperateur.

            switch (type)
            {
                case "B":
                    modelresult = "BETA";
                    break;
                case "G":
                    modelresult = "GAMA";
                    break;
                case "P":
                    modelresult = "PIKA";
                    break;
                case "M":
                    modelresult = "MOKA";
                    break;
            }
            XAttribute model = new XAttribute("MODEL", modelresult);
            mo.Add(model);

            XElement cartes = new XElement("Boards");

            // Images MT nouveau rapport
            ObservableCollection<ImgDefinition> ImgMO;
            ImgDefinition ImgPrincipal;
            DecodConfigRefIndusAppl.Get().GetRapportImg(PegaseData.Instance.MOperateur.RefIndus.Substring(0, 2), PegaseData.Instance.RefErpMO, out ImgPrincipal, out ImgMO);
            int cpt = 0;
            XElement carteP = null;
            XElement textP = null;
            XAttribute imageP = null;
            String RefSup = "";

            if (ImgMO.Count > 0)
            {
                foreach (var img in ImgMO)
                {
                    if (img.Refsup != "")
                    {
                        RefSup = RefSup + img.Refsup;
                    }
                }
                if (ImgMO[0].Image1 == "") 
                {
                    if (ImgPrincipal.Image1 != "")
                    {
                        carteP = new XElement("Board" + cpt, "imagesrapport/" + RefSup+ImgPrincipal.Image1);
                        textP = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + ImgPrincipal.Alias));
                        imageP = new XAttribute("Image", "true");
                        carteP.Add(imageP);
                        cartes.Add(carteP);
                        cartes.Add(textP);
                        cpt++;
                    }
                }
                else
                {
                    carteP = new XElement("Board" + cpt, "imagesrapport/" + RefSup+ImgMO[0].Image1);
                    textP = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + ImgMO[0].Alias));
                    imageP = new XAttribute("Image", "true");
                    carteP.Add(imageP);
                    cartes.Add(carteP);
                    cartes.Add(textP);
                    cpt++;
                }
                foreach (var img in ImgMO)
                {
                    if (img.Image2 != "")
                    {
                        XElement carte = new XElement("Board" + cpt, "imagesrapport/" + img.Image2);
                        XAttribute image = new XAttribute("Image", "true");
                        XElement text = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + img.Alias));

                        carte.Add(image);
                        cartes.Add(carte);
                        cartes.Add(text);
                        cpt++;
                    }
                }
            }
            else
            {
                carteP = new XElement("Board" + cpt, "imagesrapport/" + ImgPrincipal.Image1);
                textP = new XElement("TextBoard" + cpt, LanguageSupport.Get().GetTextRapport("/XML_CONFIG/" + ImgPrincipal.Alias));
                imageP = new XAttribute("Image", "true");
                carteP.Add(imageP);
                cartes.Add(carteP);
                cartes.Add(textP);
                cpt++;
            }

            mo.Add(cartes);
            XElement Organes = new XElement("organe");
            XElement JOY11 = new XElement( "J11");
            XElement JOY21 = new XElement( "J21");
            XElement JOY31 = new XElement( "J31");
            XElement JOY12 = new XElement( "J12");
            XElement JOY22 = new XElement( "J22");
            XElement JOY32 = new XElement( "J32");

            uint Mask32Verr = 0;
            uint Mask16Verr = 0;
            uint Mask32Anti = 0;
            uint Mask16Anti = 0;
            // init des data de verreouillage
            try
            {
                foreach (var editable in JAY.PegaseCore.InternalDataModel.GestionVariableSimple.Instance.ListVariableEditable)
                {
                    if (editable.VarName.Equals("VerifierBpTogg"))
                    {
                        Mask32Verr = Convert.ToUInt32(editable.RefElementValue);
                    }
                }
                 String VerifierJS1;
                VerifierJS1 = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1", "", "", XML_ATTRIBUTE.VALUE);
                String VerifierJS2;
                VerifierJS2 = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2", "", "", XML_ATTRIBUTE.VALUE);
                String VerifierJS3;
                VerifierJS3 = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3", "", "", XML_ATTRIBUTE.VALUE);
                if (!VerifierJS1.Equals("0"))
                {
                    Mask32Verr = Mask32Verr & (uint)0xFFFFBFFF;
                }
                else
                {
                    Mask32Verr = Mask32Verr | (uint)0x00004000;
                }
                if (!VerifierJS2.Equals("0"))
                {
                    Mask32Verr = Mask32Verr & (uint)0xFFFF7FFF;
                }
                else
                {
                    Mask32Verr = Mask32Verr | (uint)0x00008000;
                }
                if (!VerifierJS3.Equals("0"))
                {
                    Mask32Verr = Mask32Verr & (uint)0xFFFEFFFF;
                }
                else
                {
                    Mask32Verr = Mask32Verr | (uint)0x00010000;
                }
            }
            catch
            {
                Mask32Verr = 0;
            }
            try
            {

                String VerifierJS1Ana;
                VerifierJS1Ana = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1Ana", "", "", XML_ATTRIBUTE.VALUE);
                String VerifierJS2Ana;
                VerifierJS2Ana = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2Ana", "", "", XML_ATTRIBUTE.VALUE);
                String VerifierJS3Ana;
                VerifierJS3Ana = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3Ana", "", "", XML_ATTRIBUTE.VALUE);
            }
            catch
            {
            }
            try
            {
                foreach (var editable in JAY.PegaseCore.InternalDataModel.GestionVariableSimple.Instance.ListVariableEditable)
                {
                    if (editable.VarName.Equals("MasqueBoutons"))
                    {
                        Mask32Anti = Convert.ToUInt32(editable.RefElementValue);
                    }
                }
            }
            catch
            {
                Mask32Anti = 0;
            }
            VariableEditable VAsD = GestionVariableSimple.Instance.GetVariableByName("DureeReposMin");
            String    tempoAnti = VAsD.RefElementValue;
            int tempo = 0;
            try
            {
                tempo = Convert.ToInt32(tempoAnti);
                
            }
             catch{
                tempo = 0;           
            }

            foreach (var organes in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                // 
               
                //if (!organes.MnemoHardFamilleMO.Contains("AXE"))
                if (true)
                {
                    if ((organes.MnemoHardFamilleMO == "CO" && !organes.ReferenceOrgan.Contains("VIDE") && !organes.ReferenceOrgan.Contains("Descrip_")) || organes.MnemoHardFamilleMO != "CO")
                    {
                        String PosOrgane;

                        if (organes.MnemoHardFamilleMO.Contains("AC"))
                        {
                            Int32 posO;
                            if (organes.NbPosOrgane < 15)
                            {
                                if (organes.NbPosOrgane > 6)
                                {
                                    posO = (organes.NbPosOrgane - 4) / 2;
                                    PosOrgane = String.Format(" +{0}/-{0}", posO.ToString());
                                }
                                else
                                {
                                    PosOrgane = "Lock";
                                }
                            }
                            else
                            {
                                PosOrgane = "No switch. Seq.";
                            }
                        }
                        else
                        {
                            PosOrgane = organes.NbPosOrgane.ToString();
                        }



                        if (organes.NomOrganeMO.Substring(0,1).Equals("N") || organes.NomOrganeMO.Substring(0, 1).Equals("F") || 
                            organes.NomOrganeMO.Substring(0, 1).Equals("M") || (organes.NomOrganeMO.Contains("iCharger")))
                        {
                            int verr = decodeMASK(Mask32Verr, organes.NomOrganeMO);
                            int anti = decodeMASK(Mask32Anti, organes.NomOrganeMO);
                            String verrString = DecodeImg(verr, "TypeVerr");
                            String antiString = "";
                            if (tempo == 0)
                            {
                                 antiString = DecodeImg(0, "TypeAnti");
                            }
                            else
                            {
                                 antiString = DecodeImg(anti, "TypeAnti");
                            }
                           // String antiString = DecodeImg(anti, "TypeAnti");
                            XElement ParaName = new XElement("BOUTON");
                            XAttribute attrib = new XAttribute("name",organes.NomOrganeMO);
                            ParaName.Add(attrib);
                            ParaName.Value = String.Format("{1}-{2} {0}", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/STATES"), organes.MnemoClient, PosOrgane);
                            attrib = new XAttribute("CHECK", verrString);
                            ParaName.Add(attrib);
                            attrib = new XAttribute("ANTI", antiString);
                            ParaName.Add(attrib);
                            mo.Add(ParaName);
                           
                        }
                        else if (( organes.MnemoHardFamilleMO == "AC") || (organes.MnemoHardFamilleMO == "AXE"))
                        {
                            //AXIS1 AXANA1
                            if (organes.NomOrganeMO.Equals("AXIS1"))
                            {
                                XElement name = new XElement("NbCran",organes.LibelPosition );
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY11.Add(name);

                            }
                            else if (organes.NomOrganeMO.Equals("AXIS2"))
                            {
                                XElement name = new XElement("NbCran", organes.LibelPosition);
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY12.Add(name);
                            }
                            else if (organes.NomOrganeMO.Equals("AXIS3"))
                            {
                                XElement name = new XElement("NbCran", organes.LibelPosition);
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY21.Add(name);

                            }
                            else if (organes.NomOrganeMO.Equals("AXIS4"))
                            {
                                XElement name = new XElement("NbCran", organes.LibelPosition);
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY22.Add(name);
                            }
                            else if (organes.NomOrganeMO.Equals("AXIS5"))
                            {
                                XElement name = new XElement("NbCran", organes.LibelPosition);
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY31.Add(name);

                            }
                            else if (organes.NomOrganeMO.Equals("AXIS6"))
                            {
                                XElement name = new XElement("NbCran", organes.LibelPosition);
                                XAttribute libel = new XAttribute("nb", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/NBCRANS"));
                                name.Add(libel);
                                JOY32.Add(name);
                            }
                            else if (organes.NomOrganeMO.Equals("AXANA1"))
                            {
                                XElement name;
                                if (organes.MnemoHardOrganeMO != organes.MnemoClient)
                                {

                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                }
                                else
                                {
                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO );
                                }
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXEEO"));
                                name.Add(axe);
                                JOY11.Add(name);
                                name = new XElement("VERR", organes.VerrouillageEnCroix);
                                JOY11.Add(name);
                                string verr = "img/Blank.png";
                                if (PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1Ana", "", "", XML_ATTRIBUTE.VALUE).Equals("0"))
                                {
                                    verr = "img/Verification_plus.png";
                                }
                                XAttribute attr = new XAttribute("CHECK", verr);
                                JOY11.Add(attr);
                            }
                            else if (organes.NomOrganeMO.Equals("AXANA2"))
                            {
                                XElement name;
                                if (organes.MnemoHardOrganeMO != organes.MnemoClient)
                                {

                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                }
                                else
                                {
                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO);
                                }
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXENS"));
                                name.Add(axe);
                                JOY12.Add(name);
                            }
                            else if (organes.NomOrganeMO.Equals("AXANA3"))
                            {
                                XElement name;
                                if (organes.MnemoHardOrganeMO != organes.MnemoClient)
                                {

                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                }
                                else
                                {
                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO);
                                }
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXEEO"));
                                name.Add(axe);
                                JOY21.Add(name);
                                name = new XElement("VERR", organes.VerrouillageEnCroix);
                                JOY21.Add(name);
                                string verr = "img/Blank.png";
                                if (PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2Ana", "", "", XML_ATTRIBUTE.VALUE).Equals("0"))
                                {
                                    verr = "img/Verification_plus.png";
                                }
                                XAttribute attr = new XAttribute("CHECK", verr);
                                JOY21.Add(attr);
                            }
                            else if (organes.NomOrganeMO.Equals("AXANA4"))
                            {
                                XElement name;
                                if (organes.MnemoHardOrganeMO != organes.MnemoClient)
                                {

                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                }
                                else
                                {
                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO);
                                }
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXENS"));
                                name.Add(axe);
                                JOY22.Add(name);

                            }
                            else if (organes.NomOrganeMO.Equals("AXANA5"))
                            {
                                XElement name;
                                if (organes.MnemoHardOrganeMO != organes.MnemoClient)
                                {

                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                }
                                else
                                {
                                    name = new XElement("MNEMO", organes.MnemoHardOrganeMO);
                                }
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXEEO"));
                                name.Add(axe);
                                JOY31.Add(name);
                                name = new XElement("VERR", organes.VerrouillageEnCroix);
                                JOY31.Add(name);
                                string verr = "img/Blank.png";
                                if (PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3Ana", "", "", XML_ATTRIBUTE.VALUE).Equals("0"))
                                {
                                    verr = "img/Verification_plus.png";
                                }
                                XAttribute attr = new XAttribute("CHECK", verr);
                                JOY31.Add(attr);
                            }
                            else if (organes.NomOrganeMO.Equals("AXANA6"))
                            {
                                XElement name = new XElement("MNEMO", organes.MnemoHardOrganeMO + "-" + organes.MnemoClient);
                                XAttribute axe = new XAttribute("axe", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/AXENS"));
                                name.Add(axe);
                                JOY32.Add(name);

                            }
                            else
                            {
                                XElement ParaName = new XElement("BOUTON");
                                XAttribute attrib = new XAttribute("name", organes.NomOrganeMO);
                                ParaName.Add(attrib);
                                ParaName.Value = String.Format("{1}-{2} {3} deg ", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/STATES"), organes.MnemoClient, LanguageSupport.Get().GetText("/CHECK_MATERIEL/" + organes.ReferenceOrgan), organes.OrientationOrgane);


                                int verr = decodeMASK(Mask32Verr, organes.NomOrganeMO);
                                int anti = decodeMASK(Mask32Anti, organes.NomOrganeMO);
                                String verrString = DecodeImg(verr, "TypeVerr");
                                String antiString = DecodeImg(verr, "TypeAnti");

                                attrib = new XAttribute("CHECK", verrString);
                                ParaName.Add(attrib);
                                attrib = new XAttribute("ANTI", antiString);
                                ParaName.Add(attrib);
                                mo.Add(ParaName);
                            }
                        }
                        else
                        {
                            XElement ParaName = new XElement("BOUTON");
                            XAttribute attrib = new XAttribute("name",organes.NomOrganeMO);
                            ParaName.Add(attrib);
                            ParaName.Value = String.Format("{1}-{2} {3} deg ", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/STATES"), organes.MnemoClient, LanguageSupport.Get().GetText("/CHECK_MATERIEL/" + organes.ReferenceOrgan), organes.OrientationOrgane);


                            int verr = decodeMASK(Mask32Verr, organes.NomOrganeMO);
                            int anti = decodeMASK(Mask32Anti, organes.NomOrganeMO);
                            String verrString = DecodeImg(verr, "TypeVerr");
                            String antiString = DecodeImg(verr, "TypeAnti");

                            attrib = new XAttribute("CHECK", verrString);
                            ParaName.Add(attrib);
                            attrib = new XAttribute("ANTI", antiString);
                            ParaName.Add(attrib);
                            mo.Add(ParaName);
                        }                      
                    }
                }
            }


            // titre

            int Value2;

            XElement option = new XElement("params");
            XAttribute titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TMATERIEL"));
            option.Add(titre);
            titre = new XAttribute("type", "Type0");
            option.Add(titre);
            mo.Add(option);

            
            Value2 = PegaseData.Instance.MOperateur.PresenceIR;
            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/OUI_NON"));
            string tmp = "";
            switch (Value2)
            {
                case 0:
                    tmp = LanguageSupport.Get().GetTextRapport("/LIBEL_MO/LIBEL_MO_IR_ABSENT");
                    break;
                case 1:
                    tmp = LanguageSupport.Get().GetTextRapport("/LIBEL_MO/LIBEL_MO_IR_FACE");
                    break;
                case 2:
                    tmp = LanguageSupport.Get().GetTextRapport("/LIBEL_MO/LIBEL_MO_IR_EXTENSION");
                    break;
                default:
                    tmp = LanguageSupport.Get().GetTextRapport("/LIBEL_MO/LIBEL_MO_IR_ABSENT");
                    break;
            }
             option = new XElement("params", tmp );
             titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/IR_RAPPORT"));
            option.Add(titre);
            titre = new XAttribute("type", "Type3");
            option.Add(titre);

            mo.Add(option);



            // Présence auxiliaire
            Value2 = PegaseData.Instance.MOperateur.PresenceAuxiliaire;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/BPAUX_RAPPORT"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
            option.Add(titre);
            mo.Add(option);
            // Présence IR

            // Présence vibreur
            Value2 = PegaseData.Instance.MOperateur.PresenceVibreur;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/VIBREUR_RAPPORT"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetText("/XML_CONFIG/OUI"));
            option.Add(titre);
            mo.Add(option);


            // Présence acceleromètre
            Value2 = PegaseData.Instance.MOperateur.PresenceAccelerometre;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/GSENSOR_RAPPORT"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
            option.Add(titre);
            mo.Add(option);


            // Présence buzzer
            Value2 = PegaseData.Instance.MOperateur.PresenceBuzzer;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/BUZZER_RAPPORT"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
            option.Add(titre);
            mo.Add(option);

            // Présence liaison filaire
            Value2 = PegaseData.Instance.MOperateur.Presenceliaisonfilaire;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/LIAISON_FILAIRE"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
            option.Add(titre);
            mo.Add(option);
            // Présence bau gris
            Value2 = PegaseData.Instance.MOperateur.PresenceBAUGris;
            
            tmp = "";
            switch (Value2)
            {
                case 0:
                    tmp = LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ARRET_URGENCE");
                    break;
                case 1:
                    tmp = LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ARRET_SECURITE");
                    break;
            }

            option = new XElement("params", tmp);
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/BAU_ARRET"));
            option.Add(titre);
            titre = new XAttribute("type", "Type3");
            option.Add(titre);
            mo.Add(option);
            
            // Présence galva
            Value2 = PegaseData.Instance.MOperateur.PresenceBAUGris;
            option = new XElement("params", LCM.GetLabel(Value2.ToString()));
            titre = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/CHECK_MATERIEL/BOUTON_GALVA"));
            option.Add(titre);
            titre = new XAttribute("type", "Type1");
            option.Add(titre);
            titre = new XAttribute("choix1", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/NON"));
            option.Add(titre);
            titre = new XAttribute("choix2", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/OUI"));
            option.Add(titre);
            if (Value2 > 0)
            {
                mo.Add(option);
            }


            if (JOY11.FirstNode != null)
            {
                mo.Add(JOY11);
            }
            if (JOY12.FirstNode != null)
            {
                mo.Add(JOY12);
            }
            if (JOY21.FirstNode != null)
            {
                mo.Add(JOY21);
            }
            if (JOY22.FirstNode != null)
            {
                mo.Add(JOY22);
            }
            if (JOY31.FirstNode != null)
            {
                mo.Add(JOY31);
            }
            if (JOY32.FirstNode != null)
            {
                mo.Add(JOY32);
            }
            // ajout des options MO 
            

            parent.Add(mo);
            return parent;
        }

        public String DecodeImg(int code,String Type)
        {
            if (Type.Equals("TypeVerr"))
            {
                if (code.Equals(1))
                {
                    return ("img/Verification.png");
                }
                else if (code.Equals(2))
                {
                    return ("img/Verification_plus.png");
                }
                else
                {
                    return ("img/Blank.png");
                }
            }
            else if (Type.Equals("TypeAnti"))
            {
                if (code.Equals(2))
                {
                    return ("img/Antipianotage.png");
                }
                else
                {
                    return ("img/Blank.png");
                }
            }
            else
            {
                return "";
            }
        }

        public int decodeMASK(UInt32 Mask32, string bp)
        {
            int result = 0;

            switch (bp)
            {
                case "M" :
                    result = 1;
                    break;
                case "N1":
                    result = 2;
                    break;
                case "N2":
                    result = 3;
                    break;
                case "F1":
                    result = 4;
                    break;
                case "F2":
                    result = 5;
                    break;
                case "F3":
                    result = 6;
                    break;
                case "F4":
                    result = 7;
                    break;
                case "F5":
                    result = 8;
                    break;
                case "F6":
                    result = 9;
                    break;
                case "F7":
                    result = 10;
                    break;
                case "F8":
                    result = 11;
                    break;
                case "F9":
                    result = 12;
                    break;
                case "F10":
                    result = 13;
                    break;
                case "V1":
                    result = 14;
                    break;
                case "V2":
                    result = 15;
                    break;
                case "V3":
                    result = 16;
                    break;
                case "A13":
                    result = 17;
                    break;
                case "C1_1":
                    result = 18;
                    break;
                case "C1_2":
                    result = 19;
                    break;
                case "A1":
                    result = 20;
                    break;
                case "A2":
                    result = 21;
                    break;
                case "A3":
                    result = 22;
                    break;
                case "A4":
                    result = 23;
                    break;
                case "A5":
                    result = 24;
                    break;
                case "A6":
                    result = 25;
                    break;
                case "A7":
                    result = 26;
                    break;
                case "A8":
                    result = 27;
                    break;
                case "A9":
                    result = 28;
                    break;
                case "A10":
                    result = 29;
                    break;
                case "A11":
                    result = 30;
                    break;
                case "A12":
                    result = 31;
                    break;
            }
            //M32[1] = this.EtatBtnMarche;
            //M32[2] = this.EtatBtnNav1;
            //M32[3] = this.EtatBtnNav2;
            //M32[4] = this.EtatBtn01;
            //M32[5] = this.EtatBtn02;
            //M32[6] = this.EtatBtn03;
            //M32[7] = this.EtatBtn04;
            //M32[8] = this.EtatBtn05;
            //M32[9] = this.EtatBtn06;
            //M32[10] = this.EtatBtn07;
            //M32[11] = this.EtatBtn08;
            //M32[12] = this.EtatBtn09;
            //M32[13] = this.EtatBtn10;
            //M32[14] = this.EtatBtnJoystick01;
            //M32[15] = this.EtatBtnJoystick02;
            //M32[16] = this.EtatBtnJoystick03;
            //M32[17] = this.EtatBtnAuxiliaire01;
            //M32[18] = this.EtatBtnAuxiliaire02;
            //M32[19] = this.EtatBtnAuxiliaire03;
            //M32[20] = this.EtatToggle01 || !_testToggle01;
            //M32[21] = this.EtatToggle02 || !_testToggle02;
            //M32[22] = this.EtatToggle03 || !_testToggle03;
            //M32[23] = this.EtatToggle04 || !_testToggle04;
            //M32[24] = this.EtatToggle05 || !_testToggle05;
            //M32[25] = this.EtatToggle06 || !_testToggle06;
            //M32[26] = this.EtatToggle07 || !_testToggle07;
            //M32[27] = this.EtatToggle08 || !_testToggle08;
            //M32[28] = this.EtatToggle09 || !_testToggle09;
            //M32[29] = this.EtatToggle10 || !_testToggle10;
            //M32[30] = this.EtatToggle11 || !_testToggle11;
            //M32[31] = this.EtatToggle12 || !_testToggle12;
            UInt32 calc = Mask32 >> result;

            if((calc & 0x01)!=0)
            {
                if (bp.Substring(0, 1).Equals("F") ||
                          bp.Substring(0, 1).Equals("M") )
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                    return 2;
            }
        }
    public XElement InitNewAlarm(XElement parent ,String Langue)
        {
            XElement Alarme;
            int i = 0;
            foreach (var alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
            {
                // Auto-acquitement
                Alarme = new XElement("ALARME" + i.ToString());
                XElement Item;
                if (alarme.AutoAcquit)
                {
                    Item = new XElement("TEMPO", alarme.Tempo.ToString());
                }
                else
                {
                    Item = new XElement("TEMPO", "");
                }
                Alarme.Add(Item);

                // Nom de l'alarme
                int nbligne;
                XElement Text = this.Transform2Pargraphe(alarme.Fonction, out nbligne);

                Item = new XElement("NAME");
                Item.Add(Text);
                Alarme.Add(Item);
                // libellés 

                Item = new XElement("PICTO", BuildNewAlarmLabel(alarme));
                Alarme.Add(Item);
                // Vibrator
                Item = new XElement("VIBRER", alarme.Vibrer);
                Alarme.Add(Item);
                // Event
                Item = new XElement("EVENT", alarme.Mnemologique.ToString());
                Alarme.Add(Item);

                // RC1 SBC

                Item = new XElement("RELAIS1SBC", alarme.CmdRelai1SBC);
                Alarme.Add(Item);
                // RC2 SBC
                Item = new XElement("RELAIS2SBC", alarme.CmdRelai2SBC);
                Alarme.Add(Item);
                // Buzzer SBC
                Item = new XElement("CMDBUZ", alarme.CmdBuzSBC);
                Alarme.Add(Item);
                // <CONDITION>
                Item = new XElement("CONDITION", LanguageSupport.Get().GetTextRapport("/XMLTranslation/ALARME" + i.ToString()));
                Alarme.Add(Item);
                
                i++;
                parent.Add(Alarme);
            }
            return parent;
        }

        public XElement initNewNetwork(XElement parent, string Langue)
        {

           
            // ajout des information reseaux 
            XElement reseaux = new XElement("RESEAUX");
            String modelresult = "Non";
            // recherche du reseaux actif
            // modbus canopen ou module nemo
            VariableEditable VE = GestionVariableSimple.Instance.GetVariableByName("ModBus");
            String resultModbus = "";
            String resultCanopen = "";
            String typebus = "";
            TypeBusEasyConfig BusAAfficher = TypeBusEasyConfig.AUCUN;
            if (VE != null)
            {
                if (!String.IsNullOrEmpty(VE.RefElementValue))
                {
                    if (Convert.ToInt32(VE.RefElementValue) > 0)
                    {
                        resultModbus = LanguageSupport.Get().GetText("/XML_CONFIG/OUI", Langue);
                        modelresult = "VALIDE";
                        if (BusAAfficher.Equals(TypeBusEasyConfig.AUCUN))
                        {
                            BusAAfficher = TypeBusEasyConfig.MODBUS;
                        }
                    }
                }
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("CanOpen");
            if (VE != null)
            {
                if (!String.IsNullOrEmpty(VE.RefElementValue))
                {
                    if (Convert.ToInt32(VE.RefElementValue) > 0)
                    {
                        resultCanopen = LanguageSupport.Get().GetText("/XML_CONFIG/OUI", Langue);
                        modelresult = "VALIDE";
                        if (BusAAfficher.Equals(TypeBusEasyConfig.AUCUN))
                        {
                            BusAAfficher = TypeBusEasyConfig.CANOPEN;
                        }
                    }
                }
            }
            VE = GestionVariableSimple.Instance.GetVariableByName("CRProfibus");
            if (VE != null)
            {
                if (!String.IsNullOrEmpty(VE.RefElementValue) )
                {
                    if (Convert.ToInt32(VE.RefElementValue) > 0)
                    {
                        ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_CRProfibus", Langue));
                        typebus = LCM.GetLabel(VE.RefElementValue);
                        modelresult = "VALIDE";
                        if (BusAAfficher.Equals(TypeBusEasyConfig.AUCUN))
                        {
                            switch (VE.RefElementValue)
                            {
                                case "2":
                                    BusAAfficher = TypeBusEasyConfig.PROFIBUSV1;
                                    break;
                                case "4":
                                    BusAAfficher = TypeBusEasyConfig.DEVICENET;
                                    break;
                                case "8":
                                    BusAAfficher = TypeBusEasyConfig.ETHERCAT;
                                    break;
                                case "9":
                                    BusAAfficher = TypeBusEasyConfig.PROFINET;
                                    break;
                                case "13":
                                    BusAAfficher = TypeBusEasyConfig.ETHRNETIP;
                                    break;
                                case "15":
                                    BusAAfficher = TypeBusEasyConfig.POWERLINK;
                                    break;
                                case "16":
                                    BusAAfficher = TypeBusEasyConfig.MODBUS;
                                    break;
                                default:
                                    BusAAfficher = TypeBusEasyConfig.AUCUN;
                                    break;
                            }
                        }
                    }
                }
            }

            // activation affichage reseaux
            XAttribute model = new XAttribute("MODEL", modelresult);
            reseaux.Add(model);

            if (!String.IsNullOrEmpty(resultModbus))
            {

                XElement  Result = new XElement("params");
                XAttribute item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                Result.Add(item);
                item = new XAttribute("type", "Type5");
                Result.Add(item);
                reseaux.Add(Result);

                 Result = new XElement("params");
                item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TITRE_MODBUS"));
                Result.Add(item);
                item = new XAttribute("type", "Type0");
                Result.Add(item);
                reseaux.Add(Result);

                Result = new XElement("params", resultModbus);
                item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ACTIVE_MODBUS"));
                Result.Add(item);
                item = new XAttribute("type", "Type2");
                Result.Add(item);
                item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                Result.Add(item);
                reseaux.Add(Result);
                VE = GestionVariableSimple.Instance.GetVariableByName("MB_Vitesse");
                if (VE != null)
                {
                    Result = new XElement("params", VE.AutorizedValues.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/VITESSE_MODBUS"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }


                VE = GestionVariableSimple.Instance.GetVariableByName("MB_Parite");
                if (VE != null)
                {
                    Result = new XElement("params", VE.AutorizedValues.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/PARITE_MODBUS"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
                VE = GestionVariableSimple.Instance.GetVariableByName("MB_adresse");
                if (VE != null)
                {
                    Result = new XElement("params", VE.RefElementValue.ToString());
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ADRESSE_MODBUS"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
                VE = GestionVariableSimple.Instance.GetVariableByName("OffsetModbus");
                if (VE != null)
                {
                    Result = new XElement("params", VE.RefElementValue.ToString());
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ADRESSE_MODBUS_OFFSET"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
                VE = GestionVariableSimple.Instance.GetVariableByName("SecuSortieModbus");
                ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_SecuSortieModbus", Langue));
                if (VE != null)
                {
                    Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTranslation/Com_SecuSortieModbus"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
            }
            if (!String.IsNullOrEmpty(resultCanopen))
            {
                XElement Result = new XElement("params");
                XAttribute item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/TITRE_CANOPEN"));
                Result.Add(item);
                item = new XAttribute("type", "Type0");
                Result.Add(item);
                reseaux.Add(Result);

                Result = new XElement("params", resultCanopen);
                item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ACTIVE_CANOPEN"));
                Result.Add(item);
                item = new XAttribute("type", "Type2");
                Result.Add(item);
                item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                Result.Add(item);
                reseaux.Add(Result);
                VE = GestionVariableSimple.Instance.GetVariableByName("COVitesse");
                if (VE != null)
                {
                    Result = new XElement("params", VE.AutorizedValues.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/VITESSE_CANOPEN"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
                VE = GestionVariableSimple.Instance.GetVariableByName("COAdresse");
                if (VE != null)
                {
                    Result = new XElement("params",VE.RefElementValue.ToString());
                    item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/ADRESSE_CANOPEN"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
                VE = GestionVariableSimple.Instance.GetVariableByName("SecuSortieModbus");
                ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_SecuSortieModbus", Langue));
                if (VE != null)
                {
                    Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTranslation/Com_SecuSortieModbus"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
            }
            if (!string.IsNullOrEmpty(typebus))
            {
                XElement Result = new XElement("params", typebus);
                XAttribute item = new XAttribute("item", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/MODULE_NEMO"));
                Result.Add(item);
                item = new XAttribute("type", "Type2");
                Result.Add(item);
                item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                Result.Add(item);
                reseaux.Add(Result);
                VE = GestionVariableSimple.Instance.GetVariableByName("SecuSortieModbus");
                ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_SecuSortieModbus", Langue));
                if (VE != null)
                {
                    Result = new XElement("params", LCM.GetLabel(VE.RefElementValue));
                    item = new XAttribute("item", LanguageSupport.Get().GetToolTipRapport("/XMLTranslation/Com_SecuSortieModbus"));
                    Result.Add(item);
                    item = new XAttribute("type", "Type2");
                    Result.Add(item);
                    item = new XAttribute("choix", LanguageSupport.Get().GetTextRapport("/XML_CONFIG/SI_AUTRE"));
                    Result.Add(item);
                    reseaux.Add(Result);
                }
            }
            parent.Add(reseaux);
            //switch (BusAAfficher)
            //{
            //    case TypeBusEasyConfig.MODBUS:
            //        return AdrMODBus;
            //        break;
            //    case TypeBusEasyConfig.CANOPEN:
            //        return AdrCANRPDO + "\n" + AdrCANSDO;
            //        break;
            //    case TypeBusEasyConfig.ETHERCAT:
            //    case TypeBusEasyConfig.POWERLINK:
            //        return AdrEthercat;
            //        break;
            //    case TypeBusEasyConfig.PROFIBUSV1:
            //    case TypeBusEasyConfig.PROFINET:
            //        return AdrProfibus;
            //        break;
            //    default:
            //        return "Not Defined";
            //        break;
            //}
            EasyConfigData.Get().InitModes();
            EasyConfigData.Get().VarNetworks.Clear();
            EasyConfigData.Get().InitNetwork();
            EasyConfigData.Get().VarNetworks.Clear();
            EasyConfigData.Get().InitNetwork();
            EasyConfigData.Get().InitDataSystem();
            EasyConfigData.Get().InitBoolInput();
            EasyConfigData.Get().InitBoolOutput();
            EasyConfigData.Get().InitNumericalInput();
            EasyConfigData.Get().InitNumericalOutput();
            EasyConfigData.Get().Load();
            XElement reseaux1 = new XElement("RESEAUX1");

            foreach (var variable in EasyConfigData.Get().VarDataSys)
            {
                reseaux1.Add(MiseEnFormeFixe(variable,BusAAfficher));
            }
             parent.Add(reseaux1);
             XElement reseaux2 = new XElement("RESEAUX2");
             foreach (var variable in EasyConfigData.Get().VarONum)
                {
                    reseaux2.Add(MiseEnFormeOutput(variable,BusAAfficher));
                }
            parent.Add(reseaux2);


            XElement reseaux4 = new XElement("RESEAUX4");
            // Données numériques en entrée
            foreach (var variable in EasyConfigData.Get().VarINum)
                {
                    reseaux4.Add(MiseEnFormeInput(variable,BusAAfficher));
                }
            parent.Add(reseaux4);

            String titre_colonne1 = "";
            String titre_colonne2 = "";

            if (!String.IsNullOrEmpty(resultCanopen))
            {
                titre_colonne1 = "@rel TPDO SDO";
                titre_colonne2 = "@abs TPDO ";
            }
            else
            {
                titre_colonne1 = "@rel ";
                titre_colonne2 = "@abs ";
            }

            XElement reseaux3 = new XElement("RESEAUX3");
            // Données logiques en sortie
            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", "");
            result.Add(colonne);
            colonne = new XAttribute("colonne2", titre_colonne1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", titre_colonne2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", "");
            result.Add(colonne);
            reseaux3.Add(result);
            foreach (var variable in EasyConfigData.Get().VarOBool)
            {
                reseaux3.Add(MiseEnFormeOutputBool(variable, BusAAfficher));
            }
            parent.Add(reseaux3);


            XElement reseaux5 = new XElement("RESEAUX5");
            result = new XElement("LIGNE");
            colonne = new XAttribute("colonne1", "");
            result.Add(colonne);
            colonne = new XAttribute("colonne2", titre_colonne1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", titre_colonne2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", "");
            result.Add(colonne);
            reseaux5.Add(result);
            // Données logiques en entrée
            foreach (var variable in EasyConfigData.Get().VarIBool)
                {
                    reseaux5.Add(MiseEnFormeInputBool(variable,BusAAfficher));
                }
            parent.Add(reseaux5);
            return parent;
        }
        public XElement initNewPlastron(XElement parent, string Langue)
        {
            // construire l'image du plastron avec la superposition de plusieurs images
            ObservableCollection<OrganCommand> list = new ObservableCollection<OrganCommand>();
            bool zn1 = true;
            bool zn2 = true;
            bool zn3 = true;

            foreach (var org in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                if (org.ZoneOrganeMO == "ZN1")
                {
                    zn1 = false;
                }
                if (org.ZoneOrganeMO == "ZN2")
                {
                    zn2 = false;
                }
                if (org.ZoneOrganeMO == "ZN3")
                {
                    zn3 = false;
                }
            }

            foreach (OrganCommand organ in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                if ((organ.ReferenceOrgan != null) && (organ.ReferenceOrgan != "") && (organ.ReferenceOrgan != "Descrip_NomOrganeMo"))
                {
                    list.Add(organ);
                }
            }
            ObservableCollection<imagePlastron> result = DicoImage(list, zn1 ,zn2,zn3);

            String UriText = AppDomain.CurrentDomain.BaseDirectory + @"Resources\";
            Drwg.Bitmap Base;
            int offsetImage = 0;
            
            if (PegaseData.Instance.MOperateur.RefIndus.Substring(0, 1).Equals("P"))
            {
                if (PegaseData.Instance.RefErpMO.Contains("_X"))
                {
                    Base = new Drwg.Bitmap(UriText + "PIKA_X.png");
                }
                else
                {
                    Base = new Drwg.Bitmap(UriText + "PIKA.png");
                }
                offsetImage = 71;

            }
            else if (PegaseData.Instance.MOperateur.RefIndus.Substring(0, 1).Equals("M"))
            {
                if (PegaseData.Instance.RefErpMO.Contains("_X"))
                {
                    Base = new Drwg.Bitmap(UriText + "MOKA_X.png");
                }
                else
                {
                    Base = new Drwg.Bitmap(UriText + "MOKA.png");
                }
                offsetImage=82;


            }
            else
            {
                Base = null; // pas d'image
            }
            if (Base != null)
            {
                Int32 hauteur = Base.Height;
                Int32 largueur = Base.Width;
                float dpihoz = Base.HorizontalResolution; //pixel par pouce
                float dpiver = Base.VerticalResolution;
                foreach (imagePlastron img in result)
                {
                    string image = img.pos.Attribute(XMLCore.XML_ATTRIBUTE.REFERENCE).Value;
                    string valeurx = img.pos.Attribute(XMLCore.XML_ATTRIBUTE.VALEURX ).Value;
                    string valeury = img.pos.Attribute(XMLCore.XML_ATTRIBUTE.VALEURY).Value;
                    int orientation = img.organ.OrientationOrgane;

                    string nomimage = img.organ.ReferenceOrgan;
                    // on remplace * par _ pour pouvoir gerer le nom fichier
                    nomimage = nomimage.Replace('*', '_');
                    if (orientation != 0)
                    {
                        nomimage = "R" + nomimage;
                    }
                    // rajouter image à l'image de base
                    if (File.Exists(UriText+ nomimage + ".png"))
                    {
                        

                        
                        Drwg.Bitmap imageAdd = new System.Drawing.Bitmap(UriText + nomimage + ".png");
                        using (Drwg.Graphics g = Drwg.Graphics.FromImage(Base))
                        {
                            float dpihozimg = imageAdd.HorizontalResolution; //pixel par pouce
                            float dpiverimg = imageAdd.VerticalResolution;
                            Int32 Ximage = imageAdd.Height;
                            Int32 Yimage = imageAdd.Width;
                            Int32 posx = offsetImage + (Int32)((dpiverimg * float.Parse(valeurx, CultureInfo.InvariantCulture.NumberFormat) )/ (float)25.4) - (Yimage/2);
                            Int32 posy = offsetImage + (Int32)((dpihozimg * float.Parse(valeury, CultureInfo.InvariantCulture.NumberFormat)) / (float)25.4) - (Ximage / 2);
                            //if (orientation != 0)
                            //{
                            //    imageAdd = RotateImageByAngle(imageAdd, (float) orientation);
                            //}
                            g.DrawImage(imageAdd, posx, posy);
                        }
                        imageAdd.Dispose();
                    }

                }
                Base.Save(DefaultValues.Get().RapportFolderImages + "Plastron.png");
                Base.Dispose();
                // insertion de la balise imgplastron
                XElement Result = new XElement("IMGPLASTRON");
                Result.Value = DefaultValues.Get().RelativeRapportFolderImages + "Plastron.png";
                Result.Value = Result.Value.Replace('\\', '/');
                parent.Add(Result);
            }

            return parent;
        }
        private static Drwg.Bitmap RotateImageByAngle(Drwg.Bitmap oldBitmap, float angle)
        {
            var newBitmap = new Drwg.Bitmap(oldBitmap.Width, oldBitmap.Height);
            newBitmap.SetResolution(oldBitmap.HorizontalResolution, oldBitmap.VerticalResolution);
            var graphics =  Drwg.Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)oldBitmap.Width / 2, (float)oldBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)oldBitmap.Width / 2, -(float)oldBitmap.Height / 2);
           // graphics.Clip = new Drwg.Region(new Drwg.Rectangle(0, 0, oldBitmap.Width, oldBitmap.Height));
            graphics.DrawImage(oldBitmap, new Drwg.Point(0, 0));
           
            return newBitmap;
        }
        XElement MiseEnFormeFixe(EC_VarNetwork variable, TypeBusEasyConfig BusAAfficher)
        {
            string type = "";
            string adresse1 = "";
            string adresse2 = "";
            string valeur1 = "";

            type = variable.Colonne1R.ToString() +  variable.AdrReseau1R.ToString();
            adresse1 = variable.Colonne2R.ToString() + variable.AdrReseau2R.ToString();
            adresse2 = variable.Colonne3R.ToString() + variable.AdrReseau3R.ToString();
            valeur1 = variable.Adresse;
            valeur1 = valeur1.Replace("\n", "&#13");
            valeur1 = valeur1.Replace("'", "\\'");

            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", type);
            result.Add(colonne);
            colonne = new XAttribute("colonne2", adresse1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", adresse2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", valeur1);
            result.Add(colonne);

            return result;
        }


        XElement MiseEnFormeOutput(EC_VarNetwork variable, TypeBusEasyConfig BusAAfficher)
        {
            string type = "";
            string adresse1 = "";
            string adresse2 = "";
            string valeur1 = "";

            type = variable.Colonne1W.ToString() + variable.AdrReseau1W.ToString();
            adresse1 = variable.Colonne2W.ToString() + variable.AdrReseau2W.ToString();
            adresse2 = variable.Colonne3W.ToString() + variable.AdrReseau3W.ToString();
            if ((variable.CurrentVariable != null) && (variable.CurrentVariable.UserName != null))
            {
                valeur1 = variable.CurrentVariable.UserName;
            }
            valeur1 = valeur1.Replace("\n", "&#13");
            valeur1 = valeur1.Replace("'", "\\'");

            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", type);
            result.Add(colonne);
            colonne = new XAttribute("colonne2", adresse1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", adresse2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", valeur1);
            result.Add(colonne);

            return result;
        }
        XElement MiseEnFormeOutputBool(EC_VarNetwork variable, TypeBusEasyConfig BusAAfficher)
        {
            string type = "";
            string adresse1 = "";
            string adresse2 = "";
            string valeur1 = "";

            type = variable.Colonne1W.ToString() + variable.AdrReseau1W.ToString();
            adresse1 = variable.AdrReseau2W_bis.ToString();
            adresse2 = variable.AdrReseau3W_bis.ToString();
            if ((variable.CurrentVariable != null) && (variable.CurrentVariable.UserName != null))
            {
                valeur1 = variable.CurrentVariable.UserName;
            }
            valeur1 = valeur1.Replace("\n", "&#13");
            valeur1 = valeur1.Replace("'", "\\'");

            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", type);
            result.Add(colonne);
            colonne = new XAttribute("colonne2", adresse1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", adresse2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", valeur1);
            result.Add(colonne);

            return result;
        }
        XElement MiseEnFormeInput(EC_VarNetwork variable, TypeBusEasyConfig BusAAfficher)
        {
            string type = "";
            string adresse1 = "";
            string adresse2 = "";
            string valeur1 = "";

            type = variable.Colonne1W.ToString() + variable.AdrReseau1W.ToString();
            adresse1 = variable.Colonne2W.ToString() + variable.AdrReseau2W.ToString();
            adresse2 = variable.Colonne3W.ToString() + variable.AdrReseau3W.ToString();
            if ((variable.CurrentVariable != null) && (variable.CurrentVariable.UserName != null))
            {
                valeur1 = variable.CurrentVariable.UserName;
            }
            valeur1 = valeur1.Replace("\n", "&#13");
            valeur1 = valeur1.Replace("'", "\\'");

            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", type);
            result.Add(colonne);
            colonne = new XAttribute("colonne2", adresse1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", adresse2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", valeur1);
            result.Add(colonne);

            return result;
        }
        XElement MiseEnFormeInputBool(EC_VarNetwork variable, TypeBusEasyConfig BusAAfficher)
        {
            string type = "";
            string adresse1 = "";
            string adresse2 = "";
            string valeur1 = "";

            type = variable.Colonne1R.ToString() + variable.AdrReseau1R.ToString();
            adresse1 = variable.AdrReseau2R_bis.ToString();
            adresse2 = variable.AdrReseau3R_bis.ToString();
            if ((variable.CurrentVariable != null) && (variable.CurrentVariable.UserName != null))
            {
                valeur1 = variable.CurrentVariable.UserName;
            }
            valeur1 = valeur1.Replace("\n", "&#13");
            valeur1 = valeur1.Replace("'", "\\'");

            XElement result = new XElement("LIGNE");
            XAttribute colonne = new XAttribute("colonne1", type);
            result.Add(colonne);
            colonne = new XAttribute("colonne2", adresse1);
            result.Add(colonne);
            colonne = new XAttribute("colonne3", adresse2);
            result.Add(colonne);
            colonne = new XAttribute("colonne4", valeur1);
            result.Add(colonne);

            return result;
        }
        /// <summary>
        /// Construit le dictionnaire de l'intégralité des données à imprimer
        /// </summary>
        public void InitData ( String Langue )
        {
            this.LanguageName = Langue;

            this.Fields.Clear();
            // 1 - Données du header
            this.InitDataHeader();

            // 2 - Données des cadres MT / SIM / MO
            this.InitDataMTFrame();
            this.InitDataSIMFrame();
            this.InitDataMOFrame();

            // 3 - Nom des images
            this.InitImagesZones();

            // 4 - Gestion des alarmes
            this.InitDataAlarmes();

            // 5 - Gestion des données des modes
            this.InitDataModesExplTab();
            this.InitOrganesNav();
            this.InitModes();

            // 6 - Gestion des données d'entrées / sorties
            // this.InitCarteMode();

            // 7 - Commentaires
            this.InitCommentaire();

            // 8 - Table de description des boutons
            this.InitTableBoutons();
            this.BuildCableTable();

            // 9 - Modbus
            this.BuildNetWorkData();

            // 10 - Données venant des paramètres simples
            this.BuildParamSimpleData();

            // 11 - MO
            this.BuildMOScreens();

            // 12 - Tableau de données 'utilisation et fonctionnement'
            this.BuildApplicativeSolutionTable();
        } // endMethod: InitData

        /// <summary>
        /// Construire la liste de bouton et les renseignements associés
        /// </summary>
        public void InitTableBoutons ()
        {
            XElement CollectionBtn1 = new XElement("div");
            XElement CollectionBtn2 = new XElement("div");
            Int32 compteur = 0;

            foreach (var organes in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                if (!organes.MnemoHardFamilleMO.Contains("AXE"))
                {
                    if ((organes.MnemoHardFamilleMO == "CO" && !organes.ReferenceOrgan.Contains("VIDE") && !organes.ReferenceOrgan.Contains("Descrip_")) || organes.MnemoHardFamilleMO != "CO")
                    {
                        XElement paragraphe = new XElement("p");
                        XAttribute attrib = new XAttribute("class", "MsoNormal1");
                        paragraphe.Add(attrib);

                        String ParaValue;
                        String PosOrgane;

                        if (organes.MnemoHardFamilleMO.Contains("AC"))
                        {
                            Int32 posO;
                            if (organes.NbPosOrgane < 15)
                            {
                                if (organes.NbPosOrgane > 6)
                                {
                                    posO = (organes.NbPosOrgane - 4) / 2;
                                    PosOrgane = String.Format(" +{0}/-{0}", posO.ToString()); 
                                }
                                else
                                {
                                    PosOrgane = "Lock";
                                }
                            }
                            else
                            {
                                PosOrgane = "No switch. Seq.";
                            }
                        }
                        else
                        {
                            PosOrgane = organes.NbPosOrgane.ToString();
                        }

                        if (organes.NomOrganeMO.Contains("N") || organes.NomOrganeMO.Contains("F") || organes.NomOrganeMO.Contains("M") || organes.MnemoHardFamilleMO == "AC")
                        {
                            ParaValue = String.Format("{0}-{1} states", organes.NomOrganeMO, PosOrgane, organes.OrientationOrgane);
                        }
                        else
                        {
                            ParaValue = String.Format("{0}-{1} states-orientation {2} deg", organes.NomOrganeMO, PosOrgane, organes.OrientationOrgane);
                        }

                        paragraphe.Value = ParaValue;
                        if (compteur < 12)
                        {
                            CollectionBtn1.Add(paragraphe);
                        }
                        else
                        {
                            CollectionBtn2.Add(paragraphe);
                        }
                        compteur++; 
                    }
                }
            }

            this.Fields.Add(TABLE_BOUTON1, CollectionBtn1);
            this.Fields.Add(TABLE_BOUTON2, CollectionBtn2);
        } // endMethod: InitTableBoutons

        /// <summary>
        /// Ajouter les données contenant les images et le commentaire
        /// </summary>
        private void InitImagesZones ( )
        {
            this.Fields.Add("COMMENT", new XText(PegaseData.Instance.IdentificationPack.Commentaire));
        } // endMethod: InitImagesZones
        
        /// <summary>
        /// Initialise les données des modes
        /// </summary>
        private void InitModes ( )
        {
            String Key, Value;
            // nom de l'équipement
            this.Fields.Add("EQUIPEMENT", new XText(PegaseData.Instance.ParamHorsMode.LibelleEquipement.Label));

            // Pour chacun des modes
            for (int i = 0; i < PegaseData.Instance.OLogiciels.NbModes; i++)
            {
                // Nom du mode
                Key = String.Format("NOM_M{0:00}", i+1);
                if (PegaseData.Instance.OLogiciels.ModesExploitation[i].ModeLabel != null)
                {
                    this.Fields.Add(Key, new XText(PegaseData.Instance.OLogiciels.ModesExploitation[i].ModeLabel.LibelSelecteur));
                }

                // Inter-verrouillage
                this.InitVerrouillage(i);

                // Retour d'informations par Mode
                this.InitRetourInfoMode(i);

                // Commentaire par mode
                Key = String.Format("Comment_M{0:00}", i + 1);
                Value = PegaseData.Instance.OLogiciels.ModesExploitation[i].Commentaire;

                this.Fields.Add(Key, new XText(Value));
            }
        } // endMethod: InitModes
        
        /// <summary>
        /// Initialise l'interverrouillage pour le mode
        /// </summary>
        private void InitVerrouillage ( Int32 NumMode )
        {
            
        } // endMethod: InitVerrouillage
        
        /// <summary>
        /// Initialise le retour d'information par mode
        /// </summary>
        private void InitRetourInfoMode ( Int32 NumMode )
        {
            Int32 NumRI = 1;
            String Key, Value;

            foreach (var ri in PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos)
            {
                if (ri != null)
                {
                    // N° de retour
                    Key = String.Format("NRetour{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = NumRI.ToString();

                    this.Fields.Add(Key, new XText(Value));

                    // Désignation
                    Key = String.Format("Design{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = ri.Information.Designation;

                    this.Fields.Add(Key, new XText(Value));

                    // Evènement
                    Key = String.Format("Event{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = ri.Information.TypeVariable.ToString();

                    this.Fields.Add(Key, new XText(Value));

                    // Echelle
                    Key = String.Format("Scale{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = String.Format("{0:0.0000}", ri.Information.Scaling_a);

                    this.Fields.Add(Key, new XText(Value));

                    // Unité
                    Key = String.Format("U{0:00}_M{1:00}", NumRI, NumMode + 1);
                    if (ri.InfoLibelUnit != null)
                    {
                        Value = ri.InfoLibelUnit.LibelInformation;
                    }
                    else
                    {
                        Value = "";
                    }

                    this.Fields.Add(Key, new XText(Value));

                    // Valeur min / Max
                    Key = String.Format("ValmM{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = String.Format("{0:0.000} / {1:0.000}", ri.ValMin, ri.ValMax);

                    this.Fields.Add(Key, new XText(Value));

                    // Nb décimal
                    Key = String.Format("NbD{0:00}_M{1:00}", NumRI, NumMode + 1);
                    Value = ri.NbDecimales.ToString();

                    this.Fields.Add(Key, new XText(Value));

                    // Libellés si valeurs min / Max sont atteintes
                    Key = String.Format("Lib{0:00}_M{1:00}", NumRI, NumMode + 1);
                    String LMin, LMax;
                    if (ri.InfoLibelMin != null)
                    {
                        LMin = ri.InfoLibelMin.LibelInformation;
                    }
                    else
                    {
                        LMin = "";
                    }
                    if (ri.InfoLibelMax != null)
                    {
                        LMax = ri.InfoLibelMax.LibelInformation;
                    }
                    else
                    {
                        LMax = "";
                    }

                    Value = String.Format("{0} / {1}", LMin, LMax);

                    this.Fields.Add(Key, new XText(Value)); 
                }

                NumRI++;
            }
        } // endMethod: InitRetourInfoMode
        
        /// <summary>
        /// Initialiser les données de cablage pour une carte 
        /// </summary>
        public Int32 InitCarteMode ( Carte carte, out ObservableCollection<CablageRow> cablageRow )
        {
            Int32 Result = 0;

            cablageRow = new ObservableCollection<CablageRow>();
            CablageRow CR;
            // Nom des entrées / sorties
            foreach (var IO in PegaseData.Instance.ModuleT.EAnas)
            {
                CR = this.InitES(Result, IO, carte.ID);
                if (CR != null)
                {
                    if (!(PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && carte.Nom == "Mother board"))
                    {
                        cablageRow.Add(CR);
                    }
                }
                Result++;
            }

            foreach (var IO in PegaseData.Instance.ModuleT.SAnas)
            {
                CR = this.InitES(Result, IO, carte.ID);
                if (CR != null)
                {
                    if (!(PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && carte.Nom == "Mother board"))
                    {
                        cablageRow.Add(CR);
                    }
                }
                Result++;
            }

            foreach (var IO in PegaseData.Instance.ModuleT.ETORS)
            {
                CR = this.InitES(Result, IO, carte.ID);
                if (CR != null)
                {
                    if (!(PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && carte.Nom == "Mother board"))
                    {
                        cablageRow.Add(CR);
                    }
                }
                Result++;
            }

            foreach (var IO in PegaseData.Instance.ModuleT.STORS)
            {
                CR = this.InitES(Result, IO, carte.ID);
                if (CR != null)
                {
                    if (!(PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && carte.Nom != "Mother board"))
                    {
                        cablageRow.Add(CR);
                    }
                }
                Result++;
            }

            foreach (var IO in PegaseData.Instance.ModuleT.ListESDivers)
            {
                CR = this.InitES(Result, IO, carte.ID);
                if (CR != null)
                {
                    if (!(PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && carte.Nom == "Mother board"))
                    {
                        cablageRow.Add(CR);
                    }
                }
                Result++;
            }

            return Result;
        } // endMethod: InitCarteMode

        /// <summary>
        /// Initialiser l'ensemble des ES pour le mode spécifié
        /// </summary>
        public CablageRow InitES ( Int32 NumES, ES IO, Int32 idcarte )
        {
            CablageRow Result = null;

            // ES
            if (idcarte == IO.IDCarte || (PegaseData.Instance.ModuleT.TypeMT == MT.ELIO && idcarte - 1 == IO.IDCarte))
            {
                Result = new CablageRow(IO.MnemoHardware, IO.MnemoBornier, IO.MnemoClient);
            }

            return Result;
        } // endMethod: InitES

        /// <summary>
        /// Construire la clé pour le RI Screen display
        /// </summary>
        private String BuildRISdKey ( Int32 NumMode, Int32 NumRI )
        {
            String Result = String.Format("RI{0:00}_SD_M{1:00}", NumRI + 1, NumMode + 1);
            
            return Result;
        } // endMethod: BuildRISdKey
        
        /// <summary>
        /// Construire la valeur RI pour le Screen display
        /// </summary>
        private String BuildRISdValue ( Int32 NumMode, Int32 NumRI )
        {
            String Result = this.BuildRIValue(NumMode, NumRI);
            
            return Result;
        } // endMethod: BuildRISdValue
        
        /// <summary>
        /// Construire la valeur RI Device Identification
        /// </summary>
        private String BuildRIDiValue ( Int32 NumMode, Int32 NumRI )
        {
            String Result = SCREEN_VIDE;

            if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI] != null)
            {
                if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].Information != null)
                {
                    Result = PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].Information.Designation; 
                }
            }

            return Result;
        } // endMethod: BuildRIDiValue

        /// <summary>
        /// Construire la clé de données pour un retour d'info 
        /// </summary>
        private String BuildRIKey(Int32 NumMode, Int32 NumRI)
        {
            String Result = String.Format("RI{0:00}_M{1:00}", NumRI + 1, NumMode + 1);

            return Result;
        } // endMethod: BuildRIKey
        
        /// <summary>
        /// Construire la valeur d'un retour d'information
        /// </summary>
        private String BuildRIValue ( Int32 NumMode, Int32 NumRI )
        {
            String Result = SCREEN_VIDE;

            if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI] != null)
            {
                switch (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].Information.TypeVariable)
                {
                    case RIType.RINNONDEFINI:
                        Result = SCREEN_INDETERMINE;
                        break;
                    case RIType.RIBOOL:
                        Result = PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].InfoLibelRetour.LibelInformation + " " +
                                PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].InfoLibelMax.LibelInformation;
                        break;
                    case RIType.RIANA:
                        Result = PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].InfoLibelRetour.LibelInformation + " " +
                                PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].ValueDemo.ToString() + " " +
                                PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].InfoLibelUnit.LibelInformation;
                        break;
                    case RIType.RIVARNUM:
                        Result = PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].InfoLibelRetour.LibelInformation + " " +
                                PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsInfoByPos[NumRI].LibelsVarNumerique[0].LibelInformation;
                        break;
                    default:
                        Result = SCREEN_VIDE;
                        break;
                }
            }

            return Result;
        } // endMethod: BuildRIValue

        /// <summary>
        /// Construire la clé de données pour un retour d'info 
        /// </summary>
        private String BuildRI_DIKey ( Int32 NumMode, Int32 NumRI )
        {
            String Result = String.Format("RI{0:00}_DI_M{1:00}", NumRI + 1, NumMode + 1);

            return Result;
        } // endMethod: BuildRI_DIKey

        /// <summary>
        /// Construire la clé de données pour un retour d'info 
        /// </summary>
        private String BuildRI_SDKey(Int32 NumMode, Int32 NumRI)
        {
            String Result = String.Format("RI{0:00}_SD_M{1:00}", NumRI + 1, NumMode + 1);

            return Result;
        } // endMethod: BuildRI_SDKey

        /// <summary>
        /// Construire la clé de données pour la légende d'un sélecteur 
        /// </summary>
        private String BuildSelecteur_LKey(Int32 NumMode, Int32 NumS)
        {
            String Result = String.Format("S{0:00}_L_M{1:00}", NumS + 1, NumMode + 1);

            return Result;
        } // endMethod: BuildSeleceur_LKey

        /// <summary>
        /// Construire la valeur d'un sélecteur
        /// </summary>
        private String BuildSelecteur_LValue ( Int32 NumMode, Int32 NumS )
        {
            String Result = VIDE;

            if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS] != null)
            {
                if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS].Count > 0)
                {
                    Result = "";

                    if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS] != null)
                    {
                        if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].NbPosition > 1)
                        {
                            if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].OrganeARecopier != "")
                            {
                                Result = this.BuildOrganeCommande(PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].OrganeARecopier, PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS]);
                            }
                            else 
                            {
                                if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].BtIncrementer != "")
                                {
                                    Result = this.BuildOrganeCommande(PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].BtIncrementer, PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS]);
                                }
                                if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].BtDecrementer != "")
                                {
                                    Result += " / " + this.BuildOrganeCommande(PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS].BtDecrementer, PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].Selecteurs[NumS]);
                                }
                            }
                            Result += "-> ";
                        }
                    }

                    for (int i = 0; i < PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS].Count; i++)
                    {
                        Result += PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS][i].LibelSelecteur + " / "; 
                    }
                    if (Result.Length > 3)
                    {
                        Result = Result.Substring(0, Result.Length - 3);
                    }
                }
            }

            return Result;
        } // endMethod: BuildSelecteur_LValue

        /// <summary>
        /// Le nom de l'organe de commande, le type...
        /// </summary>
        private String BuildOrganeCommande ( String NomOrgane, Selecteur selecteur )
        {
            String Result = "";
            var Query = from organe in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organe.NomOrganeMO == NomOrgane
                        select organe;

            if (Query.Count() > 0)
            {
                OrganCommand Organe = Query.First();
                String Mode = "";
                if (selecteur.OrganeARecopier != "")
                {
                    if (selecteur.NbPosition == 2)
                    {
                        Mode = "Off/On";
                    }
                    else if(selecteur.NbPosition == 3)
                    {
                        Mode = "Off/Sv/Dv";
                    }
                }
                if (Mode != "")
                {
                    Result = String.Format("{0}({1}) {2} ", Organe.NomOrganeMO, Organe.MnemoHardFamilleMO, Mode); 
                }
                else
                {
                    Result = String.Format("{0}({1})", Organe.NomOrganeMO, Organe.MnemoHardFamilleMO);
                }

                Result = Result.Trim() + " ";
            }
            
            return Result;
        } // endMethod: BuildOrganeCommande

        /// <summary>
        /// Construire la clé de données pour un retour d'info 
        /// </summary>
        private String BuildSelecteurKey(Int32 NumMode, Int32 NumS)
        {
            String Result = String.Format("S{0:00}_M{1:00}", NumS + 1, NumMode + 1);

            return Result;
        } // endMethod: BuildSelecteurKey

        /// <summary>
        /// Construire la valeur d'un sélecteur
        /// </summary>
        private String BuildSelecteurValue(Int32 NumMode, Int32 NumS)
        {
            String Result = SCREEN_VIDE;

            if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS] != null)
            {
                if (PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS].Count > 0)
                {
                    Result = PegaseData.Instance.OLogiciels.ModesExploitation[NumMode].LabelsSelecteurByPos[NumS][0].LibelSelecteur;
                }
            }
            
            return Result;
        } // endMethod: BuildSelecteurValue

        /// <summary>
        /// Initialiser les organes de navigation
        /// </summary>
        private void InitOrganesNav ( )
        {
            this.Fields.Add("MODE_INC", new XText(PegaseData.Instance.ParamHorsMode.NavigationInc));
            this.Fields.Add("MODE_DEC", new XText(PegaseData.Instance.ParamHorsMode.NavigationDec));
        } // endMethod: InitOrganesNav


        /// <summary>
        /// Tableau décrivant l'ensemble des modes d'exploitation
        /// </summary>
        private void InitDataModesExplTab ( )
        {
            Int32 i, j;
            String Key;

            // Balayer tous les modes existants
            for (i = 0; i < PegaseData.Instance.OLogiciels.NbModes; i++)
            {
                Key = this.BuildKeyTabModes(i + 1);
                if (PegaseData.Instance.OLogiciels.ModesExploitation[i].ModeLabel != null)
                {
                    this.Fields.Add(Key, new XText(PegaseData.Instance.OLogiciels.ModesExploitation[i].ModeLabel.LibelSelecteur));
                }
                else
                {
                    this.Fields.Add(Key, new XText(""));
                }
            }

            // Compléter les modes avec des modes vides
            for (j = i; j < NB_MODE_MAX; j++)
            {
                Key = this.BuildKeyTabModes(j + 1);
                this.Fields.Add(Key, new XText(""));
            }
        } // endMethod: InitDataModesExplTab
        
        /// <summary>
        /// Construire la clé des données pour le tableau des modes
        /// </summary>
        private String BuildKeyTabModes ( Int32 NumMode )
        {
            String Result;

            Result = String.Format("NOM_MODE_{0:00}", NumMode);
            
            return Result;
        } // endMethod: BuildKeyTabModes

        /// <summary>
        /// Initaliser les données de l'alarme
        /// </summary>
        private void InitDataAlarmes( )
        {
            Int32 i = 1;
            foreach (var alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
            {
                String Key, Value;

                // Fonction
                Key = String.Format("ALARM_F{0:00}", i);
                this.Fields.Add(Key, new XText(alarme.Fonction));

                // Evènement
                Key = String.Format("ALARM_E{0:00}", i);
                this.Fields.Add(Key, new XText(alarme.Event.ToString()));

                // Vibreur
                Key = String.Format("ALARM_V{0:00}", i);
                if (alarme.Vibrer)
                {
                    Value = TRUE;
                }
                else
                {
                    Value = FALSE;
                }
                this.Fields.Add(Key, new XText(Value));

                // Acquittement auto
                Key = String.Format("ALARM_Aa{0:00}", i);
                if (alarme.AutoAcquit)
                {
                    Value = TRUE;
                }
                else
                {
                    Value = FALSE;
                }
                this.Fields.Add(Key, new XText(Value));

                // Acquittement temps
                Key = String.Format("ALARM_At{0:00}", i);
                this.Fields.Add(Key, new XText(alarme.Tempo.ToString()));

                // Description ou pictogramme
                Key = String.Format("ALARM_D{0:00}", i);
                this.Fields.Add(Key, new XText(alarme.LibelInformation));

                // Description ou pictogramme
                Key = String.Format("ALARM_T{0:00}", i);
                this.Fields.Add(Key, new XText(alarme.LibelTitre));

                i++;
            }

            // Données des alarmes pour le nouveau rapport
            this.BuildAlarmeData();
        } // endMethod: InitDataAlarmes

        /// <summary>
        /// Initialiser les données du header
        /// </summary>
        private void InitDataHeader( )
        {
          

            if (PegaseData.Instance.IdentificationPack.NumFiche != null)
            {

            }

            this.Fields.Add(NUMFICHE, new XText(PegaseData.Instance.IdentificationPack.NumFiche));
            this.Fields.Add("REF_PACK", new XText(PegaseData.Instance.IdentificationPack.PackSerialNumber));
            this.Fields.Add("DATE_PROG", new XText(PegaseData.Instance.IdentificationPack.PackDateProg));
        } // endMethod: InitDataHeader
        
        /// <summary>
        /// Initialiser les données du cadre 'MT'
        /// </summary>
        private void InitDataMTFrame( )
        {
            this.Fields.Add(MT_REFERENCE, new XText(PegaseData.Instance.ModuleT.RefIndus));
            this.Fields.Add(MT_NUMERO_SERIE, new XText(PegaseData.Instance.ModuleT.SerialNumber));
            this.Fields.Add(MT_FREQ, new XText(PegaseData.Instance.ModuleT.Frequence));

            // Unique ID
           // Int32 ID;
           // ID = XMLCore.Tools.ConvertASCIIToInt16(PegaseData.Instance.ModuleT.UniqueID);
            this.Fields.Add(MT_CODE_ID, new XText(PegaseData.Instance.ModuleT.UniqueID));

            this.Fields.Add("MT_TENSION", new XText(PegaseData.Instance.ModuleT.Tension));
            this.Fields.Add(MT_TYPE_CABLE, new XText(PegaseData.Instance.ModuleT.TypeCable));
            this.Fields.Add(MT_KLAXON, new XText(PegaseData.Instance.ModuleT.Klaxon));
            
            // Ref ERP MT
            this.Fields.Add(MT_CODE_CONFIGURATEUR, new XText(PegaseData.Instance.RefErpMT));
        } // endMethod: InitDataMTFrame
        
        /// <summary>
        /// Initialiser les données de la SIM
        /// </summary>
        private void InitDataSIMFrame( )
        {
            String Value;
            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Mode", this.LanguageName));
            Int32 IntValue;
            try
            {
                IntValue = Convert.ToInt32(PegaseData.Instance.OLogiciels.GestionIR);
                if (IntValue < 0)
                {
                    IntValue = 0;
                }
            }
            catch
            {
                IntValue = 0;
            }
            String TradValue;
            TradValue = LCM.GetLabel(IntValue.ToString());
            this.Fields.Add(SIM_OPTION_IR, new XText(TradValue));


            this.Fields.Add(SIM_REF, new XText(PegaseData.Instance.CarteSIM.RefIndus));
            this.Fields.Add(SIM_NUM_SERIE, new XText(PegaseData.Instance.CarteSIM.NumeroDeSerie));
            this.Fields.Add(SIM_CANAL, new XText(PegaseData.Instance.CarteSIM.CanalDemande));

            // Arrêt passif
            String SIMPassStop = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            Int32 Temps = PegaseData.ConvertString2Value(SIMPassStop);

            this.Fields.Add(SIM_ARRET_PASSIF, new XText(String.Format("{0}",Temps)));

            // Temps d'attente
            String IsOn = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            Int32 IIsOn = PegaseData.ConvertString2Value(IsOn);

            String SIMStandby;
            Int32 ISimStandby = 0;
            if (IIsOn != 0)
            {
                SIMStandby = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                ISimStandby = PegaseData.ConvertString2Value(SIMStandby) / 60;
                SIMStandby = String.Format("{0}", ISimStandby);
            }
            else
            {
                SIMStandby = VIDE;
            }

            this.Fields.Add(SIM_STANDBY, new XText(SIMStandby));

            // Mot de passe démarrage
            if (PegaseData.Instance.PassWordStart != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordStart);
                
            }
            else
            {
                Value = VIDE;
            }
            this.Fields.Add("PW_START", new XText(Value));

            // Mot de passe démarrage++
            if (PegaseData.Instance.PassWordStartPP != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordStartPP);
            }
            else
	        {
                Value = VIDE;
	        }
            this.Fields.Add("PW_STARTPP", new XText(Value));

            // Mot de passe association
            if (PegaseData.Instance.PassWordAssociation != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordAssociation);
            }
            else
	        {
                Value = VIDE;
	        }
            this.Fields.Add("PW_ASSOCIATION", new XText(Value));

            // Mot de passe configuration
            if (PegaseData.Instance.PassWordConfig != 0)
            {
                Value = String.Format("{0:0000}", PegaseData.Instance.PassWordConfig);
            }
            else
            {
                Value = VIDE;
            }
            this.Fields.Add("PW_CONFIG", new XText(Value));

            // Option Mode -> identifier la valeur
            var QueryOMode = from omode in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                             where omode.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionExploit.ToString() && omode.NameOption == "ModeExploitation"
                             select omode.Libelle;

            if (QueryOMode.Count() > 0)
            {
                this.Fields.Add(SIM_MODES_OPT, new XText(QueryOMode.First()));
            }
            else
            {
                this.Fields.Add(SIM_MODES_OPT, new XText("No Data"));
            }

            // Option Synchronisation -> identifier la valeur
            var QueryOSynchro = from osynchro in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                                where osynchro.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionCouplage.ToString() && osynchro.NameOption == "Couplage"
                                select osynchro.Libelle;

            if (QueryOSynchro.Count() > 0)
            {
                this.Fields.Add(SIM_SYNCHRO_OPT, new XText(QueryOSynchro.First()));
            }
            else
            {
                this.Fields.Add(SIM_SYNCHRO_OPT, new XText("No Data"));
            }

            // Option Homme Mort -> Identifier la valeur
            var QueryOHommeMort = from ohommemort in Helper.ResolvOptionsLogiciels.ListValeurOptionsLogiciel
                                  where ohommemort.CodeEmbarque == PegaseData.Instance.CarteSIM.OptionHommeMort.ToString() && ohommemort.NameOption == "Homme mort"
                                  select ohommemort.Libelle;

            if (QueryOHommeMort.Count() > 0)
            {
                this.Fields.Add(SIM_DEADMAN_OPT, new XText(QueryOHommeMort.First()));
            }
            else
            {
                this.Fields.Add(SIM_DEADMAN_OPT, new XText("No Data"));
            }

            // Ref ERP SIM
            this.Fields.Add(SIM_CODE_CONFIGURATEUR, new XText(PegaseData.Instance.RefErpSIM));
            // Ref ERP CHARGE
            this.Fields.Add(CHARGE_CODE_CONFIGURATEUR, new XText(PegaseData.Instance.RefErpCHARGE));
        } // endMethod: InitDataSIMFrame
        
        /// <summary>
        /// Initialiser les données du MO
        /// </summary>
        private void InitDataMOFrame( )
        {
            this.Fields.Add(MO_REF, new XText(PegaseData.Instance.MOperateur.RefIndus));
            this.Fields.Add(MO_NUM_SERIE, new XText(PegaseData.Instance.MOperateur.SerialNumber));
            this.Fields.Add(MO_FREQ, new XText(PegaseData.Instance.MOperateur.Frequence));

            // Unique ID
           // Int32 ID;
           // ID = XMLCore.Tools.ConvertASCIIToInt16(PegaseData.Instance.MOperateur.UniqueID);
            this.Fields.Add(MO_CODE_ID, new XText(PegaseData.Instance.MOperateur.UniqueID));

            // Acquérir les traductions présence / absence
            ListChoixManager LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/PRESENCE_ABSENCE", this.LanguageName));
            Int32 Value;
            String TradValue;

            // Présence auxiliaire
            Value = PegaseData.Instance.MOperateur.PresenceAuxiliaire;
            TradValue = LCM.GetLabel(Value.ToString());
            this.Fields.Add(MO_AUX, new XText(TradValue));

            // Présence IR
            Value = PegaseData.Instance.MOperateur.PresenceIR;
            TradValue = LCM.GetLabel(Value.ToString());
            this.Fields.Add(MO_IR_STUP, new XText(TradValue));

            // Présence vibreur
            Value = PegaseData.Instance.MOperateur.PresenceVibreur;
            TradValue = LCM.GetLabel(Value.ToString());
            this.Fields.Add(MO_VIBREUR, new XText(TradValue));

            // Présence acceleromètre
            Value = PegaseData.Instance.MOperateur.PresenceAccelerometre;
            TradValue = LCM.GetLabel(Value.ToString());
            this.Fields.Add(MO_ACCEL, new XText(TradValue));

            // Présence buzzer
            Value = PegaseData.Instance.MOperateur.PresenceBuzzer;
            TradValue = LCM.GetLabel(Value.ToString());
            this.Fields.Add(MO_BUZZER, new XText(TradValue));

            // Comportement sur support de charge
            Value = PegaseData.Instance.CurrentStartOnSBCMode;

            switch (Value)
            {
                case 0:
                    TradValue = LanguageSupport.Get().GetText("LIBEL_MO/COMBO_PAS_00");
                    break;
                case 1:
                    TradValue = LanguageSupport.Get().GetText("LIBEL_MO/COMBO_DEM_01");
                    break;
                case 2:
                    TradValue = LanguageSupport.Get().GetText("LIBEL_MO/COMBO_DEM_SECU_02");
                    break;
                default:
                    TradValue = LanguageSupport.Get().GetText("LIBEL_MO/COMBO_PAS_00");
                    break;
            }

            this.Fields.Add(MO_SBC_BEHAVIOR, new XText(TradValue));

            // CODE Configurateur
            this.Fields.Add(MO_CODE_CONFIGURATEUR, new XText(PegaseData.Instance.RefErpMO));
        } // endMethod: InitDataMOFrame
        
        /// <summary>
        /// Initialiser tous les champs liés aux commentaires
        /// </summary>
        private void InitCommentaire ( )
        {
            // Commentaire principale
            XElement Text;
            
           // Text = this.Transform2Pargraphe(PegaseData.Instance.Commentaire.DescriptifGeneral);
           // this.Fields.Add(COMMENTAIRE_PRINCIPALE, Text);
        } // endMethod: InitCommentaire

        /// <summary>
        /// Transformer une chaîne de caractères en paragraphe html. Les retours à la ligne sont utilisés pour délimiter les paragraphes "<p></p>"
        /// </summary>
        public XElement Transform2Pargraphe ( String Str, out int nbligne )
        {
            XElement Result = new XElement("div");
            nbligne = 0;
            XElement paragraphe = new XElement("pre");
            //  Char delimiter = '\n';
            //  Str = Str.Replace("'", "&#39");
            Str = Str.Replace("'", "\\'");
            Str = Str.Replace("\t", "    ");
            String[] substrings = Str.Split('\n');
            foreach(String ligne in substrings)
            {
                if (ligne.Length != 0)
                {
                    nbligne = nbligne + (ligne.Length / 150) + 1;
                }
            }


            Str = Str.Replace("\n", "&#13");
            //   String[] substrings = Str.Split(delimiter);
            //   foreach (var substring in substrings)
            // {
            //XElement par = new XElement("p");
            //par.Value = substring;
            //  XElement saut = new XElement("br");
            //   par.Add(saut);
            

            paragraphe.Value = Str; 

            //}
             

            //paragraphe.Value = Str;
            Result.Add(paragraphe);

            return Result;
        } // endMethod: Transform2Pargraphe
        
        /// <summary>
        /// Construire les données des alarmes
        /// </summary>
        public void BuildAlarmeData ( )
        {
            Int32 numBalise = 4001;
            String Balise;
            XNode Value;
            XElement conditions = new XElement("div");
            Int32 i = 1;

            foreach (var alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
            {
                // Auto-acquitement
                Balise = String.Format("$BALISE{0:0000}", numBalise);
                if (alarme.AutoAcquit)
                {
                    Value = new XText(alarme.Tempo.ToString() + " s");
                }
                else
                {
                    Value = new XText(" ");
                }
                
                this.Fields.Add(Balise, Value);

                // Nom de l'alarme
                Balise = String.Format("$BALISE{0:0000}", numBalise + 1);
                Value = new XText(alarme.Fonction);
                this.Fields.Add(Balise, Value);

                // libellés
                Balise = String.Format("$BALISE{0:0000}", numBalise + 2);
                String V = this.BuildInfoLabel(alarme.InformationL);
                if (alarme.InformationL.IsBitmap)
                {
                    Value = XElement.Parse(V);
                }
                else
                {
                    Value = new XText(V);
                }
                this.Fields.Add(Balise, Value);

                // Vibrator
                Balise = String.Format("$BALISE{0:0000}", numBalise + 3);
                if (alarme.Vibrer)
                {
                    Value = new XText("X"); 
                }
                else
                {
                    Value = new XText(" ");
                }
                this.Fields.Add(Balise, Value);

                // Event
                Balise = String.Format("$BALISE{0:0000}", numBalise + 4);
                Value = new XText(alarme.Mnemologique.ToString());
                this.Fields.Add(Balise, Value);

                // RC1 SBC
                Balise = String.Format("$BALISE{0:0000}", numBalise + 5);

                if (alarme.CmdRelai1SBC)
                {
                    Value = new XText("X");
                }
                else
                {
                    Value = new XText(" ");
                }

                this.Fields.Add(Balise, Value);

                // RC2 SBC
                Balise = String.Format("$BALISE{0:0000}", numBalise + 6);
                if (alarme.CmdRelai2SBC)
                {
                    Value = new XText("X");
                }
                else
                {
                    Value = new XText(" ");
                }

                this.Fields.Add(Balise, Value);

                // Buzzer SBC
                Balise = String.Format("$BALISE{0:0000}", numBalise + 7);
                if (alarme.CmdBuzSBC)
                {
                    Value = new XText("X");
                }
                else
                {
                    Value = new XText(" ");
                }

                this.Fields.Add(Balise, Value);

                // Commentaire alarme
                Balise = String.Format("$BALISE{0:0000}", numBalise + 8);
                if (alarme.CommentaireAlarme != null && alarme.CommentaireAlarme != "")
                {
                    String reference = String.Format("c{0:00}*", i);
                    Value = new XText(reference);
                    XElement paragraphe = new XElement("p");
                    paragraphe.Value = reference + " : " + alarme.CommentaireAlarme;
                    conditions.Add(paragraphe);
                }
                else
                {
                    Value = new XText(" ");
                }

                this.Fields.Add(Balise, Value);
                i++;

                numBalise += 10;
            }

            this.Fields.Add("$BALISE4200", conditions);
        } // endMethod: BuildAlarmeData

        /// <summary>
        /// Construire les données liées au réseau
        /// </summary>
        public void BuildNetWorkData ( )
        {
            Int32 NumBalise = 301;

            // Données numériques en sortie
            foreach (var item in EasyConfigData.Get().VarONum)
            {
                this.BuildNetworkVarValue(item, NumBalise++);
            }

            // Données numériques en entrée
            NumBalise = 320;
            foreach (var item in EasyConfigData.Get().VarINum)
            {
                this.BuildNetworkVarValue(item, NumBalise++);
            }

            // Données logiques en sortie
            NumBalise = 401;
            foreach (var item in EasyConfigData.Get().VarOBool)
            {
                this.BuildNetworkVarValue(item, NumBalise++);
            }

            // Données logiques en entrée
            NumBalise = 501;
            foreach (var item in EasyConfigData.Get().VarIBool)
            {
                this.BuildNetworkVarValue(item, NumBalise++);
            }
        } // endMethod: BuildNetWorkData

        /// <summary>
        /// Construire une variable XText contenant la valeur associée à une E/S modbus
        /// </summary>
        private void BuildNetworkVarValue ( EC_VarNetwork item, Int32 NumBalise )
        {
            XText Value;
            String Balise;

            Balise = String.Format("$BALISE{0:0000}", NumBalise++);
            if (item.CurrentVariable != null)
            {
                Value = new XText(item.CurrentVariable.UserName);
            }
            else
            {
                Value = new XText("-");
            }

            this.Fields.Add(Balise, Value);
        } // endMethod: BuildNetworkVarValue

        /// <summary>
        /// Construire la(les) table(s) des cablages
        /// </summary>
        public void BuildCableTable ( )
        {
            Int32 NbColonneMax = 5;
            XElement section;
            XElement tableCarte = null;
            Int32 i;

            section = new XElement("div");
            XAttribute attrib = new XAttribute("id", "cablage");
            section.Add(attrib);

            // 1- pour toutes les cartes
            foreach (var carte in PegaseData.Instance.ModuleT.Cartes)
            {
                // 2 - pour tous les modes
                //for (i = 0; i < PegaseData.Instance.OLogiciels.ModesExploitation.Count; i += NbColonneMax)
                for (i = 0; i < EasyConfigData.Get().Modes.Count; i += NbColonneMax)
                {
                    Int32 NbColonne = EasyConfigData.Get().Modes.Count - i;
                    XElement table;

                    if (NbColonne > NbColonneMax)
                    {
                        NbColonne = NbColonneMax;
                    }

                    tableCarte = this.BuildEmptyCableTable(carte, i, NbColonne, out table);

                    section.Add(tableCarte);

                    // 3 - pour toutes les entrées / sorties

                    // Eanas
                    IEnumerable<ES> esCarte = from es in PegaseData.Instance.ModuleT.EAnas
                                              where es.Carte == carte.ID
                                              select es;

                    if (esCarte.Count() > 0)
                    {
                        foreach (var es in esCarte)
                        {
                            table.Add(this.BuildCablageRow(es, i, NbColonne));
                        }
                    }

                    // ETors
                    esCarte = from es in PegaseData.Instance.ModuleT.ETORS
                              where es.Carte == carte.ID
                              select es;

                    if (esCarte.Count() > 0)
                    {
                        foreach (var es in esCarte)
                        {
                            table.Add(this.BuildCablageRow(es, i, NbColonne));
                        }
                    }

                    // SAnas
                    esCarte = from es in PegaseData.Instance.ModuleT.SAnas
                              where es.Carte == carte.ID
                              select es;

                    if (esCarte.Count() > 0)
                    {
                        foreach (var es in esCarte)
                        {
                            table.Add(this.BuildCablageRow(es, i, NbColonne));
                        } 
                    }

                    // STors
                    esCarte = from es in PegaseData.Instance.ModuleT.STORS
                              where es.Carte == carte.ID
                              select es;

                    if (esCarte.Count() > 0)
                    {
                        foreach (var es in esCarte)
                        {
                            table.Add(this.BuildCablageRow(es, i, NbColonne));
                        }
                    }
                }
            }

            // Ajouter la section commentaire si besoin
            XElement EasyCommentaireTable = this.BuildEasyCommentaireTable();
            if (EasyCommentaireTable != null)
            {
                section.Add(EasyCommentaireTable);
            }

            this.Fields.Add(TABLE_CABLAGE, section);
        } // endMethod: BuildCableTable
        
        /// <summary>
        /// Construire la table des commentaires 'EasyConfig'
        /// </summary>
        public XElement BuildEasyCommentaireTable (  )
        {
            XElement Result = null;

            String section = "";
            Dictionary<String, String> commentaireByES = new Dictionary<String, String>();

            section += "<div>";
			section += "<table class='MsoTableGrid_cartouche' border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"990px\" style='border-collapse: collapse;table-layout:fixed;width:240pt'>";
			section += "<col width=\"990px\" />";
			section += "<tr>";
			section += "<td height=\"21\" class=\"xl6613982\" style='vertical-align:top;border:.5pt solid windowtext;font-size:8.0pt;height:15.75pt'>Comments : <BR/> $BALISE0110</td>";
			section += "</tr>";
		 	section += "</table>";
		    section += "</div>";

            // Construire le dictionnaire de données
            foreach (var mode in PegaseData.Instance.Commentaire.CommentaireByModes)
            {
                foreach (var commentaire in mode.EasyCommentaires)
                {
                    if (commentaire.Commentaire != "")
                    {
                        String previousCommentaire;
                        String modeName;

                        if (mode.LinkedMode.Position == 32)
                        {
                            modeName = "All modes";
                        }
                        else
                        {
                            modeName = String.Format("Mode {0}", mode.LinkedMode.Position + 1);
                        }
                        // 1 - un commentaire existe-t-il déjà pour cette entrée / sortie?
                        if (commentaireByES.TryGetValue(commentaire.LinkedES.MnemoHardware, out previousCommentaire))
                        {
                            commentaireByES[commentaire.LinkedES.MnemoHardware] += String.Format("     {0} -> {1}\n", modeName, commentaire.Commentaire);
                        }
                        else
                        {
                            commentaireByES.Add(commentaire.LinkedES.MnemoHardware, String.Format("     {0} -> {1}\n", modeName, commentaire.Commentaire));
                        }
                    }
                }
            }

            if (commentaireByES.Count > 0)
            {
                String textBlock = "<pre>";
                String textblobk2 = "";
                IEnumerable<String> esKeys = from es in commentaireByES.Keys
                                             orderby es ascending
                                             select es;
                // Construire la section commentaire
                foreach (String es in esKeys)
                {
                    //textBlock += es + "\n";
                    //textBlock += commentaireByES[es];
                    textblobk2 += es + "\n";
                    textblobk2 += commentaireByES[es];
                    
                    //byte[] bytes = Encoding.Default.GetBytes(commentaireByES[es]);
                    //textBlock +=  Encoding.UTF8.GetString(bytes);
                }
               // textBlock += "</pre>";
                XElement replace = new XElement("pre",textblobk2);
                textBlock = replace.ToString();
                section = section.Replace("$BALISE0110", textBlock);
                
                Result = XElement.Parse(section);
            }

            return Result;
        } // endMethod: BuildEasyCommentaireTable

        /// <summary>
        /// Construire la ligne du tableau de cablage correspondant à l'entrée ou la sortie jointe (es)
        /// </summary>
        public XElement BuildCablageRow ( ES es, Int32 Start, Int32 NbColonne )
        {
            XElement Result = new XElement("tr");
            Boolean Physic = false, Logic = false;
            String esName = es.MnemoHardware;

            for (int j = Start; j < Start + NbColonne; j++)
            {
                if (PegaseData.Instance.Commentaire.GetEasyCommentaire(j, es.MnemoHardware) != null)
                {
                    esName = es.MnemoHardware + " *";
                } 
            }
            Result.Add(this.CreateTd("MsoCol1 MsoCol", "MsoNormal", esName));
            Result.Add(this.CreateTd("MsoCol2 MsoCol", "MsoNormal", es.MnemoBornier));
            Result.Add(this.CreateTd("MsoCol3 MsoCol", "MsoNormal", es.MnemoClient));

            for (int j = Start; j < Start + NbColonne; j++)
            {
                String Cmd = "";

                foreach (var effecteur in EasyConfigData.Get().Modes[j].Effecteurs)
                {
                    foreach (var position in effecteur.Positions)
                    {
                        var query = from val in position.Outputs
                                    where val.STOR.MnemoHardware == es.MnemoHardware
                                    select val;

                        if (query.Count() > 0)
                        {
                            foreach (var item in query)
                            {
                                if (item.IsUsed)
                                {
                                    if (Cmd == "")
                                    {
                                        if (effecteur.Organ != null)
                                        {
                                            Cmd = effecteur.Organ.MnemoHardOrganeMO;
                                            Physic = true;
                                        }
                                        else
                                        {
                                            Cmd = effecteur.Name;
                                            Logic = true;
                                        }
                                    }
                                    else if (!Cmd.Contains(effecteur.Name))
                                    {
                                        String AddCmdTxt;
                                        if (effecteur.Organ != null)
                                        {
                                            AddCmdTxt = effecteur.Organ.MnemoHardOrganeMO;
                                            Physic = true;
                                        }
                                        else
                                        {
                                            AddCmdTxt = effecteur.Name;
                                            Logic = true;
                                        }
                                        if (!Cmd.Contains(AddCmdTxt))
                                        {
                                            Cmd += " | " + AddCmdTxt; 
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Cmd != "")
                {
                    String Type;

                    if (Physic && Logic)
                    {
                        Type = "MS / ES";
                    }
                    else if (Physic)
                    {
                        Type = "MS";
                    }
                    else
                    {
                        Type = "ES";
                    }
                    Result.Add(this.CreateTd("MsoCol4 MsoCol", "MsoNormal", Cmd));
                    Result.Add(this.CreateTd("MsoCol5 MsoCol", "MsoNormal", Type));
                }
                else
                {
                    Result.Add(this.CreateTd("MsoCol4 MsoCol", "MsoNormal", Cmd));
                    Result.Add(this.CreateTd("MsoCol5 MsoCol", "MsoNormal", " "));
                }
            }

            return Result;
        } // endMethod: BuildCablageRow

        /// <summary>
        /// Construire la table des cablages
        /// </summary>
        private XElement BuildEmptyCableTable( Carte carte, Int32 modeInf, Int32 NbMode, out XElement Table )
        {
            XElement Result;
            XAttribute Attrib;

            Result = new XElement("div");
            Attrib = new XAttribute("id", "Motherboard");
            Result.Add(Attrib);

            // Créer la table
            Table = new XElement("table");
            Attrib = new XAttribute("class", "MsoTableGrid");
            Table.Add(Attrib);
            Result.Add(Table);

            // Définir la taille des colonnes
            XElement colgroup = new XElement("colgroup");
            Table.Add(colgroup);

            colgroup.Add(this.NewColForCableTable(81));
            colgroup.Add(this.NewColForCableTable(81));
            colgroup.Add(this.NewColForCableTable(82));
            for (int i = modeInf; i < modeInf + NbMode; i++)
            {
                colgroup.Add(this.NewColForCableTable(20));
                colgroup.Add(this.NewColForCableTable(20));
            }

            // Définir les headers des colonnes
            XElement Header1 = new XElement("tr");
            XElement Header1_1 = new XElement("td");
            Attrib = new XAttribute("class", "MsoHeader");
            Header1_1.Add(Attrib);
            Attrib = new XAttribute("colspan", "3");
            Header1_1.Add(Attrib);

            XElement PHeader1_1 = new XElement("p");
            Attrib = new XAttribute("class", "MsoNormalG");
            PHeader1_1.Add(Attrib);
            PHeader1_1.Value = carte.Nom;

            Header1.Add(Header1_1);
            Header1_1.Add(PHeader1_1);
            Table.Add(Header1);

            for (int i = modeInf; i < modeInf + NbMode; i++)
            {
                XElement Header1_2 = new XElement("td");
                Attrib = new XAttribute("class", "MsoHeader2");
                Header1_2.Add(Attrib);
                Attrib = new XAttribute("colspan", "2");
                Header1_2.Add(Attrib);
                XElement PHeader1_2 = new XElement("p");
                Attrib = new XAttribute("class", "MsoNormalG");
                PHeader1_2.Add(Attrib);
                // PHeader1_2.Value = PegaseData.Instance.OLogiciels.ModesExploitation[i].ModeLabel.LibelSelecteur;
                PHeader1_2.Value = EasyConfigData.Get().Modes[i].ModeName;
                if (PHeader1_2.Value == "Tous les modes")
                {
                    PHeader1_2.Value = "All modes";
                }

                Header1_2.Add(PHeader1_2);
                Header1.Add(Header1_2);
            }

            // Définir les headers des lignes
            XElement Header2 = new XElement("tr");
            Table.Add(Header2);

            Header2.Add(this.CreateTd("MsoCol1 MsoCol", "MsoNormalG", "E/S"));
            Header2.Add(this.CreateTd("MsoCol2 MsoCol", "MsoNormalG", "Header"));
            Header2.Add(this.CreateTd("MsoCol3 MsoCol", "MsoNormalG", "Function"));

            for (int i = modeInf; i < modeInf + NbMode; i++)
            {
                Header2.Add(this.CreateTd("MsoCol4 MsoCol", "MsoNormalG", "Cmd"));
                Header2.Add(this.CreateTd("MsoCol5 MsoCol", "MsoNormalG", "Type"));
            }

            return Result;
        } // endMethod: BuildEmptyCableTable
        
        /// <summary>
        /// Création d'une colonne HTML
        /// </summary>
        private XElement CreateTd ( String classTd, String classP, String Value )
        {
            XElement Result;
            XAttribute Attrib;

            Result = new XElement("td");

            Attrib = new XAttribute("class", classTd);
            Result.Add(Attrib);
            XElement PHeader2_1 = new XElement("p");
            Result.Add(PHeader2_1);
            Attrib = new XAttribute("class", classP);
            PHeader2_1.Add(Attrib);
            PHeader2_1.Value = Value;

            return Result;
        } // endMethod: CreateTd

        /// <summary>
        /// Créer la table des colonnes pour la table des cablages
        /// </summary>
        private XElement NewColForCableTable ( Int32 Width )
        {
            XElement Result;
            XAttribute attrib;

            Result = new XElement("col");
            attrib = new XAttribute("width", String.Format("{0}px", Width));
            Result.Add(attrib);

            attrib = new XAttribute("style", String.Format("width;{0}pt", Width));
            Result.Add(attrib);

            return Result;
        } // endMethod: NewColForCableTable
        
        /// <summary>
        /// Construire une vue des différents écrans des MO
        /// </summary>
        public void BuildMOScreens( )
        {
            XElement screenContener = new XElement("div");
            XAttribute Attrib = new XAttribute("id", "retourparmode");
            screenContener.Add(Attrib);
            String EmptyScreen = "", TableScreen = "";

            Int32 i = 1;
            // 0 - Ajout du tableau de résumé
            TableScreen += "<div class=\"tableaux\">";
            TableScreen += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"240\" style='border-collapse:collapse;table-layout:fixed;width:240px'>";
			TableScreen += "<col width=\"50\" style='width:50px'/>";
			TableScreen += "<col width=\"4\" style=\"mso-width-source:userset;mso-width-alt:731;width:4px\" />";
			TableScreen += "<tr height=\"19\" style='mso-height-source:userset;height:19px'>";
			TableScreen += "<td height=\"19\" class=\"xl7314659\" width=\"150\" style='height:19px;width:150px'> </td>";
			TableScreen += "</tr>";
			TableScreen += "<tr>";
			TableScreen += "<td  height=\"19\" class=\"xl6914985\" width=\"150\" style='border-top:border-right:1.0pt solid black;border-right:1.0pt solid black;;height:19px;width:150px'>";
			TableScreen += "<li>Number of modes : $BALISE0637</li>";
			TableScreen += "<li>Navigation control : $BALISE0638</li>";
			TableScreen += "<li>Accept change mode : $BALISE0639</li>";
			TableScreen += "<li>TM behavior : $BALISE0640</li>";
			TableScreen += "</td>";
			TableScreen += "<td class=\"xl1514658\"></td>";
            TableScreen += "</tr>";
			TableScreen += "</table>";
            TableScreen += "</div>";

            TableScreen = TableScreen.Replace("$BALISE0637", this.Fields["$BALISE0637"].ToString());
            TableScreen = TableScreen.Replace("$BALISE0638", this.Fields["$BALISE0638"].ToString());
            TableScreen = TableScreen.Replace("$BALISE0639", this.Fields["$BALISE0639"].ToString());
            TableScreen = TableScreen.Replace("$BALISE0640", this.Fields["$BALISE0640"].ToString());

            screenContener.Add(XElement.Parse(TableScreen));
            // 1 - Définition de l'écran

            EmptyScreen += "<div class=\"tableaux\">";
            EmptyScreen += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"240\" style=\"border-collapse:collapse;table-layout:fixed;width:240px\">";
            EmptyScreen += "<col width=\"50\" span=\"2\" style=\"width:50px\" />";
            EmptyScreen += "<col width=\"4\" style=\"mso-width-source:userset;mso-width-alt:731;width:4px\" />";
            EmptyScreen += "<tr height=\"19\" style=\"mso-height-source:userset;height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl7314659\" width=\"150\" style=\"border-right:none;height:19px;width:150px\">$BALISE1001</td>";
            EmptyScreen += "<td class=\"xl1514658\" width=\"10px\" style=\"width:10px\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"18\" class=\"xl7314658\" style=\"border-right:1.0pt solid black;height:19px;\">";
            EmptyScreen += "<img src=\"images/bandeau.bmp\" alt=\"\" width=\"200\" height=\"18\" />";
            EmptyScreen += "</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl6514658\" style=\"vertical-align:middle ;border-right:1.0pt solid black; height:19px\">";
            EmptyScreen += "$BALISE1002";
            EmptyScreen += "</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl6714658\" style=\"border-right:1.0pt solid black; height:19px\">";
            EmptyScreen += "$BALISE1003";
            EmptyScreen += "</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl6714658\" style=\"border-right:1.0pt solid black; height:19px\">$BALISE1004</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl6714658\" style=\"border-right:1.0pt solid black; height:19px\">$BALISE1005</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"19\" class=\"xl6714658\" style=\"border-right:1.0pt solid black; height:19px\">";
            EmptyScreen += "$BALISE1006</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td height=\"19\" class=\"xl6914658\" style=\"height:19px;border-top:none\">$BALISE1011</td>";
            EmptyScreen += "<td class=\"xl7014658\" style=\"border-top:none;border-left:none\">$BALISE1012</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:19px\">";
            EmptyScreen += "<td height=\"19\" class=\"xl6914658\" style=\"height:19px;border-top:none\">$BALISE1009</td>";
            EmptyScreen += "<td class=\"xl7014658\" style=\"border-top:none;border-left:none\">$BALISE1010</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"19\" style=\"height:15.75pt\">";
            EmptyScreen += "<td height=\"19\" class=\"xl7114658\" style=\"height:19px;border-top:none\">$BALISE1007</td>";
            EmptyScreen += "<td class=\"xl7214658\" style=\"border-top:none;border-left:none\">$BALISE1008</td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "<tr height=\"3\" style=\"mso-height-source:userset;height:3px\">";
            EmptyScreen += "<td colspan=\"2\" height=\"3\" class=\"xl7314659\" style=\"border-right:none;height:3px;width:150px\"></td>";
            EmptyScreen += "<td class=\"xl1514658\"></td>";
            EmptyScreen += "</tr>";
            EmptyScreen += "</table>";
            EmptyScreen += "</div>";

            // 2 - Création des écrans par remplacement des balises
            foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
            {
                String Screen = EmptyScreen.Substring(0);

                // 2.1 - Numéro du mode
                String NumMode = String.Format("Mode {0}", i++);
                Screen = Screen.Replace("$BALISE1001", NumMode);

                // 2.2 - Nom de l'équipement
                Screen = Screen.Replace("$BALISE1002", this.BuildSelecteurLabel(PegaseData.Instance.ParamHorsMode.LibelleEquipement));

                // 2.3 - Nom du mode
                Screen = Screen.Replace("$BALISE1003", this.BuildSelecteurLabel(mode.ModeLabel));

                // 2.4 - Retour d'information
                Int32 j = 1004;
                for(Int32 k = 0 ; k < 3; k++)
                {
                    InfoByMode retourInfo = mode.LabelsInfoByPos[k];
                    String LibelleInfo = " ";
                    if (retourInfo != null)
                    {
                        if (retourInfo.Information.TypeVariable == RIType.RIANA)
                        {
                            LibelleInfo = String.Format("{0} {1} {2}", this.BuildInfoLabel(retourInfo.InfoLibelRetour), retourInfo.ValueDemo, this.BuildInfoLabel(retourInfo.InfoLibelUnit));
                        }
                        else if (retourInfo.Information.TypeVariable == RIType.RIBOOL)
                        {
                            LibelleInfo = String.Format("{0}", this.BuildInfoLabel(retourInfo.InfoLibelMin));
                        }
                        else if (retourInfo.Information.TypeVariable == RIType.RIVARNUM)
                        {
                            if (retourInfo.LibelsVarNumerique.Length > 0)
	                        {
		                        LibelleInfo = String.Format("{0}", this.BuildInfoLabel(retourInfo.LibelsVarNumerique[0])); 
	                        }
                        }
                    }

                    Screen = Screen.Replace(String.Format("$BALISE{0:0000}", j++), LibelleInfo);
                }

                // 2.5 - Selecteur
                foreach (var selecteur in mode.LabelsSelecteurByPos)
                {
                    String LibelSelecteur = " ";
                    if (selecteur != null && selecteur.Count > 0)
                    {
                        LibelSelecteur = this.BuildSelecteurLabel(selecteur[0]);
                    }
                    Screen = Screen.Replace(String.Format("$BALISE{0:0000}", j++), LibelSelecteur);
                }

                // 3 - Création de la balise écran
                XElement XScreen = XElement.Parse(Screen);
                screenContener.Add(XScreen);
            }

            this.Fields.Add("$BALISE1000", screenContener);
        } // endMethod: BuildMOScreen

        /// <summary>
        /// Construire les libellés
        /// </summary>
        private String BuildSelecteurLabel ( SelecteurLabel selecteurLabel )
        {
            String Result = "";

            if (selecteurLabel != null)
            {
                if (selecteurLabel.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result = this.BuildBaliseImgUserFile(selecteurLabel.NomFichierBitmap, 0, 0);
                }
                else
                {
                    Result = selecteurLabel.Label;
                    Result = Result.Replace("<", " ");
                    Result = Result.Replace(">", " ");
                }
            }

            return Result;
        } // endMethod: BuildLabel
        private String BuildNewInformationLabel(InformationLabel information)
        {
            String Result = "";

            if (information != null)
            {
                if (information.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result = this.BuildNewBaliseImgUserFile(information.NomFichierBitmap, 0, 0);
                }
                else
                {
                    if (information.PoliceGras)
                    {
                        Result = GestBbLibelleVersBitBoard(information.Label, 1);
                    }
                    else
                    {
                        Result = GestBbLibelleVersBitBoard(information.Label, 0);
                    }
                }
            }

            Result = Result.Replace('\\','/');
            return Result;
        } // endMethod: BuildLabel

        private String BuildNewInformationLabelMulti(InformationLabel label, InformationLabel texte, InformationLabel unite)
        {
            String Result1 = "";
            String Result2 = "";
            String Result3 = "";
            string Result = "";

            if (label != null)
            {
                if (label.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result1 = this.BuildNewBaliseImgUserFile(label.NomFichierBitmap, 0, 0);
                }
                else
                {
                    if (label.PoliceGras)
                    {
                        Result1 = GestBbLibelleVersBitBoard(label.Label, 1);
                    }
                    else
                    {
                        Result1 = GestBbLibelleVersBitBoard(label.Label, 0);
                    }
                }
            }
            if (texte != null)
            {
                if (texte.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result2 = this.BuildNewBaliseImgUserFile(texte.NomFichierBitmap, 0, 0);
                }
                else
                {
                    if (texte.PoliceGras)
                    {
                        Result2 = GestBbLibelleVersBitBoard(texte.Label, 1);
                    }
                    else
                    {
                        Result2 = GestBbLibelleVersBitBoard(texte.Label, 0);
                    }
                }
            }
            if (unite != null)
            {
                if (unite.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result3 = this.BuildNewBaliseImgUserFile(unite.NomFichierBitmap, 0, 0);
                }
                else
                {
                    if (unite.PoliceGras)
                    {
                        Result3 = GestBbLibelleVersBitBoard(unite.Label, 1);
                    }
                    else
                    {
                        Result3 = GestBbLibelleVersBitBoard(unite.Label, 0);
                    }
                }
            }
            Result3 = Result3.Replace('\\', '/');
            // concatenation des images

            Result = Concatener3(Result1, Result2, Result3);

            return Result;
        } // endMethod: BuildLabel

        private string Concatener3(string image1, string image2, string image3)
        {
            string result = "";
            //mesimages est la chaîne qui contient le chemin vers ... Mes Images !
            string mesimages = DefaultValues.Get().RapportFolder;

            //ImagesOfTheCell contiendra tous les fichiers ".jpg" de Mes Images !
            string[] ImagesOfTheCell = new string[3];
            string name = "";
            if (System.IO.Directory.GetFiles(mesimages, image1).Count() > 0)
            {
                ImagesOfTheCell[0] =( System.IO.Directory.GetFiles(mesimages, image1).First());
                name= ImagesOfTheCell[0].GetHashCode().ToString();
            }
            if (System.IO.Directory.GetFiles(mesimages, image2).Count() > 0)
            {
                ImagesOfTheCell[1] = (System.IO.Directory.GetFiles(mesimages, image2)).First();
                name += ImagesOfTheCell[1].GetHashCode().ToString();
            }
            if (System.IO.Directory.GetFiles(mesimages, image3).Count() >0)
            {
                ImagesOfTheCell[2] = (System.IO.Directory.GetFiles(mesimages, image3)).First();
                name += ImagesOfTheCell[2].GetHashCode().ToString();
            }
           name =  name.Replace('-', '_');
            //cwidth contiendra la largeur totale de l'image
            int cwidth = 0;
            //cheight contiendra la plus grande hauteur de toutes les images donc la hauteur de l'image finale.
            int cheight = 0;
            //bmplist contiendra tous les bitmaps de Mes Images, comme ça on ne les recharge pas deux fois ^^
            Drwg.Bitmap[] bmplist = new Drwg.Bitmap[ImagesOfTheCell.Length];
            for (int i = 0; i < ImagesOfTheCell.Length; i++)
            {
                if (ImagesOfTheCell[i] != null)
                {
                    string str = ImagesOfTheCell[i];
                    //On charge le bitmap.
                    Drwg.Bitmap bmp = new Drwg.Bitmap(str);
                    
                        bmplist[i] = bmp;
                        //On ajoute sa largeur à cwidth;
                        cwidth += bmp.Size.Width;
                        //Si la hauteur est plus grande alors on la redéfinit
                        cheight = Math.Max(cheight, bmp.Size.Height);
                   
                }
            }

            //Le bitmap final a donc une taille de cwidth, cheight.
            Drwg.Bitmap finalbitmap = new Drwg.Bitmap(cwidth, cheight);
            //On créé un nouveau Graphics à partir de finalbitmap.
            Drwg.Graphics g = Drwg.Graphics.FromImage(finalbitmap);
            //oldWidth contiendra la position en largeur de la prochaine image donc commence à 0.
            int oldWidth = 0;
            for (int i = 0; i < ImagesOfTheCell.Length; i++)
            {
                if (ImagesOfTheCell[i] != null)
                {
                    //On recharge nos bitmaps de tout à l'heure
                    Drwg.Bitmap bmp = bmplist[i];
                    //Point en haut à droite du rectangle.
                    System.Drawing.Point p = new Drwg.Point(oldWidth, 0);
                    //On définit le rectangle.
                    System.Drawing.Rectangle rect = new Drwg.Rectangle(p, bmp.Size);
                    //On dessine l'image dans finalbitmap via Graphics.DrawImage(..., ...);
                    g.DrawImage(bmp, rect);
                    //On ajoute à oldWidth la largeur de l'image qu'on vient de dessiner
                    oldWidth += bmp.Size.Width;
                    bmp.Dispose();
                }
            }
            //Enfin, pour voir le résultat on enregistre le tout dans concatenation.bmp dans Mes Images.
            finalbitmap.Save(mesimages + DefaultValues.Get().RelativeRapportFolderImages+ name+".png");
            finalbitmap.Dispose();
            String destRelativeFile = String.Format("{0}{1}", DefaultValues.Get().RelativeRapportFolderImages, name + ".png");
            bmplist = null;


            // 2 - Construire la balise
            result = destRelativeFile;
            result = result.Replace('\\', '/');
            return result;
        }



        private String BuildNewSelecteurLabel(SelecteurLabel selecteurLabel)
        {
            String Result = "";

            if (selecteurLabel != null)
            {
                if (selecteurLabel.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result = this.BuildBaliseImgUserFile(selecteurLabel.NomFichierBitmap, 0, 0);
                }
                else
                {
                    if (selecteurLabel.PoliceGras)
                    {
                        Result = GestBbLibelleVersBitBoard(selecteurLabel.Label, 1);
                    }
                    else
                    {
                        Result = GestBbLibelleVersBitBoard(selecteurLabel.Label, 0);
                    }                   
                }
            }
            Result = Result.Replace('\\', '/');
            return Result;
        } // endMethod: BuildLabel

        private String BuildNewAlarmLabel(Alarme alarme)
        {
            String Result = "";

            if (alarme != null)
            {
                if (alarme.InformationL.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result = this.BuildBaliseImgUserFile(alarme.InformationL.NomFichierBitmap , 0, 0);
                }
                else
                {
                    if (alarme.InformationT.PoliceGras)
                    {
                        Result = GestBbLibelleVersBitBoard(alarme.InformationT.Label, 1);
                    }
                    else
                    {
                        Result = GestBbLibelleVersBitBoard(alarme.InformationT.Label, 0);
                    }
                }
            }
            Result = Result.Replace('\\', '/');
            return Result;
        } // endMethod: BuildLabel
        public string GestBbLibelleVersBitBoard(string texte, int gras)
        {
            String fileName = "";
            try
            {
                // ecrire dans le canvas
                Drwg.Bitmap Btm;

                switch (gras)
                {
                    case 2:     // Double hauteur
                        Btm = this.TextToBitmap(texte, 24, 127, 28, "Microsoft Sans Serif", (int)1);
                        break;
                    case 3: // Gras double
                        Btm = this.TextToBitmap(texte, 24, 127, 28, "Microsoft Sans Serif", (int)0);
                        break;
                    case 1:   // gras
                        Btm = this.TextToBitmap(texte, 12, 127, 14, "Microsoft Sans Serif", (int)1);
                        break;
                    default:
                        Btm = this.TextToBitmap(texte, 12, 127, 14, "Microsoft Sans Serif", (int)0);
                        break;
                }

                string path = DefaultValues.Get().RapportFolderImages;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                fileName = Btm.GetHashCode().ToString();
                Btm.Save(path + fileName + ".png");
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return DefaultValues.Get().RelativeRapportFolderImages+ fileName + ".png";
        }
        public Drwg.Bitmap TextToBitmap(String Text, Int32 FontHeight, Int32 BitmapWidth, Int32 BitmapHeight, String FontName, Int32 FontWeight)
        {
            Drwg.Bitmap Btp = new Drwg.Bitmap(BitmapWidth, BitmapHeight, Drwg.Imaging.PixelFormat.Format24bppRgb);

            Drwg.Graphics dc = Drwg.Graphics.FromImage(Btp);
            dc.SmoothingMode = Drwg.Drawing2D.SmoothingMode.None;
            dc.TextRenderingHint = Drwg.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            Drwg.Font font;
            if (FontWeight > 0)
            {
                font = new System.Drawing.Font(FontName, FontHeight, Drwg.FontStyle.Bold, Drwg.GraphicsUnit.Pixel);
            }
            else
            {
                font = new System.Drawing.Font(FontName, FontHeight, Drwg.FontStyle.Regular, Drwg.GraphicsUnit.Pixel);
            }

            Drwg.Brush white = new Drwg.SolidBrush(Drwg.Color.White);
            Drwg.Brush black = new Drwg.SolidBrush(Drwg.Color.Black);

            dc.FillRectangle(white, new Drwg.Rectangle(0, 0, BitmapWidth, BitmapHeight));
            if (FontHeight > 12)
            {
                dc.DrawString(Text, font, black, new Drwg.PointF(0, 3));
            }
            else
            {
                dc.DrawString(Text, font, black, new Drwg.PointF(0, -2));
            }

            // recherche taille bitmap
            int xmax = 0;
            for (int y = 0; y < Btp.Height; y++)
            {
                for (int x = 0; x < Btp.Width ; x++)
                {
                    System.Drawing.Color tmp =new System.Drawing.Color();
                    tmp = Drwg.Color.FromArgb(255,255,255);
                    if (Btp.GetPixel(x, y) != tmp)
                    {
                        if (x > xmax)
                        {
                            xmax = x;
                        }
                    }
                }
            }
            xmax += 1;
            Btp = new Drwg.Bitmap(xmax, BitmapHeight, Drwg.Imaging.PixelFormat.Format24bppRgb);

            dc = Drwg.Graphics.FromImage(Btp);
            dc.SmoothingMode = Drwg.Drawing2D.SmoothingMode.None;
            dc.TextRenderingHint = Drwg.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            if (FontWeight > 0)
            {
                font = new System.Drawing.Font(FontName, FontHeight, Drwg.FontStyle.Bold, Drwg.GraphicsUnit.Pixel);
            }
            else
            {
                font = new System.Drawing.Font(FontName, FontHeight, Drwg.FontStyle.Regular, Drwg.GraphicsUnit.Pixel);
            }


            dc.FillRectangle(white, new Drwg.Rectangle(0, 0, xmax, BitmapHeight));
            if (FontHeight > 12)
            {
                dc.DrawString(Text, font, black, new Drwg.PointF(0, 3));
            }
            else
            {
                dc.DrawString(Text, font, black, new Drwg.PointF(0, -2));
            }

            white.Dispose();
            black.Dispose();
            font.Dispose();
            dc.Dispose();

            return Btp;
        } // endMethod: TextToBitmap

        /// <summary>
        /// Libellé d'un retour d'information
        /// </summary>
        private String BuildInfoLabel ( InformationLabel informationLabel )
        {
            String Result = "";

            if (informationLabel != null)
            {
                if (informationLabel.IsBitmap)
                {
                    // construire la balise image faisant référence
                    Result = this.BuildBaliseImgUserFile(informationLabel.NomFichierBitmap, 0, 0);
                }
                else
                {
                    Result = informationLabel.Label;
                    Result = Result.Replace("<", " ");
                    Result = Result.Replace(">", " ");
                }
            }

            return Result;
        } // endMethod: BuildInfoLabel
        
        /// <summary>
        /// Construire l'image pour le rapport
        /// </summary>
        public String BuildBaliseImgRapport ( String FileName, Int32 Width, Int32 Height )
        {
            String Result = "";
            String DestFile;

            DestFile = String.Format("imagesrapport\\{0}", FileName);

            Result = String.Format("<img src=\"{0}\"", DestFile);

            if (Width > 0)
            {
                Result += String.Format(" width=\"{0}\"", Width);
            }
            if (Height > 0)
            {
                Result += String.Format(" height=\"{0}\"", Height);
            }

            Result += "/>";

            return Result;
        } // endMethod: BuildBaliseImgRapport

        /// <summary>
        /// Construire une balise image et recopier le bitmap
        /// </summary>
        public String BuildBaliseImgUserFile ( String FileName, Int32 Width, Int32 Height )
        {
            String Result = "";
            String destFile = "";

            // 1 - Copier le fichier
            if (File.Exists(FileName))
            {
                destFile = String.Format("{0}imagesuser\\{1}", DefaultValues.Get().RapportFolder, Path.GetFileName(FileName));
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                File.Copy(FileName, destFile);
            }
            else
            {
                // Chercher le fichier dans le fichier en cours d'utilisation
                Uri imageUri = new Uri(FileName, UriKind.Relative);
                
                if (!PegaseData.Instance.CurrentPackage.IsOpen)
                {
                    PegaseData.Instance.CurrentPackage.OpenPackage(); 
                }
                Stream stream = PegaseData.Instance.CurrentPackage.GetPartStream(imageUri);

                if (stream != null)
                {
                    using (BinaryReader BR = new BinaryReader(stream))
                    {
                        String fname;
                        Int32 pos;
                        Byte[] buffer = new Byte[BR.BaseStream.Length];
                        pos = FileName.LastIndexOf('/');
                        fname = FileName.Substring(pos);
                        destFile = String.Format("{0}imagesuser\\{1}", DefaultValues.Get().RapportFolder, fname);
                        BR.Read(buffer, 0, (Int32)BR.BaseStream.Length);
                        using (BinaryWriter BW = new BinaryWriter(File.Open(destFile, FileMode.Create)))
                        {
                            BW.Write(buffer);
                            BW.Flush();
                        }
                    }
                }

                PegaseData.Instance.CurrentPackage.ClosePackage();
            }
            String destRelativeFile = String.Format("imagesuser\\{0}", Path.GetFileName(destFile));

            

            Result = destRelativeFile;

            return Result;
        } // endMethod: BuildBaliseImgUserFile
        /// <summary>
        /// Construire une balise image et recopier le bitmap
        /// </summary>
        public String BuildNewBaliseImgUserFile(String FileName, Int32 Width, Int32 Height)
        {
            String Result = "";
            String destFile = "";

            // 1 - Copier le fichier
            if (File.Exists(FileName))
            {
                string tmp = Path.GetFileName(FileName).GetHashCode().ToString();
                tmp = tmp.Replace('-', '_');
                string ext = Path.GetExtension(FileName);
                tmp = tmp + ext;
                destFile = String.Format("{0}{1}{2}", DefaultValues.Get().RapportFolder, DefaultValues.Get().RelativeRapportFolderImages, tmp);
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
               
                File.Copy(FileName, destFile);
            }
            else
            {
                // Chercher le fichier dans le fichier en cours d'utilisation
                Uri imageUri = new Uri(FileName, UriKind.Relative);

                if (!PegaseData.Instance.CurrentPackage.IsOpen)
                {
                    PegaseData.Instance.CurrentPackage.OpenPackage();
                }
                Stream stream = PegaseData.Instance.CurrentPackage.GetPartStream(imageUri);

                if (stream != null)
                {
                    using (BinaryReader BR = new BinaryReader(stream))
                    {
                        String fname;
                        Int32 pos;
                        Byte[] buffer = new Byte[BR.BaseStream.Length];
                        pos = FileName.LastIndexOf('/');
                        if ((pos + 1) < FileName.Length)
                        {
                            fname = FileName.Substring(pos + 1);


                            destFile = String.Format("{0}{1}{2}", DefaultValues.Get().RapportFolder, DefaultValues.Get().RelativeRapportFolderImages, fname);
                            BR.Read(buffer, 0, (Int32)BR.BaseStream.Length);
                            using (FileStream file = (File.Open(destFile, FileMode.OpenOrCreate)))
                            {
                                using (BinaryWriter BW = new BinaryWriter(file))
                                {
                                    BW.Write(buffer);
                                    BW.Flush();
                                    BW.Close();
                                }
                            }
                        }
                        else
                        {
                            destFile = "";
                        }
                    }
                }
                PegaseData.Instance.CurrentPackage.ClosePackage();
            }
            String destRelativeFile = String.Format("{0}{1}", DefaultValues.Get().RelativeRapportFolderImages,Path.GetFileName(destFile));

            // 2 - Construire la balise
            Result = destRelativeFile;
            Result= Result.Replace('\\','/');

            return Result;
        } // endMethod: BuildBaliseImgUserFile
        /// <summary>
        /// Construire les données venant des paramètres simples
        /// </summary>
        public void BuildParamSimpleData ( )
        {
            XText text;
            VariableEditable VE;
            ListChoixManager LCM;

            text = new XText(PegaseData.Instance.RefCompany);
            this.Fields.Add("$BALISE0207", text);
            text = new XText(PegaseData.Instance.RefCustomer);
            this.Fields.Add("$BALISE0209", text);
            text = new XText(PegaseData.Instance.RefCustomerCode);
            this.Fields.Add("$BALISE0208", text);
            CultureInfo ci = new CultureInfo(LanguageSupport.Get().LanguageTypeDe(this.LanguageName));
            text = new XText(PegaseData.Instance.RefDate.ToString("d",ci));
            this.Fields.Add("$BALISE0205", text);
            text = new XText(PegaseData.Instance.RefIndice);
            this.Fields.Add("$BALISE0201", text);
            text = new XText(PegaseData.Instance.RefPartNumber);
            this.Fields.Add("$BALISE0204", text);


            // Startup screen
            text = new XText(GestionVariableSimple.Instance.GetVariableByName("BitmapLogo").RefElementValue);
            this.Fields.Add("$BALISE0601", text);

            // Language du module operateur
            VE = GestionVariableSimple.Instance.GetVariableByName("NumLangue");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_NumLangue", "Anglais"));
            text = new XText(LCM.GetLabel(VE.RefElementValue));
            this.Fields.Add("$BALISE0602", text);

            // Language ideogramme
            VE = GestionVariableSimple.Instance.GetVariableByName("ChinoisOn");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ChinoisOn", "Anglais"));
            text = new XText(LCM.GetLabel(VE.RefElementValue));
            this.Fields.Add("$BALISE0603", text);

            // Canal demandé
            VE = GestionVariableSimple.Instance.GetVariableByName("CanalDemande");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_CanalDemande", "Anglais"));
            text = new XText(LCM.GetLabel(VE.RefElementValue));
            this.Fields.Add("$BALISE0604", text);

            // Mode infrarouge
            VE = GestionVariableSimple.Instance.GetVariableByName("Mode");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Mode", "Anglais"));
            text = new XText(LCM.GetLabel(VE.RefElementValue));
            this.Fields.Add("$BALISE0605", text);

            // Association infrarouge
            VE = GestionVariableSimple.Instance.GetVariableByName("ModeAssociation");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ModeAssociation", "Anglais"));
            text = new XText(LCM.GetLabel(VE.RefElementValue));
            this.Fields.Add("$BALISE0606", text);

            // Identification IR
            VE = GestionVariableSimple.Instance.GetVariableByName("CharIdent");
            text = new XText(VE.RefElementValue);
            this.Fields.Add("$BALISE0607", text);

            // Arrêt passif
            text = new XText(String.Format("{0} s", (Double)PegaseData.Instance.GestionTemporisation.DelaiArretPassif / (Double)10));
            this.Fields.Add("$BALISE0610", text);

            // Delai avant mise en veille
            text = new XText(String.Format("{0} s", PegaseData.Instance.GestionTemporisation.DelaiMiseEnVeille));
            this.Fields.Add("$BALISE0611", text);

            // Delai de l'abscence radio transceiver
            text = new XText(String.Format("{0} s", PegaseData.Instance.GestionTemporisation.DelaiABSRadioMT));
            this.Fields.Add("$BALISE0612", text);

            // Periodicité de fonction
            text = new XText(String.Format("{0} s", PegaseData.Instance.GestionTemporisation.DureeAlarmeHommeMort));
            this.Fields.Add("$BALISE0613", text);

            // Fonction pré-alarme
            text = new XText(String.Format("{0} s", PegaseData.Instance.GestionTemporisation.DelaiAlarmeHommeMort));
            this.Fields.Add("$BALISE0614", text);

            // Active sur le mode
            String t = "";
            Int32 i = 1;
            foreach (var mode in PegaseData.Instance.OLogiciels.ModesExploitation)
            {
                if (mode.MaskHMDispo)
                {
                    if (t == "")
                    {
                        t += String.Format("mode {0}", i);
                    }
                    else
                    {
                        t += String.Format(", mode {0}", i);
                    }
                }
                i++;
            }
            text = new XText(t);
            this.Fields.Add("$BALISE0615", text);

            // Mot de passe Setup
            if (PegaseData.Instance.PassWordConfig > 0)
            {
                text = new XText(String.Format("{0:0000}", PegaseData.Instance.PassWordConfig)); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0616", text);

            // Mot de passe association
            if (PegaseData.Instance.PassWordAssociation > 0)
            {
                text = new XText(String.Format("{0:0000}", PegaseData.Instance.PassWordAssociation)); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0617", text);

            // Mot de passe Start ++
            if (PegaseData.Instance.PassWordStartPP > 0)
            {
                text = new XText(String.Format("{0:0000}", PegaseData.Instance.PassWordStartPP)); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0618", text);

            // Mot de passe Start
            if (PegaseData.Instance.PassWordStart > 0)
            {
                text = new XText(String.Format("{0:0000}", PegaseData.Instance.PassWordStart)); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0619", text);

            // Homme mort automatique
            VE = GestionVariableSimple.Instance.GetVariableByName("Accelerometre");
            if ((VE != null) && (Convert.ToInt32(VE.RefElementValue).Equals(1)))
            {
                VE = GestionVariableSimple.Instance.GetVariableByName("Seuil");
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Seuil", "Anglais"));
                text = new XText(LCM.GetLabel(VE.RefElementValue));
            }
            else
            {
                text = new XText("");
            }
            this.Fields.Add("$BALISE0620", text);

            // Substitut radio
            VE = GestionVariableSimple.Instance.GetVariableByName("SubstituRadioRS485");
            if ((VE != null) && (Convert.ToInt32(VE.RefElementValue).Equals(1)))
            {
                VE = GestionVariableSimple.Instance.GetVariableByName("Active");
                LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Active", "Anglais"));
                text = new XText(LCM.GetLabel(VE.RefElementValue));
            }
            else
            {
                text = new XText("");
            }
            this.Fields.Add("$BALISE0621", text);

            // Anti stumming delay
            

            VE = GestionVariableSimple.Instance.GetVariableByName("MasqueBoutons");
            UInt32 MaskAntiStumming;
            try
            {
                if (!VE.RefElementValue.Contains("0x"))
                {
                    MaskAntiStumming = Convert.ToUInt32(VE.RefElementValue);
                }
                else
                {
                    MaskAntiStumming = Convert.ToUInt32(VE.RefElementValue.Substring(2), 16);
                }
            }
            catch
            {
                MaskAntiStumming = 0xFFFFFFFF;
            }
            
            if (MaskAntiStumming != 0xFFFFFFFF)
            {
                VariableEditable VAsD = GestionVariableSimple.Instance.GetVariableByName("DureeReposMin");
                text = new XText(VAsD.RefElementValue);
            }
            this.Fields.Add("$BALISE0622", text);

            // Anti stumming
            
            if (MaskAntiStumming == 0xFFFFFFFF)
            {
                text = new XText("off");
            }
            else
            {
                text = new XText("on");
            }

            this.Fields.Add("$BALISE0623", text);

            // Synchronized control mode
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ModeCouplage", "Anglais"));
            text = new XText(LCM.GetLabel(PegaseData.Instance.CouplageMTs.ModeCouplage.ToString()));
            this.Fields.Add("$BALISE0624", text);

            // Operating mode
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_Master", "Anglais"));
            text = new XText(LCM.GetLabel(PegaseData.Instance.CouplageMTs.MasterSlave.ToString()));
            this.Fields.Add("$BALISE0625", text);

            // Channel listening period
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_PeriodeEcouteCanal", "Anglais"));
            text = new XText(LCM.GetLabel(PegaseData.Instance.CouplageMTs.PeriodeEcouteCanal.ToString()));
            this.Fields.Add("$BALISE0626", text);

            // Release mode
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_ModeLibCouplage", "Anglais"));
            text = new XText(LCM.GetLabel(PegaseData.Instance.CouplageMTs.ModeLibCouplage.ToString()));
            this.Fields.Add("$BALISE0627", text);

            // Solo modes number
            text = new XText(PegaseData.Instance.CouplageMTs.NbModeSolo.ToString());
            this.Fields.Add("$BALISE0628", text);

            // ID MT1
            if (PegaseData.Instance.CouplageMTs.MTCouplage.Count > 0)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MTCouplage[0].CodeNatifMT); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0629", text);

            // ID MT1
            if (PegaseData.Instance.CouplageMTs.MTCouplage.Count > 0)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MTCouplage[0].CanalRadioMT.ToString()); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0630", text);

            // ID MT2
            if (PegaseData.Instance.CouplageMTs.MTCouplage.Count > 1)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MTCouplage[1].CodeNatifMT); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0631", text);

            // Channel MT2
            if (PegaseData.Instance.CouplageMTs.MTCouplage.Count > 1)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MTCouplage[1].CanalRadioMT.ToString()); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0632", text);

            // ID MO1
            if (PegaseData.Instance.CouplageMTs.MOCouplage.Count > 0)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MOCouplage[0].CodeNatifMO); 
            }
            else
            {
                text = new XText("-");
            }

            this.Fields.Add("$BALISE0633", text);

            // ID MO2
            if (PegaseData.Instance.CouplageMTs.MOCouplage.Count > 1)
            {
                text = new XText(PegaseData.Instance.CouplageMTs.MOCouplage[1].CodeNatifMO); 
            }
            else
            {
                text = new XText("-");
            }
            this.Fields.Add("$BALISE0634", text);



            // Switch control
            String XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionFonctionCle/OrganeMO", "", "", XML_ATTRIBUTE.VALUE);
            text = new XText("-");
            text.Value = XValue;
            this.Fields.Add("$BALISE0635", text);

            // Locking position
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionFonctionCle/PositionVerrou", "", "", XML_ATTRIBUTE.VALUE);
            text = new XText("-");
            text.Value = "Pos_0" + XValue;
            this.Fields.Add("$BALISE0636", text);

            // Number of modes
            text = new XText(PegaseData.Instance.OLogiciels.NbModes.ToString());
            this.Fields.Add("$BALISE0637", text);

            // Navigation control
            t = "";
            if (PegaseData.Instance.ParamHorsMode.NavigationInc != null && PegaseData.Instance.ParamHorsMode.NavigationInc != "")
            {
                t = PegaseData.Instance.ParamHorsMode.NavigationInc;
            }

            if (PegaseData.Instance.ParamHorsMode.NavigationDec != null && PegaseData.Instance.ParamHorsMode.NavigationDec != "")
            {
                if (t != "")
                {
                    t = t + ", " + PegaseData.Instance.ParamHorsMode.NavigationDec;
                }
                else
                {
                    t = PegaseData.Instance.ParamHorsMode.NavigationDec;
                }
            }

            text = new XText(t);
            this.Fields.Add("$BALISE0638", text);

            // Accept change mode
            String listChoix = LanguageSupport.Get().GetToolTipRapport("Plage_ValiderChangementT", "English");
            if (listChoix == null)
            {
                listChoix = "/No=0/Yes=1/";
            }
            LCM = new ListChoixManager(listChoix);
            if (PegaseData.Instance.ParamHorsMode.ValiderChangement)
            {
                t = LCM.GetLabel("1");
            }
            else
            {
                t = LCM.GetLabel("0");
            }

            text = new XText(t);
            this.Fields.Add("$BALISE0639", text);

            // TM behavior
            listChoix = LanguageSupport.Get().GetToolTipRapport("Plage_ComportementMT", "English");
            if (listChoix == null)
            {
                listChoix = "/No effect=-1/Output in safety=0/";
            }
            LCM = new ListChoixManager(listChoix);
            if (PegaseData.Instance.ParamHorsMode.SortieSecu)
            {
                t = LCM.GetLabel("0");
            }
            else
            {
                t = LCM.GetLabel("-1");
            }

            text = new XText(t);
            this.Fields.Add("$BALISE0640", text);

            // ------------  Network  ----------
            // ModBUS activation
            // Modbus speed
            VE = GestionVariableSimple.Instance.GetVariableByName("MB_Vitesse");
            if (VE != null)
            {
                text = new XText(VE.AutorizedValues.GetLabel(VE.RefElementValue));
                this.Fields.Add("$BALISE0351", text);
            }

            // RS485 modbus parity
            VE = GestionVariableSimple.Instance.GetVariableByName("MB_Parite");

            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_MB_Parite", "Anglais"));
            if (VE != null && LCM != null)
            {
                text = new XText(LCM.GetLabel(VE.RefElementValue));
                //text = new XText(VE.AutorizedValues.GetLabel(VE.RefElementValue));
                this.Fields.Add("$BALISE0352", text);
            }

            // Modbus address
            VE = GestionVariableSimple.Instance.GetVariableByName("MB_adresse");
            if (VE != null)
            {
                text = new XText(VE.RefElementValue);
                this.Fields.Add("$BALISE0353", text);
            }

            // Modbus adress offset
            VE = GestionVariableSimple.Instance.GetVariableByName("OffsetModbus");
            if (VE != null)
            {
                text = new XText(VE.RefElementValue);
                this.Fields.Add("$BALISE0354", text);
            }

            // CanOpen activation
            VE = GestionVariableSimple.Instance.GetVariableByName("CanOpen");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_CanOpen", "Anglais"));
            if (VE != null && LCM != null)
            {
                text = new XText(LCM.GetLabel(VE.RefElementValue));
                //text = new XText(VE.AutorizedValues.GetLabel(VE.RefElementValue));
            }

            // CanOpen Baudrate
            VE = GestionVariableSimple.Instance.GetVariableByName("COVitesse");
            if (VE != null)
            {
                text = new XText(VE.AutorizedValues.GetLabel(VE.RefElementValue));
                this.Fields.Add("$BALISE0356", text);
            }

            // CanOpen address bus tranceiver
            VE = GestionVariableSimple.Instance.GetVariableByName("COAdresse");
            if (VE != null)
            {
                text = new XText(GestionVariableSimple.Instance.GetVariableByName("COAdresse").RefElementValue);
                this.Fields.Add("$BALISE0357", text);
            }

            // Output network on secure
            VE = GestionVariableSimple.Instance.GetVariableByName("SecuSortieModbus");
            LCM = new ListChoixManager(LanguageSupport.Get().GetToolTipRapport("XMLTranslation/Plage_SecuSortieModbus", "Anglais"));
            if (VE != null)
            {
                text = new XText(LCM.GetLabel(VE.RefElementValue));
                //text = new XText(VE.AutorizedValues.GetLabel(VE.RefElementValue));
                this.Fields.Add("$BALISE0358", text);
            }

        } // endMethod: BuildParamSimpleData
        
        /// <summary>
        /// Construire le tableau 'Utilisation et Fonctionnement de la solution applicative'
        /// </summary>
        public void BuildApplicativeSolutionTable ( )
        {
            XElement div = new XElement("div");
            XAttribute Attrib = new XAttribute("id", "OutputbyOrg");
            div.Add(Attrib);
            ObservableCollection<XElement> tables = new ObservableCollection<XElement>();
            ObservableCollection<XElement> tds = new ObservableCollection<XElement>();
            String Value;
            String classe;

            Int32 offset = 0, nbCol = 0, numOrgane, numMode, numEtat;

            for (offset = 0; offset < EasyConfigData.Get().Modes.Count; offset += 4)
            {
                XElement table = HTMLHelper.CreateTable(0, 0, 248, "border-collapse:collapse;table-layout:fixed;width:186pt");

                // Fabrication des colonnes
                table = HTMLHelper.AddColumn(table, 80, 1, "width:60pt");
                table = HTMLHelper.AddColumn(table, 80, 1, "width:60pt");
                for (nbCol = offset; nbCol - offset < 5 && nbCol < EasyConfigData.Get().Modes.Count; nbCol++)
                {
                    table = HTMLHelper.AddColumn(table, 80, 1, "width:60pt");
                    table = HTMLHelper.AddColumn(table, 8, 1, "mso-width-source:userset;mso-width-alt:292;width:6pt"); 
                }

                // Fabrication des lignes

                // En-tête
                tds.Clear();
                tds.Add(HTMLHelper.CreateTd(21, "xl6323777", 80, "height:15.75pt;width:60pt", " "));
                tds.Add(HTMLHelper.CreateTd(21, "xl6323777", 80, "height:15.75pt;width:60pt", " "));

                for (int i = 0; i < EasyConfigData.Get().Modes.Count - offset && i < 4; i++)
                {
                    tds.Add(HTMLHelper.CreateTd(0, "xl6323777", 80, "width:60pt", this.BuildSelecteurLabel(EasyConfigData.Get().Modes[offset + i].RefMode.ModeLabel)));
                    tds.Add(HTMLHelper.CreateTd(0, "xl6323777", 8, "border-left:none;width:6pt", " "));
                }

                table = HTMLHelper.AddTr(table, 21, "height:15.75pt", tds);
                
                // Lignes
                for (numOrgane = 0; numOrgane < EasyConfigData.Get().Modes[0].Effecteurs.Count; numOrgane++)
                {
                    for (numEtat = 0; numEtat < EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Positions.Count; numEtat++)
                    {
                        tds.Clear();
                        
                        // En tête de ligne
                        if (numEtat == 0)
	                    {
                            if (EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Organ != null)
                            {
                                Value = EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Organ.MnemoHardOrganeMO; 
                            }
                            else
                            {
                                Value = EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Name;
                            }
	                    }
                        else
	                    {
                            Value = " ";
	                    }

                        if (numEtat == EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Positions.Count - 1)
                        {
                            classe = "xl638216";
                        }
                        else
                        {
                            classe = "xl638215";
                        }

                        tds.Add(HTMLHelper.CreateTd(20, classe, 80, "height:15.0pt;width:60pt", Value));

                        // Classe

                        if (numEtat == EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Positions.Count - 1)
                        {
                            classe = "xl638214";
                        }
                        else
                        {
                            classe = "xl638213";
                        }

                        // Etat
                        Value = EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Positions[numEtat].Name;
                        tds.Add(HTMLHelper.CreateTd(20, classe, 80, "height:15.0pt;width:60pt", Value));

                        for (numMode = 0; numMode < EasyConfigData.Get().Modes.Count - offset && numMode < 4; numMode++)
                        {
                            Value = "";
                            EC_Mode CurrentMode = EasyConfigData.Get().Modes[numMode + offset];

                            var queryEffecteur = from eff in CurrentMode.Effecteurs
                                                 where eff.Name == EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Name
                                                 select eff;
                            if (queryEffecteur.Count() > 0)
                            {
                                EC_Effecteur CurrentEffecteur = queryEffecteur.First();

                                var queryPosition = from pos in CurrentEffecteur.Positions
                                                    where pos.Name == EasyConfigData.Get().Modes[0].Effecteurs[numOrgane].Positions[numEtat].Name
                                                    select pos;

                                if (queryPosition.Count() > 0)
                                {
                                    EC_Position CurrentPositions = queryPosition.First();

                                    for (int i = 0; i < CurrentPositions.Outputs.Count; i++)
                                    {
                                        if (CurrentPositions.Outputs[i].IsUsed)
                                        {
                                            String outputname = "";
                                            if (CurrentPositions.Outputs[i].STOR != null)
                                            {
                                                outputname = " " + CurrentPositions.Outputs[i].STOR.MnemoHardware;
                                            }
                                            Value += outputname;
                                        }
                                    }
                                }
                            }
                            tds.Add(HTMLHelper.CreateTd(20, classe, 80, "border-left:none;width:60pt", Value));
                            tds.Add(HTMLHelper.CreateTd(20, classe, 8, "border-left:none;width:6pt", " "));
                        }

                        table = HTMLHelper.AddTr(table, 20, "height:15.0pt", tds);
                    }
                }

                // Ajouter la table à la collection
                tables.Add(table);
            }

            // Ajouter l'ensemble des tables à la section
            foreach (XElement table in tables)
            {
                div.Add(table);
            }
            this.Fields.Add("$BALISE5000", div);

        } // endMethod: BuildApplicativeSolutionTable

        public struct imagePlastron
        {
            public XElement pos;
            public OrganCommand organ;
        }
        #endregion
        ObservableCollection<imagePlastron> DicoImage(ObservableCollection<OrganCommand> list,bool zn1,bool zn2,bool zn3)
        {
            ObservableCollection<imagePlastron> image = new ObservableCollection<imagePlastron>();
            FileCore.FilePackage ParamData = FileCore.FilePackage.GetParamData();
            //MessageBox.Show("1");
            if (ParamData != null)
            {
                Stream stream = ParamData.GetPartStream(new Uri(DefaultValues.Get().ConfigMOFileName, UriKind.Relative));
                //MessageBox.Show("2");
                XDocument doc = XDocument.Load(stream);
                stream.Close();
                ParamData.ClosePackage();
                bool Ispika = PegaseData.Instance.MOperateur.RefIndus.Substring(0, 1).Equals("P");
                //MessageBox.Show("3");
                // 2 - parser l'intégralité du fichier
                foreach (XElement refbase in doc.Root.Descendants("Refbase"))
                {
                    foreach (XElement produit in refbase.Descendants("Produit"))
                    {
                        String code = produit.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value;
                        if (code == PegaseData.Instance.MOperateur.RefIndus.Substring(0, 2))
                        {
                            foreach (XElement position in produit.Descendants("position"))
                            {
                                string  pos = position.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                                string lib = position.Attribute(XMLCore.XML_ATTRIBUTE.LIBELLE).Value;
                                string zone = "";
                                if (position.Attribute(XMLCore.XML_ATTRIBUTE.ZONE) != null)
                                {
                                    zone = position.Attribute(XMLCore.XML_ATTRIBUTE.ZONE).Value;
                                }
                                bool valizone = false;
                                if (zone.Equals("ZN1") && zn1.Equals(true))
                                {
                                    valizone = true;
                                }
                                else if (zone.Equals("ZN2") && zn2.Equals(true))
                                {
                                    valizone = true;
                                }
                                else if (zone.Equals("ZN3") && zn3.Equals(true))
                                {
                                    valizone = true;
                                }
                                else if (zone.Equals(""))
                                {
                                    valizone = true;
                                }
                                foreach (OrganCommand org in list)
                                {
                                    if ((lib == (org.NomOrganeMO) && ((valizone == true) || (Ispika == false)))||
                                            (((pos == "POS_J1") && (org.NomOrganeMO == "AXIS1")) ||
                                             ((pos == "POS_J2") && (org.NomOrganeMO == "AXIS3")) ||
                                             ((pos == "POS_J3") && (org.NomOrganeMO == "AXIS5")) ||
                                             ((pos == "POSITION_B1") && (org.NomOrganeMO == "AXIS1")) ||
                                             ((pos == "POSITION_B2") && (org.NomOrganeMO == "AXIS2")) ||
                                             ((pos == "POSITION_B3") && (org.NomOrganeMO == "AXIS3")) ||
                                             ((pos == "POSITION_B4") && (org.NomOrganeMO == "AXIS4")) ||
                                             ((pos == "POSITION_B5") && (org.NomOrganeMO == "AXIS5")) ||
                                             ((pos == "POSITION_B6") && (org.NomOrganeMO == "AXIS6")) ))
                                    {
                                        foreach (XElement reference in position.Descendants("organe"))
                                        {
                                            string ref1 = reference.Attribute(XMLCore.XML_ATTRIBUTE.REFERENCE).Value;
                                            if (ref1 == org.ReferenceOrgan)
                                            {
                                                imagePlastron imagep = new imagePlastron();
                                                imagep.pos = reference;
                                                imagep.organ = org;
                                                image.Add(imagep);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                           
                        }
                    }
                }
            }
            return image;
        }
        // Messages
        #region Messages

        #endregion

    } // endClass: RapportData
}
