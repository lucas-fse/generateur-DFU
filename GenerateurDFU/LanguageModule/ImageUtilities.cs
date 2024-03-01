using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JAY
{
    /// <summary>
    /// ------------------------------------------------------------------------------
    /// La classe ImageUtilities présente des méthodes static utilitaires ainsi que des
    /// méthodes et propriétés utilisées pour manipuler des images
    /// ------------------------------------------------------------------------------
    ///                         Auteur     : Claude BRUNET
    ///                         Année      : 2010
    ///                         Entreprise : 
    /// </summary>
    public class ImageUtilities
    {
        /// <summary>
        /// Enregistrer l'image dans le flux indiqué
        /// </summary>
        public static void SaveImage ( Stream ImageStream, String FileName, BitmapSource bitmap )
        {
            String FileType;

            FileType = Path.GetExtension ( FileName );
            FileType = FileType.ToLower ( );

            switch ( FileType )
            {
                case ".png":
                    ImageUtilities.SavePNG ( ImageStream, bitmap );
                    break;
                case ".jpg":
                case ".jpeg":
                    ImageUtilities.SaveJPG ( ImageStream, bitmap );
                    break;
                case ".tif":
                case ".tiff":
                    ImageUtilities.SaveTIFF ( ImageStream, bitmap );
                    break;
                case ".bmp":
                    ImageUtilities.SaveBMP ( ImageStream, bitmap );
                    break;
                case ".gif":
                    ImageUtilities.SaveGIF ( ImageStream, bitmap );
                    break;
                default:
                    throw new Exception ( "Type d'images non pris en charge" );
            }
            ImageStream.Close();
        } // endMethod: SaveImage

        /// <summary>
        /// Enregistrer l'image au format PNG
        /// </summary>
        /// <param name="ImageStream">
        ///     Stream ImageStream -> le flux à utiliser pour enregistrer l'image
        /// </param>
        /// <param name="bitmap">
        ///     BitmapSource bitmap -> l'image à enregistrer
        /// </param>
        public static void SavePNG ( Stream ImageStream, BitmapSource bitmap )
        {
            PngBitmapEncoder PngBE = new PngBitmapEncoder ( );
            PngBE.Frames.Add ( ImageUtilities.CreateBitmapFrameFromBitmapSource (bitmap) );
            PngBE.Interlace = PngInterlaceOption.Default;
            PngBE.Save ( ImageStream );
        } // endMethod: SavePNG

        /// <summary>
        /// Enregistrer l'image au format JPG
        /// </summary>
        /// <param name="ImageStream">
        ///     Stream ImageStream -> le flux à utiliser pour enregistrer l'image
        /// </param>
        /// <param name="bitmap">
        ///     BitmapSource bitmap -> l'image à enregistrer
        /// </param>
        public static void SaveJPG ( Stream ImageStream, BitmapSource bitmap )
        {
            JpegBitmapEncoder JpegBE = new JpegBitmapEncoder ( );
            JpegBE.Frames.Add ( ImageUtilities.CreateBitmapFrameFromBitmapSource ( bitmap ) );
            JpegBE.QualityLevel = 100;
            JpegBE.Save ( ImageStream );
        } // endMethod: SaveJPG
        
        /// <summary>
        /// Enregistrer l'image au format TIFF
        /// </summary>
        /// /// <param name="ImageStream">
        ///     Stream ImageStream -> le flux à utiliser pour enregistrer l'image
        /// </param>
        /// <param name="bitmap">
        ///     BitmapSource bitmap -> l'image à enregistrer
        /// </param>
        public static void SaveTIFF ( Stream ImageStream, BitmapSource bitmap )
        {
            TiffBitmapEncoder TiffBE = new TiffBitmapEncoder ( );
            TiffBE.Frames.Add ( ImageUtilities.CreateBitmapFrameFromBitmapSource ( bitmap ) );
            TiffBE.Compression = TiffCompressOption.Default;
            TiffBE.Save ( ImageStream );
        } // endMethod: SaveTIFF

        /// <summary>
        /// Enregistrer l'image au format BMP
        /// </summary>
        /// /// <param name="ImageStream">
        ///     Stream ImageStream -> le flux à utiliser pour enregistrer l'image
        /// </param>
        /// <param name="bitmap">
        ///     BitmapSource bitmap -> l'image à enregistrer
        /// </param>
        public static void SaveBMP ( Stream ImageStream, BitmapSource bitmap )
        {
            BmpBitmapEncoder BmpBE = new BmpBitmapEncoder ( );
            BmpBE.Frames.Add ( ImageUtilities.CreateBitmapFrameFromBitmapSource ( bitmap ) );
            BmpBE.Save ( ImageStream );
        } // endMethod: SaveBMP

        /// <summary>
        /// Enregistrer l'image au format GIF
        /// </summary>
        /// /// <param name="ImageStream">
        ///     Stream ImageStream -> le flux à utiliser pour enregistrer l'image
        /// </param>
        /// <param name="bitmap">
        ///     BitmapSource bitmap -> l'image à enregistrer
        /// </param>
        public static void SaveGIF ( Stream ImageStream, BitmapSource bitmap )
        {
            GifBitmapEncoder GifBE = new GifBitmapEncoder ( );
            GifBE.Frames.Add ( ImageUtilities.CreateBitmapFrameFromBitmapSource ( bitmap ) );
            GifBE.Save ( ImageStream );
        } // endMethod: SaveGIF
        
        /// <summary>
        /// Calculer la largeur de numérisation d'une image
        /// </summary>
        public static Int32 RawStride ( BitmapSource Image )
        {
            Int32 Result;
            PixelFormat pf;

            pf = Image.Format;
            Result = ( Image.PixelWidth * pf.BitsPerPixel + 7 ) / 8;
            
            return Result;
        } // endMethod: RawStride

        /// <summary>
        /// Calculer la largeur de numérisation d'une image
        /// </summary>
        public static Int32 RawStride ( PixelFormat pf, Int32 Width )
        {
            Int32 Result;

            Result = ( Width * pf.BitsPerPixel + 7 ) / 8;

            return Result;
        } // endMethod: RawStride

        /// <summary>
        /// Convertir un BitmapSource en BitmapFrame
        /// </summary>
        private static BitmapFrame CreateBitmapFrameFromBitmapSource ( BitmapSource source )
        {
            BitmapFrame Result;
            //Int32 Width;
            //Int32 Height;
            //Int32 RawStride;
            //Byte[]rawImage;
            //PixelFormat pf;
            //BitmapPalette Palette = null;
            
            //// Recopier les données dans un BitmapFrame
            //Width = ( Int32 )source.PixelWidth;
            //Height = ( Int32 )source.PixelHeight;
            //pf = source.Format;
            //RawStride = ( Width * pf.BitsPerPixel + 7 ) / 8;
            //rawImage = new Byte[ RawStride * Height ];
            //source.CopyPixels ( rawImage, RawStride, 0 );

            //if ( pf.BitsPerPixel == PixelFormats.Indexed1.BitsPerPixel ||
            //     pf.BitsPerPixel == PixelFormats.Indexed2.BitsPerPixel ||
            //     pf.BitsPerPixel == PixelFormats.Indexed4.BitsPerPixel ||
            //     pf.BitsPerPixel == PixelFormats.Indexed8.BitsPerPixel )
            //{
            //    Palette = source.Palette;
            //}
            Result = BitmapFrame.Create ( source );
            return Result;
        } // endMethod: CreateBitmapFrameFromBitmapSource


        /// <summary>
        /// Charger l'image avec la bonne méthode en fonction du type d'images (png, gif, jpg, bmp, tiff)
        /// </summary>
        /// <param name="FileName">
        ///     String FileName -> Le nom du fichier image avec l'extension
        /// </param>
        /// <param name="ImageStream">
        ///     Stream ImageStream -> Le flux permettant de charger l'image
        /// </param>
        /// <returns>
        ///     BitmapSource -> L'image chargée
        /// </returns>
        public static BitmapSource LoadImage ( String FileName, Stream ImageStream )
        {
            BitmapSource Result;
            String Extension = Path.GetExtension ( FileName );
            Extension = Extension.ToLower ( );

            switch ( Extension )
            {
                case ".gif":
                    Result = ImageUtilities.LoadGif ( ImageStream );
                    break;
                case ".tiff":
                case ".tif":
                    Result = ImageUtilities.LoadTiff ( ImageStream );
                    break;
                case ".jpeg":
                case ".jpg":
                    Result = ImageUtilities.LoadJpg ( ImageStream );
                    break;
                case ".bmp":
                    Result = ImageUtilities.LoadBmp ( ImageStream );
                    break;
                case ".png":
                    Result = ImageUtilities.LoadPng ( ImageStream );
                    break;
                default:
                    Result = new BitmapImage();
                    break;
            }

            ImageStream.Close();
            return Result;
        } // endMethod: LoadImage

        /// <summary>
        /// Charger une image
        /// </summary>
        public static BitmapSource LoadImage ( String FileName )
        {
            BitmapSource Result;

            try
            {
                Result = ImageUtilities.LoadImage(FileName, File.OpenRead(FileName));//File.Open(FileName, FileMode.Open));
            }
            catch (IOException)
            {
                Result = null;
            }

            return Result;
        } // endMethod: LoadImage

        /// <summary>
        /// Crée un BitmapSource à partir d'un BitmapFrame obtenu après un décodage JPG, BMP, PNG...
        /// </summary>
        /// <param name="BmpFrame">
        /// Le BitmapFrame décodé, contenant les données du BitmapSource à créer
        /// </param>
        /// <returns>
        /// Le BitmapSource Créé à partir du BitmapFrame.
        /// </returns>
        private static BitmapSource CreateBitmapSourceFromBitmapFrame ( BitmapFrame BmpFrame )
        {
            BitmapSource BmpSource;
            Int32 Width;
            Int32 Height;
            Int32 RawStride;
            Byte[]rawImage;
            PixelFormat pf;
            BitmapPalette Palette = null;

            // Recopier les données dans un BitmapSource
            Width = ( Int32 )BmpFrame.PixelWidth;
            Height = ( Int32 )BmpFrame.PixelHeight;
            pf = BmpFrame.Format;
            RawStride = ( Width * pf.BitsPerPixel + 7 ) / 8;
            rawImage = new Byte[ RawStride * Height ];
            BmpFrame.CopyPixels ( rawImage, RawStride, 0 );

            if ( pf.BitsPerPixel == PixelFormats.Indexed1.BitsPerPixel ||
                 pf.BitsPerPixel == PixelFormats.Indexed2.BitsPerPixel ||
                 pf.BitsPerPixel == PixelFormats.Indexed4.BitsPerPixel ||
                 pf.BitsPerPixel == PixelFormats.Indexed8.BitsPerPixel )
            {
                Palette = BmpFrame.Palette;
            }
            BmpSource = BitmapSource.Create ( Width, Height, 96, 96, pf, Palette, rawImage, RawStride );

            return BmpSource;
        } // endMethod: CreateBitmapSourceFromBitmapFrame

        /// <summary>
        /// Crée un BitmapSource à partir d'un BitmapFrame obtenu après un décodage JPG, BMP, PNG...
        /// </summary>
        /// <param name="BmpFrame">
        /// Le BitmapFrame décodé, contenant les données du BitmapSource à créer
        /// </param>
        /// <returns>
        /// Le BitmapSource Créé à partir du BitmapFrame.
        /// </returns>
        public static BitmapSource CreateBitmapSourceFromRow(Byte[] Raw, Int32 Width, Int32 Height, PixelFormat pixelFormat)
        {
            BitmapSource BmpSource;
            Int32 RawStride;
            BitmapPalette Palette = null;

            // Recopier les données dans un BitmapSource
            RawStride = (Width * pixelFormat.BitsPerPixel + 7) / 8;

            if (pixelFormat.BitsPerPixel == PixelFormats.Indexed1.BitsPerPixel ||
                 pixelFormat.BitsPerPixel == PixelFormats.Indexed2.BitsPerPixel ||
                 pixelFormat.BitsPerPixel == PixelFormats.Indexed4.BitsPerPixel ||
                 pixelFormat.BitsPerPixel == PixelFormats.Indexed8.BitsPerPixel)
            {
                //Palette = BmpFrame.Palette;
            }
            BmpSource = BitmapSource.Create(Width, Height, 96, 96, pixelFormat, Palette, Raw, RawStride);

            return BmpSource;
        } // endMethod: CreateBitmapSourceFromRaw

        /// <summary>
        /// Charger un fichier png dans un BitmapImage
        /// </summary>
        /// <param name="BmpStream">
        /// Un flux vers le fichier contenant les données de l'image à charger
        /// </param>
        /// <returns>
        /// Un BitmapSource utilisable pour le rendu dans un control Image par exemple
        /// </returns>
        /// <remarks>
        /// Le BitmapSource retourné est indépendant du flux d'entré. De ce fait, la ressource
        /// du flux peut être libérée
        /// </remarks>
        public static BitmapSource LoadPng ( Stream PngStream )
        {
            PngBitmapDecoder PngDecoder;
            BitmapFrame BmpFrame;
            BitmapSource Result;

            // Décoder l'image
            PngDecoder = new PngBitmapDecoder ( PngStream, BitmapCreateOptions.None, BitmapCacheOption.None );
            BmpFrame = PngDecoder.Frames[ 0 ];

            Result = ImageUtilities.CreateBitmapSourceFromBitmapFrame ( BmpFrame );
            PngStream.Close();
            return Result;
        } // endMethod: LoadPng

        /// <summary>
        /// Charger un fichier jpg dans un BitmapImage
        /// </summary>
        /// <param name="BmpStream">
        /// Un flux vers le fichier contenant les données de l'image à charger
        /// </param>
        /// <returns>
        /// Un BitmapSource utilisable pour le rendu dans un control Image par exemple
        /// </returns>
        /// <remarks>
        /// Le BitmapSource retourné est indépendant du flux d'entré. De ce fait, la ressource
        /// du flux peut être libérée
        /// </remarks>
        public static BitmapSource LoadJpg ( Stream JpgStream )
        {
            JpegBitmapDecoder JpgDecoder;
            BitmapFrame BmpFrame;
            BitmapSource Result;

            JpgDecoder = new JpegBitmapDecoder ( JpgStream, BitmapCreateOptions.None, BitmapCacheOption.None );
            BmpFrame = JpgDecoder.Frames[ 0 ];

            Result = ImageUtilities.CreateBitmapSourceFromBitmapFrame ( BmpFrame );
            JpgStream.Close();
            return Result;
        } // endMethod: LoadJpg

        /// <summary>
        /// Charger un fichier bmp dans un BitmapImage
        /// </summary>
        /// <param name="BmpStream">
        /// Un flux vers le fichier contenant les données de l'image à charger
        /// </param>
        /// <returns>
        /// Un BitmapSource utilisable pour le rendu dans un control Image par exemple
        /// </returns>
        /// <remarks>
        /// Le BitmapSource retourné est indépendant du flux d'entré. De ce fait, la ressource
        /// du flux peut être libérée
        /// </remarks>
        public static BitmapSource LoadBmp ( Stream BmpStream )
        {
            BmpBitmapDecoder BmpDecoder;
            BitmapFrame BmpFrame;
            BitmapSource Result;

            BmpDecoder = new BmpBitmapDecoder ( BmpStream, BitmapCreateOptions.None, BitmapCacheOption.None );
            BmpFrame = BmpDecoder.Frames[ 0 ];

            Result = ImageUtilities.CreateBitmapSourceFromBitmapFrame ( BmpFrame );
            BmpStream.Close();
            return Result;
        } // endMethod: LoadBmp

        /// <summary>
        /// Charger un fichier gif dans un BitmapImage
        /// </summary>
        /// <param name="BmpStream">
        /// Un flux vers le fichier contenant les données de l'image à charger
        /// </param>
        /// <returns>
        /// Un BitmapSource utilisable pour le rendu dans un control Image par exemple
        /// </returns>
        /// <remarks>
        /// Le BitmapSource retourné est indépendant du flux d'entré. De ce fait, la ressource
        /// du flux peut être libérée
        /// </remarks>
        public static BitmapSource LoadGif ( Stream GifStream )
        {
            GifBitmapDecoder GifDecoder;
            BitmapFrame BmpFrame;
            BitmapSource Result;

            GifDecoder = new GifBitmapDecoder ( GifStream, BitmapCreateOptions.None, BitmapCacheOption.None );
            BmpFrame = GifDecoder.Frames[ 0 ];
            Result = ImageUtilities.CreateBitmapSourceFromBitmapFrame ( BmpFrame );
            GifStream.Close();

            return Result;
        } // endMethod: LoadGif

        /// <summary>
        /// Charger un fichier tiff dans un BitmapImage
        /// </summary>
        /// <param name="BmpStream">
        /// Un flux vers le fichier contenant les données de l'image à charger
        /// </param>
        /// <returns>
        /// Un BitmapSource utilisable pour le rendu dans un control Image par exemple
        /// </returns>
        /// <remarks>
        /// Le BitmapSource retourné est indépendant du flux d'entré. De ce fait, la ressource
        /// du flux peut être libérée
        /// </remarks>
        public static BitmapSource LoadTiff ( Stream TifStream )
        {
            TiffBitmapDecoder TiffDecoder;
            BitmapFrame BmpFrame;
            BitmapSource Result;

            TiffDecoder = new TiffBitmapDecoder ( TifStream, BitmapCreateOptions.None, BitmapCacheOption.None );
            BmpFrame = TiffDecoder.Frames[ 0 ];
            Result = ImageUtilities.CreateBitmapSourceFromBitmapFrame ( BmpFrame );
            TifStream.Close();

            return Result;
        } // endMethod: LoadTiff
    }
}
