using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using Util;

namespace GenerateurDFUSafir.Models
{
    public sealed class TestOutilProduction
    {
        private string ZPLStringTest =
        "^XA" +
        "^POI" +
        "^FO400,800" +
        "^GFA,12000,12000,30,gV01RF8,gV03RFC,gV07RFC,::::::::::::::::::::::::::S0FFEg07RFC,Q01KFY07RFC,P01LFEX07RFC,P07MFCW07RFC,O03OFW07RFC,O0PFCV07RFC,N01QFV07RFC,N07QF8U07RFC,N0RFCU07RFC,M03MFC01IFU07RFC,M07LFK07F8T07RFC,M0LF8L0FCT07RFC,L01KFCM01ET07RFC,L03KF003IFC007T07RFC,L07JFC01KFC018S07RFC,L0KF807LF00CS07RFC,K01JFE01MFC02S07RFC,K01JFC07NFU07RFC,K03JF01OFCT07RFC,K07IFE03OFET07RFC,K07IFC07PFT07RFC,K0JF80QF8S07RFC,J01JF01QFCS07RFC,J01IFE03IFE003JFES07RF8,J03IFC07FFCJ03JFS07KF8,J03IF80FFEL07IF8R07KF8,J07IF81FFJ07001IF8R07KF8,J07IF01FCJ01F807FFCR07KF8,J07FFE03F8K07F03FFER07KF8,J0IFE07EL01FC0FFER07KF8,J0IFC07CM0FF07FFR07KF8,J0IFC0F06L03F83FFR07KF8,I01IF80E1F8K01FE1FFR07KF8,I01IF81C3FFL0FF0FF8Q07KF8,I01IF0187FFCK07F87F8Q07KF8,I01IF0107IFK03FC3F8Q07KF8,I01FFE0307IFEJ01FE3FCQ07KF8,I03FFE020KF8I01FF1FCQ07KF8,I03FFEI0LFJ0FF0FCQ07KF8,I03FFCI0LFCI07F8FCQ07KF8,I03FFCI0MFI07FC7CQ07KF8,I03FFCI0MF8003FC7EQ07KF8,I03FFCI0FF1JFC003FE3EQ07gFC,I03FF8I0FF07IFE001FE3EQ07gFE,I03FF8I0FF01JF001FF3EQ07gGF,I03FF8I0FF003IFI0FF1EQ07gGF,I03FF8I0FFI0IF800FF1EQ07gGF,I03FF8I0FFI01FF800FF1EQ07gGF,I03FF8I0FFJ07FC007F8EQ07gGF,I01FF8I0FFJ03FC007F8EQ07gGF,I01FF8I0FFJ01FC007F8EQ07gGF,I01FF8I0FFJ01FC003F8CQ07gGF,:I01FF8I0FFJ01FE003F84Q07gGF,J0FF8I0FFJ01FE003F84Q07gGF,J0FF8I07F8I01FE003F84Q07gGF,J0FF8I07FFI01FE003F84Q07gGF,J07F8I07FF8001FE003F8R07gGF,J07FCI07FF8001FE003F8R07gGF,J07FCI03FF8001FE003F8R07gGF,J03FCI03FF8001FE003F8R07gGF,J03FCI01FF8021FE003F8R07gGF,J01FEJ0FF803FFE003FS07gGF,J01FEJ03F803FFE003FS07gGF,K0FEK0F803FFE003FS07gGF,K0FFK01803FFE003ES07gGF,K07FN03FFC003ES07gGF,K03FN03FFC007ES07gGF,K03F8I0FI03FFC007CS07gGF,K01F8001FC003FFC0078S07gGF,L0FC003FF801FF80078S07gGF,L07C003FFE007FI0FT07gGF,L03E007IFC01EI0FT07gGF,L01F007JFK01ET07gGF,M0F007JFCJ01CT07gGF,M07807KF8I038T07gGF,M03C07KFEI03U07gGF,M01E07LFI06U07gGF,N0707F3JFC004U07gGF,N0307F07IFCX07gGF,O0C7F01IFEX07gGF,P07F007IFX07gGF,P07FI0IFX07gGF,P07FI03FF8W07gGF,P07FJ07F8W07gGF,P07FJ03F8W07gGF,P07FJ01F8W07gGF,P07FJ01FCW07gGF,::P07F8I01FCW07gGF,P07FCI01FCW07gGF,P07FFI01FCW03gGF,P03FFE001FCW03gFE,P03IF801FCX0gFC,P01JF01FC,P01JFC1FC,Q0KF3FC,Q03LFC,R0LFC,R03KFC,P06007JFC,P07801JFC,P07E007IF8,P07FC00IF8,P07FF003FF,P07FFC00FF,P07IF801C,N0707IFEgH0QFC03LFC,M03F07JF8g03QFC07LFE,L01FF03KFg07QFC07MF,L0IF007JFCY07QFC07MF,K07IF001KFY07QFC07MF,J03JFI07JFEX07QFC07MF,I03JFCJ0KF8W07QFC07MF,I03IFEK03JFCW07QFC07MF,I03FFEM0JFCW07QFC07MF,I03FEN01IFCW07QFC07MF,I03FCO07FFCW07QFC07MF,I03FFCO0FFCW07QFC07MF,I03IFCN07FCW07QFC07MF,J0JFE07MFCW07QFC07MF,K0JF07MFCW07QFC07MF,L0IF07MFCW07QFC07MF,M0FF07MFCW07QFC07MF,L01FF07MFCW07QFC07MF,K01IF07MFCW07QFC07MF,J01JF07MFCW07QFC07MF,I01JFC07MFCW07QFC07MF,I03IFC007FCgI07QFC07MF,I03FFCI07FFgI07QFC07MF,I03FCJ07FFEgH07QFC07MF,I03FFJ07IF8gG07QFC07MF,I03IFI07JFgG07QFC07MF,I03IFE007JFCg07QFC07MF,I01JFE01KFg07QFC07MF,J03JF003JFEY07QFC07MF,K03IFI0KF8X07QFC07MF,L07FFI01JFEX07QFC07MF,M0FF06007JFCW07QFC07MF,M01F07801JFCW07QFC07MF,N0307E003IFCW07QFC07MF,J07EJ07FC00IFCW07QFC07MF,J0FF87807FF003FFCW07QFC07MF,I01FF87C07FFC007FCW07QFC07MF,I03FFC7E07IF801FCW07QFC07MF,I03FFC7F07IFE007CW07QFC07MF,I07FFC7F07JF800CW07QFC07MF,I07E3E3F07KFg07QFC07MF,I07C1E0F87KFCY07QFC07MF,I0781E0F87LF8X07QFC07MF,I0380E0787F3JFEX07QFC07MF,I0380E0787F0KF8W07QFC07MF,I03C0E0787F01JFCW07QFC07MF,I01C0F0787F007IFCW07QFC07MF,I01E070F87F001IFCW07QFC07MF,J0KF87FI03FFCK03KFEK07QFC07MF,I03KF07FJ0FFCK03KFEK07QFC07MF,I03KF07FJ03FCK01F03EFEK07QFC07MF,I03JFE07FJ01FCK03800EM07QFC07MF,I03JFE07FJ01FCK03I07M07QFC03MF,I03JF807FJ01FCK07I03M07IFCI03IFCK0JF,I02L07FJ01FCK07I03M07IF8I01IFCK0JF,P07F8I01FCK07I07M07IF8I01IFCK0JF,P07FCI01FCK038007M07IF8I01IFCK0JF,I03KF07FFI01FCK03E01EM07IF8I01IFCK0JF,I03KF03FFC001FCK01IFEM07IF8I01IFCK0JF,I03KF03IF801FCL07FF8M07IF8I01IFCK0JF,I03KF03IFE01FCL01FEN07IF8I01IFCK0JF,I03KF01JF81FCW07IF8I01IFCK0JF,I03KF00KF1FCW07IF8I01IFCK0JF,M07C007LFCO07M07IF8I01IFCK0JF,M01E001LFCI038I07FM07IF8I01IFCK0JF,N0FI03KFCI038003FEM07IF8I01IFCK0JF,N0FJ0KFCI03C01FFN07IF8I01IFCK0JF,N0F8I03JFCI01F0FFO07IF8I01IFCK0JF,N0F802007IF8J0IF8O07IF8I01IFCK0JF,N0F81F801IF8J03FCP07IF8I01IFCK0JF,M01F83FF007FF8K07FCO07IFCI03IFEJ01JF,I03KF83FFC00FFM0FF8N07gGF,I03KF07IF003EN0FFN07gGF,I03KF07IFEQ01FFM07gGF,I03JFE07JF8Q03FM07gGF,I03JFC07KFR03M07gGF,I03JF807KFCY07gGF,M07E07LFY07gGF,M01E07LFEX07gGF,N0F07F0KF8W07gGF,N0F07F03JFCW07gGF,N0F87F00JFCW07gGF,N0F87F001IFCW07gGF,N0F87FI07FFCW07gGF,M01F87FJ0FFCW07gGF,I03KF87FJ03FCW07gGF,I03KF07FK0FCW07gGF,I03KF07FK01CW07gGF,I03JFE07FL04W07gGF,I03JFC07FgJ07gGF,I03JF007FgJ07gGF,P07F8gI07gGF,P07FEgI07gGF,P03FFCgH07gGF,OF03IFgH07gGF,OF03IFEgG07gGF,OF01JF8g07gGF,OF00JFEg07gGF,OF007JFCY07gGF,J07F3FC001KFY07gGF,J0F803EI07JFCX07gGF,I01E001FJ0KF8W07gGF,I03EI0FJ03JFCW07gGF,I03CI0F8J0JFCW07gGF,I03CI0F80F801IFCW07gGF,I07CI0F81FE007FFCW07gGF,I07CI0F83FFC01FFCW07gGF,I03E001F87IF003FCW07gGF,I03F003F87IFC00FCW07gGF,I03FC0FF07JF803CW07gGF,I03KF07JFE004W07gGF,I01JFE07KFCY07gGF,J0JFC07LFY07gGF,J07IF807LF8X07gGF,J01IF007F1JFCX07gGF,K03F0707F03IFEX07gGF,N0F07F00IFEX07gGF,N0F07F003IFX07gGF,N0F01FI07FFX07gGF,I03LF8FI01FF8W07gGF,I03LFE7J07F8W07gGF,I03LFE3J03F8W07gGF,I03MF3J01FCW03gFE,I03MF3J01FCW01gFC,I03MF9J01FC,N0F0F9J01FC,:::V01FC,:I03OFC001FC,I03OF8021FC,I03OF803BFC,I03OF803FFC,I03OFC03FFC,I03OF803FFC,U03FFCW01KF801SF,U03FFCW03KF801SF,K0FFCM03FF8W07KF801SF,J03IF004J03FF8W07KF801SF,J07IF807J01FFX07KF801SF,J0JFC07EJ07EX07KF801SF,I01JFE07F8J08X07KF801SF,I01KF07FEK0CW07KF801SF,I03F0E3F07FFC001FCW07KF801SF,I03E0F1F07IF001FCW07KF801SF,I03C0F0F87IFC01FCW07KF801SF,I07C0F0F87JF81FCW07KF801SF,I07C0F0F83JFE1FCW07KF801SF,I07C0F0F80KF9FCW07KF801SF,I07C0F0F803LFCW07KF801SF,I03C0F1F8007KFCW07KF801SF,I03E0E3FI01KFCW07KF801SF,I03F0IFJ07JFCW07KF801SF,I01F8FFEK0JFCW07KF801SF,J0F8FFCK03IFCW07KF801SF,J078FF8L0IFCW07KF801SF,J038FFM01FFCW07KF801SF,K08FCN07FCW07KF801SF,V01FCW07KF801SF,:I03KF04K01FCW07KF801SF,I03KF07K01FCW07KF801SF,I03KF07CJ01FCW07KF801SF,I03KF07FJ01FCW07KF801SF,I03KF07FEI01FCW07KF801SF,I03KF07FF8001FCW07KF801SF,L01F807IF001FCW07KF801SF,M07C07IFC01FCW07KF801SF,M03E07JF003CW07KF801SF,M01F07JFE00CW07KF801SF,M01F01KF8Y07KF801SF,M01F803JFEY07KF801SF,M01F800KFCX07KF801SF,M01F8003KFX07KF801SF,S07JFCW07KF801SF,S01JFCW07KF801SF,T07IFCW07KF801SF,U0IFCW07KF801SF,P04J03FFCW07KF801SF,P06K0FFCW07KF801SF,P07K01FCW07KF801SF,P078K07CW07KF801SF,P07CL0CW07KF801SF,P07EgJ07KF801SF,P07FgJ07KF801SF,P07F8gI07KF801SF,P07FCK0CW07KF801SF,P07FEJ01CW07KF801RFE,P07FFJ07CW07KF801IFE,P07FFC001FCW07KF801IFE,P07FFE003FCW07KF801IFE,P03IF00FFCW07KF801IFE,P01IF83FFCW07KF801IFE,Q0IFCIFCW07KF801IFE,Q07LFCW07KF801IFE,Q03LFCW07KF801IFE,Q01LF8W07KF801IFE,R0KFEX07KF801IFE,R03JFCX07KF801IFE,R03JFY07KF801IFE,R0JFCY07KF801IFE,Q01JFCY07KF801IFE,Q07KFY07KF801IFE,P01LF8X07KF801IFE,P03LFCX07KF801IFE,P07LFEX07gF8,P07IF3IFX07gFE,P07FFC1IF8W07gGF,P07FF80IFCW07gGF,P07FE007FFCW07gGF,P07F8001FFCW07gGF,P07EJ0FFCW07gGF,P07CJ07FCW07gGF,P07K03FCW07gGF,P04K01FCW07gGF,W0FCW07gGF,W07CW07gGF,W03CW07gGF,W01CW07gGF,X0CW07gGF,X04W07gGF,gV07gGF,::::::::::::::::::::::::::::::::::gV03gGF,gV01gFE,gW0gF8,,:" +
        "^FS" +
        "^FO680,100" +
        "^A0R120,120" +
        "^CI28" +
        "^FD" +
        "^FS^A0R30,30" +
        "^FO650,240" +
        "^FDTest" +
        "^FS^A0R40,40" +
        "^FO600,180" +
        "^FDJAY ELECTRONIQUE" +
        "^FS^A0R40,40" +
        "^FO560,180" +
        "^FDZAC La Batie" +
        "^FS^A0R40,40 " +
        "^FO520,180" +
        "^FDrue Champrond" +
        "^FS^A0R40,40" +
        "^FO480,180" +
        "^FD38330 Saint Ismier FRANCE" +
        "^FS" +
        "^FO470,95" +
        "^FX graphics box." +
        "^GB230,680,5,B,4" +
        "^FS" +
        "^FO480,120" +
        "^GD200,620,5,B,R" +
        "^FS" +
        "^A0R40,40" +
        "^FO390,240" +
        "^FD " +
        "^FS^A0R40,40" +
        "^FO340,180" +
        "^FD" +
        "^FS^A0R30,30" +
        "^FO300,180" +
        "^FD" +
        "^FS^A0R30,30 " +
        "^FO260,180" +
        "^FD" +
        "^FS^A0R30,30" +
        "^FO220,180" +
        "^FD" +
        "^FS^A0R30,30" +
        "^FO170,180" +
        "^FD" +
        "^FS" +
        "^FO160,95" +
        "^FX graphics box." +
        "^GB290,680,5,B,4" +
        "^FS" +
        "^PON" +
        "^XZ";

        private List<string> _result;
        public List<string> Result { get; set; }
        List<Tests> ListTests { get; set; }
        private TestOutilProduction() { }
        private bool _DiagRun = false;
        private bool DiagRun { get { return _DiagRun; } set { DiagRun = value; } }
        
        private static TestOutilProduction _instance;

        public static TestOutilProduction GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TestOutilProduction();
                
            }
            return _instance;
        }
        private readonly object balanceLock = new object();
        public void  TestOutilProductionExecute()
        {
            bool execute = false;
            lock (balanceLock)

            {
                if (!_DiagRun) { _DiagRun = true; execute = true; }
            }
            try
            {
                
                if (execute)
                {
                    Result = new List<string>();
                    GestionCodeOF();
                    Result.Add("Start test at " + DateTime.Now.ToString());
                    foreach (var groupe in ListTests)
                    {
                        Result.Add("Groupe de test : " + groupe.valeur);
                        Result.Add("--> " + groupe.Libelle);
                        foreach (var sousgroupe in groupe.listSousTests)
                        {
                            Result.Add("     Sousgroupe de test : " + sousgroupe.test);
                            Result.Add("     --> " + sousgroupe.Commentaire);
                            foreach (var test in sousgroupe.Listtest)
                            {
                                Result.Add("           test : " + test.Commentaire);
                                Thread th = new Thread(
                                        new ThreadStart(() => TestMethode(test.Methode, test.Value1, test.Value2))
                                        );
                                th.Start();
                                bool fini = th.Join(100000);
                                if (!fini)
                                {
                                    Result.Add(" Dépassement délai de réponse");
                                    Result.Add("Test FAILED");
                                    th.Abort();
                                    break;
                                }
                                Result.Add("");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    Result.Add("<-------------------> ");
                    Result.Add("Test Ended ");
                }
            }
            catch
            { 
                
            }
            lock (balanceLock)
            {
                 _DiagRun = false; 
            }
        }
        private void TestMethode(string methode, string value1, string value2)
        {
            try
            {
                Result.Add("     -> Start " + methode + " test  " + DateTime.Now.ToString());
                switch (methode)
                {
                    case "Ping":
                        TestPing(value1);
                        break;
                    case "TestReadDataBase":
                        TestReadDataBase(value1, value2);
                        break;
                    case "Impression":
                        PrintEtiquetteTest(value1, value2);
                        break;
                }
                Result.Add("      -> End " + methode + " test  " + DateTime.Now.ToString());
            }
            catch (Exception ex )
            {
                Result.Add("      -> Exception  " + ex.Message.ToString() + " test  " + DateTime.Now.ToString()) ; 
            }
        }
        private void TestPing(string ip)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                Result.Add(string.Concat("Address: ", reply.Address.ToString()));
                Result.Add(string.Concat("RoundTrip time: ", reply.RoundtripTime));
                if (reply.Options!= null) Result.Add(string.Concat("Time to live: ", reply.Options.Ttl));
                if (reply.Options != null) Result.Add(string.Concat("Don't fragment: ", reply.Options.DontFragment));
                if (reply.Buffer != null)  Result.Add(string.Concat("Buffer size: ", reply.Buffer.Length));
            }
        }
        private void TestReadDataBase(string connectionstring, string table)
        {
            try
            {
                SqlConnection db1 = null;
                SqlCommand RequeteSql = new SqlCommand();
                db1 = new SqlConnection(connectionstring);
                RequeteSql.Connection = db1;
                RequeteSql.CommandText = "SELECT TOP (1000) * FROM " + table;
                RequeteSql.CommandTimeout = 60;
                db1.Open();
                SqlDataReader monReader = RequeteSql.ExecuteReader();
                DataTable table1 = new DataTable();
                table1.Load(monReader);
                try
                {
                    DataRow row = table1.Rows[0];
                    Result.Add(string.Concat("Connectionstring", connectionstring));
                    Result.Add(string.Concat("Connexion OK"));
                }
                catch (Exception ex1)
                {
                    Result.Add(string.Concat("Connectionstring", connectionstring));
                    Result.Add(string.Concat("Connexion  Failed"));
                    Result.Add(string.Concat(ex1.Message));
                }
            }
            catch (Exception ex)
            {
                Result.Add(string.Concat("Connectionstring", connectionstring));
                Result.Add(string.Concat("Connexion  Failed"));
                Result.Add(string.Concat(ex.Message));
            }
        }
        private void PrintEtiquetteTest(string value1, string value2)
        {
            Result.Add(" Lancement de l'impression");           
                switch (value1)
                {
                    case "scrutation":
                        using (new Impersonation("Product", "jayelectronique.org", "2017Pr01"))
                        {
                            EtiquetteScrutation();
                        }
                        break;
                    case "scrutation_bis":
                        using (new Impersonation("Product", "jayelectronique.org", "2017Pr01"))
                        {
                            EtiquetteScrutation_bis();
                        }
                        break;
                    case "scrutation2_bis":
                        using (new Impersonation("Product", "jayelectronique.org", "2017Pr01"))
                        {
                            EtiquetteScrutation2();
                        }
                        break;
                    case "scrutation2":
                        using (new Impersonation("Product", "jayelectronique.org", "2017Pr01"))
                        {
                            EtiquetteScrutation2_bis();
                        }
                        break;
                    case "logistique":
                        EtiquetteZPL(value2);
                        break;
                    case "logistique2":
                        EtiquetteZPL(value2);
                        break;
                }
            
        }
        public void GestionCodeOF()
        {
            ListTests = new List<Tests>();
            string path = HttpContext.Current.Request.MapPath(".");
            path = "";
            //string file = path + "C:\\Users\\fournier\\source\\repos\\GenerateurDFUSafir\\data\\Test.xml";
            string file = path + "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\data\\Test.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            foreach (XmlNode xmlnode in xmlDoc.DocumentElement)
            {
                Tests tests = new Tests();
                tests.valeur = xmlnode.Attributes["valeur"].Value;
                tests.Libelle = xmlnode.Attributes["libelle"].Value;
                tests.listSousTests = new List<SousTests>();
                ListTests.Add(tests);
                foreach (XmlNode xmlchil in xmlnode.ChildNodes)
                {
                    SousTests soustests = new SousTests();
                    tests.listSousTests.Add(soustests);
                    soustests.test = xmlchil.Attributes["valeur"].Value;
                    soustests.Commentaire = xmlchil.Attributes["libelle"].Value;
                    soustests.Listtest = new List<Test>();
                    foreach (XmlNode xmlchil2 in xmlchil.ChildNodes)
                    {
                        Test codegroupe = new Test();
                        codegroupe.nmrTest = Convert.ToInt32(xmlchil2.Attributes["valeur"].Value);
                        codegroupe.Methode = xmlchil2.Attributes["methode"].Value;
                        codegroupe.Value1 = xmlchil2.Attributes["value1"].Value;
                        codegroupe.Value2 = xmlchil2.Attributes["value2"].Value;
                        codegroupe.Commentaire = xmlchil2.Attributes["commentaire"].Value;
                        soustests.Listtest.Add(codegroupe);
                    }
                }
            }
        }
        private void EtiquetteScrutation()
        {
            string path = Resource1.REP_SCRUTATION;
            //path = @"REP_SCRUTATION";
            FileStream fileStream = new FileStream(string.Concat(path, "\\label_MO_", "TEST",DateTime.Now.Millisecond.ToString(), ".cmd"), FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_FABUDE, "\""));
            writer.WriteLine(string.Concat(StructLabel.DATE, "=\"", 01.ToString("00"), "/", DateTime.Now.ToString("yy"), "\""));
            writer.WriteLine(string.Concat(StructLabel.BANDE_FREQ, "=\"", "1", "\""));
            writer.WriteLine(string.Concat(StructLabel.FCC_ID, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.FOURNISSEUR, "=\"", Resource1.FOURNISSEUR, "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_ENSEMBLE, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.COMP_REF_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.DOSSIER_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.ARRET_PASSIF, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.ALIMENTATION, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.PONT_COUPLE_TXT, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.CANAL, "=\"", 1.ToString(), "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_EMETTEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.COMP_REF_EMETTEUR, "=\"", "111", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_EMETTEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.DOSSIER_EMETTEUR, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR_2, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.OPTION_TEXTE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_CLE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_CLE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.HOMME_MORT, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.FICHE_PERSO, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.AR_OF, "=\"", "C202312345", "\""));

            writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
            writer.Close();
            fileStream.Close();
        }
        
        private void EtiquetteScrutation_bis()
        {
            string path = Resource1.REP_SCRUTATION;
            FileStream fileStream = new FileStream(string.Concat(path, "\\label_MO_", "TEST", DateTime.Now.Millisecond.ToString(), ".cmd"), FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", @"\\Jaysvrfiles\donnees_production\DONNEES_INDUSTRIELLES\ETIQUETTES\Etiquettes_Models\UD\planche_CLE_UD.lab", "\""));
             writer.WriteLine(string.Concat(StructLabel.DATE, "=\"", 01.ToString("00"), "/", DateTime.Now.ToString("yy"), "\""));
            writer.WriteLine(string.Concat(StructLabel.BANDE_FREQ, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.FCC_ID, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.FOURNISSEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_ENSEMBLE, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.COMP_REF_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.DOSSIER_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_RECEPTEUR, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.ARRET_PASSIF, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.ALIMENTATION, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.PONT_COUPLE_TXT, "=\"", "\""));
            writer.WriteLine(string.Concat(StructLabel.CANAL, "=\"", 1.ToString(), "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_EMETTEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.COMP_REF_EMETTEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_EMETTEUR, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.DOSSIER_EMETTEUR, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR_2, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.OPTION_TEXTE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUM_CLE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.REF_CLE, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.HOMME_MORT, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.FICHE_PERSO, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.AR_OF, "=\"", "TEST", "\""));
        
            writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
            writer.Close();
            fileStream.Close();
        }
        
        private void EtiquetteScrutation2()
        {
            string path = Resource1.REP_SCRUTATION2;
            FileStream fileStream = new FileStream(string.Concat(path, "\\label_MO_", "TEST", DateTime.Now.Millisecond.ToString(), ".cmd"), FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", @"\\Jaysvrfiles\donnees_production\DONNEES_INDUSTRIELLES\ETIQUETTES\Etiquettes_Models\PEGASE\MO.Lab", "\""));
            writer.WriteLine(string.Concat(StructLabel.REFCOMMO, "=\"", "TEST", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUMMO, "=\"", "01", "\""));
            writer.WriteLine(string.Concat(StructLabel.CMDCLIENT, "=\"", "C202312345", "\""));
            writer.WriteLine(string.Concat(StructLabel.DATE, "=\"", "01/23", "\""));
            writer.WriteLine(string.Concat(StructLabel.FPERSO, "=\"", "Fiche", "\""));
            writer.WriteLine(string.Concat(StructLabel.NUMSERIE, "=\"", "Test01", "\""));

            writer.WriteLine(string.Concat(StructLabel.CODEPIN0, "=\"", "0000", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEPIN1, "=\"", "0000", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEPIN2, "=\"", "0000", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEPIN3, "=\"", "0000", "\""));

            writer.WriteLine(string.Concat(StructLabel.FIRMWAREMO, "=\"", "FIRM", "\""));
            writer.WriteLine(string.Concat(StructLabel.CODEIDENT, "=\"", "FFFFF", "\""));
            writer.WriteLine(string.Concat(StructLabel.CMDSYNCH, "=\"", "", "\""));

            writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", Resource1.MoNomImprimanteEtiquette, "\""));
            writer.WriteLine(string.Concat(StructLabel.LABELQUANTITY, "=\"", "1", "\""));

            writer.Close();
            fileStream.Close();
        }
        private void EtiquetteScrutation2_bis()
        {
            string path = Resource1.REP_SCRUTATION2;
            FileStream fileStream = new FileStream(string.Concat(path, "\\label_MO_", "TEST", DateTime.Now.Millisecond.ToString(), ".cmd"), FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", @"\\JAYSVRFiles\donnees_production\DONNEES_INDUSTRIELLES\ETIQUETTES\Etiquettes_Models\PEGASE\ValidMO.Lab", "\""));
            writer.WriteLine(string.Concat("REF_PRODUIT", "=\"TEST", "\""));
            writer.WriteLine(string.Concat("REF_APPLICATIVE_PRODUIT=\"TEST", "\""));


            writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", "", "\""));
            writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
            writer.Close();
            fileStream.Close();
        }
        private void EtiquetteZPL(string ip)
        {
            Utile.impression_zebra(ip, ZPLStringTest);
        }
        public class Tests
        {
            public string valeur { get; set; }
            public string Libelle { get; set; }
            public List<SousTests> listSousTests { get; set; }
        }
        public class SousTests
        {
            public string test { get; set; }
            public string Commentaire { get; set; }
            public List<Test> Listtest { get; set; }
        }
        public class Test
        {
            public int nmrTest { get; set; }
            public string Methode { get; set; }
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Commentaire { get; set; }
        }
    }
}