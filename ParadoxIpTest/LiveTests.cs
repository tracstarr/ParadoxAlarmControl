using System;
using System.Configuration;
using System.Security.Policy;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ParadoxIp.Enum;
using ParadoxIp.Managers;
using ParadoxIp.Model;

namespace ParadoxIpTest
{
    //NOTE: These run against your LIVE paradox IP module. 
    [TestFixture]
    public class LiveTests
    {
        private readonly IpModule module = new IpModule(new Url(ConfigurationManager.AppSettings["moduleIp"]), Convert.ToInt32(ConfigurationManager.AppSettings["panelCode"]), ConfigurationManager.AppSettings["password"]);

        [Test]
        public void LoginTest()
        {
            var manager = new IpModuleManager(module);
            Assert.IsTrue(manager.Login());
            manager.Logout();
        }

        [Test]
        public void GetSystemDataTest()
        {
            using (var manager = new IpModuleManager(module))
            {
                manager.Login();
                manager.GetAlarmInformation();
                manager.Logout();

                Assert.IsNotEmpty(manager.TroubleCodes);
                Assert.IsNotEmpty(manager.Devices);
                Assert.IsNotEmpty(manager.PartitionNames);
            }

        }

        [Test]
        public void GetVersionInfoTest()
        {
            using (var manager = new IpModuleManager(module))
            {
                manager.Login();
                manager.GetVersionInformation();
                manager.Logout();
            }

            //NOTE: fill in with your own values.
            Assert.That(module.AlarmPanelVersion.Type, Is.EqualTo(ConfigurationManager.AppSettings["moduleIp"]));
            Assert.That(module.AlarmPanelVersion.SerialNumber, Is.EqualTo("03 00 61 3B"));
            Assert.That(module.AlarmPanelVersion.FirmwareVersion, Is.EqualTo("1.10"));

            Assert.That(module.IpModuleVersion.FirmwareVersion, Is.EqualTo("1.26.01"));
            Assert.That(module.IpModuleVersion.HardwareVersion, Is.EqualTo("010"));
            Assert.That(module.IpModuleVersion.Eco, Is.EqualTo("M009"));
            Assert.That(module.IpModuleVersion.SerialBoot, Is.EqualTo("N/A"));
            Assert.That(module.IpModuleVersion.IpBoot, Is.EqualTo("2.12"));
            Assert.That(module.IpModuleVersion.SerialNumber, Is.EqualTo("71 00 4A C9"));
            Assert.That(module.IpModuleVersion.MacAddress, Is.EqualTo("00:19:BA:02:16:9D"));
        }
        
        [Test]
        public void GetStatusTest()
        {

            using (var manager = new IpModuleManager(module))
            {
                manager.Login();
                manager.GetAlarmInformation();
                manager.GetStatus();
                manager.Logout();
            }

        }

        [Test]
        public void StayArmTest()
        {

            using (var manager = new IpModuleManager(module))
            {
                manager.Login();
                manager.GetAlarmInformation();
                manager.AlarmAction(PartitionNumber.Two, AlarmMode.RegularArm);
                manager.Logout();

            }

        }

        [Test]
        public void StatusEventTest()
        {

            using (var manager = new IpModuleManager(module))
            {
                manager.Login();
                manager.GetAlarmInformation();
                manager.AlarmStatusUpdate += (sender, args) => Console.Write("raised");
                manager.MonitorEvents();
                manager.GetStatus();
                Thread.Sleep(1500);
                manager.GetStatus();

                manager.ShouldRaise("AlarmStatusUpdate");

                manager.Logout();

            }

        }

    }
}