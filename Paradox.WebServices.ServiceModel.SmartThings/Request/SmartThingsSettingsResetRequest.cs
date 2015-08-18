using ServiceStack;

namespace Paradox.WebServices.ServiceModel.SmartThings.Request
{
    [Route("/smartthings/configure/reset", "GET", Summary = "Reset SmartThings configuration.")]
    public class SmartThingsSettingsResetRequest
    { }
}