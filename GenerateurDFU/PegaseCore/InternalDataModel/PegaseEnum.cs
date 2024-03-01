using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Le diagnostique de compilation des équations
    /// </summary>
    public enum DiagnosticCompilEquation_e
    {
        PAS_DE_DIAGNOSTIC,
        NOM_INCONNU,
        NOM_INCORRECT,
        EQUATION_VIDE,
        PARENTHESE_FERMANTE_MANQUANTE,
        EXPRESSION_CORRECTE,
        MANQUE_ESPACE_APRES_PARENTHESE_FERMANTE,
        EXTRA_CHAR_AFTER_EXPR,
        OPERATEUR_CONDITIONNEL_INCOMPLET,
        NOMBRE_DECIMAL_INCORRECT,
        AGE_INDISPONIBLE,
        OPERANDE_NON_BINAIRE,
        OPERANDE_NON_NUMERIQUE,
        OPERANDES_DE_TYPES_INCOMPATIBLES,
        OPERANDES_DE_FAMILLES_INCOMPATIBLES,
        OPERANDES_DE_TYPES_DIFFERENTS,
        SORTIE_ET_EXPRESSION_DE_TYPES_INCOMPATIBLES,
        OPERANDE_MANQUANT_OU_INCORRECT,
        DEF_OP_ABSENT
    }

    /// <summary>
    /// La famille des données (des organes)
    /// </summary>
    public enum FamilleDeDonnees_e
    {
        FAMILLE_BOUTON = 1,
        FAMILLE_SELECTEUR = 2,
        FAMILLE_COMMUTATEUR = 3,
        FAMILLE_COMMUTATEUR_A_12_POSITIONS = 4,
        FAMILLE_AXE_ANALOGIQUE = 5,
        FAMILLE_AXE_A_CRAN = 6,
        FAMILLE_POTENTIOMETRE = 7,
        FAMILLE_ENTREE_TOUT_OU_RIEN = 8,
        FAMILLE_SORTIE_TOUT_OU_RIEN = 9,
        FAMILLE_ENTREE_ANALOGIQUE = 10,
        FAMILLE_SORTIE_ANALOGIQUE = 11,
        FAMILLE_RELAIS = 12,
        FAMILLE_VARIABLE_NUMERIQUE = 13,
        FAMILLE_VARIABLE_BOOLEENNE = 14
    }
}
