using System;
using Paradox.WebServices.SmartThings.Settings;
using ParadoxIp;
using ParadoxIp.Enum;
using ParadoxIp.Model;
using RestSharp;

namespace Paradox.WebServices.SmartThings
{
    /// This functionality is directly linked to the SmartThings app. The endpoints are defined there that are used.
    public class SmartThingsCallbacks : IParadoxEventCallbacks
    {
        const string SmartthingsUrl = "https://graph.api.smartthings.com";

        private readonly SmartThingsSettings settings;
        private readonly RestClient client;
        private readonly string rootPath;

        public SmartThingsCallbacks(SmartThingsSettings settings)
        {
            this.settings = settings;
            client = new RestClient(SmartthingsUrl);
            rootPath = string.Format("/api/smartapps/installations/{0}/",settings.ApplicationId);
        }

        public bool Authorization()
        {
            string path = String.Format("{0}link",rootPath);
            var request = new RestRequest(path, Method.GET) {RequestFormat = DataFormat.Json};
            request.AddQueryParameter("access_token", settings.AccessToken);
            
            var response = client.Execute(request);
            return response.Content.Contains("ok");
        }

        public bool AuthorizationRevoke()
        {
            string path = String.Format("{0}revoke",rootPath);
            var request = new RestRequest(path, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", settings.AccessToken);

            var response = client.Execute(request);
            return response.Content.Contains("ok");
        }

        public void PutDeviceUpdate(Device device)
        {
            if (device.Status == DeviceStatus.Unknown)
                return;

            string path = string.Format("{0}zone/{1}/{2}",rootPath, device.ZoneId, (int) device.Status);
            var request = new RestRequest(path, Method.PUT) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", settings.AccessToken);
            
            var response = client.Execute(request);
            //return response.Content.Contains("ok");
        }

        public void PutPartitionUpdate(Partition partition)
        {
            if (partition.Status == PartitionStatus.Unknown)
                return;
            
            string path = string.Format("{0}partition/{1}/{2}",rootPath, partition.Id, (int) partition.Status);
            var request = new RestRequest(path, Method.PUT) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", settings.AccessToken);

            var response = client.Execute(request);
            //return response.Content.Contains("ok");     
        }
    }
}
