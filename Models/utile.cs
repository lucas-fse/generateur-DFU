using GenerateurDFUSafir.Models.DAL;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateurDFUSafir.Models
{
    public static class Utile
    {
        static Regex ipAdress = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
        static public bool  impression_zebra(string ImpNameOrIp, string ZPLString)
        {
            bool result = false;
            if (ipAdress.IsMatch(ImpNameOrIp))
            {
                result = impression_zebra_ip(ImpNameOrIp, ZPLString);
            }
            else
            {
                 result = Print_Zebra_name(ImpNameOrIp, ZPLString);
            }
            return result;
        }

        static bool impression_zebra_ip(string ip_adr, string ZPLString)
        {
            string ipAddress = ip_adr;
            int port = 9100;
            bool result = false;
            //ZPL Command(s)

            if (true)
            {
                try
                {
                    //  Open connection
                    System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                    client.Connect(ipAddress, port);

                    //Write ZPL String to connection
                    System.IO.StreamWriter writer =
                    new System.IO.StreamWriter(client.GetStream());
                    writer.Write(ZPLString);
                    writer.Flush();

                    //Close Connection
                    writer.Close();
                    client.Close();
                    result = true;
                }
                catch (Exception ex)
                {
                    // Catch Exception
                    result = false;
                }
            }
            return result;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
        uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
        uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        static bool Print_Zebra_name(string name, string ZPLString)
        {
            //Command to be sent to the printer
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            bool result = false;
            try
            {
                string erreur = "";
                result = RawPrinterHelper.SendStringToPrinter(name, ZPLString, ref erreur);
                GestionTracaProd gp = new GestionTracaProd();
                gp.TracaAction("impression zebra", erreur);
            }
            catch (Exception ex)
            {
                GestionTracaProd gp = new GestionTracaProd();
                gp.TracaAction("impression zebra", ex.Message);
            }
            return result;

            // \\\\JAY137.JAYELECTRONIQUE.ORG\\ZDesigner GK420d

            //// Create a buffer with the command
            //Byte[] buffer = new byte[ZPLString.Length];
            //buffer = System.Text.Encoding.ASCII.GetBytes(ZPLString);
            //// Use the CreateFile external func to connect to the LPT1 port
            //SafeFileHandle printer = CreateFile("USB001", FileAccess.ReadWrite, 0, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            //// Aqui verifico se a impressora é válida
            //if (printer.IsInvalid == true)
            //{
            //    return false;
            //}

            //// Open the filestream to the lpt1 port and send the command
            //FileStream lpt1 = new FileStream(printer, FileAccess.ReadWrite);
            //lpt1.Write(buffer, 0, buffer.Length);
            //// Close the FileStream connection
            //lpt1.Close();
            //return true;

        }
    }
}
