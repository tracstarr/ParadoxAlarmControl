using System.Security.Policy;
using NUnit.Framework;
using ParadoxIp.Enum;
using ParadoxIp.Managers;
using ParadoxIp.Model;

namespace ParadoxIpTest
{
    [TestFixture]
    public class HashComputeTests
    {
        const string Session = "52C2BA535E51C1A1";
        const string ExpectedHash = "DB2916A7C33D423D70B0D73DFD198B25"; //TESThash
        const string ExpectedFirstHash = "16BBC0AEE56F8D75C5B674E525A6C67C52C2BA535E51C1A1";
        const string ExpectedRc4 = "B4A672AE"; //8888

        [Test]
        public void Md5HashTest()
        {
            var module = new IpModule(new Url("http://192.168.1.199"), 8888,"TESThash")
            {
                Module = IpModuleType.Ip150,
                Session = Session,
            };

            var manager = new IpModuleManager(module);
            var hash = manager.GetMd5Hash();

            Assert.AreEqual(ExpectedHash, module.PasswordHash);
        }

        [Test]
        public void Rc4Test()
        {
            var module = new IpModule(new Url("http://192.168.1.199"), 8888, "TESThash")
            {
                Module = IpModuleType.Ip150,
                Session = Session,
                PasswordHash = ExpectedHash,
                PasswordFirstHash = ExpectedFirstHash,
            };

            var manager = new IpModuleManager(module);
            var rc4 = manager.GetRc4AsHexString();

            Assert.AreEqual(ExpectedRc4, module.UserKey);
        }
    }
}
