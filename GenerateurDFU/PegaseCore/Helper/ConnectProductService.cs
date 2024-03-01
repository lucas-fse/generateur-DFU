using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Management;
using Threading = System.Windows.Threading;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Service d'aide pour la détection des produit connecté et leur gestion
    /// </summary>
    public class ConnectProductService : Mvvm.ViewModelBase
    {
        // Variables singleton
        private static ConnectProductService _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private ObservableCollection<ConnectedProduct> _currentConnectedProducts;
        private ManagementEventWatcher _watcher;
        private Boolean _isListen;
        private Threading.DispatcherTimer _timer;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La collection des produit actuellement connecté
        /// </summary>
        public ObservableCollection<ConnectedProduct> CurrentConnectedProducts
        {
            get
            {
                return this._currentConnectedProducts;
            }
            private set
            {
                this._currentConnectedProducts = value;
            }
        } // endProperty: CurrentConnectedProducts

        #endregion


        // Constructeur
        #region Constructeur

        private ConnectProductService()
        {
            this._isListen = false;
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static ConnectProductService Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new ConnectProductService();
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Détecter l'ensemble des produits SAFIR connectés
        /// Les produits connectés sont répertoriés dans la propriété CurrentConnectedProduct
        /// </summary>
        public void DetectConnectedProduct ( )
        {
            Boolean Continue = true;
            Int32 i = 0;

            while (Continue)
            {
                if (this.GetListOfConnectedProduct())
                {
                    Continue = false;
                }
                Helper.TimeHelper.Wait(500);
                i++;
                if (i > 4)
                {
                    Continue = false;
                }
            }

            // Si au moins un produit est détecté
            // Ecouter l'USB au cas ou le produit soit déconnecté
            if (this.CurrentConnectedProducts.Count > 0)
            {
                this.StartListen();
            }
        } // endMethod: DetectConnectedProduct

        /// <summary>
        /// Découvrir la liste des produits connectés au PC via USB et en mode HID
        /// </summary>
        public Boolean GetListOfConnectedProduct()
        {
            ObservableCollection<Helper.ConnectedProduct> result;
            Boolean Result = false;

            result = new ObservableCollection<Helper.ConnectedProduct>();
            // Scruter les ports pour voir si un produit est connecté
            //Form.MessageBox.Show("Init");
            PegaseCore.Hid.HidDll hidDll = new PegaseCore.Hid.HidDll();
            //Form.MessageBox.Show("PegaseCore.Hid.HidDll hidDll = new PegaseCore.Hid.HidDll() = OK");
            foreach (Helper.VidPid item in VidPidHelper.GetListProductVidPid())
            {
                //Form.MessageBox.Show(String.Format("Try to connect {0}, {1}", item.Vid, item.Pid));
                Hid.HidDll.HidStatus test = hidDll.Connecter(item.Vid, item.Pid, false);
                //Form.MessageBox.Show("Connect -> OK");
                if (test == Hid.HidDll.HidStatus.ERR_NO || test == Hid.HidDll.HidStatus.ERR_CONTEXT_ALREADY_SET)
                {
                    ConnectedProduct CP = new ConnectedProduct(item);
                    result.Add(CP);
                }

                //Form.MessageBox.Show("Before deconnect");
                hidDll.Deconnecter();
                //Form.MessageBox.Show("Deconnect");
            }

            //Form.MessageBox.Show("OK");
            if (result.Count > 0)
            {
                Result = true;
            }

            this.CurrentConnectedProducts = result;

            return Result;
        } // endMethod: GetListOfConnectedProduct

        /// <summary>
        /// Deconnecter tous les produits par soft
        /// pour les reconnecter, relancer une détection des connexions
        /// </summary>
        public void DeconnectAll( )
        {
            this.StopListen();
            this.CurrentConnectedProducts = new ObservableCollection<ConnectedProduct>();
        } // endMethod: DeconnectAll

        /// <summary>
        /// Démarrer l'écoute des USB
        /// </summary>
        private void StartListen()
        {
            if (!this._isListen && this._watcher == null)
            {
                WqlEventQuery q = new WqlEventQuery();
                q.EventClassName = "__InstanceOperationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = @"TargetInstance ISA 'Win32_USBControllerDevice' ";
                this._watcher = new ManagementEventWatcher(q);
                this._watcher.EventArrived += new EventArrivedEventHandler(this.UsbEventArrived);
                this._watcher.Start();
                this._isListen = true;

                // Initialiser le timer
                this._timer = new Threading.DispatcherTimer();
                this._timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                this._timer.Tick += new EventHandler(_timer_Tick);
                this._timer.Start();
            }
        }

        /// <summary>
        /// Stopper l'écoute des USB
        /// </summary>
        private void StopListen()
        {
            if (this._isListen)
            {
                if (this._watcher != null)
                {
                    this._watcher.EventArrived -= this.UsbEventArrived;
                    this._watcher.Stop();
                    this._watcher = null;
                }
                this._isListen = false;
                if (this._timer != null)
                {
                    this._timer.Stop();
                    this._timer = null;
                }
            }
        } // endMethod: StopListen

        /// <summary>
        /// Gérer l'évennement USB
        /// </summary>
        /// <param name="sender">
        /// Le port qui est responsable de l'évenement
        /// </param>
        /// <param name="e">
        /// Les arguments accompagnant l'évenement
        /// </param>
        private void UsbEventArrived(object sender, EventArrivedEventArgs e)
        {
            this.GetListOfConnectedProduct();
            if (this.CurrentConnectedProducts.Count == 0)
            {
                this.StopListen();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (this.CurrentConnectedProducts != null && this.CurrentConnectedProducts.Count > 0)
            {
                foreach (var mt in this.CurrentConnectedProducts)
                {
                    if (mt.TypeProduct == ProductType.MT)
                    {
                        mt.MAJCurrentDateTime();
                    }
                }
            }
        }

        // Messages
        #region Messages

        #endregion

    } // endClass: ConnectProductService
}
