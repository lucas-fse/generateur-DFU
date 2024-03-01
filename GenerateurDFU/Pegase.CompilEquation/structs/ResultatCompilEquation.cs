using Pegase.CompilEquation.enums;

namespace Pegase.CompilEquation.structs
{
    /// <summary>
    /// resultat de la compilation d'equation
    /// </summary>
	public class ResultatCompilEquation
    {
        #region private properties
        /// <summary>
        /// L'indice de l'erreur
        /// </summary>
        private int indice;

        /// <summary>
        /// Le diagnostique de la compilation
        /// </summary>
        private EnumDiagnosticCompilEquation diagnostique;
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the indice.
        /// </summary>
        /// <value>
        /// The indice.
        /// </value>
        public int Indice
        {
            get { return this.indice; }
            set { this.indice = value; }
        }

        /// <summary>
        /// Gets or sets the diagnostique.
        /// </summary>
        /// <value>
        /// The diagnostique.
        /// </value>
        public EnumDiagnosticCompilEquation Diagnostique
        {
            get { return this.diagnostique; }
            set { this.diagnostique = value; }
        }

        #endregion

        
    }
}
