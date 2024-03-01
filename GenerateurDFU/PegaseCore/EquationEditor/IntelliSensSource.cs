using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La source des données utilisées pour l'IntelliSens
    /// </summary>
    public class IntelliSensSource
    {
        #region Constantes

        public const String MN_SELECTEUR = "sélecteurs";
        public const String MN_LIBEL_SELECTEUR = "libellé sélecteur";
        public const String MN_RETOUR_ALARME = "retour et alarme";
        public const String MN_LIBEL_RETOUR_ALARME = "libellé retour et alarme";

        #endregion

        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des Mnémoniques des sélecteurs
        /// </summary>
        public static ObservableCollection<String> MNemoSelecteur
        {
            get
            {
                ObservableCollection<String> Result = new ObservableCollection<String>();

                var Query = from row in PegaseData.Instance.OLogiciels.Selecteurs
                            where row.IdentSelecteur != ""
                            orderby row.IdentSelecteur ascending
                            select row.IdentSelecteur;

                foreach (var item in Query)
                {
                    Result.Add(item);
                }

                return Result;
            }
        } // endProperty: MNemoSelecteur


        /// <summary>
        /// La liste des Mnémoniques des libellés sélecteurs
        /// </summary>
        public static ObservableCollection<String> MnemoLibelleSelecteur
        {
            get
            {
                ObservableCollection<String> Result = new ObservableCollection<String>();

                var Query = from row in PegaseData.Instance.OLogiciels.LibelSelecteurs
                            where row.IdentLibelSelecteur != ""
                            orderby row.IdentLibelSelecteur ascending
                            select row.IdentLibelSelecteur;

                foreach (var item in Query)
                {
                    Result.Add(item);
                }

                return Result;
            }
        } // endProperty: MnemoLibelleSelecteur

        /// <summary>
        /// La liste des Mnémoniques des retours et alarmes
        /// </summary>
        public static ObservableCollection<String> MnemoRetourEtAlarme
        {
            get
            {
                ObservableCollection<String> Result = new ObservableCollection<String>();

                var Query = from row in PegaseData.Instance.OLogiciels.Informations
                            where row.IdentRetour != ""
                            orderby row.IdentRetour ascending
                            select row.IdentRetour;

                foreach (var item in Query)
                {
                    Result.Add(item);
                }

                return Result;
            }
        } // endProperty: MnemoRetourEtAlarme

        /// <summary>
        /// La liste des Mnémoniques des Libellés Retours et Alarmes
        /// </summary>
        public static ObservableCollection<String> MnemoLibelleRetourEtAlarme
        {
            get
            {
                ObservableCollection<String> Result = new ObservableCollection<String>();
                var Query = from row in PegaseData.Instance.OLogiciels.LibelRI
                            where row.IdentLibelInformation != ""
                            orderby row.IdentLibelInformation ascending
                            select row.IdentLibelInformation;

                foreach (var item in Query)
                {
                    Result.Add(item);
                }

                return Result;
            }
        } // endProperty: MnemoLibelleRetourEtAlarme

        /// <summary>
        /// La liste des Mnémoniques Physique
        /// </summary>
        public static ObservableCollection<String> MnemoPhysique
        {
            get
            {
                ObservableCollection<String> Result = new ObservableCollection<String>();

                Result.Add("A implementer...");
                return Result;
            }
        } // endProperty: MnemoPhysique

        #endregion

        // Constructeur
        #region Constructeur

        public IntelliSensSource()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: IntelisensSource
}