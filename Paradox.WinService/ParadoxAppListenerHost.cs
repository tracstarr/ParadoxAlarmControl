using System;
using System.Threading;
using Funq;
using Paradox.WinService.Services;
using Paradox.WinService.Settings;
using ParadoxIp.Managers;
using ParadoxIp.Model;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Text;
using SettingsProviderNet;

namespace Paradox.WinService
{
    public class ParadoxAppListenerHost
        : AppHostHttpListenerBase
    {
        private readonly IpModule module;
        private Thread notificationListenerThread;
        private Thread alarmStatusCheckThread;

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
            if (notificationListenerThread.IsAlive)
            {
                notificationListenerThread.Join();
            }
            
            base.Stop();
        }

        public override void Configure(Container container)
        {
         
            JsConfig.EmitCamelCaseNames = true;
            Plugins.Add(new SwaggerFeature());
            var manager = new IpModuleManager(module);
           
            manager.DeviceStatusChanged += (sender, args) =>
            {
                Console.WriteLine(args.Device.Name + ":" + args.Device.Status + "[" + args.Device.ZoneId + "]");
                var settings = container.Resolve<SmartThingsSettings>();
                var cb = new SmartThingsCallbacks(settings);
                cb.PutDeviceUpdate(args.Device);
            };

            //note: should we hide these endpoints and always start automatically?
            manager.Login();
            manager.GetAlarmInformation();
            
            container.Register(manager);

            var settingsProvider = new SettingsProvider(new RoamingAppDataStorage("Paradox"));
            var mySettings = settingsProvider.GetSettings<SmartThingsSettings>();

            container.Register(mySettings);

            alarmStatusCheckThread = new Thread(manager.StartStatusUpdates);
            container.Register(alarmStatusCheckThread);
         
        }

        public override ServiceStackHost Start(string urlBase)
        {
            StartNotificationListener();
            return base.Start(urlBase);
        }

        private void StartNotificationListener()
        {
            notificationListenerThread = new Thread(ParadoxNotificationListener.Main);
            notificationListenerThread.Start();
        }
    }
}