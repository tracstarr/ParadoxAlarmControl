using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/configure", "PUT", Summary = "Configure SmartThings Hub information.")]
    public class SmartThingsSettingsRequest
    {
        public string Location { get; set; }
        public string AppId { get; set; }
        public string AccessToken { get; set; }
    }
}