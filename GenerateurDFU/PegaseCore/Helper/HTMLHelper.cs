using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Classe de construction d'éléments HTML (tableau entre autre)
    /// </summary>
    public class HTMLHelper
    {
        /// <summary>
        /// Créer une balise table Vide
        /// </summary>
        public static XElement CreateTable ( Int32 border, Int32 cellpadding, Int32 width, String style )
        {
            XElement Result;
            XAttribute Attrib;

            Result = new XElement("table");
            Attrib = new XAttribute("border", border.ToString());
            Result.Add(Attrib);

            Attrib = new XAttribute("cellpadding", cellpadding.ToString());
            Result.Add(Attrib);

            Attrib = new XAttribute("width", width.ToString());
            Result.Add(Attrib);

            Attrib = new XAttribute("style", style);
            Result.Add(Attrib);

            return Result;
        } // endMethod: CreateTable

        /// <summary>
        /// Ajouter une colonne à la table source 'table'
        /// Attention, il s'agit d'ajouter toutes les colonnes après la création de la table
        /// Aucun contrôle de cohérence n'est effectué dans cette version
        /// </summary>
        public static XElement AddColumn ( XElement table, Int32 width, Int32 span, String style )
        {
            XElement Result;
            XElement column;
            XAttribute Attrib;

            Result = table;
            column = new XElement("col");

            Attrib = new XAttribute("width", width.ToString());
            column.Add(Attrib);

            Attrib = new XAttribute("span", span.ToString());
            column.Add(Attrib);

            Attrib = new XAttribute("style", style);
            column.Add(Attrib);

            Result.Add(column);

            return Result;
        } // endMethod: AddColumn

        /// <summary>
        /// Créer une balise Tr et l'ajouter à la table fournie en argument 
        /// </summary>
        public static XElement AddTr (XElement table, Int32 height, String style, ObservableCollection<XElement> tdCollection )
        {
            XElement Result;
            XElement tr;
            XAttribute Attrib;

            Result = table;

            tr = new XElement("tr");
            
            Attrib = new XAttribute("height", height.ToString());
            tr.Add(Attrib);

            Attrib = new XAttribute("style", style);
            tr.Add(Attrib);

            if (tdCollection != null)
            {
                if (tdCollection.Count > 0)
                {
                    foreach (var td in tdCollection)
                    {
                        tr.Add(td);
                    }
                }
            }

            Result.Add(tr);

            return Result;
        } // endMethod: AddTr

        /// <summary>
        /// Créer une balise td complète avec son contenu
        /// </summary>
        public static XElement CreateTd ( Int32 height, String classe, Int32 width, String style, String Value )
        {
            XElement Result;
            XAttribute Attrib;

            Result = new XElement("td");

            if (height > 0)
            {
                Attrib = new XAttribute("height", height.ToString());
                Result.Add(Attrib); 
            }

            if (classe != "")
            {
                Attrib = new XAttribute("class", classe);
                Result.Add(Attrib);
            }

            if (width > 0)
            {
                Attrib = new XAttribute("width", width.ToString());
                Result.Add(Attrib);
            }

            if (style != "")
            {
                Attrib = new XAttribute("style", style);
                Result.Add(Attrib);
            }

            if (Value.Length > 0 && Value.Substring(0, 1) == "<")
            {
                Result.Add(XElement.Parse(Value));
            }
            else
            {
                Result.Add(new XText(Value));
            }
            
            return Result;
        } // endMethod: CreateTd
    }
}
