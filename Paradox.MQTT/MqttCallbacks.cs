using System.Text;
using ParadoxIp;
using ParadoxIp.Enum;
using ParadoxIp.Model;
using ServiceStack.Logging;
using uPLibrary.Networking.M2Mqtt;

namespace Paradox.MQTT
{
    public class MqttCallbacks : IParadoxEventCallbacks
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(MqttCallbacks));
        private readonly ParadoxMqttSettings settings;
        private readonly MqttClient client;
        public MqttCallbacks(ParadoxMqttSettings settings)
        {
            this.settings = settings;
            client = new MqttClient(settings.BrokerUrl);
        }

        public void PutDeviceUpdate(Device device)
        {
            if (device.Status == DeviceStatus.Unknown)
                return;

            string topic = string.Format("{0}/zone/{1}/state",settings.RootTopic, device.ZoneId);
            client.Connect(settings.ClientId);
            client.Publish(topic, Encoding.UTF8.GetBytes(device.Status.ToString()), settings.QosLevel, settings.Retain);
            logger.DebugFormat("{0}[{1}]",topic, device.Status);
        }

        public void PutPartitionUpdate(Partition partition)
        {
            if (partition.Status == PartitionStatus.Unknown)
                return;

            string topic = string.Format("{0}/partition/{1}/status",settings.RootTopic, (int)partition.Id);
            client.Connect(settings.ClientId);
            client.Publish(topic, Encoding.UTF8.GetBytes(partition.Status.ToString()), settings.QosLevel, settings.Retain);
            logger.DebugFormat("{0}[{1}]",topic, partition.Status);
        }
    }

}
