using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models.DAL
{

    public partial class OF_PROD_TRAITE
    {

        public string Description
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(MFGDES))
                {
                    return ITEMREF + "-" + MFGDES;
                }
                else
                {
                    return ITEMREF;
                }
            }
        }
        public Nullable<System.DateTime> ENDTIMETHEORIQUE { get; set; }

        //[DisplayFormat(NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public string StartAt
        {
            get
            {
                string format = "dd-MM H:mm ";
                string tmp = "";
                try
                {
                    tmp = ((DateTime)this.STARTTIME).ToString(format, DateTimeFormatInfo.InvariantInfo);
                }
                catch {tmp = "Invalid";  }
                return tmp;
            }
        }
        public string PauseReprise
        {
            get
            {
                try
                {
                    if (STATUSTYPE.Contains("ENPAUSE"))
                    {
                        return "Reprise";
                    }
                    else if (STATUSTYPE.Contains("CLOSED"))
                    {
                        return "Reprise";
                    }
                    else
                    {
                        return "Pause";
                    }
                }
                catch
                {
                    return "null";
                }
            }
        }
        public string Arret
        {
            get
            {
                try
                {
                    if (STATUSTYPE.Contains("CLOSED"))
                    {
                        return "     ";
                    }
                    else
                    {
                        return "Arrêt";
                    }
                }
                catch { return "Arrêt"; }
            }
        }
        // valeur de retour
        public int TempsId { get; set; }
        //valeur init
        public int DefaultTempsId { get;set;}
        public IEnumerable<SelectListItem>  ListTempsSuppl 
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = "0 mn", Value="0"},
                    new SelectListItem {Text = "5 mn", Value="1"},
                    new SelectListItem {Text = "10 mn", Value="2"},
                    new SelectListItem {Text = "15 mn", Value="3"},
                    new SelectListItem {Text = "20 mn", Value="4"},
                    new SelectListItem {Text = "25 mn", Value="5"},
                    new SelectListItem {Text = "30 mn", Value="6"},
                    new SelectListItem {Text = "40 mn", Value="7"},
                    new SelectListItem {Text = "50 mn", Value="8"},
                    new SelectListItem {Text = "1 h", Value="9"},
                    new SelectListItem {Text = "1 h 30 mn", Value="10"},
                    new SelectListItem {Text = "2 h", Value="11"}
                };
            }
        }
        public string Readonly 
        { 
            get
            {
                if (STATUSTYPE.Contains("CLOSED"))
                {
                    return "readonly";
                }
                else
                {
                    return "";
                }
            } 
        }
        // list des alea de l'of avec les descriptions
        public List<CodeGroupe> ListAlea
        {
            get
            {
                List<CodeGroupe> list = new List<CodeGroupe>();
                DeclarationAlea aleas = new DeclarationAlea();
                aleas.GestionCodeOF();
                foreach (var alea in ALEAS_OF)
                {
                    if (alea.NMR_ALEA != null)
                    {
                        try
                        {
                            list.Add((CodeGroupe)aleas.CodeAlea.Where(p => p.Code == Int32.Parse(alea.NMR_ALEA.ToString())).First());
                        }
                        catch
                        {

                        }
                    }
                }
                return list;
            }
        }
        public string EmplacementItem { get; set; }
    }
}