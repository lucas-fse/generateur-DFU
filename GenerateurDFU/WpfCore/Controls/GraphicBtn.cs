using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JAY.WpfCore
{
    public class GraphicBtn : Button
    {

        #region ImageBkg
        /// <summary>
        /// The <see cref="ImageBkg" /> dependency property's name.
        /// </summary>
        public const string ImageBkgPropertyName = "ImageBkg";

        /// <summary>
        /// Gets or sets the value of the <see cref="ImageBkg" />
        /// property. This is a dependency property.
        /// </summary>
        public BitmapSource ImageBkg
        {
            get { return (BitmapSource)GetValue(ImageBkgProperty); }
            set { SetValue(ImageBkgProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ImageBkg" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageBkgProperty = DependencyProperty.Register
        (
            ImageBkgPropertyName,
            typeof(BitmapSource),
            typeof(GraphicBtn)
        ); 
        #endregion

        public GraphicBtn()
        {
            ToolTipService.SetShowDuration(this, 15000);
        }

    }
}
