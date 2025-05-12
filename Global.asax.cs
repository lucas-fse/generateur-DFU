using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GenerateurDFUSafir.App_Start;
using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.DAL;
using Hangfire;
using Hangfire.SqlServer;


namespace GenerateurDFUSafir
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = System.Web.Http.GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders[typeof(double)] = new DoubleModelBinder();

            TimedHostedService thread = new TimedHostedService();

            // Mise en place Hangfire
            Hangfire.GlobalConfiguration.Configuration
            .UseSqlServerStorage("HangfireConnection");

            // Lancer le serveur Hangfire
            var options = new BackgroundJobServerOptions
            {
                ServerName = "PEGASE_Hangfire_Server"
            };

            new BackgroundJobServer(options);

            // Planification tâche récurrente
            RecurringJob.AddOrUpdate(
                "suppression-tokens-expirés",
                () => GestionOperateursProd.SuppTousLesTokens(),
                Cron.Daily);
        }
    }
}
