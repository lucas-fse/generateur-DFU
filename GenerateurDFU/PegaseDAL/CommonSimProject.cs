using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.DAL
{
    /// <summary>
    /// Classe permettant d'homogénéïser les données SIMProject distant et local
    /// Permet également d'optimiser le téléchargement des données distantes
    /// </summary>
    public class CommonSimProject
    {
        /// <summary>
        /// L'ID de la SIM
        /// </summary>
        public String IDSim
        {
            get;
            set;
        } // endProperty: IDSim

        /// <summary>
        /// La date et l'heure de flashage du projet
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        } // endProperty: Date

        public CommonSimProject()
        {
        }
    }
}
