using JAY.PegaseCore.InternalDataModel;
using JAY.XMLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class GestionModbus
    {
        public GestionModbus()
        {

             this.ChoixModBus = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigReseau/IdentBus/ModBus", "", "", XML_ATTRIBUTE.VALUE);
           
        }
            List<int> _ListModBusAutorise;
        public List<int> ListModBusAutorise
        {
            get
            {
                int liaionactive = 0;
                if (_ListModBusAutorise != null)
                {
                    _ListModBusAutorise.Clear();
                }
                else
                {
                    _ListModBusAutorise = new List<int>();
                }
                String XValue = PegaseData.Instance.GestionLiaisonFilaire.ChoixLiaisonFilaire;
                if (XValue != null && XValue != "")
                {
                    liaionactive = Tools.ConvertASCIIToInt32(XValue);
                }
                else
                {
                    liaionactive = 0;
                }
                _ListModBusAutorise.Add(0);
                _ListModBusAutorise.Add(2);
                if (liaionactive == 0)
                {
                    _ListModBusAutorise.Add(1);
                    _ListModBusAutorise.Add(3);
                }
                _ListModBusAutorise.Sort();
                return _ListModBusAutorise;
            }
            set
            {
                _ListModBusAutorise = value;
            }
        }

        public string ChoixModBus
        {
            get;set;
        }
        

        public void  Save()
        {
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigReseau/IdentBus/ModBus", "", "", XML_ATTRIBUTE.VALUE, this.ChoixModBus);
        }
    }
}
