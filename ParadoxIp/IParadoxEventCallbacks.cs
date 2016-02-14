
using ParadoxIp.Model;

namespace ParadoxIp
{
    public interface IParadoxEventCallbacks
    {
        void PutDeviceUpdate(Device device);
        void PutPartitionUpdate(Partition partition);
    }
}