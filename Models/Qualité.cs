using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace GenerateurDFUSafir.Models
{
    public class Qualite
    {
        public List<Processus> ProcessusTop;
        public List<Processus> ProcessusMiddle;
        public List<Processus> ProcessusBottom;
        public String RefVersion { get; set; }
        public String Nom { get; set; }

        public Qualite()
        {
            ProcessusTop = new List<Processus>();
            ProcessusMiddle = new List<Processus>();
            ProcessusBottom = new List<Processus>();
            RefVersion = "";


            string path = HttpContext.Current.Request.MapPath(".");
            path = "";
            string file = path + "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\data\\Referentielqualite.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            
            foreach (XmlNode xmlqualite in xmlDoc.DocumentElement)
            {
                if (xmlqualite.NodeType is XmlNodeType.Element)
                {
                    Nom = xmlqualite.Attributes["valeur"].Value;
                    RefVersion = xmlqualite.Attributes["RefVersion"].Value;
                    foreach (XmlNode xmlProcessus in xmlqualite.ChildNodes)
                    {
                        string typeprocessus = xmlProcessus.Attributes["type"].Value;
                        Processus process = new Processus();
                        if (typeprocessus == "ProcessusTop") { ProcessusTop.Add(process); }
                        else if (typeprocessus == "ProcessusMiddle") { ProcessusMiddle.Add(process); }
                        else if (typeprocessus == "ProcessusBottom") { ProcessusBottom.Add(process); }

                        process.NomProcessus = xmlProcessus.Attributes["NomProcessus"].Value;
                        process.RefProcessus = xmlProcessus.Attributes["RefProcessus"].Value;
                        process.IDProcessus = xmlProcessus.Attributes["idprocess"].Value;
                        process.BottomBox = new List<Boite>();
                        process.DataInput = new List<string>();
                        process.DataOutput = new List<string>();
                        process.ListActivite = new List<Activite>();
                        process.Mission = xmlProcessus.Attributes["mission"].Value;
                        foreach (XmlNode xmlActivite in xmlProcessus.ChildNodes)
                        {
                            
                            if (xmlActivite.Name == "Activite")
                            {
                                Activite activite = new Activite();
                                process.ListActivite.Add(activite);
                                activite.Nom = xmlActivite.Attributes["Libelle"].Value;
                                activite.IDActivite = xmlActivite.Attributes["idprocess"].Value;
                                activite.ListProcedure = new List<Docref>();
                                activite.ListModeOpe = new List<Docref>();
                                activite.ListFormulaire = new List<Docref>();
                                activite.ListDocument = new List<Docref>();
                                foreach (XmlNode xmlfile in xmlActivite.ChildNodes)
                                {                                    
                                    string type = xmlfile.Attributes["type"].Value;
                                    string nom = xmlfile.Attributes["nom"].Value;
                                    string chemin = xmlfile.Attributes["chemin"].Value;
                                    string date = xmlfile.Attributes["Date"].Value;
                                    
                                    Docref doc = new Docref();
                                    try
                                    {
                                        doc.date = DateTime.Parse(date);
                                    }
                                    catch
                                    { doc.date = DateTime.Parse("01/01/2021"); }
                                    doc.Directory = chemin;
                                    doc.Directory = doc.Directory.Replace(@"\\Jaysvrfiles\societe\QUALITE", @"\pdf");
                                    doc.Directory= doc.Directory.Replace(@"\",@"/");
                                    //doc.Directory = doc.Directory.Replace("ô", "%C3%B4");
                                    doc.Directory = doc.Directory.Replace(" ", "%20");
                                    doc.Directory = doc.Directory.Replace("é", "%C3%A9");
                                    // doc.Directory = doc.Directory.Replace("è", "%C3%A8");
                                    doc.nom = nom;
                                    if (chemin.ToUpper().EndsWith(".PDF"))
                                    {
                                        doc.TypePdf = true;
                                    }
                                    else
                                    {
                                        doc.TypePdf = false;
                                    }
                                    if (!string.IsNullOrWhiteSpace(doc.Directory) && !string.IsNullOrWhiteSpace(doc.nom))
                                    {
                                        if (type == "Procedure") { activite.ListProcedure.Add(doc); }
                                        else if (type == "Modeope") { activite.ListModeOpe.Add(doc); }
                                        else if (type == "Formulaire") { activite.ListFormulaire.Add(doc); }
                                        else if (type == "Document") { activite.ListDocument.Add(doc); }
                                    }
                                }
                            }
                            else if (xmlActivite.Name == "DataInput")
                            {
                                process.nomDataInput = xmlActivite.Attributes["nom"].Value;
                                foreach (XmlNode item in xmlActivite.ChildNodes)
                                {
                                    string nomitem = item.Attributes["nom"].Value;
                                    process.DataInput.Add(nomitem);
                                }
                            }
                            else if (xmlActivite.Name == "DataOutput")
                            {
                                process.nomDataOutput = xmlActivite.Attributes["nom"].Value;
                                foreach (XmlNode item in xmlActivite.ChildNodes)
                                {
                                    string nomitem = item.Attributes["nom"].Value;
                                    process.DataOutput.Add(nomitem);
                                }
                            }
                            else if (xmlActivite.Name == "BotommBox")
                            {
                                Boite boite = new Boite();
                                boite.ListAction = new List<string>();
                                boite.Nom = xmlActivite.Attributes["nom"].Value;
                                process.BottomBox.Add(boite);
                                foreach (XmlNode item in xmlActivite.ChildNodes)
                                {
                                    string nomitem = item.Attributes["nom"].Value;
                                    boite.ListAction.Add(nomitem);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    public class Processus
    {
        public string IDProcessus;
        public string nomDataInput;
        public string nomDataOutput;
        public string nomBottomBox;
        public List<string> DataInput;
        public List<string> DataOutput;
        public List<Boite> BottomBox;
        public string NomProcessus;
        public string RefProcessus;
        public List<Activite> ListActivite;
        public string Mission;
        
    }

    public class Boite
    {
        public string Nom;
        public List<string> ListAction;
    }
    public class Activite
    {
        public string IDActivite;
        public string Nom;
        public List<Docref> ListProcedure;
        public List<Docref> ListModeOpe;
        public List<Docref> ListFormulaire;
        public List<Docref> ListDocument;
    }
    public class Docref
    {
        public string nom;
        public string Directory;
        public DateTime date;
        public Boolean TypePdf;
    }
}