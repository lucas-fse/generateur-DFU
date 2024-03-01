using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Configuration;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Class VidPidHelper
    /// </summary>
    public static class VidPidHelper
    {
        private static String[] _productName = new String[] {  "GAMA",
                                                               "BETA",
                                                               "PIKA",
                                                               "MOKA",
                                                               "ELIO",
                                                               "ALTO",
                                                               "TIMO",
                                                               "NEMO",
                                                               "SIMOUTIL",
                                                               "SBC"
                                                                };

        /// <summary>
        /// Gets the vid pid.
        /// </summary>
        /// <param name="codeProduit">The code produit.</param>
        /// <param name="vid">The vid.</param>
        /// <param name="pid">The pid.</param>
        public static void GetVidPid(string codeProduit, out ushort vid, out ushort pid)
        {
            vid = ushort.MinValue;
            pid = ushort.MinValue;

            if (!string.IsNullOrEmpty(codeProduit))
            {
                string vidpidString = ConfigurationReader.Instance.GetValue(codeProduit);

                if (!string.IsNullOrEmpty(vidpidString))
                {
                    // Les couples vid-pid sont stockés dans le fichier de configuration de la manière suivante value="VId 0x0483;PId 0x5711"
                    string[] couple = vidpidString.Split(';');

                    if (couple.Length == 2)
                    {
                        // On enlève la chaine VId
                        string vidString = Regex.Replace(couple[0], "VId", "").Trim();

                        // On enlève la chaine PId
                        string pidString = Regex.Replace(couple[1], "PId", "").Trim();

                        // Conversion VId
                        vid = System.Convert.ToUInt16(vidString, 16);

                        // Conversion PId
                        pid = System.Convert.ToUInt16(pidString, 16);
                    }
                }
            }
        }

        /// JAY : CB
        
        /// <summary>
        /// Définir la liste des Pid Vid de l'ensemble des produit de la gamme Pégase
        /// </summary>
        public static ObservableCollection<VidPid> GetListProductVidPid ( )
        {
            ObservableCollection<VidPid> Result;

            Result = new ObservableCollection<VidPid>();

            foreach (String Name in _productName)
            {
                VidPid VP = new VidPid(Name);
                Result.Add(VP);
            }

            return Result;
        } // endMethod: GetListProductVidPid
    }
}
