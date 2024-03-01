using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Pegase.CompilEquation.enums;
using Pegase.CompilEquation.structs;

namespace Pegase.CompilEquation
{
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
        DEF_OP_ABSENT,
        MACRO_INVALIDE,
        HORS_MODE,
        EXPRESSION_TROP_LONGUE
    }

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

    [StructLayout(LayoutKind.Explicit)]
    struct ResultatCompilEquation_s
    {
        [FieldOffset(0)]
        public int Diagnostique;
        [FieldOffset(1)]
        public Byte V1;
        [FieldOffset(2)]
        public Byte V2;
        [FieldOffset(3)]
        public Byte V3;
        [FieldOffset(4)]
        public Byte V4;
    }

    public class ResultatCompilEquation
    {
        /// <summary>
        /// Le diagnostic de la compilation
        /// </summary>
        public DiagnosticCompilEquation_e Diagnostique
        {
            get;
            set;
        } // endProperty: Diagnostique

        /// <summary>
        /// La position de l'erreur
        /// </summary>
        public int Position
        {
            get;
            set;
        } // endProperty: Position
    }

    public class NativeMethods
    {
    }

    public class DLL_Equation
    {
        private const String DllPegaseEquation = "DLL_Pegase_Equations.dll";
        // TesterEquation(const char* TexteEquation, int *Famille, int *Indice, int *LgPgm, uint16_t PgmEq[] )
        // s_ResultatCompilEquation TesterEquation(const char* TexteEquation, int *Famille, int *Indice, int *LgPgm, uint16_t PgmEq[] );
        [DllImport(DllPegaseEquation, CallingConvention = CallingConvention.Cdecl)]
        private static extern int CompilerEquation(
                                               char[] TexteEquation,
                                               ref int Famille,
                                               ref int Indice,
                                               ref int LgPgm,
                                               UInt16[] PgmEq
                                              );

        [DllImport(DllPegaseEquation, CallingConvention = CallingConvention.Cdecl)]
        private static extern ResultatCompilEquation_s TesterEquation(
                               char[] TexteEquation,
                               ref int Famille,
                               ref int Indice,
                               ref int LgPgm,
                               UInt16[] PgmEq
                           );
        [DllImport(DllPegaseEquation, CallingConvention = CallingConvention.Cdecl)]
        private static extern void  FamilleParametre(
                                ref bool result,
                               char[] TexteEquation,
                               ref int Famille,
                               ref bool Binaire,
                               ref bool Constante
                           );
        public static bool RecupererFamilleDeLaDonnee(string Equation,ref int Famille,ref bool Binaire,ref bool Constante)
        {
            char[] TexteEquation;
            int Fam;
            bool Result;


            TexteEquation = new Char[Equation.Length + 1];
            for (int i = 0; i < Equation.Length; i++)
            {
                TexteEquation[i] = Equation[i];
            }
            TexteEquation[Equation.Length] = '\0';

            Fam = (int)-1;
            Result = false;

            FamilleParametre(ref Result,TexteEquation, ref Fam, ref Binaire, ref Constante);
            Famille = Fam;

            return Result;
        }
        public static DiagnosticCompilEquation_e CompilerEquation(String Equation, ref int Famille, ref int Indice, out int LongueurProgramme, out UInt16[] ProgrammeEquation)
        {
            char[] TexteEquation;
            int Fam, Ind, LgPgm;
            UInt16[] PgmEg = new UInt16[255];
            DiagnosticCompilEquation_e Result;

            TexteEquation = new Char[Equation.Length + 1];
            for (int i = 0; i < Equation.Length; i++)
            {
                TexteEquation[i] = Equation[i];
            }
            TexteEquation[Equation.Length] = '\0';

            Fam = (int)Famille;
            Ind = Indice;
            LgPgm = 0;

            Result = (DiagnosticCompilEquation_e)CompilerEquation(TexteEquation, ref Fam, ref Ind, ref LgPgm, PgmEg);
            LongueurProgramme = LgPgm;
            ProgrammeEquation = PgmEg;

            return Result;
        }

        public static ResultatCompilEquation TesterEquation(String Equation, ref int Famille, ref int Indice, out int LongueurProgramme, out UInt16[] ProgrammeEquation)
        {
            char[] TexteEquation;
            int Fam, Ind, LgPgm;
            UInt16[] PgmEg = new UInt16[255];
            ResultatCompilEquation Result;
            ResultatCompilEquation_s RCE;

            TexteEquation = new char[Equation.Length + 1];
            for (int i = 0; i < Equation.Length; i++)
            {
                TexteEquation[i] = Equation[i];
            }
            TexteEquation[Equation.Length] = '\0';

            LgPgm = 0;
            Fam = 0;
            Ind = 0;
            Result = new ResultatCompilEquation();
            if (TexteEquation.Length > 1023)
            {
                int tmp = TexteEquation.Length - 1;
                Result.Diagnostique = (DiagnosticCompilEquation_e)Pegase.CompilEquation.DiagnosticCompilEquation_e.EXPRESSION_TROP_LONGUE;
                Result.Position = tmp;
                LongueurProgramme = 0;
                ProgrammeEquation = null;
            }
            else
            {
                RCE = TesterEquation(TexteEquation, ref Fam, ref Ind, ref LgPgm, PgmEg);

                Famille = Fam;
                Indice = Ind;
                LongueurProgramme = LgPgm;
                ProgrammeEquation = PgmEg;

                
                Result.Diagnostique = (DiagnosticCompilEquation_e)RCE.Diagnostique;
                Result.Position = ((Int32)RCE.V1 * 256 * 256 * 256) + ((Int32)RCE.V2 * 256 * 256) + ((Int32)RCE.V3 * 256) + (Int32)RCE.V4;
            }
            return Result;
        }
    }
}
