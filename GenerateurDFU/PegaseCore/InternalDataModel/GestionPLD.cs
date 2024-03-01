using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class GestionPLD
    {
        JAY.PegaseCore.AnalyseEquation _analyse;
        public JAY.PegaseCore.AnalyseEquation Analyse
        {
            get
            {
                if (_analyse == null)
                {
                    _analyse = new AnalyseEquation();
                }
                return _analyse;
            }
            set
            {
                _analyse = value; 
            }
        }
        private int _pLDLevel = 0;
        public int PLDLevel
        {
            get
            {
                return _pLDLevel;
            }
        }


        public void ConversionPLDLevel()
        {
            
            string result;
            String value = XamlWriter.Save(PegaseData.Instance.ParamHorsMode.EquationTxt);
            Analyse = new AnalyseEquation();
            Analyse.RechercheCommentaire(PegaseData.Instance.ParamHorsMode.EquationTxt, out result);
            List<AnalyseEquation.EquationBymode> list = Analyse.AnalyseEquationByModeByFonction(ref result);

            if (Analyse.PLDConfig == null ||Analyse.PLDConfig.Count == 0)
            {
                _pLDLevel = 0;
            }
            // equation pld sur produit avec boutons secu
            else if (PegaseData.Instance.MOperateur.PresenceSafetyButton > 0)
            {
                var query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes where organ.NomOrganeMO == "A14" select organ;

                if (query != null && query.Count() > 0)
                {
                    OrganCommand BPSecu = query.First();
                    if (BPSecu.ReferenceOrgan == "D31061A")
                    {
                        _pLDLevel = 3;
                    }
                    else if (BPSecu.ReferenceOrgan == "D31062A")
                    {
                        _pLDLevel = 2;
                    }
                    else
                    {
                        _pLDLevel =0;
                    }
                    
                }
            }
            // equation pld sur produit sans boutons
            else
            {
                _pLDLevel = 1;
            }
        }

        public void Save()
        {
            // sauvegarde des ligne de config PLD
            ObservableCollection<XElement> DicoConfigPLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/DicoConfigPLD/");
            string offsetAbsolu = "";
            string offsetRelatif = "";
            if (DicoConfigPLD != null)
            {
                if (LutteAntiGFI.Instance.FicheExtension == TypeFiche.TYPE0)
                {
                    LutteAntiGFI.Instance.FicheExtension = TypeFiche.TYPE1;
                }
            }
            if (LutteAntiGFI.Instance.FicheExtension != TypeFiche.TYPE0)
            {
                if (LutteAntiGFI.Instance.EtatTypeFiche == TypeFiche.TYPE0)
                {
                    //LutteAntiGFI.Instance.EtatTypeFiche = TypeFiche.TYPE1;
                }
                if (DicoConfigPLD != null && DicoConfigPLD.Count > 0)
                {
                    foreach (var elem in DicoConfigPLD)
                    {
                        if (elem.Attribute(JAY.XMLCore.XML_ATTRIBUTE.CODE).Value == "DicoConfigPLD")
                        {
                            offsetAbsolu = elem.Attribute("offsetAbsolu").Value;
                            offsetRelatif = elem.Attribute("offsetRelatif").Value;
                            break;
                        }
                    }

                    UInt32 offsetAbsoluint = Convert.ToUInt32(offsetAbsolu);
                    UInt32 offsetRelatifint = Convert.ToUInt32(offsetRelatif);
                    ObservableCollection<XElement> lignePLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/DicoConfigPLD/");
                    lignePLD.First().Elements().Remove();
                    int cpt_pld = 0;
                    foreach (var lignepld in Analyse.PLDConfig)
                    {
                        XElement XCmdPLD = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateConfigCmdPLD);
                        JAY.XMLCore.XMLProcessing XPLD = new JAY.XMLCore.XMLProcessing();
                        XPLD.OpenXML(XCmdPLD);
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = lignepld.Buttons.ToString();
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = lignepld.Modes.ToString();
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = (offsetAbsoluint + 4).ToString();
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = (offsetRelatifint + 4).ToString();
                        XPLD.RootNode.Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                        XPLD.RootNode.Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                        lignePLD[0].Add(XPLD.RootNode);
                        offsetAbsoluint += 8;
                        offsetRelatifint += 8;
                        cpt_pld++;
                    }
                    for (; cpt_pld < 48; cpt_pld++)
                    {
                        XElement XCmdPLD = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateConfigCmdPLD);
                        JAY.XMLCore.XMLProcessing XPLD = new JAY.XMLCore.XMLProcessing();
                        XPLD.OpenXML(XCmdPLD);
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = "0xFFFFFFFF";
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = "0xFFFFFFFF";
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                        XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = (offsetAbsoluint + 4).ToString();
                        XPLD.GetNodeByPath("MasqueModes").First().Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = (offsetRelatifint + 4).ToString();
                        XPLD.RootNode.Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                        XPLD.RootNode.Attribute(JAY.XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                        lignePLD[0].Add(XPLD.RootNode);
                        offsetAbsoluint += 8;
                        offsetRelatifint += 8;
                    }
                }
                try
                {
                    XElement ligneNivSecuPLDPLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/TypeDeSecurite/NivSecu/NivSecuPLD/").First();
                   // var query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes where organ.NomOrganeMO == "A14" select organ;
                    ligneNivSecuPLDPLD.Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = PLDLevel.ToString();
                    XElement variableXml = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/GestionTypeFiche/TypeFiche/").First();
                    if (variableXml != null)
                    {
                        if ((LutteAntiGFI.Instance.EtatTypeFiche == TypeFiche.TYPE0)&& PLDLevel != 0)
                        {
                            LutteAntiGFI.Instance.EtatTypeFiche = TypeFiche.TYPE1;
                        }
                        if ((variableXml.Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value == "0") && PLDLevel != 0)
                        {
                            variableXml.Attribute(JAY.XMLCore.XML_ATTRIBUTE.VALUE).Value = "1";
                        }
                    }                        
                }
                catch { }
            }
        }
    }
}
