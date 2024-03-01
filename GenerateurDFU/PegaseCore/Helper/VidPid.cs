using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Class permettant de manipuler les données Vid Pid des produits
    /// </summary>
    public class VidPid
    {
        /// <summary>
        /// Le VidPid est-il cohérent?
        /// </summary>
        public Boolean IsOk
        {
            get
            {
                Boolean Result = false;

                if (this.Vid != 0 && this.Pid != 0)
                {
                    Result = true;
                }
                return Result;
            }
        } // endProperty: IsOk

        /// <summary>
        /// Le nom du produit
        /// </summary>
        public String Name
        {
            get;
            set;
        } // endProperty: Name

        /// <summary>
        /// Le Vid du produit
        /// </summary>
        public ushort Vid
        {
            get;
            set;
        } // endProperty: Vid

        /// <summary>
        /// Le Pid du produit
        /// </summary>
        public ushort Pid
        {
            get;
            set;
        } // endProperty: Pid

        /// <summary>
        /// Constructeur de la classe VidPid
        /// </summary>
        /// <param name="Name">
        /// Le nom du produit correspondant
        /// </param>
        public VidPid(String Name)
        {
            this.Name = Name;
            ushort vid, pid;

            VidPidHelper.GetVidPid(Name, out vid, out pid);

            this.Vid = vid;
            this.Pid = pid;
        }
    }
}
