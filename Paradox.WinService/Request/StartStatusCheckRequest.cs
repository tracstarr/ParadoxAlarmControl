using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/startcheck", "GET", Summary = "Start the continuous status check message pump")]
    public class StartStatusCheckRequest
    {
    }
}