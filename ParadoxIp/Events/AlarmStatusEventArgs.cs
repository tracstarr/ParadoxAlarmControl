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
        public List<object> UserAccess { get; set; }
        public List<object> Troubles { get; set; }
        public string Stayd { get; set; }
        public string Option { get; set; }
        public ReadOnlyCollection<DeviceStatus> PreviousDeviceStatus { get; set; }

        public AlarmStatusEventArgs(List<DeviceStatus> deviceStatus, List<object> alarms, List<object> userAccess, List<object> troubles, string stayd, string option, ReadOnlyCollection<DeviceStatus> previousDeviceStatus)
        {
            DeviceStatus = deviceStatus;
            Alarms = alarms;
            UserAccess = userAccess;
            Troubles = troubles;
            Stayd = stayd;
            Option = option;
            PreviousDeviceStatus = previousDeviceStatus;
        }
    }
}