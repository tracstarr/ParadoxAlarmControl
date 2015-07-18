using System.Security.Policy;
using ParadoxIp.Enum;

namespace ParadoxIp.Model
{
    public class IpModule
    {
        public IpModuleType Module { get; set; }
        public Url Url { get; private set; }
        public int PanelCode { get; private set; }
        public string Password { get; private set; }
        public string UserKey { get; set; }
        public string PasswordHash { get; set; }
        public string Session { get; set; }
        public string PasswordFirstHash { get; set; }
        public AlarmPanelVersion AlarmPanelVersion { get; set; }
        public IpModuleVersion IpModuleVersion { get; set; }

        public IpModule(Url alarmUrl, int panelCode, string password)
        {
            Url = alarmUrl;
            PanelCode = panelCode;
            Password = password;
        }
    }
}
