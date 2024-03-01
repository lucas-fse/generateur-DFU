using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;

namespace JAY.PegaseCore.Hid
{

    /// <summary>
    /// Classe d'import des méthodes d'acces aux produits par USB:HID
     /// </summary>
    public sealed class HidDll
    {
        private static Int32 Pos = 0;

        private string location = "";
        /* define target memory */
        public enum TARGET_TYPE_s
        {
            TARGET_MT = 0,
            TARGET_MO = 1,
            TARGET_UNKNOWN = 10
        }

        /* define microcontroller  target */
        public enum TARGET_MCU_e
        {
            TARGET_CPU_0 = 0,
            TARGET_CPU_1
        } ;

        /* define target memory */
        public enum TARGET_MEM_e
        {
            TARGET_EEP = 0,
            TARGET_SIM,
            TARGET_INTERNAL_FLASH
        } ;

        /* define target structure */
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TARGET_s
        {
            public TARGET_MCU_e target_mcu;
            public TARGET_MEM_e target_mem;
        };

        /* define target memory */
        public enum CIBLE_HID_e
        {
            CIBLE_CPU_0 = 0,  // la memoire Flash du µC STM32F105 ou MASTER
            CIBLE_CPU_1,      // la memoire Flash du µC STM32F103 ou SLAVE
            CIBLE_EEP_0,        // E2Prom du µC STM32F105 ou MASTER
            CIBLE_EEP_1,        // E2Prom du µC STM32F103 ou SLAVE
            CIBLE_SIM,          // la memoire amovible des MT
        } ;

        enum DEVICE_STATUS_e
        {
            DEVICE_NOT_FOUND = 0,    // USB HID target device not found
            DEVICE_FOUND             // USB HID target device found
        } ;

        enum ERR_STATUS_e
        {
            SUCCESS     =	0 ,
            ERR_ERROR	=   -1,
            ERR_TIMEOUT	=   -2,
        } ;

        enum CONTEXT_e
        {
            CONTEXT_AUCUN = 0,		// Define for Test context
            CONTEXT_TEST = 1,		// Define for Test context
            CONTEXT_CONFIG	= 2		// Define for Configuration context
        } ;

        // cr retourné par les méthode de cette classe 
        public enum HidStatus
        {
            //Success = 0,
            //ErrDeviceNotFound = 1,
            //ErrTimeOut = 2,
            //Error = 3,
            ERR_NO = 0x00000000,
            ERR_UNSUPPORTED_CMD = -1,
            ERR_BAD_CMD_FORMAT = -2,
            ERR_BAD_CONTEXT_ID = -3,
            ERR_CONTEXT_NOT_SET = -4,
            ERR_CONTEXT_ERROR_BADID = 0x00000001,
            ERR_CONTEXT_ALREADY_SET = 0x00000002,
            ERR_UNSUPPORTED_GPIO_ID = 0x00000003,
            ERR_BAD_ADDRESS = 0x00000004,
            ERR_MEDIUM_ACCESS_ERROR = 0x00000006,
            ERR_DATA_TRANFERT_ERROR = 0x00000007,
            ERR_NOT_CONNECTED = 0x00000008
        } ;

        public string Get_Location
        { get; set; }
        /// <summary>
        /// Traduction des cr retournés par la DLL c++
        /// - traduire ERR_STATUS_e
        /// </summary>
        /// <param name="err_status">The err_status.</param>
        /// <returns></returns>
        private HidStatus translate_err_status(int err_status)
        {
            HidStatus result = (HidStatus)err_status; //HidStatus.Success;
            //switch ((ERR_STATUS_e)err_status)
            //{
            //    case ERR_STATUS_e.SUCCESS: 
            //        result = HidStatus.Success ;
            //        break;

            //    case ERR_STATUS_e.ERR_TIMEOUT :
            //        result = HidStatus.ErrTimeOut;
            //        break;

            //    case ERR_STATUS_e.ERR_ERROR :
            //        result = HidStatus.ErrTimeOut;
            //        break ;
            //}
            return result;
        }

        // - traduire DEVICE_STATUS_e
        private HidStatus translate_device_status(int device_status)
        {
            HidStatus result = HidStatus.ERR_NO;
            switch ((DEVICE_STATUS_e)device_status)
            {
                case DEVICE_STATUS_e.DEVICE_FOUND :
                    result = HidStatus.ERR_NO;
                    break;
                case DEVICE_STATUS_e.DEVICE_NOT_FOUND :
                    result = HidStatus.ERR_NOT_CONNECTED;
                    break;
            }
            return result;
        }


        /// <summary>
        /// Constructeur
        /// </summary>
        public HidDll()
        {
        }

        /// <summary>
        /// Tester la taille mémoire du produit, 32 ou 128 ko
        /// </summary>
        /// <param name="target"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 Test_Memory_Size(TARGET_TYPE_s target, UInt32 address);

        /// <summary>
        /// Tester la taille de la mémoire
        /// </summary>
        public Int32 TestMemorySize ( TARGET_TYPE_s target, UInt32 address )
        {
            Int32 Result = 0;

            Result = Test_Memory_Size(target, address);

            return Result;
        } // endMethod: TestMemorySize

        /// <summary>
        /// Lire le journal de bord
        /// typedef int  (*DLL_Fxn_Set_Cmd_Function) 
        ///     (TARGET_MCU_e target, unsigned char Cmd,  unsigned char *buffer_in, unsigned char *buffer_out, unsigned long buffer_len);
        /// </summary>
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 Set_Cmd_Function(TARGET_MCU_e target, Byte Cmd,  Byte[] buffer_in, Byte[] buffer_out, UInt32 buffer_len);

        /// <summary>
        /// Appeler la méthode de la dll et retourner le bon résultat
        /// </summary>
        public Int32 SetCmdFunction ( TARGET_MCU_e Cible, Byte Cmd, Byte[] buffer1, Byte[] buffer2, UInt32 BufferLength )
        {
            Int32 Result = 0;

            Result = HidDll.Set_Cmd_Function(Cible, Cmd, buffer1, buffer2, BufferLength);

            return Result;
        } // endMethod: SetCmdFunction

        /// <summary>
        /// Fixer la date via la dll HID
        /// </summary>
        /// <param name="date"></param>
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Set_Time_in_sec(UInt32 date);

        /// <summary>
        /// Fixer la date
        /// </summary>
        /// <param name="date"></param>
        public void SetDate(UInt32 date)
        {
            Set_Time_in_sec(date);
        }
        
        /// <summary>
        /// Lire la date du produit
        /// </summary>
        public UInt32 GetDate ( )
        {
            UInt32 Result;

            Result = Read_Time_in_sec();

            return Result;
        } // endMethod: GetDate

        /// <summary>
        /// Lire la date via la dll HID
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 Read_Time_in_sec();

        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Find_Device(UInt16 vid, UInt16 pid);
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Find_Device_At(UInt16 vid, UInt16 pid, string location, byte[] position);
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Set_Context(CONTEXT_e NEW_CONTEXT);
        /// <summary>
        /// Vérifier si le périphérique est branché.
        /// </summary>
        /// <param name="VID">The VID. tjrs = 0x0483</param>
        /// <param name="PID">The PID. Selectyionne la cible HID a connecter</param>
        /// <returns>
        /// HidStatus.Success si le périphérique est connecté au PC
        /// HidStatus.ErrDeviceNotFound si non trouvé
        /// HidStatus.ErrTimeOut si le device ne repond pas
        /// HidStatus.Error dans les autres cas
        /// </returns>
        public HidStatus Connecter(ushort VID, ushort PID, bool retry)
        {

            HidStatus result = HidStatus.ERR_BAD_ADDRESS;
            Get_Location = "";
            try
            {
                //Wait(JAY.DefaultValues.Get().WaitFlashTimeMs);
                //System.Windows.MessageBox.Show("HidDll -> Find_Device (enter)");
                Byte[] newBuffer = new Byte[1024];
                int res = Find_Device_At(VID, PID,"", newBuffer);
                //System.Windows.MessageBox.Show("Find_Device -> OK");
                if ((DEVICE_STATUS_e)res == DEVICE_STATUS_e.DEVICE_FOUND)
                {
                    Get_Location = System.Text.Encoding.ASCII.GetString(newBuffer,0,20);
                    res = Set_Context(CONTEXT_e.CONTEXT_CONFIG);
                    //System.Windows.MessageBox.Show("Set Context -> OK");
                    res = Get_Last_Error();
                    //System.Windows.MessageBox.Show("Get Last Error -> OK");
                    result = translate_err_status(res);
                    //System.Windows.MessageBox.Show("Translate Err -> OK");
                }
                else
                {
                    // traduire erreur vers HidStatus 
                    if (retry)
                    {
                        Wait(JAY.DefaultValues.Get().WaitFlashTimeMs);
                        res = Find_Device_At(VID, PID,"",newBuffer);
                        Get_Location = System.Text.Encoding.ASCII.GetString(newBuffer, 0, 20);
                        //System.Windows.MessageBox.Show("Find_Device -> OK");
                        if ((DEVICE_STATUS_e)res == DEVICE_STATUS_e.DEVICE_FOUND)
                        {
                            res = Set_Context(CONTEXT_e.CONTEXT_CONFIG);
                            //System.Windows.MessageBox.Show("Set Context -> OK");
                            res = Get_Last_Error();
                            //System.Windows.MessageBox.Show("Get Last Error -> OK");
                            result = translate_err_status(res);
                            //System.Windows.MessageBox.Show("Translate Err -> OK");
                        }
                        else
                        {
                            // traduire erreur vers HidStatus 
                            result = HidStatus.ERR_NOT_CONNECTED;
                        }
                    }
                    else
                    {
                        // traduire erreur vers HidStatus 
                        result = HidStatus.ERR_NOT_CONNECTED;
                    }
                }
            }
            catch (System.DllNotFoundException ex)
            {
                //Erreur absence fichier DLL_PEGASE_HID.dll : Copier le fichier DLL_PEGASE_HID.dll dans le répertoire de l'exécutable
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Close_Device();
        /// <summary>
        /// Déconnecter le périphérique.
        /// </summary>
        public void Deconnecter()
        {
            Close_Device();
        }

        // int Write_Buffer(const TARGET_s *target, const unsigned char *buffer, unsigned long address, unsigned long buffer_len);
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Write_Buffer(ref TARGET_s target, Byte[] buffer, UInt32 address, UInt32 buffer_len);
        /// <summary>
        /// Ecrire un buffer dans une zone memoire du produit
        /// </summary>
        public HidStatus EcrireBuffer(CIBLE_HID_e Cible, Byte[] Buffer, UInt32 Adresse, UInt32 NombreOctets)
        {
            HidStatus result = HidStatus.ERR_NO;
            TARGET_s Target = new TARGET_s();
            
            switch(Cible) // generer champs de la structure Target
            {
                case CIBLE_HID_e.CIBLE_CPU_0 :
                    Target.target_mcu= TARGET_MCU_e.TARGET_CPU_0;
                    Target.target_mem= TARGET_MEM_e.TARGET_INTERNAL_FLASH;
                break;

                case CIBLE_HID_e.CIBLE_CPU_1 :
                    Target.target_mcu= TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem= TARGET_MEM_e.TARGET_INTERNAL_FLASH;
                break;

                case CIBLE_HID_e.CIBLE_EEP_0 :
                    Target.target_mcu= TARGET_MCU_e.TARGET_CPU_0;
                    Target.target_mem= TARGET_MEM_e.TARGET_EEP;
                break;

                case CIBLE_HID_e.CIBLE_EEP_1:
                    Target.target_mcu= TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem= TARGET_MEM_e.TARGET_EEP;
                break;

                case CIBLE_HID_e.CIBLE_SIM :
                    Target.target_mcu= TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem= TARGET_MEM_e.TARGET_SIM;
                break;
            }

            int resultat = Write_Buffer(ref Target, Buffer, Adresse, NombreOctets);

            if (LutteAntiGFI.Instance.VerifyWriteProduct && Cible == CIBLE_HID_e.CIBLE_SIM)
            {
                StreamWriter SWB;
                String BeforeFileName;

                BeforeFileName = "SIM_BeforeFlashage.csv";

                // Créer un fichier temporaire contenant les données s'il n'existe pas
                if (!File.Exists(DefaultValues.Get().TempFilePath + BeforeFileName))
                {
                    SWB = File.CreateText(DefaultValues.Get().TempFilePath + BeforeFileName);
                    SWB.WriteLine(String.Format("adresse début : {0}", Adresse));
                    SWB.Write(SWB.NewLine);
                    Pos = 0;
                }
                else
                {
                    SWB = new StreamWriter(File.Open(DefaultValues.Get().TempFilePath + BeforeFileName, FileMode.Append));
                    SWB.BaseStream.Position = SWB.BaseStream.Length;
                }

                for (int i = 0; i < NombreOctets; i++)
                {
                    SWB.Write(Buffer[i].ToString("X"));
                    SWB.Write(";");
                    Pos++;
                    if (Pos > 7)
                    {
                        Pos = 0;
                        SWB.Write(SWB.NewLine);
                    }
                }
                SWB.Flush();
                SWB.Close();

                //LutteAntiGFI.Instance.NbWriteError += NbErreur;
                LutteAntiGFI.Instance.ErrorMessage = "Erreur d'écriture, différence entre les données à écrire et les données écrites"; 
            }
            // Résultat
            result = translate_err_status(resultat);
   
            return result ;
       }

        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Read_Buffer(ref TARGET_s target, Byte[] buffer, UInt32 address, UInt32 buffer_len);
        /// <summary>
        /// Lire un buffer depuis une zone memoire du produit
        /// </summary>
        public HidStatus LireBuffer(CIBLE_HID_e Cible, Byte[] Buffer, UInt32 Adresse, UInt32 NombreOctets)
        {
            HidStatus result = HidStatus.ERR_NO;
            TARGET_s Target = new TARGET_s();

            switch (Cible) // generer champs de la structure Target
            {
                case CIBLE_HID_e.CIBLE_CPU_0:
                    Target.target_mcu = TARGET_MCU_e.TARGET_CPU_0;
                    Target.target_mem = TARGET_MEM_e.TARGET_INTERNAL_FLASH;
                    break;

                case CIBLE_HID_e.CIBLE_CPU_1:
                    Target.target_mcu = TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem = TARGET_MEM_e.TARGET_INTERNAL_FLASH;
                    break;

                case CIBLE_HID_e.CIBLE_EEP_0:
                    Target.target_mcu = TARGET_MCU_e.TARGET_CPU_0;
                    Target.target_mem = TARGET_MEM_e.TARGET_EEP;
                    break;

                case CIBLE_HID_e.CIBLE_EEP_1:
                    Target.target_mcu = TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem = TARGET_MEM_e.TARGET_EEP;
                    break;

                case CIBLE_HID_e.CIBLE_SIM:
                    Target.target_mcu = TARGET_MCU_e.TARGET_CPU_1;
                    Target.target_mem = TARGET_MEM_e.TARGET_SIM;
                    break;
            }

            int resultat = Read_Buffer(ref Target, Buffer, Adresse, NombreOctets);
            result = translate_err_status(resultat);
            return result;
        }

        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Get_Software_Information(TARGET_MCU_e target, StringBuilder Software_Info);
        /// <summary>
        /// Lire infos de version du firmware du CPU_0 ou CPU_1
        /// </summary>
        public HidStatus LireVersionFirmware(CIBLE_HID_e CPU_Cible, ref String SoftInfo)
        {
            HidStatus result = HidStatus.ERR_NO;
            TARGET_MCU_e target_cpu = TARGET_MCU_e.TARGET_CPU_0;
            StringBuilder sb = new StringBuilder(40);

            switch (CPU_Cible) // 
            {
                case CIBLE_HID_e.CIBLE_CPU_0:
                    target_cpu = TARGET_MCU_e.TARGET_CPU_0;
                    break;

                case CIBLE_HID_e.CIBLE_CPU_1:
                    target_cpu = TARGET_MCU_e.TARGET_CPU_1;
                    break;
            }
            int resultat = Get_Software_Information(target_cpu, sb);
            result = translate_err_status(resultat);
            if (HidStatus.ERR_NO == result)
            {
                SoftInfo = sb.ToString();
            }
            return result;
        }
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Get_Hardware_Information(StringBuilder Hardware_Info);
        public HidStatus LireInformationHardware(ref String HardInfo)
        {
            HidStatus result = HidStatus.ERR_NO;
            StringBuilder sb = new StringBuilder(40);

            int resultat = Get_Hardware_Information(sb);
            result = translate_err_status(resultat);
            if (HidStatus.ERR_NO == result)
            {
                HardInfo = sb.ToString();
            }
            return result;
        }
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Enter_DFU_Mode();
        /// <summary>
        /// Passage du mode HID au mode DFU 
        /// </summary>
        public HidStatus EntrerEnModeDFU()
        {
            HidStatus result = HidStatus.ERR_NO;
            int resultat;
            try
            {
                resultat = Enter_DFU_Mode();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "HidDll");
                throw;
            }
            
            result = translate_err_status(resultat);
            Helper.TimeHelper.Wait(DefaultValues.Get().WaitFlashTimeMs);
            return result;
        }


        /*
        * API Name       : Get_Last_Error.
        * Description    : This API retrieves the Last received error
        * Parameters     :  None
        * Return         : ERR_NO                                0x00
                            ERR_UNSUPPORTED_CMD                   0xFF
                            ERR_BAD_CMD_FORMAT                    0xFE
                            ERR_BAD_CONTEXT_ID                    0xFD
                            ERR_CONTEXT_NOT_SET                   0xFC
                            ERR_CONTEXT_ERROR_BADID               0x01
                            ERR_CONTEXT_ALREADY_SET               0x02
                            ERR_UNSUPPORTED_GPIO_ID               0x03
                            ERR_BAD_ADDRESS                       0x04
                            ERR_MEDIUM_ACCESS_ERROR               0x06
                            ERR_DATA_TRANFERT_ERROR               0x07
         */
        [DllImport("dll_pegase_hid.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Get_Last_Error();
        /// <summary>
        /// renvoi le code de la derniere erreur
        /// </summary>
        /// <returns></returns>
        public int GetLastError()
        {
            int erreur = Get_Last_Error(); 

            return erreur;
        }

        public static void Wait(Int32 milliseconds)
        {
            TimeSpan ts = new TimeSpan();
            DateTime start = DateTime.Now;

            while (ts.TotalMilliseconds < milliseconds)
            {
                ts = DateTime.Now - start;
            }
        } // endMethod: Wait
    }
}
