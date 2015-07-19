using ServiceStack;

namespace Paradox.WebServices.Request
{
    [Route("/configure/reset", "GET", Summary = "Reset SmartThings configuration.")]
    public class SmartThingsSettingsResetRequest
    { }
}