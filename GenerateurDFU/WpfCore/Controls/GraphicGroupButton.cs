using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using mvvm = GalaSoft.MvvmLight;

namespace JAY.WpfCore
{
    /// <summary>
    /// Un classe dérivée de la classe ToggleButton envoyant un message lorsque 
    /// le bouton est cliqué
    /// </summary>
    public class GraphicGroupButton : ToggleButton
    {
        public GraphicGroupButton():base()
        {
            this.Click += new System.Windows.RoutedEventHandler(GraphicGroupButton_Click);
            ToolTipService.SetShowDuration(this, 15000);
        }

        void GraphicGroupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ButtonClick();
        }

        /// <summary>
        /// Envoyer un message spécifiant que le bouton a été cliqué
        /// </summary>
        public void ButtonClick ( )
        {
            mvvm.Messaging.Messenger.Default.Send<JAY.CommandMessage>(new CommandMessage(this, "GMClick"));
        } // endMethod: ButtonClick
    }
}
