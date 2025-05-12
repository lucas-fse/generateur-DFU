using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace GenerateurDFUSafir.Models
{
    public class InfoQRQC
    {
        public string MaxDateActionSecu
        {
            get
            {
                return DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            }
        }
        public string MaxDateSolDurable
        {
            get
            {
                return DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            }
        }
        public long Index { get; set; }
        public Dictionary<string, string> Questions = new Dictionary<string, string>() { { "Q1", "Quoi" }, { "Q2", "Qui" }, { "Q3", "Où" }, { "Q4", "Quand" }, { "Q5", "Comment" }, { "Q6", "Combien" }, { "Q7", "Pourquoi" } };
        public int? IdQrqc { get; set; }
        public DateTime DateOuverture { get; set; }
        public string DateOuvertureString
        {
            get
            {
                return DateOuverture.ToString("yyyy-MM-dd");
            }
        }
        public DateTime? DateCloture { get; set; }
        public string DateClotureString
        {
            get
            {
                if (DateCloture != null)
                {
                    return ((DateTime)DateCloture).ToString("yyyy-MM-dd");
                }
                else
                {
                    return "";
                }
            }
        }
        public DateTime? DateSuivis { get; set; }
        public string DateSuivisString
        {
            get
            {
                if (DateSuivis != null)
                {
                    return ((DateTime)DateSuivis).ToString("yyyy-MM-dd");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Participants { get; set; }
        public string Pilote { get; set; }
        public string Origine { get; set; }

        public string MaxDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public Dictionary<string, string> SevenQuestion { get; set; }
        public string DescriptionProcess { get; set; }
        public List<ActionImmediate> ActionsImmediat { get; set; }
        public List<string> Occurrence { get; set; }
        public List<string> NonDetection { get; set; }
        public List<SolutionDurable> Solutions{get; set;} 
        
        public string Image { get; set; }

        public void UpdateInfoQRQC(int? index)
        {
            if (index != null)
            {
                IdQrqc = index;
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
                List<QRQC> listQrqc = pEGASE_PROD2Entities.QRQC.Include("ACTIONIMMEDIATE").Include("QUESTIONS_QRQC").Include("SOLUTION_DURABLE").Include("NON_DETECTION").Include("OCCURRENCE").Where(i => i.ID==index).ToList();
                if (listQrqc.Count()>0)
                {
                    QRQC result = listQrqc.First();
                    this.Index = result.ID;
                    this.DateOuverture = result.DateOuverture;
                    this.DateCloture = result.DateCloture;
                    this.DateSuivis = result.DateSuivis;
                    this.Pilote = result.Pilote.Trim();
                    this.Participants = result.Participants.Trim();
                    this.Origine = result.Origine.Trim();
                    

                    this.DescriptionProcess = result.DescriptionProcess;
                    this.SevenQuestion = new Dictionary<string, string>();
                    int cpt = 1;
                    if (result.QUESTIONS_QRQC!= null && result.QUESTIONS_QRQC.Count()>0)
                    {
                        QUESTIONS_QRQC tmp = result.QUESTIONS_QRQC.First();
                        SevenQuestion.Add("Q1", tmp.Q1.Trim());
                        SevenQuestion.Add("Q2", tmp.Q2.Trim());
                        SevenQuestion.Add("Q3", tmp.Q3.Trim());
                        SevenQuestion.Add("Q4", tmp.Q4.Trim());
                        SevenQuestion.Add("Q5", tmp.Q5.Trim());
                        SevenQuestion.Add("Q6" , tmp.Q6.Trim());
                        SevenQuestion.Add("Q7" , tmp.Q7.Trim());
                    }
                    else
                    {
                        for (int i=1; i<8;i++)
                        {
                            SevenQuestion.Add("Q" + i.ToString(),""); 
                        }
                    }
                    this.ActionsImmediat = new List<ActionImmediate>();
                    foreach (var action in result.ACTIONIMMEDIATE.OrderBy(i => i.ID))
                    {   
                        ActionsImmediat.Add(new ActionImmediate(action.ActionPourClient.Trim(), action.Pilote.Trim(), action.Delai, action.Status.Trim())); 
                    } 
                                       
                    
                    this.Occurrence = new List<string>();
                    foreach (var action in result.OCCURRENCE.OrderBy(i=>i.ID))
                    {
                        Occurrence.Add(action.Item.Trim());
                    }

                    this.NonDetection = new List<string>();
                    foreach (var action in result.NON_DETECTION.OrderBy(i => i.ID))
                    {
                        NonDetection.Add(action.item.Trim());
                    }
                    this.Solutions = new List<SolutionDurable>();


                    foreach (var action in result.SOLUTION_DURABLE.OrderBy(i => i.ID))
                    {

                        Solutions.Add(new SolutionDurable(action.NmrItem.Trim(), action.ActionAmelioration.Trim(), action.Pilote.Trim(), action.Delai, action.Statut.Trim()));
                    }
                    if (!string.IsNullOrWhiteSpace(result.UrlImage.Trim()))
                    {
                        string path = @"ImageQRQC\" + result.UrlImage.Trim() + ".img";
                        path = "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\ImageQRQC\\" + result.UrlImage.Trim() + ".img";
                        path = "C:\\tmp\\ImageQRQC\\" + result.UrlImage.Trim() + ".img";
                        using (FileStream fs = File.OpenRead(path))
                        {
                            byte[] b = new byte[1024];
                            UTF8Encoding temp = new UTF8Encoding(true);
                            Image = "";
                            int lenght = 0;
                            while (true)
                            {
                                lenght = fs.Read(b, 0, b.Length);
                                if (lenght == 0) break;
                                Image = Image + temp.GetString(b,0, lenght);
                                
                            }
                        }
                        int len = Image.Length;
                    }
                    pEGASE_PROD2Entities.Dispose();
                }
            }
            else
            {
                this.DateOuverture = DateTime.Now;
                this.DateCloture = null;
                this.DateSuivis = null;
                this.SevenQuestion = new Dictionary<string, string>();
                foreach(var q in Questions)
                {
                    SevenQuestion.Add(q.Key, "");
                }
                this.ActionsImmediat = new List<ActionImmediate>();
                this.Occurrence = new List<string>();
                this.NonDetection = new List<string>();
                this.Solutions = new List<SolutionDurable>();

                
                for (int i = 0; i < 7; i++)
                {
                    ActionsImmediat.Add(new ActionImmediate("", "", null, ""));
                    Occurrence.Add("");
                    NonDetection.Add("");
                    Solutions.Add(new SolutionDurable((i+1).ToString(), "", "", null, ""));
                }
            }
        }
        public void SaveQRQC(string _Id, DateTime? _DateOuverture, DateTime? _DateCloture, DateTime? _DateSuivis, string _Participants,
                     string _Pilote, string _Origine, Dictionary<string, string> _SevenQuestion, string _DescriptionProcess,
                     List<ActionImmediate> _ActionsImmediat, List<string> _Occurrence, List<string> _NonDetection,
                     List<SolutionDurable> _Solutions, string _image)
        {
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            QRQC result = null;
            int index;

            try
            {
                index = Convert.ToInt32(_Id);
            }
            catch
            {
                index = -1;
            }

            try
            {
                var listQrqc = pEGASE_PROD2Entities.QRQC
                    .Include("ACTIONIMMEDIATE")
                    .Include("QUESTIONS_QRQC")
                    .Include("SOLUTION_DURABLE")
                    .Include("NON_DETECTION")
                    .Include("OCCURRENCE")
                    .Where(i => i.ID.Equals(index)).ToList();

                result = listQrqc.FirstOrDefault();

                if (result == null && index == -1)
                {
                    result = new QRQC();
                    pEGASE_PROD2Entities.QRQC.Add(result);
                }

                if (result == null) throw new Exception("Impossible d’instancier ou de récupérer l’objet QRQC.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'initialisation du QRQC : " + ex.Message, ex);
            }

            try
            {
                result.DateOuverture = _DateOuverture ?? DateTime.Now;
                result.DateCloture = _DateCloture;
                result.DateSuivis = _DateSuivis;
                result.DescriptionProcess = _DescriptionProcess;
                result.Participants = _Participants;
                result.Pilote = _Pilote;
                result.Origine = _Origine;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l’affectation des champs principaux du QRQC : " + ex.Message, ex);
            }

            try
            {
                QUESTIONS_QRQC Questions = result.QUESTIONS_QRQC?.FirstOrDefault() ?? new QUESTIONS_QRQC { ID_QRQC = result.ID };
                if (Questions.ID == 0) pEGASE_PROD2Entities.QUESTIONS_QRQC.Add(Questions);

                foreach (var question in _SevenQuestion)
                {
                    switch (question.Key)
                    {
                        case "Q1": Questions.Q1 = question.Value; break;
                        case "Q2": Questions.Q2 = question.Value; break;
                        case "Q3": Questions.Q3 = question.Value; break;
                        case "Q4": Questions.Q4 = question.Value; break;
                        case "Q5": Questions.Q5 = question.Value; break;
                        case "Q6": Questions.Q6 = question.Value; break;
                        case "Q7": Questions.Q7 = question.Value; break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des 7 questions : " + ex.Message, ex);
            }

            try
            {
                var existingActions = result.ACTIONIMMEDIATE?.OrderBy(p => p.ID).ToList() ?? new List<ACTIONIMMEDIATE>();
                for (int i = 0; i < _ActionsImmediat.Count; i++)
                {
                    var ai = (i < existingActions.Count) ? existingActions[i] : new ACTIONIMMEDIATE { ID_QRQC = result.ID };
                    if (i >= existingActions.Count) result.ACTIONIMMEDIATE.Add(ai);

                    ai.Pilote = _ActionsImmediat[i].Pilote;
                    ai.Status = _ActionsImmediat[i].Statut;
                    ai.ActionPourClient = _ActionsImmediat[i].ActionPourClient;
                    ai.Delai = _ActionsImmediat[i].Delai;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des actions immédiates : " + ex.Message, ex);
            }

            try
            {
                var occurences = result.OCCURRENCE?.OrderBy(p => p.ID).ToList() ?? new List<OCCURRENCE>();
                for (int i = 0; i < _Occurrence.Count; i++)
                {
                    var o = (i < occurences.Count) ? occurences[i] : new OCCURRENCE { ID_QRQC = result.ID };
                    if (i >= occurences.Count) result.OCCURRENCE.Add(o);
                    o.Item = _Occurrence[i];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des occurrences : " + ex.Message, ex);
            }

            try
            {
                var nonDetections = result.NON_DETECTION?.OrderBy(p => p.ID).ToList() ?? new List<NON_DETECTION>();
                for (int i = 0; i < _NonDetection.Count; i++)
                {
                    var nd = (i < nonDetections.Count) ? nonDetections[i] : new NON_DETECTION { ID_QRQC = result.ID };
                    if (i >= nonDetections.Count) result.NON_DETECTION.Add(nd);
                    nd.item = _NonDetection[i];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des non-détections : " + ex.Message, ex);
            }

            try
            {
                var solutions = result.SOLUTION_DURABLE?.OrderBy(p => p.ID).ToList() ?? new List<SOLUTION_DURABLE>();
                for (int i = 0; i < _Solutions.Count; i++)
                {
                    var s = (i < solutions.Count) ? solutions[i] : new SOLUTION_DURABLE { ID_QRQC = result.ID };
                    if (i >= solutions.Count) result.SOLUTION_DURABLE.Add(s);

                    s.NmrItem = _Solutions[i].item;
                    s.ActionAmelioration = _Solutions[i].ActionAmelioration;
                    s.Pilote = _Solutions[i].Pilote;
                    s.Delai = _Solutions[i].Delai;
                    s.Statut = _Solutions[i].Statut;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des solutions durables : " + ex.Message, ex);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(result.UrlImage))
                {
                    result.UrlImage = DateTime.Now.Ticks.ToString();
                }
                string path = @"C:\tmp\ImageQRQC\" + result.UrlImage.Trim() + ".img";

                using (FileStream fs = File.OpenWrite(path))
                {
                    fs.SetLength(0);
                    byte[] info = new UTF8Encoding(true).GetBytes(_image);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l’écriture de l’image : " + ex.Message, ex);
            }

            try
            {
                pEGASE_PROD2Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la sauvegarde dans la base de données : " + ex.Message, ex);
            }
        }

    }

    public class ActionImmediate
    {
        public string ActionPourClient { get; set; }
        public string Pilote { get; set; }
        public DateTime? Delai { get; set; } // int represente un nombre de jour
        public string DelaiString
        {
            get
            {
                if (Delai != null)
                {
                    return ((DateTime)Delai).ToString("yyyy-MM-dd"); 
                }
                else
                {
                    return null;
                }
            }
        }

        public string Statut { get; set; } // true si fait false sinon

        public ActionImmediate(string _ActionPourClient, string _Pilote, DateTime? _Delai, string _Statut)
        {
            ActionPourClient = _ActionPourClient.Trim();
            Pilote = _Pilote.Trim();
            Delai = _Delai;
            Statut = _Statut.Trim();
        }
    }

    public class SolutionDurable
    {
        public string item { get; set; }
        public string ActionAmelioration { get; set; }
        public string Pilote { get; set; }
        public DateTime? Delai { get; set; }
        public string DelaiString
        {
            get
            {
                if (Delai != null)
                {
                    return ((DateTime)Delai).ToString("yyyy-MM-dd");
                }
                else
                {
                    return null;
                }
            }
        }
        public string Statut { get; set; }

        public SolutionDurable(string _item, string _ActionAmelioration, string _Pilote, DateTime? _Delai, string _Statut)
        {
            item = _item;
            ActionAmelioration = _ActionAmelioration.Trim();
            Pilote= _Pilote.Trim();
            Delai= _Delai;
            Statut= _Statut.Trim();
        }

    }
}
   