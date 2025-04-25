using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace GenerateurDFUSafir.DAL
{
    public class OfX3
    {
        public List<OrdreFabricationBiDir> ListOfBidirX3()
        {
            SqlConnection db1 = null;
            List<OrdreFabricationBiDir> result = new List<OrdreFabricationBiDir>();
            try
            {

                SqlCommand RequeteSql = new SqlCommand();
                string text = @"SELECT SORDERQ.SOHNUM_0,SORDERQ.SOPLIN_0,SORDERQ.DLVPIO_0,SORDER.ZSTATCDE_0, MFGHEAD.MFGTRKFLG_0,SORDERQ.BPCORD_0,BPADDRESS.BPAADD_0, BPCUSTOMER.BPCNAM_0, BPCUSTOMER.TSCCOD_2,BPCUSTOMER.REP_0, BPCUSTOMER.ZSTC_0, BPCUSTOMER.ZTCS_0, SORDERQ.ORDDAT_0, 
                            SORDERQ.DEMDLVDAT_0, SORDERQ.SHIDAT_0, SORDERQ.EXTDLVDAT_0,SORDERQ.CREDAT_0, ITMMASTER.TCLCOD_0,SORDERQ.ITMREF_0, ITMMASTER.ITMDES1_0, ITMMASTER.ITMDES2_0, ITMMASTER.ITMDES3_0, ITMMASTER.CFGFLDALP1_0, ITMMASTER.CFGFLDALP2_0, 
                            ITMMASTER.CFGFLDALP3_0, ITMMASTER.CFGFLDALP4_0, MFGITM.MFGNUM_0, MFGITM.MFGSTA_0, MFGITM.STRDAT_0,MFGITM.ENDDAT_0,ZCDECARACT1.VAL_FPARAM_0, ZCDECARACT1.VAL_PLASTRON_0, ZCDECARACT1.VAL_SYNCHRO_0, 
                            ZCDECARACT1.VAL_CABLAGE_0, ZFPA.ZFPS_0, ZFPA.ZFPAV_0, ZFPA.ZFPSV_0, ZFPA.ZDATS_0, ZFPA.ZFPCONTROL_0,SORDERQ.QTY_0,ZFPA.ZVERSION_0,SORDER.ZDELAIOK_0,MFGMAT1.ITMREF_0 as 'MO-I',MFGMAT2.ITMREF_0 as 'MO-C',MFGMAT3.ITMREF_0 as 'MT-I',MFGMAT4.ITMREF_0 as 'MT-C', MFGMAT5.ITMREF_0 as 'SIM-I',MFGMAT6.ITMREF_0 as 'SIM-C'
                            
                            FROM CLTPROD.SORDERQ SORDERQ

                            JOIN CLTPROD.SORDER SORDER
                            ON SORDERQ.SOHNUM_0 = SORDER.SOHNUM_0

                            JOIN CLTPROD.BPCUSTOMER BPCUSTOMER
                            ON SORDERQ.BPCORD_0 = BPCUSTOMER.BPCNUM_0

                            JOIN CLTPROD.ITMMASTER ITMMASTER
                            ON SORDERQ.ITMREF_0 = ITMMASTER.ITMREF_0
                            AND((ITMMASTER.TCLCOD_0 = 'PF03') OR (ITMMASTER.TCLCOD_0 = 'PDR01' AND (ITMMASTER.CFGLIN_0 ='MOB' or ITMMASTER.CFGLIN_0 ='MT')))

                            LEFT OUTER JOIN CLTPROD.MFGITM MFGITM
                            ON SORDERQ.SOHNUM_0 = MFGITM.VCRNUMORI_0
                            AND SORDERQ.SOPLIN_0 = MFGITM.VCRLINORI_0
                            AND SORDERQ.SOQSEQ_0 = MFGITM.VCRSEQORI_0
                            AND SORDERQ.ITMREF_0 = MFGITM.ITMREF_0

                            LEFT join  CLTPROD.BPADDRESS BPADDRESS
							on SORDERQ.BPCORD_0 = BPADDRESS.BPANUM_0
							and SORDERQ.BPAADD_0 = BPADDRESS.BPAADD_0

                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT1
                            ON MFGITM.MFGNUM_0 = MFGMAT1.MFGNUM_0
                            AND((MFGMAT1.XCOMBOMP_0 = 'MO-I'))
                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT2
                            ON MFGITM.MFGNUM_0 = MFGMAT2.MFGNUM_0
                            AND((MFGMAT2.XCOMBOMP_0 = 'MO-C'))
                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT3
                            ON MFGITM.MFGNUM_0 = MFGMAT3.MFGNUM_0
                            AND((MFGMAT3.XCOMBOMP_0 = 'MT-I'))
                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT4
                            ON MFGITM.MFGNUM_0 = MFGMAT4.MFGNUM_0
                            AND((MFGMAT4.XCOMBOMP_0 = 'MT-C'))
                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT5
                            ON MFGITM.MFGNUM_0 = MFGMAT5.MFGNUM_0
                            AND((MFGMAT5.XCOMBOMP_0 = 'SIM-I'))
                            LEFT OUTER JOIN CLTPROD.MFGMAT MFGMAT6
                            ON MFGITM.MFGNUM_0 = MFGMAT6.MFGNUM_0
                            AND((MFGMAT6.XCOMBOMP_0 = 'SIM-C'))

                            LEFT OUTER JOIN CLTPROD.MFGHEAD MFGHEAD
                            ON MFGHEAD.MFGNUM_0 = MFGITM.MFGNUM_0
                           
                            LEFT OUTER JOIN CLTPROD.ZCDECARACT1 ZCDECARACT1
                            ON SORDERQ.SOHNUM_0 = ZCDECARACT1.SOHNUM_0
                            AND SORDERQ.SOPLIN_0 = ZCDECARACT1.SOPLIN_0

                            LEFT OUTER JOIN CLTPROD.ZFPA ZFPA
                            ON ZCDECARACT1.VAL_FPARAM_0 = ZFPA.ZFPA_0
                            WHERE  SORDERQ.SOQSTA_0 not like '3' and (MFGITM.MFGSTA_0 not like '4' OR MFGITM.MFGSTA_0 IS NULL)
                            ORDER BY SORDERQ.SOHNUM_0, SORDERQ.SOPLIN_0"; //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 

                // ITMREF ref com
                //CFGLIN egal indus 


                //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 
                string connectionString = Resource1.ERPString;
                db1 = new SqlConnection(connectionString);
                x160Entities db = new x160Entities();
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = text;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(monReader);
                int cpt = 1;
                foreach (DataRow item in table.Rows)
                {
                    int status = 0;
                    if (String.IsNullOrWhiteSpace(item["MFGTRKFLG_0"].ToString()))
                    {
                        status = 0; ;
                    }
                    else
                    {
                        status = Convert.ToInt32(item["MFGTRKFLG_0"]);
                    }
                    string client = item["BPCORD_0"].ToString(); string adress = item["BPAADD_0"].ToString();
                    List<BPADDRESS> adresses = db.BPADDRESS.Where(p => p.BPAADD_0 == adress && p.BPANUM_0 == client).ToList();
                    string pays = adresses.First().CRY_0;
                    result.Add(new OrdreFabricationBiDir(cpt.ToString(), item["MFGNUM_0"].ToString(), Convert.ToInt32(item["QTY_0"]), item["ITMREF_0"].ToString(),
                                                    item["VAL_FPARAM_0"].ToString(), item["ZFPS_0"].ToString(),
                                                    0, item["SIM-C"].ToString(), item["CFGFLDALP4_0"].ToString(),
                                                    0, item["MT-C"].ToString(), item["MT-I"].ToString(), item["CFGFLDALP3_0"].ToString(),
                                                    0, item["MO-C"].ToString(), item["MO-I"].ToString(), item["CFGFLDALP2_0"].ToString(), item["VAL_SYNCHRO_0"].ToString(), item["ZVERSION_0"].ToString(),
                                                    Convert.ToDateTime(item["EXTDLVDAT_0"].ToString()), status,
                                                    client, adress, pays));
                    cpt++;
                }

            }
            finally
            {
                db1.Close();
            }
            return result;
        }
        public List<string> ListCasDEmploi(string article)
        {
            SqlConnection db1 = null;
            List<string> result = new List<string>();
            SqlCommand RequeteSql = new SqlCommand();
            string text = @"SELECT ZEMPLOIFAN.ARTICLE_INIT_0, ZEMPLOIFAN.DES_ART_INIT_0, ZEMPLOIFAN.NIVEAU_0, ZEMPLOIFAN.ARTICLE_0 AS ARTICLE1, ZEMPLOIFAN.CATEG_ART_0,ZEMPLOIFAN.QTE_0 
                            FROM CLTPROD.ITMMASTER ITMMASTER, CLTPROD.ZEMPLOIFAN ZEMPLOIFAN
                            WHERE ZEMPLOIFAN.ARTICLE_0 = ITMMASTER.ITMREF_0 AND ((ZEMPLOIFAN.ARTICLE_INIT_0='" + article +
                            @"') AND (ZEMPLOIFAN.CATEG_ART_0 Not In ('FAN01','FAN02','MRP01','MRP02','MRP03','MRP04','MRP05','MRP06','MRP07')) AND 
                            (ITMMASTER.ITMSTA_0 In (1,2)) AND (ZEMPLOIFAN.ALT_0=1))";
            string connectionString = Resource1.ERPString;
            db1 = new SqlConnection(connectionString);
            x160Entities db = new x160Entities();
            RequeteSql.Connection = db1;
            RequeteSql.CommandText = text;
            db1.Open();
            SqlDataReader monReader = RequeteSql.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(monReader);
            foreach (DataRow item in table.Rows)
            {
                string itemRef = item["ARTICLE1"].ToString();
                if (!result.Contains(itemRef))
                {
                    result.Add(itemRef);
                }
            }
            return result;
        }
        public List<OrdreFabricationMono> ListOfMono()
        {
            SqlConnection db1 = null;
            List<OrdreFabricationMono> result = new List<OrdreFabricationMono>();
            try
            {
                SqlCommand RequeteSql = new SqlCommand();
                string text = @"SELECT MFGHEAD.MFGNUM_0,
                                MFGITM.ITMREF_0,
                                MFGITM.EXTQTY_0,
                                MFGITM.STRDAT_0,
                                SORDERQ.SOHNUM_0,
                                YCARTEC_CODE_ID.XVAL_0 AS 'CODE-ID'
                                FROM CLTPROD.MFGHEAD MFGHEAD

                                inner JOIN CLTPROD.MFGITM MFGITM
                                ON MFGHEAD.MFGNUM_0 = MFGITM.MFGNUM_0
                                
                                LEFT JOIN CLTPROD.SORDERQ SORDERQ
                                ON SORDERQ.SOHNUM_0 = MFGITM.VCRNUMORI_0
                                AND SORDERQ.SOPLIN_0 = MFGITM.VCRLINORI_0
                                AND SORDERQ.SOQSEQ_0 = MFGITM.VCRSEQORI_0
                                AND SORDERQ.ITMREF_0 = MFGITM.ITMREF_0

                                LEFT OUTER JOIN CLTPROD.YCARTEC YCARTEC_CODE_ID
							    ON SORDERQ.SOHNUM_0 = YCARTEC_CODE_ID.VCRNUM_0 
                                AND SORDERQ.SOPLIN_0 = YCARTEC_CODE_ID.VCRLIN_0  
								AND YCARTEC_CODE_ID.XCACOD_0 like 'CODE-ID'  

                             WHERE(((MFGITM.ITMREF_0 Like 'UD%') OR(MFGITM.ITMREF_0 Like 'UC%')OR(MFGITM.ITMREF_0 Like 'ADE%') OR(MFGITM.ITMREF_0 Like 'UR%')OR(MFGITM.ITMREF_0 Like 'D228%') OR(MFGITM.ITMREF_0 Like 'PR0124%')) and(MFGITM.MFGSTA_0  like '1' OR  MFGITM.MFGSTA_0  like '2' or MFGITM.MFGSTA_0  like '3' OR MFGITM.MFGSTA_0 is null))";
                //WHERE(((SORDERQ.ITMREF_0 Like 'UD%') OR(SORDERQ.ITMREF_0 Like 'UC%') OR(SORDERQ.ITMREF_0 Like 'UR%')) and SORDERQ.SOQSTA_0 not like '3' and(MFGITM.MFGSTA_0  like '1' OR  MFGITM.MFGSTA_0  like '2' or MFGITM.MFGSTA_0  like '3' OR MFGITM.MFGSTA_0 is null))

                string connectionString = Resource1.ERPString;
                db1 = new SqlConnection(connectionString);
                x160Entities db = new x160Entities();
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = text;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(monReader);
                int cpt = 1;
                foreach (DataRow item in table.Rows)
                {
                    OrdreFabricationMono itemOF = new OrdreFabricationMono(item["MFGNUM_0"].ToString(),
                                                                   item["ITMREF_0"].ToString(),
                                                                   Convert.ToDateTime(item["STRDAT_0"]),
                                                                   Convert.ToDouble(item["EXTQTY_0"]),
                                                                   item["SOHNUM_0"].ToString(),
                                                                   item["CODE-ID"].ToString());
                    result.Add(itemOF);
                    if (itemOF.Item.Contains("UDE"))
                    {

                    }
                }
            }
            catch
            {

            }
            return result;
        }

        public List<OrdreFabrication> ListOfAllProductionX3()
        {
            SqlConnection db1 = null;
            List<OrdreFabrication> result = new List<OrdreFabrication>();
            try
            {
                SqlCommand RequeteSql = new SqlCommand();
                string text = @"SELECT MFGHEAD.MFGNUM_0,MFGITM.VCRNUMORI_0, MFGHEAD.MFGSTA_0, MFGHEAD.STRDAT_0, MFGHEAD.ENDDAT_0,MFGHEAD.OBJDAT_0, MFGOPE.EXTWST_0, MFGOPE.EXTQTY_0, 
                                       MFGOPE.EXTUNTTIM_0, MFGOPE.EXTOPETIM_0, ATEXTRA.TEXTE_0, MFGITM.TCLCOD_0, MFGITM.ITMREF_0, MFGITM.MFGDES_0, 
                                       MFGITM.TSICOD_4, MFGHEAD.MFGPIO_0, MFGHEAD.MFGTRKFLG_0, ORDERS.MRPMES_0, ORDERS.WIPTYP_0, ORDERS.ITMREFORI_0, ITMMASTER.SERMGTCOD_0,
                                       ORDERS.ORI_0, MFGHEAD.ALLSTA_0, ORDERS.RMNEXTQTY_0
                                       FROM CLTPROD.ATEXTRA ATEXTRA, CLTPROD.MFGHEAD MFGHEAD, CLTPROD.MFGITM MFGITM, CLTPROD.MFGOPE MFGOPE, CLTPROD.ORDERS ORDERS
									   
									   , CLTPROD.ITMMASTER ITMMASTER

                                       WHERE MFGHEAD.MFGNUM_0 = MFGOPE.MFGNUM_0 AND MFGOPE.EXTWST_0 = ATEXTRA.IDENT1_0 AND MFGHEAD.MFGNUM_0 = MFGITM.MFGNUM_0 AND MFGITM.ITMREF_0 = ORDERS.ITMREF_0 AND 
                                       MFGITM.MFGNUM_0 = ORDERS.VCRNUM_0 AND ((MFGHEAD.MFGSTA_0=1) AND (ATEXTRA.CODFIC_0='WORKSTATIO') AND (ATEXTRA.ZONE_0='WSTDESAXX') AND (ATEXTRA.LANGUE_0='FRA') AND 
                                       (ORDERS.WIPTYP_0=5)) AND ITMMASTER.ITMREF_0 = MFGITM.ITMREF_0
                                        ORDER BY MFGHEAD.ENDDAT_0
                            "; //wHERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 

                // ITMREF ref com
                //CFGLIN egal indus 


                //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 


                string connectionString = Resource1.ERPString;
                db1 = new SqlConnection(connectionString);
                x160Entities db = new x160Entities();
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = text;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(monReader);
                int cpt = 1;
                foreach (DataRow item in table.Rows)
                {
                    OrdreFabrication itemOF = new OrdreFabrication(item["MFGNUM_0"].ToString(),
                                                                   item["VCRNUMORI_0"].ToString(),
                                                                   item["MFGSTA_0"].ToString(),
                                                                   Convert.ToDateTime(item["STRDAT_0"]),
                                                                   Convert.ToDateTime(item["ENDDAT_0"]),
                                                                   item["EXTWST_0"].ToString(),
                                                                   item["EXTQTY_0"].ToString(),
                                                                   item["EXTUNTTIM_0"].ToString(),
                                                                   item["EXTOPETIM_0"].ToString(),
                                                                   item["TEXTE_0"].ToString(),
                                                                   item["TCLCOD_0"].ToString(),
                                                                   item["ITMREF_0"].ToString(),
                                                                   item["MFGDES_0"].ToString(),
                                                                   item["TSICOD_4"].ToString(),
                                                                   item["MFGPIO_0"].ToString(),
                                                                   item["MFGTRKFLG_0"].ToString(),
                                                                   item["MRPMES_0"].ToString(),
                                                                   item["WIPTYP_0"].ToString(),
                                                                   item["ITMREFORI_0"].ToString(),
                                                                   item["ORI_0"].ToString(),
                                                                   item["ALLSTA_0"].ToString(),
                                                                   item["RMNEXTQTY_0"].ToString()
                                                                   , Convert.ToDateTime(item["OBJDAT_0"]),
                                                                   Convert.ToInt32(item["SERMGTCOD_0"].ToString()));
                    result.Add(itemOF);
                }
            }
            finally
            {
                db1.Close();
            }
            return result;
        }
        public List<OrdreFabrication> ListOfAllProductionX3Bis(DateTime olddate)
        {
            SqlConnection db1 = null;
            List<OrdreFabrication> result = new List<OrdreFabrication>();
            try
            {
                SqlCommand RequeteSql = new SqlCommand();
                string text = @"SELECT  MFGHEAD.MFGNUM_0,MFGITM.VCRNUMORI_0, MFGHEAD.MFGSTA_0, MFGHEAD.STRDAT_0, MFGHEAD.ENDDAT_0,MFGHEAD.OBJDAT_0, MFGOPE.EXTWST_0, MFGOPE.EXTQTY_0, 
                MFGOPE.EXTUNTTIM_0, MFGOPE.EXTOPETIM_0, MFGITM.TCLCOD_0, MFGITM.ITMREF_0, MFGITM.MFGDES_0, 
                MFGITM.TSICOD_4, MFGHEAD.MFGPIO_0, MFGHEAD.MFGTRKFLG_0,MFGHEADTRK.MFGTRKDAT_0

                FROM  CLTPROD.MFGHEAD MFGHEAD

		        LEFT join CLTPROD.MFGITM MFGITM
		        on MFGHEAD.MFGNUM_0 = MFGITM.MFGNUM_0
		        LEFT join CLTPROD.MFGOPE MFGOPE
		        on MFGHEAD.MFGNUM_0 = MFGOPE.MFGNUM_0
                LEFT join CLTPROD.MFGHEADTRK MFGHEADTRK
		        on MFGHEAD.MFGNUM_0 = MFGHEADTRK.MFGNUM_0

                WHERE  MFGHEADTRK.MFGTRKDAT_0 > '" + olddate.Day + "/" + olddate.Month + "/" + olddate.Year + "'";
                //wHERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 

                // ITMREF ref com
                //CFGLIN egal indus 


                //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 
                string connectionString = Resource1.ERPString;
                db1 = new SqlConnection(connectionString);
                x160Entities db = new x160Entities();
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = text;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(monReader);
                int cpt = 1;

                foreach (DataRow item in table.Rows)
                {

                    OrdreFabrication itemOF = new OrdreFabrication(item["MFGNUM_0"].ToString(),
                                                                    item["VCRNUMORI_0"].ToString(),
                                                                   item["MFGSTA_0"].ToString(),
                                                                   Convert.ToDateTime(item["STRDAT_0"]),
                                                                   Convert.ToDateTime(item["ENDDAT_0"]),
                                                                   item["EXTWST_0"].ToString(),
                                                                   item["EXTQTY_0"].ToString(),
                                                                   item["EXTUNTTIM_0"].ToString(),
                                                                   item["EXTOPETIM_0"].ToString(),
                                                                   "",
                                                                   item["TCLCOD_0"].ToString(),
                                                                   item["ITMREF_0"].ToString(),
                                                                   item["MFGDES_0"].ToString(),
                                                                   item["TSICOD_4"].ToString(),
                                                                   item["MFGPIO_0"].ToString(),
                                                                   item["MFGTRKFLG_0"].ToString(),
                                                                   "",
                                                                   "",
                                                                   "",
                                                                   "",
                                                                   "",
                                                                   "",
                                                                   Convert.ToDateTime(item["MFGTRKDAT_0"]),
                                                                   0);
                    result.Add(itemOF);
                }
            }
            finally
            {
                db1.Close();
            }
            return result;
        }

        public List<OPERATEURS> ListOPERATEURs()
        {
            return ListOPERATEURs("");
        }
        public List<OPERATEURS> ListOPERATEURs(string service)
        {
            List<OPERATEURS> result = new List<OPERATEURS>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                if (!string.IsNullOrWhiteSpace(service))
                {
                    IQueryable<OPERATEURS> queryoperateurs = _db.OPERATEURS.Include("TEMPS_SEMAINE").Include("TEMPS_SAISI").Where(p => p.SERVICE.Contains(service) && (p.FINCONTRAT == null || p.FINCONTRAT > DateTime.Now));
                    result = queryoperateurs.ToList();
                }
                else
                {
                    IQueryable<OPERATEURS> queryoperateurs = _db.OPERATEURS.Include("TEMPS_SEMAINE").Include("TEMPS_SAISI").Where(p => p.SERVICE != null && (p.FINCONTRAT == null || p.FINCONTRAT > DateTime.Now));
                    result = queryoperateurs.ToList();
                }
            }
            return result;
        }


        public List<OF_PROD_TRAITE> ListOFsTraite()
        {
            List<OF_PROD_TRAITE> result = new List<OF_PROD_TRAITE>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Include("ALEAS_OF.ALEAS_OF_DETAILS").Where(p => p.ISALIVE != false && !p.STATUSTYPE.Contains("CLOSED"));
                //IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Where(p => p.ISALIVE != false );
                if (queryoftraite != null && queryoftraite.Count() > 0)
                {
                    result = queryoftraite.ToList();
                }
            }
            return result;
        }
        public List<OF_PROD_TRAITE> ListOFsTraite(string Nmrof)
        {
            List<OF_PROD_TRAITE> result = new List<OF_PROD_TRAITE>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Where(p => p.NMROF.Contains(Nmrof));
                if (queryoftraite != null && queryoftraite.Count() > 0)
                {
                    result = queryoftraite.ToList();
                }
            }
            return result;
        }
        public OPERATEURS OperateurById(long? op)
        {
            OPERATEURS result = null;
            if (op != null)
            {
                using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
                {
                    IQueryable<OPERATEURS> queryoftraite = _db.OPERATEURS.Where(p => p.ID == op);
                    if (queryoftraite != null && queryoftraite.Count() > 0)
                    {
                        result = queryoftraite.First();
                    }
                }
            }
            return result;
        }
        public bool SaveDbOperateur(OPERATEURS ope)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            IQueryable<OPERATEURS> queryopee = _db.OPERATEURS.Where(p => p.ID == ope.ID);
            if (queryopee != null && queryopee.Count() > 0)
            {
                OPERATEURS ope1 = queryopee.FirstOrDefault();
                if (!ope1.ANIMATEUR)
                {
                    //  ope1.POSTE = ope.POSTE;
                }
                _db.SaveChanges();
            }
            return true;
        }
        public bool SaveDbOperateurAll(OPERATEURS ope)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            IQueryable<OPERATEURS> queryopee = _db.OPERATEURS.Where(p => p.ID == ope.ID);
            if (queryopee != null && queryopee.Count() == 1)
            {
                OPERATEURS ope1 = queryopee.FirstOrDefault();
                ope1.NOM = ope.NOM;
                ope1.PRENOM = ope.PRENOM;
                ope1.ANIMATEUR = ope.ANIMATEUR;
                ope1.ANNIVERSAIRE = ope.ANNIVERSAIRE;
                ope1.FINCONTRAT = ope.FINCONTRAT;
                if (ope.ID == 0)
                {
                    ope1.INITIAL = ope.INITIAL;
                }
                ope1.SERVICE = ope.SERVICE;
                ope1.SousService = ope.SousService;
                if (!string.IsNullOrWhiteSpace(ope.PATHA))
                {
                    ope1.PATHA = ope.PATHA;
                }
                _db.SaveChanges();
            }
            else if (ope.ID == 0)
            {
                _db.OPERATEURS.Add(ope);
                _db.SaveChanges();
            }
            return true;
        }
        public bool SaveDbTempsSemainer(OPERATEURS ope, TEMPS_SAISI week)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            IQueryable<OPERATEURS> queryopee = _db.OPERATEURS.Where(p => p.ID == ope.ID);
            if (queryopee != null && queryopee.Count() > 0)
            {
                OPERATEURS ope1 = queryopee.FirstOrDefault();
                week.OPERATEURS = null;
                week.SOUSPROJET = null;
                ope1.TEMPS_SAISI.Add(week);
                _db.SaveChanges();
            }
            return true;
        }
        public bool SaveDbTemsSaisi(OPERATEURS ope, TEMPS_SAISI week)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            IQueryable<TEMPS_SAISI> querysemainaUpgrede = _db.TEMPS_SAISI.Where(p => p.OPERATEURS_ID == ope.ID && p.Semaine == week.Semaine && p.Annee == week.Annee && p.SOUSPROJET_ID == week.SOUSPROJET_ID);
            if (querysemainaUpgrede != null && querysemainaUpgrede.Count() == 1)
            {
                TEMPS_SAISI semaineupgrade1 = querysemainaUpgrede.FirstOrDefault();

                semaineupgrade1.Days1 = Math.Round(week.Days1, 1);
                semaineupgrade1.Days2 = Math.Round(week.Days2, 1);
                semaineupgrade1.Days3 = Math.Round(week.Days3, 1);
                semaineupgrade1.Days4 = Math.Round(week.Days4, 1);
                semaineupgrade1.Days5 = Math.Round(week.Days5, 1);
                semaineupgrade1.Days6 = Math.Round(week.Days6, 1);
                semaineupgrade1.Days7 = Math.Round(week.Days7, 1);

                _db.SaveChanges();
            }
            else if (querysemainaUpgrede != null && querysemainaUpgrede.Count() == 0)
            {
                SOUSPROJET ssprojet = _db.SOUSPROJET.Where(p => p.ID == week.SOUSPROJET_ID).First();
                if (ssprojet != null && week.Days1 + week.Days2 + week.Days3 + week.Days4 + week.Days5 + week.Days6 + week.Days7 != 0)
                {

                    TEMPS_SAISI semaineupgrade1 = new TEMPS_SAISI();
                    semaineupgrade1.Annee = week.Annee;
                    semaineupgrade1.Semaine = week.Semaine;
                    semaineupgrade1.Days1 = Math.Round(week.Days1, 1);
                    semaineupgrade1.Days2 = Math.Round(week.Days2, 1);
                    semaineupgrade1.Days3 = Math.Round(week.Days3, 1);
                    semaineupgrade1.Days4 = Math.Round(week.Days4, 1);
                    semaineupgrade1.Days5 = Math.Round(week.Days5, 1);
                    semaineupgrade1.Days6 = Math.Round(week.Days6, 1);
                    semaineupgrade1.Days7 = Math.Round(week.Days7, 1);
                    semaineupgrade1.OPERATEURS_ID = ope.ID;
                    semaineupgrade1.OPERATEURS = null;
                    semaineupgrade1.SOUSPROJET = null;
                    semaineupgrade1.SOUSPROJET_ID = ssprojet.ID;

                    _db.TEMPS_SAISI.Add(semaineupgrade1);
                    _db.SaveChanges();
                }
            }
            return true;
        }
        public void SaveDbTempsSemaine(short id, int semaine, int annee, bool full)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            var query = _db.TEMPS_SEMAINE.Where(s => s.OPERATEURS.ID == id && s.Semaine == semaine && s.Annee == annee);
            TEMPS_SEMAINE temspByweek = new TEMPS_SEMAINE();
            if (query != null && query.Count() > 0)
            {
                temspByweek = query.First();
            }

            if (temspByweek != null && query.Count() > 0)
            {
                temspByweek.Complete = full;
                _db.SaveChanges();
            }
            else
            {
                OPERATEURS op = _db.OPERATEURS.Where(s => s.ID == id).First();
                temspByweek = new TEMPS_SEMAINE();

                temspByweek.Annee = (short)annee;
                temspByweek.Semaine = (short)semaine;
                temspByweek.Complete = full;
                op.TEMPS_SAISI = null;
                temspByweek.OPERATEURS = op;
                _db.TEMPS_SEMAINE.Add(temspByweek);
                _db.SaveChanges();
            }
        }
        public int SaveDbOF_PROD_TRAITE(OF_PROD_TRAITE OfGenere)
        {
            int result = -1;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                List<OF_PROD_TRAITE> resultof = new List<OF_PROD_TRAITE>();
                IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Include("ALEAS_OF").Where(p => p.OPERATEUR == OfGenere.OPERATEUR && p.NMROF == OfGenere.NMROF && p.ISALIVE != false);
                IQueryable<OF_PROD_TRAITE> queryoftraiteAllOpe = _db.OF_PROD_TRAITE.Include("ALEAS_OF").Where(p => p.OPERATEUR != OfGenere.OPERATEUR && p.NMROF == OfGenere.NMROF && p.ISALIVE != false);
                IQueryable<PLANIF_OF> queryofplanif = _db.PLANIF_OF.Where(p => p.NumOF == OfGenere.NMROF);
                List<PLANIF_OF> resultplanifof = new List<PLANIF_OF>();

                resultof = queryoftraite.ToList();
                OF_PROD_TRAITE of_db;

                // OF_PROD_TRAITE existe il ya a deja une ligne avec un nmr of 
                if (queryoftraite.Count() > 0)
                {
                    // init de l'ancien ligne 
                    of_db = resultof.First();
                    if (!of_db.STATUSTYPE.Contains("CLOSED") && OfGenere.STATUSTYPE.Contains("CLOSED"))
                    {
                        of_db.QTRREEL = OfGenere.QTRREEL;
                        of_db.ENDTIME = OfGenere.ENDTIME;
                        of_db.TEMPSSUPPL = OfGenere.TEMPSSUPPL;
                        of_db.STATUSTYPE = OfGenere.STATUSTYPE;

                        of_db.ISALIVE = true;
                        List<ALEAS_OF> tmpalea = of_db.ALEAS_OF.ToList();
                        // on arrete t t les aleas de l'of
                        foreach (var alea in of_db.ALEAS_OF)
                        {
                            if (alea.IsAlwaysOn == true)
                            {
                                alea.IsAlwaysOn = false;
                                alea.StopTime = DateTime.Now;
                                foreach (var details in alea.ALEAS_OF_DETAILS)
                                {
                                    if (details.StopTime == null)
                                    {
                                        details.StopTime = alea.StopTime;
                                        if (details.DateStart == null)
                                        {
                                            details.DateStart = details.StopTime;
                                        }
                                    }
                                }
                            }
                        }


                        // on solde tt les autres of qui était en cours si la quantité est atteinte
                        if (queryoftraiteAllOpe.Count() > 0 && of_db.QTRREEL >= of_db.QTRTHEORIQUE)
                        {
                            // on solde les autres of 
                            foreach (var query in queryoftraiteAllOpe)
                            {
                                query.QTRREEL = 0;
                                if (query.STATUSTYPE.Equals("CLOSED"))
                                {
                                    query.ENDTIME = DateTime.Now;
                                }
                                // on arret tt les aleas des autre of aussi
                                foreach (var alea in query.ALEAS_OF)
                                {
                                    alea.IsAlwaysOn = false;
                                    foreach (var details in alea.ALEAS_OF_DETAILS)
                                    {
                                        if (details.StopTime == null)
                                        {
                                            details.StopTime = DateTime.Now;
                                        }
                                    }
                                }
                                query.STATUSTYPE = "CLOSED";
                            }
                        }
                    }
                    else if (of_db.STATUSTYPE.Contains("ENPAUSE") && !OfGenere.STATUSTYPE.Contains("ENPAUSE"))
                    {
                        // test reprise d'un of en pause
                        OF_PROD_TRAITE tmp = new OF_PROD_TRAITE();
                        tmp.NMROF = of_db.NMROF;
                        tmp.ITEMREF = of_db.ITEMREF;
                        tmp.Alea = of_db.Alea;
                        tmp.ILOT = of_db.ILOT;
                        tmp.TEMPSTHEORIQUE = of_db.TEMPSTHEORIQUE;
                        tmp.QTRTHEORIQUE = of_db.QTRTHEORIQUE;
                        tmp.MFGDES = of_db.MFGDES;
                        tmp.OPERATEUR = of_db.OPERATEUR;
                        tmp.ISALIVE = true;
                        tmp.TEMPSSUPPL = of_db.TEMPSSUPPL;
                        tmp.QTRREEL = of_db.QTRREEL;
                        tmp.STARTTIME = OfGenere.STARTTIME;
                        tmp.ENDTIME = OfGenere.ENDTIME;
                        tmp.TEMPSSUPPL = OfGenere.TEMPSSUPPL;
                        tmp.STATUSTYPE = OfGenere.STATUSTYPE;
                        tmp.TCLCOD_0 = OfGenere.TCLCOD_0;
                        of_db.ISALIVE = false;
                        List<ALEAS_OF> tmpalea = of_db.ALEAS_OF.ToList();

                        foreach (var alea in tmpalea)
                        {
                            if (alea.ID_OF_PROD_ORIG == null)
                            {
                                alea.ID_OF_PROD_ORIG = alea.ID_OF_PROD_TRAITE;
                            }
                            alea.ID_OF_PROD_TRAITE = tmp.ID;
                            if (alea.IsAlwaysOn == true)
                            {
                                ALEAS_OF_DETAILS newdetails = new ALEAS_OF_DETAILS(); newdetails.DateStart = DateTime.Now;
                                alea.ALEAS_OF_DETAILS.Add(newdetails);
                            }
                        }
                        _db.OF_PROD_TRAITE.Add(tmp);
                    }
                    else if (of_db.STATUSTYPE.Contains("INPROGRESS") && !OfGenere.STATUSTYPE.Contains("INPROGRESS"))
                    {
                        // test mise en pause d'un of in progress
                        //of_db.STARTTIME = OfGenere.STARTTIME;
                        of_db.ENDTIME = OfGenere.ENDTIME;
                        //of_db.OPERATEUR = OfGenere.OPERATEUR;
                        of_db.STATUSTYPE = OfGenere.STATUSTYPE;
                        foreach (var alea in of_db.ALEAS_OF)
                        {
                            if (of_db.STATUSTYPE.Contains("ENPAUSE"))
                            {
                                //alea.IsAlwaysOn = true;
                            }
                            else
                            {
                                alea.IsAlwaysOn = false;
                            }
                            if (alea.IsAlwaysOn == true)
                            {
                                foreach (var details in alea.ALEAS_OF_DETAILS)
                                {
                                    if (details.StopTime == null)
                                    {
                                        details.StopTime = DateTime.Now;
                                    }
                                }
                            }
                        }
                        of_db.QTRREEL = OfGenere.QTRREEL;
                        of_db.Alea = OfGenere.Alea;
                        of_db.QTRREEL = OfGenere.QTRREEL;
                        of_db.TEMPSSUPPL = OfGenere.TEMPSSUPPL;
                        of_db.TCLCOD_0 = OfGenere.TCLCOD_0;
                    }
                    else if (of_db.STATUSTYPE.Contains("CLOSED"))
                    {
                        OF_PROD_TRAITE tmp = new OF_PROD_TRAITE();
                        tmp.NMROF = of_db.NMROF;
                        tmp.ITEMREF = of_db.ITEMREF;
                        tmp.Alea = of_db.Alea;
                        tmp.ILOT = of_db.ILOT;
                        tmp.TEMPSTHEORIQUE = of_db.TEMPSTHEORIQUE;
                        tmp.QTRTHEORIQUE = of_db.QTRTHEORIQUE;
                        tmp.MFGDES = of_db.MFGDES;
                        tmp.OPERATEUR = of_db.OPERATEUR;
                        tmp.ISALIVE = true;
                        tmp.TEMPSSUPPL = of_db.TEMPSSUPPL;
                        tmp.QTRREEL = of_db.QTRREEL;
                        tmp.STARTTIME = DateTime.Now;
                        tmp.ENDTIME = OfGenere.ENDTIME;
                        tmp.TEMPSSUPPL = OfGenere.TEMPSSUPPL;
                        tmp.STATUSTYPE = OfGenere.STATUSTYPE;
                        tmp.TCLCOD_0 = OfGenere.TCLCOD_0;
                        of_db.ISALIVE = false;
                        List<ALEAS_OF> tmpalea = of_db.ALEAS_OF.ToList();
                        //_db.SaveChanges();

                        foreach (var alea in tmpalea)
                        {
                            if (alea.ID_OF_PROD_ORIG == null)
                            {
                                alea.ID_OF_PROD_ORIG = alea.ID_OF_PROD_TRAITE;
                            }
                            alea.ID_OF_PROD_TRAITE = tmp.ID;

                        }
                        _db.OF_PROD_TRAITE.Add(tmp);
                    }
                    int resultsave = _db.SaveChanges();
                    result = 1;
                }
                // on cree la ligne de suivi OF_PROD_TRAITE avec le nmr of et utilisateur
                else
                {
                    OfGenere.ISALIVE = true;
                    _db.OF_PROD_TRAITE.Add(OfGenere);
                    _db.SaveChanges();
                    JobERP.CreateEditeOF(OfGenere);
                    result = 2;
                }

            }
            catch (DbEntityValidationException ex)
            {
                //ErrorPrinter(ex.EntityValidationErrors);
                foreach (var error in ex.EntityValidationErrors)
                {
                    //Le changement c'est maintenant :
                    Console.WriteLine("Entité : {0}", error.Entry.Entity.GetType().Name);
                    foreach (var item in error.ValidationErrors)
                    {
                        Console.WriteLine("{0} : {1}", item.PropertyName, item.ErrorMessage);
                    }
                }
                result = -2;
            }
            catch (Exception e)
            {
                result = -3;
                return result;
            }
            return result;
        }
        public bool SaveDbOF_PLANIF_OF(string nmrof, int etat)
        {
            bool result = false;
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            IQueryable<PLANIF_OF> queryofplanif = _db.PLANIF_OF.Where(p => p.NumOF == nmrof);
            List<PLANIF_OF> resultplanifof = new List<PLANIF_OF>();
            if (queryofplanif != null && queryofplanif.Count() > 0)
            {
                resultplanifof = queryofplanif.ToList();
            }
            PLANIF_OF ofplanif;
            if (resultplanifof.Count() == 0)
            {
                List<POSTES> ListPostes = _db.POSTES.ToList();

                ofplanif = new PLANIF_OF();
                _db.PLANIF_OF.Add(ofplanif);
                ofplanif.NumOF = nmrof;

                ofplanif.DatePlanif = DateTime.Now;
                // recherche du poste
                DataTable rawResult = new DataTable();
                ofProdIieCmd Ofs = new ofProdIieCmd();
                Ofs.RequeteOF(ref rawResult, "EDITE");
                foreach (DataRow row in rawResult.Rows)
                {
                    if (row["MFGNUM_0"].ToString().Contains(nmrof))
                    {
                        ofplanif.Pole = ListPostes.Where(i => i.libelle.Trim().Equals(row["EXTWST_0"].ToString())).First().pole;
                    }
                }
                ofplanif.Rang = -1;
            }
            else
            {
                ofplanif = resultplanifof.First();
            }
            ofplanif.Etat = etat;
            _db.SaveChanges();
            return result;

        }
        public bool SaveDbAddSSProjet(SOUSPROJET ssprj)
        {
            bool result = true;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                _db.SOUSPROJET.Add(ssprj);
                _db.SaveChanges();

            }
            catch
            {

            }
            return result;
        }
        public bool SaveDbSSProjet(SOUSPROJET ssprj)
        {
            bool result = false;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                List<SOUSPROJET> resultof = new List<SOUSPROJET>();
                IQueryable<SOUSPROJET> queryoftraite = _db.SOUSPROJET.Where(p => p.IDSOUSPROJET == ssprj.IDSOUSPROJET);
                resultof = queryoftraite.ToList();
                if (queryoftraite.Count() > 0)
                {
                    SOUSPROJET tt = resultof.First();
                    tt.NomSousProjet = ssprj.NomSousProjet;
                    tt.TitreSousProjet = ssprj.TitreSousProjet;
                    tt.Commentaire = ssprj.Commentaire;
                    tt.Service = tt.Service;
                    tt.DateFinProjet = ssprj.DateFinProjet;
                    _db.SaveChanges();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        public bool SavePasswordOperateur(long idOperateur, string hashedPassword)
        {
            bool result = false;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();

                var operateur = _db.OPERATEURS.FirstOrDefault(op => op.ID == idOperateur);
                if (operateur != null)
                {
                    operateur.Password = hashedPassword;
                    _db.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                // log éventuellement
            }

            return result;
        }

        public string GetPasswordOperateur(long idOperateur)
        {
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                var entry = _db.OPERATEURS_PWD.FirstOrDefault(p => p.ID == idOperateur);
                return entry?.Password;
            }
            catch
            {
                return null;
            }
        }



        public bool SaveDbOFNewAlea(OF_PROD_TRAITE OfGenere)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();

            try
            {
                List<OF_PROD_TRAITE> resultof = new List<OF_PROD_TRAITE>();
                var queryoftraite = _db.OF_PROD_TRAITE.Include("ALEAS_OF").Where(p => p.OPERATEUR == OfGenere.OPERATEUR && p.NMROF == OfGenere.NMROF && p.ISALIVE != false);
                resultof = queryoftraite.ToList();
                OF_PROD_TRAITE of_db;
                if (queryoftraite.Count() > 0)
                {
                    of_db = resultof.First();
                    foreach (var alea in OfGenere.ALEAS_OF)
                    {
                        ALEAS_OF newAlea = new ALEAS_OF();
                        newAlea.NMR_ALEA = alea.NMR_ALEA;
                        if (alea.DateStart != null)
                        {
                            newAlea.DateStart = alea.DateStart;
                        }
                        else if (alea.Delai != null)
                        {
                            newAlea.Delai = alea.Delai;
                        }
                        of_db.ALEAS_OF.Add(newAlea);
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        public List<TEMPS_SAISI> ListTempsSaisiById(long? ID)
        {
            OPERATEURS result = new OPERATEURS();
            List<TEMPS_SAISI> tata = new List<TEMPS_SAISI>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                if (ID != null)
                {
                    try
                    {
                        //IQueryable<OPERATEURS> querytemspsaisi = _db.OPERATEURS;
                        IQueryable<OPERATEURS> querytemspsaisi = _db.OPERATEURS.Include("TEMPS_SAISI").Include("TEMPS_SEMAINE").Where(x => x.ID == ID);
                        IQueryable<TEMPS_SAISI> querytemspsaisi2 = _db.TEMPS_SAISI.Include("OPERATEURS").Include("SOUSPROJET").Where(x => x.OPERATEURS_ID == ID);
                        var toto = (from c in querytemspsaisi select c.TEMPS_SAISI);
                        result = querytemspsaisi.ToList().First();
                        tata = querytemspsaisi2.ToList();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return tata;//result;
        }

        public List<SOUSPROJET> ListSousProjet()
        {
            List<SOUSPROJET> result = new List<SOUSPROJET>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {

                try
                {

                    IQueryable<SOUSPROJET> querytemsp = _db.SOUSPROJET;
                    result = querytemsp.ToList();
                }
                catch (Exception e)
                {

                }

            }
            return result;
        }

        public bool SaveDbOFAlea(OF_PROD_TRAITE OfGenere, long? idAlea) //!!!Attention le nom a changer, l'ancien était SaveDbOFNewAlea
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();

            try
            {
                List<OF_PROD_TRAITE> resultof = new List<OF_PROD_TRAITE>();
                IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Include("ALEAS_OF.ALEAS_OF_DETAILS").Where(p => p.OPERATEUR == OfGenere.OPERATEUR && p.NMROF == OfGenere.NMROF && p.ISALIVE != false);
                resultof = queryoftraite.ToList();

                OF_PROD_TRAITE of_db;
                if (queryoftraite.Count() > 0)
                {
                    of_db = resultof.FirstOrDefault();
                    if (idAlea != null)
                    {
                        of_db.ALEAS_OF.Where(p => p.ID == idAlea).First().StopTime = DateTime.Now;
                        of_db.ALEAS_OF.Where(p => p.ID == idAlea).First().IsAlwaysOn = OfGenere.ALEAS_OF.Where(p => p.ID == idAlea).First().IsAlwaysOn;
                        if (of_db.ALEAS_OF.Where(p => p.ID == idAlea).First().ALEAS_OF_DETAILS.Where(t => t.StopTime == null).Count() > 0)
                        {
                            of_db.ALEAS_OF.Where(p => p.ID == idAlea).First().ALEAS_OF_DETAILS.Where(t => t.StopTime == null).First().StopTime = DateTime.Now;
                        }
                        _db.SaveChanges();
                    }
                    else
                    {
                        foreach (var alea in OfGenere.ALEAS_OF)
                        {
                            if (alea.ID == 0)
                            {
                                ALEAS_OF newAlea = new ALEAS_OF();
                                ALEAS_OF_DETAILS newAleaDetails = new ALEAS_OF_DETAILS();
                                newAlea.NMR_ALEA = alea.NMR_ALEA;
                                if (alea.DateStart != null)
                                {
                                    newAlea.DateStart = alea.DateStart;
                                    newAleaDetails.DateStart = alea.ALEAS_OF_DETAILS.Where(p => p.StopTime == null).First().DateStart;
                                }
                                else if (alea.Delai != null)
                                {
                                    newAlea.Delai = alea.Delai;
                                    newAleaDetails.DateStart = DateTime.Now;
                                    newAleaDetails.StopTime = newAleaDetails.DateStart;
                                }
                                if (of_db.STATUSTYPE.Contains("INPROGRESS"))
                                {
                                    newAlea.IsAlwaysOn = alea.IsAlwaysOn;
                                }
                                else
                                {
                                    newAlea.IsAlwaysOn = false;
                                    newAleaDetails.StopTime = newAleaDetails.DateStart;
                                }
                                newAlea.ALEAS_OF_DETAILS.Add(newAleaDetails);
                                of_db.ALEAS_OF.Add(newAlea);

                                _db.SaveChanges();
                                newAlea.ID_OF_PROD_ORIG = newAlea.ID_OF_PROD_TRAITE;
                                _db.SaveChanges();

                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool SaveDbDeleteAlea(long? idAlea)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            try
            {
                List<ALEAS_OF> resultAlea = new List<ALEAS_OF>();
                IQueryable<ALEAS_OF> queryAlea = _db.ALEAS_OF.Where(p => p.ID == idAlea);
                resultAlea = queryAlea.ToList();
                ALEAS_OF alea_db;
                alea_db = resultAlea.FirstOrDefault();

                List<ALEAS_OF_DETAILS> tmp = alea_db.ALEAS_OF_DETAILS.ToList();
                foreach (var details in tmp)
                {
                    _db.Entry(details).State = System.Data.Entity.EntityState.Deleted;
                }
                _db.Entry(alea_db).State = System.Data.Entity.EntityState.Deleted;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}