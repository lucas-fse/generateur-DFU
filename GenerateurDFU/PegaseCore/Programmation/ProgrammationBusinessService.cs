using System;

namespace JAY.PegaseCore.Programmation
{
    /// <summary>
    /// Programmation BusinessService Class
    /// </summary>
    public class ProgrammationBusinessService
    {
        /// <summary>
        /// Un CRC est defini par
        /// *  - son @ de stockage (ou il se trouve)
        /// *  - l'adresse de debut du bloc a couvrir
        /// *  - la taille du bloc a couvrir
        /// </summary>
        private Hid.Hid portHid = null;

        /// <summary>
        /// Identifie le type de cible a programmer
        /// </summary>
        private enum EnumTypeCibleHid
        {
            /// <summary>
            /// The product key
            /// </summary>
            CIBLE_PRODUIT = 0,

            /// <summary>
            /// The sim target
            /// </summary>
            CIBLE_SIM = 1,
        }

        #region EcrireEepEtSim
        /// <summary>
        /// Recuperer @ des CRC32 dans le modele via le singleton
        /// </summary>
        /// <param name="codeSousBloc">The code sous bloc.</param>
        /// <param name="podeCrc32">The code CR C32.</param>
        /// <returns>
        /// The Variable
        /// </returns>
        //private VariableXml GetInfosCrc32(string codeSousBloc, string podeCrc32)
        //{
        //    VariableXml infosCrc = null;

        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        // recupere le bloc dans lequel rechercher le CRC32
        //        SousBlocXml sousBloc = FichePackModele.GetSousBlocByCode(null, codeSousBloc, EnumPartieXml.TECHNIQUE);

        //        if (sousBloc != null)
        //        {
        //            // recupere infos sur le CRC dans le bloc
        //            infosCrc = FichePackModele.GetVariableByCode(sousBloc, podeCrc32, EnumPartieXml.TECHNIQUE);
        //        }
        //    }
        //    catch (GToolBase.ApplicativeException aex)
        //    {
        //        Log.Error(string.Empty, aex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }

        //    return infosCrc;
        //}

        /// <summary>
        /// Ecrires the cible.
        /// </summary>
        /// <param name="typeCible">The type cible.</param>
        /// <param name="sousBloc">The sous bloc.</param>
        /// <returns>
        /// True if ok
        /// </returns>
        //private bool SendToTargetHid(EnumTypeCibleHid typeCible, SousBlocXml sousBloc)
        //{
        //    bool result = false;

        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        // Convertir sousBloc Composite en tableau de byte  
        //        ConversionBusinessService convService = new ConversionBusinessService();
        //        byte[] memory;
        //        long offset;
        //        long taille;
        //        convService.ConvertObjetTechniqueToByteArray(sousBloc, out memory, out offset, out taille);

        //        // CRC32
        //        // VariableXml infoCRC = this.GetInfosCrc32(sousBloc.Code, "CRC32");
        //        VariableXml infoCRC = FichePackModele.GetVariableByCode(sousBloc, "CRC32", EnumPartieXml.TECHNIQUE);

        //        // une variable 'CRC32' est trouvée dans le sous bloc
        //        if (infoCRC != null)
        //        {
        //            //// -------------------------------------------------------------
        //            //// calculer le crc sur les (taille - 4) derniers octets de memory
        //            //// stocker le crc dans les 4 premiers octets de memory
        //            //// -------------------------------------------------------------
        //            //// calculer CRC sur la taille du bloc - 4 octets 
        //            UInt32 crc = ByteHelper.CalculCrc32(memory, sizeof(UInt32), (UInt32)taille - sizeof(UInt32));

        //            // UInt32  -> byte array
        //            byte[] array = BitConverter.GetBytes(crc);

        //            // stocker le CRC en tete du sousbloc
        //            Buffer.BlockCopy(array, 0, memory, 0, sizeof(UInt32));
        //        }

        //        // transfert Hid vers les 2 eep cible)
        //        if (this.portHid != null)
        //        {
        //            switch (typeCible)
        //            {
        //                case EnumTypeCibleHid.CIBLE_SIM:
        //                    result = this.portHid.EcrireBloc(CIBLE_HID_e.CIBLE_SIM, memory, (uint)offset, (uint)taille);
        //                    break;

        //                // si c'est un produit : ecrire les 2 eep
        //                case EnumTypeCibleHid.CIBLE_PRODUIT:
        //                    result = this.portHid.EcrireBloc(CIBLE_HID_e.CIBLE_EEP_0, memory, (uint)offset, (uint)taille);
        //                    if (result)
        //                    {
        //                        result = this.portHid.EcrireBloc(CIBLE_HID_e.CIBLE_EEP_1, memory, (uint)offset, (uint)taille);
        //                    }

        //                    break;
        //            }
        //        }
        //    }
        //    catch (GToolBase.ApplicativeException aex)
        //    {
        //        Log.Error(string.Empty, aex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Ecrire la cible.
        /// </summary>
        /// <param name="composite">Le BlocXml ou SousBlocXml a ecrie .</param>
        /// <param name="idEtapeParametre">L'etape en cours.</param>
        /// <param name="cible">La cible parmi enumTypeCibleHid PRODUIT ou SIM .</param>
        /// <returns>
        /// True if ok
        /// </returns>
        //private bool EcrireCible(Composite composite, long idEtapeParametre, EnumTypeCibleHid cible)
        //{
        //    bool result = false;

        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        // Verifier qu'il s'agit d'un bloc ou d'un sous bloc
        //        if ((composite is BlocXml) || (composite is SousBlocXml))
        //        {
        //            string code_bloc = String.Empty;

        //            // si composite est un sous bloc : recuperer code du bloc parent 
        //            if (composite is SousBlocXml)
        //            {
        //                code_bloc = composite.ParentElement.Code;
        //            }
        //            else
        //            {
        //                // sinon c'est le code composite
        //                code_bloc = composite.Code;
        //            }

        //            // recuperer autorisation d'ecriture pour le bloc en fonction de l'étape
        //            ParametreTechniqueBusinessService ptbs = new ParametreTechniqueBusinessService();

        //            // REM : pb avec la base ?
        //            bool canWrite = ptbs.IsBlocWritable(code_bloc, idEtapeParametre);

        //            if (canWrite)
        //            {
        //                if (composite is BlocXml)
        //                {
        //                    // YVA protection de code
        //                    if (composite.Children != null)
        //                    {
        //                        foreach (SousBlocXml ssbloc in composite.Children)
        //                        {
        //                            if (ssbloc != null)
        //                            {
        //                                canWrite = ptbs.IsSousBlocWritable(ssbloc.Code, idEtapeParametre);

        //                                if (canWrite)
        //                                {
        //                                    result = this.SendToTargetHid(cible, ssbloc);

        //                                    if (!result)
        //                                    {
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // c'est un sous bloc
        //                    // JMC 05Mars2012- Bug cible result = this.SendToTargetHid(EnumTypeCibleHid.CIBLE_PRODUIT, (SousBlocXml)composite);
        //                    result = this.SendToTargetHid(cible, (SousBlocXml)composite);
        //                }
        //            }
        //            else
        //            {
        //                ////TODO : signaler ecriture non autoriser
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            //// TODO : signaler composite doit etre un bloc ou un sous bloc
        //            return false;
        //        }
        //    }
        //    catch (GToolBase.ApplicativeException aex)
        //    {
        //        Log.Error(string.Empty, aex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Connecters the cible.
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <returns>
        /// True if ok
        /// </returns>
        public bool ConnecterCible(ushort vid, ushort pid)
        {
            bool result = false;

            //Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

            //try
            //{
                if (this.portHid == null)
                {
                    this.portHid = new Hid.Hid();
                }

                result = this.portHid.Connecter(vid, pid);
            //}
            //catch (GToolBase.ApplicativeException aex)
            //{
            //    Log.Error(string.Empty, aex);
            //    throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(string.Empty, ex);
            //    throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
            //}
            //finally
            //{
            //    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name + "Cmd : " + result.ToString());
            //}

            return result;
        }

        /// <summary>
        /// Deconnecter la cible.
        /// </summary>
        public void DeconnecterCible()
        {
            //Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

            //try
            //{
                if (this.portHid != null)
                {
                    this.portHid.Deconnecter();
                    this.portHid = null;
                }
            //}
            //catch (GToolBase.ApplicativeException aex)
            //{
            //    Log.Error(string.Empty, aex);
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(string.Empty, ex);
            //    throw;
            //}
            //finally
            //{
            //    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
            //}
        }

        /// <summary>
        /// Ecrires the sim.
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="composite">The composite.</param>
        /// <param name="idEtapeParametre">The id etape parametre.</param>
        /// <returns>
        /// True if ok
        /// </returns>
        //public bool EcrireSim(ushort vid, ushort pid, Composite composite, long idEtapeParametre)
        //{
        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        bool result = false;

        //        // connexion
        //        result = this.ConnecterCible(vid, pid);

        //        // Ecriture
        //        if (result)
        //        {
        //            ParametreTechniqueBusinessService ptbs = new ParametreTechniqueBusinessService();
        //            IList<Bloc> listeBlocAEcrire = ptbs.GetListeBlocAvailableForProduitOrAplication(EnumTypeProduitApplication.APPLICATION);
        //            if (listeBlocAEcrire != null)
        //            {
        //                foreach (var bloc in listeBlocAEcrire)
        //                {
        //                    result = this.EcrireCible(FichePackModele.GetBlocByCode(composite, bloc.Code, EnumPartieXml.TECHNIQUE), idEtapeParametre, EnumTypeCibleHid.CIBLE_SIM);
        //                }
        //            }
        //            else
        //            {
        //                result = false;
        //            }

        //            this.DeconnecterCible();
        //            return result;
        //        }
        //        else
        //        {
        //            // throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
        //            return false;
        //        }
        //    }
        //    catch (ApplicativeException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw new ApplicativeException("ProgrammationBusinessService_ErreurEcriture");
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }
        //}

        /// <summary>
        /// Connexion, ecriture dans le produit, deconnexion
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="composite">The composite.</param>
        /// <param name="idEtapeParametre">The id etape parametre.</param>
        /// <returns>
        /// True if ok
        /// </returns>
        //public bool EcrireProduit(ushort vid, ushort pid, Composite composite, long idEtapeParametre, EnumTypeProduitApplication enumTypeProduitApplication)
        //{
        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        bool result = false;

        //        // Connexion
        //        result = this.ConnecterCible(vid, pid);

        //        // Ecriture
        //        if (result)
        //        {
        //            if (composite.Code == "XmlTechnique")
        //            {
        //                foreach (BlocXml bloc in composite.Children)
        //                {
        //                    result &= this.EcrireCible(bloc, idEtapeParametre, EnumTypeCibleHid.CIBLE_PRODUIT);
        //                }
        //            }
        //            else
        //            {
        //                result = this.EcrireCible(composite, idEtapeParametre, EnumTypeCibleHid.CIBLE_PRODUIT);
        //            }
                    
        //            this.DeconnecterCible();
        //            return result;
        //        }
        //        else
        //        {
        //            throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
        //        }
        //    }
        //    catch (ApplicativeException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw new ApplicativeException("ProgrammationBusinessService_ErreurEcriture");
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }
        //}

        #endregion

        #region VerifierFirmware

        /// <summary>
        /// Verifie les versions du firmware et recharge le firmware si elles ne sont pas égale
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="codeArticleLogiciel">The code article logiciel.</param>
        /// <param name="nomFichier">The nom fichier.</param>
        /// <returns>
        /// Ture if ok
        /// </returns>
        //public bool VerifierChargerFirmware(ushort vid, ushort pid, String codeArticleLogiciel, string nomFichier)
        //{
        //    //Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    //DFU.Error resultDfu = DFU.Error.NO_ERROR;
        //    try
        //    {
        //        bool result = this.ConnecterCible(vid, pid);

        //        if (result && this.portHid != null)
        //        {
        //            bool areEgal = false;
        //            //try
        //            //{
        //                areEgal = this.portHid.CheckFirmwareVersion(CIBLE_HID_e.CIBLE_CPU_0, codeArticleLogiciel);
        //                areEgal &= this.portHid.CheckFirmwareVersion(CIBLE_HID_e.CIBLE_CPU_1, codeArticleLogiciel);
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    Log.Error(string.Empty, ex);
        //            //}



        //            // telechargement si different
        //            if (!areEgal)
        //            {
        //                this.portHid.EnterDfuMode();
        //                Pegase.Dfu.DFU dfu = new DFU();
        //                try
        //                {
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Connecter");
        //                    resultDfu = dfu.Connecter();
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Connecter result :" + resultDfu);
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Telecharger");
        //                    resultDfu = dfu.Telecharger(nomFichier);
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Telecharger result :" + resultDfu.ToString());
        //                }
        //                catch (Exception ex)
        //                {
        //                    Log.Error(string.Empty, ex);
        //                    throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
        //                }
        //                finally
        //                {
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Deconnecter");
        //                    resultDfu = dfu.Deconnecter();
        //                    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Deconnecter result :" + resultDfu.ToString());
        //                }
        //            }

        //            this.DeconnecterCible();
        //            return areEgal || resultDfu == DFU.Error.NO_ERROR;
        //        }

        //        return false;
        //    }
        //    catch (ApplicativeException ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw new ApplicativeException("ProgrammationBusinessService_ErreurEcriture");
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);

        //    }
        //}

        /// <summary>
        /// Verifie les versions du firmware et recharge le firmware si elles ne sont pas égale
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="codeArticleLogiciel">The code article logiciel.</param>
        /// <param name="nomFichier">The nom fichier.</param>
        /// <returns>
        /// Ture if ok
        /// </returns>
        //public bool VerifierDfuChargerFirmware(ushort vid, ushort pid, String codeArticleLogiciel, string nomFichier)
        //{
        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //        DFU.Error resultDfu = DFU.Error.NO_ERROR;
        //        Pegase.Dfu.DFU dfu = new DFU();
        //        try
        //        {
        //            Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Connecter");
        //            resultDfu = dfu.Connecter();
        //            Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Connecter result :" + resultDfu.ToString());
        //            Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Telecharger");
        //            resultDfu = dfu.Telecharger(nomFichier);
        //            Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Telecharger result :" + resultDfu.ToString());
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(string.Empty, ex);
        //            throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
        //        }
        //        finally
        //        {
        //            Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, "dfu.Deconnecter");
        //            resultDfu = dfu.Deconnecter();
        //            Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, "dfu.Deconnecter result :" + resultDfu.ToString());
        //        }

        //        return resultDfu == DFU.Error.NO_ERROR;
        //    }
        //    catch (ApplicativeException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw new ApplicativeException("ProgrammationBusinessService_ErreurEcriture");
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);

        //    }
        //}

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <param name="cible">The cible.</param>
        /// <returns>
        /// Firmware version
        /// </returns>
        public string GetFirmwareVersion(Hid.CIBLE_HID_e cible)
        {
            String firmwareVersion = string.Empty;

            if (this.portHid != null)
            {
                bool result = this.portHid.GetFirmwareVersion(cible, ref firmwareVersion);

                if (result)
                {
                    return firmwareVersion;
                }

                return null;
            }

            return null;
        }

        #endregion

        #region Lecture
        /// <summary>
        /// Permet de récupérer un tableau de byte de la valeur lue en mémoire à l'offset donné pour la taille donnée
        /// </summary>
        /// <param name="vid">The VID.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="taille">The taille.</param>
        /// <returns>
        /// Le tableau de byte
        /// </returns>
        public byte[] LireEeprom(ushort vid, ushort pid, long offset, long taille)
        {
            //Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

            //try
            //{
            // TODO JMC
            if (this.portHid != null)
            {
                byte[] buf_lecture = new byte[taille];
                bool result = this.portHid.LireBloc(Hid.CIBLE_HID_e.CIBLE_EEP_0, buf_lecture, (uint)offset, (uint)taille);
                if (!result)
                {
                    buf_lecture = null;
                }

                return buf_lecture;
            }
            else
            {
                return null;
            }
            //}
            //catch (GToolBase.ApplicativeException aex)
            //{
            //    Log.Error(string.Empty, aex);
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(string.Empty, ex);
            //    throw;
            //}
            //finally
            //{
            //    Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
            //}
        }

        /// <summary>
        /// Permet de lire une vairable en memoire et de récuperer un objet variable
        /// </summary>
        /// <param name="vid">The vid.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="codeSousBloc">The code sous bloc.</param>
        /// <param name="codeVariable">The code variable.</param>
        /// <returns>La variable</returns>
        //public VariableXml LireVariable(ushort vid, ushort pid, string codeSousBloc, string codeVariable)
        //{
        //    Log.DebugFormat(Properties.DefaultMessages.Log_BeginMethod, new StackFrame().GetMethod().Name);

        //    try
        //    {
        //          bool result = this.ConnecterCible(vid, pid);
        //          if (result)
        //          {
        //              // Récuperation de la définition de la variable          
        //              VariableXml variableDefinition = FichePackModele.GetVariableByCode(null, codeVariable, codeSousBloc, 1, EnumPartieXml.TECHNIQUE);

        //              if (variableDefinition != null)
        //              {
        //                  if (variableDefinition.OffsetAbsolu != -1 && variableDefinition.Taille != -1)
        //                  {
        //                      // Lecture en memoire selon l'offset et la taille donnée
        //                      byte[] byteArrayValue = this.LireEeprom(vid, pid, variableDefinition.OffsetAbsolu, variableDefinition.Taille);

        //                      if (byteArrayValue != null && byteArrayValue.Length > 0)
        //                      {
        //                          // Conversion du tableau de byte en valeur
        //                          ConversionBusinessService conversion = new ConversionBusinessService();
        //                          variableDefinition.Valeur = conversion.ConvertToStringValueFromByteArray(variableDefinition, byteArrayValue);
        //                      }
        //                  }
        //              }

        //              this.DeconnecterCible();
        //              return variableDefinition;
        //          }
        //          else
        //          {
        //              // throw new ApplicativeException("ProgrammationBusinessService_ErreurConnexion");
        //              return new VariableXml();
        //          }
        //    }
        //    catch (ApplicativeException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(string.Empty, ex);
        //        throw new ApplicativeException("ProgrammationBusinessService_LectureEcriture");
        //    }
        //    finally
        //    {
        //        Log.DebugFormat(Properties.DefaultMessages.Log_EndMethod, new StackFrame().GetMethod().Name);
        //    }      
        //}
        #endregion
    }

    /// <summary>
    /// TEST de la classe ProgrammationBusinessService
    /// </summary>
    //public static class ProgrammationBusinessService_Test
    //{
        /// <summary>
        /// The PBS_Test
        /// </summary>
        //private static ProgrammationBusinessService pbsTest = null;

        /// <summary>
        /// Tests the JMC.
        /// </summary>
        //public static void TestJmc()
        //{
        //    if (pbsTest == null)
        //    {
        //        pbsTest = new ProgrammationBusinessService();
        //    }

        //    // TEST avec Carte de test ELIO VID/PID : 0x0483/0x5750
        //    // Version CPU_0 = MASTED0101-MASTEF0101
        //    // Version CPU_1 = MASTED0101-MASTEF0101
        //    // Test Lecture variable 
        //    bool connecte = pbsTest.ConnecterCible(0x0483, 0x5750);
        //    if (connecte)
        //    {
        //        // C:\Firmware\DFEX0A020xx.dfu
        //        bool update_ok = pbsTest.VerifierChargerFirmware(0x0483, 0x5750, "TOTOR_EST_FORT", "C:\\Firmware\\DFEX0A020xx.dfu");
        //        string version = pbsTest.GetFirmwareVersion(CIBLE_HID_e.CIBLE_CPU_0);

        //        if (null != version)
        //        {
        //            version = pbsTest.GetFirmwareVersion(CIBLE_HID_e.CIBLE_CPU_1);
        //        }

        //        /*
        //        byte[] memory = PBS_Test.LireEeprom(0x0483, 0x5750, 128, 4);
        //        VariableXml var_xml = PBS_Test.LireVariable(0x0483, 0x5750,"MT", "CRC32"); // justement a l'@ 128 !
        //         */
        //    }

        //    ////PBS_Test.DeconnecterCible();
        //}
    //}
}
