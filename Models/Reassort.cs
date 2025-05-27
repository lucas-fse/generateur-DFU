using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class Reassort
    {
        public int AddReassort(DMD_REASSORT dmdReassort)
        {
            int result = 0;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                DMD_REASSORT dr = new DMD_REASSORT();
                dr.date = dmdReassort.date;
                dr.idPiece = dmdReassort.idPiece;
                dr.operateur = dmdReassort.operateur;
                dr.traiteLe = null;
                dr.description = dmdReassort.description;
                dr.quantite = dmdReassort.quantite;
                dr.dispo = null;
                _db.DMD_REASSORT.Add(dr);
                _db.SaveChanges();
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