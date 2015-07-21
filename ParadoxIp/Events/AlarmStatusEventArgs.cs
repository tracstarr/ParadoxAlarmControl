using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ParadoxIp.Enum;

namespace ParadoxIp.Events
{
    public class AlarmStatusEventArgs : EventArgs
    {
        public List<DeviceStatus> DeviceStatus { get; set; }
        public List<object> Alarms { get; set; }
        public List<PartitionStatus> PartitionStatus { get; set; }
        public List<object> Troubles { get; set; }
        public string Stayd { get; set; }
        public string Option { get; set; }
        public ReadOnlyCollection<DeviceStatus> PreviousDeviceStatus { get; set; }
        public ReadOnlyCollection<PartitionStatus> PreviousPartitionStatus { get; set; }

        public AlarmStatusEventArgs(List<DeviceStatus> deviceStatus, List<object> alarms, List<PartitionStatus> partitionStatus,
            List<object> troubles, string stayd, string option,
            ReadOnlyCollection<DeviceStatus> previousDeviceStatus, ReadOnlyCollection<PartitionStatus> previousPartitionStatus)
        {
            DeviceStatus = deviceStatus;
            Alarms = alarms;
            PartitionStatus = partitionStatus;
            Troubles = troubles;
            Stayd = stayd;
            Option = option;
            PreviousDeviceStatus = previousDeviceStatus;
            PreviousPartitionStatus = previousPartitionStatus;
        }
    }
}