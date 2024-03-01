using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class OrdreFabrication
    {
        public string MFGNUM_0 { get; set; }
        public string VCRNUMORI_0 { get; set; }
        public string MFGSTA_0 { get; set; }
        public DateTime STRDAT_0 { get; set; }
        public DateTime ENDDAT_0 { get; set; }
        public string EXTWST_0 { get; set; }
        public string EXTQTY_0 { get; set; }
        public string EXTUNTTIM_0 { get; set; }
        public string EXTOPETIM_0 { get; set; }
        public string TEXTE_0 { get; set; }
        public string TCLCOD_0 { get; set; }
        public string ITMREF_0 { get; set; }
        public string MFGDES_0 { get; set; }
        public string TSICOD_4 { get; set; }
        public string MFGPIO_0 { get; set; }
        public string MFGTRKFLG_0 { get; set; }
        public string MRPMES_0 { get; set; }
        public string WIPTYP_0 { get; set; }
        public string ITMREFORI_0 { get; set; }
        public string ORI_0 { get; set; }
        public string ALLSTA_0 { get; set; }
        public string RMNEXTQTY_0 { get; set; }
        public DateTime OBJDAT_0 { get; set; }
        public Image Etiquette { get; set; }
        public int SERNUM { get; set; }

        public OrdreFabrication(
                     string _MFGNUM_0,
                     string _VCRNUMORI_0,
                     string _MFGSTA_0,
                     DateTime _STRDAT_0,
                     DateTime _ENDDAT_0,
                     string _EXTWST_0,
                     string _EXTQTY_0,
                     string _EXTUNTTIM_0,
                     string _EXTOPETIM_0,
                     string _TEXTE_0,
                     string _TCLCOD_0,
                     string _ITMREF_0,
                     string _MFGDES_0,
                     string _TSICOD_4,
                     string _MFGPIO_0,
                     string _MFGTRKFLG_0,
                     string _MRPMES_0,
                     string _WIPTYP_0,
                     string _ITMREFORI_0,
                     string _ORI_0,
                     string _ALLSTA_0,
                     string _RMNEXTQTY_0,
                     DateTime _OBJDAT_0,
                     int _SERNUM)
        {
            MFGNUM_0 = _MFGNUM_0;
            VCRNUMORI_0 = _VCRNUMORI_0;
            MFGSTA_0 = _MFGSTA_0;
            STRDAT_0 = _STRDAT_0;
            ENDDAT_0 = _ENDDAT_0;
            OBJDAT_0 = _OBJDAT_0;
            EXTWST_0 = _EXTWST_0;
            EXTQTY_0 = _EXTQTY_0;
            EXTUNTTIM_0 = _EXTUNTTIM_0;
            EXTOPETIM_0 = _EXTOPETIM_0;
            TEXTE_0 = _TEXTE_0;
            TCLCOD_0 = _TCLCOD_0;
            ITMREF_0 = _ITMREF_0;
            MFGDES_0 = _MFGDES_0;
            TSICOD_4 = _TSICOD_4;
            MFGPIO_0 = _MFGPIO_0;
            MFGTRKFLG_0 = _MFGTRKFLG_0;
            MRPMES_0 = _MRPMES_0;
            WIPTYP_0 = _WIPTYP_0;
            ITMREFORI_0 = _ITMREFORI_0;
            ORI_0 = _ORI_0;
            ALLSTA_0 = _ALLSTA_0;
            RMNEXTQTY_0 = _RMNEXTQTY_0;
            SERNUM = _SERNUM;

        }
    }
}