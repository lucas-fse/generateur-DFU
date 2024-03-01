using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using System.Windows;
using JAY.DAL;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace JAY.DAL.BDDDistante
{
    public enum ConnectResult
    {
        ConnectOK,
        PasswordFalse,
        AllFalse
    }
    /// <summary>
    /// Class de gestion de l'accès des données hébergées sur le serveur
    /// </summary>
    public class BDDServeur
    {
        // Variables singleton
        private static BDDServeur _instance;
        static readonly object instanceLock = new object();
        private Uri _dataServiceUri = new Uri(DefaultValues.Get().UriWCFDataService);
        private ServiceData.PEGASE_SUPPORTEntities _pegaseSupportEntities;
        private Boolean _isConnected = false;

        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La base de données serveur est actuellement connectées à iDialog
        /// </summary>
        public Boolean IsConnected
        {
            get
            {
                if (this._pegaseSupportEntities == null)
                {
                    this._isConnected = false;
                }
                else
                {
                    this._isConnected = true;
                }

                return this._isConnected;
            }
        } // endProperty: IsConnected

        /// <summary>
        /// Les entités de données de la base distante
        /// </summary>
        public ServiceData.PEGASE_SUPPORTEntities PegaseSupportEntities
        {
            get
            {
                if (this._pegaseSupportEntities == null)
                {
                    try
                    {
                        //_dataServiceUri = new Uri("http://10.147.20.154/WCFIDialog/ServerDataAcces.svc/");
                        this._pegaseSupportEntities = new ServiceData.PEGASE_SUPPORTEntities(this._dataServiceUri);

                        // Tester si la BDD est accessible
                        var Query = from user in PegaseSupportEntities.Users
                                    where user.emailAdress == "c.fournier@jay-electronique.fr" 
                                    select user;

                        if (Query.Count() > 0)
                        {

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Warning! Synchronisation with JAY database is impossible. Please check connection...", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        this._pegaseSupportEntities = null;
                    }
                }
                return this._pegaseSupportEntities;
            }
        } // endProperty: PegaseSupportEntities

        #endregion

        // Constructeur
        #region Constructeur

        private BDDServeur()
        {
            
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static BDDServeur Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new BDDServeur();

                return _instance;
            }
        }

        /// <summary>
        /// Obtenir la liste des projects SIM sur le serveur correspondant à cette IDSim
        /// Les données sont transformées en CommonSimProject afin de facilité la comparaison avec la BDD locale
        /// et diminué la taille de téléchargement depuis le serveur
        /// </summary>
        public ObservableCollection<CommonSimProject> GetSimProject ( String IDSim, String IDProduct )
        {
            ObservableCollection<CommonSimProject> Result;
            IEnumerable<CommonSimProject> Query;
            try
            {
                if (this.PegaseSupportEntities.SimPROJECT.Count() > 0)
                {
                    if (IDProduct != "")
                    {
                        Query = from simPrj in this.PegaseSupportEntities.SimPROJECT
                                where simPrj.IdSIM == IDSim && (simPrj.IdMO == IDProduct || simPrj.IdMT == IDProduct)
                                select new CommonSimProject()
                                {
                                    IDSim = simPrj.IdSIM,
                                    Date = simPrj.Date.Value
                                };
                    }
                    else
                    {
                        Query = from simPrj in this.PegaseSupportEntities.SimPROJECT
                                where simPrj.IdSIM == IDSim
                                select new CommonSimProject()
                                {
                                    IDSim = simPrj.IdSIM,
                                    Date = simPrj.Date.Value
                                };
                    }

                    if (Query.Count() > 0)
                    {
                        Result = new ObservableCollection<CommonSimProject>(Query);
                    }
                    else
                    {
                        Result = new ObservableCollection<CommonSimProject>();
                    }
                }
                else
                {
                    Result = new ObservableCollection<CommonSimProject>();
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                Result = new ObservableCollection<CommonSimProject>();
            }

            return Result;
        } // endMethod: GetSimProjects
        
        /// <summary>
        /// Acquérir le project Sim correspondant à cette ID et cette date
        /// </summary>
        public ServiceData.SimPROJECT GetSimProject ( String IDSim, DateTime Date )
        {
            ServiceData.SimPROJECT Result = null;

            var Query = from simPrj in this.PegaseSupportEntities.SimPROJECT
                        where simPrj.Date == Date && simPrj.IdSIM == IDSim
                        select simPrj;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }

            return Result;
        } // endMethod: GetSimProject

        /// <summary>
        /// Uploader un projet SIM sur le serveur distant
        /// </summary>
        /// <param name="FileName">
        /// Le chemin d'accès pour le fichier iDialog depuis le disque local
        /// </param>
        /// <param name="simProject">
        /// Les données SimProject liées au fichier à Uploader
        /// </param>
        public Boolean UploadSimProject ( ServiceData.SimPROJECT simProject, String FileName )
        {
            Boolean Result = false;
            MergeOption cachedMergeOption = MergeOption.OverwriteChanges;
            EntityDescriptor entity = null;

            // Reset the image stream.
            FileStream iDialogFileStream = null;

            try
            {
                // FileName = "C:\\Test\\toot.txt";
                // Définir le FileStream permettant d'ouvrir le fichier
               // iDialogFileStream.Close();
                iDialogFileStream = new FileStream(FileName, FileMode.Open);

                // Définir les en-têtes des données binaires et le flux de sauvegarde pour le streaming vers le serveur
                DataServiceRequestArgs DRA = new DataServiceRequestArgs();
                DRA.AcceptContentType = "application/atom+xml";
                DRA.ContentType = "application/octet-stream; charset=binary";
                DRA.Slug = simProject.Hash;

                this.PegaseSupportEntities.AddToSimPROJECT(simProject);
                this.PegaseSupportEntities.SetSaveStream(simProject, iDialogFileStream, true, DRA);
                int erreur_position = 0;
                int cpt = 0;
                while ((Result == false) && (cpt<2))
                {
                    erreur_position = 0;
                    // Enregistrer les données
                    try
                    {
                        ChangeOperationResponse response;

                        response = this.PegaseSupportEntities.SaveChanges().FirstOrDefault() as ChangeOperationResponse;
                        erreur_position = 1;
                        // When we issue a POST request, the photo ID and edit-media link are not updated 
                        // on the client (a bug), so we need to get the server values.
                        if (simProject.Id == 0)
                        {
                            if (response != null)
                            {
                                erreur_position = 2;
                                entity = response.Descriptor as EntityDescriptor;
                                erreur_position = 3;
                            }
                            erreur_position = 4;
                            // Verify that the entity was created correctly.
                            if (entity != null && entity.EditLink != null)
                            {
                                erreur_position = 5;
                                // Cache the current merge option (we reset to the cached 
                                // value in the finally block).
                                cachedMergeOption = this.PegaseSupportEntities.MergeOption;
                                erreur_position = 6;
                                // Set the merge option so that server changes win.
                                this.PegaseSupportEntities.MergeOption = MergeOption.OverwriteChanges;
                                erreur_position = 7;
                                // Get the updated entity from the service.
                                // Note: we need Count() just to execute the query.
                                this.PegaseSupportEntities.Execute<ServiceData.SimPROJECT>(entity.EditLink).Count();
                                erreur_position = 8;
                            }
                        }

                        Result = true;
                    }
                    catch (DataServiceRequestException ex)
                    {
                        // Get the change operation result.
                        ChangeOperationResponse response =
                            ex.Response.FirstOrDefault() as ChangeOperationResponse;

                        string errorMessage = string.Empty;

                        // Check for a resource not found error.
                        if (response != null && response.StatusCode == 404)
                        {
                            // Tell the user to refresh the photos from the data service.
                            errorMessage = "The selected image may have been removed from the data service.\n"
                            + "Click the Refresh Photos button to refresh photos from the service.";
                        }
                        else
                        {
                            // Just provide the general error message.
                            errorMessage = string.Format("The file {0} could not be updated or saved. Error-{1} : {2}", simProject.NomIDialog, erreur_position,
                                                            response.Descriptor.State + " " + response.Headers.ToString() + " " + ex.Message);
                        }

                        // Show the messsage box.
                        MessageBox.Show(errorMessage, "Operation Failed");

                        // Return false since we could not add or update the photo.
                        Result = false;
                        cpt++;
                    }

                    finally
                    {
                        // Reset to the cached merge option.
                        this.PegaseSupportEntities.MergeOption = cachedMergeOption;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    string.Format("The selected file could not be opened. {0}",
                    ex.Message), "Operation Failed");

                Result = false;
            }

            return Result;
        } // endMethod: UploadSimProject
        
        /// <summary>
        /// Downloader un projet SIM depuis le serveur distant
        /// </summary>
        /// <param name="CurrentFilename">
        /// Le chemin sur lequel copier le fichier iDialog depuis le serveur distant
        /// </param>
        /// <param name="simProject">
        /// Le SIMProject correspondant au fichier iDialog à télécharger
        /// </param>
        public void DownLoadSimProject(ServiceData.SimPROJECT simProject, String FileName)
        {
            try
            {
                // Use the ReadStreamUri of the Media Resource for selected PhotoInfo object
                // as the URI source of a new bitmap image.
                DataServiceStreamResponse serverFile = this.PegaseSupportEntities.GetReadStream(simProject);
                using (BinaryWriter BW = new BinaryWriter(File.Open(FileName, FileMode.Create)))
                {
                    using (BinaryReader BR = new BinaryReader(serverFile.Stream))
                    {
                        Byte[] buffer = new Byte[1024];
                        Int32 Length;

                        do
                        {
                            Length = BR.Read(buffer, 0, 1024);
                            BW.Write(buffer, 0, Length);
                        } while (Length > 0);

                        BW.Flush();
                    }
                }
            }
            catch (DataServiceClientException ex)
            {
                XElement error = XElement.Parse(ex.Message);

                // Display the error messages.
                MessageBox.Show(error.Value,
                    string.Format("Error Status Code: {0}", ex.StatusCode));
            }
        } // endMethod: DownLoadSimProject
        
        /// <summary>
        /// Changer le mot de passe
        /// </summary>
        public void ChangePassword ( String UserID, String password )
        {
            IEnumerable<ServiceData.Users> query = from user in this.PegaseSupportEntities.Users
                        where user.emailAdress == UserID
                        select user;

            if (query.Count() > 0)
            {
                ServiceData.Users user = query.First();
                user.Password = this.CryptageSha1(password);
                this.PegaseSupportEntities.UpdateObject(user);
                this.PegaseSupportEntities.SaveChanges();
            }
        } // endMethod: ChangePassword

        /// <summary>
        /// Vérifier si l'utilisateur est enregistré dans la base de traçabilité
        /// </summary>
        public ConnectResult VerifyAccessRights ( String Identity, String Password )
        {
            ConnectResult Result = ConnectResult.AllFalse;

            if (this.PegaseSupportEntities != null)
            {
                // 1 - crypter le mot de passe
                String CryptPassword = this.CryptageSha1(Password);

                // 2 - vérifier si l'identifiant et le mot de passe existe dans la base
                var Query = from user in PegaseSupportEntities.Users
                            where user.emailAdress == Identity && user.Password == CryptPassword
                            select user;

                if (Query.Count() > 0)
                {
                    Result = ConnectResult.ConnectOK;
                }
                else
                {
                    Query = from user in PegaseSupportEntities.Users
                            where user.emailAdress == Identity
                            select user;

                    if (Query.Count() > 0)
                    {
                        Result = ConnectResult.PasswordFalse;
                    }
                }
            }

            return Result;
        } // endMethod: VerifyAccessRights
        
        /// <summary>
        /// Créer un utilisateur dans la base
        /// </summary>
        public void CreateUser ( ServiceData.Users user )
        {
            this.PegaseSupportEntities.AddToUsers(user);
            this.PegaseSupportEntities.SaveChanges();
        } // endMethod: CreateUser

        /// <summary>
        /// Calculer la signature unique à partir de l'élément XML spécifié
        /// </summary>
        public String CryptageSha1(String StrElement)
        {
            String Result = "";
            Byte[] computeHash;

            if (StrElement != null && StrElement != "")
            {
                SHA1 hashage = SHA1.Create();

                computeHash = hashage.ComputeHash(Encoding.UTF8.GetBytes(StrElement));

                for (int i = 0; i < computeHash.Length; i++)
                {
                    Result += computeHash[i].ToString("X2");
                } 
            }

            return Result;
        } // endMethod: CalculateHash

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: BDDServeur
}
