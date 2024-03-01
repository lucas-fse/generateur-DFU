using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using JAY.PegaseCore;

namespace JAY.PegaseCore.Converter
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
    public class StringToBitmapSourceConverter : IValueConverter
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
            String Value;
            BitmapSource Result = null;
            Boolean IsInverted = false;

            // récuperer la valeur
            if ( value is String )
            {
                Value = (String)value;
            }
            else
            {
                Value = "";
            }

            // Ouvrir la bitmap
            if (Value != "")
            {
                Uri ImageUri = new Uri(Value, UriKind.Relative);
                if (!PegaseData.Instance.CurrentPackage.IsOpen)
                {
                    PegaseData.Instance.CurrentPackage.OpenPackage();
                }
                Stream BitmapStream = PegaseData.Instance.CurrentPackage.GetPartStream(ImageUri);
                if (BitmapStream == null && File.Exists(Value))
                {
                    BitmapStream = File.OpenRead(Value);
                }

                String Extension = System.IO.Path.GetExtension(Value).ToLower();
                try
                {
                    switch (Extension)
                    {
                        case ".jpg":
                            Result = FileCore.BitmapTools.OpenBitmapJPG(BitmapStream);
                            break;
                        case ".bmp":
                            Result = FileCore.BitmapTools.OpenBitmapBmp(BitmapStream);
                            break;
                        case ".png":
                            Result = FileCore.BitmapTools.OpenBitmapPng(BitmapStream);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    Result = null;
                }

                if (parameter!= null && ((String)parameter).ToLower() == "true")
                {
                    IsInverted = true;
                }
                // Dupliquer l'image en homogénéisant le nombre de dpi
                if (Result != null)
                {
                    Int32 Stride, size;

                    switch (Result.Format.BitsPerPixel)
                    {
                        case 1:
                            Stride = (Result.PixelWidth * ((Result.Format.BitsPerPixel + 7) / 8) + 7) / 8;
                            break;
                        case 8:
                        case 24:
                        case 32:
                            Stride = (Result.PixelWidth * Result.Format.BitsPerPixel + 7) / 8;
                            break;
                        default:
                            Stride = 0;
                            break;
                    }
                    size = Stride * Result.PixelHeight;

                    Byte[] Pixels = new Byte[size];

                    Result.CopyPixels(Pixels, Stride, 0);
                    if (IsInverted)
                    {
                        for (int i = 0; i < Pixels.Length; i++)
                        {
                            Pixels[i] = Pixels[i] ^= 0xFF;
                        }
                    }
                    Result = BitmapImage.Create(Result.PixelWidth, Result.PixelHeight, 48.0, 48.0, Result.Format, Result.Palette, Pixels, Stride); 
                }
                PegaseData.Instance.CurrentPackage.ClosePackage();
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
