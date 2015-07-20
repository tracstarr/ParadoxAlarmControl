using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/zonestatus/{ZoneId}/{SendEvent}", "GET", Summary = "Get alarm device list")]
    public class ZoneStatusRequest : IReturn<string>
    {
        public string ZoneId { get; set; }
        public bool SendEvent { get; set; }
    }
}
