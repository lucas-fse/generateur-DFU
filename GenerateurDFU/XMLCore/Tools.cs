using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace JAY.XMLCore
{
    /// <summary>
    /// Class statics présentant des utilitaires divers
    /// </summary>
    public static class Tools
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Convertir un XDocument en XML document
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Convertir un XmlDocument en XDocument
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Convertir un Int32 en un champ de bit: 0xFF, 0x12, 0x25, 0xFA
        /// </summary>
        public static String UInt32ToBitfield32 ( UInt32 Value )
        {
            String Result;
            String SValue;

            SValue = Convert.ToString(Value, 16);

            Result = SValue.Substring(0, 2);
            for (Int32 i = 2; i < 8; i += 2)
            {
                Result += ", 0x" + SValue.Substring(i, 2);
            }

            return Result;
        } // endMethod: Int32ToBitfield

        /// <summary>
        /// Convertir un Bitfield 32 en Int32
        /// </summary>
        public static UInt32 Bitfield32ToInt32 ( String Value )
        {
            UInt32 Result;
            String[] Parts;

            Parts = Value.Split(new Char[]{ ',' });

            if (Parts.Length > 1)
            {
                Value = "";
                for (int i = 0; i < Parts.Count(); i++)
                {
                    Value += Parts[i].Replace("0x", "").Trim();
                }

                Result = Convert.ToUInt32(Value, 16); 
            }
            else
            {
                Result = 0xFFFFFFFF;
            }
            
            return Result;
        } // endMethod: Bitfield32ToInt32
        
        /// <summary>
        /// Convertir un Int16 en Bitfield 16 : 0xFA, 0xFF
        /// </summary>
        public static String UInt16ToBitfield16 ( UInt16 Value )
        {
            String Result;
            String SValue;

            SValue = Convert.ToString(Value, 16);
            Result = SValue.Substring(0, 2);
            Result += ", 0x" + SValue.Substring(2, 2);

            return Result;
        } // endMethod: Int16ToBitfield16
        
        /// <summary>
        /// Convertir un Bitfield 16 en UInt16
        /// </summary>
        public static UInt16 Bitfield16ToInt16 ( String Value )
        {
            UInt16 Result;
            String[] Parts;

            
            Parts = Value.Split(new Char[] { ',' });

            if (Parts.Length > 1)
            {
                Value = "";
                for (int i = 0; i < Parts.Count(); i++)
                {
                    Value += Parts[i].Replace("0x", "").Trim();
                } 
            }
            else
            {
                Result = 0xFFFF;
            }

            Result = Convert.ToUInt16(Value, 16);

            return Result;
        } // endMethod: Bitfield16ToInt16

        /// <summary>
        /// Remplacer le séparateur de la virgule utilisé dans la chaine de charactère transmise
        /// par le séparateur de virgule de la culture en cours
        /// </summary>
        /// <param name="Value">
        /// La valeur initiale. Le séparateur de virgule peut être le point ou la virgule
        /// </param>
        /// <returns>
        /// La valeur traduite. Le séparateur de virgule est conformae à celui de la culture en cours
        /// </returns>
        public static String FixFloatStringSeparator ( String Value )
        {
            String Result;

            //if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator == ",")
            if(   CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
            {
                Result = Value.Replace('.', ',');
            }
            else
            {
                Result = Value.Replace(',', '.');
            }
            
            return Result;
        } // endMethod: FixFloatStringSeparator

        /// <summary>
        /// Reconstruit une chaine de caractères décrites par une suite de codes Ascci hexadécimaux
        /// avec un séparateur ","
        /// </summary>
        /// <param name="CodeASCII">
        /// la suite des codes hexadécimaux (exemple : 0x31, 0x33x 0x54)
        /// </param>
        /// <returns>
        /// la chaine de caractères reconstruite
        /// </returns>
        public static String ConvertFromASCII2Text(String CodeASCII)
        {
            String Result = "";
            String[] Hexa;
            Char[] separator = new Char[] { ',' };
            Int32 ASCIIValue, i;

            if (CodeASCII == null)
            {
                CodeASCII = "";
            }
            Hexa = CodeASCII.Split(separator);

            for (i = 0; i < Hexa.Length; i++)
            {
                try
                {
                    ASCIIValue = Convert.ToInt32(Hexa[i], 16);
                }
                catch
                {
                    ASCIIValue = 0;
                }
                
                if (ASCIIValue != 0 & ASCIIValue != 255)
                {
                    Result += (Char)ASCIIValue;
                }
            }

            return Result;
        }
        
        /// <summary>
        /// Convertir une valeur booléenne (vrai / faux) en valeur texte (0 / 1)
        /// </summary>
        public static string ConvertFromBoolean2String ( Boolean Value )
        {
            String Result = "";
            if (Value)
            {
                Result = "1";
            }
            else
            {
                Result = "0";
            }

            return Result;
        } // endMethod: ConvertFromBoolean2String

        /// <summary>
        /// Convertir tous les caractères d'une chaine sous la forme d'un code hexadécimal, 
        /// lettre par lettre
        /// </summary>
        /// <param name="Text">
        /// Le texte à convertir. Par exemple "E06B14"
        /// </param>
        /// <returns>
        /// Le texte converti -> "0x30,0x32,0x50"...
        /// </returns>
        public static String ConvertFromText2ASCII ( String Text )
        {
            String Result = "";

            foreach (char c in Text)
            {
                Result += String.Format("0x{0:X},", Convert.ToInt32(c));
            }

            Result = Result.Substring(0, Result.Length - 1);
            
            return Result;
        } // endMethod: ConvertFromText2ASCII

        /// <summary>
        /// Convertir une chaine de caractère en float
        /// </summary>
        /// <param name="str">
        /// La chaine de caractères. Elle est de la forme "0x00, 0x30, 0x55, 0x0f"
        /// </param>
        public static float ConvertFromStringIEEE_2Float ( String str )
        {
            float Result = 0;
            String[] Octets;
            Char[] Separator = new Char[]{','};
            Byte[] Buffer = new Byte[4];

            // 0 - Fixer le bon séparateur de virgule dans le texte transmis
            str = Tools.FixFloatStringSeparator(str);

            // 1 - récupérer la valeur de chacun des octets
            Octets = str.Split(Separator);

            if(Octets.Length != 4)
            {
                return Result;
            }

            for (int i = 0; i < Octets.Length; i++)
            {
                Octets[i] = Octets[i].Trim();
                Octets[i] = Octets[i].Substring(2);
            }

            for (int i = 0; i < Octets.Length; i++)
            {
                Buffer[i] = Convert.ToByte(Octets[i], 16);
            }

            Result = BitConverter.ToSingle(Buffer, 0);

            return Result;
        } // endMethod: ConvertFromString2Double

        /// <summary>
        /// Convertir un float en valeur hexadécimal de chacun des octets
        /// </summary>
        /// <param name="fv">
        /// la valeur de type float
        /// </param>
        /// <returns>Une chaine de charactères de la forme : "0x00, 0x30, 0x55, 0x0f"</returns>
        public static String ConvertFromfloat_2StringIEEE ( float fv )
        {
            String Result = "";
            Byte[] Buffer = new Byte[4];

            for (int i = 0; i < 4; i++)
            {
                Buffer[i] = 0;
            }

            MemoryStream MStream = new MemoryStream(Buffer);
            BinaryWriter BW = new BinaryWriter(MStream);
            BW.Write(fv);
            BW.Flush();
            BW.Close();
            MStream.Close();
            String[] B = new String[4];

            for (int i = 0; i < Buffer.Length; i++)
            {
                B[i] = String.Format("0x{0:X}", Buffer[i]);
            }

            Result = B[0] + ", ";
            Result += B[1] + ", ";
            Result += B[2] + ", ";
            Result += B[3];

            return Result;
        } // endMethod: ConvertFromfloat_2StringIEEE

        /// <summary>
        /// Convertir un texte ASCII en valeur
        /// </summary>
        /// <param name="ASCII">
        /// La chaine de caractère portant la valeur. Elle est du format "0xF5" par exemple.
        /// </param>
        /// <returns>
        /// Un Byte de la valeur transmise sous forme Hexadécimale
        /// </returns>
        public static Byte ConvertASCCI2Byte ( String ASCII )
        {
            Byte Result = 255;

            // Nettoyage de la chaine
            ASCII = ASCII.Trim();
            
            // Conversion
            try
            {
                if (ASCII.Length > 2 && ASCII.Substring(0, 2) == "0x")
                {
                    ASCII = ASCII.Substring(2);
                    Result = Convert.ToByte(ASCII, 16);
                }
                else
                {
                    Result = Convert.ToByte(ASCII, 10);
                }
            }
            catch
            {
                Result = 0xFF;
            }
            
            return Result;
        } // endMethod: ConvertASCCI2Byte
        
        /// <summary>
        /// Convertir un Byte en valeur Hexadécimal sur 8 bytes
        /// </summary>
        /// <param name="Value">
        /// La valeur à convertir
        /// </param>
        /// <returns>
        /// La chaine de charactères hexadécimales correspondante
        /// </returns>
        public static String ConvertByteASCIIByte ( Byte Value )
        {
            String Result;

            try
            {
                Result = String.Format("{0:X}", Value);
                // Compléter le nombre de charactères si besoin
                if (Result.Length < 2)
                {
                    for (int i = 0; Result.Length != 2; i++)
                    {
                        Result = "0" + Result;
                    }
                }
                // Ajoute "0x"
                Result = "0x" + Result;
            }
            catch
            {
                Result = "0xFF";
            }
            
            return Result;
        } // endMethod: ConvertByteASCIIByte

        /// <summary>
        /// Convertir un Byte en valeur Hexadécimal sur 8 bytes
        /// </summary>
        /// <param name="Value">
        /// La valeur à convertir
        /// </param>
        /// <returns>
        /// La chaine de charactères hexadécimales correspondante
        /// </returns>
        public static String ConvertByteASCIIhex(Int32 Value,bool prefixe)
        {
            String Result;

            try
            {
                Result = String.Format("{0:X}", Value);
                // Compléter le nombre de charactères si besoin
                if (Result.Length < 2)
                {
                    for (int i = 0; Result.Length != 2; i++)
                    {
                        Result = "0" + Result;
                    }
                }
                // Ajoute "0x"
                if (prefixe == true)
                {
                    Result = "0x" + Result;
                }
            }
            catch
            {
                Result = "0xFF";
            }

            return Result;
        } // endMethod: ConvertByteASCIIByte

        /// <summary>
        /// Convertir un nombre 'string' en Int32
        /// </summary>
        public static Int32 ConvertASCIIToInt32(String ASCII)
        {
            Int32 Result = 0;

            // Nettoyer la chaine
            ASCII = ASCII.Trim();
            try
            {
                if (ASCII.Length > 2 && ASCII.Substring(0, 2) == "0x")
                {
                    ASCII = ASCII.Remove(0, 2);
                    Result = Convert.ToInt32(ASCII, 16);
                }
                else
                {
                    Result = Convert.ToInt32(ASCII, 10);
                }
            }
            catch
            {
                Result = -1;
            }

            return Result;
        } // endMethod: ConvertASCII2Int16

        /// <summary>
        /// Convertir un nombre Hexa Ascii en Int16
        /// </summary>
        public static Int16 ConvertASCIIToInt16 ( String ASCII )
        {
            Int16 Result = 0;

            // Nettoyer la chaine
            ASCII = ASCII.Trim();
            try
            {
                if (ASCII.Length > 2 && ASCII.Substring(0,2) == "0x")
                {
                    ASCII = ASCII.Remove(0, 2);
                    Result = Convert.ToInt16(ASCII, 16);
                }
                else
                {
                    Result = Convert.ToInt16(ASCII, 10);
                }
                
            }
            catch
            {
                Result = -1;
            }
            
            return Result;
        } // endMethod: ConvertASCII2Int16
        
        /// <summary>
        /// Convertir l'Int16 en une chaine de charactères contenant ça chaine Hexa
        /// </summary>
        public static String ConvertInt16ToString ( Int16 Value )
        {
            String Result;

            Result = String.Format("{0:X}", Value);
            Result = Add0(Result, 16);
            Result = "0x" + Result;

            return Result;
        } // endMethod: ConvertInt16ToString
        
        /// <summary>
        /// Ajouter autant de zéro que nécessaire pour que le nombre Hexa face NbCharacters
        /// de long.
        /// </summary>
        /// <param name="Number">
        /// Le nombre Hexa sous forme de chaine de charactères
        /// </param>
        /// <param name="NbCharacters">
        /// Le nombre de Charactères souhaités
        /// </param>
        /// <returns>
        /// Le nombre hexadécimal completé
        /// </returns>
        private static String Add0 ( String Number, Int32 NbCharacters )
        {
            String Result = Number;

            for (int i = 0; Result.Length == NbCharacters; i++)
            {
                Result = "0" + Result;
            }
            
            return Result;
        } // endMethod: Add0

        /// <summary>
        /// Methode permettant de retrouver la valeur Int32 d'un paramètre décrit par du texte
        /// </summary>
        /// <param name="ListValues">
        /// La liste des valeurs sous la forme : "/absent=0/présent=1"
        /// </param>
        /// <param name="Value">
        /// La valeur textuel dans la liste, par exemple : "absent"
        /// </param>
        /// <returns>
        /// La valeur Int32 correspondant à la valeur textuelle
        /// </returns>
        public static Int32 GetCorrespondantIntValue ( String ListValues, String Value )
        {
            Int32 Result = 0;
            Int32 Pos = 0;
            String[] LValues;
            String IValue;
            Char[] Separator = new Char[]{'/'};

            if (ListValues == null || Value == null)
            {
                return -1;
            }

            LValues = ListValues.Split(Separator);
            foreach (String item in LValues)
            {
                if (item.Contains(Value))
                {
                    // Rechercher le = dans la chaine
                    Pos = item.IndexOf('=');
                    IValue = item.Substring(Pos + 1);
                    try
                    {
                        Result = Convert.ToInt32(IValue);
                    }
                    catch
                    {
                        Result = 0;
                    }
                    break;
                }
            }

            return Result;
        } // endMethod: GetCorrespondantValue
        
        /// <summary>
        /// Retrouve la valeur textuel correspondant à la valeur 
        /// </summary>
        public static String GetCorrespondantStringValue ( String ListValues, Int32 Value )
        {
            String Result = "";
            Int32 Pos = 0;
            String[] LValues;
            String SValue;
            Char[] Separator = new Char[] { '/' };

            if (ListValues != null)
            {
                LValues = ListValues.Split(Separator);

                foreach (String item in LValues)
                {
                    SValue = String.Format("{0}", Value);
                    if (item.Contains(SValue))
                    {
                        // Rechercher le = dans la chaine
                        Pos = item.IndexOf('=');
                        if (Pos > 0 && Pos < item.Length)
                        {
                            Result = item.Substring(0, Pos);
                        }
                    }
                }
            }

            return Result;
        } // endMethod: GetCorrespondantStringValue

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Tools
}
