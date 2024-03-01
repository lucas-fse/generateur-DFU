using System;

namespace JAY.PegaseCore.Hid
{
    /* definition des cible accéssibles via HID */
    public enum CIBLE_HID_e
    {
        CIBLE_CPU_0 = HidDll.CIBLE_HID_e.CIBLE_CPU_0, // la memoire Flash du µC STM32F105 ou MASTER
        CIBLE_CPU_1 = HidDll.CIBLE_HID_e.CIBLE_CPU_1, // la memoire Flash du µC STM32F103 ou SLAVE
        CIBLE_EEP_0 = HidDll.CIBLE_HID_e.CIBLE_EEP_0, // E2Prom du µC STM32F105 ou MASTER
        CIBLE_EEP_1 = HidDll.CIBLE_HID_e.CIBLE_EEP_1, // E2Prom du µC STM32F103 ou SLAVE
        CIBLE_SIM = HidDll.CIBLE_HID_e.CIBLE_SIM,   // la memoire amovible des MT
    } ;


    public class Hid
    {
        public const ushort VidParDefaut = 0x0483;
        public const ushort PidParDefaut = 0x5750;
        //public const UInt32 NombreOctetMax = (60 * 250); // 15 000 max
        public const UInt32 NombreOctetMax = 8192; // 15 000 max
        /// <summary>
        /// Logger de la classe ManagerBase
        /// </summary>
        //private ILog log;

        ///// <summary>
        ///// Gets l'objet utilisé pour les traces
        ///// </summary>
        //protected ILog Log
        //{
        //    get
        //    {
        //        if (this.log == null)
        //        {
        //            this.log = LogManager.GetLogger(this.GetType());
        //        }

        //        return this.log;
        //    }
        //}

        HidDll hidDll = null;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Hid()
        {
        }
        
        /// <summary>
        /// Executer une fonction du processeur
        /// </summary>
        public Int32 SetCmdFunction ( JAY.PegaseCore.Hid.HidDll.TARGET_MCU_e Cible, Byte Cmd, Byte[] buffer1, Byte[] buffer2, UInt32 BufferLength )
        {
            Int32 Result;

            if (hidDll != null)
            {
                Result = hidDll.SetCmdFunction(Cible, Cmd, buffer1, buffer2, BufferLength);
            }
            else
            {
                Result = -1;
            }

            return Result;
        } // endMethod: MethodName

        /// <summary>
        /// Fixer la date du timer du produit
        /// </summary>
        public Boolean SetDate ( UInt32 date )
        {
            Boolean Result;

            if (hidDll != null)
            {
                hidDll.SetDate(date);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        } // endMethod: SetDate
        
        /// <summary>
        /// Lire la date du produit
        /// </summary>
        public UInt32 GetDate ( )
        {
            UInt32 Result;

            if (hidDll != null)
            {
                Result = hidDll.GetDate();
            }
            else
            {
                Result = 0;
            }

            return Result;
        } // endMethod: GetDate

        /// <summary>
        /// Connecter la cible
        /// </summary>
        /// <param name="vid">The vid.</param>
        /// <param name="pid">The pid.</param>
        /// <returns>The result of the methode</returns>
        public bool Connecter(ushort vid, ushort pid)
        {
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;
            bool result = false;
            //try
            //{
                if (null == hidDll)
                {
                    hidDll = new HidDll();
                }
                
                resultat = hidDll.Connecter(vid, pid, false);

                if (HidDll.HidStatus.ERR_NO == resultat || HidDll.HidStatus.ERR_CONTEXT_ALREADY_SET == resultat)
                {
                    result = true;
                }
            //}
            //catch (Exception ex)
            //{
            //    //Log.Error(string.Empty, ex);
            //    hidDll = null;
            //    result = false;
            //}
            //finally
            //{
            //    switch ((int)resultat)
            //    {
            //        case 0:
            //            //Log.InfoFormat("Resultat connection : succes");
            //            break;

            //        case 1:
            //            //Log.ErrorFormat("Resultat connection : ErrDeviceNotFound");
            //            break;

            //        case 2:
            //            //Log.ErrorFormat("Resultat connection : ErrTimeOut");
            //            break;

            //        case 3:
            //            //Log.ErrorFormat("Resultat connection : Error");
            //            break;
            //    }                
            //}

            return result;
        }

        /// <summary>
        /// Déconnecter la cible
        /// </summary>
        public void Deconnecter()
        {
            
            //try
            //{
                if (null != hidDll)
                {
                    hidDll.Deconnecter();
                    hidDll = null;
                }
            //}
            //catch (Exception ex)
            //{
            //    hidDll = null;
            //    //Log.ErrorFormat(ex.Message);
            //}            
        }

        /// <summary>
        /// Envoyer un buffer
        /// </summary>
        /// <param name="cible">The cible.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="adresse">The adresse.</param>
        /// <param name="nombreOctets">The nombre octets.</param>
        /// <returns>The result of the methode</returns>
        public bool EcrireBloc(CIBLE_HID_e cible, Byte[] buffer, UInt32 adresse, UInt32 nombreOctets)
        {
            bool result = false;
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;
            HidDll.CIBLE_HID_e target = (HidDll.CIBLE_HID_e)cible;

            //try
            //{
                if (null != hidDll)
                {
                    UInt32 taille = nombreOctets;
                    UInt32 adresseCourante = adresse;
                    int offsetBuffer = 0;

                    while (taille > NombreOctetMax)
                    {
                        Byte[] newBuffer = new Byte[NombreOctetMax];
                        Buffer.BlockCopy(buffer, offsetBuffer, newBuffer, 0, Convert.ToInt32(NombreOctetMax));
                        resultat = hidDll.EcrireBuffer(target, newBuffer, adresseCourante, NombreOctetMax);
                        result = HidDll.HidStatus.ERR_NO == resultat;
                        if (!result)
                        {
                            throw new Exception("Erreur Ecriture HID");
                        }

                        taille -= NombreOctetMax;
                        offsetBuffer += Convert.ToInt32(NombreOctetMax);
                        adresseCourante += NombreOctetMax;
                        LutteAntiGFI.Instance.CurrentFlashPos += (Int32)NombreOctetMax;
                    }

                    Byte[] bufferRestant = new Byte[taille];
                    Buffer.BlockCopy(buffer, offsetBuffer, bufferRestant, 0, Convert.ToInt32(taille));
                
                    resultat = hidDll.EcrireBuffer(target, bufferRestant, adresseCourante, taille);
                    
                    LutteAntiGFI.Instance.CurrentFlashPos += (Int32)taille;
                    result = HidDll.HidStatus.ERR_NO == resultat;
                }
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //    return result;
            //}
            //finally
            //{
            //    switch ((int)resultat)
            //    {
            //        case 0:
            //            //Log.InfoFormat("Resultat connection : succes");
            //            break;

            //        case 1:
            //            //Log.ErrorFormat("Resultat connection : ErrDeviceNotFound");
            //            break;

            //        case 2:
            //            //Log.ErrorFormat("Resultat connection : ErrTimeOut");
            //            break;

            //        case 3:
            //            //Log.ErrorFormat("Resultat connection : Error");
            //            break;
            //    }
            //}

            return result;
        }
        
        /// <summary>
        /// Tester la taille de la mémoire
        /// </summary>
        public Int32 TestMemorySize( HidDll.TARGET_TYPE_s target )
        {
            Int32 Result = 0;
            UInt32 adresse = 0xFFFFFFFF;

            if (hidDll != null)
            {
                Result = hidDll.TestMemorySize(target, adresse);
            }

            return Result;
        } // endMethod: TestMemorySize

        /// <summary>
        /// Recuperer un bloc de données lu dans la cible vers le buffer
        /// la cible de type CIBLE_HID_e peut être  CIBLE_EEP_0, CIBLE_EEP_1
        /// ou pour le MT seulement CIBLE_SIM
        /// </summary>
        /// <param name="cible">The cible.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="adresse">The adresse.</param>
        /// <param name="nombreOctets">The nombre octets.</param>
        /// <returns>The result of the methode</returns>
        public bool LireBloc(CIBLE_HID_e cible, Byte[] buffer, UInt32 adresse, UInt32 nombreOctets)
        {
            bool result = false;
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;
            HidDll.CIBLE_HID_e target = (HidDll.CIBLE_HID_e)cible;

            //try
            //{
                if (null != this.hidDll)
                {
                    UInt32 taille = nombreOctets;
                    UInt32 adresseCourante = adresse;
                    int offset = 0;
                    
                    while (taille >= NombreOctetMax)
                    {
                        Byte[] newBuffer = new Byte[NombreOctetMax];
                        resultat = hidDll.LireBuffer(target, newBuffer, adresseCourante, NombreOctetMax);
                        result = HidDll.HidStatus.ERR_NO == resultat;
                        if (!result)
                        {
                            throw new Exception("Erreur Lecture HID");
                        }

                        Buffer.BlockCopy(newBuffer, 0, buffer, offset, Convert.ToInt32(NombreOctetMax));
                        taille -= NombreOctetMax;
                        adresseCourante += NombreOctetMax;
                        offset += Convert.ToInt32(NombreOctetMax);
                    }
                    if (taille > 0)
                    {
                        Byte[] tmpBuffer = new Byte[taille];
                        resultat = hidDll.LireBuffer(target, tmpBuffer, adresseCourante, taille);
                        result = HidDll.HidStatus.ERR_NO == resultat;
                        Buffer.BlockCopy(tmpBuffer, 0, buffer, offset, Convert.ToInt32(taille));
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //    return result;
            //}
            //finally
            //{
            //    switch ((int)resultat)
            //    {
            //        case 0:
            //            //Log.InfoFormat("Resultat connection : succes");
            //            break;

            //        case 1:
            //            //Log.ErrorFormat("Resultat connection : ErrDeviceNotFound");
            //            break;

            //        case 2:
            //            //Log.ErrorFormat("Resultat connection : ErrTimeOut");
            //            break;

            //        case 3:
            //            //Log.ErrorFormat("Resultat connection : Error");
            //            break;
            //    }
            //}

            return result;
        }
        public bool GetHarwareInformation(ref string hard)
        {
            bool result = false;
            String hardware = "";
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;

            //try
            //{
            if (null != hidDll)
            {
                resultat = hidDll.LireInformationHardware(ref hardware);
                result = HidDll.HidStatus.ERR_NO == resultat;
                if (result)
                {
                    hard = hardware;
                }
            }

            return result;
        }
        /// <summary>
        /// Recuperer la version du firmaware de la cible CIBLE_CPU_0 ou CIBLE_CPU_1
        /// le resultat est dans la chaine 'version' passée par référence
        /// </summary>
        /// <param name="cible">The cible.</param>
        /// <param name="version">The version.</param>
        /// <returns>The result of the methode</returns>
        public bool GetFirmwareVersion(CIBLE_HID_e cible, ref string version)
        {
            bool result = false;
            String versionCourante = "";
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;
            HidDll.CIBLE_HID_e target = (HidDll.CIBLE_HID_e)cible;
            //try
            //{
                if (null != hidDll)
                {
                    resultat = hidDll.LireVersionFirmware(target, ref versionCourante);
                    result = HidDll.HidStatus.ERR_NO == resultat;
                    if (result)
                    {
                        version = versionCourante;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //    return result;
            //}
            //finally
            //{
            //    switch ((int)resultat)
            //    {
            //        case 0:
            //            //Log.InfoFormat("Resultat connection : succes");
            //            break;

            //        case 1:
            //            //Log.ErrorFormat("Resultat connection : ErrDeviceNotFound");
            //            break;

            //        case 2:
            //            //Log.ErrorFormat("Resultat connection : ErrTimeOut");
            //            break;

            //        case 3:
            //            //Log.ErrorFormat("Resultat connection : Error");
            //            break;
            //    }
            //}

            return result;
        }

        /// <summary>
        /// Verifier la version du firmaware de la cible CIBLE_CPU_0 ou CIBLE_CPU_1
        /// La chaine lue sur la cible est comparée à la chaine expectedVersion
        /// retour : true si la version lue = expectedVersion
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <param name="expectedVersion">The expected version.</param>
        /// <returns>The result of the methode</returns>
        public bool CheckFirmwareVersion(CIBLE_HID_e cpu, String expectedVersion, ref string lu)
        {
            bool result = false;
            String versionLue = "";

            //try
            //{
                result = GetFirmwareVersion(cpu, ref versionLue);
                if (true == result)
                {
                lu = versionLue;
                 int comparaison = String.Compare(versionLue.Substring(0, 10), expectedVersion); //Modif mga : prendre que le debut de version lue : .Substring(0,10) = nom du fichier dfu
                    //if (0 == comparaison)
                    //{
                    //    result = true;
                    //}
                    result = comparaison == 0;
                }
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //}
            
            return result;
        }

        /// <summary>
        /// Quitter le mode HID et entrer dans le Mode DFU (Telechargement de firmware)
        /// retour : true si passage en DFU effectué
        /// </summary>
        /// <returns>The result of the methode</returns>
        public bool EnterDfuMode()
        {
            HidDll.HidStatus resultat = HidDll.HidStatus.ERR_BAD_ADDRESS;
            
            bool result = false;
            //try
            //{
                resultat = hidDll.EntrerEnModeDFU();
                result = HidDll.HidStatus.ERR_NO == resultat;
                
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //}
            //finally
            //{
            //    switch ((int)resultat)
            //    {
            //        case 0:
            //            //Log.InfoFormat("Resultat connection : succes");
            //            break;

            //        case 1:
            //            //Log.ErrorFormat("Resultat connection : ErrDeviceNotFound");
            //            break;

            //        case 2:
            //            //Log.ErrorFormat("Resultat connection : ErrTimeOut");
            //            break;

            //        case 3:
            //            //Log.ErrorFormat("Resultat connection : Error");
            //            break;
            //    }
            //}

            return result;
        }

        /// <summary>
        /// renvoie le code de la derniere erreur de l'API HID
        /// retour : int error
        /// </summary>
        /// <returns>The result of the methode</returns>
        public int GetLastError()
        {
            int result = 0;
            //try
            //{
                result = hidDll.GetLastError();
            //}
            //catch (Exception ex)
            //{
            //    //Log.ErrorFormat(ex.Message);
            //}
            
            return result;
        }
    }
}
