namespace Pegase.CompilEquation.enums
{
    /// <summary>
    /// Enum des différentes valeur de diagnostique de la compilation d'une équation
    /// </summary>
    public enum EnumDiagnosticCompilEquation
    {
        /// <summary>
        /// Pas de diagnostic
        /// </summary>
        PasDeDiagnostic = 0,

        /// <summary>
        /// Nom de variable inconnu
        /// </summary>
        NomInconnu = 1,

        /// <summary>
        /// Nom de variable incorrect
        /// </summary>
        NomIncorrect = 2,

        /// <summary>
        /// Equation vide
        /// </summary>
        EquationVide = 3,

        /// <summary>
        /// Paranthese fermante manquante
        /// </summary>
        ParentheseFermanteManquante = 4,

        /// <summary>
        /// Expression Correcte
        /// </summary>
        ExpressionCorrecte = 5,

        /// <summary>
        /// Pas d'espace apres la paranthèse fermante
        /// </summary>
        ManqueEspaceApresParentheseFermante = 6,

        /// <summary>
        /// Caractere inattendu apres l'expression
        /// </summary>
        ExtraCharAfterExpr = 7,

        /// <summary>
        /// Operateur conditionel incomplet
        /// </summary>
        OperateurConditionnelIncomplet = 8,

        /// <summary>
        /// Nombre de decimal incorrect
        /// </summary>
        NombreDecimalIncorrect = 9,

        /// <summary>
        /// Age indisponible
        /// </summary>
        AgeIndisponible = 10,

        /// <summary>
        /// Operande non binaire
        /// </summary>
        OperandeNonBinaire = 11,

        /// <summary>
        /// Operande non numérique
        /// </summary>
        OperandeNonNumerique = 12,
    }
}
