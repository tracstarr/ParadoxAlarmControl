using Paradox.WebServices.ServiceModel.Response;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/smartthings/status", "GET", Summary = "Get the current status of the Smartthings configuration.")]

    public class SmartThingsStatusRequest : IReturn<SmartThingsStatusResponse>
    {
    }
}
