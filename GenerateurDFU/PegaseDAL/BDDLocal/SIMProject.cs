using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.DAL
{
    /// <summary>
    /// Ajout de code sur la classe générée automatiquement par Entity Framework
    /// </summary>
    public partial class SIMProject
    {
        #region Propriétés

        /// <summary>
        /// Libellé pour le chargement
        /// </summary>
        public String LibelLoad
        {
            get
            {
                return "Plg_ConnexionT/TOOLS_LOAD";
            }
        } // endProperty: LibelLoad


        /// <summary>
        /// AFaire: Ajouter une description de la propriété
        /// </summary>
        public String VersionNumber
        {
            get
            {
                String Result;
                Result = String.Format("{0:0000}", this.Version);

                return Result;
            }
        } // endProperty: VersionNumber

        #endregion

        public SIMProject()
        {
            this.CreateCommandOpen();
        }

        #region Commandes

        #region CommandOpen
        /// <summary>
        /// La commande Open
        /// </summary>
        public ICommand CommandOpen
        {
            get;
            internal set;
        } // endProperty: CommandOpen

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandOpen()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandOpen = new Mvvm.Command.RelayCommand(ExecuteCommandOpen, CanExecuteCommandOpen);
        } // endMethod: CreateCommandOpen

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandOpen()
        {
            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, JAY.PegaseCore.Commands.CMD_LOAD_FLASH));
        } // endMethod: ExecuteCommandOpen

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandOpen()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandOpen 
        #endregion

        #endregion

    }
}
