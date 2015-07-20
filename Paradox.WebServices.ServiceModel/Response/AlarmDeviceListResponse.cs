using System.Collections.Generic;
using Paradox.WebServices.ServiceModel.Model;

namespace Paradox.WebServices.ServiceModel.Response
{
    public class AlarmDeviceListResponse : ParadoxBaseResponse
    {
        public List<Device> Devices { get; set; }
    }
}