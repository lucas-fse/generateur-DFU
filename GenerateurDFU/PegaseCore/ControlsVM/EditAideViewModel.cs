using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Form = System.Windows.Forms;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Xml;
using System.Xml.Linq;
using JAY.PegaseCore;
using JAY.XMLCore;
using JAY.FileCore;
using System.Configuration;
using System.Collections.Specialized;


namespace JAY.PegaseCore.ControlsVM
{
    class EditAideViewModel : Mvvm.ViewModelBase
    {
        public Style Style1
        {
            get { return (Style)Application.Current.Resources[EditMasquesViewModel.DEFAULT_STYLE_GREY]; }
        }
        public Style Style2
        {
            get { return (Style)Application.Current.Resources[EditMasquesViewModel.CHECK_STYLE]; }
        }
        public Style Style3
        {
            get { return (Style)Application.Current.Resources[EditMasquesViewModel.CHECK_STYLEPLUS]; }
        }

        public string PictoVerr
        {
            get
            {
                return LanguageSupport.Get().GetText("EDIT_VARIABLE/PICTOVERR");
            }
        }
        public string PictoCoche
        {
            get
            {
                return LanguageSupport.Get().GetText("EDIT_VARIABLE/PICTOCOCHE");
            }
        }
        public string PictoCochePlus
        {
            get
            {
                return LanguageSupport.Get().GetText("EDIT_VARIABLE/PICTOCOCHEPLUS");
            }
        }
        public string PictoVide
        {
            get
            {
                return LanguageSupport.Get().GetText("EDIT_VARIABLE/PICTOVIDE");
            }
        }
    }
}
