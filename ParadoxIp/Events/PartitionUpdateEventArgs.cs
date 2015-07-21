using System;
using ParadoxIp.Model;

namespace ParadoxIp.Events
{
    public class PartitionUpdateEventArgs: EventArgs
    {
        public Partition Partition { get; private set; }

        public PartitionUpdateEventArgs(Partition partition)
        {
            Partition = partition;
        }
    }
}