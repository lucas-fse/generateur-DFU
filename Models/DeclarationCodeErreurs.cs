//using GenerateurDFUSafir.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace GenerateurDFUSafir.Models
{
    public class DeclarationCodeErreurs
    {
        public Dictionary<int,CodeGroupe> CodeErreurs { get; set; }

        public  DeclarationCodeErreurs()
        {
            CodeErreurs = new Dictionary<int, CodeGroupe>();
            string path = HttpContext.Current.Request.MapPath(".");
            path = "";
            string file = path + "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\data\\CodeErreurs.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            foreach (XmlNode xmlnode in xmlDoc.DocumentElement)
            {
                string groupe = xmlnode.Attributes["libelle"].Value;
                CodeGroupe newsousgroupe = new CodeGroupe();

                foreach (XmlNode xmlchil in xmlnode.ChildNodes)
                {
                    string sousgroupe = xmlchil.Attributes["libelle"].Value;
                    string sousgroupecouleur = xmlchil.Attributes["couleur"].Value;
                    foreach (XmlNode xmlchil2 in xmlchil.ChildNodes)
                    {
                        CodeGroupe codegroupe = new CodeGroupe();
                        codegroupe.Code = Convert.ToInt32(xmlchil2.Attributes["valeur"].Value);
                        codegroupe.Erreur = xmlchil2.Attributes["libelle"].Value;
                        codegroupe.ischecked = false;
                        codegroupe.Couleur = sousgroupecouleur;
                        codegroupe.SouSGroupe = sousgroupe;
                        codegroupe.Groupe = groupe;
                        CodeErreurs.Add(codegroupe.Code, codegroupe);
                    }
                }
            }
        }

    }
}