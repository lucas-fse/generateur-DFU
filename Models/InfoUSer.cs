﻿using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class InfoUSer
    {
        
        public List<String> ListUser { get; set; }
        public InfoUSer(string NmrOf)
        {
            ListUser = new List<string>();
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            var query = db.OF_PROD_TRAITE.Where(p => p.NMROF.Contains(NmrOf)).OrderByDescending(p=> p.ENDTIME).Select(p => p.OPERATEUR);
            if (query != null && query.Count() > 0)
            {
                // on prend le dernier operateur ayant touché a l'OF
                List<long?> tmpuser = new List<long?>();
                tmpuser.Add(query.First());
                var query2 = db.OPERATEURS.Where(i => tmpuser.Contains(i.ID));
                foreach (var u in query2.ToList())
                {
                    ListUser.Add(u.NOM + " " + u.PRENOM);
                }
            }
        }
        
    }
}