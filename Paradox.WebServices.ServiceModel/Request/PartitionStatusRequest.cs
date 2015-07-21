using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{

    [Route("/partitionstatus/{id}/{sendevent}", "GET", Summary = "Get status of given partition")]
    public class PartitionStatusRequest
    {
        public int Id { get; set; }
        public bool SendEvent { get; set; }
    }
}