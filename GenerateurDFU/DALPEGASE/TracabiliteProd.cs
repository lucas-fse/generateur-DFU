using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALPEGASE
{
    
    public class TracabiliteProd
    {
        public ORDRE_FABRICATION of { get; set; }
        public MO mo { get; set; }
        public MT mt { get; set; }
        public SIM sim { get; set; }

        public ORDRE_FABRICATION_GENERE OF_genere { get; set; }

        public PACK_INSTALLE pack { get; set; }
        private PEGASE_PRODEntities db;
        private PEGASE_PROD2Entities db2;

        public TracabiliteProd()
        {
            db = new PEGASE_PRODEntities();
            db2 = new PEGASE_PROD2Entities();
            of = new ORDRE_FABRICATION();
            
        }
        public bool PackInstalleExiste(string num_serie)
        {
            bool result = true;
            try
            {
                var query_packexiste = db.PACK_INSTALLE.Where(p => p.NUM_SERIE_PACK == num_serie );
                if (query_packexiste.Count() == 1)
                {
                    result =  true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            { }
            
            return result;
        }
        public List<ORDRE_FABRICATION>  OrdreFabricationExiste(string of)
        {
            List<ORDRE_FABRICATION> result = new List<ORDRE_FABRICATION>();
            try
            {
                var query_packexiste = db.ORDRE_FABRICATION.Where(p => p.NUM_OF.Contains(of));
                if (query_packexiste.Count() == 1)
                {
                    result =  query_packexiste.ToList();
                }
                else
                {
                   
                }
            }
            catch (Exception e)
            { }

            return result;
        }


        public void Savedb()
        {
            ORDRE_FABRICATION of_existe = null;
            PACK_INSTALLE pack_existe = null;
            MO mo_existe = null;
            MT mt_existe = null;
            SIM sim_existe = null;

            try
            {
                var query_of_existe = db.ORDRE_FABRICATION.Where(p => p.NUM_OF == of.NUM_OF);
                of_existe = query_of_existe.FirstOrDefault();
            }
            catch
            {

            }
            if (of_existe == null)
            {
                db.ORDRE_FABRICATION.Add(of);
                db.SaveChanges();
                of_existe = db.ORDRE_FABRICATION.First(p => p.NUM_OF == of.NUM_OF);
            }
            else
            {
                of_existe.copy(of);
                db.SaveChanges();
            }
            long id_of = of_existe.ID;
            pack.ID_OF = id_of;
           
            try
            {
                var query_packexiste = of_existe.PACK_INSTALLE.Where(p => p.ID_OF == id_of && p.NUM_SERIE_PACK == pack.NUM_SERIE_PACK);
                pack_existe = query_packexiste.FirstOrDefault();
            }
            catch
            { }
            if (pack_existe == null)
            {
                db.PACK_INSTALLE.Add(pack);
                db.SaveChanges();
                pack_existe = db.PACK_INSTALLE.First(p => p.ID_OF == id_of && p.NUM_SERIE_PACK == pack.NUM_SERIE_PACK);
            }
            long id_pack = pack_existe.ID;
            if (mo != null)
            { 
                try
                {
                    mo.ID_PACK_INSTALLE = id_pack;
                    var query_moexiste = pack_existe.MO.Where(p => p.ID_PACK_INSTALLE == id_pack && p.NUM_SERIE_MO == mo.NUM_SERIE_MO);
                    mo_existe = query_moexiste.FirstOrDefault();
                }
                catch
                { }
                if (mo_existe == null)
                {
                    db.MO.Add(mo);
                    db.SaveChanges();
                }
                else
                {
                    mo_existe.copy(mo);
                    db.SaveChanges();
                }
            }
            if (mt!= null)
            {
                try
                {
                    mt.ID_PACK_INSTALLE = id_pack;
                    var query_mtexiste = pack_existe.MT.Where(p => p.ID_PACK_INSTALLE == id_pack && p.NUM_SERIE_MT == mt.NUM_SERIE_MT);
                    mt_existe = query_mtexiste.FirstOrDefault();
                }
                catch
                { }
                if (mt_existe == null)
                {
                    db.MT.Add(mt);
                    db.SaveChanges();
                }
                else
                {
                    mt_existe.copy( mt);
                    db.SaveChanges();
                }
            }
            if (sim != null)
            {
                try
                {
                    sim.ID_PACK_INSTALLE = id_pack;
                    var query_simexiste = pack_existe.SIM.Where(p => p.ID_PACK_INSTALLE == id_pack && p.NUM_SERIE_SIM == sim.NUM_SERIE_SIM);
                    sim_existe = query_simexiste.FirstOrDefault();
                }
                catch
                { }
                if (sim_existe == null)
                {
                    db.SIM.Add(sim);
                    db.SaveChanges();
                }
                else
                {
                    sim_existe.copy(sim);
                    db.SaveChanges();
                }
            }
        }
        public List<ORDRE_FABRICATION_GENERE> OFGenereExistedb2(string NmrOf,bool genere,bool manuel)
        {
            List<ORDRE_FABRICATION_GENERE> result = new List<ORDRE_FABRICATION_GENERE>();
            try
            {
                IQueryable<ORDRE_FABRICATION_GENERE> query_OFgenereexiste;
                if (genere && manuel)
                {
                    query_OFgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == NmrOf && p.GENERE == true && p.MODIF_MANUEL == true && p.FABRIQUE != true);
                }
                else if (genere)
                {
                    query_OFgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == NmrOf && p.GENERE == true && p.FABRIQUE != true);
                }
                else if (manuel)
                {
                    query_OFgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == NmrOf && p.MODIF_MANUEL == true && p.FABRIQUE != true);
                }
                else
                {
                    query_OFgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == NmrOf && p.FABRIQUE != true);
                }
                 
                if (query_OFgenereexiste.Count() == 1)
                {
                    result = query_OFgenereexiste.ToList();
                }
                else
                {
                    result = null;
                }
            }
            catch
            { }
            return result;
        }
        public bool SaveDb2(ORDRE_FABRICATION_GENERE OfGenere)
        {
            if (OfGenere != null)
            {
                ORDRE_FABRICATION_GENERE ofgenere_existe = null;
                try
                {
                    string nmr = OfGenere.NUM_OF;
                    var query_ofgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == nmr);
                    ofgenere_existe = query_ofgenereexiste.FirstOrDefault();
                }
                catch
                { }
                if (ofgenere_existe != null)
                {
                    ofgenere_existe.COMMANDE_SYNCHRO = OfGenere.COMMANDE_SYNCHRO;
                    ofgenere_existe.DATE_GENERATION = OfGenere.DATE_GENERATION;
                    ofgenere_existe.GENERE = OfGenere.GENERE;
                    ofgenere_existe.MARCHE = OfGenere.MARCHE;
                    ofgenere_existe.OPTIONS_LOGICIELLES = OfGenere.OPTIONS_LOGICIELLES;
                    ofgenere_existe.OPTION_MATERIEL_MO = OfGenere.OPTION_MATERIEL_MO;
                    ofgenere_existe.OPTION_MATERIEL_MT = OfGenere.OPTION_MATERIEL_MT;
                    ofgenere_existe.REF_COMMERCIALE_MO = OfGenere.REF_COMMERCIALE_MO;
                    ofgenere_existe.REF_INDUSTRIELLE_MO = OfGenere.REF_INDUSTRIELLE_MO;
                    ofgenere_existe.REF_COMMERCIALE_MT = OfGenere.REF_COMMERCIALE_MT;
                    ofgenere_existe.REF_INDUSTRIELLE_MT = OfGenere.REF_INDUSTRIELLE_MT;
                    ofgenere_existe.REF_COMMERCIALE_SIM = OfGenere.REF_COMMERCIALE_SIM;
                    ofgenere_existe.REF_FICHE_PERSO = OfGenere.REF_FICHE_PERSO;
                    ofgenere_existe.MODIF_MANUEL = OfGenere.MODIF_MANUEL;
                    ofgenere_existe.NB_SIM = OfGenere.NB_SIM;
                    ofgenere_existe.NB_MO = OfGenere.NB_MO;
                    ofgenere_existe.NB_MT = OfGenere.NB_MT;
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_SIM))
                    //{
                    //    OfGenere.NB_SIM = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_SIM = 0;
                    //}
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MO))
                    //{
                    //    OfGenere.NB_MO = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_MO = 0;
                    //}
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MT))
                    //{
                    //    OfGenere.NB_MT = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_MT = 0;
                    //}
                    db2.SaveChanges();
                    return true;
                }
                else
                {
                    OfGenere.GENERE = false;
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_SIM))
                    //{
                    //    OfGenere.NB_SIM = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_SIM = 0;
                    //}
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MO))
                    //{
                    //    OfGenere.NB_MO = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_MO = 0;
                    //}
                    //if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MT))
                    //{
                    //    OfGenere.NB_MT = 1;
                    //}
                    //else
                    //{
                    //    OfGenere.NB_MT = 0;
                    //}
                    db2.ORDRE_FABRICATION_GENERE.Add(OfGenere);
                    db2.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    //    public bool AddSaveDb2(ORDRE_FABRICATION_GENERE OfGenere)
    //    {
    //        if (OfGenere != null)
    //        {
    //            bool findof = false;
    //            try
    //            {
    //                string nmr = OfGenere.NUM_OF;
    //                var query_ofgenereexiste = db2.ORDRE_FABRICATION_GENERE.Where(p => p.NUM_OF == nmr);
    //                if (query_ofgenereexiste!= null && query_ofgenereexiste.Count()>0)
    //                { findof = true; }
    //            }
    //            catch
    //            { }
    //            if (!findof)
    //            {
    //                OfGenere.GENERE = false;
    //                OfGenere.DATE_GENERATION = null;
    //                OfGenere.MODIF_MANUEL = false;
    //                if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_SIM))
    //                {
    //                    OfGenere.NB_SIM = 1;
    //                }
    //                else
    //                {
    //                    OfGenere.NB_SIM = 0;
    //                }
    //                if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MO))
    //                {
    //                    OfGenere.NB_MO = 1;
    //                }
    //                else
    //                {
    //                    OfGenere.NB_MO = 0;
    //                }
    //                if (!string.IsNullOrWhiteSpace(OfGenere.REF_COMMERCIALE_MT))
    //                {
    //                    OfGenere.NB_MT = 1;
    //                }
    //                else
    //                {
    //                    OfGenere.NB_MT = 0;
    //                }
    //                db2.ORDRE_FABRICATION_GENERE.Add(OfGenere);
    //                db2.SaveChanges();
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //        return false;
    //    }
    
    }
}
