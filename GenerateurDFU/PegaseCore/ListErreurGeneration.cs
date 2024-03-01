using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Listes des erreurs de génération
    /// </summary>
    public class ListErreurGeneration
    {
        #region private properties
        /// <summary>
        /// Liste des erreurs liées à l'étape de conversion des modes d'exploitation
        /// </summary>
        private ObservableCollection<string> erreursModeExploitation = new ObservableCollection<string>();

        /// <summary>
        /// Liste des erreurs liées à l'étape de conversion des sélecteurs
        /// </summary>
        private ObservableCollection<string> erreursSelecteur = new ObservableCollection<string>();

        /// <summary>
        /// Liste des erreurs liées à l'étape de conversion des retours d'information
        /// </summary>
        private ObservableCollection<string> erreursRetourInformation = new ObservableCollection<string>();

        /// <summary>
        /// Liste des erreurs liées à l'étape de conversion des équations
        /// </summary>
        private ObservableCollection<string> erreursEquations = new ObservableCollection<string>();

        /// <summary>
        /// Liste des erreurs liées à l'étape de conversion des alarmes
        /// </summary>
        private ObservableCollection<string> erreurAlarmes = new ObservableCollection<string>();

        /// <summary>
        /// liste des erreurs liées à l'étape de conversion des entrées sorties
        /// </summary>
        private ObservableCollection<string> erreurEntreesSorties = new ObservableCollection<string>();

        /// <summary>
        /// liste des erreurs liées à l'étape de conversion du logo
        /// </summary>
        private ObservableCollection<string> erreurLogo = new ObservableCollection<string>();

        #endregion

        #region public properties

        /// <summary>
        /// Gets or sets the erreurs mode exploitation.
        /// </summary>
        /// <value>
        /// The erreurs mode exploitation.
        /// </value>
        public ObservableCollection<string> ErreursModeExploitation
        {
            get { return this.erreursModeExploitation; }
            set { this.erreursModeExploitation = value; }
        }

        /// <summary>
        /// Gets or sets the erreurs selecteur.
        /// </summary>
        /// <value>
        /// The erreurs selecteur.
        /// </value>
        public ObservableCollection<string> ErreursSelecteur
        {
            get { return this.erreursSelecteur; }
            set { this.erreursSelecteur = value; }
        }

        /// <summary>
        /// Gets or sets the erreurs retour information.
        /// </summary>
        /// <value>
        /// The erreurs retour information.
        /// </value>
        public ObservableCollection<string> ErreursRetourInformation
        {
            get { return this.erreursRetourInformation; }
            set { this.erreursRetourInformation = value; }
        }

        /// <summary>
        /// Gets or sets the erreurs equations.
        /// </summary>
        /// <value>
        /// The erreurs equations.
        /// </value>
        public ObservableCollection<string> ErreursEquations
        {
            get { return this.erreursEquations; }
            set { this.erreursEquations = value; }
        }

        /// <summary>
        /// Gets or sets the erreur alarmes.
        /// </summary>
        /// <value>
        /// The erreur alarmes.
        /// </value>
        public ObservableCollection<string> ErreurAlarmes
        {
            get { return this.erreurAlarmes; }
            set { this.erreurAlarmes = value; }
        }

        /// <summary>
        /// Gets or sets the erreur entrees sorties.
        /// </summary>
        /// <value>
        /// The erreur entrees sorties.
        /// </value>
        public ObservableCollection<string> ErreurEntreesSorties
        {
            get { return this.erreurEntreesSorties; }
            set { this.erreurEntreesSorties = value; }
        }

        /// <summary>
        /// Gets or sets the erreur logo.
        /// </summary>
        /// <value>
        /// The erreur entrees sorties.
        /// </value>
        public ObservableCollection<string> ErreurLogo
        {
            get { return this.erreurLogo; }
            set { this.erreurLogo = value; }
        }

        #endregion
    }
}
