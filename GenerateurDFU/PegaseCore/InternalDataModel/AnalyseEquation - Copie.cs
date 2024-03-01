using JAY.PegaseCore;
using Pegase.CompilEquation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using System.Runtime.InteropServices;

using Pegase.CompilEquation.enums;
using Pegase.CompilEquation.structs;
using System.Collections.ObjectModel;

namespace JAY.PegaseCore
{
    public class AnalyseEquation
    {
        public struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;
            public Color couleur;
            public string tooltip;
        }
        public struct EquationBymode
        {
            public int mode;
            public string equation;
        }

        private List<Tag> baliseeq = new List<Tag>();

        private String BALISEFINMODEREGEX = @"((<\\Security_Mode>)|(<\\Universal_Mode>)|(<\\Mode_((0[1-9]{1})|(1[0-9]{1})|(2[0-9]{1})|(3[0-2]{1}))>))";
        private String BALISEMODEREGEX = @"((True)|(False)|(<\\?Security_Mode>)|(<\\?Universal_Mode>)|(<\\?Mode_((0[1-9]{1})|(1[0-9]{1})|(2[0-9]{1})|(3[0-2]{1}))>))";
        private String FINDEQUATION = @"(((.\s*)*)(?:<>))*(.\s*)*";

        private String COMMENTAIREREGEX = @"(?:/\*(?:[^*]|(?:\*+[^*/]))*\*+/)|(?:\/\/[^\r\n]*)"; //4 groupes

        private Dictionary<String, String> correspondance = new Dictionary<string, string>();

        private List<String> motInterdit = new List<string>();

        public AnalyseEquation()
        {
            string value;
           
            foreach (OrganCommand org in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                motInterdit.Add(org.Mnemologique);
                if (correspondance.TryGetValue(org.NomOrganeMO, out value))
                {

                }
                else
                {
                    this.correspondance.Add(org.NomOrganeMO, org.Mnemologique);
                }
            }

            foreach (ESAna org in PegaseData.Instance.ModuleT.EAnas)
            {
                motInterdit.Add(org.MnemoLogique);
                if (correspondance.TryGetValue(org.MnemoHardware, out value))
                {

                }
                else
                {
                    correspondance.Add(org.MnemoHardware, org.MnemoLogique);
                }
            }
            foreach (ESTOR org in PegaseData.Instance.ModuleT.ETORS)
            {
                motInterdit.Add(org.MnemoLogique);
                if (correspondance.TryGetValue(org.MnemoHardware, out value))
                {

                }
                else
                {
                    correspondance.Add(org.MnemoHardware, org.MnemoLogique);
                }
            }
            foreach (ESAna org in PegaseData.Instance.ModuleT.SAnas)
            {
                motInterdit.Add(org.MnemoLogique);
                if (correspondance.TryGetValue(org.MnemoHardware, out value))
                {

                }
                else
                {
                    correspondance.Add(org.MnemoHardware, org.MnemoLogique);
                }
            }
            foreach (ESTOR org in PegaseData.Instance.ModuleT.STORS)
            {
                motInterdit.Add(org.MnemoLogique);
                if (correspondance.TryGetValue(org.MnemoHardware, out value))
                {

                }
                else
                {
                    correspondance.Add(org.MnemoHardware, org.MnemoLogique);
                }
            }
            foreach (VariableE var in PegaseData.Instance.Variables)
            {
                motInterdit.Add(var.Name);
                if (correspondance.TryGetValue(var.UserName, out value))
                {

                }
                else
                {
                    correspondance.Add(var.UserName, var.Name);
                }
            }
        }

        public List<String> Findsugestion(string buffer)
        {
            List<string> result = new List<string>();
            if (!String.IsNullOrEmpty(buffer))
            foreach ( var suggestion in correspondance)
            {
                if ((suggestion.Key.IndexOf(buffer) == 0) && !suggestion.Key.Equals(buffer))
                {
                    result.Add(suggestion.Key);
                }
            }
            return result;
        }

        public Dictionary<String,String> Correspondance
        {
            get { return correspondance; } set { correspondance = value; }
        }
        public List<String> MotInterdit
        {
            get { return motInterdit; }
            set { motInterdit = value; }
        }
        public List<String> MacroName
        {
            get;set;
        }

        //private const String SWITCHREGEX = @"( *\/\/.*\s*)* *switch *\( *mode * \) *\/\/.*\s* *{(\\s|.)*}";
        /// anaylyse la strure de base d
        public List<Tag> AnalyseStructureEquation(RichTextBox textbox, ref String structuremodifié)
        {
            
            List<Tag> result = new List<Tag>();
            Tag old_tag = new Tag();
            Regex verifstruct = new Regex(BALISEMODEREGEX);
            TextPointer start = textbox.Document.ContentStart;
            String structuremodifié2 = structuremodifié;
            List<string> balise = new List<string>();
            if (verifstruct.IsMatch(structuremodifié))
            {
                // colorié en clair
                MatchCollection matches = verifstruct.Matches(structuremodifié);
                if (matches.Count != 0)
                {
                    foreach (Match m in matches)
                    {
                        string chaine = m.Groups[1].Value.ToString();

                        string chainetmp = structuremodifié.Substring(0, m.Groups[1].Index);
                        int lines = this.GetLinesCount(chainetmp, m.Groups[1].Index);
                        int linesmot = this.GetLinesCount(chaine, chaine.Length);
                        AnalyseEquation.Tag t = new AnalyseEquation.Tag();
                        t.StartPosition = start.GetPositionAtOffset(m.Groups[1].Index + lines + 2);
                        t.EndPosition = start.GetPositionAtOffset(m.Groups[1].Index + m.Groups[1].Length + lines + 2 + linesmot);
                        if ((t.EndPosition != null) && (t.StartPosition != null))
                        {
                            TextRange essai = new TextRange(t.StartPosition, t.EndPosition);
                            t.couleur = Colors.Red;
                            t.Word = chaine;
                            t.tooltip = "Non-compliant markup";

                            if ((t.Word != null) && (old_tag.Word != null) && (!balise.Contains(chaine)))
                            {
                                String tmp = old_tag.Word.Insert(1, "\\");
                                if (t.Word.Equals(tmp))
                                {
                                    result.Remove(result.Last());
                                }
                                else
                                {
                                    result.Add(t);
                                }
                                baliseeq.Add(t);
                                balise.Add(chaine);
                            }
                            else
                            {
                                result.Add(t);
                                baliseeq.Add(t);
                                balise.Add(chaine);
                            }
                        }
                        old_tag = t;
                    }
                }
            }
            structuremodifié = structuremodifié2;
            return result;

        }

        public List<Tag> RechercheKeyword(RichTextBox textbox, String structuremodifié)
        {
            //structuremodifié =  run.Text;
            List<Tag> result = new List<Tag>();
            Regex verifstruct = new Regex(BALISEMODEREGEX);
            TextPointer start = textbox.Document.ContentStart;
            if (verifstruct.IsMatch(structuremodifié))
            {
                // colorié en clair
                MatchCollection matches = verifstruct.Matches(structuremodifié);
                foreach (Match m in matches)
                {
                    string chaine = m.Groups[1].Value.ToString();
                    string chainetmp = structuremodifié.Substring(0, m.Groups[1].Index);
                    int lines = this.GetLinesCount(chainetmp, m.Groups[1].Index);
                    int linesmot = this.GetLinesCount(chaine, chaine.Length);
                    AnalyseEquation.Tag t = new AnalyseEquation.Tag();
                    t.StartPosition = start.GetPositionAtOffset(m.Groups[1].Index + lines + 2);
                    t.EndPosition = start.GetPositionAtOffset(m.Groups[1].Index + m.Groups[1].Length + lines + 2 + linesmot);
                    if ((t.EndPosition != null) && (t.StartPosition != null))
                    {
                        TextRange essai = new TextRange(t.StartPosition, t.EndPosition);
                        t.couleur = Colors.Blue;
                        t.Word = chaine;
                        result.Add(t);
                    }
                }
            }

            return result;
        }


        // public List<Tag> RechercheCommentaire(RichTextBox textbox, out String structuremodifié)
        public List<Tag> RechercheCommentaire(FlowDocument textbox, out String structuremodifié)
        {
            TextRange textRange = new TextRange(
                                     // TextPointer to the start of content in the RichTextBox.
                                     textbox.ContentStart,
                                     // TextPointer to the end of content in the RichTextBox.
                                     textbox.ContentEnd);
            structuremodifié = textRange.Text;
            string structuremodifié2 = structuremodifié;
            List<Tag> result = new List<Tag>();
            Regex verifstruct = new Regex(COMMENTAIREREGEX);
            TextPointer start = textbox.ContentStart;
            if (verifstruct.IsMatch(structuremodifié))
            {
                // colorié en vert
                MatchCollection matches = verifstruct.Matches(structuremodifié);
                foreach (Match m in matches)
                {
                    string chaine = m.ToString();
                    string chainetmp = structuremodifié.Substring(0, m.Groups[0].Index);
                    int lines = this.GetLinesCount(chainetmp, m.Groups[0].Index);
                    int linesmot = this.GetLinesCount(chaine, chaine.Length);
                    AnalyseEquation.Tag t = new AnalyseEquation.Tag();
                    t.StartPosition = start.GetPositionAtOffset(m.Groups[0].Index + lines + 2);
                    t.EndPosition = start.GetPositionAtOffset(m.Groups[0].Index + m.Groups[0].Length + lines + 2 + linesmot);
                    if ((t.EndPosition != null) && (t.StartPosition != null))
                    {
                        TextRange essai = new TextRange(t.StartPosition, t.EndPosition);
                        t.couleur = Colors.Green;
                        t.Word = chaine;
                        result.Add(t);
                    }
                    //String s = new String(' ', chaine.Length +  linesmot);

                    //var aStringBuilder = new StringBuilder(structuremodifié2);
                    //aStringBuilder.Remove(m.Groups[0].Index, m.Groups[0].Length);
                    //aStringBuilder.Insert(m.Groups[0].Index, s);
                    //structuremodifié2 = aStringBuilder.ToString();
                    for (int i = 0; i < m.Groups[0].Length; i++)
                    {
                        if ((structuremodifié2[m.Groups[0].Index+i] != '\r') && (structuremodifié2[m.Groups[0].Index+i] != '\n'))
                        {
                            structuremodifié2 = structuremodifié2.Remove(m.Groups[0].Index + i, 1);
                            structuremodifié2 = structuremodifié2.Insert(m.Groups[0].Index + i, " ");
                        }
                    }
                   // structuremodifié2 = aStringBuilder.ToString();
                }
            }
            structuremodifié = structuremodifié2;
            return result;
        }

        private int GetLinesCount(string str, int startPosition)
        {
            int lines =  str.Take(startPosition).Count(k => (k.Equals((char)13) || k.Equals((char)10)));

            // lines + 1 because if not, missing lines with searched tring
            return lines ;
        }
        Dictionary<int, int> offsetmot = new Dictionary<int, int>();
        public List<Tag> AnalyseEquationByMode(RichTextBox textbox, ref String structuremodifié)
        {
            TextRange textRange = new TextRange(
                                     // TextPointer to the start of content in the RichTextBox.
                                     textbox.Document.ContentStart,
                                     // TextPointer to the end of content in the RichTextBox.
                                     textbox.Document.ContentEnd);
            offsetmot.Clear();
            String structuremodifié2 = structuremodifié;
            TextPointer start = textbox.Document.ContentStart;
            List<Tag> result = new List<Tag>();
            Regex verifstruct = new Regex(BALISEMODEREGEX);
            int startindex = 0;
            int compteurEquation = 0;
            int CompteurEquationSize = 0;
            if (verifstruct.IsMatch(structuremodifié))
            {
                // colorié en clair
                // RechercheCommentaire des baslise l'ouverture fermeture a deja ete verifié
                MatchCollection matches = verifstruct.Matches(structuremodifié);
                foreach (Match m in matches)
                {
                    // recuperation de la section entre balise
                    string ZoneEquation = structuremodifié.Substring(startindex, m.Groups[1].Index- startindex);
                    //recuperation du numero de groupe
                    string Lemode = m.Groups[0].ToString();
                    // recuperation de l'ensemble des equations 
                    string[] substrings = Regex.Split(ZoneEquation, ";");

                    int compteurPosition = 0;
                    foreach (string match in substrings)
                    {

                        // verifier les equatin
                        Int32 LongueurProgramme;
                        ushort[] Programme;
                        Int32 Indice = 0;
                        Int32 Famille = 0;
                        offsetmot.Clear();
                        string eq_local = match.Replace("\r", " ");
                        eq_local = eq_local.Replace("\n", " ");
                        eq_local = eq_local.Replace("\t", " ");
                        // remplacement de noms par le meno hardware

                        // recherche et remplacement
                        Regex motentier = new Regex(@"([a-zA-Z0-9_]*)",RegexOptions.RightToLeft);
                        String eq_local_tmp = eq_local;
                        string replacement = "";
                        
                        Collection<string> resultequationsbrut = new Collection<string>();
                        List<int> Listerreur = new List<int>();
                        if (motentier.IsMatch(eq_local))
                        {
                            MatchCollection matchesMot = motentier.Matches(eq_local);

                            foreach (Match mots in matchesMot)
                            {
                                if (!String.IsNullOrEmpty(mots.Groups[1].Value))
                                {
                                    // correspondance des mot clé direct exemple bouton_01 pour F1
                                    if (Correspondance.TryGetValue(mots.Groups[1].Value, out replacement))
                                    {
                                        //eq_local_tmp = eq_local_tmp.Replace(mots.Groups[1].Value, replacement);
                                        eq_local_tmp = eq_local_tmp.Remove(mots.Groups[1].Index, mots.Groups[1].Length);
                                        eq_local_tmp = eq_local_tmp.Insert(mots.Groups[1].Index, replacement);
                                        offsetmot.Add(mots.Groups[1].Index, (mots.Groups[1].Length - replacement.Length));
                                    }
                                    // verification des mot interdit exemple bouton_01 
                                    else if (MotInterdit.Contains(mots.Groups[1].Value))
                                    {
                                        //eq_local_tmp = eq_local_tmp.Replace(mots.Groups[1].Value, "invalide");
                                        eq_local_tmp = eq_local_tmp.Remove(mots.Groups[1].Index, mots.Groups[1].Length);
                                        eq_local_tmp = eq_local_tmp.Insert(mots.Groups[1].Index, "invalide");
                                        offsetmot.Add(mots.Groups[1].Index, mots.Groups[1].Length - "invalide".Length);
                                    }
                                }
                            }
                            resultequationsbrut.Add(eq_local_tmp);
                            MatchCollection matchesMotMacro = motentier.Matches(eq_local_tmp);
                            // remplacement des proto de macro
                           
                            int Compteur_Macro = 0;
                            foreach (Match mots in matchesMotMacro)
                            {
                                if (PegaseData.Instance.ListMacroName.Contains(mots.Groups[1].Value))
                                {
                                    Compteur_Macro++;
                                    resultequationsbrut.Clear();
                                    // on verifi les parametres
                                  
                                   List<int> erreur = CheckParamatreMacro(ref eq_local_tmp, mots.Groups[1].Value);
                                    if (Compteur_Macro > 1)
                                    {
                                        // pas d'imbrication de macro
                                        Listerreur.Add(mots.Groups[1].Index + mots.Groups[1].Length);
                                        resultequationsbrut.Add(eq_local_tmp);
                                    }
                                    else if (erreur.Count > 0)
                                    {
                                        // erreur sur la liste des paramètres de la macro
                                        Listerreur.AddRange(erreur);
                                        resultequationsbrut.Add(eq_local_tmp);
                                    }
                                    else
                                    {
                                        // liste des equation a compiler
                                        resultequationsbrut = InterpreteurMacro(ref eq_local_tmp, mots.Groups[1].Value);
                                    }
                                    // il faut vérifier les paramètres de la macro
                                  }
                            }
                        }

                        // il faut creer une collection d'equation pour ensuite la compiler
                        //
                       //if (Listerreur.Count == 0)

                        for (int cp_eqt = 0; cp_eqt < resultequationsbrut.Count(); cp_eqt++)
                        {
                            string EquACompiler = resultequationsbrut.ElementAt(cp_eqt);
                            Pegase.CompilEquation.ResultatCompilEquation resultatcompil = DLL_Equation.TesterEquation(EquACompiler, ref Famille, ref Indice, out LongueurProgramme, out Programme);
                            if ((resultatcompil.Diagnostique != Pegase.CompilEquation.DiagnosticCompilEquation_e.EXPRESSION_CORRECTE) &&
                                (resultatcompil.Diagnostique != Pegase.CompilEquation.DiagnosticCompilEquation_e.EQUATION_VIDE))
                            {
                                // position l'erreur
                                int erreur = resultatcompil.Position;

                                if (((erreur > 0) && (erreur <= EquACompiler.Length)))
                                {
                                    // erreur est a la position erreur + compteurPosition
                                    // il faut trouver le mot qui est associé
                                    int erreur_orig = erreur;
                                    foreach (var cpt in offsetmot.Reverse())
                                    {
                                        if (erreur_orig < cpt.Key)
                                        {
                                            //erreur_orig
                                            break;
                                        }
                                        else
                                        {
                                            erreur_orig = erreur_orig + cpt.Value;
                                        }

                                    }

                                    int nb_ligne = GetLinesCount(structuremodifié2, startindex + compteurPosition + erreur_orig);
                                    AnalyseEquation.Tag t = new AnalyseEquation.Tag();
                                    t.EndPosition = start.GetPositionAtOffset(startindex + compteurPosition + nb_ligne + erreur_orig + 2);
                                    int mot = Findword(match, erreur_orig);
                                    t.StartPosition = start.GetPositionAtOffset(startindex + compteurPosition + nb_ligne + mot + 2);
                                    if ((t.EndPosition != null) && (t.StartPosition != null))
                                    {
                                        TextRange essai = new TextRange(t.StartPosition, t.EndPosition);
                                        t.couleur = Colors.Red;
                                        t.Word = "";
                                        t.tooltip = resultatcompil.Diagnostique.ToString();
                                        result.Add(t);
                                    }
                                }
                            }
                            //
                            if (cp_eqt == 0)
                            {
                                if (resultatcompil.Diagnostique != Pegase.CompilEquation.DiagnosticCompilEquation_e.EQUATION_VIDE)
                                {
                                    compteurEquation++;
                                    CompteurEquationSize = CompteurEquationSize + LongueurProgramme + 4;
                                }
                                compteurPosition = compteurPosition + match.Length + 1;
                            }
                        }// fin du for
                    }
                    startindex = m.Groups[1].Index + m.Groups[1].Length;
                }
            }
            structuremodifié = structuremodifié2;
            PegaseData.Instance.ParamHorsMode.NbEquations =(int) (Math.Max((float)compteurEquation * 100 / 256, (float)CompteurEquationSize * 100 / 5120) );
            PegaseData.Instance.ParamHorsMode.NbEquations32 = compteurEquation;
            PegaseData.Instance.ParamHorsMode.TailleEquations32 = CompteurEquationSize;
            return result;
        }

        private List<int> CheckParamatreMacro(ref string eq_local_tmp, string value)
        {
            MacroFonction proto = new MacroFonction();
            foreach (var macro in PegaseData.Instance.CollecMacro)
            {
                if (macro.Name.Equals(value))
                {
                    proto = macro;
                    break;
                }
            }
            Regex param = new Regex(proto.Analyse);
            MatchCollection matches = param.Matches(eq_local_tmp); // recherche de la macro et de ces parametres 
            Dictionary<int, string> parametreslu = new Dictionary<int, string>();
            List<int> ErreurParametres = new List<int>();
            //FAMILLE_INDEFINIE = 0,      
            //FAMILLE_BOUTON=1,
            //FAMILLE_SELECTEUR=2,
            //FAMILLE_COMMUTATEUR=3,
            //FAMILLE_COMMUTATEUR_A_12_POSITIONS4,
            //FAMILLE_AXE_ANALOGIQUE 5,
            //FAMILLE_AXE_A_CRAN 6,
            //FAMILLE_POTENTIOMETRE 7,
            //FAMILLE_ENTREE_TOUT_OU_RIEN 8,
            //FAMILLE_SORTIE_TOUT_OU_RIEN 9,
            //FAMILLE_ENTREE_ANALOGIQUE 10,
            //FAMILLE_SORTIE_ANALOGIQUE 11,
            //FAMILLE_RELAIS 12,
            //FAMILLE_VARIABLE_NUMERIQUE 13,
            //FAMILLE_VARIABLE_BOOLEENNE 14,
            //FAMILLE_VARIABLE_BOOLEENNE_BIS 15,
            try
            {
                int position = 0;
                foreach (Match match in matches)
                {
                    //iteration sur les parametres de la macro
                    for (int counter = 1; counter < match.Groups.Count; counter++)
                    {
                        string parametre = match.Groups[counter].Value;

                        int Famille = 0;
                        bool Binaire = false;
                        bool resultatcompil = DLL_Equation.RecupererFamilleDeLaDonnee(parametre, ref Famille, ref Binaire);
                        if (resultatcompil)
                        {
                            switch (Famille)
                            {
                                case 9:// bool affectable
                                case 12:
                                case 14:
                                case 15:
                                    parametreslu.Add(position, "Bool1");
                                    break;
                                case 8:
                                    parametreslu.Add(position, "Bool2");
                                    break;
                                case 11:
                                case 13:
                                    parametreslu.Add(position, "Int1");
                                    break;
                                case 7:
                                case 6:
                                case 5:
                                case 4:
                                case 3:
                                case 2:
                                case 1:
                                    parametreslu.Add(position, "Int2");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            parametreslu.Add(position, "Error");
                        }
                        position++;
                    }

                    // comparaison des 2 dictionnaires
                    if (parametreslu.Count == proto.Params_eq.Count)
                    {
                        for (int cmp = 0; cmp < parametreslu.Count; cmp++)
                        {
                            // 1 modififbale 2 modiafiable ou non
                            string para = "";
                            string para_proto = "";
                            parametreslu.TryGetValue(cmp, out para);
                            proto.Params_eq.TryGetValue(cmp, out para_proto);
                            switch(para_proto)
                            {
                                case "Int1":
                                    if (!para.Equals("Int1"))
                                    {
                                        int erreur = match.Groups[cmp + 1].Index + match.Groups[cmp + 1].Length;
                                        ErreurParametres.Add(erreur);
                                    }
                                    break;
                                case "Int2":
                                    if (!para.Equals("Int1") && !para.Equals("Int2"))
                                    {
                                        int erreur = match.Groups[cmp + 1].Index + match.Groups[cmp + 1].Length;
                                        ErreurParametres.Add(erreur);
                                    }
                                    break;
                                case "Bool1":
                                    if (!para.Equals("Bool1"))
                                    {
                                        int erreur = match.Groups[cmp + 1].Index + match.Groups[cmp + 1].Length;
                                        ErreurParametres.Add(erreur);
                                    }
                                    break;
                                case "Bool2":
                                    if (!para.Equals("Bool1") && !para.Equals("Bool2"))
                                    {
                                        int erreur = match.Groups[cmp + 1].Index + match.Groups[cmp + 1].Length;
                                        ErreurParametres.Add(erreur);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        // pas le meme nombre de parametre
                        for (int counter = 1; counter < match.Groups.Count; counter++)
                        {
                            int erreur = match.Groups[counter + 1].Index + match.Groups[counter + 1].Length;
                            ErreurParametres.Add(erreur);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return ErreurParametres;
        }

        private Collection<string> InterpreteurMacro(ref string eq_local_tmp, string value)
        {
            Collection<string> result = new Collection<string>();
            MacroFonction proto = new MacroFonction();
            
            foreach(var macro in PegaseData.Instance.CollecMacro)
            {
                if (macro.Name.Equals(value))
                {
                    proto = macro;
                    break;
                }
            }
            if (proto != null)
            {
                Regex param = new Regex(proto.Analyse, RegexOptions.RightToLeft);
                MatchCollection matches = param.Matches(eq_local_tmp); // recherche de la macro et de ces parametres 
                try
                {
                    if (proto.Equation.Count() > 1)
                    {
                        for (int cpt_eq = 0;  cpt_eq < (proto.Equation.Count()-1); cpt_eq++)
                        {
                            string tmp = proto.Equation.ElementAt(cpt_eq);
                            foreach (Match match in matches)
                            {
                                for (int position = 0; position < proto.NbParams; position++)
                                {
                                    string patern = "$" + position.ToString();
                                    tmp = tmp.Replace(patern, match.Groups[position + 1].Value);
                                }
                            }
                            result.Add(tmp);
                        }
                    }
                    // pour la derniere equation celle de l'affectation
                    foreach (Match match in matches)
                    {
                        //Remplacement du prototype  de la macro par l'equation de la macro
                        eq_local_tmp = eq_local_tmp.Remove(match.Groups[0].Index, match.Groups[0].Length);
                        eq_local_tmp = eq_local_tmp.Insert(match.Groups[0].Index, proto.Equation.ElementAt(proto.Equation.Count() - 1));
                       // offsetmot.Add(match.Groups[1].Index, (match.Groups[1].Length - proto.Equation.ElementAt(proto.Equation.Count() - 1).Length));
                        // mise a jour des parametre de la macro
                        for (int position = 0; position < proto.NbParams; position++)
                        {
                            string patern = "$" + position.ToString();
                            eq_local_tmp = eq_local_tmp.Replace(patern, match.Groups[position + 1].Value);
                        }
                    }
                    result.Add(eq_local_tmp);
                    
                }// fin du try
                catch
                {

                }
            }
            return result;
        }

        public int Findword(string chaine, int pos)
        {
            for (int i = pos-1; i >= 0; i--)
            {
                char c = chaine[i];
                if ((chaine[i] == ' ') || (chaine[i] == '\n') || (chaine[i] == '\t') || (chaine[i] == '\r'))
                {
                    if ((i+1) == pos)
                    {
                        return i;
                    }
                    else
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }
        
        /// <summary>
        /// generation d'une equation conforme au nouveau modele a partir de l'ancien forme de saisie
        /// </summary>
        /// <param name="chaine"></param>
        /// <param name="result"></param>
        public void RetroFit(String chaine, out String result)
        {
            Regex motentier = new Regex(@"([a-zA-Z0-9_]*)", RegexOptions.RightToLeft);
            String eq_local_tmp = chaine;
            string replacement = "";
            Dictionary<int, int> offsetmot = new Dictionary<int, int>();

            if (motentier.IsMatch(chaine))
            {
                MatchCollection matchesMot = motentier.Matches(chaine);

                foreach (Match mots in matchesMot)
                {
                    foreach (var mot in Correspondance)
                    {
                        if (mot.Value.Equals(mots.Value))
                        {
                            //eq_local_tmp = eq_local_tmp.Replace(mots.Groups[1].Value, replacement);
                            eq_local_tmp = eq_local_tmp.Remove(mots.Groups[1].Index, mots.Groups[1].Length);
                            eq_local_tmp = eq_local_tmp.Insert(mots.Groups[1].Index, mot.Key);
                            offsetmot.Add(mots.Groups[1].Index, (mots.Groups[1].Length - replacement.Length));
                            break;
                        }
                        else if (MotInterdit.Contains(mots.Groups[1].Value))
                        {
                            // do nothings
                        }
                    }
                }
            }
            result = eq_local_tmp;
            return;
        }
        /// <summary>
        /// structuremodifié doit être libre de commentaire
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="structuremodifié"></param>
        /// <returns></returns>
        public List<EquationBymode> AnalyseEquationByModeByFonction( ref String structuremodifié)
        {
            //TextRange textRange = new TextRange(
            //                         // TextPointer to the start of content in the RichTextBox.
            //                         textbox.Document.ContentStart,
            //                         // TextPointer to the end of content in the RichTextBox.
            //                         textbox.Document.ContentEnd);
            String structuremodifié2 = structuremodifié;
            //TextPointer start = textbox.Document.ContentStart;
            List<EquationBymode> result = new List<EquationBymode>();
            Regex verifstruct = new Regex(BALISEMODEREGEX);
            int startindex = 0;
            int NmrMode = -1;
            if (verifstruct.IsMatch(structuremodifié))
            {
                // colorié en clair
                MatchCollection matches = verifstruct.Matches(structuremodifié);
                foreach (Match m in matches)
                {
                    string ZoneEquation = structuremodifié.Substring(startindex, m.Groups[1].Index - startindex);
                    
                    string[] substrings = Regex.Split(ZoneEquation, ";");
                    // le numéro du mode d'exploitation
                    foreach (string match in substrings)
                    {
                        // verifier les equatin
                        string eq_local = match.Replace("\r", " ");
                        eq_local = eq_local.Replace("\n", " ");
                        eq_local = eq_local.Replace("\t", " ");
                        // remplacement de noms par le meno hardware

                        // recherche et remplacement
                        Regex motentier = new Regex(@"([a-zA-Z0-9_]*)", RegexOptions.RightToLeft);
                        String eq_local_tmp = eq_local;
                        string replacement = "";
                        Dictionary<int, int> offsetmot = new Dictionary<int, int>();

                        if (motentier.IsMatch(eq_local))
                        {
                            MatchCollection matchesMot = motentier.Matches(eq_local);

                            foreach (Match mots in matchesMot)
                            {
                                if (Correspondance.TryGetValue(mots.Groups[1].Value, out replacement))
                                {
                                    //eq_local_tmp = eq_local_tmp.Replace(mots.Groups[1].Value, replacement);
                                    eq_local_tmp = eq_local_tmp.Remove(mots.Groups[1].Index, mots.Groups[1].Length);
                                    eq_local_tmp = eq_local_tmp.Insert(mots.Groups[1].Index, replacement);
                                    offsetmot.Add(mots.Groups[1].Index, (mots.Groups[1].Length - replacement.Length));
                                }
                                else if (MotInterdit.Contains(mots.Groups[1].Value))
                                {
                                    //eq_local_tmp = eq_local_tmp.Replace(mots.Groups[1].Value, "invalide");
                                    eq_local_tmp = eq_local_tmp.Remove(mots.Groups[1].Index, mots.Groups[1].Length);
                                    eq_local_tmp = eq_local_tmp.Insert(mots.Groups[1].Index, "invalide");
                                    offsetmot.Add(mots.Groups[1].Index, mots.Groups[1].Length - "invalide".Length);
                                }
                            }
                        }
                        //eq_local_tmp correspondat a l'equation prete a etre compilé
                        // on l'ajoute au fonction de l'ancien 

                        if (!String.IsNullOrWhiteSpace(eq_local_tmp) && (NmrMode >=0))
                        {
                            EquationBymode eq = new EquationBymode();
                            eq.mode = NmrMode;
                            eq.equation = eq_local_tmp;
                            result.Add(eq);
                        }
                    }
                    startindex = m.Groups[1].Index + m.Groups[1].Length;
                    string Lemode = m.Groups[0].ToString();
                  
                    if (Lemode.Contains("<\\"))
                    {
                        // cet une balise de fin
                        NmrMode = -1; // mode invalide
                    }
                    else if (Lemode.Contains("Universal_Mode>"))
                    {
                        NmrMode = 32;
                    }
                    else if (Lemode.Contains("Security_Mode>"))
                    {
                        NmrMode = 33;
                    }
                    else if (Lemode.Contains("Mode_"))
                    {
                        Regex moderegex = new Regex("Mode_([0-9]{2})>");
                        Match matchesModes = moderegex.Match(Lemode);
                        String LeModenmr = matchesModes.Groups[1].ToString();
                        NmrMode = Convert.ToInt16(LeModenmr) - 1;
                    }
                }
            }
            structuremodifié = structuremodifié2;
            return result;
        }
    }
}
