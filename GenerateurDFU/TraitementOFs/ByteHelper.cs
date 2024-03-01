
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;



    namespace TraitementOFs
{
        /// <summary>
        /// Class ByteHelper
        /// </summary>
        public static class ByteHelper
        {
            const UInt32 POLYNOME = 0x4C11DB7;

            /* valeur initiale de la somme de controle */
            const UInt32 SOMME_INIT = 0xFFFFFFFF;

            /// <summary>
            /// Permet d'initialiser à 0xFF le tableau de byte à l'offsetdonnée et sur la longueur donnée
            /// Si le tableau est null et que l'offset est zéro alors retourne un tableau de byte initialisé de la taille donnée
            /// </summary>
            /// <param name="memoryEep">The memory eep.</param>
            /// <param name="offset">The offset.</param>
            /// <param name="taille">The taille.</param>
            /// <returns>Tableau de byte</returns>
            public static byte[] InitializeByteArray(byte[] memoryEep, long offset, long taille)
            {
                // Si l'objet passé en paramètre est null on en crée un nouveau de la taille fourni
                if (memoryEep == null)
                {
                    if (offset == 0)
                    {
                        memoryEep = new byte[taille];
                    }
                    else
                    {
                        return null;
                    }
                }

                // On vérifie que la longueur à écrire à l'offset donné ne depasse pas la longueur du tableau
                if (memoryEep.Length >= offset + taille)
                {
                    for (long i = offset; i < offset + taille; i++)
                    {
                        memoryEep[i] = 0xFF;
                    }
                }

                return memoryEep;
            }

            /// <summary>
            /// Permet d'écrire un tableau de byte donné dans un dans un autre tableau de byte à un offset donné
            /// </summary>
            /// <param name="inputByteArray">The input byte array.</param>
            /// <param name="valByteArray">The val byte array.</param>
            /// <param name="offset">The offset.</param>
            public static void WriteByteArrayInByteArray(ref byte[] inputByteArray, byte[] valByteArray, long offset)
            {
                if (inputByteArray != null && valByteArray != null && offset >= 0)
                {
                    // On vérifie que la longueur à écrire à l'offset donné ne depasse pas la longueur du tableau
                    if (inputByteArray.Length >= offset + valByteArray.Length)
                    {
                        for (long i = 0; i < valByteArray.Length; i++)
                        {
                            inputByteArray[offset + i] = valByteArray[i];
                        }
                    }
                }
            }

            /// <summary>
            /// Gets the byte array from blob.
            /// </summary>
            /// <param name="stringBlob">The string BLOB.</param>
            /// <returns></returns>
            public static byte[] GetByteArrayFromBlobString(string stringBlob)
            {
                if (!string.IsNullOrEmpty(stringBlob))
                {
                    // Séparation de la chaine
                    string[] tabChaine = stringBlob.Split(',');

                    if (tabChaine.Length > 0)
                    {
                        byte[] blob = new byte[tabChaine.Length];

                        for (int i = 0; i < tabChaine.Length; i++)
                        {
                            blob[i] = Convert.ToByte(tabChaine[i].Trim(), 16);
                        }

                        return blob;
                    }
                }

                return null;
            }

            /// <summary>
            /// Gets the blob string byte array from byte array.
            /// </summary>
            /// <param name="byteArray">The bite array.</param>
            /// <returns>
            /// The Blob string
            /// </returns>
            public static string GetBlobStringFromByteArray(byte[] byteArray)
            {
                if (byteArray != null)
                {
                    // Séparation de la chaine
                    string blobString = string.Empty;

                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        blobString += "0x" + System.Convert.ToString(byteArray[i], 16) + ",";
                    }

                    return blobString.Substring(0, blobString.Length - 1);
                }

                return string.Empty;
            }
        public static string GetBlobStringFromString(string str)
        {
            string result = "";
            if (!String.IsNullOrWhiteSpace(str))
            {
                for (int i=0;i<str.Length;i++)
                {
                    int c = (int)str[i];
                    result += "0x" + System.Convert.ToString(c, 16) + ",";
                }
                //suppression du caractere , de fin 
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
            /// <summary>
            /// Calcul le CRC32 sur les données du blocDonnees 
            /// Le calcul est effectué sur les NbOctetsToCheck octets à partir de l'octet d'offset offsetDebutCalcul .
            /// nbOctetsToCheck doit etre un multiple de 4 car le CRC32 est calculé sur des UInt32
            /// Si le calcul ne peut être effectué, SOMME_INIT est retourné (0xFFFFFFFF);
            /// </summary>
            /// <param name="blocDonnees">The bloc donnees.</param>
            /// <param name="offsetDebutCalcul">The offset debut calcul.</param>
            /// <param name="nbOctetsToCheck">The nb octets to check.</param>
            /// <returns></returns>
        public static UInt32 CalculCrc32(Byte[] blocDonnees, int offsetDebutCalcul, UInt32 nbOctetsToCheck)
            {
                UInt32 CrcCalcule = SOMME_INIT;
                UInt32 Cpt;
                UInt32 Data;
                UInt32[] Bloc32;

                // on ne sait pas calculer sur une taille de bloc non multiple de 4 octets
                // ni sur un bloc inexistant
                if ((nbOctetsToCheck % sizeof(UInt32) != 0) || blocDonnees == null || (blocDonnees.Length < nbOctetsToCheck + offsetDebutCalcul))
                {
                    return CrcCalcule;
                }

                try
                {
                    Bloc32 = new UInt32[nbOctetsToCheck / sizeof(UInt32)];
                    // copie le buffer d'octet dans buffer d'UInt32  
                    Buffer.BlockCopy(blocDonnees, offsetDebutCalcul, Bloc32, 0, (int)nbOctetsToCheck);
                    CrcCalcule = SOMME_INIT;
                    // calcul du CRC sur le bloc 
                    for (Cpt = 0; Cpt < Bloc32.Length; Cpt++)
                    {
                        Data = Bloc32[Cpt];
                        CrcCalcule = Crc32Slow(CrcCalcule, Data);
                    }
                }
                catch (Exception ex)
                {
                    //  Log.Error(string.Empty, ex);
                    throw ex;
                }
                finally
                {
                }
                return CrcCalcule;
            }

            /// <summary>
            /// CRC32s the slow.
            /// </summary>
            /// <param name="Crc">The CRC.</param>
            /// <param name="Data">The data.</param>
            /// <returns></returns>
            private static UInt32 Crc32Slow(UInt32 Crc, UInt32 Data)
            {
                UInt32 i;

                Crc = Crc ^ Data;

                for (i = 0; i < 32; i++)
                {
                    if ((Crc & 0x80000000) != 0)
                    {
                        Crc = (Crc << 1) ^ POLYNOME;
                    }
                    else
                    {
                        Crc = (Crc << 1);
                    }
                }
                return Crc;
            }

            /// <summary>
            /// Gets the string from byte aray.
            /// </summary>
            /// <param name="tmpbyteArray">The tmpbyte array.</param>
            /// <returns>the string value</returns>
            public static string GetStringFromByteAray(byte[] tmpbyteArray)
            {
                if (tmpbyteArray == null && !tmpbyteArray.Any())
                {
                    return string.Empty;
                }

                char[] endOfLine = ASCIIEncoding.ASCII.GetString(new Byte[1] { 0x00 }, 0, 1).ToCharArray();
                string stringValue = string.Empty;
                stringValue = ASCIIEncoding.ASCII.GetString(tmpbyteArray, 0, tmpbyteArray.Length);
                return stringValue.Split(endOfLine)[0];
            }

            /// <summary>
            /// Gets the string ASCII form BLOB string.
            /// </summary>
            /// <param name="blobString">The BLOB string.</param>
            /// <returns></returns>
            public static string GetStringAsciiFormBlobString(string blobString)
            {
                string value;
                if (blobString.Length < 2)
                {
                    value = "";
                }
                else
                {
                    byte[] tmpbyteArray = ByteHelper.GetByteArrayFromBlobString(blobString);
                    value = ByteHelper.GetStringFromByteAray(tmpbyteArray);
                }
                return value;
            }
        }
    }

