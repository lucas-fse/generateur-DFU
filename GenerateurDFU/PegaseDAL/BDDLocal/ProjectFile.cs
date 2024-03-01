using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using JAY.DAL;
using JAY.PegaseCore.Helper;
using Form = System.Windows.Forms;
using Mvvm = GalaSoft.MvvmLight;
using System.IO.Packaging;

namespace JAY.DAL
{
    public class ProjectFile
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string MOType { get; set; }
        public string MTType { get; set; }

        public ProjectFile() :base()
        {
            this.CreateCommandOpenLastFile();
        }
        public string UserProjectName
        {
            get
            {
                if (!ProjectChemin.Substring(ProjectChemin.Length - 1, 1).Equals("\\"))
                {
                    ProjectChemin += "\\";
                }
                return ProjectChemin + ProjectName;
            }
        }
        public string ProjectChemin { get; set; }
        public ICommand CommandOpenLastFile
        {
            get;
            internal set;
        } // endProperty: CommandOpenLastVersion
        private void CreateCommandOpenLastFile()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandOpenLastFile = new Mvvm.Command.RelayCommand(ExecuteCommandOpenLastFile, CanExecuteCommandOpenLastFile);
        } // endMethod: CreateCommandOpenLastVersion

        public bool CanExecuteCommandOpenLastFile()
        {
            return true;
        }

        public void ExecuteCommandOpenLastFile()
        {
            // à faire : implémenter la commande
            
            System.Windows.Controls.TextBlock obj = new System.Windows.Controls.TextBlock();
            obj.ToolTip = this.UserProjectName;
            string toto = JAY.PegaseCore.Commands.CMD_LOAD_FILE;
            Messenger.Default.Send<CommandMessage>(new CommandMessage(obj, toto));
        }
    }
}