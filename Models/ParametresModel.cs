using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public   class ParametresModel
    {
        [Display(Name = @"Reference  etiquette emballage 
                          -TYPE_354140A 
                          -TYPE_319321 ")]
        public IEnumerable<DATA_GENERIQUE> DataGeneriques { get; set; }
        public   ParametresModel(bool? editable)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            List<DATA_GENERIQUE> imprimantes;
            if (editable != null )
            {
                imprimantes  = _db.DATA_GENERIQUE.Where(e=> e.Editable==editable).ToList();
            }
            else
            {
                imprimantes = _db.DATA_GENERIQUE.ToList();
            }
            DataGeneriques = imprimantes;
            
        }
        public  string ParametresModelValue(int param)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            DATA_GENERIQUE imprimante =  _db.DATA_GENERIQUE.Where(p => p.ID == param).First();
            return imprimante.StringValue1.Trim();
        }
        public  void Save(int param,string TypeEtiquetteEmballage)
        {
            PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
            DATA_GENERIQUE imprimante = _db.DATA_GENERIQUE.Where(p => p.ID == param).First();
            imprimante.StringValue1 = TypeEtiquetteEmballage;
            _db.SaveChanges();
        }
    }
}