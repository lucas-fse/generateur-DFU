using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class InfoOf
    {
		public int? IdPole { get; set; }
		public List<OFATraite> ListOfprod { get; set; }
		public List<string> ListOfprodString { get; set; }
		public InfoOf(int? pole)
        {
			InfoOfByDate(pole, null);

		}
		public InfoOf(int? pole, DateTime? dateTime)
        {
			InfoOfByDate(pole, dateTime);
		}
		public void InfoOfByDate(int? pole,DateTime? dateTime)
		{			
			DataTable table1 = new DataTable();
			ofProdIieCmd Ofs = new ofProdIieCmd();
			Ofs.RequeteOF(ref table1, "EDITE");
			ListOfprod = new List<OFATraite>();
			ListOfprodString = new List<string>();
			foreach (DataRow row in table1.Rows)
            {
				OFATraite oFATraite = new OFATraite();
				oFATraite.NmrOF = row["MFGNUM_0"].ToString();
				oFATraite.Article = row["ITMREF_0"].ToString();
				oFATraite.Designation = row["ITMDES1_0"].ToString();
				oFATraite.StatusOfInt = Convert.ToInt32(row["MFGTRKFLG_0"].ToString());
				if (!String.IsNullOrWhiteSpace(row["SHIDAT_0"].ToString()))
				{
					oFATraite.DateLivraison = Convert.ToDateTime(row["SHIDAT_0"].ToString());
				}
				else
                {
					oFATraite.DateLivraison = null;
				}
				if (!String.IsNullOrWhiteSpace(row["STRDAT_0"].ToString()))
				{
					oFATraite.DateOF = Convert.ToDateTime(row["STRDAT_0"].ToString());
				}
				else
				{
					oFATraite.DateOF = null;
				}
				oFATraite.SituationAllocationInt = Convert.ToInt32(row["ALLSTA_0"].ToString()); 
				
				oFATraite.QtrOf = (int)Convert.ToDouble( row["EXTQTY_0"].ToString());
				oFATraite.QtrNecessaire = "todo" ;     //row[""].ToString();
				oFATraite.ListCmd = new List<string>();
				oFATraite.CmdOrigine = row["VCRNUMORI_0"].ToString();
				oFATraite.MessMrpint = Convert.ToInt32(row["MRPMES_0"].ToString()); 
				if (oFATraite.CmdOrigine.StartsWith("C20"))
                {
					oFATraite.ListCmd.Add(oFATraite.CmdOrigine);
				}
                else if (String.IsNullOrWhiteSpace(oFATraite.CmdOrigine))
                {
					oFATraite.ListCmd.Add("Autre");
				}
                else
                {
					oFATraite.ListCmd.Add(oFATraite.CmdOrigine);
				}
				oFATraite.OFOrigine = "";
				string tmp = row["EXTSETTIM_0"].ToString();
				if (!String.IsNullOrWhiteSpace(tmp))
				{
					oFATraite.TempsReglage = Convert.ToDouble(row["EXTSETTIM_0"].ToString());
				}else { oFATraite.TempsReglage = 0; }
				tmp = row["EXTOPETIM_0"].ToString();
				if (!String.IsNullOrWhiteSpace(tmp))
				{
					oFATraite.TempsOperatoire = Convert.ToDouble(row["EXTOPETIM_0"].ToString());
                }
                else { oFATraite.TempsOperatoire = 0; }
				oFATraite.Poste1 = row["EXTWST_0"].ToString();
				oFATraite.Certif = row["ITMSTD_0"].ToString();
				oFATraite.ServiceAutre = "";
				if (row["XTYPOF_0"].ToString().Contains("4"))
                {
					oFATraite.ServiceAutre = "SAV";

				}
				oFATraite.Test = row["LANMES_0"].ToString();

				// on recupere que les of appartenant au pole
				if (PosteProduction.IsPosteIsInPole(oFATraite.Poste1, pole))
				{
					ListOfprod.Add(oFATraite);
					ListOfprodString.Add(oFATraite.NmrOF);
				}
			}
		}

		public List<lienOf> Analyse(DataTable table1)
		{
			List<lienOf> result = new List<lienOf>();
			foreach (DataRow row in table1.Rows)
			{
				lienOf tmp = new lienOf();
				tmp.NmrOf = row["MFGNUM_0"].ToString();
				tmp.Article = row["ITMREF_0"].ToString();
				result.Add(tmp);
			}
			return result;
		}
	}

	public class OFATraite
	{
		public Dictionary<int, string> DicMessMRP = new Dictionary<int, string> {
					{ 1   ,"Pas d'action" },
					{2   ,"Avancer" },
					{3   ,"Retarder" },
					{4   , "Augmenter" },
					{5   , "Diminuer" },
					{6   , "Inutile" },
					{7   , "Avancer/augmenter" },
					{8   , "Avancer/diminuer" },
					{9   , "Retarder/augmenter" },
					{10  , "Retarder/diminuer" },
					{11  , "Retard sur objectif" },
					{12  , "Article obsolète(fin de vie)" },
					{13  , "Surstock" }
			};
		public Dictionary<int, string> DicMessAlloc = new Dictionary<int, string> {
					{ 1   ,"Non alloué" },
					{2   ,"Partielle" },
					{3   ,"Compléte" },
					{4   , "Partielle/rupture" },
					{5   , "Complète/rupture" }					
			};
		public Dictionary<int, string> StatusOF = new Dictionary<int, string>() { { 0, "Non généré" }, { 1, "En Attente" }, { 2, "En étude" }, { 3, "Edité" }, { 4, "En cours" }, { 5, "Soldé" }, { 6, "Calcul" } };

		public string NmrOF { get; set; }
		public string ClassNmrOF { get
            {
				string tmp = NmrOF;
				if (Planifie)
                {
					tmp = tmp + " oftri";
				}
				return tmp;
			} }
		public bool Planifie { get; set; }
		public int MessMrpint { get; set; }
		public string MessMRp { get {
				string result = "";
				 DicMessMRP.TryGetValue(MessMrpint,  out result);
				return result;
			} }
		public string Article { get; set; }
		public string Designation { get; set; }
		public int SituationAllocationInt { get; set; }
		public string SituationAllocation
		{
			get
			{
				string result = "";
				DicMessAlloc.TryGetValue(SituationAllocationInt, out result);
				return result;
			}
		}
		public int StatusOfInt { get; set; }
		public string StatusOf
		{
			get
			{
				string result = "";
				StatusOF.TryGetValue(StatusOfInt, out result);
				return result;
			}
		}
		public DateTime? DateLivraison { get; set; }
		public string DateLivraisonString { 
			get
			{ 
				if ((DateLivraison== null))
                {
					return "";
				}
				else
                {
					return ((DateTime)DateLivraison).ToString("dd/MM/yy");

				}
			} 
		}
		public int DateLivraisonUpdated { get; set; }
		public string BackgroundColor 
		{ 
			get
            {
				if (DateLivraisonUpdated ==0)
                {
					return "#FFFFFF";
                }
				else if (DateLivraisonUpdated == 1)
				{
					return "#00FFFF";
                }
				else if (DateLivraisonUpdated == 2)
				{
					return "#AFEEEE";
                }
				else if (DateLivraisonUpdated == 3)
				{
					return "#41D1CC";
				}
				else
				{
					return "#FF0000";
                }
			} 
		}
		public DateTime? DateOF { get; set; }
		public string DateOFString 
		{ 
			get
			{
				if ((DateOF == null))
				{
					return "";
				}
				else
				{
					return ((DateTime)DateOF).ToString("dd/MM");

				}
			}
		}
		public int QtrOf { get; set; }
		public string QtrNecessaire { get; set; }
		public List<string> ListCmd { get; set; }
		public string CmdOrigine { get; set; }
		public string CmdOrigineString
		{
			get
			{
				if (CmdOrigine.StartsWith("SG"))
                {
					return "Sugg";
                }
                else
                {
					return CmdOrigine;
                }
			}
		}
		public string OFOrigine { get; set; } // memoire tempo pour recherche de l'of
		public double TempsReglage { get; set; }
		public double TempsOperatoire { get; set; }
		public string TempsOF { get
            {
				return (TempsReglage + TempsOperatoire).ToString("F", CultureInfo.CreateSpecificCulture("en-US"));

			}
        }
		public string TempsOperatoireTotalString 
		{ 
			get
            {
				double tmp = TempsReglage + TempsOperatoire;
				int heure = (int)tmp;
				int minute = (int)((tmp % 1)*60 +0.4);
				return heure.ToString() + "h" + minute.ToString("00");
			} 
		}
		public string Poste1 { get; set; }
		public string Certif { get; set; }
		public string ServiceAutre{get;set;}
		public string Test { get; set; }
		public int InfoOFIcone 
		{	get
            {
				if (Certif.ToUpper().Contains("ATEX"))
                {
					return 1;
                }
				else if (ServiceAutre.ToUpper().Contains("SAV)"))
                {
					return 2;
                }
				else if (Test.ToUpper().Contains("R&D"))
                {
					return 3;
                }
				else if (Test.ToUpper().Contains("STC"))
				{
					return 4;
				}
				else if (Certif.ToUpper().Contains("UL"))
				{
					return 5;
				}
				else { return 0; }
			} 
		}
		public List<string> ListAServir { get; set; }
		public OFATraite Clone()
        {
			OFATraite result = new OFATraite() ;
			result.NmrOF = NmrOF;
			result.MessMrpint = MessMrpint;
			result.Article = Article;
			result.Designation = Designation;
			result.SituationAllocationInt = SituationAllocationInt;
			result.StatusOfInt = StatusOfInt;
			result.DateLivraison = DateLivraison;
			result.DateLivraisonUpdated = DateLivraisonUpdated;
			result.DateOF = DateOF;
			result.QtrOf = QtrOf;
			result.QtrNecessaire = QtrNecessaire;
			result.ListCmd = ListCmd;
			result.CmdOrigine = CmdOrigine;
			result.OFOrigine = OFOrigine;
			result.TempsReglage = TempsReglage;
			result.TempsOperatoire = TempsOperatoire;
			result.Poste1 = Poste1;
			result.ServiceAutre = ServiceAutre;
			result.Test = Test;
			result.Certif = Certif;
			return result;
		}
	}

	public class lienOf
    {
		public string NmrOf { get; set; }
		public string Article { get; set; }		
	}
}