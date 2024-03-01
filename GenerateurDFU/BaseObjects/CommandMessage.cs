using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace JAY
{
    /// <summary>
    /// Classe définissant un message de commande paramétrable
    /// </summary>
    public class CommandMessage : MessageBase
    {
        /// <summary>
        /// La commande véhiculée par le message
        /// </summary>
        public String Command
        {
            get;
            set;
        } // endProperty: Command

        // Constructeur
        public CommandMessage(object sender, String Command)
        {
            this.Sender = sender;
            this.Command = Command;
        }

        /// <summary>
        /// Set value to property target
        /// </summary>
        public void SetTarget ( object targetMessage )
        {
            this.Target = targetMessage;
        } // endMethod: SetTarget
    }
}
