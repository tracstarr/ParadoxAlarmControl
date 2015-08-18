using Paradox.WebServices.ServiceModel.SmartThings.Response;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.SmartThings.Request
{
    [Route("/smartthings/status", "GET", Summary = "Get the current status of the Smartthings configuration.")]

    public class SmartThingsStatusRequest : IReturn<SmartThingsStatusResponse>
    {
    }
}
