using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TraitementOFs;

namespace ServiceGenerationDFU
{
    public partial class ServiceGenerationDFU : ServiceBase
    {
        private int eventId = 1;

        public ServiceGenerationDFU()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("GenerateurDFUSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "GenerateurDFUSource", "MyNewLog");
            }
            eventLog1.Source = "GenerateurDFUSource";
            eventLog1.Log = "GenerateurDFULog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Generateur DFU OnStart.");

            // Set up a timer that triggers every minute.
            Timer timer = new Timer();
            timer.Interval = 3600000; // 1heures
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }


        protected override void OnStop()
        {
            eventLog1.WriteEntry("Generateur DFU Stopped.");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            TraitementOF traitement = new TraitementOF();
        }
    }
}
