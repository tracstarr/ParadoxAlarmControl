using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/stopcheck", "GET", Summary = "Start the continuous status check message pump")]
    public class StopStatusCheckRequest
    {
    }
}