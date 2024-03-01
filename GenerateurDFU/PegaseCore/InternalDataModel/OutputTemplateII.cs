using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;
using System.Xml;

using System.IO;
using System.Text;

using JAY.XMLCore;
using JAY.FileCore;

using JAY.PegaseCore.Helper;

namespace JAY.PegaseCore
{
    public class OutputTemplateII : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private Boolean _isUsed = false;
        private Boolean _isEnable;

        #endregion

        //constructeur
        /// <summary>
        /// La sortie est-elle disponible dans ce contexte ?
        /// </summary>
        public Boolean IsEnable
        {
            get
            {
                return this._isEnable;
            }
            set
            {
                this._isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        } // endProperty: IsEnable
        //constructeur
        /// <summary>
        /// La sortie est-elle disponible dans ce contexte ?
        /// </summary>
        public Boolean IsUsed
        {
            get
            {
                return this._isUsed;
            }
            set
            {
                this._isUsed = value;
                RaisePropertyChanged("IsUsed");
            }
        } // endProperty: IsEnable

    }
}
