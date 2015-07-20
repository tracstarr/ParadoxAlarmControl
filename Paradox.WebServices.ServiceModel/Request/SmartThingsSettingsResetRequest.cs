using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/configure/reset", "GET", Summary = "Reset SmartThings configuration.")]
    public class SmartThingsSettingsResetRequest
    { }
}