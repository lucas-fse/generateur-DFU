using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// classe static fournissant des fonctions de manipulations d'octet, lecture de bit...
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Lire un le bit spécifié de l'octet b
        /// </summary>
        public static Boolean ReadBit ( Byte b, UInt16 BitNumber )
        {
            Boolean Result = false;

            if (BitNumber < 8)
            {
                Byte M = 1;
                M = (Byte)(M << BitNumber);

                if ((M & b) == M)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            } 

            return Result;
        } // endMethod: ReadBit_Byte

        /// <summary>
        /// Lire un le bit spécifié de l'UInt16 b
        /// </summary>
        public static Boolean ReadBit(UInt16 b, UInt16 BitNumber)
        {
            Boolean Result = false;

            if (BitNumber < 16)
            {
                UInt16 M = 1;
                M = (UInt16)(M << BitNumber);

                if ((M | b) == b)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        } // endMethod: ReadBit_Byte

        /// <summary>
        /// Lire un le bit spécifié de l'UInt32 b
        /// </summary>
        public static Boolean ReadBit(UInt32 b, UInt16 BitNumber)
        {
            Boolean Result = false;

            if (BitNumber < 32)
            {
                UInt32 M = 1;
                M = (UInt32)(M << BitNumber);

                if ((M & b) == M)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        } // endMethod: ReadBit_Byte

        /// <summary>
        /// Assigner le bit BitNumber à la valeur Value de l'octet b
        /// </summary>
        public static Byte SetBit ( Byte b, UInt16 BitNumber, Boolean Value )
        {
            Byte M = 0;
            UInt16 MAX = 8;
            Boolean[] BitValue = new Boolean[MAX];
            Byte Result;

            if (BitNumber < MAX)
            {
                for (UInt16 i = 0; i < MAX; i++)
                {
                    BitValue[i] = BitHelper.ReadBit(b, i);
                }

                if (Value)
                {
                    BitValue[BitNumber] = true;
                }
                else
                {
                    BitValue[BitNumber] = false;
                }

                Result = 0;
                for (UInt16 i = 0; i < MAX; i++)
                {
                    if (BitValue[i] == true)
                    {
                        M = 1;
                        M = (Byte)(M << i);
                        Result += M;
                    }
                } 
            }
            else
            {
                Result = b;
            }

            return Result;
        } // endMethod: SetBit

        /// <summary>
        /// Assigner le bit BitNumber à la valeur Value de l'UInt16 b
        /// </summary>
        public static UInt16 SetBit(UInt16 b, UInt16 BitNumber, Boolean Value)
        {
            UInt16 M = 0;
            UInt16 MAX = 16;
            Boolean[] BitValue = new Boolean[MAX];
            UInt16 Result;

            if (BitNumber < MAX)
            {
                for (UInt16 i = 0; i < MAX; i++)
                {
                    BitValue[i] = BitHelper.ReadBit(b, i);
                }

                if (Value)
                {
                    BitValue[BitNumber] = true;
                }
                else
                {
                    BitValue[BitNumber] = false;
                }

                Result = 0;
                for (UInt16 i = 0; i < MAX; i++)
                {
                    if (BitValue[i] == true)
                    {
                        M = 1;
                        M = (UInt16)(M << i);
                        Result += M;
                    }
                }
            }
            else
            {
                Result = b;
            }

            return Result;
        } // endMethod: SetBit

        /// <summary>
        /// Assigner le bit BitNumber à la valeur Value de l'UInt16 b
        /// </summary>
        public static UInt32 SetBit(UInt32 b, UInt16 BitNumber, Boolean Value)
        {
            UInt32 M = 0;
            UInt16 MAX = 32;
            Boolean[] BitValue = new Boolean[MAX];
            UInt32 Result;

            if (BitNumber < MAX)
            {
                for (UInt16 i = 0; i < MAX; i++)
                {
                    BitValue[i] = BitHelper.ReadBit(b, i);
                }

                if (Value)
                {
                    BitValue[BitNumber] = true;
                }
                else
                {
                    BitValue[BitNumber] = false;
                }

                Result = 0;
                for (UInt16 i = 0; i < MAX; i++)
                {
                    if (BitValue[i] == true)
                    {
                        M = 1;
                        M = (UInt32)(M << i);
                        Result += M;
                    }
                }
            }
            else
            {
                Result = b;
            }

            return Result;
        } // endMethod: SetBit
    }
}
