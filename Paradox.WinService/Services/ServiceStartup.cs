using System;
using System.Configuration;
using System.Security.Policy;
using ParadoxIp.Model;
using ServiceStack;
using ServiceStack.Logging;

namespace Paradox.WinService.Services
{
    public static class ServiceStartup
    {
        static ParadoxAppListenerHost appHost;

        public static string ListeningOn
        {
            get
            {
                return ConfigurationManager.AppSettings["listenOn"];
            }
        }

        public static void Run()
        {
            LogManager.LogFactory = new ConsoleLogFactory();
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
            if (appHost != null)
            {
                appHost.Stop();
            }

        }

        public static AppHostHttpListenerBase GetAppHostListner()
        {
           // LogManager.LogFactory = new ConsoleLogFactory();
            var module = new IpModule(new Url(ConfigurationManager.AppSettings["moduleIp"]), Convert.ToInt32(ConfigurationManager.AppSettings["panelCode"]), ConfigurationManager.AppSettings["password"]);
            return new ParadoxAppListenerHost(module);
        }

    }


}
