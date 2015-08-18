using ServiceStack;

namespace Paradox.WebServices.ServiceModel.SmartThings.Request
{
    [Route("/smartthings/configure", "PUT", Summary = "Configure SmartThings Hub information.")]
    public class SmartThingsSettingsRequest
    {
        public string Location { get; set; }
        public string AppId { get; set; }
        public string AccessToken { get; set; }
    }
}