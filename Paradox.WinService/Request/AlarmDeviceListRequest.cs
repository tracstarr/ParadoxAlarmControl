using Paradox.WinService.Response;
using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/devices", "GET", Summary = "Get alarm device list")]
    public class AlarmDeviceListRequest : IReturn<AlarmDeviceListResponse>
    {
    }
}