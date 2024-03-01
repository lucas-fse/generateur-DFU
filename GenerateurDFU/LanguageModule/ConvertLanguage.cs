using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace JAY
{
    /// <summary>
    /// ------------------------------------------------------------------------------
    /// La classe ConverterBase fournit un convertisseur pour le DataBinding
    /// implémentant l'interface IValueConverter
    /// ------------------------------------------------------------------------------
    ///                         Auteur     : Claude BRUNET
    ///                         Année      : 2010
    ///                         Entreprise :
    /// </summary>
    /// <remarks>
    /// Le convertisseur attend le code du champ et en paramètre le type TEXT ou TOOLTIPS
    /// </remarks>
    public class ConvertLanguage : IValueConverter
    {

        #region IValueConverter Membres

        /// <summary>
        /// La méthode Convert permet de convertir une donnée depuis le ViewModel
        /// vers l'affichage
        /// par exemple, une heure peut être converti en degré pour afficher une aiguille,
        /// un nombre / texte peut être converti en couleur...
        /// </summary>
        /// <param name="value">
        ///     object value -> un objet contenant la valeur initial reçue depuis le viewmodel
        /// </param>
        /// <param name="targetType">
        ///     Type targetType -> un type définissant le type de valeur attendu après conversion
        /// </param>
        /// <param name="parameter">
        ///     object parameter -> des paramètres pouvant être transmis lors du databinding (sous Blend 4)
        /// </param>
        /// <param name="culture">
        ///     object culture -> la culture d'utilisation du logiciel (français, anglais...)
        /// </param>
        /// <returns>
        ///     object -> la valeur convertie
        /// </returns>
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            String XMLPath = "";
            String Param = "";
            String Result = null;

            if ( value is String )
            {
                XMLPath = value as String;
                XMLPath = XMLPath.ToUpper();
            }
            else
            {
                return "ITEM";
            }

            if (parameter is String)
            {
                Param = parameter as String;

                switch (Param)
                {
                    case "TEXT":
                        Result = LanguageSupport.Get().GetText(XMLPath);
                        break;
                    case "TOOLTIPS":
                    case "TOOLTIP":
                        Result = LanguageSupport.Get().GetToolTip(XMLPath);
                        break;
                    default:
                        Result = "Invalid Param";
                        break;
                }
            }

            if (Result == null || Result == "")
            {
                Result = XMLPath;
            }

            return Result;
        }

        /// <summary>
        /// La méthode ConvertBack permet de convertir une valeur depuis l'interface vers le ViewModel
        /// Par exemple, le degré de rotation d'une aiguille peut être converti en heure, une couleur en texte / nombre ...
        /// </summary>
        /// <param name="value">
        ///     object value -> un objet contenant la valeur initial reçue depuis la View
        /// </param>
        /// <param name="targetType">
        ///     Type targetType -> un type définissant le type de valeur attendu après conversion
        /// </param>
        /// <param name="parameter">
        ///     object parameter -> des paramètres pouvant être transmis lors du databinding (sous Blend 4)
        /// </param>
        /// <param name="culture">
        ///     object culture -> la culture d'utilisation du logiciel (français, anglais...)
        /// </param>
        /// <returns>
        ///     object -> la valeur convertie
        /// </returns>
        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException ( );
        }

        #endregion
    }
}
