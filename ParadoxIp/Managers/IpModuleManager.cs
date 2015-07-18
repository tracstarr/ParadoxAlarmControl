using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using ParadoxIp.Enum;
using ParadoxIp.Events;
using ParadoxIp.Model;

namespace ParadoxIp.Managers
{
    public sealed class IpModuleManager : IDisposable
    {
        private readonly object lockObject = new object();
        private readonly string loginUrl;
        private readonly string versionUrl;
        private readonly string eventUrl;
        private readonly string statusLiveUrl;
        private readonly string logoutUrl;

        private IWebDriver driver;

        private bool statusPageOpen = false;
        private bool stopRunner;
        private bool haveAlarmInfo;
        private ReadOnlyCollection<DeviceStatus> previousDeviceStatus;

        public IpModule Module { get; private set; }
        public List<string> PartitionNames { get; set; }
        public bool IsLoggedIn { get; set; }
        public string SystemName { get; set; }
        private string MainWindowHandle { get; set; }
        public ReadOnlyCollection<string> TroubleCodes { get; set; }
        public List<Device> Devices { get; set; }
        
        public event EventHandler<AlarmStatusEventArgs> AlarmStatusUpdate;
        public event EventHandler<DeviceUpdateEventArgs> DeviceStatusChanged;

        public IpModuleManager(IpModule module)
        {
            Module = module;
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            driver = new PhantomJSDriver(driverService);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            loginUrl = string.Format("{0}/login_page.html", Module.Url.Value.ToString());
            logoutUrl = string.Format("{0}/logout.html", Module.Url.Value.ToString());
            versionUrl = string.Format("{0}/version.html", Module.Url.Value.ToString());
            eventUrl = string.Format("{0}/event.html", Module.Url.Value.ToString());
            statusLiveUrl = string.Format("{0}/statuslive.html", Module.Url.Value.ToString());

            Devices = new List<Device>();
        }
        public bool Login()
        {
            lock (lockObject)
            {
                try
                {
                    if (IsLoggedIn)
                        return true;

                    driver.Navigate().GoToUrl(loginUrl);

                    Module.Session = driver.FindElement(By.Id("ses")).GetAttribute("value");

                    if (string.IsNullOrEmpty(Module.Session))
                        return false;

                    GetMd5Hash();
                    GetRc4AsHexString();

                    driver.FindElement(By.Id("user")).SendKeys(Module.PanelCode.ToString());
                    driver.FindElement(By.Id("pass")).SendKeys(Module.Password);

                    driver.FindElement(By.Name("loginsub")).Click();
                    MainWindowHandle = driver.CurrentWindowHandle;

                    var mainframe = driver.SwitchTo().Frame("mainframe");
                    var wait = new WebDriverWait(mainframe, TimeSpan.FromSeconds(10));
                    wait.Until((d) => d.FindElement(By.Id("fsIndex")));

                    IsLoggedIn = true;
                    SystemName = driver.Title;
                }
                catch (Exception)
                {
                    //try
                    //{
                    //    var error = driver.FindElement(By.Id("ERROR"));

                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}

                    return false;
                }

                return true;
            }
        }
        public void StartStatusUpdates()
        {
            if (!haveAlarmInfo)
                throw new Exception("Must obtain alarm information first");

            while (!stopRunner)
            {
                GetStatus();
                Thread.Sleep(1500);
            }
        }
        public void StopStatusUpdates()
        {
            stopRunner = true;
        }
        public void GetStatus()
        {
            lock (lockObject)
            {
                if (!IsLoggedIn)
                    throw new Exception("Must login first.");

                if (!haveAlarmInfo)
                    throw new Exception("Must obtain alarm information first");

                var js = driver as IJavaScriptExecutor;

                //note: we could just use the stats from in the main window to get updates, but we want to be able to query as necessary for our own needs

                if (!statusPageOpen)
                {
                    statusPageOpen = true;

                    js.ExecuteScript("window.open('" + statusLiveUrl + "','status')");
                }

                driver.SwitchTo().Window("status");
                driver.Navigate().Refresh();
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until((d) => d.FindElement(By.Name("statuslive")));

                var zoneStatus = (new List<object>(js.ExecuteScript("return tbl_statuszone") as IEnumerable<object>)).ToList();
                var alarms = (new List<object>(js.ExecuteScript("return tbl_alarmes") as IEnumerable<object>));
                var userAccess = (new List<object>(js.ExecuteScript("return tbl_useraccess") as IEnumerable<object>));
                var troubles = (new List<object>(js.ExecuteScript("return tbl_troubles") as IEnumerable<object>));
                var stayd = js.ExecuteScript("return stayd").ToString();
                var option = js.ExecuteScript("return option").ToString();

                List<DeviceStatus> ds = zoneStatus.Take(Devices.Count).Select(zoneStatu => (DeviceStatus)Convert.ToInt32(zoneStatu.ToString())).ToList();

                OnAlarmStatusUpdate(new AlarmStatusEventArgs(ds, alarms, userAccess, troubles, stayd, option, previousDeviceStatus));

                previousDeviceStatus = new ReadOnlyCollection<DeviceStatus>(ds);
            }
        }
        public void AlarmAction(PartitionNumber partitionNumber, AlarmMode mode)
        {
            lock (lockObject)
            {
                if (!IsLoggedIn)
                    throw new Exception("Must login first.");

                var js = driver as IJavaScriptExecutor;

                js.ExecuteScript("window.open('','action')");
                driver.SwitchTo().Window("action");

                driver.Navigate().GoToUrl(statusLiveUrl);
                js.ExecuteScript("document.getElementsByName('area')[0].value='" + (int)partitionNumber + "'");
                js.ExecuteScript("document.getElementsByName('value')[0].value='" + AlarmActionAsString(mode) + "'");

                driver.FindElement(By.Name("statuslive")).Submit();
                driver.Close();
                driver.SwitchTo().Window(MainWindowHandle);
            }
        }
        private string AlarmActionAsString(AlarmMode mode)
        {
            switch (mode)
            {
                case AlarmMode.RegularArm:
                    return "r";
                case AlarmMode.ForceArm:
                    return "f";
                case AlarmMode.StayArm:
                    return "s";
                case AlarmMode.InstantArm:
                    return "i";
                case AlarmMode.Disarm:
                    return "d";
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
        public void GetAlarmInformation()
        {
            if (haveAlarmInfo)
                return;
            
            lock (lockObject)
            {
                if (!IsLoggedIn)
                    throw new Exception("Must login first.");

                driver.SwitchTo().Window(MainWindowHandle);
                driver.SwitchTo().Frame("mainframe");
                var js = driver as IJavaScriptExecutor;

                TroubleCodes = new ReadOnlyCollection<string>((new List<object>(js.ExecuteScript("return tbl_troublename") as IEnumerable<object>)).OfType<string>().ToList());

                var areas = (new List<object>(js.ExecuteScript("return tbl_areanam") as IEnumerable<object>)).OfType<string>().ToList();
                var zones = (new List<object>(js.ExecuteScript("return tbl_zone") as IEnumerable<object>));

                ProcessAreaAndZoneInformation(areas, zones);
                haveAlarmInfo = true;

                // setup first time previous status
                var defaultPrevious = new List<DeviceStatus>();
                defaultPrevious.AddRange(Devices.Select(device => device.PreviousStatus));
                previousDeviceStatus = new ReadOnlyCollection<DeviceStatus>(defaultPrevious);
            }
        }
        private void ProcessAreaAndZoneInformation(List<string> areas, List<object> zones)
        {

            int usedAreas = 0;
            int zoneNum = 1;
            
            for (int i = 0; i < zones.Count(); i++)
            {
                var partition = Convert.ToInt32(zones[i]);

                if (partition == 0)
                    break;

                usedAreas = Math.Max(usedAreas, partition);

                var device = new Device()
                {
                    DeviceType = DeviceType.Unknown,
                    Name = zones[++i].ToString(),
                    Partition = (PartitionNumber)(partition - 1),
                    Status = DeviceStatus.Unknown,
                    PreviousStatus = DeviceStatus.Unknown,
                    ZoneId = zoneNum
                };

                zoneNum++;
                device.DeviceType = ParseDeviceType(device.Name);
                Devices.Add(device);
            }

            PartitionNames = new List<string>();

            for (int i = 0; i < usedAreas; i++)
            {
                PartitionNames.Add(areas[i]);
            }
        }
        private DeviceType ParseDeviceType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return DeviceType.Unknown;

            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(name, "door", CompareOptions.IgnoreCase) >= 0)
                return DeviceType.OpenCloseSensor;

            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(name, "glass", CompareOptions.IgnoreCase) >= 0)
                return DeviceType.GlassBreak;

            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(name, "motion", CompareOptions.IgnoreCase) >= 0)
                return DeviceType.MotionSensor;

            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(name, "smoke", CompareOptions.IgnoreCase) >= 0)
                return DeviceType.SmokeDetector;

            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(name, "heat", CompareOptions.IgnoreCase) >= 0)
                return DeviceType.HeatDetector;
            
            return DeviceType.Unknown;
        }
        public void GetVersionInformation()
        {
            if (Module.AlarmPanelVersion != null && Module.IpModuleVersion != null)
                return;

            lock (lockObject)
            {
                if (!IsLoggedIn)
                    throw new Exception("Must login first.");

                var js = driver as IJavaScriptExecutor;

                try
                {
                    driver.SwitchTo().Window("version");
                }
                catch (NoSuchWindowException)
                {
                    js.ExecuteScript("window.open('" + versionUrl + "','version')");
                    driver.SwitchTo().Window("version");
                }

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until((d) => d.FindElement(By.Id("VERSION")));

                var panelVersion = (new List<object>(js.ExecuteScript("return tbl_panel") as IEnumerable<object>));
                //type, firmware, serial
                var ipVersion = (new List<object>(js.ExecuteScript("return tbl_ipmodule") as IEnumerable<object>));
                //firmware, hardware, eco, serial boot, ip boot, serial number, mac

                var moduleVer = new IpModuleVersion()
                {
                    FirmwareVersion = ipVersion[0].ToString(),
                    HardwareVersion = ipVersion[1].ToString(),
                    Eco = ipVersion[2].ToString(),
                    SerialBoot = ipVersion[3].ToString(),
                    IpBoot = ipVersion[4].ToString(),
                    SerialNumber = ipVersion[5].ToString(),
                    MacAddress = ipVersion[6].ToString()
                };

                var panerVer = new AlarmPanelVersion()
                {
                    Type = panelVersion[0].ToString(),
                    FirmwareVersion = panelVersion[1].ToString(),
                    SerialNumber = panelVersion[2].ToString()
                };

                Module.IpModuleVersion = moduleVer;
                Module.AlarmPanelVersion = panerVer;

                driver.Close();
                driver.SwitchTo().Window(MainWindowHandle);
            }
        }
        public void Logout()
        {
            IsLoggedIn = false;
            driver.Navigate().GoToUrl(logoutUrl);
        }
        public string GetMd5Hash()
        {

            Module.PasswordFirstHash = Md5Hash(Module.Password) + Module.Session;
            var hash2 = Md5Hash(Module.PasswordFirstHash);

            Module.PasswordHash = hash2;
            return Module.PasswordHash;
        }
        private string Md5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString().ToUpper();
            }
        }
        public string GetRc4AsHexString()
        {
            string input = Module.PanelCode.ToString();
            string key = Module.PasswordFirstHash;

            StringBuilder result = new StringBuilder();
            int x, y, j = 0;
            int[] box = new int[256];

            for (int i = 0; i < 256; i++)
            {
                box[i] = i;
            }

            int length = key.Length - 1;

            for (int i = length; i >= 0; i--)
            {
                j = ((Convert.ToChar(key[i])) + box[i] + j) % 256;
                x = box[i];
                box[i] = box[j];
                box[j] = x;
            }

            j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                y = i & 255;
                j = (box[y] + j) & 255;
                x = box[y];
                box[y] = box[j];
                box[j] = x;

                result.Append((char)(input[i] ^ box[(box[y] + box[j]) % 256]));
            }

            Module.UserKey = StrToHexStr(result.ToString());

            return Module.UserKey;
        }
        static string StrToHexStr(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                int v = Convert.ToInt32(str[i]);
                sb.Append(string.Format("{0:X2}", v));
            }
            return sb.ToString();
        }
        public void Dispose()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
        private void OnAlarmStatusUpdate(AlarmStatusEventArgs e)
        {
            int i = 0;
            foreach (var deviceStatuse in e.DeviceStatus)
            {
                if (previousDeviceStatus[i] != deviceStatuse)
                {
                    Devices[i].PreviousStatus = previousDeviceStatus[i];
                    Devices[i].Status = deviceStatuse;
                    OnDeviceUpdate(new DeviceUpdateEventArgs(Devices[i]));
                }

                i++;
            }

            var handler = AlarmStatusUpdate;
            if (handler != null) handler(this, e);
        }
        private void OnDeviceUpdate(DeviceUpdateEventArgs e)
        {
            var handler = DeviceStatusChanged;
            if (handler != null) handler(this, e);
        }
    }
}