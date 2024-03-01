using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JAY
{
    public class BatchUtilities
    {
        /// <summary>
        /// Executer un fichier bat avec les bons arguments en boucle
        /// </summary>
        public static void ExecuteBat(String Filename, String BatchFileName, System.Diagnostics.ProcessWindowStyle StyleFenetre)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = BatchFileName;
            proc.StartInfo.RedirectStandardError = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.StartInfo.WindowStyle = StyleFenetre;
            proc.StartInfo.UseShellExecute = true;

            if (Filename != "" && File.Exists(Filename))
            {
                proc.StartInfo.Arguments = Filename;
            }

            proc.Start();
            proc.WaitForExit();
        } // endMethod: ExecuteBat
    }
}
