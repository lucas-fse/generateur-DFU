using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class ACCIDENT
    {
        public string DateString
        {
            get
            {
                return Date.ToString("dd/MM/yyyy");
            }
        }
        public string TypeString
        {
            get
            {
                switch(Type)
                {
                    case 1:
                        return "Actes dangeureux";
                        break;
                    case 2:
                        return "Situations dangereuses";
                        break;
                    case 3:
                        return "Presque accident";
                        break;
                    case 4:
                        return "Soins benins";
                        break;
                    case 5:
                        return "Accidents déclarés";
                        break;
                    case 6:
                        return "Accident Grave";
                        break;
                    default:
                        return "Non définie";
                        break;
                }
            }
        }
        public string FullUrlImage
        {
            get
            {
                return @"ImageAccident\" + UrlImage;
                //return @"C:\Users\cogne\Desktop\tmp\" + UrlImage;
            }
        }
    }
}