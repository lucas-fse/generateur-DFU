using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;
using System.Xml.Linq;

namespace JAY.DAL
{

    public class ProjectsFile
    {
        public ObservableCollection<ProjectFile> ProjetFileList { get; set; }

        public void SaveFileRecent()
        {
            XDocument RecentFile = new XDocument();
            XElement NiveauRoot = new XElement("INFO");
            XElement NiveauPrj = new XElement("PROJETS");

            // creation du nombre de MT
            foreach (ProjectFile prj in ProjetFileList)
            {
                XElement Niveauelement = new XElement("Element");// + i);
                
                XAttribute AttName = new XAttribute("NameProjet", prj.ProjectName);
                Niveauelement.Add(AttName);
                XAttribute AttChemin = new XAttribute("CheminProjet", prj.ProjectChemin);
                Niveauelement.Add(AttChemin);
                XAttribute AttMo = new XAttribute("MOType", prj.MOType);
                Niveauelement.Add(AttMo);
                XAttribute AttMt = new XAttribute("MTType", prj.MTType);
                Niveauelement.Add(AttMt);
                NiveauPrj.Add(Niveauelement);
            }
            NiveauRoot.Add(NiveauPrj);
            RecentFile.Add(NiveauRoot);
            try
            {
                string fileName = JAY.DefaultValues.Get().RecentOpenFile;
                Stream stream = File.Open(fileName, System.IO.FileMode.Create);
                
                RecentFile.Save(stream);
                stream.Close();

            }
            // En cas d'erreur...
            catch (Exception ex)
            {

            }
        }
        public void RemoveItemFromList(string chemin)
        {
            ObservableCollection<ProjectFile> copy = new ObservableCollection<ProjectFile>(ProjetFileList);
            foreach ( ProjectFile prj in copy)
            {
                if(!prj.ProjectChemin.EndsWith("\\"))
                        {
                    prj.ProjectChemin = prj.ProjectChemin + "\\";
                }
                if ( chemin.Equals(prj.ProjectChemin+prj.ProjectName))
                {
                    ProjetFileList.Remove(prj);
                }
            }
            SaveFileRecent();
        }

        public void AddItemToList(string nom, string chemin, string mo, string mt)
        {
            ProjectFile item = new ProjectFile();
            ObservableCollection<ProjectFile> copy = new ObservableCollection<ProjectFile>(ProjetFileList);
            bool find = false;
            if (!chemin.EndsWith("\\"))
            {
                chemin = chemin + "\\";
            }
            foreach (ProjectFile prj in copy)
            {
                if (prj.ProjectChemin.Equals(chemin) && prj.ProjectName.Equals(nom))
                {
                    find = true;
                    ProjetFileList.Remove(prj);
                    ProjetFileList.Insert(0, prj);
                }
            }
            if (!find)
            {
                item.ProjectChemin = chemin;
                item.ProjectName = nom;
                item.MOType = mo;
                item.MTType = mt;
                this.ProjetFileList.Insert(0, item);


                if (this.ProjetFileList.Count()> 15 )
                {
                    ObservableCollection<ProjectFile> copy2 = new ObservableCollection<ProjectFile>(ProjetFileList);
                    ProjetFileList.Clear();
                    int comp = 0;
                    foreach (var it in copy2)
                    {
                        if (comp<10)
                        {
                            ProjetFileList.Add(it);
                            comp++;
                        }
                    }
                }
                SaveFileRecent();
            }
            SaveFileRecent();
        }
        public  ProjectsFile()
        {
            // recherche du fichier de suivie
            XElement DocRoot = null;
            ObservableCollection<ProjectFile> projetFileList = new ObservableCollection<ProjectFile>();
            if (this.ProjetFileList == null)
            {
                if (File.Exists(DefaultValues.Get().RecentOpenFile))
                {
                    Stream historique = File.Open(DefaultValues.Get().RecentOpenFile, FileMode.Open);
                    if (historique != null)
                    {
                        XDocument document = XDocument.Load(historique);
                        DocRoot = document.Root;
                        IEnumerable<XElement> SectionsPROJETS = DocRoot.Descendants("PROJETS");
                        if (SectionsPROJETS != null && SectionsPROJETS.Count() > 0)
                        {
                            IEnumerable<XElement> SectionsElement = SectionsPROJETS.Descendants("Element");
                            for (int i = 0; i < SectionsElement.Count(); i++)
                            {

                                ProjectFile projet = new ProjectFile();
                                XElement element = SectionsElement.ElementAt(i);
                                projet.ProjectName = element.Attribute("NameProjet").Value;
                                projet.ProjectChemin = element.Attribute("CheminProjet").Value;
                                projet.MOType = element.Attribute("MOType").Value;
                                projet.MTType = element.Attribute("MTType").Value;
                                projetFileList.Add(projet);
                            }
                        }
                        else
                        {
                            // collection vide
                        }
                        historique.Close();
                    }
                }
                else
                {
                    // le fichier n'existe pas'
                }
            }
            this.ProjetFileList = projetFileList;
        }
    }
}
