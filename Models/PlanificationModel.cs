using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows;

namespace GenerateurDFUSafir.Models
{
    public class PlanificationModel
    {
        public DateTime Date { get; set; }
        public List<OfByOperateur> ListofByOperateur { get; set; }       
       

        public void LoadPlanificationOfFromDb()
        {

            List<string> result = new List<string>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                var query = _db.PLANIFICATION_OF.Where(p => p.Validite == true);
                List<long> Listidoperateur = new List<long>();
                foreach (var of in query.ToList())
                {
                    result.Add(of.NMROF);
                    if (!Listidoperateur.Contains((long)of.Operateur))
                    {
                        Listidoperateur.Add((long)of.Operateur);
                    }
                }                

                // mise a jour de la planification
                ListofByOperateur = new List<OfByOperateur>();
                foreach (var id in Listidoperateur)
                {
                    OfByOperateur ofByOperateur = new OfByOperateur();
                    ofByOperateur.ID = id;
                    ofByOperateur.ListeOf = new List<string>();
                    var queryorder = query.Where(p => p.Operateur == id).OrderBy(D => D.Datetime).OrderBy(n => n.NmrOrdre);
                    foreach(var of in queryorder.ToList())
                    {
                        ofByOperateur.ListeOf.Add(of.NMROF);
                    }
                    ListofByOperateur.Add(ofByOperateur);
                }
            }    
        }
        public List<string>  LoadPlanificationOfFromDb( long IDope)
        {
            List<string> ListOfForOperateur = new List<string>();
            List<string> result = new List<string>();
            using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
            {
                var query = _db.PLANIFICATION_OF.Where(p => p.Validite == true && p.Operateur == IDope);
                
               if (query!= null && query.Count()>0)
               {
                    foreach(var of in query.OrderBy(p=>p.Datetime).OrderBy(n=>n.NmrOrdre))
                    {
                        ListOfForOperateur.Add(of.NMROF.Trim());
                    }
                }
            }
            return ListOfForOperateur;
        }
        public PlanificationModel()
        {

        }
    }
    public class OfByOperateur
    {
        public long ID { get; set; }
        [JsonIgnore]
        public OPERATEURS operateur
        {
            get
            {
                OPERATEURS op = null;
                using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
                {
                     op = _db.OPERATEURS.Where(o => o.ID == ID).First();
                }
                return op;
            }
        }
        public List<string> ListeOf  { get;set;}
    }
    
}