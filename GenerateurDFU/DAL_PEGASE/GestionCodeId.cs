using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL_PEGASE
{
    public class GestionCodeId
    {

        public string GestionNewIdCode(string type)
        {
            using (PEGASE_PRODEntities1 _db = new PEGASE_PRODEntities1())
            {
                IQueryable<COMPTEUR> result = _db.COMPTEUR;
                switch (type)
                {
                    case "CODE_MO":
                        result = result.Where(i => i.CODE_COMPTEUR.ToString() == "CODE_IDENTIFICATION_MO");
                        break;
                    case "CODE_MT":
                        result = result.Where(i => i.CODE_COMPTEUR.ToString() == "CODE_IDENTIFICATION_MT");
                        break;
                    case "NUM_SIM":
                        result = result.Where(i => i.CODE_COMPTEUR.ToString() == "NUM_SERIE_SIM");
                        break;
                    default:
                        break;
                }
                string nextchrono = null;
                if ((result != null) && (result.Count() == 1))
                {
                    foreach (COMPTEUR item in result)
                    {
                        nextchrono = GetNextNumChrono(item);
                        item.NEXT_NUM_CHRONO = nextchrono;
                        break;
                    }
                    _db.SaveChanges();
                    return nextchrono;
                }

            }

             
            return null;
        }
        private string GetNextNumChrono(COMPTEUR item)
        {
            string nextValue = null;
            if (item != null)
            {
                if (item.TYPE_COMPTEUR == (int) 4)
                {
                    nextValue = this.GetNextNumChronoHexadecimal(item);
                }
                else if (item.TYPE_COMPTEUR == (int)2)
                {
                    nextValue = this.GetNextNumChronoAlphaNumerique(item);
                }
               
            }

            return nextValue;
        }
        private string GetNextNumChronoAlphaNumerique(COMPTEUR item)
        {
            if (item != null)
            {
                // Calcul du prochain compteur
                string nextValue = item.NEXT_NUM_CHRONO;

                bool incrementationDone = false;

                // Parcours des char en partant de la fin 
                for (int i = nextValue.Length - 1; i >= 0; i--)
                {
                    if (nextValue[i] == '9')
                    {
                        nextValue = ReplaceCharAt(nextValue, i, 'A');
                        incrementationDone = true;
                        break;
                    }

                    if (nextValue[i] == 'Z')
                    {
                        if (i > 0)
                        {
                            nextValue = ReplaceCharAt(nextValue,i, '0');
                            continue;
                        }
                        else
                        {
                            // On est sur le caractère le plus à gauche donc il faut remettre à zero le compteur
                            // on sort donc de la boucle
                            break;
                        }
                    }

                    char theChar = nextValue[i];
                    theChar++;
                    incrementationDone = true;
                    nextValue = ReplaceCharAt(nextValue,i, theChar);
                    break;
                }

                // Si aucune incrémentation n'a été faite alors c'est qu'on est arrivé au max donc on repart à zero
                if (!incrementationDone)
                {
                    nextValue = "0";
                }

                // On retourne la nouvelle valeur (Complété par des zéros suivant la longueur du compteur)
                return nextValue.PadLeft(item.LONGUEUR_COMPTEUR, item.CARACTERE_COMPLETION.ToCharArray()[0]);
            }

            return string.Empty;
        }
        private string GetNextNumChronoHexadecimal(COMPTEUR item)
        {
            if (item != null)
            {
                // Calcul du prochain compteur
                string nextValue = item.NEXT_NUM_CHRONO;
                bool incrementationDone = false;

                // Cas particulier si on atteind FFFFE on repasse ensuite à 0
                if (nextValue.Length == 5 && nextValue == "FFFFE")
                {
                    nextValue = "0";
                }
                else
                {
                    // Parcours des char en partant de la fin 
                    for (int i = nextValue.Length - 1; i >= 0; i--)
                    {
                        // Dans le cas d'un 9 on passe directemnt au A
                        if (nextValue[i] == '9')
                        {
                            nextValue = ReplaceCharAt(nextValue, i, 'A');
                            incrementationDone = true;
                            break;
                        }

                        // Dans le cas d'un F on passe directemnt au 0 et on continue pour incrémenter le caractère de gauche
                        if (nextValue[i] == 'F')
                        {
                            if (i > 0)
                            {
                                nextValue = ReplaceCharAt(nextValue, i, '0');
                                continue;
                            }
                            else
                            {
                                // On est sur le caractère le plus à gauche donc il faut remettre à zero le compteur
                                // on sort donc de la boucle
                                break;
                            }
                        }

                        char theChar = nextValue[i];
                        theChar++;
                        incrementationDone = true;
                        nextValue = ReplaceCharAt(nextValue, i, theChar);
                        break;
                    }

                    // Si aucune incrémentation n'a été faite alors c'est qu'on est arrivé au max donc on repart à zero
                    if (!incrementationDone)
                    {
                        nextValue = "0";
                    }
                }

                // On retourne la nouvelle valeur (Complété par des zéros suivant la longueur du compteur)
                return nextValue.PadLeft(item.LONGUEUR_COMPTEUR, item.CARACTERE_COMPLETION.ToCharArray()[0]);
            }

            return string.Empty;
        }
        public static string ReplaceCharAt( string theString, int indiceReplace, char charReplace)
        {
            string resultString = theString;

            if (!string.IsNullOrEmpty(theString))
            {
                if (indiceReplace < theString.Length)
                {
                    if (indiceReplace > 0)
                    {
                        resultString = theString.Substring(0, indiceReplace) + charReplace + theString.Substring(indiceReplace + 1, theString.Length - indiceReplace - 1);
                    }

                    if (indiceReplace == 0)
                    {
                        resultString = charReplace + theString.Substring(1, theString.Length - indiceReplace - 1);
                    }
                }
            }

            return resultString;
        }
    }
}
