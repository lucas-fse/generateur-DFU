using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public static class GestionArticlePNC
    {
        /// <summary>
        /// recupere les article lié a un masque données
        /// </summary>
        /// <param name="article a trouve dans la base"></param>
        /// <returns></returns>
        public static List<ArticleSite> GetArticleSite(string article)
        {
            List<ArticleSite> result = new List<ArticleSite>();
            DataTable table1 = new DataTable();
            ModelOF1.RequeteArticleSite(article, ref table1);
            if (table1!= null && table1.Rows!= null)
            {
                foreach(DataRow row in table1.Rows)
                {
                    ArticleSite artsite = new ArticleSite();
                    artsite.Description = row["ITMDES1_0"].ToString();
                    artsite.Itemref = row["ITMREF_0"].ToString();
                    artsite.Localisation = row["Emplacement2"].ToString();
                    
                    result.Add(artsite);
                }
            }
            return result;
        }
    }
    public class ArticleSite
    {
        public string Itemref { get; set; }
        public string Description { get; set; }
        public string Localisation { get; set; }
    }

}