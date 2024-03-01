using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class TestOutilProductionVue
    {
       
        //public void RequestTest ()
        //{
        //    TestOutilProduction tt =  TestOutilProduction.GetInstance();
        //    tt.TestOutilProductionExecute();
        //}
        public List<string> RequestResult()
        {
            TestOutilProduction tt = TestOutilProduction.GetInstance();
            if (tt.Result == null)
                return new List<string>();
            return tt.Result;
        }
    }
}