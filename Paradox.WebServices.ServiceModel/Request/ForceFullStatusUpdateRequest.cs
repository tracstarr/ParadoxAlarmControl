using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/forcestatusrefresh", "GET", Summary = "Force status update of all partitions and zones, sending out all webhooks")]
    public class ForceFullStatusUpdateRequest: IReturn<bool>
    {
         
    }
}