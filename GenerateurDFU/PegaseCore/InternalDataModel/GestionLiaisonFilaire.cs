using JAY.XMLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class GestionLiaisonFilaire
    {
        public GestionLiaisonFilaire()
        {

            this.ChoixLiaisonFilaire = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionSubstituRadioRS485/Active", "", "", XML_ATTRIBUTE.VALUE);

        }
        List<int> _ListLiaisonFilaireAutorise;
        public List<int> ListLiaisonFilaireAutorise
        {
            get
            {
                int ModbusActif = 0;
                int ModeCouplage = 0;
                int ModeInfraRouge = 0;
                if (_ListLiaisonFilaireAutorise != null)
                {
                    _ListLiaisonFilaireAutorise.Clear();
                }
                else
                {
                    _ListLiaisonFilaireAutorise = new List<int>();
                }
             
                String XValue1 = PegaseData.Instance.GestionModBus.ChoixModBus;
                int XValue2 = PegaseData.Instance.CouplageMTs.ModeCouplage;
                int ModeAssoCouplagePandC = PegaseData.Instance.CouplageMTs.NbModeAssoCouplagePCTRL;
                String XValue3 = PegaseData.Instance.GestionInfraRouge.ChoixInfraRouge;
                
                if (XValue1 != null && XValue1 != "")
                {
                    ModbusActif = Tools.ConvertASCIIToInt32(XValue1);
                }
                else
                {
                    ModbusActif = 0;
                }

                    ModeCouplage = XValue2;

                if (XValue3 != null && XValue3 != "")
                {
                    ModeInfraRouge = Tools.ConvertASCIIToInt32(XValue3);
                }
                else
                {
                    ModeInfraRouge = 0;
                }

                _ListLiaisonFilaireAutorise.Add(0);

                if ((ModbusActif != 1) && (ModbusActif != 3) && ((ModeCouplage == 0) || (ModeCouplage == 5)) && (ModeInfraRouge != 3) && (ModeInfraRouge != 5))
                {
                    _ListLiaisonFilaireAutorise.Add(1);
                }
                _ListLiaisonFilaireAutorise.Sort();
                return _ListLiaisonFilaireAutorise;
            }
            set
            {
                _ListLiaisonFilaireAutorise = value;
            }
        }
        public string ChoixLiaisonFilaire
        {
            get;
            set;
        }

        public void Save()
        {
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionSubstituRadioRS485/Active", "", "", XML_ATTRIBUTE.VALUE, this.ChoixLiaisonFilaire);
        }
    }
}
