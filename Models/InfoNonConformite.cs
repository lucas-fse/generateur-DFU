using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class InfoNonConformite
    {
        public short? InfoStatusPNCnullable { get; set; }
        public IEnumerable<SelectListItem> ListStatusPNC
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                result.Add(new SelectListItem { Text = "Liste PNC Non traité", Value = "0".ToString() });
                result.Add(new SelectListItem { Text = "Liste PNC Status R3-Q", Value = "1".ToString() });
                result.Add(new SelectListItem { Text = "Liste PNC traité", Value = "2".ToString() });
                return result;
            }
        }

        public List<NON_CONFORMITE> ListNonConformite = new List<NON_CONFORMITE>();
        private int? _typedata = 0;
        public int? TypeData
        {
            get
            {
                if (_typedata == null)
                {
                    return 0;
                }
                else
                {
                    return _typedata;
                }
            }
            set
            {
                _typedata = value;
                RefreshPNC();
            }
        }

        private void RefreshPNC()
        {
            try
            {
                if (true)
                {
                    PEGASE_PROD2Entities2 data = new PEGASE_PROD2Entities2();
                    if (TypeData == null || TypeData == 0)
                    {
                        ListNonConformite = data.NON_CONFORMITE.Where(i => i.Status == 0).OrderBy(p => p.Datetime).ToList();
                    }
                    else if (TypeData == 1)
                    {
                        ListNonConformite = data.NON_CONFORMITE.Where(i => i.Status == 2).OrderBy(p => p.Datetime).ToList();
                    }
                    else
                    {
                        ListNonConformite = data.NON_CONFORMITE.Where(i => i.Status != 2 && i.Status != 0).OrderBy(p => p.Datetime).ToList();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}