using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class AMELIORATION
    {
        public string SujetString
        {
            get
            {
                return StringSujet(Type);
            }
        }
        public string StatusString
        {
            get
            {
                return StringStatus(Status);
            }
        }
        public static string StringSujet(int type)
        {
            switch (type)
            {
                case 1:
                    return "5S";
                    break;
                case 2:
                    return "AMELIORATION CONTINUE";
                    break;
                case 4:
                    return "ENVIRONNEMENT";
                    break;
                case 3:
                    return "ERGONOMIE";
                    break;
                case 5:
                    return "MAINTENANCE";
                    break;
                case 6:
                    return "QUALITE";
                    break;
                case 7:
                    return "SECURITE";
                    break;
                case 8:
                    return "";
                    break;
                default:
                    return "invalide";
                    break;

            }
        }
        public static string StringStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return "REALISEE";
                    break;
                case 2:
                    return "EN ATTENTE";
                    break;
                case 4:
                    return "EN COURS";
                    break;
                case 3:
                    return "REFUSEE";
                    break;
                default:
                    return "inconnu";
                    break;
            }
            return "";
        }
        public static short SujetbyString(string type)
        {
            switch (type.Trim().ToUpper())
            {
                case "5S":
                    return 1;
                    break;
                case "AMELIORATION CONTINUE":
                    return 2;
                    break;
                case "ENVIRONNEMENT":
                    return 4;
                    break;
                case "ERGONOMIE":
                    return 3;
                    break;
                case "MAINTENANCE":
                    return 5;
                    break;
                case "QUALITE":
                    return 6;
                    break;
                case "SECURITE":
                    return 7;
                    break;
                case "":
                    return -1;
                    break;

                default:
                    return 0;
                    break;

            }
        }
        public string FullUrlImage
        {
            get
            {
                if (UrlImage == null)
                {
                    return "";
                }
                else
                {
                    return UrlImage;
                }
            }
        }
    }
}