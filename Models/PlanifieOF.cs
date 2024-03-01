using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class PlanifieOF
    {
        public OPERATEURS op { get; set; }
        public int AppartientAuPole { get; set; }// 0 appartient au pole 1 n'appartien pas au pole
        public string BackgroundcolorOperateur
        {
            get
            {
                if (AppartientAuPole!=0)
                {
                    return "#FFFFFF";
                }
                else
                {                    
                    return "#555555";
                }
            }
        }
        public List<OFATraite> ListOfPlanifie {get;set;}
    }
}