using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/stopstatusupdates", "GET", Summary = "Start the continuous status check message pump")]
    public class StopStatusCheckRequest
    {
    }
}