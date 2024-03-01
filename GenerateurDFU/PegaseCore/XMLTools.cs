using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Class présentant des méthodes statiques permettant d'optimiser un fichier de paramétrage
    /// iDialog. Par optimisation on entend supression des lignes inutiles du fichier XML. Par exemple, si seul 
    /// 2 modes d'exploitation sont utilisés, alors seuls deux balises correspondant à la description des modes seront concervées.
    /// A l'origine, les fichiers XML comportent 32 modes...
    /// </summary>
    public static class XMLTools
    {
        private const String PICTURE_PATH = "/pictures/";

        #region Méthodes statiques

        /// <summary>
        /// Optimise le nombre de modes réellement présents dans le fichier XML en fonction du nombre de mode déclarés
        /// </summary>
        public static void OptimiseNbMode( )
        {
            XMLCore.XMLProcessing MetierProces = new XMLCore.XMLProcessing();
            MetierProces.OpenXML(PegaseData.Instance.MetierRoot);
            String[] XMLPath = new String[] { 
                                                "ParametresApplicatifsFixesMO/ModesExploitation/ModeExploitation",
                                                "ParametresApplicatifsFixesMO/ModesExploitation/DescriptInterverrouillage/",
                                                "ParametresApplicatifsFixesMO/ModesExploitation/ConfigSelecteursMode/",
                                                "ParametresApplicatifsFixesMO/ModesExploitation/CollecConfigRetour/",
                                                "ParametresApplicatifsFixesMO/ModesExploitation/CollecFormules/"
                                            };
            IEnumerable<XElement> Nodes;
            String Value;
            Int32 NbModes;

            // 1 - rechercher le nombre de modes (N) déclarer dans la section mode universel
            Nodes = MetierProces.GetNodeByPath("HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels");
            if (Nodes == null)
            {
                return;
            }
            Value = Nodes.Attributes(XMLCore.XML_ATTRIBUTE.VALUE).First().Value;

            try
            {
                NbModes = Convert.ToInt32(Value);
            }
            catch
            {
                NbModes = 0;
            }

            // 2 - charger tous les modes, concerver les N premiers et supprimer les autres
            foreach (String xmlPath in XMLPath)
            {
                Nodes = MetierProces.GetNodeByPath(xmlPath);
                for (int i = Nodes.Count() - 1; i > NbModes - 1; i--)
			    {
			        XElement DelNode = Nodes.ElementAt(i);
                    DelNode.Remove();
			    }
            }
        } // endMethod: OptimiseNbMode

        /// <summary>
        /// Importer les nouvelles bitmaps déclarées dans le .iDialog en cours d'utilisation
        /// Dans les items comportants le paramétrage 'DIRECT_TO_.BMP'
        /// </summary>
        public static void ImportNewBitmaps ( )
        {
            ObservableCollection<XElement> listPictures = new ObservableCollection<XElement>();

            // rechercher tous les champs XmlMétier pouvant comporter des définitions 

            // 1 - le logo
            IEnumerable<XElement> Query = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LogoClient/BitmapLogo");
            listPictures.Add(Query.First());

            // 2 - le libellé équipement
            IEnumerable<XElement> LibelEquip = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LibelleEquipement");
            if (LibelEquip != null)
            {
                foreach (var row in LibelEquip)
                {
                    XMLCore.XMLProcessing process = new XMLCore.XMLProcessing();
                    process.OpenXML(row);

                    if (process.GetValue("BitmapEquip", "", "", XMLCore.XML_ATTRIBUTE.VALUE) == Constantes.DIRECT_TO_BMP)
                    {
                        listPictures.Add(process.GetNodeByPath("FichierBitmap").First());
                    }
                } 
            }

            // 3 - les libellés
            IEnumerable<XElement> LibelSelecteurs = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleSelecteurs/LibellesSelecteur");
            if (LibelSelecteurs != null)
            {
                foreach (var row in LibelSelecteurs)
                {
                    XMLCore.XMLProcessing process = new XMLCore.XMLProcessing();
                    process.OpenXML(row);

                    String LibelSelecteur = process.GetValue("LibelSelecteur", "", "", XMLCore.XML_ATTRIBUTE.VALUE);

                    if (LibelSelecteur == Constantes.DIRECT_TO_BMP)
                    {
                        listPictures.Add(process.GetNodeByPath("NomFichierBitmapSelecteur").First());
                    }
                } 
            }

            // 4 - les retours d'informations
            IEnumerable<XElement> LibelInformations = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleInformation/LibellesInformation");
            if (LibelInformations != null)
            {
                foreach (var row in LibelInformations)
                {
                    XMLCore.XMLProcessing process = new XMLCore.XMLProcessing();
                    process.OpenXML(row);

                    if (process.GetValue("LibelInformation", "", "", XMLCore.XML_ATTRIBUTE.VALUE) == Constantes.DIRECT_TO_BMP)
                    {
                        listPictures.Add(process.GetNodeByPath("NomFichierBitmapInformation").First());
                    }
                } 
            }

            //Query.Concat<XElement>(PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/"));
            //Query.Concat<XElement>(PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/"));
            ImportNewBitmaps(listPictures);
        } // endMethod: ImportNewBitmaps

        /// <summary>
        /// Importer les nouvelles bitmaps déclarées
        /// </summary>
        public static void ImportNewBitmaps ( ObservableCollection<XElement> elements )
        {
            foreach (var variable in elements)
            {
                // 0 - vérifier s'il s'agit d'un chemin réseau ou d'un chemin iDialog (PackageZIP)
                String FileName = variable.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                if (FileName.Contains('\\'))
                {
                    // 1 - Vérifier si le fichier existe dans le .iDialog
                    String FileN = PICTURE_PATH + Path.GetFileName(FileName);
                    FileN = FileN.Replace(' ', '_');
                    FileN = CleanFileName(FileN);
                    JAY.PegaseCore.PegaseData.Instance.CurrentPackage.OpenPackage();
                    Uri PicturePart = new Uri(FileN, UriKind.Relative);
                    Stream FileStream = JAY.PegaseCore.PegaseData.Instance.CurrentPackage.GetPartStream(PicturePart);

                    // 2 - s'il existe, l'effacer
                    if (FileStream != null)
                    {
                        JAY.PegaseCore.PegaseData.Instance.CurrentPackage.DeletePackagePart(PicturePart);
                    }
                    // 3 - copier le fichier dans le iDialog

                    FileInfo fileTmp = new FileInfo(FileName);
                    if (fileTmp.Exists)
                    {
                        ZipPackagePart packPart = JAY.PegaseCore.PegaseData.Instance.CurrentPackage.AddPicturePart(FileName, FileN);
                    }
                    // 4 - copier le nom du fichier dans le XML
                    variable.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = FileN;

                    JAY.PegaseCore.PegaseData.Instance.CurrentPackage.ClosePackage();
                }
            }
        } // endMethod: ImportNewBitmaps

        /// <summary>
        /// Importer les nouvelles bitmaps déclarées
        /// </summary>
        public static void ImportNewBitmaps(ObservableCollection<String> filenames, FileCore.iDialogPackage idp)
        {
            if (filenames != null && filenames.Count > 0)
            {
                foreach (var filename in filenames)
                {
                    // 0 - vérifier s'il s'agit d'un chemin réseau ou d'un chemin iDialog (PackageZIP)
                    // 1 - Vérifier si le fichier existe dans le .iDialog
                    String FileN = filename;
                    FileN = FileN.Replace(' ', '_');
                    FileN = "/pictures/" + Path.GetFileName(FileN);
                    FileN = CleanFileName(FileN);

                    Uri PicturePart = new Uri(FileN, UriKind.Relative);
                    Stream FileStream = idp.GetPartStream(PicturePart);

                    // 2 - s'il existe, l'effacer
                    if (FileStream != null)
                    {
                        idp.DeletePackagePart(PicturePart);
                    }
                    // 3 - copier le fichier dans le iDialog

                    FileInfo fileTmp = new FileInfo(filename);
                    if (fileTmp.Exists)
                    {
                        ZipPackagePart packPart = idp.AddPicturePart(filename, FileN);
                    }
                } 
            }
        } // endMethod: ImportNewBitmaps
        public static void ImportNewBitmaps(String StremName, MemoryStream streamdata, FileCore.iDialogPackage idp)
        {
            if ((streamdata != null ) && (!String.IsNullOrEmpty(StremName)))
            {
                
                    // 0 - vérifier s'il s'agit d'un chemin réseau ou d'un chemin iDialog (PackageZIP)
                    // 1 - Vérifier si le fichier existe dans le .iDialog
                    String FileN = StremName;
                    FileN = FileN.Replace(' ', '_');
                    FileN = "/pictures/" + Path.GetFileName(FileN);
                    FileN = CleanFileName(FileN);

                    Uri PicturePart = new Uri(FileN, UriKind.Relative);
                    Stream FileStream = idp.GetPartStream(PicturePart);

                    // 2 - s'il existe, l'effacer
                    if (FileStream != null)
                    {
                        idp.DeletePackagePart(PicturePart);
                    }
                    // 3 - copier le fichier dans le iDialog

                    FileInfo fileTmp = new FileInfo(StremName);
                    ZipPackagePart packPart = idp.AddPicturePart( streamdata, FileN);
                    
            }
        } // endMethod: ImportNewBitmaps

        /// <summary>
        /// Nettoyer le nom de fichier en supprimant les caractères inattendus
        /// </summary>
        private static String CleanFileName ( String filename )
        {
            String Result = "";
            Result = Regex.Replace(filename, @"[^0-9a-zA-Z/.]", String.Empty);
            return Result;
        } // endMethod: CleanFileName



        #region Calcul CRC

        const UInt32 POLYNOME = 0x4C11DB7;
        const UInt32 SOMME_INIT = 0xFFFFFFFF;

        /// <summary>
        /// Calculer le CRC d'un élément XML
        /// </summary>
        public static UInt32 CalculCRCStr(String chaine)
        {
            UInt32 Result;

            // fabriquer le tableau de byte pour le calcul du crc
            Int32 Complement = (sizeof(UInt32) - chaine.Length % sizeof(UInt32));
            Int32 Lenght = chaine.Length + Complement;
            Byte[] buffer = new Byte[Lenght];
            Int32 i;

            for (i = 0; i < chaine.Length; i++)
            {
                buffer[i] = (Byte)chaine[i];
            }

            for (; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            Result = CalculCrc32(buffer, 0, (UInt32)buffer.Length);

            return Result;
        } // endMethod: CalculCRCXML

        /// <summary>
        /// Calculer le CRC d'un élément XML
        /// </summary>
        public static UInt32 CalculCRCXML ( XElement Element )
        {
            UInt32 Result;

            // fabriquer le tableau de byte pour le calcul du crc
            String xml = Element.ToString();
            Int32 Complement = (sizeof(UInt32) - xml.Length % sizeof(UInt32));
            Int32 Lenght = xml.Length + Complement;
            Byte[] buffer = new Byte[Lenght];
            Int32 i;

            for (i = 0; i < xml.Length; i++)
            {
                buffer[i] = (Byte)xml[i];
            }

            for (; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            Result = CalculCrc32(buffer, 0, (UInt32)buffer.Length);

            return Result;
        } // endMethod: CalculCRCXML

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
        private static UInt32 CalculCrc32(Byte[] blocDonnees, int offsetDebutCalcul, UInt32 nbOctetsToCheck)
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

        #endregion

        #endregion
    }
}