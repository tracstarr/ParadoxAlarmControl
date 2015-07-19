using System.Collections.Generic;
using ParadoxIp.Model;

namespace Paradox.WebServices.Response
{
    public class AlarmDeviceListResponse : ParadoxBaseResponse
    {
        public IList<Device> Devices { get; set; }
    }
}