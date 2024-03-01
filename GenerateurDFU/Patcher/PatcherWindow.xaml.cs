using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace JAY.Patcher
{
    /// <summary>
    /// Logique d'interaction pour PatcherWindow.xaml
    /// </summary>
    public partial class PatcherWindow : Window
    {
        public PatcherWindow()
        {
            InitializeComponent();
            // Enregistrer la récupération des messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }
        
        /// <summary>
        /// Traiter la réception des messages
        /// </summary>
        public void ReceiveMessage ( CommandMessage message )
        {
            if (message.Sender is ViewModelPatcherWindow)
            {
                if (message.Command == JAY.PegaseCore.Commands.CMD_CLOSE_WINDOW)
                {
                    // Fermer la fenêtre proprement
                    Messenger.Default.Unregister(this);
                    this.Close();
                }
            }   
        } // endMethod: ReceiveMessage

        protected override void OnClosed(EventArgs e)
        {
            Messenger.Default.Unregister(this);
            base.OnClosed(e);
        }
    }
}
