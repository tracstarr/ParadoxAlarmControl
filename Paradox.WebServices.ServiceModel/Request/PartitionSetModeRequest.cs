using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/setpartitionmode", "PUT", Summary = "Set the alarm partition mode (arm, disarm, instant, force, stay)")]
   
    public class PartitionSetModeRequest
    {
        public int PartitionId { get; set; }
        public ArmingMode Mode { get; set; }
    }
}