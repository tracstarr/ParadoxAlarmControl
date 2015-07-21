using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/refresh/partition/{id}", "GET", Summary = "Refresh partition Status")]
    public class RefreshPartitionRequest: IReturn<bool>
    {
         
    }
}