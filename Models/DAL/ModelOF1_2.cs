using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public static class ModelOF1
    {
		public static void RequeteArticleSite(string refarti, ref DataTable table1)
        {
			string connectionString = Resource1.ERPString;
			SqlCommand RequeteSql = new SqlCommand();
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(8, refarti, "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];
					
				}
				catch { }
				db1.Close();
			}
			catch
			{
				
			}
		}
		public static void RequeteSolde1(string nmrof, ref DataTable table1)
        {
			string connectionString = Resource1.ERPString;
			SqlCommand RequeteSql = new SqlCommand();
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(7, nmrof, "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];
					//itmref = row["MFGNUM_0"].ToString();
					//TypeOf = row["MFGLIN_0"].ToString();
					//itmref = row["ITMREF_0"].ToString();
					//TypeOf = row["UOMCPLQTY_0"].ToString();
					//itmref = row["UOM_0"].ToString();
					//TypeOf = row["LOTMGTCOD_0"].ToString();
					//itmref = row["SERMGTCOD_0"].ToString();
				}
				catch { }
				db1.Close();
			}
			catch
            {
			}
		}
        public static void RequeteOF1(string nmrof, ref DataTable table1 , ref DataTable table2, ref DataTable table3, ref DataTable table4, ref DataTable table5, ref DataTable table6)
        {
			bool result = true;
			string itmref = "";
			string TypeOf = "";
			DateTime now = DateTime.Now;
            SqlCommand RequeteSql = new SqlCommand();      
			string connectionString = Resource1.ERPString;
			try
            {
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(1, nmrof, "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];
					itmref = row["ITMREF_0"].ToString();
					TypeOf = row["CFGLIN_0"].ToString();
				}
                catch { }
				

				RequeteSql.CommandText = GetOFString(2, nmrof, ""); ;
				monReader = RequeteSql.ExecuteReader();
				table2 = new DataTable();
				table2.Load(monReader);

				RequeteSql.CommandText = GetOFString(3, nmrof, ""); ;
				monReader = RequeteSql.ExecuteReader();
				table3 = new DataTable();
				table3.Load(monReader);
				if (TypeOf.Contains("PACK"))
                {
					RequeteSql.CommandText = GetOFString(4, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table4 = new DataTable();
					table4.Load(monReader);

					RequeteSql.CommandText = GetOFString(5, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table5 = new DataTable();
					table5.Load(monReader);
				}
                else
                {

					RequeteSql.CommandText = GetOFString(51, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table5 = new DataTable();
					table5.Load(monReader);

					RequeteSql.CommandText = GetOFString(6, itmref, "");
					monReader = RequeteSql.ExecuteReader();
					table6 = new DataTable();
					table6.Load(monReader);
				}

				
				result = true;
				db1.Close();
			}
			catch(Exception ex)
            {
				result = false;
				
			}            
		}
		public static void RequeteOF2(string nmrof, ref DataTable table1 , ref DataTable table4, ref DataTable table5, ref DataTable table6)
		{
			bool result = true;
			string itmref = "";
			string TypeOf = "";
			DateTime now = DateTime.Now;
			SqlCommand RequeteSql = new SqlCommand();
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(1, nmrof, "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];
					itmref = row["ITMREF_0"].ToString();
					TypeOf = row["CFGLIN_0"].ToString();
				}
				catch { }


				if (TypeOf.Contains("PACK"))
				{
					RequeteSql.CommandText = GetOFString(4, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table4 = new DataTable();
					table4.Load(monReader);

					RequeteSql.CommandText = GetOFString(5, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table5 = new DataTable();
					table5.Load(monReader);
				}
				else
				{

					RequeteSql.CommandText = GetOFString(51, nmrof, ""); ;
					monReader = RequeteSql.ExecuteReader();
					table5 = new DataTable();
					table5.Load(monReader);

					RequeteSql.CommandText = GetOFString(6, itmref, "");
					monReader = RequeteSql.ExecuteReader();
					table6 = new DataTable();
					table6.Load(monReader);
				}
				result = true;
				db1.Close();
			}
			catch (Exception ex)
			{
				result = false;
			}
		}
		public static void RequeteControleFinal(string nmrof, ref DataTable tables1)
		{
			bool result = true;
			string itmref = "";
			string TypeOf = "";
			DateTime now = DateTime.Now;
			SqlCommand RequeteSql = new SqlCommand();
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(9, nmrof.ToUpper(), "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				tables1 = new DataTable();
				tables1.Load(monReader);
				try
				{
					DataRow row = tables1.Rows[0];
					itmref = row["ITMREF_0"].ToString();
					TypeOf = row["MFGLIN_0"].ToString();
				}

				catch { }
				db1.Close();
			}
            catch { }
		}
		public static void RequeteControleFinalScan(string nmrof,ref DataTable table1, ref DataTable table2)
        {
			DateTime now = DateTime.Now;
			SqlCommand RequeteSql = new SqlCommand();
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(11, nmrof.ToUpper(), "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];

                }
                catch { }
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(10, nmrof.ToUpper(), "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				monReader = RequeteSql.ExecuteReader();
				table2 = new DataTable();
				table2.Load(monReader);
				try
				{
					DataRow row = table2.Rows[0];

				}
				catch { }
				db1.Close();
			}
			catch { }
		}
		public static void RequeteStatusOF(string nmrof, ref DataTable table1)
        {
			DateTime now = DateTime.Now;
			SqlCommand RequeteSql = new SqlCommand();
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = GetOFString(12, nmrof.ToUpper(), "");
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				try
				{
					DataRow row = table1.Rows[0];

				}
				catch (Exception ex) 
				{ 
				}
				db1.Close();
			}
			catch (Exception eb) { 
			}
		}
		private static string  GetOFString( int Nmrstring, string nmrof, string typeOF)
        {
			if (Nmrstring==1 )
            {
				return @"
					SELECT
						MFGHEAD.MFGNUM_0,
						MFGHEAD.EXTQTY_0,
						MFGHEAD.[STRDAT_0],
						[AREPORTM].USR_0,
						[AREPORTM].RPTCOD_0,
						[AREPORTM].NUMREQ_0,

						[TEXCLOB].CODE_0,
						[TEXCLOB].IDENT1_0,
						[TEXCLOB].IDENT2_0,
						[TEXCLOB].IDENT3_0,
						[TEXCLOB].TEXTE_0 AS CODE_ID_SG,

						[FACILITY].FCY_0,
						[FACILITY].FCYNAM_0,
						[FACILITY].FCYSHO_0,
						[FACILITY].CRY_0,

						[TABUNIT].UOM_0,
						[TABUNIT].UOMDEC_0,
						[TABUNIT].UOMSYM_0,
						[TABUNIT].UOMTYP_0,

						[MFGOPE].MFGNUM_0,
						[MFGOPE].OPENUM_0,
						[MFGOPE].ROUOPENUM_0,
						[MFGOPE].OPESPLNUM_0,

						[MFGITM].MFGNUM_0,
						[MFGITM].MFGLIN_0,
						[MFGITM].ITMREF_0,
						[MFGITM].ITMTYP_0,
						[MFGITM].MFGDES_0,
						[MFGITM].[UOMEXTQTY_0],
						[MFGITM].VCRNUMORI_0,

						[BPARTNER].BPRNUM_0,
						[BPARTNER].BPRNAM_0,

						[ITMMASTER].CFGLIN_0,
						[ITMMASTER].ITMDES1_0,
						[ITMMASTER].ITMDES2_0,

						[ITMMASTER].CFGFLDALP2_0 AS CONFIG,
						[ITMMASTER].SEAKEY_0,
						ITMMASTER.SERMGTCOD_0,
						ITMMASTER.LOTMGTCOD_0,
	
						[ITMFACILIT].DEFLOC_5,

						[TABUNIT].UOM_0,
						[TABUNIT].UOMDEC_0,
						[TABUNIT].UOMSYM_0,
						[TABUNIT].UOMTYP_0,

						[SORDERQ].SOHNUM_0,
						SORDERQ.SHIDAT_0,
						[SORDERQ].SOPLIN_0,
						[SORDERQ].SOQSEQ_0,
						[SORDERQ].CPY_0,
						[SORDERQ].SOHCAT_0,
						[SORDERQ].SALFCY_0,
						[SORDERQ].BPCORD_0,
						[SORDERQ].BPAADD_0,
						SORDER.YSTC_0,

						[BPCUSTOMER].BPCNUM_0,
						[BPCUSTOMER].BPCNAM_0,
						[BPCUSTOMER].BPCSHO_0,
						[BPCUSTOMER].BCGCOD_0,
						[BPCUSTOMER].GRP_0,
						[BPCUSTOMER].BPCTYP_0,

						[AUTILIS].PASSDAT_0,
						[AUTILIS].TELEP_0,
						[AUTILIS].TIMCONN_0,
						[AUTILIS].USR_0,
						[AUTILIS].CHEF_0,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '2' ) as emplacemntITEMPROD,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '4' ) as emplacemntITEMSAV,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 != '4'and [MFGHEAD].XTYPOF_0 != '2' ) as emplacemntITEMother


					  FROM[CLTPROD].[MFGHEAD][MFGHEAD]

					  left join[CLTPROD].[AREPORTM][AREPORTM]
					  on[MFGHEAD].MFGNUM_0 = [AREPORTM].CLEA1_0

					  left join[CLTPROD].[TEXCLOB][TEXCLOB]
					  on[MFGHEAD].MFGTEX_0 =[TEXCLOB].CODE_0

					  LEFT join[CLTPROD].[FACILITY][FACILITY]
					  on[MFGHEAD].MFGFCY_0 = [FACILITY].FCY_0

					  left join[CLTPROD].[MFGOPE][MFGOPE]
					  on[MFGHEAD].MFGNUM_0 =[MFGOPE].MFGNUM_0

					  left  join[CLTPROD].[MFGITM][MFGITM]
					  on[MFGHEAD].MFGNUM_0 = [MFGITM].MFGNUM_0

					  left join[CLTPROD].[TABUNIT][TABUNIT]
					  on[MFGHEAD].STU_0 = [TABUNIT].UOM_0 or
					 [MFGITM].UOM_0 = [TABUNIT].UOM_0

					  left join[CLTPROD].[BPARTNER][BPARTNER]
					  on[MFGITM].BPCNUM_0 = [BPARTNER].BPRNUM_0

					  left join[CLTPROD].[ITMMASTER][ITMMASTER]
					  on[MFGITM].ITMREF_0 = [ITMMASTER].ITMREF_0

					  left join[CLTPROD].[ITMFACILIT][ITMFACILIT]
					  on[MFGITM].ITMREF_0 = [ITMFACILIT].ITMREF_0 and
						[MFGITM].MFGFCY_0 = [ITMFACILIT].STOFCY_0

					  left join[CLTPROD].[SORDERQ][SORDERQ]
					  on[MFGITM].VCRNUMORI_0 = [SORDERQ].SOHNUM_0 and
						[MFGITM].VCRLINORI_0 = [SORDERQ].SOPLIN_0

					  LEFT JOIN CLTPROD.SORDER SORDER
						ON SORDERQ.SOHNUM_0 = SORDER.SOHNUM_0

					   left join[CLTPROD].[BPCUSTOMER][BPCUSTOMER]
					   on[SORDERQ].BPCORD_0 = [BPCUSTOMER].BPCNUM_0

					   left join[CLTPROD].[AUTILIS][AUTILIS]
					   on[BPCUSTOMER].ZTCS_0 = [AUTILIS].USR_0

						where MFGHEAD.MFGNUM_0 like '" + nmrof + @"'  order by[MFGHEAD].MFGNUM_0";
			}
			else if (Nmrstring == 2)
            {
				return  @"SELECT  
				   ATEXTRA.TEXTE_0,
				   ZFPA.ZFPA_0,
				   ZFPA.ZFPS_0,
				   ZFPA.ZFPCONTROLM_0,
				   [APLSTD].LANMES_0,
				   ZFPA.ZCOMCONT_0,
				   YCARTEC.XCACOD_0,
				   YCARTEC.XVAL_0			  

      
			  FROM [CLTPROD].[MFGITM] [MFGITM]

			  inner join[CLTPROD].[SORDERQ][SORDERQ]
			  on[MFGITM].VCRNUMORI_0 = [SORDERQ].SOHNUM_0 and
				[MFGITM].VCRLINORI_0 = [SORDERQ].SOPLIN_0

			  LEFT OUTER JOIN CLTPROD.YCARTEC YCARTEC
				ON SORDERQ.SOHNUM_0 = YCARTEC.VCRNUM_0 
				AND SORDERQ.SOPLIN_0 = YCARTEC.VCRLIN_0		
			
			


			  LEFT outer join [CLTPROD].[ATEXTRA] [ATEXTRA]
			  on ATEXTRA.IDENT1_0 = YCARTEC.XCACOD_0
			  and ATEXTRA.LANGUE_0 = 'FRA'



			  left join [CLTPROD].[ZFPA] [ZFPA]
			  on YCARTEC.XVAL_0 = ZFPA.ZFPA_0
			  left  join [CLTPROD].[APLSTD] [APLSTD]
			 on ZFPA.ZFPCONTROLM_0 = [APLSTD].LANNUM_0
			  and [APLSTD].LANCHP_0 ='6101' and [APLSTD].LAN_0 = 'FRA'

			where [MFGITM].MFGNUM_0 like '" + nmrof + @"'";
			}
			else if (Nmrstring == 3)
			{ 
				return @"SELECT 
			[MFGOPE] .MFGNUM_0,
			[MFGOPE] .OPENUM_0 AS OPENUM_0,
			[MFGOPE] .OPEEND_0 AS OPEEND_0,
			[MFGOPE] .OPESTR_0 AS OPESTR_0,
			[MFGOPE] .ROODES_0 AS ROODES_0,
			[MFGOPE].EXTWST_0 AS EXTWST_0,
			[MFGOPE].EXTLAB_0 AS EXTLAB_0,
			[MFGOPE].EXTSETTIM_0 AS EXTSETTIM_0,
			[MFGOPE].EXTOPETIM_0 AS EXTOPETIM_0,
			[MFGOPE].TIMUOMCOD_0 AS TIMUOMCOD_0,
			[APLSTD].LANMES_0 AS LANMES_0,

			[WORKSTATIO].WSTSHO_0,
			[WORKSTATIO].WSTTYP_0,
			[WORKSTATIO].WCR_0,
			[WORKSTATIO].WCRFCY_0,

			[AVWTEXTRA].CODFIC_0,
			[AVWTEXTRA].ZONE_0,
			[AVWTEXTRA].IDENT1_0,
			[AVWTEXTRA].IDENT2_0,
			[AVWTEXTRA].TEXTE_0 AS ACWTEXTE_0,

			[ATEXTRA].TEXTE_0 AS TEXT_0,
			[AVWTEXTRA2].TEXTE_0 AS TEXT_2,

			[TEXCLOB].CODE_0,
			[TEXCLOB].IDENT1_0,
			[TEXCLOB].IDENT2_0,
			[TEXCLOB].IDENT3_0,
			[TEXCLOB].TEXTE_0 AS CODE_ID_SG,
			[TABUNIT].UOM_0,
			[TABUNIT].UOMDEC_0,

			[MFGITM].ITMREF_0,
			[MFGITM].ITMTYP_0,
			[MFGITM].ITMLIN_0,
			[MFGITM].MFGSTA_0,

			[ITMFACILIT].ZDESGAM_0 as ZDESGAM_0



			FROM [CLTPROD].[MFGOPE] [MFGOPE] 

		left  join [CLTPROD].[APLSTD] [APLSTD]
		 on [MFGOPE].TIMUOMCOD_0 = [APLSTD].LANNUM_0
		  and [APLSTD].LANCHP_0 ='301' and [APLSTD].LAN_0 = 'FRA'

			left  join [CLTPROD].[WORKSTATIO] [WORKSTATIO]
			on [MFGOPE].EXTWST_0 = [WORKSTATIO].WST_0 and 
			[MFGOPE].MFGFCY_0 = [WORKSTATIO].WCRFCY_0

			inner  join [CLTPROD].[TABWRKCTR] [TABWRKCTR]
			on [TABWRKCTR].WCR_0 = [WORKSTATIO].WCR_0 

			inner  join [CLTPROD].[ATEXTRA] [ATEXTRA]
			on [TABWRKCTR].WCR_0 = [ATEXTRA].IDENT1_0 
			

			left  join [CLTPROD].[AVWTEXTRA] [AVWTEXTRA]
			on [MFGOPE].EXTWST_0 = [AVWTEXTRA].IDENT1_0 and 
			[MFGOPE].MFGFCY_0 = [AVWTEXTRA].IDENT2_0 and
			[AVWTEXTRA].LAN_0 = 'FRA' and [AVWTEXTRA].ZONE_0 = 'WSTSHOAXX'

			left  join [CLTPROD].[AVWTEXTRA] [AVWTEXTRA2]
			on [MFGOPE].EXTLAB_0 = [AVWTEXTRA2].IDENT1_0 and 
			[MFGOPE].MFGFCY_0 = [AVWTEXTRA2].IDENT2_0 and
			[AVWTEXTRA].LAN_0 = 'FRA' and [AVWTEXTRA].ZONE_0 = 'WSTSHOAXX'

			left join [CLTPROD].[TEXCLOB] [TEXCLOB] 
			on [MFGOPE].MFOTEX_0 = [TEXCLOB].CODE_0

			inner join [CLTPROD].[TABUNIT] [TABUNIT]
			on [MFGOPE].OPEUOM_0 = [TABUNIT].UOM_0 

			inner join [CLTPROD].[MFGITM] [MFGITM]
			on [MFGOPE].MFGNUM_0 = [MFGITM].MFGNUM_0

			inner join [CLTPROD].[ITMFACILIT] [ITMFACILIT]
			on [MFGITM].ITMREF_0 = [ITMFACILIT].ITMREF_0 and
			[MFGITM].MFGFCY_0 = [ITMFACILIT].STOFCY_0
			where [MFGOPE].MFGNUM_0 like '" + nmrof + @"'";
			}
			else if (Nmrstring == 4) // pour les of de pack
			{ 
				return @"
					SELECT 
					[MFGMAT].MFGNUM_0,
					MFGMAT.XCOMBOMP_0,
					[MFGMAT].ITMREF_0,
					[MFGMAT].RETQTY_0,
					[MFGMAT].XVERSION_0,

					[STOALL].QTYSTUACT_0,
					[STOALL].VCRNUM_0,
					[ITMMASTER].ITMDES1_0,
					[ITMMASTER].CFGFLDALP1_0,
					[MFGMAT].XCOMBOMP_0,

					 [STOALL].VCRNUM_0 ,
					 [STOALL].STOFCY_0 ,
					 [STOALL].VCRLIN_0 ,
					 [STOALL].VCRSEQ_0,
					 [STOALL].STOFCY_0,
					[STOALL].ALLTYP_0,
					ITMFACILIT_ZEMPINF.DEFLOC_0 AS Emplacement2,

					  (SELECT TOP 1[STOCK].LOC_0  FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
					  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ'))  AS Emplacement,
					  (SELECT TOP 1[STOCK].LOT_0  FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
					  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ'))  AS Lot,

					CASE 
						  WHEN MFGMAT.XCOMBOMP_0 = 'MO-C' THEN [ITMMASTER].CFGFLDALP2_0
						 WHEN MFGMAT.XCOMBOMP_0 = 'MT-C' THEN [ITMMASTER].CFGFLDALP3_0
						 WHEN MFGMAT.XCOMBOMP_0 = 'SIM-C' THEN [ITMMASTER].CFGFLDALP4_0
						 ELSE  ''
				    END AS CONFIG

					  FROM [CLTPROD].[MFGMAT]

					  inner join [CLTPROD].[ITMMASTER] [ITMMASTER]
					  on [MFGMAT].ITMREF_0 = [ITMMASTER].ITMREF_0


						  left  join [CLTPROD].[ITMFACILIT] ITMFACILIT_ZEMPINF
						  on MFGMAT.ITMREF_0 = ITMFACILIT_ZEMPINF.ITMREF_0

					  left join [CLTPROD].[STOALL] [STOALL]
					  on [MFGMAT].ITMREF_0 = [STOALL].ITMREF_0 and
					  [MFGMAT].MFGNUM_0 = [STOALL].VCRNUM_0 and
					  [MFGMAT].MFGFCY_0 = [STOALL].STOFCY_0 and
					  [MFGMAT].MFGLIN_0 = [STOALL].VCRLIN_0 and
					  [MFGMAT].BOMSEQ_0 = [STOALL].VCRSEQ_0 

					  where ([MFGMAT].XCOMBOMP_0 like '%-C' or [MFGMAT].XCOMBOMP_0 like 'ACC') 
					and [MFGMAT].MFGNUM_0 like '" + nmrof + @"'";
			}
			else if (Nmrstring == 5) // pour les OF de pack
			{
				return @"
						SELECT 
						[MFGMAT].MFGNUM_0,
						[MFGMAT].XCOMBOMP_0,
						[MFGMAT].ITMREF_0,
						[MFGMAT].RETQTY_0,
						[MFGMAT].XVERSION_0,
						[STOALL].ALLTYP_0,
						[STOALL].QTYSTUACT_0,
						[STOALL].VCRNUM_0,

						[ITMMASTER].ITMDES1_0,
						[ITMMASTER].CFGFLDALP1_0,
						[MFGMAT].XCOMBOMP_0,

						[STOALL].VCRNUM_0 ,
						 [STOALL].STOFCY_0 ,
						 [STOALL].VCRLIN_0 ,
						 [STOALL].VCRSEQ_0,
						 [STOALL].STOFCY_0,
						ITMFACILIT_ZEMPINF.DEFLOC_0 AS Emplacement2,

						  (SELECT TOP 1[STOCK].LOC_0 FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
						  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ','STS'))  AS Emplacement,
						(SELECT TOP 1 [STOCK].LOT_0 FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
						  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ','STS')) AS Lot,

						  
						  (SELECT TOP 1 [ATEXTRA].TEXTE_0 FROM [CLTPROD].[ATEXTRA] [ATEXTRA] where [ATEXTRA].IDENT1_0 = [XFAMTECH].XFECOD_0 and 
						  [ATEXTRA].ZONE_0 = 'XFESHO') AS TEXTE, 

						   (SELECT TOP 1 [ATABDIV1].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV1] where ITMFACILIT_ZEMPINF.ZEMPINF_0 = [ATABDIV1].CODE_0) AS TAB1_A1_0, 
						    (SELECT TOP 1 [ATABDIV2].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV2] where ITMFACILIT_ZEMPINF.ZEMPINF_1 = [ATABDIV2].CODE_0) AS TAB2_A1_0, 
							 (SELECT TOP 1 [ATABDIV3].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV3] where ITMFACILIT_ZEMPINF.ZEMPINF_2 = [ATABDIV3].CODE_0) AS TAB3_A1_0, 

						  [XFAMTECH].XFECOD_0						

						  FROM [CLTPROD].[MFGMAT]

						  left  join [CLTPROD].[ITMFACILIT] ITMFACILIT_ZEMPINF
						  on MFGMAT.ITMREF_0 = ITMFACILIT_ZEMPINF.ITMREF_0

						  inner join [CLTPROD].[ITMMASTER] [ITMMASTER]
						  on [MFGMAT].ITMREF_0 = [ITMMASTER].ITMREF_0

						  left join [CLTPROD].[STOALL] [STOALL]
						  on [MFGMAT].ITMREF_0 = [STOALL].ITMREF_0 and
						  [MFGMAT].MFGNUM_0 = [STOALL].VCRNUM_0 and
						  [MFGMAT].MFGFCY_0 = [STOALL].STOFCY_0 and
						  [MFGMAT].MFGLIN_0 = [STOALL].VCRLIN_0 and
						  [MFGMAT].BOMSEQ_0 = [STOALL].VCRSEQ_0 

						  left join [CLTPROD].[XFAMTECH] [XFAMTECH]
						  on [XFAMTECH].XFECOD_0 = [ITMMASTER].XFECOD_0
						  where ([MFGMAT].XCOMBOMP_0 not like '%-C' and  [MFGMAT].XCOMBOMP_0 not like 'ACC') 
						  and [MFGMAT].MFGNUM_0 like '" + nmrof + @"'";
			}
			else if (Nmrstring == 51) //pour les of non pack
			{
				return @"
						SELECT 
						[MFGMAT].MFGNUM_0,
						[MFGMAT].XCOMBOMP_0,
						[MFGMAT].ITMREF_0,
						[MFGMAT].RETQTY_0,
						[MFGMAT].XVERSION_0,
						[STOALL].ALLTYP_0,
						[STOALL].QTYSTUACT_0,
						[STOALL].VCRNUM_0,

						[ITMMASTER].ITMDES1_0,
						[ITMMASTER].CFGFLDALP1_0,
						[MFGMAT].XCOMBOMP_0,

						[STOALL].VCRNUM_0 ,
						 [STOALL].STOFCY_0 ,
						 [STOALL].VCRLIN_0 ,
						 [STOALL].VCRSEQ_0,
						 [STOALL].STOFCY_0,
						ITMFACILIT_ZEMPINF.DEFLOC_0 AS Emplacement2,

						  (SELECT TOP 1[STOCK].LOC_0 FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
						  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ','STS'))  AS Emplacement,
						(SELECT TOP 1 [STOCK].LOT_0 FROM [CLTPROD].[STOCK] [STOCK] where [STOALL].STOFCY_0 = [STOCK].STOFCY_0 and
						  [STOALL].[ITMREF_0] = [STOCK].ITMREF_0 and [STOCK].LOCTYP_0 not in ('BAT','MEZ','STS')) AS Lot,

						  
						  (SELECT TOP 1 [ATEXTRA].TEXTE_0 FROM [CLTPROD].[ATEXTRA] [ATEXTRA] where [ATEXTRA].IDENT1_0 = [XFAMTECH].XFECOD_0 and 
						  [ATEXTRA].ZONE_0 = 'XFESHO') AS TEXTE, 

						   (SELECT TOP 1 [ATABDIV1].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV1] where ITMFACILIT_ZEMPINF.ZEMPINF_0 = [ATABDIV1].CODE_0) AS TAB1_A1_0, 
						    (SELECT TOP 1 [ATABDIV2].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV2] where ITMFACILIT_ZEMPINF.ZEMPINF_1 = [ATABDIV2].CODE_0) AS TAB2_A1_0, 
							 (SELECT TOP 1 [ATABDIV3].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV3] where ITMFACILIT_ZEMPINF.ZEMPINF_2 = [ATABDIV3].CODE_0) AS TAB3_A1_0, 

						  [XFAMTECH].XFECOD_0						

						  FROM [CLTPROD].[MFGMAT]

						  left join [CLTPROD].[ITMFACILIT] ITMFACILIT_ZEMPINF
						  on MFGMAT.ITMREF_0 = ITMFACILIT_ZEMPINF.ITMREF_0

						  inner join [CLTPROD].[ITMMASTER] [ITMMASTER]
						  on [MFGMAT].ITMREF_0 = [ITMMASTER].ITMREF_0

						  left join [CLTPROD].[STOALL] [STOALL]
						  on [MFGMAT].ITMREF_0 = [STOALL].ITMREF_0 and
						  [MFGMAT].MFGNUM_0 = [STOALL].VCRNUM_0 and
						  [MFGMAT].MFGFCY_0 = [STOALL].STOFCY_0 and
						  [MFGMAT].MFGLIN_0 = [STOALL].VCRLIN_0 and
						  [MFGMAT].BOMSEQ_0 = [STOALL].VCRSEQ_0 



						  left join [CLTPROD].[XFAMTECH] [XFAMTECH]
						  on [XFAMTECH].XFECOD_0 = [ITMMASTER].XFECOD_0
						  where ([MFGMAT].XCOMBOMP_0 not like '%-C' ) 
						  and [MFGMAT].MFGNUM_0 like '" + nmrof + @"'";
			}
			else if (Nmrstring == 6)
            {
				return @"SELECT TOP (1000) [BOMD].[CPNITMREF_0],
							  ITMMASTER.ITMDES1_0,
							   [BOMD].[BOMQTY_0],
							   [BOMD].[XCOMBOMP_0],
							   ITMFACILIT.[DEFLOCTYP_0],
							   ITMFACILIT.DEFLOC_0,
							(SELECT TOP 1 [ATABDIV1].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV1] where ITMFACILIT.ZEMPINF_0 = [ATABDIV1].CODE_0) AS TAB1_A1_0, 
							(SELECT TOP 1 [ATABDIV2].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV2] where ITMFACILIT.ZEMPINF_1 = [ATABDIV2].CODE_0) AS TAB2_A1_0, 
							(SELECT TOP 1 [ATABDIV3].A1_0 FROM [CLTPROD].[ATABDIV] [ATABDIV3] where ITMFACILIT.ZEMPINF_2 = [ATABDIV3].CODE_0) AS TAB3_A1_0 

						  FROM [CLTPROD].[BOMD] [BOMD]

						  left join CLTPROD.ITMFACILIT ITMFACILIT
						  on BOMD.CPNITMREF_0 = ITMFACILIT.ITMREF_0

						  left join CLTPROD.ITMMASTER ITMMASTER
						  on BOMD.CPNITMREF_0 = ITMMASTER.ITMREF_0
						  where  ITMMASTER.TCLCOD_0 =  'CON01' and [BOMD].CPNTYP_0 = '6' and [BOMD].BOMENDDAT_0 < '01/01/1900' and [BOMD].ITMREF_0 = '" + nmrof + @"'"; //nmrof est fait la reference de l'article
			}
			else if (Nmrstring == 7)// pour le solde  des OF 
            {
				return @"SELECT  [MFGITMTRK].[MFGNUM_0] as MFGNUM_0
					  ,[MFGITMTRK].[MFGLIN_0] as MFGLIN_0
					  ,[MFGITMTRK].[ITMREF_0] as ITMREF_0

					  ,[MFGITMTRK].[UOMCPLQTY_0] as UOMCPLQTY_0

					  ,[MFGITMTRK].[UOM_0] as UOM_0
					  ,[ITMMASTER].ITMREF_0
					  ,[ITMMASTER].LOTMGTCOD_0 as LOTMGTCOD_0
					  ,[ITMMASTER].SERMGTCOD_0 as SERMGTCOD_0
				  FROM [CLTPROD].[MFGITMTRK] [MFGITMTRK]
			      left join [CLTPROD].[ITMMASTER] [ITMMASTER]
				  on [MFGITMTRK].ITMREF_0 = [ITMMASTER].ITMREF_0
				  where [MFGITMTRK].[MFGNUM_0]like  '" + nmrof + @"'"; //nmrof est fait la reference de l'article

			}
			else if (Nmrstring == 8)
			{
				return @"SELECT 
						[ITMMASTER].ITMREF_0 as ITMREF_0
					  ,[ITMMASTER].[ITMDES1_0] as ITMDES1_0
						,ITMFACILIT_ZEMPINF.DEFLOC_0 AS Emplacement2
				  FROM [CLTPROD].[ITMMASTER] [ITMMASTER]
			      left  join [CLTPROD].[ITMFACILIT] ITMFACILIT_ZEMPINF
						  on [ITMMASTER].ITMREF_0 = ITMFACILIT_ZEMPINF.ITMREF_0
				  where [ITMMASTER].[ITMREF_0] like  '" + nmrof + @"%'"; //nmrof est fait la reference de l'article
			}
			else if (Nmrstring ==9)
            {
				return @"SELECT   TOP 30
				[MFGITM].MFGNUM_0,
						[MFGITM].MFGLIN_0,
						[MFGITM].ITMREF_0,
						[MFGITM].MFGDES_0
 FROM [CLTPROD].[MFGITM] [MFGITM]
						where [MFGITM].[MFGNUM_0] like  '" + nmrof + @"%'";				
            }
			else if (Nmrstring == 10)
            {
				// requet pour les composants d'un pack
				return @"SELECT TOP (1000) 
		MFGITM.MFGNUM_0,
		MFGITM.VCRNUMORI_0,
		MFGITM.VCRLINORI_0,
		MFGITM.MFGDES_0,
		BOMD.[ITMREF_0]     
      ,BOMD.[CPNITMREF_0]
      ,BOMD.[BOMENDDAT_0]
      ,BOMD.[LIKQTYCOD_0]
      ,BOMD.[QTYRND_0]
      ,BOMD.[BOMUOM_0]
      ,BOMD.[BOMSTUCOE_0]
      ,BOMD.[BOMQTY_0]
      ,BOMD.[LIKQTY_0],
	  ITMMASTER.ITMREF_0, 
	ITMMASTER.TCLCOD_0,
	ITMMASTER.SERMGTCOD_0,
	ITMMASTER.ITMDES1_0,
				ITMMASTER.LOTMGTCOD_0
  FROM  [CLTPROD].MFGITM MFGITM
  left join  [CLTPROD].[BOMD] BOMD
  on MFGITM.ITMREF_0 = BOMD.ITMREF_0
  left join CLTPROD.ITMMASTER ITMMASTER
  on ITMMASTER.ITMREF_0 = BOMD.CPNITMREF_0
  where  BOMD.[BOMALT_0] = '1' and BOMD.BOMALTTYP_0 = '2' and BOMD.BOMENDDAT_0 = '1753-01-01' and  ITMMASTER.TCLCOD_0 in ('PF01','PF02','PF03','PDR01','PFST1','PFA01')" +
  " and MFGITM.MFGNUM_0 = '" + nmrof + "'";

			}
			else if (Nmrstring == 11)
            {
				// requet pour of en controle final
				return @"SELECT
				MFGHEAD.MFGNUM_0, 
				MFGHEAD.MFGSTA_0, 
				MFGITM.MFGDES_0,
				APLSTD.LANMES_0 AS 'Statut_OF', 
				MFGHEAD.STRDAT_0, 
				MFGHEAD.ENDDAT_0, 
				MFGHEAD.EXTQTY_0, 
				MFGHEAD.CPLQTY_0, 
				MFGHEAD.RMNEXTQTY_0, 
				MFGHEAD.ROUNUM_0, 
				MFGHEAD.ROUALT_0, 
				MFGHEAD.MFGPIO_0, 
				APLSTD_1.LANMES_0 AS 'Priorite',  
				MFGHEAD.ALLSTA_0, 
				APLSTD_2.LANMES_0 AS 'Allocation', 
				MFGHEAD.MFGTRKFLG_0, 
				APLSTD_3.LANMES_0 AS 'Situation_OF', 
				MFGHEAD.CLODAT_0, 
				MFGHEAD.YILOT_0, 
				MFGITM.ITMREF_0, 
				MFGITM.UOMEXTQTY_0, 
				MFGITM.VCRNUMORI_0, 
				MFGITM.VCRLINORI_0, 
				MFGOPE.OPENUM_0, 
				MFGOPE.EXTWST_0, 
				ATEXTRA.TEXTE_0 AS 'Poste', 
				MFGOPE.EXTOPETIM_0,
				ITMMASTER.TCLCOD_0,
				ITMMASTER.SERMGTCOD_0,
				ITMMASTER.LOTMGTCOD_0,
				(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '2' ) as emplacemntITEMPROD,
				(SELECT TOP 1[ITMFACILITLOC].DEFLOC_1  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '2' ) as emplacemntITEMMZ,
				(SELECT TOP 1[ITMFACILITLOC].DEFLOC_2  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '2' ) as emplacemntITEMBZ
						
				FROM
				 CLTPROD.APLSTD,  CLTPROD.APLSTD APLSTD_1, CLTPROD.APLSTD APLSTD_2, CLTPROD.APLSTD APLSTD_3, CLTPROD.ATEXTRA, CLTPROD.MFGHEAD,CLTPROD.ITMMASTER,  CLTPROD.MFGITM,  CLTPROD.MFGOPE

				WHERE
				MFGHEAD.MFGNUM_0 = MFGITM.MFGNUM_0
				AND MFGHEAD.MFGNUM_0 = MFGOPE.MFGNUM_0
				AND ITMMASTER.ITMREF_0 = MFGITM.ITMREF_0
				AND MFGOPE.EXTWST_0 = ATEXTRA.IDENT1_0
				AND MFGHEAD.MFGSTA_0 = APLSTD.LANNUM_0
				AND MFGHEAD.MFGPIO_0 = APLSTD_1.LANNUM_0
				AND APLSTD_2.LANNUM_0 = MFGHEAD.ALLSTA_0
				AND MFGHEAD.MFGTRKFLG_0 = APLSTD_3.LANNUM_0
				AND(ATEXTRA.CODFIC_0 = 'WORKSTATIO')
				AND(ATEXTRA.ZONE_0 = 'WSTDESAXX')
				AND(ATEXTRA.LANGUE_0 = 'FRA')
				AND(APLSTD.LANCHP_0 = 317)
				AND(APLSTD.LAN_0 = 'FRA')
				AND(APLSTD_1.LANCHP_0 = 365)
				AND(APLSTD_1.LAN_0 = 'FRA')
				AND(APLSTD_2.LANCHP_0 = 336)
				AND(APLSTD_2.LAN_0 = 'FRA')
				AND(APLSTD_3.LANCHP_0 = 339)
				AND(APLSTD_3.LAN_0 = 'FRA')
				AND(MFGHEAD.MFGTRKFLG_0 < 5)
				AND MFGHEAD.MFGNUM_0 = '" + nmrof+"'";
			}
			else if (Nmrstring ==12)
            {
				return @" SELECT TOP 1 MFGHEAD.MFGNUM_0,
				MFGHEAD.MFGTRKFLG_0,
				APLSTD.LANMES_0 AS 'Statut_OF'
				FROM CLTPROD.MFGHEAD MFGHEAD ,CLTPROD.APLSTD
				WHERE 
				MFGHEAD.MFGTRKFLG_0 = APLSTD.LANNUM_0
				
				
				AND(APLSTD.LANCHP_0 = 339)
				AND(APLSTD.LAN_0 = 'FRA')
				
				AND MFGHEAD.MFGNUM_0 = '" + nmrof + "'"
				;
            }
			return "";
        }
    }
}