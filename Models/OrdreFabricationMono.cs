using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace GenerateurDFUSafir.Models
{
    public class OrdreFabricationMono
    {
        public bool isnoel
        {
            get
            {
                if (DateTime.Now.Month == 12)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string TryToOFUD { get; set; }
        public string NmrOF { get; set; }
        public string NmrCmd { get; set; }
        public string NmrCmdAff
        {
            get
            {
                return "Commande : " + NmrCmd;
            }
        }
        private string _Item = "";
        public string Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        public string Compl_Ref_Item { get; set; }
        public DateTime Date { get; set; }
        public double Qtr { get; set; }
        public string NmrOFAff
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NmrOF))
                {
                    return "NmrOF : " + "Invalide";
                }
                else
                {
                    return "NmrOF : " + NmrOF;
                }
            }
        }
        public string ItemAff
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Item))
                {
                    return "";
                }
                else
                {
                    return "Référence : " + Item;
                }
            }
        }
        public string QtrAff
        {
            get
            {
                if (Qtr == 0)
                {
                    return "";
                }
                else
                {
                    return "Quantité : " + Qtr.ToString();
                }
            }
        }
        public string CodeId { get; set; }
        public string CodeIDAff
        {
            get
            {
                return "Code-ID : " + CodeId;
            }
        }
        
        Bitmap _VueProduit = null;
        public Bitmap VueProduit
        {
            get
            {
                return _VueProduit;
            }
            set { _VueProduit = value; }
        }
        public byte[] imageBytes
        {
            get
            {
                return (byte[]) new ImageConverter().ConvertTo(VueProduit, typeof (byte[]));
            }
        }
        public bool ImpressionRapport { get; set; }
        public List<string> ListDefProduit { get; set; }
        List<ProduitMono> _ListProduit;
        public List<ProduitMono> ListProduit
        {
            get 
            {
                if (_ListProduit==null)
                {
                    GestionCodeOF();
                }
                return _ListProduit;
            }
            set
            {
                _ListProduit = value;
            }
        }
        private Dictionary<string, int> _listcodeid;
        public bool ISGenerernewCodeButton { get; set; }
        public string Erreuranalyse { get; set; }
        public int Nbnewcode { get; set; }
        public List<int> ListNewCode { get; set; }
        
        
        public OrdreFabricationMono(
                     string _MFGNUM_0,
                     string _MFGSTA_0,
                     DateTime _STRDAT_0,                    
                     double _EXTQTY_0,
                     string _SOHNUM_0,
                     string CodeID)
        {    
            NmrOF = _MFGNUM_0;
            Item = _MFGSTA_0;
            Date = _STRDAT_0;
            Qtr = _EXTQTY_0;
            NmrCmd = _SOHNUM_0;
            CodeId = CodeID;
            Nbnewcode = 0;
            ListNewCode = new List<int>();
        }
        private void GestionCodeOF()
        {
            ListProduit = new List<ProduitMono>();
            string path = HttpContext.Current.Request.MapPath(".");
            path = "";
            //string file = path + "C:\\Users\\fournier\\source\\repos\\GenerateurDFUSafirV4\\data\\CodeAlea.xml";
            string file = path + "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\data\\ConfigRefIndusMono.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            foreach (XmlNode xmlnode in xmlDoc.DocumentElement)
            {
                if (!xmlnode.NodeType.Equals(XmlNodeType.Comment))
                {
                    ProduitMono add = new ProduitMono();
                    string groupe = xmlnode.Attributes["Libelle"].Value;
                    string imprimerRp = xmlnode.Attributes["imprimerRapport"].Value;
                    

                    add.NomProduit = groupe;
                    if (imprimerRp.ToUpper().Contains("TRUE"))
                    {
                        add.ImprimerRapport = true;
                    }
                    else
                    {
                        add.ImprimerRapport = false;
                    }

                    foreach (XmlNode xmlchil in xmlnode.ChildNodes)
                    {
                        string sousgroupe = xmlchil.Attributes["Libelle"].Value;
                        TYPEBPBYLIGNETYPE option = new TYPEBPBYLIGNETYPE();
                        option.NomOption = sousgroupe;
                        string aff = xmlchil.Attributes["AfficherNomOption"].Value;
                        if (aff.Equals("Yes"))
                        {
                            option.AfficherNomOption = true; 
                        }
                        else
                        {
                            option.AfficherNomOption = false;
                        }
                        option.optionMaskDebut = Convert.ToInt32(xmlchil.Attributes["optionMaskDebut"].Value);
                        option.optionMaskTaille = Convert.ToInt32(xmlchil.Attributes["optionMaskTaille"].Value);
                        option.ptxG = Convert.ToInt32(xmlchil.Attributes["ptxG"].Value);
                        option.ptxD = Convert.ToInt32(xmlchil.Attributes["ptxD"].Value);
                        option.pty = Convert.ToInt32(xmlchil.Attributes["pty"].Value); 
                        option.Refimage = xmlchil.Attributes["Refimage"].Value;
                        option.Option = new List<OptionDispo>();
                        foreach (XmlNode xmlchil2 in xmlchil.ChildNodes)
                        {
                            OptionDispo Optionpossible = new OptionDispo();
                            Optionpossible.Libelle = xmlchil2.Attributes["Libelle"].Value;
                            Optionpossible.valeur = xmlchil2.Attributes["valeur"].Value;
                            Optionpossible.imageG = xmlchil2.Attributes["imageG"].Value;
                            Optionpossible.imageD = xmlchil2.Attributes["imageD"].Value;
                            Optionpossible.CarteHard = xmlchil2.Attributes["CarteHard"].Value;
                            Optionpossible.Commentaire = xmlchil2.Attributes["Commentaire"].Value;
                            option.Option.Add(Optionpossible);
                        }
                        add.ListOption.Add(option);
                    }
                    ListProduit.Add(add);
                }
            }
        }
        public void MiseAJourDataProduit()
        {
            int erreuranalyse = 0;
            int nbcode = 0;
            int nbtotalcode = 0;
            int nbnewcode = 0;
            //if (!AnalyseCodeidX3(out _listcodeid))
            //{
            //    nbcode = _listcodeid.Count();

            //    foreach (var c in _listcodeid)
            //    {
            //        nbtotalcode += c.Value;
            //    }
            //    Nbnewcode = _listcodeid.Where(p => p.Key.StartsWith("E") && p.Key.Length == 2).Count();
            //    if (nbtotalcode != Qtr)
            //    {
            //        erreuranalyse = 2002;
            //    }
            //}
            //else
            //{
            //    erreuranalyse = 2003;
            //}
            ////verifier si code non deja genérer
            //int codeerreurs;
            //if (erreuranalyse == 0 && !VerifGenerationCodeID(NmrOF, (int)Qtr, out codeerreurs))
            //{
            //    erreuranalyse = codeerreurs;
            //}
            //if (erreuranalyse != 0)
            //{
            //    ISGenerernewCodeButton = false;
            //    DeclarationCodeErreurs erreurslist = new DeclarationCodeErreurs();
            //    CodeGroupe errs;
            //    erreurslist.CodeErreurs.TryGetValue(erreuranalyse, out errs);
            //    Erreuranalyse = errs.Erreur;
            //}
            //else
            //{
            //    if (nbnewcode > 0)
            //    {
            //        ISGenerernewCodeButton = true;
            //    }
            //    else
            //    {
            //        ISGenerernewCodeButton = false;
            //    }
            //    Erreuranalyse = "";
            //}
            bool valide = true;
            string path = HttpContext.Current.Request.MapPath(".");
            path = "";
            VueProduit = null;
            string file = path + "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\data\\";
            ListDefProduit = new List<string>();
            foreach (var index in ListProduit)
            {
                if (Item.StartsWith( index.NomProduit))
                {
                    ImpressionRapport = index.ImprimerRapport;
                    foreach (var index1 in index.ListOption)
                    {
                         
                        int start = index1.optionMaskDebut;
                        int longueur = index1.optionMaskTaille;
                        if (Item.Length< (start+longueur))
                        {
                            valide = false;
                            break;
                        }
                        string valueitem = Item.Substring(start, longueur);
                        bool afficher = index1.AfficherNomOption;
                        foreach(var index2 in index1.Option)
                        {
                            Regex test = new Regex(index2.valeur);
                            if (test.IsMatch(valueitem))
                            {
                                if (afficher)
                                {
                                    ListDefProduit.Add(index2.Libelle);
                                }
                                if (!String.IsNullOrWhiteSpace(index2.imageD))
                                {
                                    string chemin = string.Concat(file, index2.imageD);
                                    Image newImageD = Image.FromFile(chemin);
                                    VueProduit = FusionImage.MergeTwoImages(VueProduit, newImageD, index1.ptxD, index1.pty, index1.Refimage);
                                }
                                if (!String.IsNullOrWhiteSpace(index2.imageG))
                                {
                                    string chemin = string.Concat(file, index2.imageG);
                                    Image newImageG = Image.FromFile(chemin);
                                    VueProduit = FusionImage.MergeTwoImages(VueProduit, newImageG, index1.ptxG, index1.pty, index1.Refimage);
                                }
                                if (!String.IsNullOrWhiteSpace(index2.CarteHard))
                                {
                                    string chemin = string.Concat(file, index2.CarteHard);
                                    Image newImageHard = Image.FromFile(chemin);
                                    VueProduit = FusionImage.MergeTwoImages(VueProduit, newImageHard, index1.ptxD+250, index1.pty+30, index1.Refimage);
                                }
                                break;
                            }                            
                        }
                    }
                }
            }
        }
        private bool AnalyseCodeidX3( out Dictionary<string, int> listcode)
        {   
            Regex regex_multi_codeid = new Regex(Resource1.regex_multi_codeid);
            Regex NewCodeid2 = new Regex("^([0-9]{1,2}) ? x ? (E[0-9]{1,2})");
            Regex NewCodeid1 = new Regex("^(E[0-9]{1,2})");
            Regex OldCodeid1 = new Regex("^([0-9A-F]{4})");
            Regex OldCodeid2 = new Regex("^([0-9]{1,2}) ? x ? ([0-9A-F]{4})");
            listcode = new Dictionary<string, int>();
            bool erreurs = false;
            try
            {
                if (regex_multi_codeid.IsMatch(CodeId))
                {
                    string[] codeiddiff = CodeId.Split(',');
                    foreach (var chaine in codeiddiff)
                    {
                        if (string.IsNullOrWhiteSpace(chaine))
                        {

                        }
                        else if (NewCodeid2.IsMatch(chaine.Trim()) && NewCodeid2.Matches(chaine.Trim()).Count == 2)
                        {
                            int index = 0;
                            string nmrcode = "";
                            int nbcode = 0;
                            foreach (Match m in NewCodeid2.Matches(chaine.Trim()))
                            {
                                if (index == 0) { nbcode = Convert.ToInt32(m.Value); index++; }
                                if (index == 1) { nmrcode = m.Value.Trim(); }
                            }
                            if (!listcode.ContainsKey(nmrcode))
                            {
                                listcode.Add(nmrcode, nbcode);
                            }
                            else
                            {
                                erreurs = true;
                            }
                        }
                        else if (NewCodeid1.IsMatch(chaine.Trim()) && NewCodeid1.Matches(chaine.Trim()).Count ==1)
                        {
                            string nmrcode = "";
                            int nbcode = 0;
                            Match m = NewCodeid1.Match(chaine.Trim());
                            nbcode = 1; nmrcode = m.Value.Trim(); 
                            
                            if (!listcode.ContainsKey(nmrcode))
                            {
                                listcode.Add(nmrcode, nbcode);
                            }
                            else
                            {
                                erreurs = true;
                            }
                        }
                        else if (OldCodeid1.IsMatch(chaine.Trim()) && OldCodeid1.Matches(chaine.Trim()).Count == 1)
                        {
                            string nmrcode = "";
                            int nbcode = 0;
                            Match m = OldCodeid1.Match(chaine.Trim());
                            nbcode = 1; nmrcode = m.Value.Trim();

                            if (!listcode.ContainsKey(nmrcode))
                            {
                                listcode.Add(nmrcode, nbcode);
                            }
                            else
                            {
                                erreurs = true;
                            }
                        }
                        else if (OldCodeid2.IsMatch(chaine.Trim()) && OldCodeid2.Matches(chaine.Trim()).Count == 2)
                        {
                            int index = 0;
                            string nmrcode = "";
                            int nbcode = 0;
                            foreach (Match m in OldCodeid2.Matches(chaine.Trim()))
                            {
                                if (index == 0) { nbcode = Convert.ToInt32(m.Value); index++; }
                                if (index == 1) { nmrcode = m.Value.Trim(); }
                            }
                            if (!listcode.ContainsKey(nmrcode))
                            {
                                listcode.Add(nmrcode, nbcode);
                            }
                            else
                            {
                                erreurs = true;
                            }
                        }
                        else
                        {
                            erreurs = true;
                        }
                    }
                }
                else { erreurs = true; }
            }
            catch
            { erreurs = true; }
            return erreurs;
        }
        private bool VerifGenerationCodeID(string of ,int qtr,out int codeerreurs)
        {
            
            GAMME_UD_UR_UCEntities _db1 = new GAMME_UD_UR_UCEntities();
            var querytraca = _db1.TRACACODEID.Where(p => p.NMROF.Contains(of.Trim()));
            codeerreurs = 0;
            if (querytraca == null)
            {
                // verification impossible
                codeerreurs = 2004;
                return false;
            }
            else if (querytraca.Count() == 0)
            {
                // code pas encore generé
                codeerreurs = 0;
                return true;
            }
            else if (querytraca.Count() == qtr)
            {
                // code déjà généré
                codeerreurs = 2005;
                return false;
            }
            else if (querytraca.Count() != qtr)
            {
                // code généré partiellement
                codeerreurs = 2006;
                return false;
            }
            else
            {
                // autre erreur 
                codeerreurs = 2007;
                return false;
            }
        }
    }
}