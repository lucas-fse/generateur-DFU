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
        public void SaveQRQC(string _Id, DateTime? _DateOuverture, DateTime? _DateCloture,DateTime? _DateSuivis, string _Participants, 
                             string _Pilote, string _Origine, Dictionary<string, string> _SevenQuestion, string _DescriptionProcess, 
                             List<ActionImmediate> _ActionsImmediat, List<string>_Occurrence, List<string> _NonDetection, 
                             List<SolutionDurable> _Solutions,string _image)
        {
            int index;
            QRQC result = null;
            try { index = Convert.ToInt32(_Id); } catch { index = -1; }


            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            List<QRQC> listQrqc = pEGASE_PROD2Entities.QRQC.Include("ACTIONIMMEDIATE").Include("QUESTIONS_QRQC").Include("SOLUTION_DURABLE").Include("NON_DETECTION").Include("OCCURRENCE").Where(i => i.ID.Equals(index)).ToList();
            if (listQrqc.Count() > 0)
            {  result = listQrqc.First(); }
            else if (index == -1)
            {
                result = new QRQC();
                pEGASE_PROD2Entities.QRQC.Add(result);
            }
            
            if (result!=null)
            {
               
                if (_DateOuverture != null) { result.DateOuverture = (DateTime)_DateOuverture; } else { result.DateOuverture = DateTime.Now; }
                result.DateCloture = _DateCloture;
                result.DateSuivis = _DateSuivis;
                result.DescriptionProcess = _DescriptionProcess;
                result.Participants = _Participants;
                result.Pilote = _Pilote;
                result.Origine = _Origine;

                QUESTIONS_QRQC Questions;
                if (result.QUESTIONS_QRQC != null && result.QUESTIONS_QRQC.Count > 0)
                {
                    Questions = result.QUESTIONS_QRQC.ToList().First();
                }
                else
                {
                    Questions = new QUESTIONS_QRQC();
                    Questions.ID_QRQC = result.ID;
                    pEGASE_PROD2Entities.QUESTIONS_QRQC.Add(Questions);
                }                
                foreach (var question in _SevenQuestion)
                {
                    switch(question.Key)
                    {
                        case "Q1":
                            Questions.Q1 = question.Value;
                            break;
                        case "Q2":
                            Questions.Q2 = question.Value;
                            break;
                        case "Q3":
                            Questions.Q3 = question.Value;
                            break;
                        case "Q4":
                            Questions.Q4 = question.Value;
                            break;
                        case "Q5":
                            Questions.Q5 = question.Value;
                            break;
                        case "Q6":
                            Questions.Q6 = question.Value;
                            break;
                        case "Q7":
                            Questions.Q7 = question.Value;
                            break;
                    }
                }
                List<ACTIONIMMEDIATE> actionImmediates; 
                if (result.ACTIONIMMEDIATE != null && result.ACTIONIMMEDIATE.Count > 0)
                {
                    actionImmediates = result.ACTIONIMMEDIATE.OrderBy(p=>p.ID).ToList();                    
                }
                else
                {
                    actionImmediates = new List<ACTIONIMMEDIATE>();
                }
                int cpt = 0;
                foreach(var action in _ActionsImmediat)
                {
                    ACTIONIMMEDIATE ai;
                    if (actionImmediates.Count>cpt)
                    {
                        ai = actionImmediates[cpt];
                    }
                    else
                    {
                        ai = new ACTIONIMMEDIATE();
                        ai.ID_QRQC = result.ID;
                        result.ACTIONIMMEDIATE.Add(ai);
                    }
                    ai.Pilote = action.Pilote;
                    ai.Status = action.Statut;
                    ai.ActionPourClient = action.ActionPourClient;
                    ai.Delai =  action.Delai;
                    cpt++;
                }
                List<OCCURRENCE> occurences;
                if (result.OCCURRENCE != null && result.OCCURRENCE.Count > 0)
                {
                    occurences = result.OCCURRENCE.OrderBy(p => p.ID).ToList();
                }
                else
                {
                    occurences = new List<OCCURRENCE>();
                }
                cpt = 0;
                foreach (var occurence in _Occurrence)
                {
                    OCCURRENCE ai;
                    if (occurences.Count > cpt)
                    {
                        ai = occurences[cpt];
                    }
                    else
                    {
                        ai = new OCCURRENCE();
                        ai.ID_QRQC = result.ID;
                        result.OCCURRENCE.Add(ai);
                    }
                    ai.Item = occurence;
                    cpt++;
                }
                List<NON_DETECTION> nonDetections;
                if (result.NON_DETECTION != null && result.NON_DETECTION.Count > 0)
                {
                    nonDetections = result.NON_DETECTION.OrderBy(p => p.ID).ToList();
                }
                else
                {
                    nonDetections = new List<NON_DETECTION>();
                }
                cpt = 0;
                foreach (var nondetection in _NonDetection)
                {
                    NON_DETECTION ai;
                    if (nonDetections.Count > cpt)
                    {
                        ai = nonDetections[cpt];
                    }
                    else
                    {
                        ai = new NON_DETECTION();
                        ai.ID_QRQC = result.ID;
                        result.NON_DETECTION.Add(ai);
                    }
                    ai.item = nondetection;
                    cpt++;
                }
                List<SOLUTION_DURABLE> solutionDurables;
                if (result.SOLUTION_DURABLE != null && result.SOLUTION_DURABLE.Count > 0)
                {
                    solutionDurables = result.SOLUTION_DURABLE.OrderBy(p => p.ID).ToList();
                }
                else
                {
                    solutionDurables = new List<SOLUTION_DURABLE>();
                }
                cpt = 0;
                foreach (var solution in _Solutions)
                {
                    SOLUTION_DURABLE ai;
                    if (solutionDurables.Count > cpt)
                    {
                        ai = solutionDurables[cpt];
                    }
                    else
                    {
                        ai = new SOLUTION_DURABLE();
                        ai.ID_QRQC = result.ID;
                        result.SOLUTION_DURABLE.Add(ai);
                    }
                    ai.NmrItem = solution.item;
                    ai.ActionAmelioration = solution.ActionAmelioration;
                    ai.Pilote = solution.Pilote;
                    ai.Delai = solution.Delai;
                    ai.Statut = solution.Statut;
                    cpt++;
                }
                if (String.IsNullOrWhiteSpace(result.UrlImage))
                {
                    result.UrlImage =  DateTime.Now.Ticks.ToString();
                }
                string path = @"ImageQRQC\" + result.UrlImage.Trim() + ".img";
                //path = "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\ImageQRQC\\" + result.UrlImage.Trim() + ".img";
                path = "C:\\tmp\\ImageQRQC\\" + result.UrlImage.Trim() + ".img";
                int len = _image.Length;
                using (FileStream fs = File.OpenWrite(path))
                {
                    fs.SetLength(0);
                    Byte[] info =
                        new UTF8Encoding(true).GetBytes(_image);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    
                }
                pEGASE_PROD2Entities.SaveChanges();
            }
            else
            {

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
   