
using InitFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TraitementOFs
{
    public class pack
    {
        public string Marche = "";
       
        public int NbSim = 0;

        public int NbMo = 0;
        public string ConfMatMO = "";
        public string TypeMO = "";

        public int NbMt = 0;
        public string ConfMatMT = "";
        public string TypeMT = "";




        public String file = "";

        public string RefFichePerso = "";
        public String NumCommandeClient = "";
        public string NumLigneClient = "";
        public String NumSeriePack = "";
        public String NumSeriePackProduit = "";
        public String RefFirmwareMt = "";
        public String RefCommercialePack = "";
        public String RefCommercialeMo = "";
        public String RefCommercialeMt = "";
        public String RefCommercialeSim = "";
        public String OptionsLogicielles = "";
        public String RefIndustrielleMo = "";
        public String RefIndustrielleMt = "";
        public String RefIndustrielleSim = "";
        public string NmrOF = "";
        public string codeidMO = "";
        public string codeidMT = "";
        public string nmrsim;
        public string CmdClient = "";

        public string firmwareMoFullName = "";
        public string firmwareMtFullName = "";

        public string firmwareMo = "";
        public string firmwareMt = "";

        public string nmrMo = "";
        public string nmrMt = "";
        public int QtrPack;
        public string ens_synchr = "";

        private string repertoire_firmawre;
        public pack()
        {

        }
        public pack(DataRow row)
        {
            if (!String.IsNullOrWhiteSpace(row["MFGNUM_0"].ToString()) )
            {
                IniFile ConfigFile = new IniFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\VerifStatusCmd.ini");
                repertoire_firmawre = ConfigFile.GetValue("DIRECTORY", "firmware");

               
                Marche = row["CFGFLDALP1_0"].ToString();
                NmrOF = "F" + row["MFGNUM_0"].ToString().Substring(1, 7);
                CmdClient = row["SOHNUM_0"].ToString();
                QtrPack = Convert.ToInt32(row["QTY_0"]);
                ens_synchr = row["VAL_SYNCHRO_0"].ToString();

                if (!String.IsNullOrEmpty(row["CFGFLDALP4_0"].ToString().Trim()))
                {
                    OptionsLogicielles = row["CFGFLDALP4_0"].ToString();
                    RefCommercialeSim = row["SIM-C"].ToString();
                    RefIndustrielleSim = row["SIM-I"].ToString();
                    NbSim = 1;
                }
                else
                {
                    NbSim = 0;
                    //OptionsLogicielles =  "00X0X0";
                    //RefCommercialeSim =  Marche + "S000";
                    //RefIndustrielleSim =  "S000";
                }
                if (!String.IsNullOrEmpty(row["CFGFLDALP2_0"].ToString().Trim()))
                {
                    NbMo = 1;
                    ConfMatMO = row["CFGFLDALP2_0"].ToString();
                    RefIndustrielleMo = row["MO-I"].ToString();
                    RefCommercialeMo = row["MO-C"].ToString();
                    firmwareMo = getFirmawre(RefIndustrielleMo, row["ZVERSION_0"].ToString());
                    firmwareMoFullName = getFirmawreFullName(RefIndustrielleMo, row["ZVERSION_0"].ToString());

                }
                else
                {
                    NbMo = 0;
                    //ConfMatMO = "B21A0000X";
                    //RefIndustrielleMo =  "B2-000";
                    //RefCommercialeMo =  Marche + "B20000";
                }
                if (!String.IsNullOrEmpty(row["CFGFLDALP3_0"].ToString().Trim()))
                {
                    NbMt = 1;
                    ConfMatMT = row["CFGFLDALP3_0"].ToString();
                    RefIndustrielleMt = row["MT-I"].ToString();
                    RefCommercialeMt = row["MT-C"].ToString();
                    firmwareMt = getFirmawre(RefIndustrielleMt, row["ZVERSION_0"].ToString());
                    firmwareMtFullName = getFirmawreFullName(RefIndustrielleMt, row["ZVERSION_0"].ToString());
                }
                else
                {
                    NbMt = 0;
                    //ConfMatMT =  "T11EX000X";
                    //RefIndustrielleMt =  "T1-000";
                    //RefCommercialeMt =  Marche + "T10000";
                }
                RefFichePerso = row["ZFPS_0"].ToString();
                //file = row[""].ToString();
                NumCommandeClient = row["SOHNUM_0"].ToString();
                NumLigneClient = row["SOPLIN_0"].ToString();
                NumSeriePack = row["MFGNUM_0"].ToString().Substring(1, 7);
                RefFirmwareMt = row["ZVERSION_0"].ToString();
                RefCommercialePack = row["ITMREF_0"].ToString();

                if (NbSim == 0 && NbMo == 1 && NbMt == 0)
                {
                    RefCommercialeMo = RefCommercialePack;
                }
                if (NbSim == 0 && NbMo == 0 && NbMt == 1)
                {
                    RefCommercialeMt = RefCommercialePack;
                }
                if (NbSim == 1 && NbMo == 0 && NbMt == 0)
                {
                    RefCommercialeSim = RefCommercialePack;
                }
            }
        }
        
        private string getFirmawre(string reference,string version)
        {
            DirectoryInfo dfufile = new DirectoryInfo(repertoire_firmawre);
            string pattern = "";
            if(!string.IsNullOrWhiteSpace(version) && !version.Trim().Equals("0"))
            {
                pattern = "DF" + reference.Substring(0, 1) + "*" + version + "*.dfu";
            }
            else
            {
                pattern = "DF" + reference.Substring(0, 1)+"*.dfu";
            }
            String Result = "";
            String[] Files;

            try
            {
                Files = Directory.GetFiles(dfufile.FullName, pattern);

                var Query = from file in Files
                            orderby file descending
                            select file;

                Result = Query.FirstOrDefault();

                if (Result != null)
                {
                    if (!String.IsNullOrEmpty(Result))
                    {
                        Result = Path.GetFileNameWithoutExtension(Result);
                    }
                }
            }
            catch (Exception e)
            {
                
            }

            return Result;
        } // endMethod: GetFirmwareName
        private string getFirmawreFullName(string reference, string version)
        {
            DirectoryInfo dfufile = new DirectoryInfo(repertoire_firmawre);
            string pattern = "";
            if (!string.IsNullOrWhiteSpace(version) && !version.Trim().Equals("0"))
            {
                pattern = "DF" + reference.Substring(0, 1) + "*" + version + "*.dfu";
            }
            else
            {
                pattern = "DF" + reference.Substring(0, 1) + "*.dfu";
            }
            String Result = "";
            String[] Files;

            try
            {
                Files = Directory.GetFiles(dfufile.FullName, pattern);

                var Query = from file in Files
                            orderby file descending
                            select file;

                Result = Query.FirstOrDefault();
            }
            catch (Exception e)
            {

            }

            return Result;
        } // endMethod: GetFirmwareName
    }

    
}
