using DAL_PEGASE;
using DALPEGASE;
using GenerateurDfu;
using InitFile;
using JAY.PegaseCore.Helper;
using JAY.XMLCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
//using DALPEGASE;
using System.Collections.Specialized;
using System.Configuration;

namespace TraitementOFs
{
    public class CodePack
    {
        List<string> codeMo = new List<string>();
        List<string> codeMt = new List<string>();
        List<string> nmrSim = new List<string>();
        int nbMo = 0;
        int nbMt = 0;
        int nbSim = 0;

        public List<string> CodeMo
        {
            get { return codeMo; }
        }
        public List<string> CodeMt
        {
            get { return codeMt; }
        }
        public List<string> NmrSim
        {
            get { return nmrSim; }

        }
        public int NbMo
        {
            get { return nbMo; }
        }
        public int NbMt
        {
            get { return nbMt; }
        }
        public int NbSim
        {
            get { return nbSim; }
        }
        public void Generatecode(int nbmo, int nbmt,int nbsim)
        {
            bool modetest = true; 
            List<CodePack> result = new List<CodePack>();
            for (int i = 0; i < nbmo; i++)
            {
                GestionCodeId code = new GestionCodeId();
                string code_str = "";
                if (modetest)
                {
                    code_str = "01";          
                }
                else
                {
                    code_str = code.GestionNewIdCode("CODE_MO"); 
                }
                
                int code_int = Convert.ToUInt16(code_str, 16);
                code_int = 0xFFFFF - code_int;
                code_str = code_int.ToString("X4");
                CodeMo.Add("0x"+code_str);
            }
            for (int i = 0; i < nbmt; i++)
            {
                GestionCodeId code = new GestionCodeId();
                string code_str = "";// 
                if (modetest)
                {
                    code_str = "FF";
                }
                else
                {
                    code_str = code.GestionNewIdCode("CODE_MT");
                }
                CodeMt.Add("0x"+ code_str);
            }
            for (int i = 0; i < nbsim; i++)
            {
                GestionCodeId code = new GestionCodeId();
                string code_str = "";// code.GestionNewIdCode("NUM_SIM");
                if (modetest)
                {
                    code_str = "TES";
                }
                else
                {
                    code_str = code.GestionNewIdCode("NUM_SIM");
                }
                
                string tmp = string.Concat(DateTime.Now.ToString("yy"), code_str);
                NmrSim.Add(tmp);
            }
        }
    }

    public class TraitementOF
    {
        List<pack> Rows = new List<pack>();
        String FPSRoot = "";
        String Repertoire_Travail = "";
        String Repertoire_Programmation = "";
        CodePack codeidMoMt = new CodePack();
        Dictionary<string, List<string>> RowCodeMo = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> RowCodeMt = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> RowNmrSim = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> RowCanauxPack = new Dictionary<string, List<string>>();
        int NbByPackMO = 0;
        int NbByPackMT = 0;
        string ModePontCouple = "";
        Dictionary<String,string> info_row = new Dictionary<string, string>();
        public TraitementOF()
        {
            IniFile ConfigFile = new IniFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\VerifStatusCmd.ini");
            string connectionstring = ConfigFile.GetValue("X3", "connectionstring");

            SqlConnection db1 = null;
            DataTable table = new DataTable();
            DataTable table1 = new DataTable();

            try
            {

                SqlCommand RequeteSql = new SqlCommand();
                string text = @"SELECT SORDERQ.SOHNUM_0,SORDERQ.SOPLIN_0,SORDERQ.DLVPIO_0,SORDER.ZSTATCDE_0, MFGHEAD.MFGTRKFLG_0,SORDERQ.BPCORD_0, BPCUSTOMER.BPCNAM_0, BPCUSTOMER.TSCCOD_2,BPCUSTOMER.REP_0, BPCUSTOMER.ZSTC_0, BPCUSTOMER.ZTCS_0, SORDERQ.ORDDAT_0, 
                            SORDERQ.DEMDLVDAT_0, SORDERQ.SHIDAT_0, SORDERQ.EXTDLVDAT_0, ITMMASTER.TCLCOD_0,SORDERQ.ITMREF_0, ITMMASTER.ITMDES1_0, ITMMASTER.ITMDES2_0, ITMMASTER.ITMDES3_0, ITMMASTER.CFGFLDALP1_0, ITMMASTER.CFGFLDALP2_0, 
                            ITMMASTER.CFGFLDALP3_0, ITMMASTER.CFGFLDALP4_0, MFGITM.MFGNUM_0, MFGITM.MFGSTA_0, MFGITM.STRDAT_0,MFGITM.ENDDAT_0,ZCDECARACT1.VAL_FPARAM_0, ZCDECARACT1.VAL_PLASTRON_0, ZCDECARACT1.VAL_SYNCHRO_0, 
                            ZCDECARACT1.VAL_CABLAGE_0, ZFPA.ZFPS_0, ZFPA.ZFPAV_0, ZFPA.ZFPSV_0, ZFPA.ZDATS_0, ZFPA.ZFPCONTROL_0,SORDERQ.QTY_0,ZFPA.ZVERSION_0,SORDER.ZDELAIOK_0,MFGMAT1.ITMREF_0 as 'MO-I',MFGMAT2.ITMREF_0 as 'MO-C',MFGMAT3.ITMREF_0 as 'MT-I',MFGMAT4.ITMREF_0 as 'MT-C', MFGMAT5.ITMREF_0 as 'SIM-I',MFGMAT6.ITMREF_0 as 'SIM-C',ITMMASTER.CFGLIN_0
                            
                            FROM CLTPROD.SORDERQ SORDERQ

                            JOIN CLTPROD.SORDER SORDER
                            ON SORDERQ.SOHNUM_0 = SORDER.SOHNUM_0

                            JOIN CLTPROD.BPCUSTOMER BPCUSTOMER
                            ON SORDERQ.BPCORD_0 = BPCUSTOMER.BPCNUM_0

                            JOIN CLTPROD.ITMMASTER ITMMASTER
                            ON SORDERQ.ITMREF_0 = ITMMASTER.ITMREF_0
                            AND((ITMMASTER.TCLCOD_0 = 'PF03'))

                            LEFT OUTER JOIN CLTPROD.MFGITM MFGITM
                            ON SORDERQ.SOHNUM_0 = MFGITM.VCRNUMORI_0
                            AND SORDERQ.SOPLIN_0 = MFGITM.VCRLINORI_0
                            AND SORDERQ.SOQSEQ_0 = MFGITM.VCRSEQORI_0
                            AND SORDERQ.ITMREF_0 = MFGITM.ITMREF_0

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
                            WHERE  SORDERQ.SOQSTA_0 not like '3' and (MFGITM.MFGSTA_0 not like '4' OR MFGITM.MFGSTA_0 IS NULL) and MFGHEAD.MFGTRKFLG_0 like '3' and (ITMMASTER.CFGLIN_0 like 'PACK' or ITMMASTER.CFGLIN_0 like 'MOB' or ITMMASTER.CFGLIN_0 like 'MOM' or ITMMASTER.CFGLIN_0 like 'MT' or ITMMASTER.CFGLIN_0 like 'SIM')
                            ORDER BY SORDERQ.SOHNUM_0, SORDERQ.SOPLIN_0
                            "; //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 

                // ITMREF ref com
                //CFGLIN egal indus 


                //HERE MFGITM.MFGSTA_0 not like '4' AND SORDERQ.SOQSTA_0 not like '3' MFGSTAT of clos 4 SOQSTA ligne de commande soldé == 3 

                db1 = new SqlConnection(connectionstring);
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = text;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                table.Load(monReader);


            }
            finally
            {
                db1.Close();
            }

            List<pack> Packs = new List<pack>();
            List<string> listOF = new List<string>();
            


            foreach (DataRow row in table.Rows)
            {
                listOF.Add(row["MFGNUM_0"].ToString());
            }
            DALPEGASE.PEGASE_PROD2Entities _db = new PEGASE_PROD2Entities();
            // on recherche les OF qui on été tracé
            List<string>  query = _db.OF_PROD_TRAITE.Where(p => listOF.Contains(p.NMROF) && p.ISALIVE == true && p.STATUSTYPE.Contains("INPROGRESS")).Select(i =>i.NMROF).ToList();
            List<pack> Packstmp = new List<pack>();
            if (query!= null && query.Count()>0)
            {
                foreach (DataRow row in table.Rows)
                {                    
                    if (query.Contains(row["MFGNUM_0"].ToString()))
                    {
                        Packstmp.Add(new pack(row));
                    }
                }
            }
            // pour les ponts couplés recherche les autre autre lie a cette ligne
            foreach (var pack in Packstmp)
            {
                string commande = pack.CmdClient.Trim();
                string ensemble_synch = pack.ens_synchr.Trim();
                if (!string.IsNullOrWhiteSpace(ensemble_synch) && ensemble_synch.Length > 10)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (commande == row["SOHNUM_0"].ToString() && row["VAL_SYNCHRO_0"].ToString().Contains(ensemble_synch.Substring(0,4)))
                        {
                            Packs.Add(new pack(row));
                        }
                    }
                }
            }
            // on a l'ensemble des of qui sont lie par les commandes synchro
            Packs.AddRange(Packstmp);
            TracabiliteProd traca = new TracabiliteProd();
            List<ORDRE_FABRICATION_GENERE> tmp = new List<ORDRE_FABRICATION_GENERE>();
            ORDRE_FABRICATION_GENERE tmpa = new ORDRE_FABRICATION_GENERE();
            foreach (var row in Packs)
            {
                tmp = traca.OFGenereExistedb2(row.NmrOF,false,false);
                if (tmp!=null && tmp.Count()>0)
                {
                    tmpa = tmp.First();
                }
                if ((tmpa != null) && tmpa.GENERE== false)
                {
                    // on recupere les ancienn modification manuelle
                    if (tmpa.MODIF_MANUEL)
                    {
                        row.ens_synchr = tmpa.COMMANDE_SYNCHRO;
                        row.firmwareMo = tmpa.REF_FIRMWARE_MO;
                        row.firmwareMt = tmpa.REF_FIRMWARE_MT;
                        row.RefCommercialePack = tmpa.REF_COMMERCIALE_PACK;
                        row.OptionsLogicielles = tmpa.OPTIONS_LOGICIELLES;
                        row.ConfMatMO = tmpa.OPTION_MATERIEL_MO;
                        row.ConfMatMT = tmpa.OPTION_MATERIEL_MT;
                        row.RefCommercialeMo = tmpa.REF_COMMERCIALE_MO;
                        row.RefIndustrielleMo = tmpa.REF_INDUSTRIELLE_MO;
                        row.RefCommercialeMt = tmpa.REF_COMMERCIALE_MT;
                        row.RefIndustrielleMt = tmpa.REF_INDUSTRIELLE_MT;
                        row.RefCommercialeSim = tmpa.REF_COMMERCIALE_SIM;
                        row.RefFichePerso = tmpa.REF_FICHE_PERSO;
                    }
                }
                Rows.Add(row);
            }
            //IniFile ConfigFile = new IniFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\VerifStatusCmd.ini");
            FPSRoot = ConfigFile.GetValue("DIRECTORY", "fps");
            Repertoire_Travail = ConfigFile.GetValue("DIRECTORY", "travail");
            Repertoire_Programmation = ConfigFile.GetValue("DIRECTORY", "programmation");
        } 

        public void Generate()
        {
            if (Rows.Count() > 0)
            {
                MajBddGenere();
                // creer les fiches des differents produits
                MAJFPI(); // maj des donnes de production
                GenererCodeId();
                MiseAJourFichePAckAProgrammer();                
                GenererDfuFile();
            }
        }
        private void MajBddGenere()
        {
            TracabiliteProd traca = new TracabiliteProd();
            foreach (var row in Rows)
            {
                List<ORDRE_FABRICATION_GENERE> list = traca.OFGenereExistedb2(row.NmrOF, false,false) ;
                if (list!=null && list.Count>0)
                {
                    ORDRE_FABRICATION_GENERE ofgenere = list.First();
                    if (ofgenere.MODIF_MANUEL == false)
                    {
                       if ( (ofgenere.GENERE != null && (bool)ofgenere.GENERE) &&
                            ofgenere.COMMANDE_SYNCHRO == row.ens_synchr &&
                            ofgenere.MARCHE == row.Marche &&
                            ofgenere.NB_PACK == row.QtrPack &&
                            ofgenere.NB_MO == row.NbMo &&
                            ofgenere.NB_MT == row.NbMt &&
                            ofgenere.NB_SIM == row.NbSim &&
                            ofgenere.NUM_COMMANDE_CLIENT == row.NumCommandeClient &&
                            ofgenere.NUM_LIGNE_CLIENT.Trim() == row.NumLigneClient &&
                            ofgenere.NUM_OF == row.NmrOF &&
                            ofgenere.NUM_SERIE_PACK == row.NumSeriePack &&
                            ofgenere.OPTIONS_LOGICIELLES == row.OptionsLogicielles &&
                            ofgenere.OPTION_MATERIEL_MO == row.ConfMatMO &&
                            ofgenere.OPTION_MATERIEL_MT == row.ConfMatMT &&
                            ofgenere.REF_COMMERCIALE_MO == row.RefCommercialeMo &&
                            ofgenere.REF_COMMERCIALE_MT == row.RefCommercialeMt &&
                            ofgenere.REF_COMMERCIALE_PACK == row.RefCommercialePack &&
                            ofgenere.REF_COMMERCIALE_SIM == row.RefCommercialeSim &&
                            ofgenere.REF_FICHE_PERSO == row.RefFichePerso &&
                            ofgenere.REF_FIRMWARE_MO == row.firmwareMo &&
                            ofgenere.REF_FIRMWARE_MT == row.firmwareMt &&
                            ofgenere.REF_INDUSTRIELLE_MO == row.RefIndustrielleMo &&
                            ofgenere.REF_INDUSTRIELLE_MT == row.RefIndustrielleMt)
                       {
                            // on ne fait rien
                       }
                       else
                       {
                            // on force la remise a jour des donnéee
                            ofgenere.COMMANDE_SYNCHRO = row.ens_synchr;
                            ofgenere.MARCHE = row.Marche;
                            ofgenere.NB_PACK = row.QtrPack;
                            ofgenere.NB_MO = row.NbMo;
                            ofgenere.NB_MT = row.NbMt ;
                            ofgenere.NB_SIM = row.NbSim;
                            ofgenere.NUM_COMMANDE_CLIENT = row.NumCommandeClient;
                            ofgenere.NUM_LIGNE_CLIENT = row.NumLigneClient;
                            ofgenere.NUM_OF = row.NmrOF;
                            ofgenere.NUM_SERIE_PACK = row.NumSeriePack;
                            ofgenere.OPTIONS_LOGICIELLES = row.OptionsLogicielles;
                            ofgenere.OPTION_MATERIEL_MO = row.ConfMatMO;
                            ofgenere.OPTION_MATERIEL_MT = row.ConfMatMT;
                            ofgenere.REF_COMMERCIALE_MO = row.RefCommercialeMo;
                            ofgenere.REF_COMMERCIALE_MT = row.RefCommercialeMt;
                            ofgenere.REF_COMMERCIALE_PACK = row.RefCommercialePack;
                            ofgenere.REF_COMMERCIALE_SIM = row.RefCommercialeSim;
                            ofgenere.REF_FICHE_PERSO = row.RefFichePerso;
                            ofgenere.REF_FIRMWARE_MO = row.firmwareMo;
                            ofgenere.REF_FIRMWARE_MT = row.firmwareMt;
                            ofgenere.REF_INDUSTRIELLE_MO = row.RefIndustrielleMo;
                            ofgenere.REF_INDUSTRIELLE_MT = row.RefIndustrielleMt;
                            ofgenere.GENERE = false;
                            ofgenere.DATE_GENERATION = null;
                            // sauvegarde des modification
                            traca.SaveDb2(ofgenere);
                        }
                    }
                    else
                    {
                        // forcage manuel donc on change rien
                    }
                }
                else
                {
                    ORDRE_FABRICATION_GENERE newof = new ORDRE_FABRICATION_GENERE();
                    newof.MODIF_MANUEL = false;
                    newof.COMMANDE_SYNCHRO =row.ens_synchr;
                    newof.DATE_GENERATION = null;
                    newof.GENERE = false;
                    newof.MARCHE = row.Marche;
                    newof.NB_PACK = row.QtrPack;
                    newof.NB_MO = row.NbMo;
                    newof.NB_MT = row.NbMt;
                    newof.NB_SIM = row.NbSim;
                    newof.NUM_COMMANDE_CLIENT = row.NumCommandeClient;
                    newof.NUM_LIGNE_CLIENT = row.NumLigneClient;
                    newof.NUM_OF = row.NmrOF;
                    newof.NUM_SERIE_PACK = row.NumSeriePack;
                    newof.OPTIONS_LOGICIELLES = row.OptionsLogicielles;
                    newof.OPTION_MATERIEL_MO = row.ConfMatMO;
                    newof.OPTION_MATERIEL_MT = row.ConfMatMT;
                    newof.REF_COMMERCIALE_MO = row.RefCommercialeMo;
                    newof.REF_COMMERCIALE_MT =row.RefCommercialeMt;
                    newof.REF_COMMERCIALE_PACK = row.RefCommercialePack;
                    newof.REF_COMMERCIALE_SIM = row.RefCommercialeSim;
                    newof.REF_FICHE_PERSO = row.RefFichePerso;
                    newof.REF_FIRMWARE_MO =row.firmwareMo;
                    newof.REF_FIRMWARE_MT =row.firmwareMt;
                    newof.REF_INDUSTRIELLE_MO =row.RefIndustrielleMo;
                    newof.REF_INDUSTRIELLE_MT = row.RefIndustrielleMt;
                    traca.SaveDb2(newof);
                }
            }
        }
        private void AddTracabilite(pack ligne,int cpt)
        {

            //      SELECT TOP (1000)[ID]
            //,[ID_PACK_INSTALLE]
            //,[NUM_SERIE_MO]
            //,[CODE_IDENTITE_NATIF]
            //,[CODE_IDENTITE_APPRENTISSAGE]
            //,[DATE_FABRICATION]
            //,[CODE_ARTICLE_LOGICIEL]
            //,[VERSION_LOGICIELLE]
            //,[DATE_DERNIERE_MAJ]
            //,[REF_INDUSTRIELLE]
            //,[NUM_ORDRE]
            //,[NUM_SERIE_CARTE_TEST]

            TracabiliteProd traca = new TracabiliteProd();
            if (ligne.NbMo > 0)
            {
               
                traca.mo = new DALPEGASE.MO();
                // traca.mo.ID_PACK_INSTALLE = "";
                traca.mo.NUM_SERIE_MO = ligne.NumSeriePackProduit+"E1";
                traca.mo.CODE_ARTICLE_LOGICIEL = ligne.ConfMatMO;
                traca.mo.CODE_IDENTITE_APPRENTISSAGE = (int)ligne.Marche[0];
                traca.mo.CODE_IDENTITE_NATIF =ligne.codeidMO.Substring(2, 5);
                traca.mo.DATE_FABRICATION = null;
                traca.mo.REF_INDUSTRIELLE = ligne.RefIndustrielleMo;
            }
            
            if (ligne.NbMt > 0)
            {
                traca.mt = new DALPEGASE.MT();
                traca.mt.NUM_SERIE_MT = ligne.NumSeriePackProduit+"R1";
                traca.mt.CODE_ARTICLE_LOGICIEL = ligne.ConfMatMT;
                traca.mt.CODE_IDENTITE_NATIF = ligne.codeidMT.Substring(2, ligne.codeidMT.Length-2);
                traca.mt.CODE_IDENTITE_APPRENTISSAGE = (int)ligne.Marche[0];
                traca.mt.DATE_FABRICATION = null;
                traca.mt.REF_INDUSTRIELLE = ligne.RefIndustrielleMo;
            }
            if (ligne.NbSim > 0)
            {
                //traca.sim.ID_PACK_INSTALLE = "";
                traca.sim = new DALPEGASE.SIM();
                traca.sim.NUM_SERIE_SIM = ligne.nmrsim;
                traca.sim.DATE_FABRICATION = null;
                traca.sim.REF_INDUSTRIELLE = ligne.RefCommercialeSim;

            }
            traca.pack = new DALPEGASE.PACK_INSTALLE();
            traca.pack.DATE_PROGRAMMATION = null;
            traca.pack.NUM_ORDRE = cpt + 1;
            traca.pack.NUM_SERIE_PACK = ligne.NumSeriePackProduit;
            traca.pack.REF_COMMERCIALE_PACK = ligne.RefCommercialePack;

            traca.of.NUM_OF = ligne.NmrOF;
            traca.of.NUM_COMMANDE_CLIENT = ligne.NumCommandeClient;
            traca.of.REF_INDUSTRIELLE_MO = ligne.RefIndustrielleMo;
            traca.of.OPTION_MATERIEL_MO = ligne.ConfMatMO;
            traca.of.REF_INDUSTRIELLE_MT = ligne.RefIndustrielleMt;
            traca.of.OPTION_MATERIEL_MT = ligne.ConfMatMT;
            traca.of.REF_COMMERCIALE_MO = ligne.RefCommercialeMo;
            traca.of.REF_COMMERCIALE_MT = ligne.RefCommercialeMt;
            traca.of.REF_COMMERCIALE_SIM = ligne.RefCommercialeSim;
            traca.of.REF_FIRMWARE_MO = ligne.firmwareMo;
            traca.of.REF_FIRMWARE_MT = ligne.firmwareMt;
            traca.of.REF_COMMERCIALE_PACK = ligne.RefCommercialePack;
            traca.of.OPTIONS_LOGICIELLES = ligne.OptionsLogicielles;
            traca.of.REF_FICHE_PERSO = ligne.RefFichePerso;
            traca.of.NUM_SERIE_PACK = ligne.NumSeriePack;
            traca.of.NB_PACK = ligne.QtrPack;
            traca.of.NB_MO = ligne.NbMo;
            traca.of.NB_MT = ligne.NbMt;
            traca.of.NB_SIM = ligne.NbSim;
            traca.of.COMMANDE_SYNCHRO = ligne.ens_synchr;
            traca.of.DATE_PROGRAMMATION = DateTime.Now;
            traca.Savedb();

        }
        // mise a jour des données lide fonctionnement codeid et numserie
        private void MiseAJourFichePAckAProgrammer()
        {
            foreach (pack lignerow in Rows)
            {
                //Completer la ligne pour la generation de l'image
               pack ligne =CompleterRefLigne( lignerow);
                
                if (ligne.NbMo != 0 || ligne.NbMt != 0 || ligne.NbSim != 0)
                {
                    for (int cpt = 0; cpt < ligne.QtrPack; cpt++)
                    {
                        ligne.NumSeriePackProduit = ligne.NumSeriePack + String.Format(@"{0:00}", cpt + 1);
                        string DestFile = Repertoire_Travail + String.Format(@"\FPI{0}{1:00}.iDialog", ligne.NumSeriePack, cpt + 1);
                        JAY.FileCore.iDialogPackage package = JAY.FileCore.iDialogPackage.OpenPackage(DestFile, FileMode.Open);
                        XMLProcessing XProcess = new JAY.XMLCore.XMLProcessing();
                        XProcess.OpenXML(package.GetCurrentVersionFileStream());
                        ObservableCollection<XElement> XEl1;

                        // Mettre à jour le numéro de série du pack
                        XEl1 = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/NumSeriePack");
                        if (XEl1 != null)
                        {
                            XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = ligne.NumSeriePack + String.Format(@"{0:00}", cpt + 1);
                        }

                        XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/DonneesDeFab/NumeroDeSerie");
                        if (XEl1 != null)
                        {
                            string toto = ligne.NumSeriePack + String.Format(@"{0:00}", cpt + 1) + "R1";
                            XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = ByteHelper.GetBlobStringFromString(ligne.NumSeriePack + String.Format(@"{0:00}", cpt + 1) + "R1");
                        }
                        XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/DonneesDeFab/NumeroDeSerie");
                        if (XEl1 != null)
                        {
                            XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = ByteHelper.GetBlobStringFromString(ligne.NumSeriePack + String.Format(@"{0:00}", cpt + 1) + "E1");
                        }
                        if (!String.IsNullOrWhiteSpace(ligne.ConfMatMO) && ligne.ConfMatMO.Contains("_X"))
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/AtexMo");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(ligne.ConfMatMT) && (ligne.ConfMatMT.Contains("_X")))
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/OptionsMaterielles/AtexMt");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(ligne.ConfMatMO) && (ligne.ConfMatMO.Substring(4, 1).Equals("5") || ligne.ConfMatMO.ToString().Substring(4, 1).Equals("6")))
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/Vibreur");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                            }
                        }

                        List<string> code = new List<string>();
                        bool existe = RowNmrSim.TryGetValue(ligne.NmrOF, out code);
                        if (existe && code.Count > cpt)
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationSIM/SIM/DonneesDeFab/NumeroDeSerie");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = ByteHelper.GetBlobStringFromString(code.ElementAt(cpt));
                                ligne.nmrsim = code.ElementAt(cpt);
                            }
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/NmrSimTraceur");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = ByteHelper.GetBlobStringFromString(code.ElementAt(cpt));
                            }
                        }
                        existe = RowCodeMo.TryGetValue(ligne.NmrOF, out code);
                        if (existe && code.Count > cpt)
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/DonneesDeFab/CodeIdNatifMO");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = code.ElementAt(cpt);
                                ligne.codeidMO = code.ElementAt(cpt);
                            }
                        }
                        else
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/DonneesDeFab/CodeIdNatifMO");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x0000";
                            }
                        }
                        existe = RowCodeMt.TryGetValue(ligne.NmrOF, out code);
                        if (existe && code.Count > cpt)
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/DonneesDeFab/CodeIdNatifMT");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = code.ElementAt(cpt);
                                ligne.codeidMT = code.ElementAt(cpt);
                            }
                        }
                        else
                        {
                            XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/DonneesDeFab/CodeIdNatifMT");
                            if (XEl1 != null)
                            {
                                XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x0000";
                            }
                        }

                        ObservableCollection<XElement> XEl1_code; ObservableCollection<XElement> XEl1_canal; ObservableCollection<XElement> XEl2_code; ObservableCollection<XElement> XEl2_canal;
                        XEl1_code = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableIDNatif");
                        XEl1_canal = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMTPack/TableCanal");
                        XEl2_code = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableIDNatif");
                        XEl2_canal = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/TableDesMOPack/TableCanal");

                        CompleterTableMTMOPACK(cpt, ref XEl1_code, ref XEl1_canal, ref XEl2_code, ref XEl2_canal);
                        //mise a jour de la table des MTPACK et MOPACK
                        // Liste des Codes natifs MT

                        XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/GammeFreq");
                        string value = XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                        //uniquement pour le 433Mhz
                        int tmpint = 0;
                        if (!String.IsNullOrEmpty(value))
                        {
                            if (value.Contains("0x"))
                            {
                                tmpint = Convert.ToUInt16(value, 16);
                            }
                            else
                            {
                                tmpint = Convert.ToUInt16(value, 10);
                            }
                            if (tmpint.Equals(1))
                            {
                                // nous sommes en 433
                                if (ligne.ConfMatMO.Contains("_X"))
                                {
                                    XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/PuissMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x0A";
                                }
                                else
                                {
                                    XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/PuissMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x0F";
                                }
                                XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/DebitMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x04";
                            }
                            else if (tmpint.Equals(3) || tmpint.Equals(4))
                            {
                                XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/DebitMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x04";
                            }
                        }
                        XEl1 = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/GammeFreq");
                        value = XEl1.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                        //uniquement pour le 433Mhz
                        tmpint = 0;
                        if (!String.IsNullOrEmpty(value))
                        {
                            if (value.Contains("0x"))
                            {
                                tmpint = Convert.ToUInt16(value, 16);
                            }
                            else
                            {
                                tmpint = Convert.ToUInt16(value, 10);
                            }
                            if (tmpint.Equals(1))
                            {
                                // nous sommes en 433
                                XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/PuissMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x0F";
                                XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/DebitMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x04";
                            }
                            else if (tmpint.Equals(3) || tmpint.Equals(4))
                            {
                                XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/DebitMax").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "0x04";
                            }
                        }
                        package.SaveNewVersion(XProcess.RootNode.ToString());

                        // XProcess.SaveXML(package.GetCurrentVersionFileStream());
                        package.ClosePackage();
                        AddTracabilite(ligne,cpt);
                        GenerateDataProd(Repertoire_Programmation, ligne, cpt);
                    }
                }
            }
        }
        private void MAJFPI()
        {
            Regex fps = new Regex (@"(?i)((^FPS\d{5}_\d{2}.idialog$)|(^[@BCDFGLMSJ]FP\d{4}.idialog$)|(^Base\d{7,9}.idialog$))");
            DirectoryInfo dir;
            FileInfo[] fileInfo;

            foreach (pack ligne in Rows)
            {
                if (ligne.NbMo == 0)
                {
                    ligne.ConfMatMO = "B21A0000X";
                    ligne.RefIndustrielleMo =  "B2-000";
                    ligne.RefCommercialeMo = ligne.Marche + "B20000";
                }
                if (ligne.NbMt == 0)
                {
                    ligne.ConfMatMT =  "T11EX000X";
                    ligne.RefIndustrielleMt =  "T1-000";
                    ligne.RefCommercialeMt = ligne.Marche + "T10000";
                }
                if (ligne.NbSim == 0)
                {
                    ligne.OptionsLogicielles =  "00X0X0";
                    ligne.RefCommercialeSim = ligne.Marche + "S000";
                    ligne.RefIndustrielleSim =  "S000";
                }
                //ligne.NumSeriePack;
                if (ligne.NbMo !=0 || ligne.NbMt != 0 || ligne.NbSim !=0)
                {
                    if (ligne.RefFichePerso.Equals("FBASE"))
                    {
                        fileInfo = CreateFPSFromRefIndus(ligne);
                    }
                    else
                    {
                        dir = new DirectoryInfo(FPSRoot + "\\" + ligne.RefFichePerso);
                        fileInfo = dir.GetFiles(ligne.RefFichePerso + "*.idialog");
                    }

                    string fpsTravail = "";
                    foreach (FileInfo filename in fileInfo)
                    {
                        if (fps.IsMatch(filename.Name))
                        {
                            fpsTravail = filename.FullName;
                            break;
                        }
                    }
                    //ligne.NmrOF= ligne.NumSeriePack;
                    for (int cpt = 0; cpt < ligne.QtrPack; cpt++)
                    {
                        string DestFile = Repertoire_Travail + String.Format(@"\FPI{0}{1:00}.iDialog", ligne.NumSeriePack, cpt + 1);
                        File.Copy(fpsTravail, DestFile, true);
                        info_row = MAJFPI(DestFile, ligne);
                    }
                }
            }
        }

        private void GenerateDataProd(string DestFile, pack ligne,int cpt)
        {
            List<string> lines = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(DestFile + "\\"+ligne.NmrOF);
            if (!dir.Exists)
            {
                dir.Create();
            }
            dir = new DirectoryInfo(DestFile + "\\" + ligne.NmrOF + "\\" + ligne.NmrOF + String.Format("{0:00}",cpt+1));
            if (!dir.Exists)
            {
                dir.Create();
            }
            FileInfo file = new FileInfo(dir.FullName + "\\" + ligne.NmrOF + String.Format("{0:00}", cpt + 1)+".ini");
            lines .Add("[PRODUCT]");
            lines.Add("OF=" + ligne.NmrOF + String.Format("{0:00}", cpt + 1));
            if (!string.IsNullOrWhiteSpace(ligne.RefIndustrielleMo)&& ligne.NbMo>0)
            {
                lines.Add("MO="+ligne.RefIndustrielleMo);
                lines.Add("MOID=" + ligne.codeidMO.Substring(2, ligne.codeidMO.Length-2));
            }
            else
            {
                lines.Add("MO=");
                lines.Add("MOID=");
            }
            if (!string.IsNullOrWhiteSpace(ligne.RefIndustrielleMt) && ligne.NbMt > 0)
            {
                lines.Add("MT=" + ligne.RefIndustrielleMt);
                lines.Add("MTID=" + ligne.codeidMT.Substring(2, ligne.codeidMT.Length-2));
            }
            else
            {
                lines.Add("MT=");
                lines.Add("MTID=");
            }
            if (!string.IsNullOrWhiteSpace(ligne.RefCommercialeSim) && ligne.NbSim > 0)
            {
                lines.Add("SIM=" + ligne.RefCommercialeSim);
                lines.Add("SIMID=" + ligne.nmrsim);
                lines.Add("USB=OUI");
            }
            else
            {
                lines.Add("SIM=");
                lines.Add("SIMID=");
                lines.Add("USB=NON");
            }
            lines.Add("NMR_ORDRE="+ String.Format("{0:00}", cpt + 1));
            lines.Add("NMR_SERIE=" + ligne.NmrOF.Substring(1, ligne.NmrOF.Length-1)+ String.Format("{0:00}", cpt + 1));
            
            System.IO.File.WriteAllLines(file.FullName, lines.ToArray());
        }
        private Dictionary<String,String> MAJFPI(String FPIFilename, pack OF)
        {
            Dictionary<String,String> info = new Dictionary<String, String>();
            JAY.FileCore.iDialogPackage package = JAY.FileCore.iDialogPackage.OpenPackage(FPIFilename, FileMode.Open);

            XMLProcessing XProcess = new JAY.XMLCore.XMLProcessing();
            XProcess.OpenXML(package.GetCurrentVersionFileStream());

            ObservableCollection<XElement> XEl;
            // Mettre à jour la référence de la fiche (FPA + FPS)
            String stringtmp = "";
            String stringtmp2 = "";

            //String RefFiche 
            stringtmp = XProcess.GetValue("XmlIdentification/Ident/IdentAffaire/NumFichePerso", "", "", XML_ATTRIBUTE.VALUE);
            stringtmp += " / " + OF.RefFichePerso;
            XProcess.SetValue("XmlIdentification/Ident/IdentAffaire/NumFichePerso", "", "", XML_ATTRIBUTE.VALUE, stringtmp);

            // Mettre à jour le numéro de commande du client
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentAffaire/NumCmdOI");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.NumCommandeClient;
            }

            // Mettre à jour le numéro d'OF
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentAffaire/NumOF");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = "F" + OF.NumSeriePack;
            }

            //// Mettre à jour le numéro de série du pack
            //XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/NumSeriePack");
            //if (XEl != null)
            //{
            //    XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.NumSeriePack + String.Format(@"{0:00}", cpt + 1);
            //}

            // Mettre à jour la date de programmation
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/DateProg");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = DateTime.Now.ToString();
            }

            // Mettre à jour la version de Firmware MT
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/FirmwMT");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.firmwareMt;
            }

            // Mettre à jour la version de Firmware MO
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/FirmwMO");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.firmwareMo;// OF.RefFirmwareMo;
            }

            // Mettre à jour la version de la config de production
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/VersionConfigProd");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = package.CurrentVersion.ToString();
            }

            // Mettre à jour la référence commerciale du pack
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/RefComPack");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.RefCommercialePack;
            }

            // Mettre à jour la référence des options softs
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentPack/RefOptSoft");
            if (XEl != null && OF.NbSim > 0)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = OF.OptionsLogicielles;
            }

            // Mettre à jour la référence industrielle MO
            XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/IdentificationERP/RefIndus");
            if (XEl != null)
            {
                if (OF.NbMo > 0)
                {
                    XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = JAY.XMLCore.Tools.ConvertFromText2ASCII(OF.RefIndustrielleMo);
                }
            }

            // Mettre à jour la référence industrielle MT
            XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/IdentificationERP/IdentERP");
            if (XEl != null)
            {
                if (OF.NbMt > 0)
                {
                    XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = JAY.XMLCore.Tools.ConvertFromText2ASCII(OF.RefIndustrielleMt);
                }
            }

            // Mettre à jour la référence de la SIM
            XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/RefIndus");
            if (XEl != null)
            {
                if (OF.NbSim > 0)
                {
                    XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = JAY.XMLCore.Tools.ConvertFromText2ASCII(OF.RefCommercialeSim);
                    // Mettre à jour les options logicielles
                    if (!this.MAJOptionsLogicielles(XProcess, OF.OptionsLogicielles))
                    {
                        // this.ErrorOF("Le format des données 'Options logicielles' est inconnu", OF, package);
                    }
                }
                else
                {
                    if (!this.MAJOptionsLogicielles(XProcess, "00X0X1"))
                    {
                       // this.ErrorOF("Le format des données 'Options logicielles' est inconnu", OF, package);
                    }
                }

            }

            // Mettre à jour les données marché CodeIdApprentissage dans la section MO et MT
            //String LettreMarche;
            Int32 CodeMarche;

           
            if (OF.Marche.Length > 0)
            {
                stringtmp = OF.Marche.ToUpper();
            }
            else if ((OF.NbSim > 0) && (!String.IsNullOrEmpty(OF.RefCommercialeSim)))
            {
                stringtmp = OF.RefCommercialeSim.Substring(0, 1);
            }
            else
            {
                stringtmp = "@";
            }
            
            if (stringtmp == "@")
            {
                CodeMarche = 99;
            }
            else if (stringtmp =="J")
            {
                CodeMarche = (Int32)'C' - 25;
            }
            else
            {
                CodeMarche = (Int32)stringtmp[0] - 25;
            }



            // MO
            XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/DonneesDeFab/CodeIdApprentissage");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = CodeMarche.ToString();
            }
            // MT
            XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/DonneesDeFab/CodeIdApprentissage");
            if (XEl != null)
            {
                XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value = CodeMarche.ToString();
            }

            // gerer le bouton safety
            bool pldDispo = false;
            try
            {
                int version = Convert.ToInt32(OF.firmwareMt.Substring(6, 4));
                if (version >= 245)
                {
                    pldDispo = true;
                }
            }
            catch
            {

            }
            if ( OF.ConfMatMO.Length >= 14)
            {
                if (OF.ConfMatMO.Substring(8, 1) == "4" || OF.ConfMatMO.Substring(8, 1) == "5" || OF.ConfMatMO.Substring(8, 1) == "6")
                {
                    // c'est un boutonn de secu
                    XEl = XProcess.GetNodeByPath("XmlMetier/OrganeCommande/DicoOrganeMO/configHardMO/");
                    string result = "";
                    bool found = false;
                    foreach (var organ in XEl)
                    {
                        List<XElement> list = organ.Descendants("Variable").ToList();
                        foreach (var variable in list)
                        {
                            if (variable.Attribute(XML_ATTRIBUTE.VALUE).Value == "A14")
                            {
                                result = variable.Attribute(XML_ATTRIBUTE.DESCRIPTION).Value;
                                found = true;
                                break;
                            }
                            if (found) break;
                        }
                        if (found) break;
                    }
                    if (found && pldDispo)
                    {
                        if (result.Equals("D31061A"))
                        {
                            XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/NiveauSecuriteMO/").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "3";
                        }
                        else if (result.Equals("D31062A"))
                        {
                            XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/NiveauSecuriteMO/").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "2";
                        }
                        else
                        {
                            XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/NiveauSecuriteMO/").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                        }
                    }
                    else
                    {
                        XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/OptionsMaterielles/NiveauSecuriteMO/").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                    }
                }
            }


            // Vérifier la cohérence des types MO
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/TypeMO");
            stringtmp2 = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
            //String TypeMOOF = "";
            if (XEl != null && OF.NbMo > 0)
            {
                //String TypeMOFiche; stringtmp2

                if (!String.IsNullOrEmpty(OF.ConfMatMO))
                {
                    stringtmp = OF.ConfMatMO.Substring(0, 1).ToUpper();
                    switch (stringtmp)
                    {
                        case "G":
                            stringtmp = "Gama";
                            break;
                        case "B":
                            stringtmp = "Beta";
                            break;
                        case "P":
                            stringtmp = "Pika";
                            break;
                        case "M":
                            stringtmp = "Moka";
                            break;
                        default:
                            stringtmp = stringtmp2;
                            break;
                    }


                    if (stringtmp.ToUpper() != stringtmp2.ToUpper())
                    {
                        // Erreur de cohérence, invalider l'OF
                       // this.ErrorOF("Le MO spécifié dans la référence et le MO spécifié dans la fiche sont différents, veuillez corriger cette erreur", OF, package);
                    }
                }
                OF.TypeMO = stringtmp;
            }

            // Vérifier la cohérence des types MT
            //String TypeMTOF = "";
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/TypeMT");
            stringtmp2 = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;

            if (XEl != null && OF.NbMt > 0)
            {
                //String TypeMTFiche;stringtmp2

                if (!String.IsNullOrEmpty(OF.ConfMatMT))
                {
                    stringtmp = OF.ConfMatMT.Substring(0, 1).ToUpper();
                    switch (stringtmp)
                    {
                        case "E":
                            stringtmp = "Elio";
                            break;
                        case "A":
                            stringtmp = "Alto";
                            break;
                        case "T":
                            stringtmp = "Timo";
                            break;
                        case "N":
                            stringtmp = "Nemo";
                            break;
                        default:
                            stringtmp = stringtmp2;
                            break;
                    }
                }
                OF.TypeMT = stringtmp;
                if (stringtmp2.ToUpper() != stringtmp.ToUpper())
                {
                    // Erreur de cohérence, invalider l'OF
                   // this.ErrorOF("Le MT spécifié dans la référence et le MT spécifié dans la fiche sont différents, veuillez corriger cette erreur", OF, package);
                }
            }
            NameValueCollection NVC = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");
            string too = NVC["MTIsPld"].ToString();
            if (NVC["MTIsPld"].ToString().ToLower() == "true" && pldDispo)
            {
                if (OF.TypeMT.Equals("Alto") || OF.TypeMT.Equals("Nemo"))
                {
                    try
                    {
                        XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/OptionsMaterielles/NiveauSecuriteMT/").First().Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                    }
                    catch { }
                }
            }
            // valider la fréquence du MO
            if (OF.NbMo > 0)
            {
                XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/FreqMO");
                if (XEl != null)
                {
                    // 1 - Lire la fréquence du MO dans la FPI
                    //String FrequenceMo = "0";
                    stringtmp = "0";
                    //ObservableCollection<XElement> frequencesMo = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/GammeFreqMO");
                    XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/GammeFreqMO");
                    if (XEl == null)
                    {
                        XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MO/ConfigMat/ModuleRadio/GammeFreq");
                    }

                    if (XEl != null)
                    {
                        stringtmp = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                        if (stringtmp.Contains("0x"))
                        {
                            stringtmp = Convert.ToInt32(stringtmp, 16).ToString();
                        }
                        else
                        {
                            stringtmp = Convert.ToInt32(stringtmp, 10).ToString();
                        }
                    }

                    // 2 - Lire la fréquence du MO suivant la codification
                    //String FrequenceConfMat = OF.ConfMatMO.Substring(2, 1);
                    stringtmp2 = OF.ConfMatMO.Substring(2, 1);
                    // 3 - Vérifier la cohérence des deux
                    if (stringtmp != stringtmp2)
                    {
                        // Erreur de cohérence, invalider l'OF
                        //this.ErrorOF("La fréquence MO spécifiée dans la référence et la fréquence MO spécifiée dans la fiche sont différents, veuillez corriger cette erreur", OF, package);
                    }
                }
            }

            // Valider la fréquence du MT
            if (OF.NbMt > 0)
            {
                XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/FreqMT");
                if (XEl != null)
                {
                    // 1 - Lire la fréquence du MO dans la FPI
                    // String FrequenceMt = "0";
                    stringtmp = "0";
                    //ObservableCollection<XElement> frequencesMt = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/GammeFreqMT");
                    XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/GammeFreqMT");

                    if (XEl == null)
                    {
                        XEl = XProcess.GetNodeByPath("XmlTechnique/IdentificationProduit/MT/ConfigMat/ModuleRadio/GammeFreq");
                    }

                    if (XEl != null)
                    {
                        stringtmp = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                        if (stringtmp.Contains("0x"))
                        {
                            stringtmp = Convert.ToInt32(stringtmp, 16).ToString();
                        }
                        else
                        {
                            stringtmp = Convert.ToInt32(stringtmp, 10).ToString();
                        }
                    }

                    // 2 - Lire la fréquence du MO suivant la codification
                    //String FrequenceConfMat = OF.ConfMatMT.Substring(2, 1);
                    stringtmp2 = OF.ConfMatMT.Substring(2, 1);
                    // 3 - Vérifier la cohérence des deux
                    if (stringtmp.ToUpper() != stringtmp2.ToUpper())
                    {
                        // Erreur de cohérence, invalider l'OF
                       // this.ErrorOF("La fréquence MT spécifiée dans la référence et la fréquence MO spécifiée dans la fiche sont différents, veuillez corriger cette erreur", OF, package);
                    }
                }
            }

            // Valider la tension du MT
            if (OF.NbMt > 0)
            {
                XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/TensionMT");
                if (XEl != null)
                {
                    // 1 - Lire la tension du MT dans la FPI
                    //String TensionMT = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                    stringtmp2 = XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                    Boolean ErreurTension = false;

                    // 2 - Lire la tension du MT suivant la codification
                    //String TensionMTOF 
                    stringtmp = OF.ConfMatMT.Substring(3, 1);
                    switch (OF.TypeMT.ToUpper())
                    {
                        case "ELIO":
                            if (stringtmp == "A" && !stringtmp2.Contains("12-24"))
                            {
                                ErreurTension = true;
                            }
                            else if (stringtmp == "B" && !stringtmp2.Contains("115-230"))
                            {
                                ErreurTension = true;
                            }
                            else if (stringtmp == "C" && !stringtmp2.Contains("11-24"))
                            {
                                ErreurTension = true;
                            }
                            break;
                        case "ALTO":
                            if (stringtmp == "1" && !stringtmp2.Contains("12-24 VDC 48-110-220 VAC secuB"))
                            {
                                ErreurTension = true;
                            }
                            break;
                        case "TIMO":
                            if (stringtmp != "E" || !stringtmp2.Contains("9-30"))
                            {
                                ErreurTension = true;
                            }
                            break;
                        case "NEMO":
                            if (stringtmp != "E" || !stringtmp2.Contains("9-30"))
                            {
                                ErreurTension = true;
                            }
                            break;
                        default:
                            ErreurTension = true;
                            break;
                    }

                    // 3 - Vérifier la cohérence des deux
                    if (ErreurTension)
                    {
                        //this.ErrorOF("Incohérence des données tensions entre la fiche de paramétrage et l'OF, veuillez la corriger avant de poursuivre", OF, package);
                    }
                }
            }

            // recuperation de certaine données de control pour generer l'of
            XEl = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDeCouplage/ModeCouplage");
            if (XEl != null)
            {
               ModePontCouple =  XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
            }

            // OF.TypeMO = TypeMOOF;
            // OF.TypeMT = TypeMTOF;

            // Enregistrer les changements
            package.SaveNewVersion(XProcess.RootNode.ToString());
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/FreqMT");
            if (XEl != null)
            {
                info.Add(StructLabel.FREQ, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/TensionMT");
            if (XEl != null)
            {
                info.Add(StructLabel.TENSION, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            XEl = XProcess.GetNodeByPath("XmlIdentification/Ident/IdentProduit/FreqMO");
            //if (XEl != null)
            //{
            //    info.Add(StructLabel.FREQ_MO, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            //}
            XEl = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionDesDroits/Utilisateur/TableUtilisateur/CodePin0");
            if (XEl != null)
            {
                info.Add(StructLabel.CODEPIN0, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            XEl = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionDesDroits/Utilisateur/TableUtilisateur/CodePin1");
            if (XEl != null)
            {
                info.Add(StructLabel.CODEPIN1, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            XEl = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionDesDroits/Utilisateur/TableUtilisateur/CodePin2");
            if (XEl != null)
            {
                info.Add(StructLabel.CODEPIN2, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            XEl = XProcess.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionDesDroits/Utilisateur/TableUtilisateur/CodePin3");
            if (XEl != null)
            {
                info.Add(StructLabel.CODEPIN3, XEl.First().Attribute(XML_ATTRIBUTE.VALUE).Value);
            }
            // XProcess.SaveXML(package.GetCurrentVersionFileStream());
            package.ClosePackage();
            return info;
        } // endMethod: MAJFPI
        private Boolean MAJOptionsLogicielles(XMLProcessing Xprocess, String OptLog)
        {
            if (OptLog.Length < 6)
            {
                return false;
            }
            String OptionIR, OptionExploit, OptionHommeMort, OptionDati, OptionCouplage, OptionEquation, OptionPiannotage, OptionEv, OptionCpt, OptionRadio;
            // Spliter le code Options Logicielles
            OptionIR = OptLog.Substring(0, 1);
            OptionExploit = OptLog.Substring(1, 1);
            OptionHommeMort = OptLog.Substring(2, 1);
            OptionDati = OptionHommeMort;
            OptionCouplage = OptLog.Substring(1, 1);
            OptionEquation = OptLog.Substring(4, 1);
            OptionCpt = OptLog.Substring(3, 1);
            OptionEv = OptLog.Substring(3, 1);
            OptionRadio = OptLog.Substring(5, 1);
            OptionPiannotage = OptLog.Substring(4, 1);

            // Trouver les valeurs correspondantes de l'embarqué
            OptionIR = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Detection_" + OptionIR];
            OptionExploit = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["ModeExploitation_" + OptionExploit];
            OptionHommeMort = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Homme mort_" + OptionHommeMort];
            OptionDati = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Dati_" + OptionDati];
            OptionCouplage = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Couplage_" + OptionCouplage];
            OptionEquation = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Equations_" + OptionEquation];
            OptionCpt = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["OptionCpt_" + OptionCpt];
            OptionPiannotage = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["Antipiannotage_" + OptionPiannotage];
            OptionEv = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["OptionEv_" + OptionEv];
            OptionRadio = JAY.PegaseCore.Helper.ResolvOptionsLogiciels.CodeERPCodeEmbarque["OptionRadioAuto_" + OptionRadio];

            // Ecrire les valeurs dans les options logicielles SIM et traçabilité
            // SIM
            Boolean Result = false;
            Result = Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionIR", "", "", XML_ATTRIBUTE.VALUE, OptionIR);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionExploit", "", "", XML_ATTRIBUTE.VALUE, OptionExploit);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionHommeMort", "", "", XML_ATTRIBUTE.VALUE, OptionHommeMort);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionDati", "", "", XML_ATTRIBUTE.VALUE, OptionDati);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionCouplage", "", "", XML_ATTRIBUTE.VALUE, OptionCouplage);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionEquation", "", "", XML_ATTRIBUTE.VALUE, OptionEquation);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionCpt", "", "", XML_ATTRIBUTE.VALUE, OptionCpt);
            if (Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/CanalAuto", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else if (Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionCanalAuto", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else if (Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionSup1", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else
            {
                Result = false;
            }
            bool change1 = Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/CanalAuto", "", "", XML_ATTRIBUTE.CODE, "OptionCanalAuto");

            Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionAlto", "", "", XML_ATTRIBUTE.VALUE, "0");
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionPiannotage", "", "", XML_ATTRIBUTE.VALUE, OptionPiannotage);
            Result &= Xprocess.SetValue("XmlTechnique/IdentificationSIM/SIM/DonneesConfigMat/IdentERP/OptionEv", "", "", XML_ATTRIBUTE.VALUE, OptionEv);

            // Traçabilité
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionIRTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionIR);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionExploitTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionExploit);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionHommeMortTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionHommeMort);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionDatiTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionDati);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCouplageTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionCouplage);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionEquationTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionEquation);
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCptTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionCpt);
            //Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCanalAutoTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionRadio);
            if (Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCanalAutoTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else if (Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCanalAuto", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else if (Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionSup1Traceur", "", "", XML_ATTRIBUTE.VALUE, OptionRadio))
            {
                Result &= true;
            }
            else
            {
                Result = false;
            }
            bool change2 = Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionCanalAuto", "", "", XML_ATTRIBUTE.CODE, "OptionCanalAutoTraceur");
            Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionAltoTraceur", "", "", XML_ATTRIBUTE.VALUE, "0");
            Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionPiannotageTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionPiannotage);
            //    Result &= Xprocess.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/Options/OptionEvTraceur", "", "", XML_ATTRIBUTE.VALUE, OptionEv);

            return Result;
        } // endMethod: MAJOptionsLogicielles

        private FileInfo[] CreateFPSFromRefIndus(pack ligne)
        {
            FileInfo[] result = new FileInfo[1];
            CreateAutoFPS CAF = new CreateAutoFPS();
            String directoryBase;

            IniFile ConfigFile = new IniFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\VerifStatusCmd.ini");

            directoryBase = ConfigFile.GetValue("DIRECTORY", "fps");
            string chemin = directoryBase + "\\FBASE\\" + "Base" + ligne.NumSeriePack.ToString() + ".idialog";
            CAF.CreateFPSFromRef(ligne.ConfMatMO, ligne.ConfMatMT, ligne.OptionsLogicielles, chemin);
            result[0] = new FileInfo( chemin);
            return result;
        }
       

        public void GenererCodeId()
        {
            Regex canaux = new Regex(@"C([0-9]{2})");
            codeidMoMt = new CodePack();
            int a = 1;
            // chercher le nombre de code necessaire

            foreach (pack row in Rows)
            {
                codeidMoMt = new CodePack();
                string OF = row.NmrOF;
                int qty = Convert.ToUInt16(row.QtrPack.ToString());
                int qtrmo = 0;int qtrmt = 0;int qtrsim = 0;
                if (!String.IsNullOrWhiteSpace(row.ConfMatMO))
                {
                    qtrmo = qty;
                    NbByPackMO = Math.Max(NbByPackMO, qty);
                }
                if (!String.IsNullOrWhiteSpace(row.ConfMatMT))
                {
                    qtrmt = qty;
                    NbByPackMT = Math.Max(NbByPackMT, qty);
                }
                if (!String.IsNullOrWhiteSpace(row.OptionsLogicielles))
                {
                    qtrsim = qty;
                }
                List<string> listcanaux = new List<string>();
                string ens_synchro = row.ens_synchr;
                foreach(Match m in canaux.Matches(ens_synchro))
                {
                    listcanaux.Add(m.Groups[1].Value);
                }
                if (listcanaux.Count < (qtrmt))
                {
                    listcanaux.Clear();
                    for (int i= 0; i< qtrmt; i++)
                    {
                        listcanaux.Add(( 10+2*a).ToString());
                        a++;
                    }
                    
                }
                
                codeidMoMt.Generatecode(qtrmo, qtrmt,qtrsim);
                RowCodeMo.Add(OF, codeidMoMt.CodeMo);
                RowCodeMt.Add(OF, codeidMoMt.CodeMt);
                RowNmrSim.Add(OF, codeidMoMt.NmrSim);
                RowCanauxPack.Add(OF, listcanaux);
            }
        }
        private void CompleterTableMTMOPACK(int cpt, ref  ObservableCollection<XElement> xe1_code, ref ObservableCollection<XElement> xe1_canal, ref ObservableCollection<XElement> xe2_code, ref ObservableCollection<XElement> xe2_canal)
        {
            if ((ModePontCouple != "0") && (ModePontCouple != ""))
            {
            XElement CodeNatifMT = new XElement( xe1_code.Descendants().First());
            XElement CanalRadioMT = new XElement(xe1_canal.Descendants().First());
            XElement CodeNatifMO = new XElement(xe2_code.Descendants().First());
            XElement CanalRadioMO = new XElement(xe2_canal.Descendants().First());
            xe1_code.Descendants().Remove();
            xe1_canal.Descendants().Remove();
            xe2_code.Descendants().Remove();
            xe2_canal.Descendants().Remove();

            int nbproduit = 0;
            XElement CodeNatifMT1 = new XElement(CodeNatifMT);
            XElement CanalRadioMT1 = new XElement(CanalRadioMT);
            XElement CodeNatifMO1 = new XElement(CodeNatifMO);
            XElement CanalRadioMO1 = new XElement(CanalRadioMO);

                foreach (var code in RowCodeMo)
                {
                    if (code.Value.Count > cpt)
                    {
                        string codemo = code.Value.ElementAt(cpt);
                        CodeNatifMO1.Attribute("valeur").Value = codemo;
                        List<string> canaux = new List<string>();
                        RowCanauxPack.TryGetValue(code.Key, out canaux);
                        if (canaux.Count > cpt)
                            CanalRadioMO1.Attribute("valeur").Value = canaux.ElementAt(cpt);
                        xe2_code.First().Add(CodeNatifMO1);
                        xe2_canal.First().Add(CanalRadioMO1);

                        string adr = CodeNatifMO.Attribute("offsetAbsolu").Value;
                        int adr_int = Convert.ToInt32(adr) + 4;
                        CodeNatifMO.Attribute("offsetAbsolu").Value = adr_int.ToString();
                        adr = CodeNatifMO.Attribute("offsetRelatif").Value;
                        adr_int = Convert.ToInt32(adr) + 4;
                        CodeNatifMO.Attribute("offsetRelatif").Value = adr_int.ToString();

                        adr = CanalRadioMO.Attribute("offsetAbsolu").Value;
                        adr_int = Convert.ToInt32(adr) + 1;
                        CanalRadioMO.Attribute("offsetAbsolu").Value = adr_int.ToString();
                        adr = CanalRadioMO.Attribute("offsetRelatif").Value;
                        adr_int = Convert.ToInt32(adr) + 1;
                        CanalRadioMO.Attribute("offsetRelatif").Value = adr_int.ToString();

                        CodeNatifMO.Attribute("valeur").Value = "0xFFFF";
                        CanalRadioMO.Attribute("valeur").Value = "0xFF";
                        CodeNatifMO1 = new XElement(CodeNatifMO);
                        CanalRadioMO1 = new XElement(CanalRadioMO);

                    }
                    nbproduit++;
                }
                xe2_code.First().Add(xe2_code.First().DescendantNodes().Count(), CodeNatifMO);
                xe2_canal.First().Add(xe2_canal.First().DescendantNodes().Count(), CanalRadioMO);
                nbproduit = 0;
                foreach (var code in RowCodeMt)
                {
                    if (code.Value.Count > cpt)
                    {
                        string codemt = code.Value.ElementAt(cpt);
                        CodeNatifMT1.Attribute("valeur").Value = codemt;
                        List<string> canaux = new List<string>();
                        RowCanauxPack.TryGetValue(code.Key, out canaux);
                        if (canaux.Count > cpt)
                            CanalRadioMT1.Attribute("valeur").Value = canaux.ElementAt(cpt);
                        xe1_code.First().Add( CodeNatifMT1);
                        xe1_canal.First().Add( CanalRadioMT1);

                        string adr = CodeNatifMT.Attribute("offsetAbsolu").Value;
                        int adr_int = Convert.ToInt32(adr) + 4;
                        CodeNatifMT.Attribute("offsetAbsolu").Value = adr_int.ToString();
                        adr = CodeNatifMT.Attribute("offsetRelatif").Value;
                        adr_int = Convert.ToInt32(adr) + 4;
                        CodeNatifMT.Attribute("offsetRelatif").Value = adr_int.ToString();

                        adr = CanalRadioMT.Attribute("offsetAbsolu").Value;
                        adr_int = Convert.ToInt32(adr) + 1;
                        CanalRadioMT.Attribute("offsetAbsolu").Value = adr_int.ToString();
                        adr = CanalRadioMT.Attribute("offsetRelatif").Value;
                        adr_int = Convert.ToInt32(adr) + 1;
                        CanalRadioMT.Attribute("offsetRelatif").Value = adr_int.ToString();

                        CodeNatifMT.Attribute("valeur").Value = "0xFFFF";
                        CanalRadioMT.Attribute("valeur").Value = "0xFF";

                        CodeNatifMT1 = new XElement(CodeNatifMT);
                        CanalRadioMT1 = new XElement(CanalRadioMT);
                    }
                    nbproduit++;
                }
                xe1_code.First().Add(xe1_code.First().DescendantNodes().Count(), CodeNatifMO);
                xe1_canal.First().Add(xe1_canal.First().DescendantNodes().Count(), CanalRadioMO);
            }
        }
        // genrere les fichier DFU
        private void GenererDfuFile()
        {
            bool result = true;
            foreach (pack ligne in Rows)
            {
                if (ligne.NbMo != 0 || ligne.NbMt != 0 || ligne.NbSim != 0)
                {
                    List<string> codeMO = new List<string>();
                    List<string> codeMT = new List<string>();
                    List<string> nmrSim = new List<string>();

                    RowCodeMo.TryGetValue(ligne.NmrOF, out codeMO);
                    RowCodeMt.TryGetValue(ligne.NmrOF, out codeMT);
                    RowNmrSim.TryGetValue(ligne.NmrOF, out nmrSim);

                    for (int cpt = 0; cpt < ligne.QtrPack; cpt++)
                    {
                        string idialogFileName = String.Format(@"\FPI{0}{1:00}.iDialog", ligne.NumSeriePack, cpt + 1);
                        string repertoire_prog = Repertoire_Programmation + "\\" + ligne.NmrOF + "\\" + ligne.NmrOF + String.Format("{0:00}", cpt + 1) + "\\";
                        string DestFile = Repertoire_Travail + "\\" + idialogFileName;
                        GenererDfuEeprom image = new GenererDfuEeprom();
                        image.GenerationHexImage(DestFile);
                        if (!string.IsNullOrWhiteSpace(ligne.RefIndustrielleMo) && ligne.NbMo > 0)
                        {
                            ligne.codeidMO = codeMO.ElementAt(cpt);
                            string ProgDfufile = repertoire_prog + ligne.RefIndustrielleMo + "_" + ligne.codeidMO.ToString().Substring(2, ligne.codeidMO.Length-2);
                            image.GenrateurDFUFile(ProgDfufile);

                        }
                        if (!string.IsNullOrWhiteSpace(ligne.RefIndustrielleMt) && ligne.NbMt > 0)
                        {
                            ligne.codeidMT = codeMT.ElementAt(cpt);
                            ligne.nmrMo = ligne.NmrOF.Substring(1, ligne.NmrOF.Length - 1) + String.Format("{0:00}", cpt + 1) + "E1";
                            string ProgDfufile = repertoire_prog + ligne.RefIndustrielleMt + "_" + ligne.codeidMT.ToString().Substring(2, ligne.codeidMT.Length-2);
                            image.GenrateurDFUFile(ProgDfufile);
                        }
                        if (!string.IsNullOrWhiteSpace(ligne.RefIndustrielleSim) && ligne.NbSim > 0)
                        {
                            ligne.nmrsim = nmrSim.ElementAt(cpt);
                            ligne.nmrMt = ligne.NmrOF.Substring(1, ligne.NmrOF.Length - 1) + String.Format("{0:00}", cpt + 1) + "R1";
                            string ProgDfufile = repertoire_prog + ligne.RefCommercialeSim + "_" + ligne.nmrsim.ToString();
                            image.GenrateurDFUFileSim(ProgDfufile);
                        }
                        string ProgFile = Repertoire_Programmation + "\\" + ligne.NmrOF + "\\" + ligne.NmrOF + String.Format("{0:00}", cpt + 1) + "\\" + idialogFileName;
                        File.Copy(DestFile, ProgFile, true);
                        GenerateCmdFile(repertoire_prog, ligne);
                        result &= CopyFirmwareFile(repertoire_prog, ligne);
                    }
                }
                // mettre a jour la base de tracabilite 
                if (result == true)
                {
                    TracabiliteProd traca = new TracabiliteProd();
                    List<ORDRE_FABRICATION_GENERE> list = traca.OFGenereExistedb2(ligne.NmrOF, false, false);
                    if (list!= null && list.Count>0)
                    {
                        ORDRE_FABRICATION_GENERE tmp = list.First();
                        tmp.GENERE = true;
                        tmp.DATE_GENERATION = DateTime.Now;
                        traca.SaveDb2(tmp);
                    }
                    // la generation areussi
                    //On positionne la flag genere a true

                }
            }
            
        }
        public String FormatDate(DateTime Date)
        {
            String Result;

            System.Globalization.Calendar calendrier = new System.Globalization.GregorianCalendar();
            Int32 currentWeek = calendrier.GetWeekOfYear(Date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            Result = String.Format("{0:00}/{1:yy}", currentWeek, Date);
            return Result;
        } // endMethod: FormatDate
        private void GenerateCmdFile( string repertoire ,pack ligne)
        {
            List<string> etiquetteMO = new List<string>();
            List<string> etiquetteMT = new List<string>();
            List<string> etiquetteSIM = new List<string>();
            List<string> etiquettePACK = new List<string>();

            IniFile ConfigFile = new IniFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\VerifStatusCmd.ini");
           
            string PackNomImprimanteEtiquette = ConfigFile.GetValue("PRINTER", "PackNomImprimanteEtiquette");
            string PackLabelName = ConfigFile.GetValue("PRINTER", "PackLabelName");
            string PackRepertoireFichierCommande = ConfigFile.GetValue("PRINTER", "PackRepertoireFichierCommande");

            string MoLabelName = ConfigFile.GetValue("PRINTER", "MoLabelName");
            string MoLabelAtexName = ConfigFile.GetValue("PRINTER", "MoBetaLabelAtexName");

            string MtLabelName = ConfigFile.GetValue("PRINTER", "MtLabelName");
            string MtLabelAtexName = ConfigFile.GetValue("PRINTER", "MtLabelAtexName");
            string TIMOLabelName = ConfigFile.GetValue("PRINTER", "TIMOLabelName");
            string TIMOLabelAtexName = ConfigFile.GetValue("PRINTER", "TIMOLabelAtexName");

            string SimLabelName = ConfigFile.GetValue("PRINTER", "SimLabelName");
            string SimLabelAtexName = ConfigFile.GetValue("PRINTER", "SimLabelAtexName");

            // etiquette pack
            etiquettePACK.Add((string.Concat(StructLabel.LABELNAME, "=\"", PackLabelName, "\"")));
            etiquettePACK.Add(string.Concat(StructLabel.REFCOMPACK, "=\"", ligne.RefCommercialePack, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.CMDCLIENT, "=\"", ligne.CmdClient, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.DATE, "=\"", this.FormatDate(DateTime.Now), "\""));
            etiquettePACK.Add(string.Concat(StructLabel.FPERSO, "=\"", ligne.RefFichePerso, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.NUMSERIE, "=\"", ligne.NumSeriePack, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.FIRMWAREMO, "=\"", ligne.firmwareMo, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.FIRMWAREMT, "=\"",ligne.firmwareMt, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.OPTIONS_LOGICIELLES, "=\"", ligne.OptionsLogicielles, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.CMDSYNCH, "=\"", "", "\""));
            etiquettePACK.Add(string.Concat(StructLabel.PRINTER, "=\"", PackNomImprimanteEtiquette, "\""));
            etiquettePACK.Add(string.Concat(StructLabel.LABELQUANTITY, "=\"","1", "\""));
            string etiquetteFile = "label_PACK_" + ligne.NumSeriePack+".cmd" ;
            if (ligne.RefCommercialePack != ligne.RefCommercialeMt &&
                ligne.RefCommercialePack != ligne.RefCommercialeMo &&
                ligne.RefCommercialePack != ligne.RefCommercialeSim)
            {
                System.IO.File.WriteAllLines(repertoire + etiquetteFile, etiquettePACK.ToArray());
            }
            // etiquette sim
            if (ligne.NbSim > 0)
            {
                etiquetteSIM.Add(string.Concat(StructLabel.LABELNAME, "=\"", SimLabelName, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.REFCOMSIM, "=\"", ligne.RefCommercialeSim, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.NUMSIM, "=\"", "1", "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.CMDCLIENT, "=\"", ligne.CmdClient, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.DATE, "=\"", this.FormatDate(DateTime.Now), "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.FPERSO, "=\"", ligne.RefFichePerso, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.NUMSERIE, "=\"", ligne.nmrsim, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.OPTIONS_LOGICIELLES, "=\"", ligne.OptionsLogicielles, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.PRINTER, "=\"", PackNomImprimanteEtiquette, "\""));
                etiquetteSIM.Add(string.Concat(StructLabel.LABELQUANTITY, "=\"", "1", "\""));
                etiquetteFile = "label_SIM_" + ligne.NumSeriePack + ".cmd";

                System.IO.File.WriteAllLines(repertoire + etiquetteFile, etiquetteSIM.ToArray());
            }
            //etiquette MT
            if (ligne.NbMt > 0)
            {

                if (ligne.RefIndustrielleMt.Substring(0, 2).Equals("T1"))
                {
                    if (ligne.RefIndustrielleMt.Contains("_X"))
                    {
                        etiquetteMT.Add(string.Concat(StructLabel.LABELNAME, "=\"", TIMOLabelAtexName, "\""));
                    }
                    else
                    {
                        etiquetteMT.Add(string.Concat(StructLabel.LABELNAME, "=\"", TIMOLabelName, "\""));
                    }
                }
                else
                {
                    if (ligne.RefIndustrielleMt.Contains("_X"))
                    {
                        etiquetteMT.Add(string.Concat(StructLabel.LABELNAME, "=\"", MtLabelAtexName, "\""));
                    }
                    else
                    {
                        etiquetteMT.Add(string.Concat(StructLabel.LABELNAME, "=\"", MtLabelName, "\""));
                    }
                }
                etiquetteMT.Add(string.Concat(StructLabel.REFCOMMT, "=\"", ligne.RefCommercialeMt, "\""));
                etiquetteMT.Add(string.Concat(StructLabel.NUMMT, "=\"", "1", "\""));
                etiquetteMT.Add(string.Concat(StructLabel.CMDCLIENT, "=\"", ligne.CmdClient, "\""));
                etiquetteMT.Add(string.Concat(StructLabel.DATE, "=\"", this.FormatDate(DateTime.Now), "\""));
                etiquetteMT.Add(string.Concat(StructLabel.FPERSO, "=\"", ligne.RefFichePerso, "\""));
                etiquetteMT.Add(string.Concat(StructLabel.NUMSERIE, "=\"", ligne.nmrMt, "\""));

                string tmp;
                // YVA protection de code
                info_row.TryGetValue(StructLabel.FREQ, out tmp);
                etiquetteMT.Add(string.Concat(StructLabel.FREQ, "=\"", tmp, "\""));
                info_row.TryGetValue(StructLabel.TENSION, out tmp);
                etiquetteMT.Add(string.Concat(StructLabel.TENSION, "=\"", tmp, "\""));

                etiquetteMT.Add(string.Concat(StructLabel.FIRMWAREMT, "=\"", ligne.firmwareMt, "\""));
                etiquetteMT.Add(string.Concat(StructLabel.CODEIDENT, "=\"", ligne.codeidMT.Substring(2, ligne.codeidMT.Length-2), "\""));
                etiquetteMT.Add(string.Concat(StructLabel.CMDSYNCH, "=\"", "", "\""));

                etiquetteMT.Add(string.Concat(StructLabel.PRINTER, "=\"", PackNomImprimanteEtiquette, "\""));
                etiquetteMT.Add(string.Concat(StructLabel.LABELQUANTITY, "=\"", "1", "\""));
                etiquetteFile = "label_MT_" + ligne.NumSeriePack + ".cmd";

                System.IO.File.WriteAllLines(repertoire + etiquetteFile, etiquetteMT.ToArray());
            }

            //etiquette MO
            if (ligne.NbMo > 0)
            {
                if (ligne.RefIndustrielleMo.Contains("_X"))
                {
                    etiquetteMO.Add(string.Concat(StructLabel.LABELNAME, "=\"", MoLabelAtexName, "\""));
                }
                else
                {
                    etiquetteMO.Add(string.Concat(StructLabel.LABELNAME, "=\"", MoLabelName, "\""));
                }
            etiquetteMO.Add(string.Concat(StructLabel.REFCOMMO, "=\"", ligne.RefCommercialeMo, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.NUMMO, "=\"", "1", "\""));
            etiquetteMO.Add(string.Concat(StructLabel.CMDCLIENT, "=\"", ligne.CmdClient, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.DATE, "=\"", this.FormatDate(DateTime.Now), "\""));
            etiquetteMO.Add(string.Concat(StructLabel.FPERSO, "=\"", ligne.RefFichePerso, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.NUMSERIE, "=\"", ligne.nmrMo, "\""));

            string tmp;
            info_row.TryGetValue(StructLabel.FREQ, out tmp);
            etiquetteMO.Add(string.Concat(StructLabel.FREQ, "=\"", tmp, "\""));

            info_row.TryGetValue(StructLabel.CODEPIN0, out tmp);
            etiquetteMO.Add(string.Concat(StructLabel.CODEPIN0, "=\"",tmp, "\""));
            info_row.TryGetValue(StructLabel.CODEPIN1, out tmp);
            etiquetteMO.Add(string.Concat(StructLabel.CODEPIN1, "=\"", tmp, "\""));
            info_row.TryGetValue(StructLabel.CODEPIN2, out tmp);
            etiquetteMO.Add(string.Concat(StructLabel.CODEPIN2, "=\"", tmp, "\""));
            info_row.TryGetValue(StructLabel.CODEPIN3, out tmp);
            etiquetteMO.Add(string.Concat(StructLabel.CODEPIN3, "=\"",tmp, "\""));

            etiquetteMO.Add(string.Concat(StructLabel.FIRMWAREMO, "=\"", ligne.firmwareMo, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.CODEIDENT, "=\"", ligne.codeidMO, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.CMDSYNCH, "=\"", "", "\""));

            etiquetteMO.Add(string.Concat(StructLabel.PRINTER, "=\"", PackNomImprimanteEtiquette, "\""));
            etiquetteMO.Add(string.Concat(StructLabel.LABELQUANTITY, "=\"", "1", "\""));

            etiquetteFile = "label_MO_" + ligne.NumSeriePack + ".cmd";
                System.IO.File.WriteAllLines(repertoire + etiquetteFile, etiquetteMO.ToArray());
            }
        }
        private bool CopyFirmwareFile(string destination,pack ligne)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(ligne.firmwareMoFullName) && ligne.NbMo > 0)
                {
                    FileInfo firmware = new FileInfo(ligne.firmwareMoFullName);
                    if (firmware.Exists)
                    {
                        File.Copy(ligne.firmwareMoFullName, destination + firmware.Name, true);
                    }
                }
                if (!string.IsNullOrWhiteSpace(ligne.firmwareMtFullName) && ligne.NbMt > 0)
                {
                    FileInfo firmware = new FileInfo(ligne.firmwareMtFullName);
                    if (firmware.Exists)
                    {
                        File.Copy(ligne.firmwareMtFullName, destination + firmware.Name, true);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private pack CompleterRefLigne( pack ligne)
        {
            if (ligne.NbSim == 0)
            {
                ligne.OptionsLogicielles =  "00X0X0";
                ligne.RefCommercialeSim = ligne.Marche + "S000";
                ligne.RefIndustrielleSim =  "S000";
            }
            if (ligne.NbMo ==0)
            {
                ligne.ConfMatMO = "B21A0000X";
                ligne.RefIndustrielleMo =  "B2-000";
                ligne.RefCommercialeMo = ligne.Marche + "B20000";
            }
            if (ligne.NbMt ==0)
            {
                ligne.ConfMatMT =  "T11EX000X";
                ligne.RefIndustrielleMt =  "T1-000";
                ligne.RefCommercialeMt = ligne.Marche + "T10000";
            }
            return ligne;
        }
    }
}
