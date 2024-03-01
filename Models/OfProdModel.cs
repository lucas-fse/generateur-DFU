using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GenerateurDFUSafir.Models
{
    public class OfProdModelInfo
    {
        //Model decrivant les données pour la genration d'un Of de production 
        public long? ID { get; set; }
        public string TypeOF { get; set; } // pack ens acc .....
        public string OFNmr { get; set; }
        public string EmplacementItem { get; set; }
        public string OFClient { get; set; }
        public string OFRaisonSociale { get; set; }
        public string OFAr { get; set; }
        public string OFTCS { get; set; }
        public DateTime OFDebut { get; set; }
        public DateTime OFExpedition { get; set; }
        public int OFQtr { get; set; }
        public int OFQtrLance { get; set; }
        public string OFItmref { get; set; }
        public string OFItmDescrip1 { get; set; }
        public string OFItmDescrip2 { get; set; }
        public string OFConfig { get; set; }
        public string OFSernum { get; set; }

        public List<Caracteristique> ListCaract { get; set; }
        public string CodeIDOf { get; set; }
        public string Test { get; set; }
        public string FPA { get; set; }
        public string StcOrigine { get; set; }
        public string StcEnCharge { get; set; }
        public string StcCommentaire { get; set; }
        public string FPS { get; set; }
        public string PLASTRON { get; set; }

        public string ROODES_0 { get; set; }
        public string TEXTE_0 { get; set; }
        public string POSTE1 { get; set; }
        public string POSTE2 { get; set; }
        public string ComTeteOF { get; set; }
        public string Design1 { get; set; }
        public string Design2 { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public string TempsReglage { get; set; }
        public string TempsOperatoire { get; set; }
        public string Unite { get; set; }
        public string TotalReglage { get; set; }
        public string Totaloperatoire { get; set; }
        public string CommentaireOperatoire { get; set; }
        public int Lotification { get; set; }
        public int Serialisation { get; set; }



        public List<ConstituantsPack> constituantsPacks { get; set; }
        public List<ListeAServir> listeAServirs { get; set; }
        public List<MatieresValorisation> listMatieresValorisation { get; set; }

    }
    public class OfProdModel
    {
        public OfProdModelInfo ofProdModelInfo { get; set; }
        public void OfProdSoldeOF(string nmrof)
        {
            ofProdModelInfo = new OfProdModelInfo();
            DataTable table1 = new DataTable();
            ModelOF1.RequeteSolde1(nmrof, ref table1);
            if(table1!= null && table1.Rows!= null && table1.Rows.Count == 1)
            {
                ofProdModelInfo.Lotification = Convert.ToInt32(table1.Rows[0]["LOTMGTCOD_0"].ToString());
                ofProdModelInfo.Serialisation = Convert.ToInt32(table1.Rows[0]["SERMGTCOD_0"].ToString());

            }
        }

        public void OfProdModelFull(string nmrof)
        {
            DataTable table1 = new DataTable();  
            DataTable table2 = new DataTable();  
            DataTable table3 = new DataTable();  
            DataTable table4 = new DataTable();
            DataTable table5 = new DataTable();
            DataTable table6 = new DataTable();
            ModelOF1.RequeteOF1(nmrof, ref  table1, ref  table2, ref  table3, ref  table4, ref table5, ref table6);
            ofProdModelInfo = new OfProdModelInfo();
            foreach (DataRow row in table1.Rows)
            {
                ofProdModelInfo.OFNmr = row["MFGNUM_0"].ToString();
                if (!string.IsNullOrWhiteSpace(row["emplacemntITEMPROD"].ToString()))
                {
                    ofProdModelInfo.EmplacementItem = row["emplacemntITEMPROD"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(row["emplacemntITEMSAV"].ToString()))
                {
                    ofProdModelInfo.EmplacementItem = row["emplacemntITEMSAV"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(row["emplacemntITEMother"].ToString()))
                {
                    ofProdModelInfo.EmplacementItem = row["emplacemntITEMother"].ToString();
                }
                ofProdModelInfo.OFClient = row["BPRNUM_0"].ToString();
                ofProdModelInfo.OFRaisonSociale = row["BPRNAM_0"].ToString();
                ofProdModelInfo.OFAr = row["VCRNUMORI_0"].ToString();
                ofProdModelInfo.OFTCS = row["USR_0"].ToString();
                ofProdModelInfo.OFDebut = Convert.ToDateTime(row["STRDAT_0"].ToString());
                ofProdModelInfo.TypeOF = row["CFGLIN_0"].ToString();
                if (!string.IsNullOrWhiteSpace(row["SHIDAT_0"].ToString()))
                {
                    string tt = row["SHIDAT_0"].ToString();
                    ofProdModelInfo.OFExpedition = Convert.ToDateTime(row["SHIDAT_0"]);
                }
                else
                {
                    ofProdModelInfo.OFExpedition = new DateTime();
                }
                ofProdModelInfo.OFQtr = (int)Convert.ToDouble(row["EXTQTY_0"]);
                ofProdModelInfo.OFQtrLance = (int)Convert.ToDouble(row["UOMEXTQTY_0"]);
                ofProdModelInfo.OFItmref = row["ITMREF_0"].ToString();
                ofProdModelInfo.OFItmDescrip1 = row["ITMDES1_0"].ToString();
                ofProdModelInfo.OFItmDescrip2 = row["ITMDES2_0"].ToString();
                ofProdModelInfo.OFConfig = row["CONFIG"].ToString();
                int sernum1 =  Convert.ToInt32( row["SERMGTCOD_0"]);
                int sernum2 = Convert.ToInt32(row["LOTMGTCOD_0"]);
                if (sernum1 > 0) { ofProdModelInfo.OFSernum = row["MFGNUM_0"].ToString().Trim().Substring(1, row["MFGNUM_0"].ToString().Trim().Length-1); }
                else if (sernum2 > 0) { ofProdModelInfo.OFSernum = row["MFGNUM_0"].ToString().Trim().Substring(1, row["MFGNUM_0"].ToString().Trim().Length-1); }
                else { ofProdModelInfo.OFSernum = ""; }
                ofProdModelInfo.StcOrigine = row["YSTC_0"].ToString();
                break;
            }
            ofProdModelInfo.FPA = ""; ofProdModelInfo.FPS = ""; ofProdModelInfo.PLASTRON = "";
            ofProdModelInfo.ListCaract = new List<Caracteristique>();
            foreach (DataRow row in table2.Rows)
            {
                Caracteristique car = new Caracteristique();                
                if (row["XCACOD_0"].ToString().Contains("FPARAM"))
                {
                    if (row["XVAL_0"].ToString().Equals(row["ZFPA_0"].ToString()))
                    {
                        ofProdModelInfo.FPA = row["ZFPA_0"].ToString();
                        ofProdModelInfo.FPS = row["ZFPS_0"].ToString();
                        ofProdModelInfo.StcCommentaire = row["ZCOMCONT_0"].ToString();
                        ofProdModelInfo.Test = row["LANMES_0"].ToString();
                    }
                }
                else if (row["XCACOD_0"].ToString().Contains("PLASTRON"))
                {
                    ofProdModelInfo.PLASTRON = row["XVAL_0"].ToString();
                }

                car.XCADOD = row["XCACOD_0"].ToString();
                car.AText = row["TEXTE_0"].ToString();
                car.YcartecVal = row["XVAL_0"].ToString();
                ofProdModelInfo.ListCaract.Add(car);

                
                if (car.XCADOD.Contains("CODE-ID"))
                {
                    ofProdModelInfo.CodeIDOf = "CODE-ID:" + car.YcartecVal;
                }

                //StcEnCharge = row["YSTC_0"].ToString(); a brancher sur listecmd stc
                ofProdModelInfo.StcEnCharge = "Afaire";
               
            }
            foreach(DataRow row in table3.Rows)
            {
                ofProdModelInfo.ROODES_0 = row["ROODES_0"].ToString();
                ofProdModelInfo.TEXTE_0 = row["ACWTEXTE_0"].ToString();
                ofProdModelInfo.POSTE1 = row["EXTWST_0"].ToString();
                ofProdModelInfo.POSTE2 = row["EXTLAB_0"].ToString();
                ofProdModelInfo.ComTeteOF = row["CODE_ID_SG"].ToString();

                ofProdModelInfo.Design1 = row["TEXT_0"].ToString();
                ofProdModelInfo.Design2 = row["TEXT_2"].ToString();
                ofProdModelInfo.Debut =  Convert.ToDateTime(row["OPESTR_0"].ToString());
                ofProdModelInfo.Fin = Convert.ToDateTime(row["OPEEND_0"].ToString());
                ofProdModelInfo.TempsReglage = row["EXTSETTIM_0"].ToString();
                ofProdModelInfo.TempsOperatoire = row["EXTOPETIM_0"].ToString();
                ofProdModelInfo.Unite = row["LANMES_0"].ToString();
                ofProdModelInfo.TotalReglage ="";
                ofProdModelInfo.Totaloperatoire = "";
                ofProdModelInfo.CommentaireOperatoire = row["ZDESGAM_0"].ToString();
                break;
            }
            ofProdModelInfo.constituantsPacks = new List<ConstituantsPack>();
            foreach (DataRow row in table4.Rows)
            {
                ConstituantsPack cs = new ConstituantsPack();
                cs.PackType = row["XCOMBOMP_0"].ToString();
                cs.PackRefProduit = row["ITMREF_0"].ToString();
                cs.PackQtr = (int)Convert.ToDouble(row["RETQTY_0"].ToString());
                cs.LSqtrByPack = "x" + (cs.PackQtr / ofProdModelInfo.OFQtr).ToString();
                cs.PackVersion = row["XVERSION_0"].ToString();
                cs.PackLot = row["Lot"].ToString();
                cs.PackDescription = row["ITMDES1_0"].ToString();
                cs.PackConfig = row["CFGFLDALP1_0"].ToString();
                cs.PackLibconfig = row["CONFIG"].ToString();
                cs.PackAllocation = row["ALLTYP_0"].ToString();                
                cs.PackLocalisation= row["Emplacement"].ToString();
                if (string.IsNullOrWhiteSpace(cs.PackLocalisation))
                {
                    cs.PackLocalisation = row["Emplacement2"].ToString();
                }
                ofProdModelInfo.constituantsPacks.Add(cs);
            }
            ofProdModelInfo.listeAServirs = new List<ListeAServir>();
            foreach (DataRow row in table5.Rows)
            {
                ListeAServir cs = new ListeAServir(); 
                cs.LSFamilleTech = row["TEXTE"].ToString();
                cs.LSArticle = row["ITMREF_0"].ToString();
                cs.LSQtr = (int)Convert.ToDouble(row["RETQTY_0"].ToString());
                if (row["ALLTYP_0"].ToString()!="1")
                {
                    cs.ProblemeAlloc = true;
                }
                else 
                { 
                    cs.ProblemeAlloc = false; 
                }
                if (cs.ProblemeAlloc)
                {
                    cs.LSqtrByarticle = "Rupture";
                }
                else
                {
                    cs.LSqtrByarticle = "x" + (cs.LSQtr / ofProdModelInfo.OFQtr).ToString();
                }
                cs.LSVersion = row["XVERSION_0"].ToString();                
                cs.LSDescription = row["ITMDES1_0"].ToString();
                cs.LSQtrAlloue = 0;
                if (!string.IsNullOrWhiteSpace(row["QTYSTUACT_0"].ToString()))
                {
                    cs.LSQtrAlloue = (int)Convert.ToDouble(row["QTYSTUACT_0"].ToString());
                }
                 cs.LSType = row["XCOMBOMP_0"].ToString();
                cs.LSTexte = row["TEXTE"].ToString();
                cs.LSLot = row["Lot"].ToString();
                if (string.IsNullOrWhiteSpace(cs.LSLocalisation))
                {
                    cs.LSLocalisation = row["Emplacement2"].ToString();
                }
                cs.LSLocalisationCompl1 = row["TAB1_A1_0"].ToString();
                cs.LSLocalisationCompl2 = row["TAB2_A1_0"].ToString();
                cs.LSLocalisationCompl3 = row["TAB3_A1_0"].ToString();
                ofProdModelInfo.listeAServirs.Add(cs);
            }
            ofProdModelInfo.listeAServirs = ofProdModelInfo.listeAServirs.OrderBy(p => p.LSLocalisation).ToList();
            ofProdModelInfo.listMatieresValorisation = new List<MatieresValorisation>();
            foreach (DataRow row in table6.Rows)
            {
                MatieresValorisation cs = new MatieresValorisation();
                cs.ValArticle = row["CPNITMREF_0"].ToString();
                cs.ValDescription = row["ITMDES1_0"].ToString();
                cs.ValQtr = (float)(Convert.ToDouble(row["BOMQTY_0"].ToString())* ofProdModelInfo.OFQtr);
                cs.ValQtrArt = "x" + Convert.ToDouble(row["BOMQTY_0"].ToString()).ToString();
                cs.ValCommentaire = row["XCOMBOMP_0"].ToString();
                cs.ValLoc = row["DEFLOCTYP_0"].ToString();
                cs.ValEmplacement = row["DEFLOC_0"].ToString();
                cs.ValEmplCompl = row["TAB1_A1_0"].ToString();
                ofProdModelInfo.listMatieresValorisation.Add(cs);
            }
            ofProdModelInfo.listMatieresValorisation = ofProdModelInfo.listMatieresValorisation.OrderBy(p => p.ValEmplacement).ToList();
        }
        public void OfProdModelLight(string nmrof)
        {
            DataTable table1 = new DataTable();

            DataTable table4 = new DataTable();
            DataTable table5 = new DataTable();
            DataTable table6 = new DataTable();
            ModelOF1.RequeteOF2(nmrof, ref table1, ref table4, ref table5, ref table6);
            foreach (DataRow row in table1.Rows)
            {
                ofProdModelInfo.OFNmr = row["MFGNUM_0"].ToString();
                ofProdModelInfo.OFClient = row["BPRNUM_0"].ToString();
                ofProdModelInfo.OFRaisonSociale = row["BPRNAM_0"].ToString();
                ofProdModelInfo.OFAr = row["VCRNUMORI_0"].ToString();
                ofProdModelInfo.OFTCS = row["USR_0"].ToString();
                ofProdModelInfo.OFDebut = Convert.ToDateTime(row["STRDAT_0"].ToString());
                ofProdModelInfo.TypeOF = row["CFGLIN_0"].ToString();
                if (!string.IsNullOrWhiteSpace(row["SHIDAT_0"].ToString()))
                {
                    string tt = row["SHIDAT_0"].ToString();
                    ofProdModelInfo.OFExpedition = Convert.ToDateTime(row["SHIDAT_0"]);
                }
                else
                {
                    ofProdModelInfo.OFExpedition = new DateTime();
                }
                ofProdModelInfo.OFQtr = (int)Convert.ToDouble(row["EXTQTY_0"]);
                ofProdModelInfo.OFQtrLance = (int)Convert.ToDouble(row["UOMEXTQTY_0"]);
                ofProdModelInfo.OFItmref = row["ITMREF_0"].ToString();
                ofProdModelInfo.OFItmDescrip1 = row["ITMDES1_0"].ToString();
                ofProdModelInfo.OFItmDescrip2 = row["ITMDES2_0"].ToString();
                ofProdModelInfo.OFConfig = row["CONFIG"].ToString();
                int sernum1 = Convert.ToInt32(row["SERMGTCOD_0"]);
                int sernum2 = Convert.ToInt32(row["LOTMGTCOD_0"]);
                if (sernum1 > 0) { ofProdModelInfo.OFSernum = row["MFGNUM_0"].ToString().Trim().Substring(1, row["MFGNUM_0"].ToString().Trim().Length - 1); }
                else if (sernum2 > 0) { ofProdModelInfo.OFSernum = row["MFGNUM_0"].ToString().Trim().Substring(1, row["MFGNUM_0"].ToString().Trim().Length - 1); }
                else { ofProdModelInfo.OFSernum = ""; }
                ofProdModelInfo.StcOrigine = row["YSTC_0"].ToString();
                break;
            }
            ofProdModelInfo.FPA = ""; ofProdModelInfo.FPS = ""; ofProdModelInfo.PLASTRON = "";
            ofProdModelInfo.ListCaract = new List<Caracteristique>();
            ofProdModelInfo.constituantsPacks = new List<ConstituantsPack>();
            foreach (DataRow row in table4.Rows)
            {
                ConstituantsPack cs = new ConstituantsPack();
                cs.PackType = row["XCOMBOMP_0"].ToString();
                cs.PackRefProduit = row["ITMREF_0"].ToString();
                cs.PackQtr = (int)Convert.ToDouble(row["RETQTY_0"].ToString());
                cs.LSqtrByPack = "x" + (cs.PackQtr / ofProdModelInfo.OFQtr).ToString();
                cs.PackVersion = row["XVERSION_0"].ToString();
                cs.PackLot = row["Lot"].ToString();
                cs.PackDescription = row["ITMDES1_0"].ToString();
                cs.PackConfig = row["CFGFLDALP1_0"].ToString();
                cs.PackLibconfig = row["CONFIG"].ToString();
                cs.PackAllocation = row["ALLTYP_0"].ToString();
                cs.PackLocalisation = row["Emplacement"].ToString();
                if (string.IsNullOrWhiteSpace(cs.PackLocalisation))
                {
                    cs.PackLocalisation = row["Emplacement2"].ToString();
                }
                ofProdModelInfo.constituantsPacks.Add(cs);
            }
            ofProdModelInfo.listeAServirs = new List<ListeAServir>();
            foreach (DataRow row in table5.Rows)
            {
                ListeAServir cs = new ListeAServir();
                cs.LSFamilleTech = row["TEXTE"].ToString();
                cs.LSArticle = row["ITMREF_0"].ToString();
                cs.LSQtr = (int)Convert.ToDouble(row["RETQTY_0"].ToString());
                if (row["ALLTYP_0"].ToString() != "1")
                {
                    cs.ProblemeAlloc = true;
                }
                else
                {
                    cs.ProblemeAlloc = false;
                }
                if (cs.ProblemeAlloc)
                {
                    cs.LSqtrByarticle = "Rupture";
                }
                else
                {
                    cs.LSqtrByarticle = "x" + (cs.LSQtr / ofProdModelInfo.OFQtr).ToString();
                }
                cs.LSVersion = row["XVERSION_0"].ToString();
                cs.LSDescription = row["ITMDES1_0"].ToString();
                cs.LSQtrAlloue = 0;
                if (!string.IsNullOrWhiteSpace(row["QTYSTUACT_0"].ToString()))
                {
                    cs.LSQtrAlloue = (int)Convert.ToDouble(row["QTYSTUACT_0"].ToString());
                }
                cs.LSType = row["XCOMBOMP_0"].ToString();
                cs.LSTexte = row["TEXTE"].ToString();
                cs.LSLot = row["Lot"].ToString();
                if (string.IsNullOrWhiteSpace(cs.LSLocalisation))
                {
                    cs.LSLocalisation = row["Emplacement2"].ToString();
                }
                cs.LSLocalisationCompl1 = row["TAB1_A1_0"].ToString();
                cs.LSLocalisationCompl2 = row["TAB2_A1_0"].ToString();
                cs.LSLocalisationCompl3 = row["TAB3_A1_0"].ToString();
                ofProdModelInfo.listeAServirs.Add(cs);
            }
            ofProdModelInfo.listeAServirs = ofProdModelInfo.listeAServirs.OrderBy(p => p.LSLocalisation).ToList();
            ofProdModelInfo.listMatieresValorisation = new List<MatieresValorisation>();
            foreach (DataRow row in table6.Rows)
            {
                MatieresValorisation cs = new MatieresValorisation();
                cs.ValArticle = row["CPNITMREF_0"].ToString();
                cs.ValDescription = row["ITMDES1_0"].ToString();
                cs.ValQtr = (float)(Convert.ToDouble(row["BOMQTY_0"].ToString()) * ofProdModelInfo.OFQtr);
                cs.ValQtrArt = "x" + Convert.ToDouble(row["BOMQTY_0"].ToString()).ToString();
                cs.ValCommentaire = row["XCOMBOMP_0"].ToString();
                cs.ValLoc = row["DEFLOCTYP_0"].ToString();
                cs.ValEmplacement = row["DEFLOC_0"].ToString();
                cs.ValEmplCompl = row["TAB1_A1_0"].ToString();
                ofProdModelInfo.listMatieresValorisation.Add(cs);
            }
            ofProdModelInfo.listMatieresValorisation = ofProdModelInfo.listMatieresValorisation.OrderBy(p => p.ValEmplacement).ToList();
        }

        
    }

    public class ConstituantsPack
    {
        public string PackType { get; set;  }
        public string PackRefProduit { get; set; }
        public int PackQtr { get; set; }
        public bool QtrOnhighlight
        {
            get
            {
                if (PackQtr>1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string LSqtrByPack { set; get; }
        public string PackVersion { get; set; }
        public string PackLot { get; set; }
        public string PackDescription { get; set; }
        public string PackConfig { get; set; }
        public string PackLibconfig { get; set; }
        public string PackAllocation { get; set; }
        public string PackLocalisation { get; set; }
    }
    public class MatieresValorisation
    {
        public string ValArticle { get; set; }
        public string ValDescription { get; set; }
        public float ValQtr { get; set; }
        public string ValQtrArt { get; set; }
        public string ValCommentaire { get; set; }
        public string ValLoc { get; set; }
        public string ValEmplacement { get; set; }
        public string ValEmplCompl { get; set; }
    }

    public class ListeAServir
    {
        public string LSArticle { get; set; }
        public string LSType { get; set; }
        public string LSDescription { get; set; }
        public string LSLocalisation { get; set; }
        public int LSQtr { get; set; }
        public bool ProblemeAlloc { get; set; }
        public string LSqtrByarticle  {   set;get;  }
        public int LSQtrAlloue { get; set; }
        public string LSFamilleTech { get; set; }
        public string LSTexte { get; set; }
        public string LSVersion { get; set; }
        public string LSLot { get; set; }
        public string LSLocalisationCompl1 { get; set; }
        public string LSLocalisationCompl2 { get; set; }
        public string LSLocalisationCompl3 { get; set; }
    }
    public class Caracteristique
    {
        public string AText { get; set; }
        public string YcartecVal { set; get; }
        public string XCADOD { get; set; }
    }
}