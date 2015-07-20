using ParadoxIp.Enum;

namespace ParadoxIp.Model
{
    public class Partition
    {
        public PartitionNumber Number { get; set; }
        public PartitionStatus Status { get; set; }
        public PartitionStatus PreviousStatus { get; set; }
        public string Name { get; set; }
    }
}
