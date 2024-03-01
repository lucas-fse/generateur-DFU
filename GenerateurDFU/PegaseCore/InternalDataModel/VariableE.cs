using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Définition d'une variable manipulable de l'embarqué
    /// </summary>
    public class VariableE
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La variable est de type Input ou Output?
        /// </summary>
        public String IO
        {
            get;
            set;
        } // endProperty: IO

        /// <summary>
        /// Le nom de la variable dans l'embarqué
        /// </summary>
        public String Name
        {
            get;
            set;
        } // endProperty: Name

        /// <summary>
        /// Le nom utilisateur de la variable
        /// </summary>
        public String UserName
        {
            get;
            set;
        } // endProperty: UserName

        public String UserNameTimo
        {
            get;
            set;
        } // endProperty: UserName
        public String associateoutput
        {
            get;
            set;
        } // endProperty: UserName
        /// <summary>
        /// Le type de la variable
        /// </summary>
        public Type VarType
        {
            get;
            set;
        } // endProperty: VarType

        /// <summary>
        /// Marquer les variables utilisées par l'utilisateur
        /// </summary>
        public Boolean IsUsedByUser
        {
            get;
            set;
        } // endProperty: IsUsedByUser

        /// <summary>
        /// La variable est utilisée par iDialog
        /// </summary>
        public Boolean IsUsedByiDialog
        {
            get;
            set;
        } // endProperty: IsUsedByEquation

        /// <summary>
        /// La variable est utilisée par le mode universel
        /// </summary>
        public Boolean IsUsedByUniversalMode
        {
            get;
            set;
        } // endProperty: IsUsedByUniversalMode

        #endregion

        public String TypeHard
        {
            get;
            set;
        }
        public string Label
        {
            get;
            set;
        }
        public int Image
        {
            get;
            set;
        }
        // Constructeur
        #region Constructeur

        public VariableE()
        {
        }
        public VariableE(String name, String userName, Type type, String io, string UserNameTimo, string associateoutput)
        {
            this.Name = name;
            this.UserName = userName;
            this.VarType = type;
            this.IO = io;
            this.UserNameTimo = UserNameTimo;
            this.associateoutput = associateoutput;
            this.TypeHard = "";
        }
        public VariableE(String name, String userName, Type type, String io, String typehard)
        {
            this.Name = name;
            this.UserName = userName;
            this.VarType = type;
            this.IO = io;
            this.TypeHard = typehard;
            this.UserNameTimo = "";
            this.associateoutput = "";
        }

        public VariableE(String name, String userName, Type type, String io)
        {
            this.Name = name;
            this.UserName = userName;
            this.VarType = type;
            this.IO = io;
            this.TypeHard = "";
            this.UserNameTimo = "";
            this.associateoutput = "";
        }
        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: VariableE

	
}
