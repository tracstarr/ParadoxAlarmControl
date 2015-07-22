using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Paradox.WebServices.ServiceModel.Model;
using Paradox.WebServices.ServiceModel.Request;
using Paradox.WebServices.ServiceModel.Response;
using Paradox.WebServices.Settings;
using ParadoxIp.Enum;
using ParadoxIp.Managers;
using ServiceStack;
using ServiceStack.Logging;

namespace Paradox.WebServices.Services
{
    public class ParadoxService : Service
    {
        private readonly IpModuleManager manager;
        private Thread statusThread;
        private readonly SmartThingsSettings settings;
        private readonly ILog logger = LogManager.GetLogger(typeof(ParadoxService));

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
            var response = new AlarmDeviceListResponse() { Action = "devices", Devices = new List<Device>() };
            manager.GetAlarmInformation();

            foreach (var device in manager.Devices)
            {
                response.Devices.Add(new Device() { Id = device.ZoneId, Name = device.Name, DeviceType = device.DeviceType.ToString() });
            }

            return response;
        }

        public AlarmPartitionListResponse Get(AlarmPartitionListRequest request)
        {
            var response = new AlarmPartitionListResponse() { Action = "partitions", Partitions = new List<Partition>() };
            manager.GetAlarmInformation();

            foreach (var partition in manager.Partitions)
            {
                response.Partitions.Add(new Partition() { Id = (int)partition.Id, Name = partition.Name });
            }

            return response;
        }

        public bool Put(PartitionSetModeRequest request)
        {
            var partitionId = (PartitionNumber)request.PartitionId;
            var mode = (AlarmMode)request.Mode;
            manager.AlarmAction(partitionId, mode);
            return true;
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

        public string Get(PartitionStatusRequest request)
        {
            if (statusThread == null || !statusThread.IsAlive)
            {
                manager.GetStatus();
            }

            var partition = manager.Partitions.SingleOrDefault(d => (int)d.Id == request.Id);
            if (partition != null)
            {
                if (request.SendEvent)
                {
                    var cb = new SmartThingsCallbacks(settings);
                    cb.PutPartitionUpdate(partition);
                }
                return partition.Status.ToString();
            }

            return PartitionStatus.Unknown.ToString();
        }

        public string Get(StatusRequest request)
        {
            throw new NotImplementedException();
        }

        public string Get(RefreshPartitionRequest request)
        {
            throw new NotImplementedException();
        }

        public bool Get(ForceFullStatusUpdateRequest request)
        {
            try
            {
                if (statusThread == null || !statusThread.IsAlive)
                {
                    manager.GetStatus();
                }

                var cb = new SmartThingsCallbacks(settings);

                foreach (var partition in manager.Partitions)
                {
                    cb.PutPartitionUpdate(partition);
                }

                foreach (var device in manager.Devices)
                {
                    cb.PutDeviceUpdate(device);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return false;
            }

            return true;
        }
    }
}