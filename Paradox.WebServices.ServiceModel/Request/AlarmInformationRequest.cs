using Paradox.WebServices.ServiceModel.Response;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/information", "GET", Summary = "Get alarm information")]
    public class AlarmInformationRequest: IReturn<AlarmInformationResponse>
    {
    }
}