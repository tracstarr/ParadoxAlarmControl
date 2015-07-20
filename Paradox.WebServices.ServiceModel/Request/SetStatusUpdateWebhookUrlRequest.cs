using System.Security.Policy;
using ServiceStack;

namespace Paradox.WebServices.ServiceModel.Request
{
    [Route("/setwebhook", "POST", Summary = "Set the webhook url for alarm status updates.")]
    public class SetStatusUpdateWebhookUrlRequest
    {
        public Url WebhookUrl { get; set; }
    }
}