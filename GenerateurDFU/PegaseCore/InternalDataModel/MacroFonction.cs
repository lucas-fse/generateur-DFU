using JAY.FileCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class MacroFonction
    {
    public String Name
        {
            get; set;
        }
        public String Prototype
        {
            get;set;
        }
        public String Analyse
        {
            get; set;
        }
        public ObservableCollection<string> Equation
        {
            get; set;
        }
        public int NbParams
        { get; set; }

        public Dictionary<int, string> Params_eq
        {get;set;}

        public String AliasTexte
        { get; set; }

        public bool ModeInterprete
        { get; set; }
        public string SectionValid
        { get; set; }
    }
}