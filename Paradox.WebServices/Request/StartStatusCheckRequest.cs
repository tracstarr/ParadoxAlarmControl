using ServiceStack;

namespace Paradox.WebServices.Request
{
    [Route("/startstatusupdates", "GET", Summary = "Start the continuous status check message pump")]
    public class StartStatusCheckRequest
    {
    }
}