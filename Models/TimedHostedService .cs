
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;


namespace GenerateurDFUSafir.Models
{
    public class TimedHostedService 
    {
        public TimedHostedService()
        {
            Thread childthreat = new Thread(childthreadcall);
            childthreat.Start();
        }
        private System.Threading.Timer timer;
        private Thread ThreadDFU;
        private void childthreadcall()
        {
            while (false)
            {
                
                try
                {
                    if (ThreadDFU == null )
                    {
                        ThreadDFU = new Thread(ChildTreadDfu);
                    }
                    else if (!ThreadDFU.IsAlive)
                    {
                        ThreadDFU.Start();
                    }
                    
                }
                catch
                {}
            }
        }
        private void ChildTreadDfu()
        {
            DateTime oldtime = new DateTime();
            oldtime = DateTime.Now;
            while (true)
            {
                try
                {
                    if (DateTime.Now.AddMinutes(-60)> oldtime)
                    {
                        // generer les DFU
                        string test = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        oldtime = DateTime.Now;
                    }
                    Thread.Sleep(6000);
                }
                catch
                { 
                }
            }
        }
       
    }
}