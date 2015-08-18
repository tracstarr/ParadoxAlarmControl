
using ParadoxIp.Model;

namespace ParadoxIp
{
    public interface IParadoxEventCallbacks
    {
        bool PutDeviceUpdate(Device device);
        bool PutPartitionUpdate(Partition partition);
    }
}