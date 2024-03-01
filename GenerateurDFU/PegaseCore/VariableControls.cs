using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAY.PegaseCore
{
    public class VariableControls
    {
        public Dictionary<string, string> Controles = new Dictionary<string, string>();
        public List<infoAffichage> Affichage = new List<infoAffichage>();
        public List<string> interpretations = new List<string>();       
    }
    public class infoAffichage
    {
        public string valeur = "";
        public string adresse = "";
        public string longueur = "";
        public string plage = "";
        public string module = "";
    }
}
