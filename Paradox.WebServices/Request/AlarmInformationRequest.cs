using Paradox.WebServices.Response;
using ServiceStack;

namespace Paradox.WebServices.Request
{
    [Route("/information", "GET", Summary = "Get alarm information")]
    public class AlarmInformationRequest: IReturn<AlarmInformationResponse>
    {
    }
}