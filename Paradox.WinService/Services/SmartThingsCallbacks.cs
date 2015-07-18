using Paradox.WinService.Settings;
using ParadoxIp.Enum;
using ParadoxIp.Model;
using RestSharp;

namespace Paradox.WinService.Services
{
    /// This functionality is directly linked to the SmartThings app. The endpoints are defined there that are used.
    internal class SmartThingsCallbacks
    {
        const string SmartthingsUrl = "https://graph.api.smartthings.com";

        private readonly SmartThingsSettings settings;
        private readonly RestClient client;
        private readonly string rootPath;

        public SmartThingsCallbacks(SmartThingsSettings settings)
        {
            this.settings = settings;
            client = new RestClient(SmartthingsUrl);
            rootPath = string.Format("/api/smartapps/installations/{0}/", settings.ApplicationId);
        }

        public bool Authorization()
        {
            string path = string.Format("{0}link", rootPath);
            var request = new RestRequest(path, Method.GET) {RequestFormat = DataFormat.Json};
            request.AddQueryParameter("access_token", settings.AccessToken);
            
            var response = client.Execute(request);
            return response.Content.Contains("ok");
        }

        public bool AuthorizationRevoke()
        {
            string path = string.Format("{0}revoke", rootPath);
            var request = new RestRequest(path, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", settings.AccessToken);

            var response = client.Execute(request);
            return response.Content.Contains("ok");
        }

        public bool PutDeviceUpdate(Device device)
        {
            if (device.Status == DeviceStatus.Unknown)
                return false;

            string path = string.Format("{0}zone/{1}/{2}", rootPath, device.ZoneId, (int)device.Status);
            var request = new RestRequest(path, Method.PUT) { RequestFormat = DataFormat.Json };
            request.AddQueryParameter("access_token", settings.AccessToken);
            
            var response = client.Execute(request);
            return response.Content.Contains("ok");
        }
    }
}
