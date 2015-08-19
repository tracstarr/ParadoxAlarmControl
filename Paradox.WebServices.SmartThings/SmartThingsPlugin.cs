using Paradox.WebServices.SmartThings.Settings;
using ParadoxIp;
using ParadoxIp.Managers;
using ServiceStack;
using ServiceStack.Logging;
using SettingsProviderNet;

namespace Paradox.WebServices.SmartThings
{
    public class SmartThingsPlugin : IPlugin, IPreInitPlugin, IPostInitPlugin
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(SmartThingsPlugin));

        public void Register(IAppHost appHost)
        {
            logger.Info("Registering SmartThings Services");
            appHost.RegisterService<SmartThingsService>();
        }

        public void Configure(IAppHost appHost)
        {
            var settingsProvider = new SettingsProvider(new RoamingAppDataStorage("Paradox"));
            var mySettings = settingsProvider.GetSettings<SmartThingsSettings>();
            var container = appHost.GetContainer();
            container.Register(mySettings);
            container.RegisterAs<SmartThingsCallbacks, IParadoxEventCallbacks>();
        }

        public void AfterPluginsLoaded(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            if (container.Exists<IpModuleManager>())
            {
                var manager = container.Resolve<IpModuleManager>();

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
            }
            else
            {
                logger.Error("Cannot find Ip Module Manager to register device and partition status changes events for SmartThings");
            }

        }
    }
}
