using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class ControleFinal
    {
        public string NumOf { get; set; }
        public string ItemRef { get; set; }
        public string ItemDescript { get; set; }
        //public List<OPERATEURS> OPERATEUR { get; set; }
    }
    public class ControleFinalOp
    { 
        public static List<ControleFinal> GetOFSearch(string of)
        {
            List<ControleFinal> result = new List<ControleFinal>();

            DataTable rawResult = new DataTable();
            ofProdIieCmd Ofs = new ofProdIieCmd();

            /*Faire une requete ici*/
            ModelOF1.RequeteControleFinal(of, ref rawResult);

            if (rawResult != null && rawResult.Rows != null)
            {
                foreach (DataRow row in rawResult.Rows)
                {
                    ControleFinal OFsite = new ControleFinal();
                    OFsite.NumOf = row["MFGNUM_0"].ToString();
                    OFsite.ItemRef = row["ITMREF_0"].ToString(); 
                        OFsite.ItemDescript = row["MFGDES_0"].ToString();
                    OfX3 ofx3 = new OfX3();
                    List<OF_PROD_TRAITE> oftraite = ofx3.ListOFsTraite(of);
                    var opes = oftraite.Where(i=>i.NMROF.Contains(of)).Select(p => (p.OPERATEUR)).GroupBy(o=>o).Select(y => y.Key).ToList();
                    
                    //OFsite.OPERATEUR = new List<OPERATEURS>();
                    //foreach (var op in opes)
                    //{
                    //   OPERATEURS opnew = ofx3.OperateurById(op);
                    //    if (opnew != null)
                    //    {
                    //        OFsite.OPERATEUR.Add(opnew);
                    //    }
                    //}
                    
                    result.Add(OFsite);
                }
            }
        return result;
        }
        
    }
}