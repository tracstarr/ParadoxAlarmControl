using ParadoxIp;
using ParadoxIp.Managers;
using ServiceStack;
using ServiceStack.Logging;
using SettingsProviderNet;

namespace Paradox.MQTT
{
    public class MqttPlugin : IPlugin, IPreInitPlugin, IPostInitPlugin
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(MqttPlugin));

        public void Register(IAppHost appHost)
        {
            logger.Info("Registering MQTT");
        }

        public void Configure(IAppHost appHost)
        {
            var settingsProvider = new SettingsProvider(new RoamingAppDataStorage("Paradox"));
            var mySettings = settingsProvider.GetSettings<ParadoxMqttSettings>();
            var container = appHost.GetContainer();
            container.Register(mySettings);
            container.RegisterAs<MqttCallbacks, IParadoxEventCallbacks>();
        }

        public void AfterPluginsLoaded(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            if (container.Exists<IpModuleManager>())
            {
                var manager = container.Resolve<IpModuleManager>();

                manager.DeviceStatusChanged += (sender, args) =>
                {
                    var settings = container.Resolve<ParadoxMqttSettings>();
                    var cb = new MqttCallbacks(settings);
                    cb.PutDeviceUpdate(args.Device);
                };

                manager.PartitionStatusChanged += (sender, args) =>
                {
                    var settings = container.Resolve<ParadoxMqttSettings>();
                    var cb = new MqttCallbacks(settings);
                    cb.PutPartitionUpdate(args.Partition);
                };
            }
            else
            {
                logger.Error("Cannot find Ip Module Manager to register device and partition status changes events for MQTT");
            }

        }
    }
}