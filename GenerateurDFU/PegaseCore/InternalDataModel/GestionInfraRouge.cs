using JAY.XMLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class GestionInfraRouge
    {
        public GestionInfraRouge()
        {

            this.ChoixInfraRouge = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.VALUE);

        }
        List<int> _ListInfraRougeAutorise;
        public List<int> ListInfraRougeAutorise
        {
            get
            {
                int ModeLiaisonFilaire = 0;
                int ModePickAndCtrl = 0;
                int ModePickAndCtrlInput = 0;
                if (_ListInfraRougeAutorise != null)
                {
                    _ListInfraRougeAutorise.Clear();
                }
                else
                {
                    _ListInfraRougeAutorise = new List<int>();
                }
            
                String XValue1 = PegaseData.Instance.GestionLiaisonFilaire.ChoixLiaisonFilaire;
                if (XValue1 != null && XValue1 != "")
                {
                    ModeLiaisonFilaire = Tools.ConvertASCIIToInt32(XValue1);
                }
                else
                {
                    ModeLiaisonFilaire = 0;
                }
                ModePickAndCtrl = PegaseData.Instance.CouplageMTs.ModeCouplage;
                ModePickAndCtrlInput = PegaseData.Instance.CouplageMTs.NbModeAssoCouplagePCTRL;

                _ListInfraRougeAutorise.Add(0);
                if (!((ModePickAndCtrl == 6) || (ModePickAndCtrl == 7)|| (ModePickAndCtrl == 8)))
                {
                    _ListInfraRougeAutorise.Add(1);
                    _ListInfraRougeAutorise.Add(2);
                    _ListInfraRougeAutorise.Add(4);


                    if (ModeLiaisonFilaire == 0)
                    {
                        _ListInfraRougeAutorise.Add(3);
                        _ListInfraRougeAutorise.Add(5);
                    }
                }
                _ListInfraRougeAutorise.Sort();
                return _ListInfraRougeAutorise;
            }
            set
            {
                _ListInfraRougeAutorise = value;
            }
        }
        public string ChoixInfraRouge
        {
            get; set;
        }

        public void Save()
        {
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.VALUE, this.ChoixInfraRouge);
        }
    }
}
