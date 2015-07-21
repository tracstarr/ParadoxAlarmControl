using ParadoxIp.Model;

namespace ParadoxIp.Events
{
    public class DeviceUpdateEventArgs
    {
        public Device Device { get; private set; }
        
        public DeviceUpdateEventArgs(Device device)
        {
            Device = device;
        }
    }
}