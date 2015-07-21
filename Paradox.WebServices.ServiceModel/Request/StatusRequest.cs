using Paradox.WebServices.ServiceModel.Response;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/status", "GET", Summary = "Get the current status of the Smartthings configuration.")]

    public class StatusRequest : IReturn<StatusResponse>
    {
    }
}