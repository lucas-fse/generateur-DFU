using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    public class NomOrgans
    {
        /// <summary>
        /// Le type de produit 
        /// </summary>
        public ProductType TypeProduit
        {
            get;
            set;
        } // endProperty: TypeProduit

        /// <summary>
        /// La clé
        /// </summary>
        public String Key
        {
            get;
            set;
        } // endProperty: Key

        /// <summary>
        /// Le nom de l'organe
        /// </summary>
        public String Name
        {
            get;
            set;
        } // endProperty: Name

        /// <summary>
        /// Le numéro d'ordre de l'organe
        /// </summary>
        public Int32 ID
        {
            get;
            set;
        } // endProperty: ID

        /// <summary>
        /// Le compteur associé au produit
        /// </summary>
        public UInt32 Counter
        {
            get;
            set;
        } // endProperty: Counter

        /// <summary>
        /// Le temps écoulé
        /// </summary>
        public UInt32 ElapsedTime
        {
            get;
            set;
        } // endProperty: ElapsedTime
    }
}
