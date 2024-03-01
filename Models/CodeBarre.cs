using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using DataMatrix;
using DataMatrix.net;

namespace GenerateurDFUSafir.Models
{
    public static class CodeBarre
    {
        public static Image DataMatrixCxw(string item,int Qtr,string lot)
        {
            DmtxImageEncoder encoder = new DmtxImageEncoder();
            Bitmap bmp  = encoder.EncodeImage("RQL;"+item+";"+Qtr.ToString()+";"+lot);

            return bmp;
        }
    }
}