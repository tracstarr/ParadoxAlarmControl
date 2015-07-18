using ServiceStack;

namespace Paradox.WinService.Request
{
    [Route("/configure/reset", "GET", Summary = "Reset SmartThings configuration.")]
    public class SmartThingsSettingsResetRequest
    { }
}