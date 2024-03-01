using JAY.PegaseCore.Helper;
using JAY.XMLCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GenerateurDfu
{
    public class GenererDfuEeprom
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
        UInt32[] CrcTable = new UInt32[] { 0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f,
                                0xe963a535, 0x9e6495a3, 0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988,
                                0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91, 0x1db71064, 0x6ab020f2,
                                0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
                                0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9,
                                0xfa0f3d63, 0x8d080df5, 0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172,
                                0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 0x35b5a8fa, 0x42b2986c,
                                0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
                                0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423,
                                0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
                                0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 0x76dc4190, 0x01db7106,
                                0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
                                0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d,
                                0x91646c97, 0xe6635c01, 0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e,
                                0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950,
                                0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
                                0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7,
                                0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0,
                                0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9, 0x5005713c, 0x270241aa,
                                0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
                                0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81,
                                0xb7bd5c3b, 0xc0ba6cad, 0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a,
                                0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683, 0xe3630b12, 0x94643b84,
                                0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
                                0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb,
                                0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc,
                                0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5, 0xd6d6a3e8, 0xa1d1937e,
                                0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
                                0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55,
                                0x316e8eef, 0x4669be79, 0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
                                0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28,
                                0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
                                0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f,
                                0x72076785, 0x05005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38,
                                0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 0x86d3d2d4, 0xf1d4e242,
                                0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
                                0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69,
                                0x616bffd3, 0x166ccf45, 0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2,
                                0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc,
                                0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
                                0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693,
                                0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94,
                                0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d };


        BinaryWriter Writer = null;
        int taillerelative = 0;
        int adr_depart = 0;
        public Byte[] image1 = new byte[0x8000];
        public Byte[] image2 = new byte[0x18000];
        public Boolean Image2used = false;
        private UInt32  CRC (byte octet, UInt32 crc)
        { 
            UInt32 Acrc1 = crc / 256;
            UInt32 Acrc2 = ( crc ^ octet) & 0xFF;
            UInt32 CrcTable1 = CrcTable [Acrc2] ;
            UInt32 Acrc3 = CrcTable1 ^ Acrc1;
            return (Acrc3);
        }
        //generation de l'image au format hex des données a ecrire
        public void GenerationHexImage(String filename)
        {
            JAY.FileCore.iDialogPackage package = JAY.FileCore.iDialogPackage.OpenPackage(filename, FileMode.Open);

            XMLProcessing XProcess = new JAY.XMLCore.XMLProcessing();
            XProcess.OpenXML(package.GetCurrentVersionFileStream());
            ObservableCollection<XElement> XEl1 = XProcess.GetNodeByPath("XmlTechnique");
            IEnumerable<XElement> ixel1 = XEl1.DescendantsAndSelf();
            int adresse_suivi = 0;
            for (int e = 0; e < image1.Length; e++)
            {
                image1[e] = 0xFF;
            }
            for (int e = 0; e < image2.Length; e++)
            {
                image2[e] = 0xFF;
            }
            Image2used = false;

            foreach (XElement el in ixel1)
            {
                if ((el.Name.ToString().Equals("Variable")) && (!el.Attribute("offsetAbsolu").Value.Equals("-1") && (!string.IsNullOrWhiteSpace(el.Attribute("offsetAbsolu").Value))))
                {
                    int adreeabs = Convert.ToInt32(el.Attribute("offsetAbsolu").Value);
                    int taille = Convert.ToInt32(el.Attribute("taille").Value);
                    byte[] tmp = ConvertVariableToByteArray(el);
                    uint taille_crc32 = 0;
                    if (el.Attribute("code").Value.Equals("CRC32"))
                    {
                        taille_crc32 = (uint)(adreeabs - adresse_suivi);
                        if ((taille_crc32 > 0) && (adresse_suivi != 0))
                        {
                            uint crc = ByteHelper.CalculCrc32(image1, adresse_suivi + 4, taille_crc32 - 4);
                            byte[] crc_byte = BitConverter.GetBytes(crc);
                            for (int i = 0; i < crc_byte.Length; i++)
                            {
                                if (adresse_suivi < 0x8000)
                                {
                                    image1[adresse_suivi + i] = crc_byte[i];
                                }
                                else
                                {
                                    image2[(adreeabs - 0x8000) + i] = crc_byte[i];
                                    Image2used = true;
                                }
                            }
                        }
                        adresse_suivi = adreeabs;
                    }
                    else
                    {

                    }
                    if (adreeabs < 0x8000)
                    {
                        for (int i = 0; i < taille; i++)
                        {
                            image1[adreeabs + i] = tmp[i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < taille; i++)
                        {
                            image2[(adreeabs - 0x8000) + i] = tmp[i];
                            Image2used = true;
                        }
                    }
                }
            }
            package.ClosePackage();
        }

        public static byte[] ConvertVariableToByteArray(XElement variable)
        {
            byte[] byteArray = null;
            bool conversionPossible = true;

            if (variable != null)
            {
                #region Convertion de la variable en tableau de byte
                if (string.IsNullOrEmpty(variable.Attribute("valeur").Value))
                {
                    byteArray = new byte[Convert.ToInt32(variable.Attribute("taille").Value)];
                    for (int i = 0; i < Convert.ToInt32(variable.Attribute("taille").Value); i++)
                    {
                        byteArray[i] = Convert.ToByte("0xFF", 16);
                    }
                }
                else
                {
                    // Gestion de l'écriture selon le type de la variable
                    if (!string.IsNullOrEmpty(variable.Attribute("type").Value))
                    {
                        switch (variable.Attribute("type").Value.ToUpper())
                        {
                            case StructXmlTypeData.OCTET:
                                byte[] byteArray2 = ByteHelper.GetByteArrayFromBlobString(variable.Attribute("valeur").Value);
                                byteArray = new byte[Convert.ToInt32(variable.Attribute("taille").Value)];
                                for (int y = 0; y < Convert.ToInt32(variable.Attribute("taille").Value); y++)
                                {
                                    byteArray[y] = (byte)0xFF;
                                }
                                for (int y = 0; y < byteArray2.Length; y++)
                                {
                                    byteArray[y] = (byte)byteArray2[y];
                                }
                                break;
                            case StructXmlTypeData.ASCII:
                            case StructXmlTypeData.STRING:
                                if (!string.IsNullOrEmpty(variable.Attribute("valeur").Value))
                                {
                                    byteArray = null;

                                    // Traitement de la valeur selon si decimal ou Hexa
                                    if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                    {
                                        try
                                        {
                                            byte[] byteArray3 = ByteHelper.GetByteArrayFromBlobString(variable.Attribute("valeur").Value);
                                            byteArray = new byte[Convert.ToInt32(variable.Attribute("taille").Value)];
                                            for (int y = 0; y < Convert.ToInt32(variable.Attribute("taille").Value); y++)
                                            {
                                                byteArray[y] = (byte)0xFF;
                                            }
                                            for (int y = 0; y < byteArray3.Length; y++)
                                            {
                                                byteArray[y] = (byte)byteArray3[y];
                                            }

                                        }
                                        catch
                                        {
                                            conversionPossible = false;
                                        }
                                    }
                                    else
                                    {
                                        // Conversion en tableau de byte
                                        byteArray = ASCIIEncoding.ASCII.GetBytes(variable.Attribute("valeur").Value);
                                    }
                                }

                                break;

                            case StructXmlTypeData.BLOB:
                                // Ce type n'existe normalement pas au niveau variable, il est traité au niveau supérieur
                                byteArray = ByteHelper.GetByteArrayFromBlobString(variable.Attribute("valeur").Value);

                                break;

                            case StructXmlTypeData.DICO:
                                // Ce type n'existe normalement pas au niveau variable, il est traité au niveau supérieur
                                break;

                            case StructXmlTypeData.FLOAT:
                            case StructXmlTypeData.FLOAT32a:
                            case StructXmlTypeData.FLOAT32b:
                            case StructXmlTypeData.FLOAT32c:
                                float valeurFloat = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        byte[] byteArrayFloat = ByteHelper.GetByteArrayFromBlobString(variable.Attribute("valeur").Value);

                                        valeurFloat = BitConverter.ToSingle(byteArrayFloat, 0);

                                        ////Int32 valeurint32 = System.Convert.ToInt32(variable.Valeur, 16);
                                        ////valeurFloat = (float)valeurint32;
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!float.TryParse(variable.Attribute("valeur").Value, out valeurFloat))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurFloat);
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.INT16:
                                Int16 valeurInt16 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurInt16 = System.Convert.ToInt16(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!Int16.TryParse(variable.Attribute("valeur").Value, out valeurInt16))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurInt16);
                                }
                                else
                                {
                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.INT:
                            case StructXmlTypeData.INT32:
                                Int32 valeurInt32 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurInt32 = System.Convert.ToInt32(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!Int32.TryParse(variable.Attribute("valeur").Value, out valeurInt32))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurInt32);
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.INT8:
                                sbyte valeurInt8 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurInt8 = System.Convert.ToSByte(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch
                                    {
                                        // CB -> JAY : si une valeur est trop grande, la forcer à 0XFF
                                        // variable.Valeur = "0xFF";

                                        throw new ApplicationException();
                                    }
                                }
                                else
                                {
                                    if (!sbyte.TryParse(variable.Attribute("valeur").Value, out valeurInt8))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = new byte[Convert.ToInt32(variable.Attribute("taille").Value)];
                                    byte[] byteArrayTmp = BitConverter.GetBytes(valeurInt8);

                                    if ((byteArrayTmp.Length > 0))
                                    {
                                         byteArray[0] = byteArrayTmp[0];
                                    }
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.TABLE:
                                // Ce type n'existe normalement pas au niveau variable, il est traité au niveau supérieur
                                break;

                            case StructXmlTypeData.UINT16:
                            case StructXmlTypeData.BITFIELD16:
                                UInt16 valeurUInt16 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurUInt16 = System.Convert.ToUInt16(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!UInt16.TryParse(variable.Attribute("valeur").Value, out valeurUInt16))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurUInt16);
                                }
                                else
                                {
                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.UINT32:
                            case StructXmlTypeData.BITFIELD32:
                            case StructXmlTypeData.BITFIELDT32:
                                UInt32 valeurUInt32 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurUInt32 = System.Convert.ToUInt32(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!UInt32.TryParse(variable.Attribute("valeur").Value, out valeurUInt32))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurUInt32);
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.UINT64:
                                UInt64 valeurUInt64 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurUInt64 = System.Convert.ToUInt64(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!UInt64.TryParse(variable.Attribute("valeur").Value, out valeurUInt64))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = BitConverter.GetBytes(valeurUInt64);
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.UINT8:
                            case StructXmlTypeData.BITFIELD8:
                                byte valeurUInt8 = 0;

                                // Traitement de la valeur selon si decimal ou Hexa
                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        valeurUInt8 = System.Convert.ToByte(variable.Attribute("valeur").Value, 16);
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!byte.TryParse(variable.Attribute("valeur").Value, out valeurUInt8))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    byteArray = new byte[1];
                                    byte[] byteArrayTmp = BitConverter.GetBytes(valeurUInt8);

                                    if (byteArrayTmp.Length > 0)
                                    {
                                        byteArray[0] = byteArrayTmp[0];
                                    }
                                }
                                else
                                {

                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.DATE:
                                DateTime valeurDate = new DateTime();
                                string dateString = variable.Attribute("valeur").Value;

                                if (variable.Attribute("valeur").Value.StartsWith("0x"))
                                {
                                    try
                                    {
                                        byteArray = ByteHelper.GetByteArrayFromBlobString(variable.Attribute("valeur").Value);

                                        dateString = ASCIIEncoding.ASCII.GetString(byteArray, 0, 8);

                                        if (!DateTime.TryParse(dateString, out valeurDate))
                                        {
                                            conversionPossible = false;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        conversionPossible = false;
                                    }
                                }
                                else
                                {
                                    if (!DateTime.TryParse(variable.Attribute("valeur").Value, out valeurDate))
                                    {
                                        conversionPossible = false;
                                    }
                                }

                                if (conversionPossible)
                                {
                                    // Conversion en tableau de byte
                                    // string tmpDate = valeurDate.Year.ToString() + valeurDate.Month.ToString() + valeurDate.Day.ToString();
                                    string tmpDate = valeurDate.ToString("yy/MM/dd");
                                    byteArray = ASCIIEncoding.ASCII.GetBytes(tmpDate);
                                }
                                else
                                {
                                    throw new ApplicationException();
                                }

                                break;

                            case StructXmlTypeData.BOOL:
                            case StructXmlTypeData.BOOLEAN:
                            case StructXmlTypeData.BOOLEEN:
                                bool boolValue = false;
                                if (variable.Attribute("valeur").Value == "1")
                                {
                                    boolValue = true;
                                }

                                byteArray = BitConverter.GetBytes(boolValue);

                                break;

                            default:
                                break;
                        }
                    }
                }

                #endregion
            }

            return byteArray;

        }


        public void GenrateurDFUFile(String filename)
        {
            Writer = new BinaryWriter(File.Open(filename + ".dfu", FileMode.Create), Encoding.UTF8);
            AddPrefix(ref Writer);
            int size = 0;
            if (Image2used)
            { size = 128 * 1024; } else { size = 32 * 1024; }
            AddImages(ref Writer,2,0x08080000,size);
            AddImages(ref Writer, 3, 0x18080000, size );
            AddSuffix(ref Writer,2);
            taillerelative = 0;
            Writer.Close();
        }
        public void GenrateurDFUFileSim(String filename)
        {
            Writer = new BinaryWriter(File.Open(filename + ".dfu", FileMode.Create), Encoding.UTF8);
            AddPrefix(ref Writer);
            int size = 0;
            if (Image2used)
            { size = 128 * 1024; }
            else { size = 32 * 1024; }
            AddImages(ref Writer, 4, 0x180C0000, size);
            AddSuffix(ref Writer, 1);
            taillerelative = 0;
            Writer.Close();
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

        void AddPrefix(ref BinaryWriter writer)
        {
            writer.Seek(0, 0);
            byte[] tmp = new byte[] { 0x44, 0x66, 0x75, 0x53, 0x65 };
            writer.Write(tmp, 0, 5);
            writer.Write((byte)0x01);
            writer.Write((byte)0x02); writer.Write((byte)0x02); writer.Write((byte)0x02); writer.Write((byte)0x02); // *4
            writer.Write((byte)0x00); 
        }
        void AddSuffix(ref BinaryWriter writer,int nbimages)
        {
            int taille = (int)writer.Seek(0, SeekOrigin.Current);
            writer.Seek(6, 0);
            byte size = (byte)(taille & 0xff); writer.Write(size);
            size = (byte)((taille / 256) & 0xff); writer.Write((byte)size);
            size = (byte)((taille / 256 / 256) & 0xff); writer.Write((byte)size);
            size = (byte)((taille / 256 / 256 / 256) & 0xff); writer.Write((byte)size);

            writer.Seek(10, 0);
            writer.Write((byte)nbimages);

            writer.Seek(0, SeekOrigin.End);
            writer.Write((byte)0xFF); writer.Write((byte)0xFF);
            writer.Write((byte)0xff); writer.Write((byte)0xff); writer.Write((byte)0xff); writer.Write((byte)0xff);
            writer.Write((byte)0x1a); writer.Write((byte)0x01); writer.Write((byte)0x55); writer.Write((byte)0x46); writer.Write((byte)0x44); writer.Write((byte)0x10);

            writer.Seek( 0, 0);
            writer.Flush();
            
            uint fullcrc = 0xFFFFFFFF;
            uint taillecrc = 0;
            BinaryReader reader =new BinaryReader( writer.BaseStream);
            while (writer.BaseStream.Position < writer.BaseStream.Length)
            {
                byte ByteRead = reader.ReadByte();
                fullcrc = CRC(ByteRead, fullcrc);
		        taillecrc = taillecrc + 1;
            }
            //reader.Close();
            writer.Seek( 0, SeekOrigin.End);
           
            byte bbd = (byte)(fullcrc & 0xff); writer.Write(bbd);
            bbd = (byte)((fullcrc / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)((fullcrc / 256 / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)((fullcrc / 256 / 256 / 256) & 0xff); writer.Write((byte)bbd);
        }
        void AddImages(ref BinaryWriter writer, int nmr_target, int adr, int size)
        {

            int offset = (int)writer.Seek(0, SeekOrigin.Current);
            // target 
            byte[] tmp = new byte[] { 0x54, 0x61, 0x72, 0x67, 0x65, 0x74 };
            writer.Write(tmp, 0, 6);
            writer.Write((byte)nmr_target);
            //balternate
            writer.Write((byte)0x01); writer.Write((byte)0x00); writer.Write((byte)0x00); writer.Write((byte)0x00); // *4
            //name 
            tmp = new byte[] { 0x45,0x45,0x50,0x52,0x4f,0x4d, (byte)nmr_target.ToString()[0] };
            writer.Write(tmp);
            for(int p=0;p<(255-7);p++)
            {
                writer.Write((byte)0x20);
            }
            //size

            writer.Seek(offset+266, 0); //writer.Write("t");
            byte bbd = (byte)(size+8 & 0xff); writer.Write(bbd);
            bbd = (byte)(((size+8) / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)(((size+8) / 256 / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)(((size+8) / 256 / 256 / 256) & 0xff); writer.Write((byte)bbd);

            writer.Seek(offset + 270, 0); writer.Write(0x01); writer.Write((byte)0x00); writer.Write((byte)0x00); writer.Write((byte)0x00); writer.Write((byte)0x00);
            //element adress
            writer.Seek(offset + 274, 0);
            bbd = (byte)(adr & 0xff); writer.Write(bbd);
            bbd = (byte)((adr/256) & 0xff);  writer.Write((byte)bbd);
            bbd = (byte)((adr / 256/256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)((adr / 256/256/256) & 0xff); writer.Write((byte)bbd);

            writer.Seek(offset + 278, 0);
            bbd = (byte)(size & 0xff); writer.Write(bbd);
            bbd = (byte)((size / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)((size / 256 / 256) & 0xff); writer.Write((byte)bbd);
            bbd = (byte)((size / 256 / 256 / 256) & 0xff); writer.Write((byte)bbd);

            int cpt = 0;int cpt2 = 0;
            //for (int y = 0; y < 0x8000; y++)
            //{
            //    image1[y] = (byte)cpt;
            //    cpt++;
            //    cpt2++;
            //    if (cpt > 237) { cpt = 0; }
            //    if (cpt2 > 25) { cpt2 = 0; } else if (cpt > 12) { image1[y] = 0xff; }

            //}
            //for (int y = 0; y < 96*1024;y++)
            //{
            //    image2[y] = (byte)cpt;
            //    cpt++;
               
            //    cpt2++;
            //    if (cpt > 237) { cpt = 0; }
            //    if (cpt2 > 25) { cpt2 = 0; } else if (cpt > 12) { image2[y] = 0xff; }

            //}
            for (int cur =0;cur< size && cur< (32*1024); cur++)
            {
                writer.Write(image1[cur]);
            }
            Image2used = true;
            if (Image2used)
            {
                for (int cur = 0; cur < size && cur < (96*1024); cur++)
                {
                    writer.Write(image2[cur]);
                }
            }

        }
    }
}
