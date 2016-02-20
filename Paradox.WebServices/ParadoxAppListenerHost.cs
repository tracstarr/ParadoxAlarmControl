using System;
using System.Configuration;
using System.Threading;
using Funq;
using Paradox.MQTT;
using Paradox.WebServices.Services;
using ParadoxIp.Managers;
using ParadoxIp.Model;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace Paradox.WebServices
{
    public class ParadoxAppListenerHost
        : AppHostHttpListenerBase
    {
        private readonly IpModule module;
        private Thread notificationListenerThread;
        private Thread alarmStatusCheckThread;

        private readonly ILog logger = LogManager.GetLogger(typeof(ParadoxAppListenerHost));

        public ParadoxAppListenerHost(IpModule module)
            : base("Paradox HttpListener", typeof(ParadoxService).Assembly)
        {
            this.module = module;
        }

        public override void Stop()
        {
            logger.Info("Stopping Paradox Web Services");
            var manager = Container.Resolve<IpModuleManager>();

            if (manager != null)
            {
                if (alarmStatusCheckThread.IsAlive)
                {
                    manager.StopStatusUpdates();
                    alarmStatusCheckThread.Join();
                }
                manager.Logout();
                manager.Dispose();
            }

            ParadoxNotificationListener.RequestStop();
            if (notificationListenerThread != null && notificationListenerThread.IsAlive)
            {
                notificationListenerThread.Join();
            }

            base.Stop();
        }

        public override void Configure(Container container)
        {
            GlobalResponseFilters.Add((req, res, dto) =>
            {
                res.AddHeader("X-Paradox", ServiceName);
            });

            JsConfig.EmitCamelCaseNames = true;
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new MqttPlugin());

            var manager = new IpModuleManager(module);

            // try one attempt to logout/login if can't login initially
            if (!manager.Login())
            {
                logger.Warn("Couldn't login. Attempting to logout then login to alarm module.");
                manager.Logout();
                manager.Login();
            }

            try
            {
                manager.GetAlarmInformation();
            }
            catch (Exception)
            {
                logger.Error("Problem logging in and getting alarm information. Services will start but Alarm host will not be connected.");
            }

            container.Register(manager);

            alarmStatusCheckThread = new Thread(manager.StartStatusUpdates);
            container.Register(alarmStatusCheckThread);
            alarmStatusCheckThread.Start();

        }

        public override ServiceStackHost Start(string urlBase)
        {
            StartNotificationListener();
            return base.Start(urlBase);
        }

        private void StartNotificationListener()
        {
            bool start = Convert.ToBoolean(ConfigurationManager.AppSettings["runMailServer"]);
            if (!start)
                return;

            notificationListenerThread = new Thread(ParadoxNotificationListener.Main);
            notificationListenerThread.Start();
        }
    }
}