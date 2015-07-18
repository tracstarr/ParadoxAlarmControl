using ParadoxIp.Enum;

namespace ParadoxIp.Model
{
    public class Device
    {
        public DeviceType DeviceType { get; set; }
        public string Name { get; set; }
        public PartitionNumber Partition { get; set; }
        public DeviceStatus Status { get; set; }
        public DeviceStatus PreviousStatus { get; set; }
        public int ZoneId { get; set; }
    }
}
