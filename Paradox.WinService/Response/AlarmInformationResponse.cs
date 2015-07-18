namespace Paradox.WinService.Response
{
    public class AlarmInformationResponse: ParadoxBaseResponse
    {
        public string SiteName { get; set; }
        public string PanelType { get; set; }
        public string PanelFirmwareVersion { get; set; }
        public string PanelSerialNumber { get; set; }
        public string IpModuleFirmwareVersion { get; set; }
        public string IpModuleSerialNumber { get; set; }
        public string IpModuleHardwareVersion { get; set; }
        public string IpModuleEco { get; set; }
        public string IpModuleSerialBoot { get; set; }
        public string IpModuleIpBoot { get; set; }
        public string IpModuleMacAddress { get; set; }
    }
}