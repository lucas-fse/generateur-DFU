using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace JAY
{
    /// <summary>
    /// La classe permettant de piloter la journalisation
    /// </summary>
    public class Journal
    {
        // Variables
        #region Variables
        public const String APPEND = "APPEND";

        private StreamWriter _sr;
        private String _filename;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom du fichier journal ouvert
        /// </summary>
        public String FileName
        {
            get
            {
                return this._filename;
            }
        } // endProperty: FileName

        #endregion


        // Constructeur
        #region Constructeur

        private Journal()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Ajouter un message sur le journal de log
        /// </summary>
        public void AppendLog ( String message )
        {
            // Construire le log
            String Format = String.Format("{0} : {1}", DateTime.Now, message);

            // Ajouter le log au fichier
            try
            {
                this._sr = new StreamWriter(File.Open(this._filename, FileMode.Append));
                if (this._sr != null)
                {
                    this._sr.WriteLine(Format);
                    this._sr.Close();
                }
            }
            catch
            {
            }
        } // endMethod: AppendLog

        /// <summary>
        /// Supprimer tous les logs
        /// </summary>
        public void ClearLogs()
        {
            if (this._sr != null)
            {
                this.CloseLogs();
                File.Delete(this._filename);

                this._sr = new StreamWriter(File.Open(this._filename, FileMode.Append));
                this._sr.Close();
            }
        } // endMethod: ClearLogs
        
        /// <summary>
        /// Fermer le journal
        /// </summary>
        public void CloseLogs ( )
        {
            if (this._sr != null)
            {
                this._sr.Close();
            }
        } // endMethod: CloseLogs

        #endregion

        // Messages
        #region Méthodes statics
        
        /// <summary>
        /// Ouvrir le log désigné par le nom de fichier. Si le fichier existe, il sera ouvert en mode 'Append'.
        /// </summary>
        public static Journal OpenLog ( String FileName )
        {
            Journal Result = new Journal();
            Result._filename = FileName;

            // 2 - Ouvrir le fichier en mode Append
            try
            {
                Result._sr = new StreamWriter(File.Open(FileName, FileMode.Append));

                if (Result._sr == null)
                {
                    Result = null;
                }
                else
                {
                    Result._sr.Close();
                }
            }
            catch
            {
                MessageBox.Show("Impossible de créer le fichier log, les traces ne seront pas enregistrées","Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Result._sr = null;
            }

            return Result;
        } // endMethod: OpenLog

        #endregion

    } // endClass: Journal

	
}
