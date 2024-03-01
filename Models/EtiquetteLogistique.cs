using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class EtiquetteLogistique
    {
        public string Itmeref { get; set; }
        public string Qtr { get; set; }
        public string Lot { get; set; }
        public string Reference1 { get; set; }
        public string Commande1 { get; set; }
        public string Of1 { get; set; }
        public string Date1 { get; set; }
        public string datamatrix1 = "";
        public string Reference2 { get; set; }
        public string Commande2 { get; set; }
        public string Of2 { get; set; }
        public string Date2 { get; set; }
        public string datamatrix2 = "";
        public long? id { get; set; }
        public List<string> OFdeIDList { get; set; }
        

        public bool PrintEtiquetteQTR()
        {
            string ZPLstring = @"
                    ^XA
                    ^FO170,30 ^GFA,2750,2750,11,,::::::::::Q0IFC,P0KF8,O03KFE,O0MF8,N01MFE,N07NF,N0JFE001FC,M01JFJ01E,M03IF80FFC07,
                    M07FFE07IF818,M0IFC1KF0C,L01IF07KF8,L03FFE1LFE,L03FFC3MF,L07FF87MF8,L07FF0IF01IFC,L0FFE1FEI01FFE,L0FFC3F800E03FE,
                    K01FFC3EI03E1FF,K01FF878J0F87F8,K01FF07K03E3F8,K03FF0E7CI01F1F8,K03FE0CFFJ0F8FC,K03FE18FFCI07C7C,K03FE11IF8007E7C,
                    K03FC01IFE003F3E,K03FC01JFC01F9E,K03FC01JFE01F9E,K03F801F1IF00FDE,K03F801F07FF80FCE,K03F801F00FF807CE,
                    K03F801F003FC07EE,K03F801FI0FC07E6,K03F801FI07C03E6,:K03F801FI07E03E6,K01F801F8007E03E6,K01F800FE007E03E,
                    K01F800FF007E03E,L0FC00FF007E03E,L0FC007F007E03E,L07C003F037E03E,L07C001F03FE03C,L03EI0303FE03C,L03EK03FC03C,
                    L01F003003FC038,M0F007E03FC078,M0780FF81F807,M0780FFE03006,M03C0IFCI0E,N0E0JFI0C,N060JFC018,N030FBFFE01,
                    O08F8IF,O04F81FF8,P0F807F8,P0F800F8,P0F8007C,:::P0FE007C,P0FF807C,P07FF07C,P07FFC7C,P03IF7C,Q0JFC,Q03IFC,
                    P0C07FFC,P0F81FFC,P0FE07F8,P0FF80F8,N018IF,N0F8IFC,M07F83IF8,L07FF80IFE,K03IF003IF8,K07FF8I07FFC,K07F8J01FFC,
                    K07FL07FC,K07FFL0FC,K01IF0KFC,L01FF8KFC,M01F8KFC,M0FF8KFC,L07FF8KFC,K03FFC0FC,K07FC00FF,K07EI0FFC,K07FE00IF,
                    K03FFE0IFE,L0IF81IF8,L01FF807IF,M03F980IFC,N038E03FFC,L0FI0FC0FFC,K03F8E0FF01FC,K03FCF0FFC07C,K07FCF0IF81C,
                    K079CF8IFE,K070C38JF8,K070C38KF,K070E38F8IFC,K030E38F83FFC,K03C678F80FFC,K03IF8F801FC,K07IF0F8007C,
                    :K07FFE0F8007C,P0F8007C,P0FC007C,K07IF8FF007C,K07IF8FFE07C,K07IF87FF87C,K03IF83FFE7C,N0701JFC,N07007IFC,
                    N03800IFC,N038103FFC,N0787C07FC,K03IF8FF81F8,K07IF8FFE07,K07IF0IFC,K07FFE0JF,N0E0JFC,N070FBIF8,N038F8IFC,
                    N038F81FFC,N078F807FC,N0F8F801FC,K07IF8F8003C,K07IF0F8I0C,K07IF0F8,K03FFC0F8,P0FE,007LF0FF8,007LF87FE,
                    007LF87FFC,007LF83IF,K01F3E00IFC,K03C07003IF8,K038038007FFC,K070038381FFC,K0700387E07FC,K078078FFC0FC,
                    K07C0F8IF03C,K03IF8IFC0C,K03IF0JF8,K01FFE0JFC,L0FFC0F9FFE,N038F87FF,N038F81FF8,K03IFC3803F8,K07JF9800FC,
                    K07JFD8007C,:K03JFC8007C,N039C8007C,:P0CI07C,K03LF007C,K07LF027C,K07LF03FC,:K03LF03FC,S03FC,L07F8J03FC,
                    L0FFC0C003F8,K01FFE0FI0F,K03IF0FCI04,K07CE78FF807C,K078E78FFE07C,K070E38IFC7C,K070E387IF7C,K070E781JFC,
                    K078E7803IFC,K03CFFI0IFC,K03CFFI03FFC,K01CFEJ07FC,L04F8J01FC,T07C,:K07IF8CI07C,K07IF8FI07C,K07IF8FE007C,
                    K03IF0FF807C,M01E0FFE07C,N0F0IFC0C,N0787IF,N0781IFC,N07803IF8,R0IFC,R03FFC,S07FC,O018001FC,P0CI07C,
                    P0EJ0C,P0F,P0F8,P0FCI04,P0FE001C,O01FF007C,P0FFC0FC,P07FE3FC,P03JFC,P01JFC,Q0JF8,Q03FFE,Q07FFC,Q0IF8,
                    P03IFC,P0JFE,O01KF,P0FF8FFC,P0FE07FC,P0FC03FC,P0F001FC,P0CI0FC,O018I07C,T01C,U0C,U04,,::::::::::::::::^ FS
                    ^A0R25,25
                    ^FO125,55
                    ^FDJay electronique 
                    ^FS 
                    ^A0R25,25
                    ^FO100,75
                    ^FDF - 38334 
                    ^FS
                    ^A0R25,25
                    ^FO75,85
                    ^FDSt Ismier
                    ^FS
                    ^A0R35,35
                    ^FO185,280
                    ^FD" + Itmeref.ToUpper() +
                    @"^FS
                    ^A0R30,30
                    ^FO100,280
                    ^FDQte:" +Qtr.ToUpper()+@"
                    ^FS
                    ^A0R30,30
                    ^FO50,280
                    ^FDNmrof: "+Lot.ToUpper()+@"
                    ^FS
                    ^FO95,520
                    ^BQN,2,6
                    ^FD  RQL;"+ Itmeref.Trim().ToUpper() +";"+Qtr.Trim()+";"+Lot.Trim().ToUpper()+ @";" + 
                    @"
                    ^FS
                    ^FS
                    ^XZ";

            GestionTracaProd traca = new GestionTracaProd();
            TRACA_ETIQUETTES traca_et = new TRACA_ETIQUETTES();
            traca_et.CMD = Commande1;
            traca_et.DATE = DateTime.Now;
            traca_et.ITEM = Itmeref;
            traca_et.LOT = Lot;
            traca_et.QTR = Qtr;
            traca_et.NMR_OF = Of1;
            traca_et.REFERENCE1 = Reference1;
            traca_et.TYPE_ETIQUETTE = "PrintEtiquetteQTR";
            traca_et.ID_OPE = id;
            traca.EtiquetteTracaProdAdd(traca_et);
            
            //return Utile.impression_zebra(@"\\JAY137\ZDesigner GK420d", ZPLstring);
            //return Utile.impression_zebra(@"\\JAY137.JAYELECTRONIQUE.ORG\ZDesigner GK420d", ZPLstring);
            return Utile.impression_zebra(@"192.168.1.210", ZPLstring);
            //return Utile.impression_zebra(@"192.168.1.69", ZPLstring);\\JAY137\ZDesigner GK420d

        }
        public bool PrintEtiquetteProd()
        {            
            string ZPLstring1 = @"
                    ^XA

                    ^FO50,572
                    ^GFA,855,855,9,,M07FE,L0JFE,K03KFC,J01MF,J03MFC,J0OF,I03OF8,I07OFE,I0QF,001QF8,003JF801JFC,007IFCI03IFE,00IFEK0JF,00IFCK03IF,01IFM0IF8,03FFEM07FFC,03FFCM03FFC,07FF8M01FFE,07FFO0FFE,0FFEO0IF,0FFEO07FF,1FFCO03FF8,:1FF8O01FF8,3FF8O01FFC,3FFQ0FFC,::7FEQ0FFC,7FEQ07FC,7FEQ07FE,:::::,:::::::::::::::::::L01IF8,K01KF8,K07KFE,J01MF8,J07MFE,I01OF,I03OFC,I07OFE,I0QF,001QF8,003IFE7FEJFC,007IF87FE1IFE,00IFE07FE07IF,01IF807FE01IF8,01IF007FE00IF8,03FFE007FE007FFC,07FFC007FE003FFC,07FF8007FE001FFE,0IFI07FEI0FFE,0FFEI07FEI07FF,1FFCI07FEI07FF,1FFCI07FEI03FF8,1FF8I07FEI03FF8,3FF8I07FEI01FF8,3FFJ07FEI01FFC,3FFJ07FEJ0FFC,::7FEJ07FEJ0FFC,7FEQ07FC,7FEQ07FE,::::7FEQ07FC,,:^FS
                    
                    ^A0R25,25
                    ^FO118,236
                    ^FD"+ Reference1+ @"
                    ^FS 
                    ^A0R25,25
                    ^FO77,236
                    ^FD" + Commande1 + @" 
                    ^FS
                    ^A0R25,25
                    ^FO30,236
                    ^FD" + Of1 + @" 
                    ^FS 
                    ^A0R15,15
                    ^FO131,567
                    ^FD" + Date1 + @"
                    ^FS
                    ^FO45,472
                    ^BQN,2,3
                    ^FDMM,C " + datamatrix1 + @"
                    ^FS
                    ^A0R15,15
                    ^FO23,472
                    ^FDCountry of origin : France
                    ^FS";
            string ZPLstring2 = @"
                    ^FO228,572
                    ^GFA,855,855,9,,M07FE,L0JFE,K03KFC,J01MF,J03MFC,J0OF,I03OF8,I07OFE,I0QF,001QF8,003JF801JFC,007IFCI03IFE,00IFEK0JF,00IFCK03IF,01IFM0IF8,03FFEM07FFC,03FFCM03FFC,07FF8M01FFE,07FFO0FFE,0FFEO0IF,0FFEO07FF,1FFCO03FF8,:1FF8O01FF8,3FF8O01FFC,3FFQ0FFC,::7FEQ0FFC,7FEQ07FC,7FEQ07FE,:::::,:::::::::::::::::::L01IF8,K01KF8,K07KFE,J01MF8,J07MFE,I01OF,I03OFC,I07OFE,I0QF,001QF8,003IFE7FEJFC,007IF87FE1IFE,00IFE07FE07IF,01IF807FE01IF8,01IF007FE00IF8,03FFE007FE007FFC,07FFC007FE003FFC,07FF8007FE001FFE,0IFI07FEI0FFE,0FFEI07FEI07FF,1FFCI07FEI07FF,1FFCI07FEI03FF8,1FF8I07FEI03FF8,3FF8I07FEI01FF8,3FFJ07FEI01FFC,3FFJ07FEJ0FFC,::7FEJ07FEJ0FFC,7FEQ07FC,7FEQ07FE,::::7FEQ07FC,,:^FS

                    ^A0R25,25
                    ^FO296,236
                    ^F" + Reference2 + @"
                    ^FS                      
                    ^A0R25,25
                    ^FO255,236
                    ^FD" + Commande2 + @"
                    ^FS                     
                    ^A0R25,25
                    ^FO208,236
                    ^FD" + Of2 + @"
                    ^FS                   

                    ^A0R15,15
                    ^FO309,567
                    ^FD" + Date2 + @"
                    ^FS
                    
                    ^FO223,472
                    ^BQN,2,3
                    ^FDMM,C " + datamatrix2 + @"
                    ^FS
                    ^A0R15,15
                    ^FO201,472
                    ^FDCountry of origin : France
                    ^FS";

            string ZPLstring3 = @"^XZ";
            string ZPLstring = @"S l1;0,0,68,70,100
                    T 10,10,270,3,6,b;sample
                    S l2;0,0,68,70,100
                    B 10,45,0,GS1DATAMATRIX,0.4;(01)12345678901235(240)1234567890(15)123456
                    ";
                  
            string ZPLstring4 = @"
                    d ASC;totce1
                    0053 0020
                    0000FF09
                    06
                    800207F0
                    03
                    800B007FFF003FFFE7F7FF0000
                    800101 82 800103 82 8005E7F7FFF000
                    800107 82 800107 82 8005E7F7FFF800
                    80010F 82 80011F 82 8005E7F7FFFE00
                    80011F 82 80013F 82 8002E7F7 82 01
                    80013F 82 80013F 82 8002E7F7 82 01
                    80013F 82 80017F 82 8002E7F7 82 800180
                    800B7F80007F800FE7F0007F80
                    80017F 02 8008FE000FE7F0001FC0
                    80017E 02 8008FE000FE7F0001FC0
                    0000FF04 800101 82
                    800407FFEFE7 82 8002F800
                    8007003FFF00FFEFE7 82 8002E000
                    m m
                    J
                    H 75
                    O R
                    S l1;0,0,68,73,100
                    I:TEST;3,30,0;totce1
                    A 1
                    ";
            if (!String.IsNullOrWhiteSpace(Reference2))
            {
                ZPLstring = ZPLstring1 + ZPLstring2 + ZPLstring3;
            }
            else
            {
                ZPLstring = ZPLstring1 + ZPLstring3;
            }
            GestionTracaProd traca = new GestionTracaProd();
            TRACA_ETIQUETTES traca_et = new TRACA_ETIQUETTES();
            traca_et.CMD = Commande1;
            traca_et.DATE = DateTime.Now;
            traca_et.ITEM = Itmeref;
            traca_et.LOT = Lot;
            traca_et.QTR = Qtr;
            traca_et.NMR_OF = Of1;
            traca_et.REFERENCE1 = Reference1;
            traca_et.REFERENCE2 = Reference2;
            traca_et.TYPE_ETIQUETTE = "PrintEtiquetteProd";

            traca.EtiquetteTracaProdAdd(traca_et);
            return Utile.impression_zebra(@"192.168.1.80", ZPLstring4);
        }

    }
}