using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JAY;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe décrivant une ligne du journal d'un micro
    /// </summary>
    public class JournalLine
    {
        // Variables
        private UInt32 _codeErreur;
        private DateTime _heure;
        private String _erreurText;
        private Byte _diag;

        // Propriétés

        /// <summary>
        /// Le numéro de l'erreur
        /// </summary>
        public Int32 NumErreur
        {
            get;
            set;
        } // endProperty: NumErreur

        /// <summary>
        /// Le diagnostique, == 0 si la ligne ne fait pas partie du journal
        /// </summary>
        public Byte Diag
        {
            get
            {
                return this._diag;
            }
            set
            {
                this._diag = value;
            }
        } // endProperty: Diag

        /// <summary>
        /// Le code de l'erreur
        /// </summary>
        public UInt32 CodeErreur
        {
            get
            {
                return this._codeErreur;
            }
            private set
            {
                this._codeErreur = value;
            }
        } // endProperty: CodeErreur

        /// <summary>
        /// L'heure de l'évenement
        /// </summary>
        public DateTime Heure
        {
            get
            {
                return this._heure;
            }
            private set
            {
                this._heure = value;
            }
        } // endProperty: Heure

        /// <summary>
        /// Le texte de l'erreur, traduite dans la bonne langue
        /// </summary>
        public String ErreurText
        {
            get
            {
                return this._erreurText;
            }
            private set
            {
                this._erreurText = value;
            }
        } // endProperty: ErreurText

        // Contructeurs
        public JournalLine( UInt32 temps, UInt32 CodeErreur, Byte Diag)
        {
            // Calculer la date
            DateTime time = new DateTime(Constantes.RefYear, Constantes.RefMounth, Constantes.RefDay);
            time = time.ToUniversalTime();
            Int32 heures, minutes, seconde;

            heures = (Int32)(temps / 3600);
            minutes = (Int32)((temps - (UInt32)heures * 3600)/60);
            seconde = (Int32)((temps - (UInt32)heures * 3600 - (UInt32)minutes * 60 ));

            TimeSpan timeS = new TimeSpan(heures, minutes, seconde);
            time += timeS;
            this.Heure = time;

            // Mettre à jour le texte d'erreur
            this.CodeErreur = CodeErreur;

            // Le diagnostique
            this.Diag = Diag;
        }

        /// <summary>
        /// Mettre à jour le texte de l'erreur à partir du fichier de langue
        /// </summary>
        public void MAJTextErreur ( )
        {
            var Query = from refErreur in JournalRef.Instance.JournalRefs
                        where refErreur.StartErrorNum <= this.CodeErreur && refErreur.EndErrorCode >= this.CodeErreur
                        select refErreur.ErreurText;

            this.ErreurText = Query.FirstOrDefault();
        } // endMethod: MAJTextErreur
    }
}
