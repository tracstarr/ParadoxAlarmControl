using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/status", "GET", Summary = "Get the current status of your Paradox Controller.")]

    public class StatusRequest: IReturn<StatusRequest>
    {
    }
}
