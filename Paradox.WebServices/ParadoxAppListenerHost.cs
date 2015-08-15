using System;
using System.Threading;
using Funq;
using Paradox.WebServices.Services;
using Paradox.WebServices.Settings;
using ParadoxIp.Managers;
using ParadoxIp.Model;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Logging;
using ServiceStack.Text;
using SettingsProviderNet;
using System.Configuration;

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

            this.GlobalResponseFilters.Add((req, res, dto) =>
            {

                res.AddHeader("X-SmartThings", ServiceName);

            });

            JsConfig.EmitCamelCaseNames = true;
            Plugins.Add(new SwaggerFeature());
            var manager = new IpModuleManager(module);
           
            manager.DeviceStatusChanged += (sender, args) =>
            {
                logger.DebugFormat("{0}:{1}[{2}]", args.Device.Name, args.Device.Status, args.Device.ZoneId);
                var settings = container.Resolve<SmartThingsSettings>();
                var cb = new SmartThingsCallbacks(settings);
                cb.PutDeviceUpdate(args.Device);
            };

            manager.PartitionStatusChanged += (sender, args) =>
            {
                logger.DebugFormat("{0}:{1}", args.Partition.Name, args.Partition.Status);
                var settings = container.Resolve<SmartThingsSettings>();
                var cb = new SmartThingsCallbacks(settings);
                cb.PutPartitionUpdate(args.Partition);
            };

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

            var settingsProvider = new SettingsProviderNet.SettingsProvider(new RoamingAppDataStorage("Paradox"));
            var mySettings = settingsProvider.GetSettings<SmartThingsSettings>();

            container.Register(mySettings);

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
            //bool start = Convert.ToBoolean(ConfigurationManager.AppSettings["runMailServer"]);
            //if (!start)
            //    return;

            //notificationListenerThread = new Thread(ParadoxNotificationListener.Main);
            //notificationListenerThread.Start();
        }
    }
}