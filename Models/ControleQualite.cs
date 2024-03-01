using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace GenerateurDFUSafir.Models
{
    public class ControleQualite
    {
        public CONTROLE_QUALITE CONTROLE_QUALITEEnCours { get; set; }
        public List<CONTROLE_QUALITE> Listecontroles { get; set; }
        public List<SelectListItem> ListAnomalie { get; set; }
        public List<SelectListItem> ListCause { get; set; }
        public long? IDCQ { get; set; }
        public List<string> toot { get; set; }

        public ControleQualite(long? iDCQ = null, int? type = 0)
        {
            // Vous pouvez initialiser des propriétés ou effectuer d'autres opérations ici si nécessaire
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            DateTime Now10 = DateTime.Now.AddDays(-10);
            if (type == null || type == 0)
            {
                Listecontroles = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Include("TYPE_ANOMALIE").Include("TYPE_CAUSE").Where(a => a.Conforme == 0 && a.Date> Now10).OrderByDescending(p => p.Date).ToList();
            }
            else
            {
                Listecontroles = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Include("TYPE_ANOMALIE").Include("TYPE_CAUSE").Where(a => a.Conforme == 1 && a.Date > Now10).OrderByDescending(p => p.Date).ToList();

            }
            List<TYPE_ANOMALIE> listanomalie = pEGASE_PROD2Entities2.TYPE_ANOMALIE.ToList();
            List<TYPE_CAUSE> listcause = pEGASE_PROD2Entities2.TYPE_CAUSE.ToList();
            ListAnomalie = new List<SelectListItem>();
            foreach (var i in listanomalie)
            {
                ListAnomalie.Add(new SelectListItem {Text = i.Valeur,Value= i.ID.ToString() });
            }
            ListCause = new List<SelectListItem>();
            foreach (var i in listcause)
            {
                ListCause.Add(new SelectListItem { Text = i.Valeur, Value = i.ID.ToString() });
            }
            CONTROLE_QUALITEEnCours = new CONTROLE_QUALITE();
            if (iDCQ != null)
            {
                IDCQ = iDCQ;
                var query = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Include("TYPE_ANOMALIE").Include("TYPE_CAUSE").Where(p => p.ID == (long)iDCQ);
                if (query != null && query.Count() > 0)
                {
                    CONTROLE_QUALITEEnCours = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Include("TYPE_ANOMALIE").Include("TYPE_CAUSE").Where(p => p.ID == (long)iDCQ).First();
                }
            }
        }

        public long ID { get; internal set; }

        public int AddControle(long? ID,long? idqc, string nmrof, string itemref,string itemdescipt, int? conforme,string anomalie,string cause, string description, string ImageDB, ref string pathimage)
         {
            int result = -1;
            try
            {
                if (itemref == null) { itemref = ""; }
                if (description == null) { description = ""; }
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
                int? anomalieInt = null;
                int? causeInt = null;
                try { anomalieInt = Convert.ToInt32(anomalie); } catch { }
                try { causeInt = Convert.ToInt32(cause); } catch { }

                string nameImg = pathimage;
                string namePath = "";

                if (ImageDB != null)
                {
                    // Convertir l'image en byte[]
                    byte[] byteImg = Convert.FromBase64String(ImageDB);
                    // Enregistrer l'image sur le serveur
                    nameImg = "QUAL_" + DateTime.Now.Ticks.ToString() + ".jpg";
                    namePath = @"C:\inetpub\wwwroot\GenerateurDFUSafir\ImageQualite\" + nameImg;
                    pathimage = namePath;
                    using (var bitmapImg = new Bitmap(new MemoryStream(byteImg)))
                    {
                        bitmapImg.Save(namePath, ImageFormat.Jpeg);
                    }
                }
                CONTROLE_QUALITE cq = new CONTROLE_QUALITE();
                var query = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Where(i => i.ID == idqc);
                if (query!=null && query.Count()== 1 )
                {
                    cq = pEGASE_PROD2Entities2.CONTROLE_QUALITE.Where(i => i.ID == idqc).First();
                    cq.DateModif = DateTime.Now;
                }
                else
                {
                    pEGASE_PROD2Entities2.CONTROLE_QUALITE.Add(cq);
                    cq.Date = DateTime.Now;
                }
                
                cq.ID_OPERATEUR = ID;
                cq.NMROF = nmrof;
                cq.ITEMREF = itemref;
                cq.ITEMDESCRIPT = itemdescipt;
                cq.Conforme = conforme;
                cq.ID_TYPE_ANOMALIE = anomalieInt;
                cq.ID_TYPE_CAUSE = causeInt;
                cq.Description = description.Replace("\"", "'");
                if (string.IsNullOrWhiteSpace(cq.UrlImage.Trim()) || !nameImg.Contains(cq.UrlImage.Trim()))
                {
                    cq.UrlImage = nameImg;
                }
                
                
                pEGASE_PROD2Entities2.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = -1;
            }
            return result;
        }
        
    }
}
