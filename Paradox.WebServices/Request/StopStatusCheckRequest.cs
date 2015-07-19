using ServiceStack;

namespace Paradox.WebServices.Request
{
    [Route("/stopcheck", "GET", Summary = "Start the continuous status check message pump")]
    public class StopStatusCheckRequest
    {
    }
}