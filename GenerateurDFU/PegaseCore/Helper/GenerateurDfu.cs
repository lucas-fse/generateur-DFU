using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace JAY.PegaseCore.Helper
{
    public class GenerateurDfu
    {
        // NOM_SOFTWARE ET ADRESSE de START  
        //NOM_SOFTWARE1 = ""; 
        //ADR_SOFTWARE1_REEL = 0x08003800;
        //ADR_SOFTWARE1_VIR = 0x08003800;
        //// NOM_SOFTWARE ET ADRESSE de START
        //NOM_SOFTWARE2 = "";
        //ADR_SOFTWARE2_REEL = 0x08003800;
        //ADR_SOFTWARE2_VIR = 0x18003800;
        //// NOM_SOFTWARE ET ADRESSE de START
        //NOM_EEPROM3 =   "mobbuz4330.hex";
        //ADR_EEPROM3_REEL =   0x00000000;
        //ADR_EEPROM3_VIR =   0x08080000;
        //// NOM_SOFTWARE ET ADRESSE de START
        //NOM_EEPROM4 =   "mobbuz4331.hex";
        //ADR_EEPROM4_REEL =  0x00000000;
        //ADR_EEPROM4_VIR =  0x18080000;
        //// NOM_SOFTWARE ET ADRESSE de START
        //NOM_EEPROM5 =   "";
        //ADR_EEPROM5_REEL =   0x00000000;
        //ADR_EEPROM5_VIR =  0x180C0000;
        //// NOM_SOFTWARE ET ADRESSE de START radio
        //NOM_EEPROM6 =   "";
        //ADR_EEPROM6_REEL =   0x08003800;
        //ADR_EEPROM6_VIR =  0x28003800;

        BinaryWriter Writer = null;
        int taillerelative = 0;
        int adr_depart = 0;
        public void GenrateurDFUFile(String filename, int adr_gen_soft, int adr_gen_soft_vir, int NumeroTarget)
        {
             Writer = new BinaryWriter(File.Open("toto", FileMode.CreateNew), Encoding.Unicode);
             taillerelative = 0;
        }
        public void CreateNewTargetDFU(int adr_gen_soft, int adr_gen_soft_vir, int NumeroTarget)
        {
            string nom_gen_soft = "";
            string nom_gen_output = "";
            // "adresse virtuel: $adr_gen_soft_vir adresse reelle : $adr_gen_soft \n";
            long PositionDep = Writer.Seek(0, SeekOrigin.Begin);
            // "generation de l'image de $nom_gen_soft a l\'adresse $adr_gen_soft\n curseur : $PositionDep \n";

            // le fichier hex  : SOFT_FILE
            
            int tailleCalc = adr_gen_soft;
            int tailleCalc_vir = adr_gen_soft_vir;
            adr_depart = tailleCalc_vir;
            long PositionCur = Writer.Seek(0, SeekOrigin.Begin);
            for (int cmpa = 0; cmpa < 274; cmpa++)
            { Writer.Write(0x20); }

            Writer.Seek((int)PositionDep, SeekOrigin.Begin);
            Writer.Write("Target");
            Writer.Write(NumeroTarget);
            Writer.Write(0x01); Writer.Write(0x0); Writer.Write(0x0); Writer.Write(0x0);


            Writer.Seek((int)(PositionDep + 11), SeekOrigin.Begin); Writer.Write(nom_gen_output); Writer.Write("-"); Writer.Write(nom_gen_soft);
            Writer.Seek((int)(PositionDep + 266), SeekOrigin.Begin); Writer.Write("t");
            Writer.Seek((int)(PositionDep + 270), SeekOrigin.Begin); Writer.Write(0x01); Writer.Write(0x00); Writer.Write(0x00); Writer.Write(0x00);
            Writer.Seek((int)(PositionDep + 274), SeekOrigin.Begin);
            PositionCur = Writer.Seek(0, SeekOrigin.Begin);
            Writer.Write(0xAA); Writer.Write(0xAA); Writer.Write(0xAA); Writer.Write(0xAA);
            Writer.Write(0xAA); Writer.Write(0xAA); Writer.Write(0xAA); Writer.Write(0xAA);

        }
        public void SendToTargetDFU(byte[] bloc)
        {
            for (int i = 0; i < bloc.Length; i++)
            {
                Writer.Write(bloc);
            }

        }

  /*      public void CloseTargetDFU()
        {
      
            Writer.Seek((int)PositionCur, SeekOrigin.Begin);
            printf " adresse de depart : %X taille relative %x\n",$adr_depart,$taillerelative;
                
            int AdressCourante = (adr_depart & 0x000000FF);
            Writer.Write(AdressCourante); 
            AdressCourante = (adr_depart & 0x0000FF00)/ 256;
            Writer.Write(AdressCourante);
            AdressCourante = (adr_depart & 0x00FF0000)/ 256 / 256;
            Writer.Write(AdressCourante);
            AdressCourante = (int)((adr_depart & 0xFF000000)/ 256 / 256 / 25
               6);
            Writer.Write(AdressCourante); 
  
                     // # taille des datas  
                      $AdressCourante = ($taillerelative & 0x000000FF);
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1); 
                      $AdressCourante = ($taillerelative & 0x0000FF00)/ 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1);
                      $AdressCourante = ($taillerelative & 0x00FF0000)/ 256 / 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1); 
                      $AdressCourante = ($taillerelative & 0xFF000000)/ 256 / 256 / 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1);

                //# taille des datas prefix 
                seek(DFU_FILE,$PositionDep + 266, 0);
                      $AdressCourante = ($taillerelative + 8 & 0x000000FF);
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1); 
                      $AdressCourante = ($taillerelative + 8 & 0x0000FF00)/ 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1);
                      $AdressCourante = ($taillerelative + 8 & 0x00FF0000)/ 256 / 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1); 
                      $AdressCourante = ($taillerelative + 8 & 0xFF000000)/ 256 / 256 / 256;
                syswrite(DFU_FILE, pack('s',$AdressCourante), 1);

                close(SOFT_FILE);
                seek(DFU_FILE, 0, 2);
                       $NumeroTargetValid = $NumeroTargetValid + 1;

    
            }

            

            }
        }*/
    }
}
