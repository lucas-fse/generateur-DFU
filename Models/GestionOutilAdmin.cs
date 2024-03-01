using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public class GestionOutilAdmin
    {
        public long ID { get; set; }

        public bool ExecuteSuppressionOF(string NmrOfASupprimer)
        {
            // Supprimer l'OF de la base Prod
            NmrOfASupprimer = NmrOfASupprimer.ToUpper().Trim();
            // 1 - récupérer tous les ID nécessaires
            long IDOF;
            List<long> IDPackInstalle, IDMO, IDMT, IDSIM;

            PEGASE_PRODEntities3 _db = new PEGASE_PRODEntities3();

            

            var QIDOF = from idof in _db.ORDRE_FABRICATION
                        where idof.NUM_OF == NmrOfASupprimer
                        select idof.ID;

            IDOF = QIDOF.FirstOrDefault();

            if (QIDOF.Count() == 0)
            {
                return false;
            }

            var QIDPI = from idpi in _db.PACK_INSTALLE
                        where idpi.ID_OF == IDOF
                        select idpi.ID;

            IDPackInstalle = QIDPI.ToList();
            if (IDPackInstalle == null)
            {
                return false;
            }

            // IDMO
            IDMO = new List<long>();
            foreach (var row in IDPackInstalle)
            {
                var QIDMO = from mo in _db.MO
                            where mo.ID_PACK_INSTALLE == row
                            select mo.ID;

                if (QIDMO.Count() > 0)
                {
                    List<long> listIDMO = QIDMO.ToList();
                    foreach (var item in listIDMO)
                    {
                        IDMO.Add(item);
                    }
                }
            }

            // IDMT
            IDMT = new List<long>();
            foreach (var row in IDPackInstalle)
            {
                var QIDMT = from mt in _db.MT
                            where mt.ID_PACK_INSTALLE == row
                            select mt.ID;
                if (QIDMT.Count() > 0)
                {
                    List<long> listIDMT = QIDMT.ToList();

                    foreach (var item in listIDMT)
                    {
                        IDMT.Add(item);
                    }
                }
            }

            // IDSIM
            IDSIM = new List<long>();

            foreach (var row in IDPackInstalle)
            {
                var QIDSIM = from sim in _db.SIM
                             where sim.ID_PACK_INSTALLE == row
                             select sim.ID;
                if (QIDSIM.Count() > 0)
                {
                    List<long> listIDSIM = QIDSIM.ToList();

                    foreach (var item in listIDSIM)
                    {
                        IDSIM.Add(item);
                    }
                }
            }

            // 2 - Effacer les données table par table, dans le bon ordre

            // 2.01 -> MO_Etape
            if (IDMO.Count > 0)
            {
                foreach (var idmo in IDMO)
                {
                    var DelMOEtape = from mo in _db.MO_ETAPE
                                     where mo.ID_MO == idmo
                                     select mo;

                    if (DelMOEtape.Count() > 0)
                    {
                        foreach (var row in DelMOEtape)
                        {
                            _db.MO_ETAPE.Remove(row);
                        }
                    }
                }
            }

            // 2.02 -> MO
            foreach (var idpackinstalle in IDPackInstalle)
            {
                var DelMO = from mo in _db.MO
                            where mo.ID_PACK_INSTALLE == idpackinstalle
                            select mo;

                if (DelMO.Count() > 0)
                {
                    foreach (var row in DelMO)
                    {
                        _db.MO.Remove(row);
                    }
                }
            }

            // 2.03 -> Pack_Installe_Etape
            foreach (var idpackinstalle in IDPackInstalle)
            {
                var DelPackIsntalleEtape = from pie in _db.PACK_INSTALLE_ETAPE
                                           where pie.ID_PACK_INSTALLE == idpackinstalle
                                           select pie;

                if (DelPackIsntalleEtape.Count() > 0)
                {
                    foreach (var row in DelPackIsntalleEtape)
                    {
                        _db.PACK_INSTALLE_ETAPE.Remove(row);
                    }
                }
            }

            // 2.04 -> SIM_Etape
            if (IDSIM.Count > 0)
            {
                foreach (var idsim in IDSIM)
                {
                    var DelSIMEtape = from sim in _db.SIM_ETAPE
                                      where sim.ID_SIM == idsim
                                      select sim;

                    if (DelSIMEtape.Count() > 0)
                    {
                        foreach (var row in DelSIMEtape)
                        {
                            _db.SIM_ETAPE.Remove(row);
                        }
                    }
                }
            }

            // 2.05 -> SIM
            if (IDPackInstalle.Count > 0)
            {
                foreach (var idpackinstalle in IDPackInstalle)
                {
                    var DelSIM = from sim in _db.SIM
                                 where sim.ID_PACK_INSTALLE == idpackinstalle
                                 select sim;

                    if (DelSIM.Count() > 0)
                    {
                        foreach (var row in DelSIM)
                        {
                            _db.SIM.Remove(row);
                        }
                    }
                }
            }

            // 2.06 -> MT_Etape
            if (IDMT.Count > 0)
            {
                foreach (var idmt in IDMT)
                {
                    var DelMTEtape = from mt in _db.MT_ETAPE
                                     where mt.ID_MT == idmt
                                     select mt;

                    if (DelMTEtape.Count() > 0)
                    {
                        foreach (var row in DelMTEtape)
                        {
                            _db.MT_ETAPE.Remove(row);
                        }
                    }
                }
            }

            // 2.07 -> MT
            if (IDPackInstalle.Count > 0)
            {
                foreach (var idpackinstalle in IDPackInstalle)
                {
                    var DelMT = from mt in _db.MT
                                where mt.ID_PACK_INSTALLE == idpackinstalle
                                select mt;

                    if (DelMT.Count() > 0)
                    {
                        foreach (var row in DelMT)
                        {
                            _db.MT.Remove(row);
                        }
                    }
                }
            }

            // 2.08 -> Ordre_Fabrication_Etape
            var DelOFEtape = from ofe in _db.ORDRE_FABRICATION_ETAPE
                             where ofe.ID_OF == IDOF
                             select ofe;

            if (DelOFEtape.Count() > 0)
            {
                foreach (var row in DelOFEtape)
                {
                    _db.ORDRE_FABRICATION_ETAPE.Remove(row);
                }
            }

            // 2.081 -> Fiche_Pack_Tracabilite
            if (IDPackInstalle.Count > 0)
            {
                foreach (var idpackinstalle in IDPackInstalle)
                {
                    var DelFichePackTracabilite = from fpt in _db.FICHE_PACK_TRACABILITE
                                                  where fpt.ID_PACK_INSTALLE == idpackinstalle
                                                  select fpt;

                    if (DelFichePackTracabilite.Count() > 0)
                    {
                        foreach (var row in DelFichePackTracabilite)
                        {
                            _db.FICHE_PACK_TRACABILITE.Remove(row);
                        }
                    }
                }
            }

            // 2.09 -> Pack_Installe
            var DelPackInstalle = from pi in _db.PACK_INSTALLE
                                  where pi.ID_OF == IDOF
                                  select pi;

            if (DelPackInstalle.Count() > 0)
            {
                foreach (var row in DelPackInstalle)
                {
                    _db.PACK_INSTALLE.Remove(row);
                }
            }

            // 2.10 -> Ordre Fabrication
            var DelOF = from of in _db.ORDRE_FABRICATION
                        where of.NUM_OF == NmrOfASupprimer
                        select of;

            if (DelOF.Count() > 0)
            {
                foreach (var row in DelOF)
                {
                    _db.ORDRE_FABRICATION.Remove(row);
                }
            }
            _db.SaveChanges();
            return true;
        } // endMethod: ExecuteCommandOFName

    }
}