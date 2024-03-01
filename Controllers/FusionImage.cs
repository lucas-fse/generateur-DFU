using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public static class FusionImage
    {

        public static Bitmap MergeTwoImages(Bitmap firstImage, Image secondImage,int ptx,int pty,string Refimage)
        {
            int outputImageWidth = 0;
            int outputImageHeight = 0;
            if ((firstImage == null) && (secondImage!= null))
            {
                 outputImageWidth = secondImage.Width;
                 outputImageHeight = secondImage.Height;
            }
            else if (firstImage == null)
            {
                return null;
            }
            else
            {
                outputImageWidth = firstImage.Width;
                outputImageHeight = firstImage.Height;
            }


            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                if (firstImage != null)
                {
                    graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size), new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                }
                if (secondImage != null)
                {
                    // positionnement 
                    int xx = 0; int yy = 0;
                    switch(Refimage)
                    {
                        case "HG":
                            xx = ptx; yy = pty;
                        break;
                        case "BG":
                            xx = ptx; yy = outputImageHeight-pty;
                            break;
                        case "BD":
                            xx = outputImageWidth-ptx; yy = outputImageHeight-pty;
                            break;
                        case "HD":
                            xx = outputImageWidth-ptx; yy = pty;
                            break;
                        default:
                            xx = ptx; yy = pty;
                            break;
                    }
                    graphics.DrawImage(secondImage, new Rectangle(new Point(xx, yy), secondImage.Size), new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
                }
            }

            return outputImage;
        }
    }
}