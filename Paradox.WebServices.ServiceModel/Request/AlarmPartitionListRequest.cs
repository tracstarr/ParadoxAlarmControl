using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/partitions", "GET", Summary = "Get alarm partition list")]
    public class AlarmPartitionListRequest : IReturn<AlarmPartitionListRequest>
    {
    }
}