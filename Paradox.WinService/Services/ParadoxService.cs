using System;
using System.Linq;
using System.Threading;
using Paradox.WinService.Request;
using Paradox.WinService.Response;
using Paradox.WinService.Settings;
using ParadoxIp.Enum;
using ParadoxIp.Managers;
using ServiceStack;

namespace Paradox.WinService.Services
{
    public class ParadoxService : Service
    {
        private readonly IpModuleManager manager;
        private Thread statusThread;
        private readonly SmartThingsSettings settings;

        public ParadoxService(IpModuleManager manager, Thread statusThread, SmartThingsSettings settings)
        {
            this.manager = manager;
            this.statusThread = statusThread;
            this.settings = settings;
        }

        public object Get(LoginToAlarmModuleRequest request)
        {
            if (manager.IsLoggedIn)
                return true;

            return manager.Login();
        }

        public AlarmInformationResponse Get(AlarmInformationRequest request)
        {
            manager.GetAlarmInformation();
            manager.GetVersionInformation();

            return new AlarmInformationResponse()
            {
                Action = "info",
                SiteName = manager.SystemName,
                IpModuleEco = manager.Module.IpModuleVersion.Eco,
                IpModuleFirmwareVersion = manager.Module.IpModuleVersion.FirmwareVersion,
                IpModuleHardwareVersion = manager.Module.IpModuleVersion.HardwareVersion,
                IpModuleIpBoot = manager.Module.IpModuleVersion.IpBoot,
                IpModuleMacAddress = manager.Module.IpModuleVersion.MacAddress,
                IpModuleSerialBoot = manager.Module.IpModuleVersion.SerialBoot,
                IpModuleSerialNumber = manager.Module.IpModuleVersion.SerialNumber,
                PanelFirmwareVersion = manager.Module.AlarmPanelVersion.FirmwareVersion,
                PanelSerialNumber = manager.Module.AlarmPanelVersion.SerialNumber,
                PanelType = manager.Module.AlarmPanelVersion.Type
            };

        }

        public AlarmDeviceListResponse Get(AlarmDeviceListRequest request)
        {
            manager.GetAlarmInformation();
            return new AlarmDeviceListResponse() { Action = "devices", Devices = manager.Devices };
        }

        public bool Get(StartStatusCheckRequest request)
        {
            if (statusThread != null && !statusThread.IsAlive)
            {
                if (statusThread.ThreadState == ThreadState.Unstarted)
                {
                    statusThread.Start();
                }
                else
                {
                    //bug: not working right (no events?)
                    statusThread = new Thread(manager.StartStatusUpdates);
                    statusThread.Start();
                }
                return true;
            }

            return false;
        }

        public bool Get(StopStatusCheckRequest request)
        {
            if (statusThread != null && statusThread.IsAlive)
            {
                manager.StopStatusUpdates();
                return true;
            }

            return false;
        }

        public object Put(SetStatusUpdateWebhookUrlRequest request)
        {
            return new NotImplementedException();
        }

        public string Get(ZoneStatusRequest request)
        {
            if (statusThread == null || !statusThread.IsAlive)
            {
                manager.GetStatus();
            }

            var device = manager.Devices.SingleOrDefault(d => d.ZoneId.ToString() == request.ZoneId);
            if (device != null)
            {
                if (request.SendEvent)
                {
                    var cb = new SmartThingsCallbacks(settings);
                    cb.PutDeviceUpdate(device);
                }
                return device.Status.ToString();
            }

            return DeviceStatus.Unknown.ToString();
        }
    }
}