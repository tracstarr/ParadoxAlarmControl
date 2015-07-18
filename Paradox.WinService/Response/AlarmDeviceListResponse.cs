using System.Collections.Generic;
using ParadoxIp.Model;

namespace Paradox.WinService.Response
{
    public class AlarmDeviceListResponse : ParadoxBaseResponse
    {
        public IList<Device> Devices { get; set; }
    }
}