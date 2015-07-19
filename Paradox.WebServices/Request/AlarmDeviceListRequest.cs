using Paradox.WebServices.Response;
using ServiceStack;

namespace Paradox.WebServices.Request
{
    [Route("/devices", "GET", Summary = "Get alarm device list")]
    public class AlarmDeviceListRequest : IReturn<AlarmDeviceListResponse>
    {
    }
}