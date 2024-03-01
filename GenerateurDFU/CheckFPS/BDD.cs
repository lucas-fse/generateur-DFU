using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckFPS.DAL
{
    /// <summary>
    /// </summary>
    public class BDD
    {
        // Variables singleton
        private static BDD _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables
        private PEGASE_CHECKFPSEntities _pegaseCheckFPS;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// retourne une instance unique de Pegase_CheckEntities
        /// </summary>
        public PEGASE_CHECKFPSEntities PegaseCheckFPS
        {
            get
            {
                if (this._pegaseCheckFPS == null)
                {
                    this._pegaseCheckFPS = new PEGASE_CHECKFPSEntities();
                }

                return this._pegaseCheckFPS;
            }
        } // endProperty: PegaseCheckFPS

        /// <summary>
        /// L'instance unique de la classe
        /// </summary>
        public static BDD Instance
        {
            get
            {
                return Get();
            }
        } // endProperty: Instance
        #endregion


        // Constructeur
        #region Constructeur

        private BDD()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        private static BDD Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new BDD();


                return _instance;
            }
        }

        /// <summary>
        /// Retourne la fiche spécifiée si elle existe ou null
        /// </summary>
        public Fiches GetFiche(String FicheName)
        {
            Fiches Result = null;

            var query = from fiche in this.PegaseCheckFPS.Fiches
                        where fiche.NumFiche == FicheName
                        select fiche;

            if (query.Count() > 0)
            {
                Result = query.First();
            }

            return Result;
        } // endMethod: GetFiche

        /// <summary>
        /// Créer une fiche vide et l'insérer dans la base de données
        /// </summary>
        public Fiches CreateEmptyFiche ( String FicheName )
        {
            Fiches Result;

            Result = new Fiches();
            Result.NumFiche = FicheName;
            this.PegaseCheckFPS.Fiches.Add(Result);
            this.PegaseCheckFPS.SaveChanges();

            return Result;
        } // endMethod: CreateEmptyFiche
        
        /// <summary>
        /// Créer une ligne d'information
        /// </summary>
        public SuiviMAJ CreateSuiviMAJ ( ErrorC Error )
        {
            SuiviMAJ Result;

            if (this.PegaseCheckFPS.SuiviMAJ.Count() == 0)
            {
                Result = new SuiviMAJ();
                this.PegaseCheckFPS.SuiviMAJ.Add(Result);
            }
            else
            {
                Result = this.PegaseCheckFPS.SuiviMAJ.First();
            }

            Result.LastMAJ = DateTime.Now;

            switch (Error)
            {
                case CheckFPS.ErrorC.NoError:
                    Result.Error = "pas d'erreurs";
                    break;
                case CheckFPS.ErrorC.FPSRootError:
                    Result.Error = "Le chemin d'accès au FPS est erronée";
                    break;
                case CheckFPS.ErrorC.DirectoryError:
                    Result.Error = "Le chemin d'accès au FPS est inaccessible";
                    break;
                case CheckFPS.ErrorC.NoFiches:
                    Result.Error = "Pas de fiches trouvées";
                    break;
                case CheckFPS.ErrorC.BDDError:
                    Result.Error = "Base de données inaccessible";
                    break;
                default:
                    break;
            }

            this.PegaseCheckFPS.SaveChanges();
            
            return Result;
        } // endMethod: CreateSuiviMAJ

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: BDD
}
