using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    // Données correspondant à la carte SIM présente dans la fiche
    public class SIM
    {
        // Variables
        #region Variables

        private XMLCore.XMLProcessing _xmlProcessing;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le canal demandé
        /// </summary>
        public String CanalDemande
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                if (Result == null)
                {
                    Result = "";
                }

                return Result;
            }
        } // endProperty: CanalDemande

        /// <summary>
        /// Le numéro de série de la SIM du système
        /// </summary>
        public String NumeroDeSerie
        {
            get
            {
                String Result = "";
                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesDeFab/NumeroDeSerie");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    Result = element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    Result = XMLCore.Tools.ConvertFromASCII2Text(Result);
                }
                return Result;
            }
            set
            {
                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesDeFab/NumeroDeSerie");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = value;
                }
            }
        } // endProperty: NumeroDeSerie

        /// <summary>
        /// Date de fabrication de la sim
        /// </summary>
        public DateTime DateDeFabrication
        {
            get
            {
                DateTime Result = new DateTime();

                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesDeFab/DateDeFabrication");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    String DT = element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    Result = DateTime.Parse(DT);
                }

                return Result;
            }
            set
            {
                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesDeFab/DateDeFabrication");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = value.ToString("d/M/yy");
                }
            }
        } // endProperty: DateDeFabrication

        /// <summary>
        /// Référence industrielle de la Sim
        /// </summary>
        public String RefIndus
        {
            get
            {
                String Result = "";
                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesConfigMat/IdentERP/RefIndus");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    Result  = element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    Result = XMLCore.Tools.ConvertFromASCII2Text(Result);
                }

                return Result;
            }
            set
            {
                IEnumerable<XElement> Sims = this._xmlProcessing.GetNodeByPath("/SIM/DonneesConfigMat/IdentERP/RefIndus");

                if (Sims != null)
                {
                    XElement element = Sims.First();
                    element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = value;
                }
            }
        } // endProperty: RefIndus
        
        /// <summary>
        /// La valeur de l'option IR embarquée
        /// </summary>
        public Int32 OptionIR
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionIR", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionIR

        /// <summary>
        /// Option d'exploitation
        /// </summary>
        public Int32 OptionExploit
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionExploit", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionExploit
    
        /// <summary>
        /// Option Homme Mort
        /// </summary>
        public Int32 OptionHommeMort
        {
        get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionHommeMort", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionHommeMort

        /// <summary>
        /// Option Dati
        /// </summary>
        public Int32 OptionDati
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionDati", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionDati

        /// <summary>
        /// Option Couplage
        /// </summary>
        public Int32 OptionCouplage
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionCouplage", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionCouplage

        /// <summary>
        /// Option équations
        /// </summary>
        public Int32 OptionEquation
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionEquation", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionEquation

        /// <summary>
        /// Option Antipiannotage
        /// </summary>
        public Int32 OptionPiannotage
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionPiannotage", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionPiannotage

        /// <summary>
        /// Option Ev
        /// </summary>
        public Int32 OptionEv
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionEv", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionEv

        /// <summary>
        /// Option Cpt
        /// </summary>
        public Int32 OptionCpt
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionCpt", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionCpt

        /// <summary>
        /// Option Alto
        /// </summary>
        public Int32 OptionAlto
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionAlto", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionAlto

        /// <summary>
        /// Option Sup1
        /// </summary>
        public Int32 OptionSup1
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionSup1", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionSup1

        /// <summary>
        /// Option Sup2
        /// </summary>
        public Int32 OptionSup2
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionSup2", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionSup2

        /// <summary>
        /// Option Sup3
        /// </summary>
        public Int32 OptionSup3
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionSup3", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionSup3

        /// <summary>
        /// Option Sup4
        /// </summary>
        public Int32 OptionSup4
        {
            get
            {
                String SV = this._xmlProcessing.GetValue("/SIM/DonneesConfigMat/IdentERP/OptionSup4", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
                Int32 Result = PegaseData.ConvertString2Value(SV);

                return Result;
            }
        } // endProperty: OptionSup4

        #endregion

        // Constructeur
        #region Constructeur

        public SIM(XElement simData)
        {
            this._xmlProcessing = new XMLCore.XMLProcessing();
            this._xmlProcessing.OpenXML(simData);
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: SIM
}
