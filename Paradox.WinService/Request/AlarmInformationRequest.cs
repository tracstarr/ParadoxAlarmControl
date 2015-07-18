using Paradox.WinService.Response;
using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/information", "GET", Summary = "Get alarm information")]
    public class AlarmInformationRequest: IReturn<AlarmInformationResponse>
    {
    }
}