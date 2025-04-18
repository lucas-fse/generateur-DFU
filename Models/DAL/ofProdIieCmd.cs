using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public class ofProdIieCmd
    {

		string of1 = @" SELECT
					MFGHEAD.MFGNUM_0,
					[MFGITM].ITMREF_0,
					[MFGITM].VCRNUMORI_0,
					[MFGITM].MFGDES_0,
					SORDERQ.SHIDAT_0, -- date expede commande
					
					MFGHEAD.[STRDAT_0],-- date debut of
					MFGHEAD.EXTQTY_0,
					MFGHEAD.[XDATREFEND_0], -- date de fin
					MFGHEAD.XTYPOF_0, -- SAV
					[ORDERS].MRPMES_0,
					
					MFGHEAD.MFGTRKFLG_0,
					MFGHEAD.ALLSTA_0,					

					[FACILITY].FCY_0,
					[FACILITY].FCYNAM_0,
					[FACILITY].FCYSHO_0,
					[FACILITY].CRY_0,

					[TABUNIT].UOM_0,
					[TABUNIT].UOMDEC_0,
					[TABUNIT].UOMSYM_0,
					[TABUNIT].UOMTYP_0,					
					
					[MFGITM].MFGNUM_0,
					[MFGITM].MFGLIN_0,
					[MFGITM].ITMREF_0,
					[MFGITM].ITMTYP_0,
					[MFGITM].MFGDES_0,
					[MFGITM].[UOMEXTQTY_0],
					MFGITM.MFGSTA_0,

					[ITMMASTER].ITMREF_0,
					[ITMMASTER].ITMDES1_0,
					[ITMMASTER].ITMDES2_0,
					[ITMMASTER].CFGFLDALP2_0 AS CONFIG,
					[ITMMASTER].SEAKEY_0,
					ITMMASTER.SERMGTCOD_0,
					ITMMASTER.LOTMGTCOD_0,
					ITMMASTER.ITMSTD_0, --ATEX ou UL
	
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
					[MFGOPE].EXTSETTIM_0 AS EXTSETTIM_0,
					[MFGOPE].EXTOPETIM_0 AS EXTOPETIM_0,
					[MFGOPE].EXTWST_0 AS EXTWST_0,
					[APLSTD].LANMES_0,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '2' ) as emplacemntITEMPROD,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 = '4' ) as emplacemntITEMSAV,
						(SELECT TOP 1[ITMFACILITLOC].DEFLOC_0  FROM [CLTPROD].[ITMFACILIT] [ITMFACILITLOC] where [MFGITM].ITMREF_0 = [ITMFACILITLOC].ITMREF_0 and [MFGHEAD].XTYPOF_0 != '4'and [MFGHEAD].XTYPOF_0 != '2' ) as emplacemntITEMother


				  FROM[CLTPROD].[MFGHEAD][MFGHEAD]


				  inner join[CLTPROD].[FACILITY][FACILITY]
				  on[MFGHEAD].MFGFCY_0 = [FACILITY].FCY_0

				  inner join[CLTPROD].[MFGITM][MFGITM]
				  on[MFGHEAD].MFGNUM_0 = [MFGITM].MFGNUM_0

				  inner join[CLTPROD].[TABUNIT][TABUNIT]
				  on[MFGHEAD].STU_0 = [TABUNIT].UOM_0 or
				 [MFGITM].UOM_0 = [TABUNIT].UOM_0				  

				  inner join[CLTPROD].[ITMMASTER][ITMMASTER]
				  on[MFGITM].ITMREF_0 = [ITMMASTER].ITMREF_0

				  inner join[CLTPROD].[ITMFACILIT][ITMFACILIT]
				  on[MFGITM].ITMREF_0 = [ITMFACILIT].ITMREF_0 and
					[MFGITM].MFGFCY_0 = [ITMFACILIT].STOFCY_0

				  left join[CLTPROD].[SORDERQ][SORDERQ]
				  on[MFGITM].VCRNUMORI_0 = [SORDERQ].SOHNUM_0 and
					[MFGITM].VCRLINORI_0 = [SORDERQ].SOPLIN_0

				  left JOIN CLTPROD.SORDER SORDER
				  ON SORDERQ.SOHNUM_0 = SORDER.SOHNUM_0

				  left join[CLTPROD].[BPCUSTOMER][BPCUSTOMER]
				  on[SORDERQ].BPCORD_0 = [BPCUSTOMER].BPCNUM_0

				   left join [CLTPROD].[ORDERS] [ORDERS]
				   on [ORDERS].VCRNUM_0 = [MFGITM].MFGNUM_0 and
				   [ORDERS].ITMREF_0 = [MFGITM].ITMREF_0
					
					left join [CLTPROD].[MFGOPE] [MFGOPE]
				   on [MFGOPE].MFGNUM_0 = [MFGITM].MFGNUM_0 

					LEFT OUTER JOIN CLTPROD.ZCDECARACT1 ZCDECARACT1
                            ON SORDERQ.SOHNUM_0 = ZCDECARACT1.SOHNUM_0
                            AND SORDERQ.SOPLIN_0 = ZCDECARACT1.SOPLIN_0

                    LEFT OUTER JOIN CLTPROD.ZFPA ZFPA
                    ON ZCDECARACT1.VAL_FPARAM_0 = ZFPA.ZFPA_0 

                    left  join [CLTPROD].[APLSTD] [APLSTD]
			        on ZFPA.ZFPCONTROLM_0 = [APLSTD].LANNUM_0
			        and [APLSTD].LANCHP_0 ='6101' and [APLSTD].LAN_0 = 'FRA' 
					where(";
		string of1bis = @" SELECT
					MFGHEAD.MFGNUM_0,
					[MFGITM].ITMREF_0,
					[MFGITM].VCRNUMORI_0,
					[MFGITM].MFGDES_0,
					SORDERQ.SHIDAT_0, -- date expede commande
					
					MFGHEAD.[STRDAT_0],-- date debut of
					MFGHEAD.EXTQTY_0,
					MFGHEAD.[XDATREFEND_0], -- date de fin
					MFGHEAD.XTYPOF_0, -- SAV
					
					
					MFGHEAD.MFGTRKFLG_0,
					MFGHEAD.ALLSTA_0,					

									
					
					[MFGITM].MFGNUM_0,
					[MFGITM].MFGLIN_0,
					[MFGITM].ITMREF_0,
					[MFGITM].ITMTYP_0,
					[MFGITM].MFGDES_0,
					[MFGITM].[UOMEXTQTY_0],
					MFGITM.MFGSTA_0,

					
	
					[ITMFACILIT].DEFLOC_5,

					

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

					[MFGOPE].EXTWST_0 AS EXTWST_0,
					

				  FROM[CLTPROD].[MFGHEAD][MFGHEAD]



				  inner join[CLTPROD].[MFGITM][MFGITM]
				  on[MFGHEAD].MFGNUM_0 = [MFGITM].MFGNUM_0

	
				  inner join[CLTPROD].[ITMFACILIT][ITMFACILIT]
				  on[MFGITM].ITMREF_0 = [ITMFACILIT].ITMREF_0 and
					[MFGITM].MFGFCY_0 = [ITMFACILIT].STOFCY_0

				  left join[CLTPROD].[SORDERQ][SORDERQ]
				  on[MFGITM].VCRNUMORI_0 = [SORDERQ].SOHNUM_0 and
					[MFGITM].VCRLINORI_0 = [SORDERQ].SOPLIN_0

				 

				  

				 
					
					left join [CLTPROD].[MFGOPE] [MFGOPE]
				   on [MFGOPE].MFGNUM_0 = [MFGITM].MFGNUM_0 

					

                   

                     
					where(";
		string of2 = @" )";
		string of3=@"
		order by SORDERQ.SHIDAT_0,MFGHEAD.[STRDAT_0],[MFGITM].VCRNUMORI_0";
		
		public void RequetNomenclature(string NmrOfNomenclature,ref DataTable table1)
		{
			string connectionString = Resource1.ERPString;
			string nomenclature = @"SELECT 
						[MFGMAT].MFGNUM_0,
						[MFGMAT].ITMREF_0
						FROM [CLTPROD].[MFGMAT] [MFGMAT]
						where ([MFGMAT].XCOMBOMP_0 not like '%-C' and  [MFGMAT].XCOMBOMP_0 not like 'ACC') 
						  and [MFGMAT].MFGNUM_0 like '"+ NmrOfNomenclature+ @"'";
			try
			{
				SqlConnection db1 = null;
				SqlCommand RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = nomenclature;
				RequeteSql.CommandTimeout = 60;
				db1.Open();

				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);
				db1.Close();
			}
			catch
			{ }
		}
		public void RequeteOF(ref DataTable table1, string statusof)
        {
			RequeteOF(ref table1, "", statusof);

		}
		public void RequeteOFBis(ref DataTable table1,string listeof ,string statusof)
		{
			RequeteOF(ref table1, listeof, statusof);

		}
		public void RequeteOFbis(ref DataTable table1, string listof, string statusof)
		{
			bool result = false;
			string of = "";
			if (statusof.Contains("EDITE"))
			{
				// of en attente edité ou (en cours et quantite non totalement termine)
				// 1 en attente 3 edit 4 en cours 5 soldé
				//of = "MFGHEAD.MFGTRKFLG_0 = '3' or MFGHEAD.MFGTRKFLG_0 = '1' or (MFGHEAD.MFGTRKFLG_0 = '4' and MFGITM.RMNEXTQTY_0 != '0')";
				of = "MFGHEAD.MFGTRKFLG_0 = '3'  or (MFGHEAD.MFGTRKFLG_0 = '4' and MFGITM.RMNEXTQTY_0 != '0')";
			}
			if (statusof.Contains("ENCOURS"))
			{
				if (!string.IsNullOrEmpty(of))
				{
					of += " or ";
				}
				of += "MFGHEAD.MFGTRKFLG_0 = '4'";
			}
			if (statusof.Contains("SOLDE"))
			{
				if (!string.IsNullOrEmpty(of))
				{
					of += " or ";
				}
				of += "MFGHEAD.MFGTRKFLG_0 = '5'";
			}
			of = of1 + of + of2;
			if (!string.IsNullOrWhiteSpace(listof))
			{
				of += "and MFGHEAD.MFGNUM_0 IN (" + listof + ")";
			}
			of = of + of3;
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				SqlCommand RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = of;
				RequeteSql.CommandTimeout = 60;
				db1.Open();

				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);

				result = true;
			}
			catch (Exception ex)
			{
				result = false;
			}
		}

		public void RequeteOF(ref DataTable table1, string listof, string statusof)
		{
			bool result = false;
			string of = "";
			if (statusof.Contains("EDITE"))
            {
				// of en attente edité ou (en cours et quantite non totalement termine)
				// 1 en attente 3 edit 4 en cours 5 soldé
				//of = "MFGHEAD.MFGTRKFLG_0 = '3' or MFGHEAD.MFGTRKFLG_0 = '1' or (MFGHEAD.MFGTRKFLG_0 = '4' and MFGITM.RMNEXTQTY_0 != '0')";
				of = "MFGHEAD.MFGTRKFLG_0 = '3'  or (MFGHEAD.MFGTRKFLG_0 = '4' and MFGITM.RMNEXTQTY_0 != '0')";
			}
			if (statusof.Contains("ENCOURS"))
			{
				if (!string.IsNullOrEmpty(of))
                {
					of += " or ";
                }
				of += "MFGHEAD.MFGTRKFLG_0 = '4'";
			}
			if (statusof.Contains("SOLDE"))
			{
				if (!string.IsNullOrEmpty(of))
				{
					of += " or ";
				}
				of += "MFGHEAD.MFGTRKFLG_0 = '5'";
			}
			of = of1 + of + of2;
			if (!string.IsNullOrWhiteSpace(listof))
            {
				of += "and MFGHEAD.MFGNUM_0 IN ('" + listof+ "')";
            }
			of= of +of3;
			string connectionString = Resource1.ERPString;
			try
			{
				SqlConnection db1 = null;
				SqlCommand RequeteSql = new SqlCommand();
				db1 = new SqlConnection(connectionString);
				RequeteSql.Connection = db1;
				RequeteSql.CommandText = of;
				RequeteSql.CommandTimeout = 60;
				db1.Open();
				
				SqlDataReader monReader = RequeteSql.ExecuteReader();
				table1 = new DataTable();
				table1.Load(monReader);

				result = true;
			}
			catch (Exception ex)
			{
				result = false;
			}
		}
	}
	
	
}