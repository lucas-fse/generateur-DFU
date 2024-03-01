using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace JAY.DAL
{
    /// <summary>
    /// View Model pour la fenêtre Upload To Sim Number
    /// </summary>
    public class Upload2SimNumberViewModel : Mvvm.ViewModelBase
	{
        // Variables
        #region Variables

        private String _message;
        private String _fileName;
        private String _currentIDSim;

        #endregion
        
        // Propriétés
        #region Propriétés

        /// <summary>
        /// Libellé pour le bouton Upload
        /// </summary>
        public String LibelUpload
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/UPLOAD_APPLY");
            }
        } // endProperty: LibelUpload

        /// <summary>
        /// Libellé pour l'ID SIM
        /// </summary>
        public String LibelIDSim
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/UPLOAD_ID");
            }
        } // endProperty: LibelID

        /// <summary>
        /// Les ID Sims en cours
        /// </summary>
        public String CurrentIDSim
        {
            get
            {
                return this._currentIDSim;
            }
            set
            {
                this._currentIDSim = value.ToUpper();
                RaisePropertyChanged("CurrentIDSim");
            }
        } // endProperty: CurrentIDSim

        /// <summary>
        /// Le message à afficher sur la fenêtre
        /// </summary>
        public String Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        } // endProperty: Message

        #endregion
        
        // Constructeur
        #region Constructeur
        
        public Upload2SimNumberViewModel(String FileName)
        {
            this.CreateCommandUpload();
            this._fileName = FileName;
            this.Message = String.Format("Upload {0} file", Path.GetFileNameWithoutExtension(FileName));
        }
        
        #endregion
        
        // Méthodes
        #region Méthodes
        
        #endregion
        
        // Messages
        #region Messages
        
        #endregion

        #region Commandes


        #region CommandUpload
        /// <summary>
        /// La commande Upload
        /// </summary>
        public ICommand CommandUpload
        {
            get;
            internal set;
        } // endProperty: CommandUpload

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandUpload()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandUpload = new Mvvm.Command.RelayCommand(ExecuteCommandUpload, CanExecuteCommandUpload);
        } // endMethod: CreateCommandUpload

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandUpload()
        {
            // Ne rien faire dans le viewModel
        } // endMethod: ExecuteCommandUpload

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandUpload()
        {
            Boolean Result = false;
            //String SimPattern = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("NumSerieSimRule");
            String SimPattern = "^[0-9A-Z]{5}$";
            Regex regex = new Regex(SimPattern);

            if (this.CurrentIDSim != null)
            {
                if (regex.IsMatch(this.CurrentIDSim))
                {
                    Result = true;
                } 
            }

            return Result;
        } // endMethod: CanExecuteCommandUpload 
        #endregion

        #endregion
    } // endClass: ClassName
}
