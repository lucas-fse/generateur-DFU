using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public class InfoAIC2
    {

        public string NomDuPole { get; set; }
        string[] DEFPOSTEBIDIR = new string[] { "C6/00", "P0/01", "P1/01", "P1/02", "P1/03", "P1/99", "P2/01", "P3/01", "A5/01" };
        string[] DEFPOSTEMONO = new string[] { "A1/00", "A2/00", "A3/00", "A4/00", "A6/00", "A5/00", "C1/00", "C5/00", "C3/00", "C3/01", "C3/02", "C3/99", "C4/00" };
        string[] DEFPOSTETEST = new string[] { "A5/00", "A5/01" };
        public Dictionary<long,String> Communication { get; set; }
        public Dictionary<int, InfosAic2Day> infosDeLaSemaine { get; set; }
        public List<AIC_EQUIPE> FormRow { get; set; }
        public int? PoleDemande = null;
        public string DateDuJourString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public void UpdateInfoAIC2()
        {
            
            infosDeLaSemaine = new Dictionary<int, InfosAic2Day>();
            List<OrdreFabrication> InfoProduction = new List<OrdreFabrication>();
            try
            {
                InfoProduction = InfoAllProduction.InfoAllOfProduction();
            }
            catch (Exception e)
            {

            }
            //1  En attente
            //2  A l'étude  
            //3  Edité
            //4  En cours
            //5  Soldé
            //6  Prix de revient calculé

            List<OrdreFabrication> InfoProductionBidir = InfoAllProduction.InfoAllOfProduction(DateTime.Now.AddDays(-8)).Where(p => ( p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && DEFPOSTEBIDIR.Contains(p.EXTWST_0)).ToList();
            List<OrdreFabrication> InfoProductionMono = InfoAllProduction.InfoAllOfProduction(DateTime.Now.AddDays(-8)).Where(p => ( p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && DEFPOSTEMONO.Contains(p.EXTWST_0)).ToList();
            List<OrdreFabrication> InfoProductionBidirTest = InfoAllProduction.InfoAllOfProduction(DateTime.Now.AddDays(-8)).Where(p => ( p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && DEFPOSTETEST.Contains(p.EXTWST_0)).ToList();

            DateTime startDate = DateTime.Now;
            DateTime jourcalcule = DateTime.Now.AddDays(-1);
            while (jourcalcule.DayOfWeek == DayOfWeek.Saturday || jourcalcule.DayOfWeek == DayOfWeek.Sunday)
            {
                jourcalcule = jourcalcule.AddDays(-1);
            }
            for (int i = 0; i < 7; i++)
            {
                InfosAic2Day newday = new InfosAic2Day();
                DayOfWeek NmrDayofweek = jourcalcule.DayOfWeek;

                newday.NbBidir = InfoProductionBidir.Where(t => t.OBJDAT_0 < startDate && t.OBJDAT_0 >= jourcalcule).Count();
                newday.NbMono = InfoProductionMono.Where(t => t.OBJDAT_0 < startDate && t.OBJDAT_0 >= jourcalcule).Count();
                newday.NbTest = InfoProductionBidirTest.Where(t => t.OBJDAT_0 < startDate && t.OBJDAT_0>= jourcalcule).Count();
                infosDeLaSemaine.Add((int)jourcalcule.DayOfWeek, newday);

                startDate = jourcalcule;
                jourcalcule = jourcalcule.AddDays(-1);
                while (jourcalcule.DayOfWeek == DayOfWeek.Saturday || jourcalcule.DayOfWeek == DayOfWeek.Sunday)
                {
                    i++;
                    jourcalcule = jourcalcule.AddDays(-1);
                }
            }

            // communication global equipe production
            Communication = new Dictionary<long,string>();
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            List<AIC_PROD> tmp =  pEGASE_PROD2Entities2.AIC_PROD.Where(t => (t.DateDeFin > jourcalcule|| t.DateDeFin==null)&& t.Status==0).ToList();
            foreach(var l in tmp)
            {
                Communication.Add(l.ID,l.Description);
            }

            // communication par pole
            FormRow = new List<AIC_EQUIPE>();
            if (PoleDemande != null)
            {
                NomDuPole = pEGASE_PROD2Entities2.AIC_POLE.Where(p => p.ID == (int)PoleDemande).First().POLE;
                FormRow = pEGASE_PROD2Entities2.AIC_EQUIPE.Where(p => p.Pole == (int)PoleDemande && p.Status==0).ToList();
            }
        }
        public void Addligne(Dictionary<string, string> listligne)
        {
            int id = Convert.ToInt32(listligne.Where(p => p.Key.Contains("ID+")).First().Value);
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();

            try
            {
                Dictionary<string, string> commentaire = listligne.Where(p => p.Key.Contains("INPUTCOM")).ToDictionary(i => i.Key, i => i.Value);
                AIC_PROD NewComm = new AIC_PROD();
                NewComm.Pole = (short)id;
                NewComm.DateDeFin = null;
                foreach (var item in commentaire)
                {
                    switch (item.Key)
                    {
                        case "INPUTCOM":
                            NewComm.Description = item.Value;
                            break;
                    }
                }
                if (!(String.IsNullOrWhiteSpace(NewComm.Description)))
                {
                    pEGASE_PROD2Entities2.AIC_PROD.Add(NewComm);
                    pEGASE_PROD2Entities2.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout du commentaire : " + ex.Message, ex);
            }

            try
            {
                Dictionary<string, string> NewInput = listligne.Where(p => p.Key.Contains("NEW")).ToDictionary(i => i.Key, i => i.Value);
                AIC_EQUIPE NewLigne = new AIC_EQUIPE();
                foreach (var item in NewInput)
                {
                    NewLigne.Pole = (short)id;
                    switch (item.Key)
                    {
                        case "NEWDATE":
                            try
                            {
                                NewLigne.Date = DateTime.Parse(item.Value);
                            }
                            catch { NewLigne.Date = DateTime.Now; }
                            break;
                        case "NEWDESCRIPTION":
                            NewLigne.Description = item.Value;
                            break;
                        case "NEWDESCRIPTIONBIS":
                            NewLigne.Description2 = item.Value;
                            break;
                        case "NEWACTEUR":
                            NewLigne.Acteur = item.Value;
                            break;
                        case "NEWDATERESOLUTION":
                            try
                            {
                                NewLigne.DateResolution = DateTime.Parse(item.Value);
                            }
                            catch { NewLigne.DateResolution = null; }
                            break;
                        case "NEWCHECKBOX":
                            if (item.Value.Contains("on"))
                            {
                                NewLigne.Status = 1;
                            }
                            else
                            {
                                NewLigne.Status = 0;
                            }
                            break;
                    }
                }
                if (!(String.IsNullOrWhiteSpace(NewLigne.Description) && String.IsNullOrWhiteSpace(NewLigne.Description2) && String.IsNullOrWhiteSpace(NewLigne.Acteur)))
                {
                    pEGASE_PROD2Entities2.AIC_EQUIPE.Add(NewLigne);
                    pEGASE_PROD2Entities2.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ajout d'une nouvelle ligne : " + ex.Message, ex);
            }

            Dictionary<string, string> UpdateInputCom = listligne.Where(p => p.Key.Contains("COMMUNICATION")).ToDictionary(i => i.Key, i => i.Value);
            Dictionary<long, AIC_PROD> ListToUpdateCom = new Dictionary<long, AIC_PROD>();
            Regex regexidcom = new Regex("^(COMMUNICATION|CHECKBOXCOMMUNICATION)([0-9]{1,7})");

            try
            {
                foreach (var item in UpdateInputCom)
                {
                    if (regexidcom.IsMatch(item.Key))
                    {
                        MatchCollection matches = regexidcom.Matches(item.Key);
                        long index = Convert.ToInt32(matches[0].Groups[2].Value);
                        string name = matches[0].Groups[1].Value;

                        AIC_PROD aic_prod = null;
                        ListToUpdateCom.TryGetValue(index, out aic_prod);
                        if (aic_prod == null) { aic_prod = new AIC_PROD(); ListToUpdateCom.Add(index, aic_prod); }
                        aic_prod.Pole = (short)id;
                        switch (name)
                        {
                            case "COMMUNICATION":
                                aic_prod.Description = item.Value;
                                break;
                            case "CHECKBOXCOMMUNICATION":
                                if (item.Value.Contains("on"))
                                {
                                    aic_prod.Status = 1;
                                }
                                else
                                {
                                    aic_prod.Status = 0;
                                }
                                break;
                        }
                        ListToUpdateCom[index] = aic_prod;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des communications : " + ex.Message, ex);
            }

            Dictionary<string, string> UpdateInput = listligne.Where(p => !p.Key.Contains("NEW")).ToDictionary(i => i.Key, i => i.Value);
            Dictionary<long, AIC_EQUIPE> ListToUpdate = new Dictionary<long, AIC_EQUIPE>();
            Regex regexidname = new Regex("^(LIGNE|DATE|DESCRIPTION|DESCRIPTIONBIS|ACTEUR|DATERESOLUTION|CHECKBOX)([0-9]{1,7})");

            try
            {
                foreach (var item in UpdateInput)
                {
                    if (regexidname.IsMatch(item.Key))
                    {
                        MatchCollection matches = regexidname.Matches(item.Key);
                        long index = Convert.ToInt64(matches[0].Groups[2].Value);
                        string name = matches[0].Groups[1].Value;

                        AIC_EQUIPE aic_equipe = null;
                        ListToUpdate.TryGetValue(index, out aic_equipe);
                        if (aic_equipe == null) { aic_equipe = new AIC_EQUIPE(); ListToUpdate.Add(index, aic_equipe); }
                        aic_equipe.Pole = (short)id;
                        switch (name)
                        {
                            case "LIGNE":
                                aic_equipe.ID = index;
                                break;
                            case "DATE":
                                try
                                {
                                    aic_equipe.Date = DateTime.Parse(item.Value);
                                }
                                catch { aic_equipe.Date = DateTime.Now; }
                                break;
                            case "DESCRIPTION":
                                aic_equipe.Description = item.Value;
                                break;
                            case "DESCRIPTIONBIS":
                                aic_equipe.Description2 = item.Value;
                                break;
                            case "ACTEUR":
                                aic_equipe.Acteur = item.Value;
                                break;
                            case "DATERESOLUTION":
                                try
                                {
                                    aic_equipe.DateResolution = DateTime.Parse(item.Value);
                                }
                                catch { aic_equipe.DateResolution = null; }
                                break;
                            case "CHECKBOX":
                                if (item.Value.Contains("on"))
                                {
                                    aic_equipe.Status = 1;
                                }
                                else
                                {
                                    aic_equipe.Status = 0;
                                }
                                break;
                        }
                        ListToUpdate[index] = aic_equipe;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement des lignes : " + ex.Message, ex);
            }

            try
            {
                foreach (var aic in ListToUpdateCom)
                {
                    if (pEGASE_PROD2Entities2.AIC_PROD.Where(i => i.ID == aic.Key).Count() > 0)
                    {
                        AIC_PROD update = pEGASE_PROD2Entities2.AIC_PROD.Where(i => i.ID == aic.Key).First();
                        update.Description = aic.Value.Description;
                        update.DateDeFin = aic.Value.DateDeFin;
                        update.Status = aic.Value.Status;
                        update.Pole = aic.Value.Pole;
                    }
                }

                foreach (var aic in ListToUpdate)
                {
                    if (pEGASE_PROD2Entities2.AIC_EQUIPE.Where(i => i.ID == aic.Key).Count() > 0)
                    {
                        AIC_EQUIPE update = pEGASE_PROD2Entities2.AIC_EQUIPE.Where(i => i.ID == aic.Key).First();
                        update.Description = aic.Value.Description;
                        update.Description2 = aic.Value.Description2;
                        update.Date = aic.Value.Date;
                        update.DateResolution = aic.Value.DateResolution;
                        update.Status = aic.Value.Status;
                        update.Acteur = aic.Value.Acteur;
                        update.Pole = aic.Value.Pole;
                    }
                }

                pEGASE_PROD2Entities2.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la sauvegarde finale : " + ex.Message, ex);
            }
        }


    }
    public class InfosAic2Day /* contient les informations pour un jour */
    {

        public int NbBidir { get; set; }
        public int NbMono { get; set; }
        public int NbTest { get; set; }
        public string Idees { get; set; }
        public int NbAleas { get; set; }
        public int NbQrqc { get; set; }

        public int total
        {
            get
            {
                return NbBidir + NbMono;
            }
        }


    }
}