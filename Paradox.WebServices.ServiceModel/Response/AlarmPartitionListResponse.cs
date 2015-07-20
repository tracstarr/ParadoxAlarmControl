using System.Collections.Generic;
using Paradox.WebServices.ServiceModel.Model;

namespace Paradox.WebServices.ServiceModel.Response
{
    public class AlarmPartitionListResponse : ParadoxBaseResponse
    {
        public List<Partition> Partitions { get; set; }
    }
}