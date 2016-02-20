using System;
using System.ComponentModel;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Paradox.MQTT
{
    public class ParadoxMqttSettings
    {
        [DefaultValue("192.168.20.181")]
        public string BrokerUrl { get; set; }
        [DefaultValue("ParadoxWebServices")]
        public string ClientId { get; set; }
        [DefaultValue("paradox/alarm")]
        public string RootTopic { get; set; }
        [DefaultValue(MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE)]
        public Byte QosLevel { get; set; }
        [DefaultValue(false)]
        public bool Retain { get; set; }
    }
}