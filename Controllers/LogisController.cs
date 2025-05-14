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
using System.Web.UI.WebControls;

namespace GenerateurDFUSafir.Controllers
{
    public class LogisController : Controller
    {
        public static bool CaptureOCR()
        {
            InfoAmelioration vue = new InfoAmelioration();
            return View(vue);
        }
    }
}