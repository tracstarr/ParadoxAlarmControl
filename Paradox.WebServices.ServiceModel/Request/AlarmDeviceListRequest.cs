using Paradox.WebServices.ServiceModel.Response;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/paradox/devices", "GET", Summary = "Get alarm device list")]
    public class AlarmDeviceListRequest : IReturn<AlarmDeviceListResponse>
    {
    }
}