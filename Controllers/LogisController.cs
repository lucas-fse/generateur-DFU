using GenerateurDFUSafir.DAL;

using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Util;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.WebPages;
using System.Web.Security;

namespace GenerateurDFUSafir.Controllers
{
    public class LogisController : ConnexController
    {
        public static bool Index()
        {
            LogisController Logis = new LogisController();
            return true; //View(Logis);
        }
    }
}