using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace JAY.FileCore
{

    public class FPFormat
    {
        // constantes
        public static String ID_FICHE_FP_STANDARD = "FPS";
        public static String ID_FICHE_FP_INSTALLEE = "FPI";

        // Variables singleton
        private static FPFormat _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'instance de la classe
        /// </summary>
        public static FPFormat Instance
        {
            get
            {
                return FPFormat.Get();
            }
        } // endProperty: Instance

        #endregion


        // Constructeur
        #region Constructeur

        private FPFormat()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Vérifier si la format du nom de la fiche est conforme au nom d'une fiche standard
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        public String IsFPS(String FileName)
        {
            string Result =null;

            if (FileName != null &&
                FileName.Substring(8, 1) == "_" &&
                FileName.Substring(9, 2) == "00" &&
                FileName.Substring(0, 3) == FPFormat.ID_FICHE_FP_STANDARD)
            {
                Result = Path.GetFileNameWithoutExtension(FileName);
            }
            else
            {
                Result = null;
            }
            return Result;
        }
        
        /// <summary>
        /// Vérifier si le nom de la fiche est conforme à une fiche installée
        /// </summary>
        public String IsFPI ( String FileName )
        {
            String Result =null;

            if (FileName != null &&
                FileName.Substring(0, 3) == FPFormat.ID_FICHE_FP_INSTALLEE)
            {
                Result = Path.GetFileNameWithoutExtension(FileName);
            }
            else
            {
                Result = null;
            }

            return Result;
        } // endMethod: IsFPI

        /// <summary>
        /// Retourne le numéro d'ordre du fichier
        /// </summary>
        public String ReturnNumOrdre(String FileName ,Boolean booltmp)
        {
            String Result = "";
            if ((false != booltmp) &&
                (null != IsFPS(FileName)))
            {
                Result = FileName.Substring(9, 2);
            }
            else if ((true != booltmp) &&
                (null != IsFPI(FileName)))
            {
                Result = FileName.Substring(18, 2);
            }
            else
            {
                Result = null;
            }
            return Result;
        } // endProperty: ReturnNumOrdre

        public String ReturnVersion(String FileName, Boolean booltmp)
        {
            String Result = "";
            if ((false != booltmp) &&
                (null != IsFPS(FileName)))
            {
                Result = FileName.Substring(12, 2);
            }
            else if ((true != booltmp) &&
                (null != IsFPI(FileName)))
            {
                Result = FileName.Substring(21, 2);
            }
            else
            {
                Result = null;
            }
            return Result;
        } // endProperty: ReturnVersion

        // Retourne une instance unique de la classe
        private static FPFormat Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new FPFormat();


                return _instance;
            }
        }

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: FPFormat
}
