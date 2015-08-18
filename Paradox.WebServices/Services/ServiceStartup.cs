using System;
using System.Configuration;
using System.Security.Policy;
using ParadoxIp.Model;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

namespace Paradox.WebServices.Services
{
    public static class ServiceStartup
    {
        static ParadoxAppListenerHost appHost;

        private static string ListeningOn => ConfigurationManager.AppSettings["listenOn"];

        public static void Run()
        {
            LogManager.LogFactory = new NLogFactory();
            var logger = LogManager.GetLogger(typeof(ServiceStartup));

            try
            {
                var module = new IpModule(new Url(ConfigurationManager.AppSettings["moduleIp"]), Convert.ToInt32(ConfigurationManager.AppSettings["panelCode"]), ConfigurationManager.AppSettings["password"]);
                appHost = new ParadoxAppListenerHost(module);

                appHost.Init();
                appHost.Start(ListeningOn);
                logger.InfoFormat("Listening On: {0}", ListeningOn);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0}: {1}", ex.GetType().Name, ex.Message);
                throw;
            }
        }

        public static void Stop()
        {
            appHost?.Stop();
        }

        public static AppHostHttpListenerBase GetAppHostListner()
        {
            LogManager.LogFactory = new NLogFactory();
            var module = new IpModule(new Url(ConfigurationManager.AppSettings["moduleIp"]), Convert.ToInt32(ConfigurationManager.AppSettings["panelCode"]), ConfigurationManager.AppSettings["password"]);
            return new ParadoxAppListenerHost(module);
        }

    }
}
