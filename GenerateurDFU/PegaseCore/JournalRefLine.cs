using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Une ligne de donnée pour le journal de référence
    /// </summary>
    public class JournalRefLine : Mvvm.ViewModelBase
    {
        /// <summary>
        /// Le code de l'erreur
        /// </summary>
        public String CodeErreur
        {
            get;
            set;
        } // endProperty: CodeErreur

        /// <summary>
        /// Le code d'erreur le plus petit concernant cette erreur
        /// </summary>
        public Int32 StartErrorNum
        {
            get;
            set;
        } // endProperty: StartErrorNum

        /// <summary>
        /// Le code d'erreur le plus élevé relatif à cette erreur
        /// </summary>
        public Int32 EndErrorCode
        {
            get;
            set;
        } // endProperty: EndErrorCode

        /// <summary>
        /// Le texte de l'erreur
        /// </summary>
        public String ErreurText
        {
            get;
            set;
        } // endProperty: ErreurText

        // Constructeur

        public JournalRefLine(XElement erreurRefLine)
        {
            this.CodeErreur = erreurRefLine.Attribute("code").Value;
            this.StartErrorNum = Convert.ToInt32(erreurRefLine.Attribute("Start").Value);
            this.EndErrorCode = Convert.ToInt32(erreurRefLine.Attribute("End").Value);
        }

        // Méthodes
        
        /// <summary>
        /// initialise le texte de l'erreur avec le texte de la langue en cours d'utilisation
        /// </summary>
        public void Translate ( )
        {
            this.ErreurText = LanguageSupport.Get().GetToolTip(String.Format("/ErEmb/{0}", this.CodeErreur));
        } // endMethod: Translate
    }
}
