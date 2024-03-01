using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace JAY.FileCore
{
    /// <summary>
    /// Class d'aide au chargement / enregistrement des images
    /// </summary>
    public class BitmapTools
    {
        #region Methodes static de chargement
        
        /// <summary>
        /// Ouvrir une image depuis un format jpeg
        /// </summary>
        public static BitmapSource OpenBitmapJPG ( Stream BitmapStream )
        {
            BitmapSource Result = null;

            if (BitmapStream != null)
            {
                JpegBitmapDecoder JpgBitmap = new JpegBitmapDecoder(BitmapStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Result = JpgBitmap.Frames[0]; 
            }

            return Result;
        } // endMethod: OpenBitmapJPG        

        /// <summary>
        /// Ouvrir une image depuis un format png
        /// </summary>
        public static BitmapSource OpenBitmapPng ( Stream BitmapStream )
        {
            BitmapSource Result = null;

            if (BitmapStream != null)
            {
                PngBitmapDecoder PngBitmap = new PngBitmapDecoder(BitmapStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                
                Result = PngBitmap.Frames[0];
            }

            return Result;
        } // endMethod: OpenBitmapPng
        
        /// <summary>
        /// Ouvrir une image depuis un format bmp
        /// </summary>
        public static BitmapSource OpenBitmapBmp ( Stream BitmapStream )
        {
            BitmapSource Result = null;

            if (BitmapStream != null)
            {
                BmpBitmapDecoder BmpBitmap = new BmpBitmapDecoder(BitmapStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Result = BmpBitmap.Frames[0]; 
            }

            return Result;
        } // endMethod: OpenBitmapBmp

        /// <summary>
        /// Convertir un flowdocument en bitmap
        /// </summary>
        /// <param name="document">
        /// Le document à convertir
        /// </param>
        /// <param name="size">
        /// la taille du document
        /// </param>
        /// <returns>
        /// La bitmap intégrant l'impression du FlowDocument
        /// </returns>
        public static BitmapSource FlowDocumentToBitmap(FlowDocument document, Size size)
        {
            FlowDocument flowDoc = CloneDocument(document);

            DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDoc).DocumentPaginator;
            paginator.PageSize = size;

            ContainerVisual visual = new ContainerVisual();
            //using (var drawingContext = visual.RenderOpen())
            //{
            //    // draw white background
            //    drawingContext.DrawRectangle(Brushes.White, null, new Rect(size));
            //    //drawingContext.DrawEllipse(Brushes.Red, new Pen(Brushes.Red, 2), new Point(960, 540), 960, 540);
            //}
            visual.Children.Add(paginator.GetPage(0).Visual);

            var bitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);

            return bitmap;
        }

        /// <summary>
        /// Cloner un FlowDocument
        /// </summary>
        /// <param name="document">
        /// Le document source à cloner
        /// </param>
        /// <returns>
        /// Le clone établi
        /// </returns>
        public static FlowDocument CloneDocument(FlowDocument document)
        {
            var copy = new FlowDocument();
            var sourceRange = new TextRange(document.ContentStart, document.ContentEnd);
            var targetRange = new TextRange(copy.ContentStart, copy.ContentEnd);

            using (var stream = new MemoryStream())
            {
                sourceRange.Save(stream, DataFormats.XamlPackage);
                targetRange.Load(stream, DataFormats.XamlPackage);
            }

            return copy;
        }

        /// <summary>
        /// Enregistrer une image jpg
        /// </summary>
        public static void SaveJPG ( String Filename, BitmapSource bitmap )
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new FileStream(Filename, FileMode.Create))
            {
                encoder.Save(stream);
            }   
        } // endMethod: SaveJPG

        #endregion
    }
}
